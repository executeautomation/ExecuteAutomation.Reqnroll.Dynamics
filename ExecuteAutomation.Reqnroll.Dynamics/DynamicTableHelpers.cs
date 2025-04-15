using System.Dynamic;
using System.Text.RegularExpressions;
using Dynamitey;
using Reqnroll;

namespace ExecuteAutomation.Reqnroll.Dynamics;

public static class DynamicTableHelpers
{
    private const string ERRORMESS_PROPERTY_DIFF_SET = "Properties differs between the table and the set";

    private const string ERRORMESS_INSTANCETABLE_FORMAT =
        "Can only create instances of tables with one row, or exactly 2 columns and several rows";

    private const string ERRORMESS_NOT_ON_TABLE = "The '{0}' value not present in the table, but on the instance";
    private const string ERRORMESS_NOT_ON_INSTANCE = "The '{0}' value not present on the instance, but in the table";

    private const string ERRORMESS_VALUE_DIFFERS =
        "The '{0}' value differs from table and instance.\n\tInstance:\t'{1}'(type: {2}).\n\tTable:\t\t'{3}'(type: {4})";

    private const string ERRORMESS_NUMBER_OF_ROWS_DIFFERS =
        "Number of rows for table ({0} rows) and set ({1} rows) differs";

    private const string ERRORMESS_SET_VALUES_DIFFERS =
        "A difference was found on row '{0}' for column '{1}' (property '{2}').\n\tInstance:\t'{3}'(type: {4}).\n\tTable:\t\t'{5}'(type: {6})";

    /// <summary>
    ///     Create a dynamic object from the headers and values of the <paramref name="table" />
    /// </summary>
    /// <param name="table">the table to create a dynamic object from</param>
    /// <param name="doTypeConversion">
    ///     should types be converted according to conventions described in
    ///     https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic/wiki/Conversion-conventions#property-type-conversions
    /// </param>
    /// <returns>the created object</returns>
    public static ExpandoObject CreateDynamicInstance(this Table table, bool doTypeConversion = true)
    {
        if (table.Header.Count == 2 && table.RowCount > 1)
        {
            var horizontalTable = CreateHorizontalTable(table);
            return CreateDynamicInstance(horizontalTable.Rows[0], doTypeConversion);
        }

        if (table.RowCount == 1) return CreateDynamicInstance(table.Rows[0], doTypeConversion);

        throw new DynamicInstanceFromTableException(ERRORMESS_INSTANCETABLE_FORMAT);
    }


    /// <summary>
    ///     Creates a set of dynamic objects based of the <paramref name="table" /> headers and values
    /// </summary>
    /// <param name="table">the table to create a set of dynamics from</param>
    /// <param name="doTypeConversion">
    ///     should types be converted according to conventions described in
    ///     https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic/wiki/Conversion-conventions#property-type-conversions
    /// </param>
    /// <returns>a set of dynamics</returns>
    public static IEnumerable<dynamic> CreateDynamicSet(this Table table, bool doTypeConversion = true)
    {
        return from r in table.Rows
            select CreateDynamicInstance(r, doTypeConversion);
    }

    /// <summary>
    ///     Validates if a dynamic instance <paramref name="instance" /> matches the <paramref name="table" />
    ///     Throws descriptive exception if not
    /// </summary>
    /// <param name="table">the table to compare the instance against</param>
    /// <param name="instance">the instance to compare the table against</param>
    /// <param name="doTypeConversion">
    ///     should types be converted according to conventions described in
    ///     https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic/wiki/Conversion-conventions#property-type-conversions
    /// </param>
    public static void CompareToDynamicInstance(this Table table, dynamic instance, bool doTypeConversion = true)
    {
        IList<string> propDiffs = GetPropertyDifferences(table, instance);
        if (propDiffs.Any())
            throw new DynamicInstanceComparisonException(propDiffs);

        AssertValuesOfRowDifference(table.Rows[0], instance, doTypeConversion);
    }

    /// <summary>
    ///     Validates that the dynamic set <paramref name="set" /> matches the <paramref name="table" />
    ///     Throws descriptive exception if not
    /// </summary>
    /// <param name="table">the table to compare the set against</param>
    /// <param name="set">the set to compare the table against</param>
    /// <param name="doTypeConversion">
    ///     should types be converted according to conventions described in
    ///     https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic/wiki/Conversion-conventions#property-type-conversions
    /// </param>
    public static void CompareToDynamicSet(this Table table, IList<dynamic> set, bool doTypeConversion = true)
    {
        AssertEqualNumberOfRows(table, set);

        IList<string> propDiffs = GetPropertyDifferences(table, set[0]);
        if (propDiffs.Any()) throw new DynamicSetComparisonException(ERRORMESS_PROPERTY_DIFF_SET, propDiffs);

        // Now we know that the table and the list has the same number of rows and properties

        var valueDifference = GetSetValueDifferences(table, set, doTypeConversion);

        if (valueDifference.Any())
            throw new DynamicSetComparisonException(ERRORMESS_PROPERTY_DIFF_SET, valueDifference);
    }

