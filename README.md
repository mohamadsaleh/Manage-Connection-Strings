# Manage Connection Strings

A web-based application for managing database connection strings securely.

## Description

This ASP.NET Web Forms application provides a user-friendly interface to manage and store database connection strings. It includes user authentication and secure storage of connection configurations.

![Overview](Overview1.PNG)

## Features

- User login and authentication
- Add, edit, and delete connection strings
- Secure XML-based configuration storage
- Responsive web interface
- Master page layout for consistent UI

## Technologies Used

- **ASP.NET Web Forms** - Web application framework
- **C#** - Programming language
- **XML** - Configuration data storage
- **CSS** - Styling and layout
- **Microsoft .NET Framework** - Runtime environment
- **Visual Studio** - Development environment

## Prerequisites

- .NET Framework 4.5 or higher
- IIS or Visual Studio Development Server
- Web browser (Chrome, Firefox, Edge, etc.)

## Installation and Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/mohamadsaleh/Manage-Connection-Strings.git
   ```

2. Open the solution file `WebsiteConnectionStrings.sln` in Visual Studio

3. Restore NuGet packages:
   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"

4. Configure the application:
    - Sensitive configuration files are gitignored for security
    - Create `App_Data/config/ConnectionStrings.xml` and `App_Data/config/users.xml` with your configurations

### Sample Configuration Files

#### users.xml
```xml
<ArrayOfUser xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <User>
    <UserName>admin</UserName>
    <Password>admin</Password>
  </User>
  <User>
    <UserName>user1</UserName>
    <Password>password1</Password>
  </User>
</ArrayOfUser>
```

#### ConnectionStrings.xml
```xml
<ArrayOfConnectionString xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ConnectionString>
    <ConnectionStringName>DefaultConnection</ConnectionStringName>
    <ConnectionStringValue>Server=localhost;Database=MyDB;User Id=sa;Password=mypassword;</ConnectionStringValue>
  </ConnectionString>
  <ConnectionString>
    <ConnectionStringName>TestConnection</ConnectionStringName>
    <ConnectionStringValue>Server=testserver;Database=TestDB;Integrated Security=true;</ConnectionStringValue>
  </ConnectionString>
</ArrayOfConnectionString>
```

5. Build and run the application:
   - Press F5 or click "Start Debugging" in Visual Studio
   - The application will open in your default web browser

## Project Structure

- `App_Code/` - Business logic and data access classes
- `App_Data/config/` - Configuration files (gitignored)
- `Bin/` - Compiled assemblies
- `*.aspx` - Web forms
- `*.aspx.cs` - Code-behind files
- `Web.config` - Application configuration

## Security Note

Sensitive files containing connection strings and user data are excluded from version control via `.gitignore` for security reasons. Make sure to create these files locally with appropriate configurations.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is for educational and demonstration purposes.