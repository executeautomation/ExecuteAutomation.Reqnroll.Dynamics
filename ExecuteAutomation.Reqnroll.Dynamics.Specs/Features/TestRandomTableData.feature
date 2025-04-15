Feature: DynamicRandomTestData

Test Random Data from Table

    @mytag
    Scenario: Test Random Table Instance implementation
        Given users with the following details:
          | Username    | Email      | DateOfBirth | PhoneNumber |Guid|Zipcode|
          | auto.string | auto.email | auto.date   | auto.phone  |auto.guid|auto.zipcode|