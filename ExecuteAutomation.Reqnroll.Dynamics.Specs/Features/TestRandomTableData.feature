Feature: DynamicRandomTestData

Test Random Data from Table

    @mytag
    Scenario: Test Random Table Instance implementation with auto.
        Given users with the following details:
          | Username    | Email      | DateOfBirth | Phone      | Guid      | Zipcode      |
          | auto.string | auto.email | auto.date   | auto.phone | auto.guid | auto.zipcode |
        Then the username should be a valid string
        And the email should be properly formatted
        And the date of birth should be a valid past date
        And the phone number should have a valid format
        And the GUID should be a non-empty unique identifier
        
    @mytag
    Scenario: Test Random Table Instance implementation with _
        Given users with the following details:
          | Username | Email | DateOfBirth | Phone | Guid | Zipcode |
          | _        | _     | _           | _     | _    | _       |
        Then the user set should contain 1 user
        And all users should have valid usernames
        And all users should have valid email addresses
        And all users should have valid dates of birth
        And all users should have valid phone numbers
        And all users should have valid GUIDs
        And all users should have valid zipcodes
        And all users should have all fields populated with valid data

    @mytag
    Scenario: Test Random Table Instance implementation with multiple rows using _
        Given users with the following details:
          | Username | Email | DateOfBirth | Phone | Guid | Zipcode |
          | _        | _     | _           | _           | _    | _       |
          | _        | _     | _           | _           | _    | _       |
          | _        | _     | _           | _           | _    | _       |
        Then the user set should contain 3 users
        And all users should have valid usernames
        And all users should have valid email addresses
        And all users should have valid dates of birth
        And all users should have valid phone numbers
        And all users should have valid GUIDs
        And all users should have valid zipcodes
        And all users should have all fields populated with valid data

    @mytag
    Scenario: Test Random Table Instance with comprehensive data types
        Given users with the following details:
          | StringValue    | IntValue    | BoolValue   | DecimalValue    | DateTimeValue | GuidValue   | UriValue     | TimeSpanValue    | EmailValue    | PhoneValue    | CreditCardValue    | CountryValue    | CityValue    | FirstNameValue  | LastNameValue   | StringListValue | IntListValue |
          | auto.string    | auto.int    | auto.bool   | auto.decimal    | auto.datetime | auto.guid   | auto.uri     | auto.timespan    | auto.email    | auto.phone    | auto.creditcard    | auto.country    | auto.city    | auto.firstname  | auto.lastname   | auto.stringlist | auto.intlist |
        Then all auto-generated fields should have valid values