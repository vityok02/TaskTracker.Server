# TaskTracker.Server

**TaskTracker.Server** is the backend part of a task management application built using .NET and Clean Architecture principles. It provides a REST API for managing tasks, priorities, users, and other entities related to productivity and team collaboration.

🔗 **Live Demo (Client)**: [https://tasktracker-client.azurewebsites.net](https://tasktracker-client.azurewebsites.net)

## 🔧 Technologies

- .NET 9
- ASP.NET Core
- Dapper
- MSSQL
- Clean Architecture
- AutoMapper
- FluentValidation
- JWT Authentication

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/TaskTracker.Server.git
   cd TaskTracker.Server
   
2. Configure your appsettings.json.

3. Run the API:

   ```bash  
   dotnet run --project API

4. 📦 Deployment
The server can be deployed independently or together with the frontend. The client (available at the demo link) communicates with this API using HTTP calls (REST).
