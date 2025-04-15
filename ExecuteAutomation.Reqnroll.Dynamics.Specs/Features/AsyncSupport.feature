Feature: AsyncSupport

Testing asynchronous operations with dynamic tables

    Scenario: Create dynamic instance asynchronously
        Given I have a table for async processing
          | Name  | Age | Email             |
          | John  | 30  | john@example.com  |
        When I create a dynamic instance asynchronously
        Then the async dynamic instance should have property Name with value John
        And the async dynamic instance should have property Email with value john@example.com

    Scenario: Create dynamic set asynchronously
        Given I have a table with multiple rows for async processing
          | Name   | Age | Status   |
          | John   | 30  | Active   |
          | Alice  | 25  | Inactive |
          | Bob    | 40  | Active   |
        When I create a dynamic set asynchronously
        Then the async dynamic set should have 3 items
        And the first item in the set should have Name John

    Scenario: Filter table rows asynchronously
        Given I have a table with various statuses
          | Name   | Age | Status   |
          | John   | 30  | Active   |
          | Alice  | 25  | Inactive |
          | Bob    | 40  | Active   |
          | Claire | 35  | Active   |
        When I filter the rows asynchronously where Status is Active
        Then the async filtered table should have 3 rows
        And all rows in the filtered table should have Status Active
        
    Scenario: Project table columns asynchronously
        Given I have a table with multiple columns for async projection
          | FirstName | LastName | Age | Email              | Phone        |
          | John      | Doe      | 30  | john@example.com   | 123-456-7890 |
          | Alice     | Smith    | 25  | alice@example.com  | 234-567-8901 |
        When I select only the FirstName and Email columns asynchronously
        Then the async projected table should have 2 columns
        And the projected columns should be FirstName and Email
        
    Scenario: Create nested dynamic objects asynchronously
        Given I have a table with nested JSON data for async processing
          | Entity  | Properties                                         |
          | User    | {"Name": "John", "Age": 30, "Email": "john@example.com"} |
          | Address | {"Street": "Main St", "City": "New York", "ZipCode": "10001"} |
        When I create a nested dynamic object asynchronously
        Then the async User name should be John
        And the async Address city should be New York 