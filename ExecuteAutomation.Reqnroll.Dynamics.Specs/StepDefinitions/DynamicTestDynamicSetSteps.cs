using Xunit.Abstractions;

namespace ExecuteAutomation.Reqnroll.Dynamics.Specs.StepDefinitions;

[Binding]
public sealed class DynamicTestDynamicSetSteps
{
    private readonly ITestOutputHelper _testOutputHelper;
    // For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef

    public DynamicTestDynamicSetSteps(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Given("the numbers are")]
    public void GivenTheFirstNumberIs(Table table)
    {
        var datas = table.CreateDynamicSet();

        foreach (var data in datas)
        {
            _testOutputHelper.WriteLine(data.Number.ToString());
            _testOutputHelper.WriteLine(data.Value.ToString());
            _testOutputHelper.WriteLine(data.Output.ToString());
        }
    }

    [Given(@"the second number is (.*)")]
    public void GivenTheSecondNumberIs(int number)
    {
        _testOutputHelper.WriteLine(number.ToString());
    }

    [When("the two numbers are added")]
    public void WhenTheTwoNumbersAreAdded()
    {
        _testOutputHelper.WriteLine("The two numbers are added");
    }

    [Then(@"the result should be (.*)")]
    public void ThenTheResultShouldBe(int result)
    {
        _testOutputHelper.WriteLine($"The result should be: {result.ToString()}");
    }

    [Given(@"the numbers are in dynamic instance table like")]
    public void GivenTheNumbersAreInDynamicInstanceTableLike(Table table)
    {
        dynamic data = table.CreateDynamicInstance();
        _testOutputHelper.WriteLine(data.Number.ToString());
        _testOutputHelper.WriteLine(data.Value.ToString());
        _testOutputHelper.WriteLine(data.Output.ToString());
    }

    // [Given(@"users with the following details:")]
    // public void GivenUsersWithTheFollowingDetails(Table table)
    // {
    //     var users = table.CreateDynamicSetWithAutoFixture();
    //     
    //     foreach (var user in users)
    //     {
    //         _testOutputHelper.WriteLine($"Username: {user.Username}");
    //         _testOutputHelper.WriteLine($"Email: {user.Email}");
    //         _testOutputHelper.WriteLine($"DateOfBirth: {user.DateOfBirth}");
    //         _testOutputHelper.WriteLine($"PhoneNumber: {user.PhoneNumber}");
    //         _testOutputHelper.WriteLine($"Guid: {user.Guid}");
    //         _testOutputHelper.WriteLine($"Zipcode: {user.Zipcode}");
    //         _testOutputHelper.WriteLine("---");
    //     }
    // }
}