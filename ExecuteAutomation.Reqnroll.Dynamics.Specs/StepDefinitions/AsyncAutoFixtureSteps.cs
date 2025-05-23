using Xunit;
using Xunit.Abstractions;
using ExecuteAutomation.Reqnroll.Dynamics;
using System.Text.RegularExpressions;

namespace ExecuteAutomation.Reqnroll.Dynamics.Specs.StepDefinitions;

[Binding]
public sealed class AsyncAutoFixtureSteps
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Table _autoFixtureTable;
    private dynamic _asyncAutoFixtureInstance;
    private IEnumerable<dynamic> _asyncAutoFixtureSet;
    private IEnumerable<dynamic> _asyncEntities;

    public AsyncAutoFixtureSteps(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    #region Auto.String Processing

    [Given(@"I have a table for async AutoFixture processing")]
    public void GivenIHaveATableForAsyncAutoFixtureProcessing(Table table)
    {
        _autoFixtureTable = table;
        _testOutputHelper.WriteLine($"Created table with {table.RowCount} rows for async AutoFixture processing");
    }

    [When(@"I create a dynamic instance with AutoFixture asynchronously")]
    public async Task WhenICreateADynamicInstanceWithAutoFixtureAsynchronously()
    {
        _asyncAutoFixtureInstance = await _autoFixtureTable.CreateDynamicInstanceWithAutoFixtureAsync();
        _testOutputHelper.WriteLine("Created dynamic instance with AutoFixture asynchronously");
    }

    [Then(@"the async AutoFixture instance should have all fields populated")]
    public void ThenTheAsyncAutoFixtureInstanceShouldHaveAllFieldsPopulated()
    {
        Assert.NotNull(_asyncAutoFixtureInstance.Username);
        Assert.NotNull(_asyncAutoFixtureInstance.Email);
        Assert.NotNull(_asyncAutoFixtureInstance.DateOfBirth);
        Assert.NotNull(_asyncAutoFixtureInstance.PhoneNumber);
        Assert.NotNull(_asyncAutoFixtureInstance.Guid);
        Assert.NotNull(_asyncAutoFixtureInstance.Zipcode);
        
        _testOutputHelper.WriteLine($"All fields populated: Username={_asyncAutoFixtureInstance.Username}, " +
                                  $"Email={_asyncAutoFixtureInstance.Email}, " +
                                  $"DateOfBirth={_asyncAutoFixtureInstance.DateOfBirth}, " +
                                  $"PhoneNumber={_asyncAutoFixtureInstance.PhoneNumber}, " +
                                  $"Guid={_asyncAutoFixtureInstance.Guid}, " +
                                  $"Zipcode={_asyncAutoFixtureInstance.Zipcode}");
    }

    [Then(@"the Email field should contain @ symbol")]
    public void ThenTheEmailFieldShouldContainSymbol()
    {
        Assert.Contains("@", _asyncAutoFixtureInstance.Email);
        _testOutputHelper.WriteLine($"Email contains @: {_asyncAutoFixtureInstance.Email}");
    }

    [Then(@"the Guid field should be a valid GUID")]
    public void ThenTheGuidFieldShouldBeAValidGUID()
    {
        if (_asyncAutoFixtureInstance.Guid is string guidString)
        {
            Assert.True(Guid.TryParse(guidString, out var guid));
            Assert.NotEqual(Guid.Empty, guid);
            _testOutputHelper.WriteLine($"String Guid is valid: {guidString}");
        }
        else
        {
            Assert.IsType<Guid>(_asyncAutoFixtureInstance.Guid);
            Assert.NotEqual(Guid.Empty, _asyncAutoFixtureInstance.Guid);
            _testOutputHelper.WriteLine($"Guid is valid: {_asyncAutoFixtureInstance.Guid}");
        }
    }

    #endregion

    #region Underscore Processing

    [Given(@"I have a table with multiple rows for async AutoFixture processing")]
    public void GivenIHaveATableWithMultipleRowsForAsyncAutoFixtureProcessing(Table table)
    {
        _autoFixtureTable = table;
        _testOutputHelper.WriteLine($"Created table with {table.RowCount} rows for async AutoFixture set processing");
    }

    [When(@"I create a dynamic set with AutoFixture asynchronously")]
    public async Task WhenICreateADynamicSetWithAutoFixtureAsynchronously()
    {
        _asyncAutoFixtureSet = await _autoFixtureTable.CreateDynamicSetWithAutoFixtureAsync();
        _testOutputHelper.WriteLine("Created dynamic set with AutoFixture asynchronously");
    }

    [Then(@"the async AutoFixture set should have (.*) items")]
    public void ThenTheAsyncAutoFixtureSetShouldHaveItems(int expectedCount)
    {
        Assert.Equal(expectedCount, _asyncAutoFixtureSet.Count());
        _testOutputHelper.WriteLine($"Set has {_asyncAutoFixtureSet.Count()} items");
    }

    [Then(@"all items in the set should have auto-generated fields")]
    public void ThenAllItemsInTheSetShouldHaveAutoGeneratedFields()
    {
        foreach (var item in _asyncAutoFixtureSet)
        {
            Assert.NotNull(item.Username);
            Assert.NotNull(item.Email);
            Assert.NotNull(item.DateOfBirth);
            Assert.NotNull(item.PhoneNumber);
            Assert.NotNull(item.Guid);
            Assert.NotNull(item.Zipcode);
            
            _testOutputHelper.WriteLine($"Item has: Username={item.Username}, " +
                                      $"Email={item.Email}, " +
                                      $"DateOfBirth={item.DateOfBirth}, " +
                                      $"PhoneNumber={item.PhoneNumber}, " +
                                      $"Guid={item.Guid}, " +
                                      $"Zipcode={item.Zipcode}");
        }
    }

    #endregion

    #region Entity Generation

    private class TestUser
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public bool IsActive { get; set; }
    }

    [Given(@"I have registered a User entity type for async testing")]
    public void GivenIHaveRegisteredAUserEntityTypeForAsyncTesting()
    {
        AutoFixtureTableExtensions.RegisterEntityType<TestUser>("User");
        _testOutputHelper.WriteLine("Registered User entity type for async testing");
    }

    [When(@"I create (.*) User entities asynchronously")]
    public async Task WhenICreateUserEntitiesAsynchronously(int count)
    {
        _asyncEntities = await AutoFixtureTableExtensions.CreateEntitiesAsync("User", count);
        _testOutputHelper.WriteLine($"Created {count} User entities asynchronously");
    }

    [Then(@"I should have (.*) User entities with auto-generated properties")]
    public void ThenIShouldHaveUserEntitiesWithAutoGeneratedProperties(int expectedCount)
    {
        Assert.Equal(expectedCount, _asyncEntities.Count());
        
        foreach (dynamic user in _asyncEntities)
        {
            Assert.NotNull(user.Name);
            Assert.NotNull(user.Email);
            Assert.True(user.Age > 0);
            Assert.IsType<bool>(user.IsActive);
            
            _testOutputHelper.WriteLine($"User: Name={user.Name}, Email={user.Email}, Age={user.Age}, IsActive={user.IsActive}");
        }
    }

    #endregion
} 