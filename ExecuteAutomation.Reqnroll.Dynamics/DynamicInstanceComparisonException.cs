using Reqnroll;

namespace ExecuteAutomation.Reqnroll.Dynamics;
using AutoFixture;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Text.Json;

public class DynamicInstanceComparisonException : Exception
{
    public DynamicInstanceComparisonException(IList<string> diffs)
        : base("There were some difference between the table and the instance")
    {
        Differences = diffs ?? new List<string>();
    }

    public IList<string> Differences { get; private set; }
}


public static class AutoFixtureTableExtensions
{
    private static readonly Fixture _fixture = new();
    private static readonly Regex _autoFixturePattern = new(@"auto\.(\w+)(?:\(([^)]*)\))?", RegexOptions.Compiled);
    private static readonly Dictionary<string, Func<object>> _entityGenerators = new(StringComparer.OrdinalIgnoreCase);
    
    static AutoFixtureTableExtensions()
    {
        // Configure default AutoFixture customizations
        _fixture.Customize<DateTime>(composer => 
            composer.FromFactory(() => DateTime.Now.AddDays(-Random.Shared.Next(1000))));
        
        _fixture.Customize<string>(composer => 
            composer.FromFactory(() => 
                string.Concat(Enumerable.Repeat(0, Random.Shared.Next(5, 10))
                    .Select(_ => (char)Random.Shared.Next(97, 123)))));
    }
    
    // Allow consumers to register their own entity generators
    public static void RegisterEntityGenerator<T>(string entityName, Func<T> generator) where T : class
    {
        if (string.IsNullOrEmpty(entityName))
        {
            throw new ArgumentNullException(nameof(entityName), "Entity name cannot be null or empty");
        }
        
        if (generator == null)
        {
            throw new ArgumentNullException(nameof(generator), "Generator cannot be null");
        }
        
        _entityGenerators[entityName] = () => generator();
    }
    
    // Register an entity type to be created with AutoFixture
    public static void RegisterEntityType<T>(string entityName) where T : class
    {
        if (string.IsNullOrEmpty(entityName))
        {
            throw new ArgumentNullException(nameof(entityName), "Entity name cannot be null or empty");
        }
        
        _entityGenerators[entityName] = () => _fixture.Create<T>();
    }
    
    // Get a registered entity generator
    public static Func<object> GetEntityGenerator(string entityName)
    {
        if (string.IsNullOrEmpty(entityName))
        {
            throw new ArgumentNullException(nameof(entityName), "Entity name cannot be null or empty");
        }
        
        if (_entityGenerators.TryGetValue(entityName, out var generator))
        {
            return generator;
        }
        
        throw new KeyNotFoundException($"No entity generator registered for '{entityName}'");
    }
    
    // Async versions of entity operations
    
    public static async Task<IEnumerable<dynamic>> CreateEntitiesAsync(string entityTypeName, int count)
    {
        return await Task.Run(() => CreateEntities(entityTypeName, count));
    }
    
    public static IEnumerable<dynamic> CreateEntities(string entityTypeName, int count)
    {
        var generator = GetEntityGenerator(entityTypeName);
        return Enumerable.Range(0, count).Select(_ => generator());
    }
    
    // Add this extension method to the existing DynamicTableHelpers
    public static Table WithAutoFixtureData(this Table table)
    {
        var result = new Table(table.Header.ToArray());
        
        foreach (var row in table.Rows)
        {
            var rowData = new Dictionary<string, string>();
            
            foreach (var header in table.Header)
            {
                var cellValue = row[header];
                if (IsAutoFixtureMarker(cellValue))
                {
                    rowData[header] = GenerateWithAutoFixture(cellValue, header);
                }
                else
                {
                    rowData[header] = cellValue;
                }
            }
            
            result.AddRow(rowData);
        }
        
        return result;
    }
    
