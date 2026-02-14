TaskManager
TaskManager is a comprehensive project and task management application built using ASP.NET 9.0. It provides features for user authentication, project creation and management, task handling with statuses, priorities, due dates, and advanced filtering. The application follows best practices like Repository Pattern, DTOs with AutoMapper, JWT authentication with Identity, and more. It uses SQLite as the database via Entity Framework with FluentAPI configuration.
Features
A. Account Management

Register: Allows new users to create an account.
Login (JWT): Secure login using JSON Web Tokens (JWT) for authentication.
Refresh Token: Supports token refresh for maintaining sessions without re-login.
Role Management: Users can have roles such as Admin or User, controlling access levels.

B. Project Management

Create Project: Admins or authorized users can create new projects.
Update Project: Edit existing project details.
Delete Project: Remove projects (with appropriate permissions).
Assign & Remove Members: Add or remove users as members to a project.

C. Task Management

CRUD Operations: Create, Read, Update, and Delete tasks.
Assign to User: Assign tasks to specific users.
Status: Tasks can be in states like Backlog, In Progress, or Done.
Priority: Set task priority levels.
Due Date: Assign due dates to tasks for deadline tracking.
Filtering: Filter tasks by:
Status
Project
Assigned User
Due Date

Additional Features

Pagination: Applied to lists of projects and tasks for efficient data handling.

Technologies Used
The application is built with ASP.NET 9.0 and implements the following:

Entity Framework (SQLite) + FluentAPI: For ORM and database configuration.
Repository Pattern: To abstract data access logic.
DTO + AutoMapper: Data Transfer Objects for API responses, mapped using the latest AutoMapper (directly integrated without Microsoft.Extensions.DependencyInjection).
JWT + Identity: For authentication and authorization.
ServiceResult<> Pattern: A custom pattern for handling service responses (e.g., success/failure with messages).
Swagger: Integrated for API documentation and testing.

Folder Structure
The project follows a clean architecture with the following structure:

TaskManagement.Api: Contains API controllers, startup configuration, and Swagger setup.
TaskManagement.Application: Handles application logic, services, DTOs, and AutoMapper profiles.
TaskManagement.Domain: Defines entities, interfaces, and domain models.
TaskManagement.Infrastructure: Manages data access, repositories, Entity Framework context, and database migrations.

Prerequisites

.NET SDK 9.0 or later
SQLite (handled via Entity Framework)
Visual Studio or VS Code for development

Installation

Clone the repository:textgit clone https://github.com/yourusername/TaskManager.git
Navigate to the project directory:textcd TaskManager
Restore NuGet packages:textdotnet restore
Apply database migrations (SQLite will be created automatically):textdotnet ef migrations add InitialCreate --project TaskManagement.Infrastructure
dotnet ef database update --project TaskManagement.Infrastructure

Running the Application

Build the solution:textdotnet build
Run the API:textdotnet run --project TaskManagement.Api
Access Swagger UI at https://localhost:5001/swagger (or the configured port) for API exploration.
Use tools like Postman to test endpoints, starting with registration and login.

Configuration Notes

AutoMapper: Configured directly using the latest AutoMapper package. Profiles are defined in the Application layer for mapping between entities and DTOs.
JWT Settings: Configure secrets and issuer in appsettings.json under the JWT section.
Database: Uses SQLite by default. The connection string is in appsettings.json. For production, consider switching to a more robust database.
Roles: Seed initial roles (Admin/User) in the database seeder if needed.

API Endpoints Overview

Auth: /api/auth/register, /api/auth/login, /api/auth/refresh
Projects: /api/projects (GET/POST/PUT/DELETE), /api/projects/{id}/members
Tasks: /api/tasks (CRUD with filters and pagination), e.g., ?status=InProgress&projectId=1&page=1&pageSize=10

For detailed endpoints, refer to Swagger documentation.
Testing

Unit tests can be added in a separate test project (e.g., TaskManagement.Tests).
Run tests with:textdotnet test

Contributing
Contributions are welcome! Please fork the repository and submit a pull request. Ensure code follows the Repository Pattern and other implemented practices.
