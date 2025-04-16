# ExecuteAutomation.Reqnroll.Dynamics

A powerful library that enhances [Reqnroll](https://reqnroll.net/) with dynamic data handling capabilities for BDD testing.

## Features

### Core Table Handling

- **Dynamic Object Creation**: Convert tables to dynamic objects
  ```csharp
  var data = table.CreateDynamicInstance();
  ```

- **Dynamic Set Creation**: Convert tables to collections of dynamic objects
  ```csharp
  var items = table.CreateDynamicSet();
  ```

- **Comparison**: Compare tables with dynamic objects or collections
  ```csharp
  table.CompareToDynamicInstance(instance);
  table.CompareToDynamicSet(set);
  ```

- **Table Transformations**: Transform vertical tables to horizontal and filter/project table data
  ```csharp
  var horizontal = CreateHorizontalTable(verticalTable);
  var filtered = table.FilterRows(row => row["Status"] == "Active");
  var projected = table.SelectColumns("FirstName", "LastName");
  ```

- **Nested Objects**: Create hierarchical objects from tables with JSON data
  ```csharp
  var nestedObject = table.CreateNestedDynamicInstance();
  ```

### AutoFixture Integration

- **Random Data Generation**: Create tables with auto-generated test data
  ```gherkin
  Given users with the following details:
    | Username    | Email      | DateOfBirth | PhoneNumber |
    | auto.string | auto.email | auto.date   | auto.phone  |
    | _           | _          | _           | _           |
  ```

- **Type Inference**: Automatic type detection from column names using underscore (`_`) placeholder
  ```csharp
  var userSet = table.CreateDynamicSetWithAutoFixture();
  ```

- **Supported Data Types**:
  - Basic: `string`, `int`, `decimal`, `double`, `bool`, `guid`
  - Date/Time: `date`, `datetime`, `timespan`
  - Personal: `email`, `name`, `firstname`, `lastname`
  - Contact: `phone`, `address`, `city`, `zipcode/postalcode`, `country`
  - Web: `url/uri`
  - Payment: `creditcard`
  - Collections: `stringlist`, `intlist`, `guidlist`, `datelist`

- **Custom Entity Generation**: Register your own domain entity generators
  ```csharp
  AutoFixtureTableExtensions.RegisterEntityType<User>("user");
  AutoFixtureTableExtensions.RegisterEntityGenerator<PremiumUser>("premium-user", 
    () => new PremiumUser { Level = "Gold" });
  ```

- **Batch Entity Creation**: Generate multiple entities at once
  ```csharp
  var users = AutoFixtureTableExtensions.CreateEntities("User", 5);
  ```

### Async Support

All core methods provide async versions for better performance:

```csharp
// Async dynamic objects
var instance = await table.CreateDynamicInstanceAsync();
var set = await table.CreateDynamicSetAsync();

// Async comparison
await table.CompareToDynamicInstanceAsync(instance);
await table.CompareToDynamicSetAsync(set);

// Async table transformations
var filtered = await table.FilterRowsAsync(predicate);
var projected = await table.SelectColumnsAsync("Name", "Email");
var nested = await table.CreateNestedDynamicInstanceAsync();

// Async AutoFixture integration
var autoInstance = await table.CreateDynamicInstanceWithAutoFixtureAsync();
var autoSet = await table.CreateDynamicSetWithAutoFixtureAsync();
var entities = await AutoFixtureTableExtensions.CreateEntitiesAsync("User", 5);
```

## Installation

```bash
# Package Manager
Install-Package ExecuteAutomation.Reqnroll.Dynamics

# .NET CLI
dotnet add package ExecuteAutomation.Reqnroll.Dynamics

# PackageReference
<PackageReference Include="ExecuteAutomation.Reqnroll.Dynamics" Version="1.1.2" />
```

## Common Examples

**Mixed Specific and Random Data**
```gherkin
Given I have the following users
  | FirstName | LastName | Email      | Age      | IsActive |
  | John      | Smith    | auto.email | auto.int | true     |
  | _         | _        | _          | 25       | false    |
```

**Form Submissions with Random Data**
```gherkin
When I submit the form with the following data
  | Username    | Password    | ConfirmPassword | DateOfBirth | TermsAccepted |
  | auto.string | auto.string | {Password}      | auto.date   | true          |
```

**Testing Data Tables with Multiple Random Entries**
```gherkin
Given the database contains the following products
  | ProductName | Category | Price       | StockLevel | LastUpdated    |
  | _           | _        | auto.double | auto.int   | auto.datetime  |
  | _           | _        | auto.double | auto.int   | auto.datetime  |
```

**Using Collection Types**
```gherkin
Given I have a product with the following details
  | Name        | Price        | Tags           | RelatedProductIds |
  | auto.string | auto.decimal | auto.stringlist| auto.intlist      |
```

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

This project is a fork of [Specflow.Assist.Dynamics](https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic) with enhancements for Reqnroll.