    private static List<string> GetSetValueDifferences(Table table, IList<object> set, bool doTypeConversion = true)
    {
        var memberNames = Dynamic.GetMemberNames(set[0]);
        var valueDifference = new List<string>();

        for (var i = 0; i < set.Count; i++)
            foreach (var memberName in memberNames)
            {
                var currentHeader = string.Empty;
                var rowValue = GetRowValue(i, table, memberName, out currentHeader, doTypeConversion);
                var rowType = rowValue.GetType().Name;
                var instanceValue = Dynamic.InvokeGet(set[i], memberName);
                var instanceType = instanceValue.GetType().Name;

                if (!instanceValue.Equals(rowValue))
                {
                    var difference = string.Format(ERRORMESS_SET_VALUES_DIFFERS,
                        i + 1,
                        currentHeader,
                        memberName,
                        instanceValue,
                        instanceType,
                        rowValue,
                        rowType);

                    valueDifference.Add(difference);
                }
            }

        return valueDifference;
    }

    private static object GetRowValue(int rowIndex, Table table, string memberName, out string currentHeader,
        bool doTypeConversion = true)
    {
        object rowValue = null;
        currentHeader = string.Empty;
        foreach (var header in table.Header)
            if (CreatePropertyName(header) == memberName)
            {
                currentHeader = header;
                rowValue = CreateTypedValue(table.Rows[rowIndex][header], doTypeConversion);
                break;
            }

        return rowValue;
    }

    private static void AssertValuesOfRowDifference(DataTableRow tableRow, dynamic instance,
        bool doTypeConversion = true)
    {
        IList<string> valueDiffs = ValidateValuesOfRow(tableRow, instance, doTypeConversion);
        if (valueDiffs.Any())
            throw new DynamicInstanceComparisonException(valueDiffs);
    }

    private static IList<string> GetPropertyDifferences(Table table, dynamic instance, bool doTypeConversion = true)
    {
        var tableHeadersAsPropertyNames = table.Header.Select(CreatePropertyName);
        IEnumerable<string> instanceMembers = Dynamic.GetMemberNames(instance);

        return GetPropertyNameDifferences(tableHeadersAsPropertyNames, instanceMembers);
    }

    private static void AssertEqualNumberOfRows(Table table, IList<object> set)
    {
        if (table.RowCount != set.Count)
        {
            var mess = string.Format(ERRORMESS_NUMBER_OF_ROWS_DIFFERS, table.RowCount, set.Count);
            throw new DynamicSetComparisonException(mess);
        }
    }

    private static IList<string> ValidateValuesOfRow(DataTableRow tableRow, dynamic instance,
        bool doTypeConversion = true)
    {
        var valueDiffs = new List<string>();

        foreach (var header in tableRow.Keys)
        {
            var propertyName = CreatePropertyName(header);
            var valueFromInstance = Dynamic.InvokeGet(instance, propertyName);
            var typeFromInstance = valueFromInstance.GetType().Name;
            var valueFromTable = CreateTypedValue(tableRow[header], doTypeConversion);
            var typeFromTable = valueFromTable.GetType().Name;

            if (!valueFromInstance.Equals(valueFromTable))
            {
                var mess = string.Format(ERRORMESS_VALUE_DIFFERS, propertyName, valueFromInstance, typeFromInstance,
                    valueFromTable, typeFromTable);
                valueDiffs.Add(mess);
            }
        }

        return valueDiffs;
    }

    private static IList<string> GetPropertyNameDifferences(IEnumerable<string> tableHeadersAsPropertyNames,
        IEnumerable<string> instanceMembers)
    {
        var allMembersInTableButNotInInstance = tableHeadersAsPropertyNames.Except(instanceMembers);
        var allMembersInInstanceButNotInTable = instanceMembers.Except(tableHeadersAsPropertyNames);

        var diffs = new List<string>();

        diffs.AddRange(
            allMembersInInstanceButNotInTable.Select(
                m => string.Format(ERRORMESS_NOT_ON_TABLE, m)));

        diffs.AddRange(
            allMembersInTableButNotInInstance.Select(
                m => string.Format(ERRORMESS_NOT_ON_INSTANCE, m)));

        return diffs;
    }

