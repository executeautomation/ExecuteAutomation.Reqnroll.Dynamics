# ExecuteAutomation.Reqnroll.Dynamics

A powerful library that enhances [Reqnroll](https://reqnroll.net/) (formerly SpecFlow) with dynamic data handling capabilities, simplifying how data is created and manipulated in Gherkin feature files.

---

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

### Enhanced Table Manipulation

- **Horizontal/Vertical Table Transformation**: Transform between table formats
  ```csharp
  var horizontalTable = CreateHorizontalTable(verticalTable);
  ```

- **Table Filtering**: Filter rows based on specific criteria
  ```csharp
  var filteredTable = table.FilterRows(row => row["Status"] == "Active");
  ```

- **Column Projection**: Create a new table with only specified columns
  ```csharp
  var projectedTable = table.SelectColumns("FirstName", "LastName");
  ```

- **Nested Object Creation**: Create hierarchical objects from tables with JSON data
  ```csharp
  var nestedObject = table.CreateNestedDynamicInstance();
  ```

### Async Support

- **Asynchronous Operations**: Use async versions of all core methods
  ```csharp
  var dynamicObject = await table.CreateDynamicInstanceAsync();
  var dynamicSet = await table.CreateDynamicSetAsync();
  await table.CompareToDynamicInstanceAsync(instance);
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

### Using Transformations in Step Definitions

Due to the dynamic nature of this library, automatic step argument transformations are not supported. Instead, you should explicitly call the transformation methods in your step definitions:

```csharp
// Explicitly transform the table to a dynamic object
[Given(@"I have a user with following details")]
public void GivenIHaveAUserWithFollowingDetails(Table table)
{
    dynamic user = table.CreateDynamicInstance();
    string name = user.Name;
    int age = user.Age;
}

// For collections of dynamic objects
[Given(@"I have the following users")]
public void GivenIHaveTheFollowingUsers(Table table)
{
    var users = table.CreateDynamicSet();
    foreach (var user in users)
    {
        // Process each user
    }
}

// For nested objects
[Given(@"I have the following entities")]
public void GivenIHaveTheFollowingEntities(Table table)
{
    dynamic entities = table.CreateNestedDynamicInstance();
    string userName = entities.User.Name;
    string addressStreet = entities.Address.Street;
}

// For async operations
[Given(@"I have async data")]
public async Task GivenIHaveAsyncData(Table table)
{
    dynamic data = await table.CreateDynamicInstanceAsync();
    // Use the data
}
```

---

## Installation

To install **ExecuteAutomation.Reqnroll.Dynamics**, add the NuGet package to your project using one of the following methods:

### Package Manager
```bash
Install-Package ExecuteAutomation.Reqnroll.Dynamics
```

### .NET CLI
```bash
dotnet add package ExecuteAutomation.Reqnroll.Dynamics
```

### PackageReference
Add the following line to your `.csproj` file:
```xml
<PackageReference Include="ExecuteAutomation.Reqnroll.Dynamics" Version="1.0.0" />
```

---

## Getting Started

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

---

## Advanced Features

### Dynamic Object Creation

#### Creating from a Horizontal Table

```csharp
// Given a table:
// | Name  | Age | IsActive |
// | John  | 30  | true     |

var person = table.CreateDynamicInstance();
// person.Name == "John"
// person.Age == 30 (int)
// person.IsActive == true (bool)
```

#### Creating from a Vertical Table

```csharp
// Given a table:
// | Field    | Value  |
// | Name     | John   |
// | Age      | 30     |
// | IsActive | true   |

var person = table.CreateDynamicInstance();
// person.Name == "John"
// person.Age == 30 (int)
// person.IsActive == true (bool)
```

### Creating a Set of Dynamic Objects

```csharp
// Given a table:
// | Name  | Age | IsActive |
// | John  | 30  | true     |
// | Jane  | 25  | false    |
// | Bob   | 45  | true     |

var people = table.CreateDynamicSet();
// people[0].Name == "John", people[0].Age == 30, people[0].IsActive == true
// people[1].Name == "Jane", people[1].Age == 25, people[1].IsActive == false
// people[2].Name == "Bob", people[2].Age == 45, people[2].IsActive == true
```

### Table Transformation Utilities

#### Filtering Table Rows

```csharp
// Given a table:
// | Name  | Age | IsActive |
// | John  | 30  | true     |
// | Jane  | 25  | false    |
// | Bob   | 45  | true     |

var filteredTable = table.FilterRows(row => row["IsActive"] == "true");
// filteredTable will have 2 rows: John and Bob
```

#### Projecting Table Columns

```csharp
// Given a table:
// | Name  | Age | Email             | Phone      |
// | John  | 30  | john@example.com  | 1234567890 |
// | Jane  | 25  | jane@example.com  | 0987654321 |

var projectedTable = table.SelectColumns("Name", "Email");
// projectedTable will have columns: Name, Email
```

#### Creating Nested Objects

```csharp
// Given a table:
// | Entity   | Properties                                     |
// | User     | {"Name": "John", "Age": 30, "IsActive": true}  |
// | Address  | {"Street": "Main St", "City": "New York"}      |

var entity = table.CreateNestedDynamicInstance();
// entity.User.Name == "John"
// entity.User.Age == 30
// entity.User.IsActive == true
// entity.Address.Street == "Main St"
// entity.Address.City == "New York"
```

### Async Support

Use asynchronous methods for better integration with asynchronous test frameworks:

```csharp
// Async dynamic instance creation
var dynamicObject = await table.CreateDynamicInstanceAsync();

// Async dynamic set creation
var dynamicSet = await table.CreateDynamicSetAsync();

// Async comparison operations
await table.CompareToDynamicInstanceAsync(instance);
await table.CompareToDynamicSetAsync(set);

// Async table transformations
var filteredTable = await table.FilterRowsAsync(row => row["Status"] == "Active");
var projectedTable = await table.SelectColumnsAsync("FirstName", "LastName");
var nestedObject = await table.CreateNestedDynamicInstanceAsync();
```

### Type Conversion

The library automatically handles type conversion for common types:
- Strings
- Integers
- Doubles/Decimals
- Booleans
- DateTime values

---

## Contributing

Contributions are welcome! Fork this repository, create a branch, and submit a pull request with your changes.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Acknowledgments

This project is a fork of the popular [Specflow.Assist.Dynamics](https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic) package, with enhancements tailored for the **Reqnroll** community.