    // Override existing methods to use AutoFixture
    public static ExpandoObject CreateDynamicInstanceWithAutoFixture(this Table table)
    {
        return table.WithAutoFixtureData().CreateDynamicInstance();
    }
    
    public static IEnumerable<dynamic> CreateDynamicSetWithAutoFixture(this Table table)
    {
        return table.WithAutoFixtureData().CreateDynamicSet();
    }
    
    // Async implementations for AutoFixture integration
    
    public static async Task<Table> WithAutoFixtureDataAsync(this Table table)
    {
        return await Task.Run(() => WithAutoFixtureData(table));
    }
    
    public static async Task<ExpandoObject> CreateDynamicInstanceWithAutoFixtureAsync(this Table table)
    {
        var transformedTable = await WithAutoFixtureDataAsync(table);
        return await transformedTable.CreateDynamicInstanceAsync();
    }
    
    public static async Task<IEnumerable<dynamic>> CreateDynamicSetWithAutoFixtureAsync(this Table table)
    {
        var transformedTable = await WithAutoFixtureDataAsync(table);
        return await transformedTable.CreateDynamicSetAsync();
    }
    
    private static bool IsAutoFixtureMarker(string value)
    {
        return !string.IsNullOrEmpty(value) && 
               (value.StartsWith("auto.") || value == "_");
    }
    
    private static string GenerateWithAutoFixture(string marker, string header)
    {
        if (string.IsNullOrEmpty(marker) || string.IsNullOrEmpty(header))
        {
            return string.Empty;
        }
        
        if (marker == "_")
        {
            // Infer type from header name
            return InferTypeFromHeader(header);
        }
        
        var match = _autoFixturePattern.Match(marker);
        if (!match.Success) return marker;
        
        string type = match.Groups[1].Value;
        string parameters = match.Groups.Count > 2 ? match.Groups[2].Value : string.Empty;
        
        return GenerateValueByType(type, parameters);
    }
    
    private static string InferTypeFromHeader(string header)
    {
        var lowerHeader = header.ToLowerInvariant();
        
        // Collection types
        if (IsCollectionHeader(header))
        {
            string itemType = InferCollectionItemType(header);
            return GenerateCollectionValue(itemType);
        }
        
        // Email type
        if (lowerHeader.Contains("email"))
            return _fixture.Create<EmailAddress>().Address;
        
        // Name types - handle first/last name differently from general names
        if (lowerHeader.Contains("firstname"))
            return _fixture.Create<PersonName>().First;
            
        if (lowerHeader.Contains("lastname"))
            return _fixture.Create<PersonName>().Last;
            
        if (lowerHeader.Contains("name"))
            return _fixture.Create<PersonName>().ToString();
        
        // Date and time types
        if (lowerHeader.Contains("datetime"))
            return _fixture.Create<DateTime>().ToString("yyyy-MM-dd HH:mm:ss");
            
        if (lowerHeader.Contains("date") || lowerHeader.Contains("time"))
            return _fixture.Create<DateTime>().ToString("yyyy-MM-dd");
        
        // Number types
        if (lowerHeader.Contains("decimal"))
            return _fixture.Create<decimal>().ToString();
            
        if (lowerHeader.Contains("double"))
            return _fixture.Create<double>().ToString();
            
        if (lowerHeader.Contains("int") || lowerHeader.EndsWith("count") || lowerHeader.EndsWith("number"))
            return _fixture.Create<int>().ToString();
        
        // Boolean type
        if (lowerHeader.Contains("bool") || lowerHeader.StartsWith("is") || lowerHeader.StartsWith("has"))
            return _fixture.Create<bool>().ToString();
        
        // Phone type
        if (lowerHeader.Contains("phone"))
            return _fixture.Create<PhoneNumber>().Number;
        
        // Guid type
        if (lowerHeader.Contains("guid"))
            return _fixture.Create<Guid>().ToString();
        
        // URL/URI type
        if (lowerHeader.Contains("url") || lowerHeader.Contains("uri"))
            return _fixture.Create<Uri>().ToString();
        
        // Address types
        if (lowerHeader.Contains("address"))
            return _fixture.Create<StreetAddress>().ToString();
            
        if (lowerHeader.Contains("city"))
            return _fixture.Create<City>().Name;
            
        if (lowerHeader.Contains("zipcode") || lowerHeader.Contains("postalcode"))
            return _fixture.Create<ZipCode>().ToString();
        
        // Credit card type
        if (lowerHeader.Contains("credit") || lowerHeader.Contains("card") || lowerHeader.Contains("payment"))
            return $"{_fixture.Create<int>() % 9000 + 1000}-{_fixture.Create<int>() % 9000 + 1000}-{_fixture.Create<int>() % 9000 + 1000}-{_fixture.Create<int>() % 9000 + 1000}";
        
        // ID type
        if (lowerHeader.Contains("id") || lowerHeader.EndsWith("id"))
            return _fixture.Create<int>().ToString();
        
        // Default to string for any other type
        return _fixture.Create<string>();
    }
    