    private static Table CreateHorizontalTable(Table verticalFieldValueTable)
    {
        var dic = verticalFieldValueTable.Rows.ToDictionary(row => row[0], row => row[1]);

        var horizontalTable = new Table(dic.Keys.ToArray());
        horizontalTable.AddRow(dic);
        return horizontalTable;
    }

    private static ExpandoObject CreateDynamicInstance(DataTableRow tablerow, bool doTypeConversion = true)
    {
        dynamic expando = new ExpandoObject();
        var dicExpando = expando as IDictionary<string, object>;

        foreach (var header in tablerow.Keys)
        {
            var propName = CreatePropertyName(header);
            var propValue = CreateTypedValue(tablerow[header], doTypeConversion);
            dicExpando.Add(propName, propValue);
        }

        return expando;
    }

    private static object CreateTypedValue(string valueFromTable, bool doTypeConversion = true)
    {
        if (!doTypeConversion)
            return valueFromTable;

        if (int.TryParse(valueFromTable, out var i))
            return i;

        if (double.TryParse(valueFromTable, out var db))
        {
            if (decimal.TryParse(valueFromTable, out var d) && d.Equals((decimal)db)) return db;
            return d;
        }

        if (bool.TryParse(valueFromTable, out var b))
            return b;

        if (DateTime.TryParse(valueFromTable, out var dt))
            return dt;

        return valueFromTable;
    }

    private static string CreatePropertyName(string header)
    {
        var cleanedHeader = RemoveReservedChars(header);
        var propName = FixCasing(cleanedHeader);

        // Throw if no chars in string
        if (propName.Length != 0) return propName;

        var mess = $"Property '{header}' only contains reserved C# characters";
        throw new DynamicInstanceFromTableException(mess);
    }

    private static string FixCasing(string header)
    {
        var arr = header.Split(' ');
        var propName = arr[0]; // leave the first element as is, since it might be correct cased...

        for (var i = 1; i < arr.Length; i++)
        {
            var s = arr[i];
            if (s.Length > 0)
                propName += s[0].ToString().ToUpperInvariant() +
                            s.Substring(1).ToLowerInvariant();
        }

        return propName;
    }

    private static string RemoveReservedChars(string orgPropertyName)
    {
        const string pattern = @"[^\w\s]";
        const string replacement = "";
        return Regex.Replace(orgPropertyName, pattern, replacement);
    }

    #region Table Transformation Utilities

    /// <summary>
    /// Creates a new table with rows that match the specified predicate
    /// </summary>
    /// <param name="table">The table to filter</param>
    /// <param name="predicate">The function to test each row for a condition</param>
    /// <returns>A new filtered table</returns>
    public static Table FilterRows(this Table table, Func<DataTableRow, bool> predicate)
    {
        var filteredTable = new Table(table.Header.ToArray());
        
        foreach (var row in table.Rows.Where(predicate))
        {
            var rowValues = new List<string>();
            foreach (var header in table.Header)
            {
                rowValues.Add(row[header]);
            }
            filteredTable.AddRow(rowValues.ToArray());
        }
        
        return filteredTable;
    }

    /// <summary>
    /// Creates a new table with only the specified columns
    /// </summary>
    /// <param name="table">The table to project</param>
    /// <param name="columnNames">The column names to include in the new table</param>
    /// <returns>A new projected table</returns>
    public static Table SelectColumns(this Table table, params string[] columnNames)
    {
        // Verify all column names exist
        foreach (var column in columnNames)
        {
            if (!table.Header.Contains(column))
                throw new ArgumentException($"Column '{column}' not found in the table");
        }
        
        var projectedTable = new Table(columnNames);
        
        foreach (var row in table.Rows)
        {
            var rowValues = new List<string>();
            foreach (var column in columnNames)
            {
                rowValues.Add(row[column]);
            }
            projectedTable.AddRow(rowValues.ToArray());
        }
        
        return projectedTable;
    }

