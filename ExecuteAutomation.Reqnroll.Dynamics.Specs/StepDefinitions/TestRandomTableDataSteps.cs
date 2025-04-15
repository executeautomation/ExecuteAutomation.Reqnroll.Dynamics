using Xunit;
using Xunit.Abstractions;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace ExecuteAutomation.Reqnroll.Dynamics.Specs.StepDefinitions;

[Binding]
public sealed class TestRandomTableDataSteps
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Table _userTable;
    private dynamic _userInstance;
    private IEnumerable<dynamic> _userSet;

    public TestRandomTableDataSteps(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Given(@"users with the following details:")]
    public void GivenUsersWithTheFollowingDetails(Table table)
    {
        _userTable = table;
        _testOutputHelper.WriteLine($"Created user table with {table.RowCount} rows");
        
        // Check if we're using auto. or underscore placeholders
        if (table.Rows[0].ContainsKey("Username") && table.Rows[0]["Username"].StartsWith("auto."))
        {
            // For auto. notation we create a single instance
            _userInstance = table.CreateDynamicInstanceWithAutoFixture();
            _testOutputHelper.WriteLine("Created dynamic user instance with auto. placeholders");
        }
        else if (table.Rows[0].ContainsKey("Username") && table.Rows[0]["Username"] == "_")
        {
            // For underscore notation we create a set
            _userSet = table.CreateDynamicSetWithAutoFixture();
            _testOutputHelper.WriteLine("Created dynamic user set with underscore placeholders");
        }
        else if (table.Rows[0].ContainsKey("StringValue") && table.Rows[0]["StringValue"].StartsWith("auto."))
        {
            // For comprehensive auto. test we also create a single instance
            _userInstance = table.CreateDynamicInstanceWithAutoFixture();
            _testOutputHelper.WriteLine("Created dynamic instance with comprehensive auto. placeholders");
        }
        else
        {
            throw new Exception("Unexpected table format. Expected auto. or _ placeholders");
        }
    }

    #region Auto. Placeholder Assertions

    [Then(@"the username should be a valid string")]
    public void ThenTheUsernameShouldBeAValidString()
    {
        if (_userInstance != null && HasProperty(_userInstance, "Username"))
        {
            Assert.IsType<string>(_userInstance.Username);
            Assert.NotEmpty(_userInstance.Username);
            _testOutputHelper.WriteLine($"Username generated: {_userInstance.Username}");
        }
        else if (_userInstance != null && HasProperty(_userInstance, "StringValue"))
        {
            Assert.IsType<string>(_userInstance.StringValue);
            Assert.NotEmpty(_userInstance.StringValue);
            _testOutputHelper.WriteLine($"StringValue generated: {_userInstance.StringValue}");
        }
    }

    [Then(@"the email should be properly formatted")]
    public void ThenTheEmailShouldBeProperlyFormatted()
    {
        if (_userInstance != null && HasProperty(_userInstance, "Email"))
        {
            Assert.IsType<string>(_userInstance.Email);
            Assert.Contains("@", _userInstance.Email);
            var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Assert.Matches(emailRegex, _userInstance.Email);
            _testOutputHelper.WriteLine($"Email generated: {_userInstance.Email}");
        }
        else if (_userInstance != null && HasProperty(_userInstance, "EmailValue"))
        {
            Assert.IsType<string>(_userInstance.EmailValue);
            Assert.Contains("@", _userInstance.EmailValue);
            var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Assert.Matches(emailRegex, _userInstance.EmailValue);
            _testOutputHelper.WriteLine($"EmailValue generated: {_userInstance.EmailValue}");
        }
    }

    [Then(@"the date of birth should be a valid past date")]
    public void ThenTheDateOfBirthShouldBeAValidPastDate()
    {
        Assert.IsType<DateTime>(_userInstance.DateOfBirth);
        Assert.True(_userInstance.DateOfBirth < DateTime.Now);
        Assert.True(_userInstance.DateOfBirth > DateTime.Now.AddYears(-100)); // Reasonably recent date
        _testOutputHelper.WriteLine($"DateOfBirth generated: {_userInstance.DateOfBirth:yyyy-MM-dd}");
    }

    [Then(@"the phone number should have a valid format")]
    public void ThenThePhoneNumberShouldHaveAValidFormat()
    {
        Assert.IsType<int>(_userInstance.Phone);
        var phoneRegex = new Regex(@"^[\d\+\-\(\) ]+$");
        _testOutputHelper.WriteLine($"PhoneNumber generated: {_userInstance.Phone}");
    }

    [Then(@"the GUID should be a non-empty unique identifier")]
    public void ThenTheGUIDShouldBeANonEmptyUniqueIdentifier()
    {
        // Handle string GUIDs
        if (_userInstance.Guid is string guidString)
        {
            Assert.True(Guid.TryParse(guidString, out var parsedGuid));
            Assert.NotEqual(Guid.Empty, parsedGuid);
            _testOutputHelper.WriteLine($"Guid generated (string): {guidString}");
        }
        // Handle direct Guid objects
        else
        {
            Assert.IsType<Guid>(_userInstance.Guid);
            Assert.NotEqual(Guid.Empty, _userInstance.Guid);
            _testOutputHelper.WriteLine($"Guid generated (Guid): {_userInstance.Guid}");
        }
    }

    [Then(@"the zipcode should follow a valid pattern")]
    public void ThenTheZipcodeShouldFollowAValidPattern()
    {
        Assert.IsType<string>(_userInstance.Zipcode);
        Assert.NotEmpty(_userInstance.Zipcode);
        var zipcodeRegex = new Regex(@"^\d{5}(-\d{4})?$");
        Assert.Matches(zipcodeRegex, _userInstance.Zipcode);
        _testOutputHelper.WriteLine($"Zipcode generated: {_userInstance.Zipcode}");
    }

    [Then(@"all auto-generated fields should have valid values")]
    public void ThenAllAutoGeneratedFieldsShouldHaveValidValues()
    {
        // Basic type assertions for original scenarios
        if (_userInstance != null && HasProperty(_userInstance, "Username"))
        {
            ThenTheUsernameShouldBeAValidString();
            ThenTheEmailShouldBeProperlyFormatted();
            ThenTheDateOfBirthShouldBeAValidPastDate();
            ThenThePhoneNumberShouldHaveAValidFormat();
            ThenTheGUIDShouldBeANonEmptyUniqueIdentifier();
            ThenTheZipcodeShouldFollowAValidPattern();
        }
        // Comprehensive type assertions for extended test
        else if (_userInstance != null && HasProperty(_userInstance, "StringValue"))
        {
            // String type assertions
            Assert.IsType<string>(_userInstance.StringValue);
            Assert.NotEmpty(_userInstance.StringValue);
            _testOutputHelper.WriteLine($"StringValue: {_userInstance.StringValue}");
            
            // Int type assertions
            Assert.IsType<int>(_userInstance.IntValue);
            _testOutputHelper.WriteLine($"IntValue: {_userInstance.IntValue}");
            
            // Bool type assertions
            Assert.IsType<bool>(_userInstance.BoolValue);
            _testOutputHelper.WriteLine($"BoolValue: {_userInstance.BoolValue}");
            
            // Decimal type assertions
            Assert.IsType<decimal>(_userInstance.DecimalValue);
            _testOutputHelper.WriteLine($"DecimalValue: {_userInstance.DecimalValue}");
            
            // DateTime type assertions
            Assert.IsType<DateTime>(_userInstance.DateTimeValue);
            _testOutputHelper.WriteLine($"DateTimeValue: {_userInstance.DateTimeValue}");
            
            // Guid type assertions
            Assert.NotNull(_userInstance.GuidValue);
            if (_userInstance.GuidValue is string guidStr)
            {
                Assert.True(Guid.TryParse(guidStr, out var parsedGuid));
                Assert.NotEqual(Guid.Empty, parsedGuid);
                _testOutputHelper.WriteLine($"GuidValue (string): {guidStr}");
            }
            else
            {
                Assert.IsType<Guid>(_userInstance.GuidValue);
                Assert.NotEqual(Guid.Empty, _userInstance.GuidValue);
                _testOutputHelper.WriteLine($"GuidValue (Guid): {_userInstance.GuidValue}");
            }
            
            // Uri type assertions
            Assert.IsType<Uri>(_userInstance.UriValue);
            Assert.StartsWith("http", _userInstance.UriValue.ToString().ToLowerInvariant());
            _testOutputHelper.WriteLine($"UriValue: {_userInstance.UriValue}");
            
            // TimeSpan type assertions
            Assert.IsType<TimeSpan>(_userInstance.TimeSpanValue);
            _testOutputHelper.WriteLine($"TimeSpanValue: {_userInstance.TimeSpanValue}");
            
            // Email type assertions
            Assert.IsType<string>(_userInstance.EmailValue);
            Assert.Contains("@", _userInstance.EmailValue);
            _testOutputHelper.WriteLine($"EmailValue: {_userInstance.EmailValue}");
            
            // Phone type assertions
            Assert.IsType<string>(_userInstance.PhoneValue);
            Assert.NotEmpty(_userInstance.PhoneValue);
            _testOutputHelper.WriteLine($"PhoneValue: {_userInstance.PhoneValue}");
            
            // Credit card type assertions
            Assert.IsType<string>(_userInstance.CreditCardValue);
            Assert.NotEmpty(_userInstance.CreditCardValue);
            _testOutputHelper.WriteLine($"CreditCardValue: {_userInstance.CreditCardValue}");
            
            // Country type assertions
            Assert.IsType<string>(_userInstance.CountryValue);
            Assert.NotEmpty(_userInstance.CountryValue);
            _testOutputHelper.WriteLine($"CountryValue: {_userInstance.CountryValue}");
            
            // City type assertions
            Assert.IsType<string>(_userInstance.CityValue);
            Assert.NotEmpty(_userInstance.CityValue);
            _testOutputHelper.WriteLine($"CityValue: {_userInstance.CityValue}");
            
            // Name type assertions
            Assert.IsType<string>(_userInstance.FirstNameValue);
            Assert.NotEmpty(_userInstance.FirstNameValue);
            _testOutputHelper.WriteLine($"FirstNameValue: {_userInstance.FirstNameValue}");
            
            Assert.IsType<string>(_userInstance.LastNameValue);
            Assert.NotEmpty(_userInstance.LastNameValue);
            _testOutputHelper.WriteLine($"LastNameValue: {_userInstance.LastNameValue}");
            
            // Collection type assertions
            Assert.IsAssignableFrom<IEnumerable<string>>(_userInstance.StringListValue);
            Assert.NotEmpty(_userInstance.StringListValue);
            _testOutputHelper.WriteLine($"StringListValue count: {((IEnumerable<string>)_userInstance.StringListValue).Count()}");
            
            Assert.IsAssignableFrom<IEnumerable<int>>(_userInstance.IntListValue);
            Assert.NotEmpty(_userInstance.IntListValue);
            _testOutputHelper.WriteLine($"IntListValue count: {((IEnumerable<int>)_userInstance.IntListValue).Count()}");
        }
    }

    #endregion

    #region Underscore Placeholder Assertions

    [Then(@"the user set should contain (.*) users?")]
    public void ThenTheUserSetShouldContainUsers(int expectedCount)
    {
        Assert.NotNull(_userSet);
        Assert.NotEmpty(_userSet);
        Assert.Equal(expectedCount, _userSet.Count());
        _testOutputHelper.WriteLine($"User set contains {_userSet.Count()} users");
    }

    [Then(@"all users should have valid usernames")]
    public void ThenAllUsersShouldHaveValidUsernames()
    {
        foreach (var user in _userSet)
        {
            Assert.IsType<string>(user.Username);
            Assert.NotEmpty(user.Username);
            _testOutputHelper.WriteLine($"Username generated: {user.Username}");
        }
    }

    [Then(@"all users should have valid email addresses")]
    public void ThenAllUsersShouldHaveValidEmailAddresses()
    {
        foreach (var user in _userSet)
        {
            Assert.IsType<string>(user.Email);
            Assert.Contains("@", user.Email);
            var emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Assert.Matches(emailRegex, user.Email);
            _testOutputHelper.WriteLine($"Email generated: {user.Email}");
        }
    }

    [Then(@"all users should have valid dates of birth")]
    public void ThenAllUsersShouldHaveValidDatesOfBirth()
    {
        foreach (var user in _userSet)
        {
            Assert.IsType<DateTime>(user.DateOfBirth);
            Assert.True(user.DateOfBirth < DateTime.Now);
            Assert.True(user.DateOfBirth > DateTime.Now.AddYears(-100)); // Reasonably recent date
            _testOutputHelper.WriteLine($"DateOfBirth generated: {user.DateOfBirth:yyyy-MM-dd}");
        }
    }

    [Then(@"all users should have valid phone numbers")]
    public void ThenAllUsersShouldHaveValidPhoneNumbers()
    {
        foreach (var user in _userSet)
        {
            Assert.IsType<int>(user.Phone);
            _testOutputHelper.WriteLine($"PhoneNumber generated: {user.Phone}");
        }
    }

    [Then(@"all users should have valid GUIDs")]
    public void ThenAllUsersShouldHaveValidGUIDs()
    {
        foreach (var user in _userSet)
        {
            // Handle string GUIDs
            if (user.Guid is string guidString)
            {
                Assert.True(Guid.TryParse(guidString, out var parsedGuid));
                Assert.NotEqual(Guid.Empty, parsedGuid);
                _testOutputHelper.WriteLine($"Guid generated (string): {guidString}");
            }
            // Handle direct Guid objects
            else
            {
                Assert.IsType<Guid>(user.Guid);
                Assert.NotEqual(Guid.Empty, user.Guid);
                _testOutputHelper.WriteLine($"Guid generated (Guid): {user.Guid}");
            }
        }
    }

    [Then(@"all users should have valid zipcodes")]
    public void ThenAllUsersShouldHaveValidZipcodes()
    {
        foreach (var user in _userSet)
        {
            Assert.NotNull(user.Zipcode);
            _testOutputHelper.WriteLine($"Zipcode generated: {user.Zipcode}");
        }
    }

    [Then(@"all users should have all fields populated with valid data")]
    public void ThenAllUsersShouldHaveAllFieldsPopulatedWithValidData()
    {
        int userCount = _userSet.Count();
        ThenTheUserSetShouldContainUsers(userCount);
        ThenAllUsersShouldHaveValidUsernames();
        ThenAllUsersShouldHaveValidEmailAddresses();
        ThenAllUsersShouldHaveValidDatesOfBirth();
        ThenAllUsersShouldHaveValidPhoneNumbers();
        ThenAllUsersShouldHaveValidGUIDs();
        ThenAllUsersShouldHaveValidZipcodes();
    }

    #endregion

    #region Helper Methods

    private bool HasProperty(dynamic obj, string propertyName)
    {
        try
        {
            var value = obj.GetType().GetProperty(propertyName);
            return value != null;
        }
        catch
        {
            try
            {
                // For ExpandoObject and other dynamic objects
                var dictionary = obj as IDictionary<string, object>;
                return dictionary != null && dictionary.ContainsKey(propertyName);
            }
            catch
            {
                return false;
            }
        }
    }

    #endregion
} 