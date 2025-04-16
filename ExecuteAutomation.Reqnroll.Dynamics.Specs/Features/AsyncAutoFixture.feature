Feature: AsyncAutoFixture

Testing asynchronous operations with AutoFixture integration

    Scenario: Create dynamic instance with AutoFixture asynchronously
        Given I have a table for async AutoFixture processing
          | Username    | Email      | DateOfBirth | PhoneNumber | Guid      | Zipcode      |
          | auto.string | auto.email | auto.date   | auto.phone  | auto.guid | auto.zipcode |
        When I create a dynamic instance with AutoFixture asynchronously
        Then the async AutoFixture instance should have all fields populated
        And the Email field should contain @ symbol
        And the Guid field should be a valid GUID

    Scenario: Create dynamic set with AutoFixture asynchronously
        Given I have a table with multiple rows for async AutoFixture processing
          | Username | Email | DateOfBirth | PhoneNumber | Guid | Zipcode |
          | _        | _     | _           | _           | _    | _       |
          | _        | _     | _           | _           | _    | _       |
        When I create a dynamic set with AutoFixture asynchronously
        Then the async AutoFixture set should have 2 items
        And all items in the set should have auto-generated fields

    Scenario: Create multiple entities asynchronously
        Given I have registered a User entity type for async testing
        When I create 3 User entities asynchronously
        Then I should have 3 User entities with auto-generated properties 