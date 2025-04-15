Feature: DynamicTestDynamicSetInstance

Test Dynamic Table implementation

    @mytag
    Scenario: Test Dynamic Table Instance implementation
        Given the numbers are in dynamic instance table like
          | Number | Value | Output |
          | First  | 50    | 50     |
        And the second number is 70
        When the two numbers are added
        Then the result should be 120