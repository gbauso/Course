# Course Sign-up System

This repository contains the solution for the proposed challenge using OOP, DDD, CQRS and Azure Products.
Each part has its own branch.

## Stack

* [.NET Core 3.1](https://dotnet.microsoft.com/)
* [ASP NET Core 3.1](https://docs.microsoft.com/aspnet/core) 
* [Entity Framework Core 3.1](https://docs.microsoft.com/en-us/ef/core/)
* [Automapper](https://github.com/AutoMapper/AutoMapper)
* [MediatR](https://github.com/jbogard/MediatR)
* [Azure Storage](https://azure.microsoft.com/services/storage/)
* [Azure Functions](https://azure.microsoft.com/services/functions/)
* [Azure Service Bus](https://azure.microsoft.com/services/service-bus/)
* [Azure CosmosDB](https://azure.microsoft.com/services/cosmos-db/) 
* [Microsoft SQL Server 2019](https://www.microsoft.com/sql-server/sql-server-2019) 

## How to run

### Visual Studio

1. Create a Queue with name "course.queue" at Azure Service Bus
2. Create a Container with name "Course" and Partition key "/id" on Database id "report" at Azure Cosmos Db
3. Replace the provided keys bellow on appsettings.json (Course.API and Course.Worker) and local.settings.json (Course.Job)
* {SB_URI} - Service bus url (e.g https://course.servicebus.windows.net)
* {SB_KEYNAME} - Service bus shared Key name
* {SB_KEY} - Service bus shared key
* {COSMOSDB_CS} - Cosmos DB connection string (e.g AccountEndpoint=https://*url*/;AccountKey=*key*)
* {MSSQL_CS} - MSSQL connection string
* {TABLE_STORAGE_CS} - connection string for Azure Cloud Table
* {BLOB_STORAGE_CS} - connection string for Azure Blob Storage
* {API_URL} - Base url of API
* {LOGS_PATH} - Logs Path

4. Run Migrations on Course.Infrastructure
5. Mark the projects Course.API, Course.Worker and Course.Job as Start on "Set StartUp Projects..."
6. Run the Solution

### Docker (Soon)
