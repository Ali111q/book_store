# Interview Test Project - ASP.NET

## Overview

This project implements a basic ASP.NET Core application. The functionality includes [briefly describe the feature of the app, e.g., a REST API to manage users or tasks, etc.]. The objective of this test is to showcase the ability to build scalable, maintainable solutions using ASP.NET Core.

## Requirements

Ensure you have the following software installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 9.0 or later)
- Postgres database

## Project Structure

- **Controllers**: Contains API controllers for each resource.
- **Models**: Data models used throughout the app.
- **Data**: Database context and Dtos.
- **Services**: Contains business logic and service layer.
- **Migrations**: For Entity Framework migrations (if used).
- **appsettings.json**: Configuration file for database connection strings and other settings.
- **Utils**: Contains all the utils of the project

## Setup Instructions

### 1. Clone the Repository

To get started, clone this repository to your local machine:

```
git clone https://github.com/Ali111q/book_store
cd book_store
```

### 2. Restore Dependencies

Run the following command to restore the required NuGet packages:

```
dotnet restore
```
### 3. Configure Database Connection

Open the `appsettings.json` file and update the connection string for your database:

```
{
"ConnectionStrings": {
"DefaultConnection": "Server=<your-server>;Database=<your-database>;User Id=<your-username>;Password=<your-password>;"
}
}
```

### 4. Apply Migrations (if applicable)

If using Entity Framework Core for database management, run the following command to apply any migrations:

```
dotnet ef database update
```

### 5. Change Utils
Now, You have to change util props in `/Utils/Utils.cs`

```
namespace BookStore.Utils;

public class Util
{
    public static string AppUrl = "<your frontend app>";
}
```

### 6. Run the Application

Now, you can run the application locally:

```
dotnet run
```

The application will be available at `http://localhost:<port>` based on `appsettings.json`.



### Manual Testing

you can open the application in your browser at `http://localhost:<port>/swagger/index.html` to interact with the swagger ui.


Thank you for reviewing this project!
