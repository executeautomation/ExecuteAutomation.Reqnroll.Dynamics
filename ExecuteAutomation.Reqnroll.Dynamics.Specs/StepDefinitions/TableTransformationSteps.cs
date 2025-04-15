using Xunit;
using Xunit.Abstractions;

namespace ExecuteAutomation.Reqnroll.Dynamics.Specs.StepDefinitions;

[Binding]
public sealed class TableTransformationSteps
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Table _originalTable;
    private Table _filteredTable;
    private Table _projectedTable;
    private dynamic _nestedObject;

    public TableTransformationSteps(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    #region Filter Rows Tests

    [Given(@"I have a table with multiple rows")]
    public void GivenIHaveATableWithMultipleRows(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.RowCount} rows");
    }

    [When(@"I filter the rows where Status is Active")]
    public void WhenIFilterTheRowsWhereStatusIsActive()
    {
        _filteredTable = _originalTable.FilterRows(row => row["Status"] == "Active");
        _testOutputHelper.WriteLine($"Filtered table has {_filteredTable.RowCount} rows");
    }

    [Then(@"I should get a filtered table with (.*) rows")]
    public void ThenIShouldGetAFilteredTableWithRows(int expectedRowCount)
    {
        Assert.Equal(expectedRowCount, _filteredTable.RowCount);
        
        // Verify all rows have Status = Active
        foreach (var row in _filteredTable.Rows)
        {
            Assert.Equal("Active", row["Status"]);
        }
    }

    #endregion

    #region Select Columns Tests

    [Given(@"I have a table with multiple columns")]
    public void GivenIHaveATableWithMultipleColumns(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.Header.Count} columns");
    }

    [When(@"I select only the FirstName and Email columns")]
    public void WhenISelectOnlyTheFirstNameAndEmailColumns()
    {
        _projectedTable = _originalTable.SelectColumns("FirstName", "Email");
        _testOutputHelper.WriteLine($"Projected table has {_projectedTable.Header.Count} columns");
    }

    [Then(@"I should get a projected table with (.*) columns")]
    public void ThenIShouldGetAProjectedTableWithColumns(int expectedColumnCount)
    {
        Assert.Equal(expectedColumnCount, _projectedTable.Header.Count);
    }

    [Then(@"the columns should be FirstName and Email")]
    public void ThenTheColumnsShouldBeFirstNameAndEmail()
    {
        Assert.Contains("FirstName", _projectedTable.Header);
        Assert.Contains("Email", _projectedTable.Header);
        Assert.DoesNotContain("LastName", _projectedTable.Header);
        Assert.DoesNotContain("Age", _projectedTable.Header);
        Assert.DoesNotContain("Phone", _projectedTable.Header);
    }

    #endregion

    #region Nested Objects Tests

    [Given(@"I have a table with nested JSON data")]
    public void GivenIHaveATableWithNestedJSONData(Table table)
    {
        _originalTable = table;
        _testOutputHelper.WriteLine($"Original table has {table.RowCount} rows with JSON data");
    }

    [When(@"I create a nested dynamic object")]
    public void WhenICreateANestedDynamicObject()
    {
        _nestedObject = _originalTable.CreateNestedDynamicInstance();
        _testOutputHelper.WriteLine("Created nested dynamic object");
    }

    [Then(@"the User name should be (.*)")]
    public void ThenTheUserNameShouldBe(string expectedName)
    {
        Assert.Equal(expectedName, _nestedObject.User.Name);
    }

    [Then(@"the User age should be (.*)")]
    public void ThenTheUserAgeShouldBe(int expectedAge)
    {
        Assert.Equal(expectedAge, _nestedObject.User.Age);
    }

    [Then(@"the Address street should be (.*)")]
    public void ThenTheAddressStreetShouldBe(string expectedStreet)
    {
        Assert.Equal(expectedStreet, _nestedObject.Address.Street);
    }

    #endregion
} 