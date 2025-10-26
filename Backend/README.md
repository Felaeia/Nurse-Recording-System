# Overview
The Patient Tracker System backend is a C# ASP.NET Core Web API designed to assist 
communication between students and clinic nurses. It manages patient form submissions, 
notifications, medicine inventory, and clinic status updates with real-time synchronization 
across mobile and web clients.

This system ensures that students can quickly report health issues while nurses receive timely notifications, 
manage patient records, and keep the clinic�s operational status and inventory updated.

# NurseRecordingSystem
A Project from ACLC College of Mandaue
Software Engineering I

# Features
- User Authentication
- Form Management
- Nurse Notification


# Technologies Used
- C# .NET 8
- ASP.NET Core Web API
- DAPPER
- SQL Server

# Packages Used
- Microsoft.Data.SqlClient (6.1.1)
- Swashbuckle.AspNetCore (9.0.3)

# How to Run
## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Database Setup
1. Open SQL Server Management Studio (or any other SQL Server client).
2. Open the `SqlQuerries/CreateDatabase.txt` file.
3. **Important**: In the `CREATE DATABASE` script, you may need to change the `FILENAME` path for the `.mdf` and `.ldf` files to a directory that exists on your machine.
4. Run the script to create the `NurseRecordingSystem` database and all the necessary tables.
5. After creating the database, run all the scripts in the `SqlQuerries/StoredProcedures/` directory. This will create the stored procedures used by the application.

## Configuration
1. Open the `appsettings.json` file.
2. Modify the `DefaultConnection` connection string to point to your SQL Server instance.

## Running the Application
1. Open a terminal or command prompt in the project's root directory.
2. Run the following command to start the API server:
   ```bash
   dotnet run
   ```
3. The API will be running on the port specified in the `Properties/launchSettings.json` file (usually `https://localhost:5001` or `http://localhost:5000`). You can access the Swagger UI at `https://localhost:<port>/swagger/index.html`.