### Task Management Web API

A professional Task Management Web API built with ASP.NET Core 9.0 following Clean Architecture principles. It provides a robust backend for managing projects and tasks with user authentication, authorization, and advanced filtering capabilities.

### Project Overview

This API allows users to:

- Register and authenticate using JWT (with refresh token support).
- Create, update, delete, and manage projects.
- Add/remove members to/from projects.
- Create, update, delete, and manage tasks within projects.
- Assign tasks to users.
- Filter and paginate tasks by status, priority, project, assigned user, and due date.
- Secure endpoints with role-based access control (Admin/User).

### Architecture Components

The solution is structured following Clean Architecture with four main projects:

**1. TaskManagement.Domain**

- Contains core business entities (AppUser, Project, TaskItem, RefreshToken, etc.)
- Enums (TaskItemStatus, TaskItemPriority)
- Interfaces for repositories (IRepository<T>, IProjectRepository, ITaskRepository)

**2. TaskManagement.Application**

- Implements application-specific logic (DTOs, mappers, validators, services)
- Interfaces for services (IAuthService, IProjectService, ITaskService)
- FluentValidation validators for all DTOs
- AutoMapper profiles
- ServiceResult pattern for consistent operation results

**3. TaskManagement.Infrastructure**

- Data persistence (Entity Framework Core with SQLite)
- ASP.NET Core Identity integration
- JWT token generation and validation
- Repository implementations (generic and specific)
- Database seeding
- Configuration settings (JwtSettings, RefreshTokenSettings)

**4. TaskManagement.Api**

- RESTful controllers (AuthController, ProjectsController, TasksController)
- Global error handling middleware
- Swagger/OpenAPI documentation
- Program.cs with service registrations and pipeline configuration

### Professional Features

**JWT Authentication** Secure token-based authentication with refresh token rotation
**Role-based Authorization** Admin and User roles with appropriate access restrictions
**Clean Architecture** Separation of concerns, testability, and maintainability
**Repository Pattern** Generic and specific repositories for data access abstraction
**Service Layer** Business logic encapsulated in services with ServiceResult pattern
**DTOs** Data transfer objects for all API inputs/outputs
**FluentValidation** Robust input validation with custom error messages
**AutoMapper** Object-object mapping between entities and DTOs
**User Isolation** Users can only access projects and tasks they own or are members of
**Filtering & Pagination** Advanced filtering and pagination for tasks
**Standardized API Responses** Consistent ApiResponse<T> format for all endpoints
**Database Seeding** Automatic creation of roles and default admin user
**Error Handling Middleware** Centralized exception handling
**Swagger Documentation** Interactive API documentation with JWT support
