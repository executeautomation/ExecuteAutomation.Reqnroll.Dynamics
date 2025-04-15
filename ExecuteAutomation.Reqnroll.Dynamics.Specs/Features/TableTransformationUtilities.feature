Feature: TableTransformationUtilities

Testing the new table transformation utilities

    Scenario: Filter rows from a table based on a condition
        Given I have a table with multiple rows
          | Name   | Age | Status   |
          | John   | 30  | Active   |
          | Alice  | 25  | Inactive |
          | Bob    | 40  | Active   |
          | Claire | 35  | Active   |
        When I filter the rows where Status is Active
        Then I should get a filtered table with 3 rows

    Scenario: Project specific columns from a table
        Given I have a table with multiple columns
          | FirstName | LastName | Age | Email              | Phone        |
          | John      | Doe      | 30  | john@example.com   | 123-456-7890 |
          | Alice     | Smith    | 25  | alice@example.com  | 234-567-8901 |
        When I select only the FirstName and Email columns
        Then I should get a projected table with 2 columns
        And the columns should be FirstName and Email

    Scenario: Create nested dynamic objects from a table
        Given I have a table with nested JSON data
          | Entity  | Properties                                         |
          | User    | {"Name": "John", "Age": 30, "IsActive": true}      |
          | Address | {"Street": "Main St", "Number": 123, "City": "NY"} |
        When I create a nested dynamic object
        Then the User name should be John
        And the User age should be 30
        And the Address street should be Main St 