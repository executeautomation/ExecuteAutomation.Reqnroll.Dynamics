Feature: AsyncSupport

Testing asynchronous operations with dynamic tables

    Scenario: Create dynamic instance asynchronously
        Given I have a table for async processing
          | Number | Value | Output |
          | First  | 50    | 50     |
        When I create a dynamic instance asynchronously
        Then the async dynamic instance should have property Number with value First

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