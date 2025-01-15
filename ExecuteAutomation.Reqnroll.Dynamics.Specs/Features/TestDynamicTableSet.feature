Feature: DynamicTestDynamicSetSteps

Test Dynamic Table implementation

    @mytag
    Scenario: Test Dynamic Table Set implementation
        Given the numbers are
          | Number | Value | Output |
          | First  | 50    | 50     |
          | Second | 70    | 70     |
          | Third  | 100   | 100    |
        And the second number is 70
        When the two numbers are added
        Then the result should be 120