    private static string GenerateValueByType(string type, string parameters)
    {
        // Check if this is a registered entity type
        if (_entityGenerators.TryGetValue(type, out var generator))
        {
            var entity = generator();
            return entity.ToString();
        }
        
        // Parse parameters for collection types
        int collectionCount = 3; // Default
        if (type.EndsWith("list") && !string.IsNullOrEmpty(parameters))
        {
            if (int.TryParse(parameters, out var count))
                collectionCount = count;
        }
        
        switch (type.ToLowerInvariant())
        {
            // String types
            case "string":
                return _fixture.Create<string>();
            
            // Number types
            case "int":
                return _fixture.Create<int>().ToString();
            
            case "decimal":
                return _fixture.Create<decimal>().ToString();
            
            case "double":
                return _fixture.Create<double>().ToString();
            
            // Date and time types
            case "date":
                return _fixture.Create<DateTime>().ToString("yyyy-MM-dd");
            
            case "datetime":
                return _fixture.Create<DateTime>().ToString("yyyy-MM-dd HH:mm:ss");
            
            case "timespan":
                return _fixture.Create<TimeSpan>().ToString();
            
            // Boolean type
            case "bool":
                return _fixture.Create<bool>().ToString();
            
            // Identifier types
            case "guid":
                return _fixture.Create<Guid>().ToString();
            
            // Contact types
            case "email":
                return _fixture.Create<EmailAddress>().Address;
            
            case "phone":
                return _fixture.Create<PhoneNumber>().Number;
            
            // Web types
            case "url":
            case "uri":
                return _fixture.Create<Uri>().ToString();
            
            // Name types
            case "name":
                return _fixture.Create<PersonName>().ToString();
            
            case "firstname":
                return _fixture.Create<PersonName>().First;
            
            case "lastname":
                return _fixture.Create<PersonName>().Last;
            
            // Address types
            case "address":
                return _fixture.Create<StreetAddress>().ToString();
            
            case "city":
                return _fixture.Create<City>().Name;
            
            case "zipcode":
            case "postalcode":
                return _fixture.Create<ZipCode>().ToString();
            
            case "country":
                return "United States";
            
            // Payment types
            case "creditcard":
                return $"{_fixture.Create<int>() % 9000 + 1000}-{_fixture.Create<int>() % 9000 + 1000}-{_fixture.Create<int>() % 9000 + 1000}-{_fixture.Create<int>() % 9000 + 1000}";
            
            // Collection types
            case "stringlist":
                return GenerateCollectionValue("stringlist", collectionCount);
            
            case "intlist":
                return GenerateCollectionValue("intlist", collectionCount);
            
            case "guidlist":
                return GenerateCollectionValue("guidlist", collectionCount);
            
            case "datelist":
                return GenerateCollectionValue("datelist", collectionCount);
            
            // Default
            default:
                return _fixture.Create<string>();
        }
    }

