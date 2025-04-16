# ExecuteAutomation.Reqnroll.Dynamics

**ExecuteAutomation.Reqnroll.Dynamics** is a powerful extension to enhance Reqnroll's Assist APIs, enabling seamless dynamic object creation and enhanced table manipulation for Reqnroll scenarios.

---

## Features

- **Dynamic Object Creation**: Convert Reqnroll tables directly into dynamic objects for simplified testing and validation.
- **Enhanced Table Manipulation**: Effortlessly transform vertical tables into horizontal tables for more intuitive data handling.
- **Customizable Behavior**: Fine-tune how the library processes Reqnroll tables for your specific testing needs.
- **AutoFixture Integration**: Generate realistic test data using AutoFixture with underscore (`_`) or explicit type placeholders.

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
<PackageReference Include="ExecuteAutomation.Reqnroll.Dynamics" Version="1.1.2" />
```

---

## Usage

### Dynamic Object Creation
You can convert Reqnroll tables into dynamic objects effortlessly:
```csharp
var dynamicObject = table.CreateDynamicInstance();
```

### Horizontal Table Transformation
Transform vertical tables to horizontal tables for more intuitive handling:
```csharp
var horizontalTable = CreateHorizontalTable(verticalTable);
```

### AutoFixture Data Generation

#### Using Underscore Placeholders
Use underscore (`_`) as a placeholder to automatically generate appropriate data based on column names:

```gherkin
Given users with the following details:
  | Username | Email | DateOfBirth | PhoneNumber | Guid | Zipcode |
  | _        | _     | _           | _           | _    | _       |
```

```csharp
// Create a dynamic set with auto-generated values
var userSet = table.CreateDynamicSetWithAutoFixture();
```

#### Using Explicit Type Placeholders
Specify exact data types with the `auto.` prefix for precise control:

```gherkin
Given users with the following details:
  | Username    | Email      | DateOfBirth | PhoneNumber | Guid      | Zipcode      |
  | auto.string | auto.email | auto.date   | auto.phone  | auto.guid | auto.zipcode |
```

```csharp
// Create a dynamic instance with typed auto-generated values
var userInstance = table.CreateDynamicInstanceWithAutoFixture();
```

#### Supported Data Types
The library now supports an extensive range of data types in both underscore and explicit notation:

- **Basic Types**: string, int, decimal, double, bool, guid
- **Date/Time**: date, datetime, timespan
- **Personal Info**: email, name, firstname, lastname
- **Contact**: phone, address, city, zipcode/postalcode, country
- **Web**: url/uri
- **Payment**: creditcard
- **Collections**: stringlist, intlist, guidlist, datelist

#### Common Examples

**Creating Test Users with Mixed Specific and Random Data**
```gherkin
Given I have the following users
  | FirstName | LastName | Email      | Age      | IsActive |
  | John      | Smith    | auto.email | auto.int | true     |
  | _         | _        | _          | 25       | false    |
```

**Testing Form Submissions with Random Data**
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
  | _           | _        | auto.double | auto.int   | auto.datetime  |
```

**Using Collection Types**
```gherkin
Given I have a product with the following details
  | Name        | Price        | Tags           | RelatedProductIds |
  | auto.string | auto.decimal | auto.stringlist| auto.intlist      |
```

### Async Support
The library provides async versions of all methods for better performance in I/O bound scenarios:

```csharp
// Create a dynamic instance asynchronously
var instance = await table.CreateDynamicInstanceAsync();

// Create a dynamic set asynchronously
var set = await table.CreateDynamicSetAsync();

// Create a dynamic instance with AutoFixture asynchronously
var autoFixtureInstance = await table.CreateDynamicInstanceWithAutoFixtureAsync();

// Create a dynamic set with AutoFixture asynchronously
var autoFixtureSet = await table.CreateDynamicSetWithAutoFixtureAsync();

// Create entities asynchronously
var entities = await AutoFixtureTableExtensions.CreateEntitiesAsync("User", 5);
```

---

## Contributing

Contributions are welcome! Fork this repository, create a branch, and submit a pull request with your changes.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Acknowledgments

This project is a fork of the popular [Specflow.Assist.Dynamics](https://github.com/marcusoftnet/SpecFlow.Assist.Dynamic) package, with enhancements tailored for the **Reqnroll** community.

---

## Recent Updates

### Enhanced Type Inference (September 2023)
- **Consistent Type Handling**: Added comprehensive type inference for both underscore (`_`) and explicit `auto.xyz` placeholders
- **Collection Support**: Added support for collection types including stringlist, intlist, guidlist, and datelist
- **Advanced Type Detection**: Enhanced header name analysis for more accurate type inference
- **Parameter Support**: Added customization options for collection sizes with `auto.xyzlist(n)` syntax