    /// <summary>
    /// Parses nested JSON objects from table cells to create hierarchical dynamic objects
    /// </summary>
    /// <param name="table">The table containing JSON objects</param>
    /// <param name="doTypeConversion">Whether to apply type conversion</param>
    /// <returns>A dynamic object with nested properties</returns>
    public static ExpandoObject CreateNestedDynamicInstance(this Table table, bool doTypeConversion = true)
    {
        dynamic expando = new ExpandoObject();
        var dicExpando = expando as IDictionary<string, object>;
        
        foreach (var row in table.Rows)
        {
            var entityName = row[0];
            var propertiesJson = row[1];
            
            // Parse the JSON properties string to dictionary
            try
            {
                var nestedProperties = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(propertiesJson);
                
                // Convert the dictionary to ExpandoObject
                dynamic nestedExpando = new ExpandoObject();
                var dicNestedExpando = nestedExpando as IDictionary<string, object>;
                
                foreach (var prop in nestedProperties)
                {
                    var value = prop.Value?.ToString();
                    dicNestedExpando.Add(CreatePropertyName(prop.Key), value != null && doTypeConversion ? CreateTypedValue(value, true) : prop.Value);
                }
                
                dicExpando.Add(CreatePropertyName(entityName), nestedExpando);
            }
            catch (System.Text.Json.JsonException)
            {
                // If the cell is not a valid JSON, just add it as a regular string
                dicExpando.Add(CreatePropertyName(entityName), doTypeConversion ? CreateTypedValue(propertiesJson, true) : propertiesJson);
            }
        }
        
        return expando;
    }

    #endregion

    #region Async Support

    /// <summary>
    /// Asynchronously creates a dynamic object from the headers and values of the table
    /// </summary>
    /// <param name="table">The table to create a dynamic object from</param>
    /// <param name="doTypeConversion">Whether to perform type conversion</param>
    /// <returns>Task returning the created object</returns>
    public static async Task<ExpandoObject> CreateDynamicInstanceAsync(this Table table, bool doTypeConversion = true)
    {
        return await Task.Run(() => CreateDynamicInstance(table, doTypeConversion));
    }

    /// <summary>
    /// Asynchronously creates a set of dynamic objects based on the table headers and values
    /// </summary>
    /// <param name="table">The table to create a set of dynamics from</param>
    /// <param name="doTypeConversion">Whether to perform type conversion</param>
    /// <returns>Task returning a set of dynamics</returns>
    public static async Task<IEnumerable<dynamic>> CreateDynamicSetAsync(this Table table, bool doTypeConversion = true)
    {
        return await Task.Run(() => CreateDynamicSet(table, doTypeConversion));
    }

    /// <summary>
    /// Asynchronously validates if a dynamic instance matches the table
    /// </summary>
    /// <param name="table">The table to compare the instance against</param>
    /// <param name="instance">The instance to compare the table against</param>
    /// <param name="doTypeConversion">Whether to perform type conversion</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public static async Task CompareToDynamicInstanceAsync(this Table table, dynamic instance, bool doTypeConversion = true)
    {
        await Task.Run(() => CompareToDynamicInstance(table, instance, doTypeConversion));
    }

    /// <summary>
    /// Asynchronously validates that the dynamic set matches the table
    /// </summary>
    /// <param name="table">The table to compare the set against</param>
    /// <param name="set">The set to compare the table against</param>
    /// <param name="doTypeConversion">Whether to perform type conversion</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public static async Task CompareToDynamicSetAsync(this Table table, IList<dynamic> set, bool doTypeConversion = true)
    {
        await Task.Run(() => CompareToDynamicSet(table, set, doTypeConversion));
    }

    /// <summary>
    /// Asynchronously creates a filtered table based on a predicate
    /// </summary>
    /// <param name="table">The table to filter</param>
    /// <param name="predicate">The function to test each row for a condition</param>
    /// <returns>Task returning the filtered table</returns>
    public static async Task<Table> FilterRowsAsync(this Table table, Func<DataTableRow, bool> predicate)
    {
        return await Task.Run(() => FilterRows(table, predicate));
    }

    /// <summary>
    /// Asynchronously creates a projected table with only specified columns
    /// </summary>
    /// <param name="table">The table to project</param>
    /// <param name="columnNames">The column names to include</param>
    /// <returns>Task returning the projected table</returns>
    public static async Task<Table> SelectColumnsAsync(this Table table, params string[] columnNames)
    {
        return await Task.Run(() => SelectColumns(table, columnNames));
    }

    /// <summary>
    /// Asynchronously creates a nested dynamic object from the table
    /// </summary>
    /// <param name="table">The table containing nested data</param>
    /// <param name="doTypeConversion">Whether to perform type conversion</param>
    /// <returns>Task returning the nested dynamic object</returns>
    public static async Task<ExpandoObject> CreateNestedDynamicInstanceAsync(this Table table, bool doTypeConversion = true)
    {
        return await Task.Run(() => CreateNestedDynamicInstance(table, doTypeConversion));
    }

    #endregion
}