    // Helper method to check if a header indicates a collection type
    private static bool IsCollectionHeader(string header)
    {
        var lowerHeader = header.ToLowerInvariant();
        return lowerHeader.EndsWith("list") || 
               lowerHeader.EndsWith("array") || 
               lowerHeader.EndsWith("collection") ||
               lowerHeader.EndsWith("set") ||
               lowerHeader.Contains("items");
    }
    
    // Helper method to infer the item type from a collection header
    private static string InferCollectionItemType(string header)
    {
        var lowerHeader = header.ToLowerInvariant();
        
        if (lowerHeader.Contains("string"))
            return "stringlist";
            
        if (lowerHeader.Contains("int"))
            return "intlist";
            
        if (lowerHeader.Contains("guid"))
            return "guidlist";
            
        if (lowerHeader.Contains("date"))
            return "datelist";
            
        // Default to string list
        return "stringlist";
    }
    
    // Helper method to generate randomized collection values as JSON strings
    private static string GenerateCollectionValue(string itemType, int count = 3)
    {
        switch (itemType.ToLowerInvariant())
        {
            case "stringlist":
                var strings = Enumerable.Range(0, count)
                    .Select(_ => _fixture.Create<string>())
                    .ToList();
                return JsonSerializer.Serialize(strings);
                
            case "intlist":
                var ints = Enumerable.Range(0, count)
                    .Select(_ => _fixture.Create<int>())
                    .ToList();
                return JsonSerializer.Serialize(ints);
                
            case "guidlist":
                var guids = Enumerable.Range(0, count)
                    .Select(_ => _fixture.Create<Guid>())
                    .ToList();
                return JsonSerializer.Serialize(guids);
                
            case "datelist":
                var dates = Enumerable.Range(0, count)
                    .Select(_ => _fixture.Create<DateTime>())
                    .ToList();
                return JsonSerializer.Serialize(dates);
                
            default:
                var defaultList = Enumerable.Range(0, count)
                    .Select(_ => _fixture.Create<string>())
                    .ToList();
                return JsonSerializer.Serialize(defaultList);
        }
    }
}

// Custom types to enhance AutoFixture
public class EmailAddress
{
    public string Address => $"{_fixture.Create<string>().Replace(" ", "")}@example.com";
    private static readonly Fixture _fixture = new();
}

public class PhoneNumber
{
    private readonly string _part1 = (_fixture.Create<int>() % 100).ToString("D2");
    private readonly string _part2 = (_fixture.Create<int>() % 1000).ToString("D3");
    private readonly string _part3 = (_fixture.Create<int>() % 10000).ToString("D4");

    public string Number => $"{_part1}{_part2}{_part3}";
    private static readonly Fixture _fixture = new();
}

// Additional custom types...
public class PersonName
{
    private static readonly Fixture _fixture = new();
    private static readonly string[] _firstNames = { "John", "Jane", "Robert", "Mary", "David", "Lisa" };
    private static readonly string[] _lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller" };
    
    public string First => _firstNames[_fixture.Create<int>() % _firstNames.Length];
    public string Last => _lastNames[_fixture.Create<int>() % _lastNames.Length];
    
    public override string ToString() => $"{First} {Last}";
}

public class StreetAddress
{
    private static readonly Fixture _fixture = new();
    
    public string Street => $"{_fixture.Create<int>() % 9999 + 1} {_fixture.Create<string>()} St.";
    
    public override string ToString() => Street;
}

public class City
{
    private static readonly Fixture _fixture = new();
    private static readonly string[] _cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia" };
    
    public string Name => _cities[_fixture.Create<int>() % _cities.Length];
}

public class ZipCode
{
    private static readonly Fixture _fixture = new();
    
    public string Code => $"{_fixture.Create<int>() % 90000 + 10000}";
    
    public override string ToString() => Code;
}