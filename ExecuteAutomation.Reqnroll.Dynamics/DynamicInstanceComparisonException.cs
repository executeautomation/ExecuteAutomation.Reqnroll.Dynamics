using Reqnroll;

namespace ExecuteAutomation.Reqnroll.Dynamics;
using AutoFixture;
using System.Dynamic;
using System.Text.RegularExpressions;

public class DynamicInstanceComparisonException : Exception
{
    public DynamicInstanceComparisonException(IList<string> diffs)
        : base("There were some difference between the table and the instance")
    {
        Differences = diffs;
    }

    public IList<string> Differences { get; private set; }
}


public static class AutoFixtureTableExtensions
{
    private static readonly Fixture _fixture = new Fixture();
    private static readonly Regex _autoFixturePattern = new Regex(@"auto\.(\w+)(?:\(([^)]*)\))?", RegexOptions.Compiled);
    private static readonly Dictionary<string, Func<object>> _entityGenerators = new Dictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase);
    
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
        _entityGenerators[entityName] = () => generator();
    }
    
    // Register an entity type to be created with AutoFixture
    public static void RegisterEntityType<T>(string entityName) where T : class
    {
        _entityGenerators[entityName] = () => _fixture.Create<T>();
    }
    
    // Get a registered entity generator
    public static Func<object> GetEntityGenerator(string entityName)
    {
        if (_entityGenerators.TryGetValue(entityName, out var generator))
        {
            return generator;
        }
        
        throw new KeyNotFoundException($"No entity generator registered for '{entityName}'");
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
    
    private static bool IsAutoFixtureMarker(string value)
    {
        return !string.IsNullOrEmpty(value) && 
               (value.StartsWith("auto.") || value == "_");
    }
    
    private static string GenerateWithAutoFixture(string marker, string header)
    {
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
        
        if (lowerHeader.Contains("email"))
            return _fixture.Create<EmailAddress>().Address;
        
        if (lowerHeader.Contains("name"))
            return _fixture.Create<string>();
        
        if (lowerHeader.Contains("date") || lowerHeader.Contains("time"))
            return _fixture.Create<DateTime>().ToString("yyyy-MM-dd");
        
        if (lowerHeader.Contains("phone"))
            return _fixture.Create<PhoneNumber>().Number;
        
        if (lowerHeader.Contains("id") || lowerHeader.EndsWith("id"))
            return _fixture.Create<int>().ToString();
        
        // Default to string
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
        
        switch (type.ToLowerInvariant())
        {
            case "string":
                return _fixture.Create<string>();
            
            case "int":
                return _fixture.Create<int>().ToString();
            
            case "decimal":
                return _fixture.Create<decimal>().ToString();
            
            case "double":
                return _fixture.Create<double>().ToString();
            
            case "date":
                return _fixture.Create<DateTime>().ToString("yyyy-MM-dd");
            
            case "datetime":
                return _fixture.Create<DateTime>().ToString("yyyy-MM-dd HH:mm:ss");
            
            case "bool":
                return _fixture.Create<bool>().ToString();
            
            case "guid":
                return _fixture.Create<Guid>().ToString();
            
            case "email":
                return _fixture.Create<EmailAddress>().Address;
            
            case "phone":
                return _fixture.Create<PhoneNumber>().Number;
            
            case "url":
                return _fixture.Create<Uri>().ToString();
            
            case "name":
                return _fixture.Create<PersonName>().ToString();
            
            case "firstname":
                return _fixture.Create<PersonName>().First;
            
            case "lastname":
                return _fixture.Create<PersonName>().Last;
            
            case "address":
                return _fixture.Create<StreetAddress>().ToString();
            
            case "city":
                return _fixture.Create<City>().Name;
            
            case "zipcode":
                return _fixture.Create<ZipCode>().ToString();
            
            default:
                return _fixture.Create<string>();
        }
    }
}

// Custom types to enhance AutoFixture
public class EmailAddress
{
    public string Address => $"{_fixture.Create<string>().Replace(" ", "")}@example.com";
    private static readonly Fixture _fixture = new Fixture();
}

public class PhoneNumber
{
    public string Number => $"+{_fixture.Create<int>() % 100:D2} {_fixture.Create<int>() % 1000:D3} {_fixture.Create<int>() % 10000:D4}";
    private static readonly Fixture _fixture = new Fixture();
}

// Additional custom types...
public class PersonName
{
    private static readonly Fixture _fixture = new Fixture();
    private static readonly string[] _firstNames = new[] { "John", "Jane", "Robert", "Mary", "David", "Lisa" };
    private static readonly string[] _lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller" };
    
    public string First => _firstNames[_fixture.Create<int>() % _firstNames.Length];
    public string Last => _lastNames[_fixture.Create<int>() % _lastNames.Length];
    
    public override string ToString() => $"{First} {Last}";
}

public class StreetAddress
{
    private static readonly Fixture _fixture = new Fixture();
    
    public string Street => $"{_fixture.Create<int>() % 9999 + 1} {_fixture.Create<string>()} St.";
    
    public override string ToString() => Street;
}

public class City
{
    private static readonly Fixture _fixture = new Fixture();
    private static readonly string[] _cities = new[] { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia" };
    
    public string Name => _cities[_fixture.Create<int>() % _cities.Length];
}

public class ZipCode
{
    private static readonly Fixture _fixture = new Fixture();
    
    public string Code => $"{_fixture.Create<int>() % 90000 + 10000}";
    
    public override string ToString() => Code;
}