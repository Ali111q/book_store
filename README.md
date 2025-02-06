# Interview Test Project - ASP.NET

## Overview

This project implements a basic ASP.NET Core application. The functionality includes [briefly describe the feature of the app, e.g., a REST API to manage users or tasks, etc.]. The objective of this test is to showcase the ability to build scalable, maintainable solutions using ASP.NET Core.

## Requirements

Ensure you have the following software installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- Postgres database
- mongodb databasw

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

Open `Utils/Utils.cs` fille and change 
```csharp
    public static string MongoDbConnectionString = "mongodb://localhost:27017";
    public static string MongoDbDataBaseName = "ShopCartDB";
```
to your actual mongodb connection

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
    public static string AppUrl = "http://localhost:5000";
    public static string SmtpClient = "";
    public static int SmtpPort = 587;
    public static string SmtpUser = "";
    public static string SmtpPassword = "";
    public static string SmtpEmail = "";
    public static string MongoDbConnectionString = "mongodb://localhost:27017";
    public static string MongoDbDataBaseName = "ShopCartDB";
    
    
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


## Production

### 1. Fork the Repository
### 2. Change ssh connection

go in repo to `settings=>Secrets and variables=>Actions` and add the following

| Key      | Desc                                                            |
|----------|-----------------------------------------------------------------|
| SSH_USERNAME | Server ssh username                                             |
| SSH_HOST | Server ssh IP                                                   |
| SSH_PASSWORD  | Server ssh Password                                             |   
| TELEGRAM_ID  | **optional**: telegram user id to recieve the update message on |   
| BOT_TOKEN  | **optional**: telegram bot token to send the update message     |   

### 3. Enable Github actions
### 4. Create Service 
1. in the server run `nano /etc/systemd/system/<service name>.service`
2. add this code snippet to it 
```
[Unit]
Description=Backend Service for <app name>
After=network.target

[Service]
# Working directory where your .NET Core app is deployed
WorkingDirectory=/var/www/backend/deploy

# Command to start the .NET Core application (adjust the .dll name if needed)
ExecStart=/usr/bin/dotnet /var/www/backend/deploy/<project name>.dll

# Automatically restart the service if it crashes
Restart=always

# Set the user and group to run the application as
User=www-data
Group=www-data

# Set environment variables (e.g., production environment)
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://0.0.0.0:<project port>  # Specify the port here

# Optional: Log to a specific file
StandardOutput=file:/var/log/dotnet_backend.log
StandardError=file:/var/log/dotnet_backend_error.log

[Install]
WantedBy=multi-user.target
```
3. change service name in `.github/workflows/deploy.yaml`
```yaml
            systemctl_service_name: <your service name>
```
### 5. Create nginx file for it (optional for domain) 


## Test the live version
 You can test live version here [Book Store Backend](https://api.jayak.net/swagger/index.html)

### Test SignalR

- #### clone [signalR test repo](https://github.com/Ali111q/signalRTest)
- #### open `Program.cs` and Do:
  1. Replace <Backend Link> with `http://bookstore-api.ali-hazem.com` if you are using the live version.
  2. Replace <SignalR endpoint> with `/signalR`.
  3. Replace <UserToken> with a valid Bearer token for authentication after login.
  
- #### final file should look like this 

```csharp
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://bookstore-api.ali-hazem.com/signalR", op =>
            {
                op.Headers.Add("Authorization",   "Bearer <UserToken>");
            })
            .Build();

        connection.On<string>("ReceiveNotification", message =>
        {
            Console.WriteLine($"Notification Received: {message}");
        });

        await connection.StartAsync();
        Console.WriteLine("Connected to SignalR Hub!");

        while (true)
        {
            Console.Write("Send message: ");
            var message = Console.ReadLine();
            await connection.InvokeAsync("SendNotificationToUser", Guid.Parse("0194d14a-5769-7520-a3a6-38a8d9d66d13"), message);
        }
    }
}
```
- #### run `dotnet run`
- #### change any order status for the connected user from swagger
- #### you should receive message like this in terminal 
``{ OrderId = 0194d5a0-e594-767e-8b8d-4d0cd6673891, status = ACCEPTED }``
that means it works correctly 
