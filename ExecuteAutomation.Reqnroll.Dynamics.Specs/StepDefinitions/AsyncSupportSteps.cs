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
} 