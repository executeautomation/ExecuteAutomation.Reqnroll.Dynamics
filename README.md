# ExecuteAutomation.Reqnroll.Dynamics

A powerful library that enhances [Reqnroll](https://reqnroll.net/) (formerly SpecFlow) with dynamic data handling capabilities, simplifying how data is created and manipulated in Gherkin feature files.

## Features

### Core Dynamic Table Capabilities

- **Dynamic Instance Creation**: Convert tables to dynamic objects
  ```csharp
  dynamic data = table.CreateDynamicInstance();
  ```

- **Dynamic Set Creation**: Convert tables to collections of dynamic objects
  ```csharp
  var items = table.CreateDynamicSet();
  ```

- **Dynamic Data Comparison**: Compare tables with dynamic objects or collections
  ```csharp
  table.CompareToDynamicInstance(instance);
  table.CompareToDynamicSet(set);
  ```

- **Step Argument Transformations**: Built-in transformations for automated conversion
  ```csharp
  // In step definitions, tables are automatically transformed
  public void GivenTheFollowingUsers(IEnumerable<object> users) { ... }
  public void GivenAUser(dynamic user) { ... }
  ```

### AutoFixture Integration

- **Random Data Generation**: Create tables with auto-generated test data
  ```gherkin
  Given users with the following details:
    | Username    | Email      | DateOfBirth | PhoneNumber |
    | auto.string | auto.email | auto.date   | auto.phone  |
    | _           | _          | _           | _           |
  ```

- **Type Inference**: Automatic type detection from column names
  ```gherkin
  # The underscore (_) will infer appropriate data types based on column names
  | Username | Email | DateOfBirth | PhoneNumber |
  | _        | _     | _           | _           |
  ```

- **Supported Data Types**:
  - `auto.string` - Random string values
  - `auto.int` - Random integer values
  - `auto.decimal`, `auto.double` - Random numeric values
  - `auto.date`, `auto.datetime` - Random date/time values
  - `auto.bool` - Random boolean values
  - `auto.guid` - Random GUIDs
  - `auto.email` - Random email addresses
  - `auto.phone` - Random phone numbers
  - `auto.url` - Random URLs
  - `auto.name`, `auto.firstname`, `auto.lastname` - Random name values
  - `auto.address` - Random street addresses
  - `auto.city` - Random city names
  - `auto.zipcode` - Random ZIP/postal codes

- **Custom Entity Generation**: Register your own domain entity generators
  ```csharp
  // In test setup
  AutoFixtureTableExtensions.RegisterEntityType<User>("user");
  AutoFixtureTableExtensions.RegisterEntityType<Product>("product");
  
  // Custom generator with specific logic
  AutoFixtureTableExtensions.RegisterEntityGenerator<PremiumUser>("premium-user", 
    () => new PremiumUser { Level = "Gold", ... });
  ```

- **Usage in Step Definitions**:
  ```csharp
  [Given(@"users with the following details:")]
  public void GivenUsersWithTheFollowingDetails(Table table)
  {
      var users = table.CreateDynamicSetWithAutoFixture();
      // Process users...
  }
  ```

## Getting Started

### Installation

```bash
dotnet add package ExecuteAutomation.Reqnroll.Dynamics
```

### Basic Usage

1. Import the namespace in your step definition files:
```csharp
using ExecuteAutomation.Reqnroll.Dynamics;
```

2. Use the extension methods on Reqnroll `Table` objects:
```csharp
[Given(@"the following products:")]
public void GivenTheFollowingProducts(Table table)
{
    var products = table.CreateDynamicSet();
    // Use products in your test...
}
```

### Using Random Data Generation

1. Create feature files with auto-generated data:
```gherkin
Feature: User Registration

Scenario: Register new users
  Given users with the following details:
    | Username    | Email      | DateOfBirth | PhoneNumber |
    | auto.string | auto.email | auto.date   | auto.phone  |
    | _           | _          | _           | _           |
  When I register these users
  Then all registrations should be successful
```

2. Register custom entity generators in your test setup:
```csharp
[BeforeTestRun]
public static void SetupAutoFixture()
{
    AutoFixtureTableExtensions.RegisterEntityType<User>("user");
    AutoFixtureTableExtensions.RegisterEntityType<Product>("product");
}
```

## Advanced Scenarios

### Vertical Tables

The library supports vertical tables (2 columns with key-value pairs):

```gherkin
Given a user with details:
  | Property   | Value       |
  | Username   | johndoe     |
  | Email      | auto.email  |
  | DateOfBirth| 1990-01-01  |
```

### Type Conversion

Automatic type conversion is performed for common data types:
- Integers
- Decimals/Doubles
- Booleans
- DateTimes

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
