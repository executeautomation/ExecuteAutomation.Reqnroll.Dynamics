# ExecuteAutomation.Reqnroll.Dynamics

**ExecuteAutomation.Reqnroll.Dynamics** is a powerful extension to enhance Reqnroll's Assist APIs, enabling seamless dynamic object creation and enhanced table manipulation for Reqnroll scenarios.

---

## Features

- **Dynamic Object Creation**: Convert Reqnroll tables directly into dynamic objects for simplified testing and validation.
- **Enhanced Table Manipulation**: Effortlessly transform vertical tables into horizontal tables for more intuitive data handling.
- **Customizable Behavior**: Fine-tune how the library processes Reqnroll tables for your specific testing needs.
- **Table Transformation Utilities**: Filter, project, and create nested objects from Reqnroll tables.
- **Async Support**: Use asynchronous methods for better integration with modern testing frameworks.

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

### Table Filtering
Filter rows based on specific criteria:
```csharp
var filteredTable = table.FilterRows(row => row["Status"] == "Active");
```

### Column Projection
Create a new table with only the specified columns:
```csharp
var projectedTable = table.SelectColumns("FirstName", "LastName");
```

### Nested Object Creation
Convert tables with JSON data into nested dynamic objects:
```csharp
// With a table like:
// | Entity   | Properties                              |
// | User     | { "Name": "John", "Age": 30 }           |
// | Address  | { "Street": "Main St", "Number": 123 }  |

var nestedObject = table.CreateNestedDynamicInstance();
// Access: nestedObject.User.Name, nestedObject.Address.Street
```

### Async Support
Use asynchronous methods for better integration with asynchronous test frameworks:
```csharp
var dynamicObject = await table.CreateDynamicInstanceAsync();
await table.CompareToDynamicInstanceAsync(instance);
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
