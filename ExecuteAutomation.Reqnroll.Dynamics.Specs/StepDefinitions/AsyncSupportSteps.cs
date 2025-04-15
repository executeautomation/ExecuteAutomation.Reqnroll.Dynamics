using Xunit;
using Xunit.Abstractions;

namespace ExecuteAutomation.Reqnroll.Dynamics.Specs.StepDefinitions;

[Binding]
public sealed class AsyncSupportSteps
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Table _originalTable;
    private dynamic _asyncDynamicInstance;
    private IEnumerable<dynamic> _asyncDynamicSet;
    private Table _asyncFilteredTable;
    private Table _asyncProjectedTable;
    private dynamic _asyncNestedObject;

    public AsyncSupportSteps(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    #region Async Dynamic Instance Tests

    [Given(@"I have a table for async processing")]
    public void GivenIHaveATableForAsyncProcessing(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.RowCount} rows for async processing");
    }

    [When(@"I create a dynamic instance asynchronously")]
    public async Task WhenICreateADynamicInstanceAsynchronously()
    {
        _asyncDynamicInstance = await _originalTable.CreateDynamicInstanceAsync();
        _testOutputHelper.WriteLine("Created dynamic instance asynchronously");
    }

    [Then(@"the async dynamic instance should have property (.*) with value (.*)")]
    public void ThenTheAsyncDynamicInstanceShouldHavePropertyWithValue(string propertyName, string expectedValue)
    {
        var actualValue = (string)Dynamitey.Dynamic.InvokeGet(_asyncDynamicInstance, propertyName);
        Assert.Equal(expectedValue, actualValue);
        _testOutputHelper.WriteLine($"Property {propertyName} has value {actualValue}");
    }

    #endregion

    #region Async Dynamic Set Tests

    [Given(@"I have a table with multiple rows for async processing")]
    public void GivenIHaveATableWithMultipleRowsForAsyncProcessing(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.RowCount} rows for async set processing");
    }

    [When(@"I create a dynamic set asynchronously")]
    public async Task WhenICreateADynamicSetAsynchronously()
    {
        _asyncDynamicSet = await _originalTable.CreateDynamicSetAsync();
        _testOutputHelper.WriteLine("Created dynamic set asynchronously");
    }

    [Then(@"the async dynamic set should have (.*) items")]
    public void ThenTheAsyncDynamicSetShouldHaveItems(int expectedCount)
    {
        Assert.Equal(expectedCount, _asyncDynamicSet.Count());
        _testOutputHelper.WriteLine($"Dynamic set has {_asyncDynamicSet.Count()} items");
    }

    [Then(@"the first item in the set should have Name (.*)")]
    public void ThenTheFirstItemInTheSetShouldHaveName(string expectedName)
    {
        var firstItem = _asyncDynamicSet.First();
        Assert.Equal(expectedName, firstItem.Name);
        _testOutputHelper.WriteLine($"First item has Name {firstItem.Name}");
    }

    #endregion

    #region Async Filter Tests

    [Given(@"I have a table with various statuses")]
    public void GivenIHaveATableWithVariousStatuses(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.RowCount} rows with various statuses");
    }

    [When(@"I filter the rows asynchronously where Status is Active")]
    public async Task WhenIFilterTheRowsAsynchronouslyWhereStatusIsActive()
    {
        _asyncFilteredTable = await _originalTable.FilterRowsAsync(row => row["Status"] == "Active");
        _testOutputHelper.WriteLine($"Filtered table asynchronously and got {_asyncFilteredTable.RowCount} rows");
    }

    [Then(@"the async filtered table should have (.*) rows")]
    public void ThenTheAsyncFilteredTableShouldHaveRows(int expectedCount)
    {
        Assert.Equal(expectedCount, _asyncFilteredTable.RowCount);
        _testOutputHelper.WriteLine($"Async filtered table has {_asyncFilteredTable.RowCount} rows");
    }

    [Then(@"all rows in the filtered table should have Status (.*)")]
    public void ThenAllRowsInTheFilteredTableShouldHaveStatus(string expectedStatus)
    {
        foreach (var row in _asyncFilteredTable.Rows)
        {
            Assert.Equal(expectedStatus, row["Status"]);
        }
        _testOutputHelper.WriteLine($"All rows have Status {expectedStatus}");
    }

    #endregion

    #region Async Projection Tests

    [Given(@"I have a table with multiple columns for async projection")]
    public void GivenIHaveATableWithMultipleColumnsForAsyncProjection(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.Header.Count} columns for async projection");
    }

    [When(@"I select only the FirstName and Email columns asynchronously")]
    public async Task WhenISelectOnlyTheFirstNameAndEmailColumnsAsynchronously()
    {
        _asyncProjectedTable = await _originalTable.SelectColumnsAsync("FirstName", "Email");
        _testOutputHelper.WriteLine($"Projected table asynchronously and got {_asyncProjectedTable.Header.Count} columns");
    }

    [Then(@"the async projected table should have (.*) columns")]
    public void ThenTheAsyncProjectedTableShouldHaveColumns(int expectedColumnCount)
    {
        Assert.Equal(expectedColumnCount, _asyncProjectedTable.Header.Count);
        _testOutputHelper.WriteLine($"Async projected table has {_asyncProjectedTable.Header.Count} columns");
    }

    [Then(@"the projected columns should be FirstName and Email")]
    public void ThenTheProjectedColumnsShouldBeFirstNameAndEmail()
    {
        Assert.Contains("FirstName", _asyncProjectedTable.Header);
        Assert.Contains("Email", _asyncProjectedTable.Header);
        Assert.DoesNotContain("LastName", _asyncProjectedTable.Header);
        Assert.DoesNotContain("Age", _asyncProjectedTable.Header);
        Assert.DoesNotContain("Phone", _asyncProjectedTable.Header);
        _testOutputHelper.WriteLine("Projected columns are FirstName and Email");
    }

    #endregion

    #region Async Nested Object Tests

    [Given(@"I have a table with nested JSON data for async processing")]
    public void GivenIHaveATableWithNestedJSONDataForAsyncProcessing(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.RowCount} rows with nested JSON data for async processing");
    }

    [When(@"I create a nested dynamic object asynchronously")]
    public async Task WhenICreateANestedDynamicObjectAsynchronously()
    {
        _asyncNestedObject = await _originalTable.CreateNestedDynamicInstanceAsync();
        _testOutputHelper.WriteLine("Created nested dynamic object asynchronously");
    }

    [Then(@"the async User name should be (.*)")]
    public void ThenTheAsyncUserNameShouldBe(string expectedName)
    {
        Assert.Equal(expectedName, _asyncNestedObject.User.Name);
        _testOutputHelper.WriteLine($"Async User name is {_asyncNestedObject.User.Name}");
    }

    [Then(@"the async Address city should be (.*)")]
    public void ThenTheAsyncAddressCityShouldBe(string expectedCity)
    {
        Assert.Equal(expectedCity, _asyncNestedObject.Address.City);
        _testOutputHelper.WriteLine($"Async Address city is {_asyncNestedObject.Address.City}");
    }

    #endregion
} 