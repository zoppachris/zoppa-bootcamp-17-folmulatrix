API Usage Guide for TaskManagement
This document provides a comprehensive guide on how to use the TaskManagement API. The API is built using ASP.NET 9.0, with features including user authentication (JWT), project management, task CRUD operations, pagination, and filtering. It follows a clean architecture with the following folder structure:

TaskManagement.Api: Handles API controllers, endpoints, and Swagger configuration.
TaskManagement.Application: Contains services, DTOs, AutoMapper profiles (using the latest AutoMapper directly, without Microsoft.Extensions.DependencyInjection), and ServiceResult<> pattern for handling responses.
TaskManagement.Domain: Defines entities, enums (e.g., TaskStatus, TaskPriority), and domain logic.
TaskManagement.Infrastructure: Includes repositories (Repository Pattern), Entity Framework Core with Sqlite database and FluentAPI for configuration, and JWT + Identity setup.

The API uses Swagger for interactive documentation. Access it at /swagger when the API is running.
Authentication
All endpoints (except registration and login) require JWT authentication. Use the Authorization header with Bearer {token}.

1. Register

Endpoint: POST /api/account/register
Description: Registers a new user.
Request Body (JSON):JSON{
"username": "string",
"email": "string",
"password": "string",
"role": "Admin" or "User" // Optional, defaults to "User"
}
Response:
201 Created: User registered successfully (ServiceResult with success message).
400 Bad Request: Validation errors (e.g., duplicate email).

2. Login

Endpoint: POST /api/account/login
Description: Authenticates a user and returns JWT token.
Request Body (JSON):JSON{
"email": "string",
"password": "string"
}
Response:
200 OK:JSON{
"token": "string", // JWT access token
"refreshToken": "string",
"expiresIn": "int" // In minutes
}
401 Unauthorized: Invalid credentials.

3. Refresh Token

Endpoint: POST /api/account/refresh-token
Description: Refreshes the JWT using a refresh token.
Request Body (JSON):JSON{
"refreshToken": "string"
}
Response:
200 OK: New JWT and refresh token.
401 Unauthorized: Invalid refresh token.

Projects
Projects can be managed by authenticated users. Admins have full access; Users can only manage projects they own or are assigned to.

1. Create Project

Endpoint: POST /api/projects
Description: Creates a new project.
Request Body (JSON):JSON{
"name": "string",
"description": "string"
}
Response:
201 Created: Project created (returns Project DTO).
400 Bad Request: Validation errors.

2. Update Project

Endpoint: PUT /api/projects/{id}
Description: Updates an existing project.
Path Parameter: id (int) - Project ID.
Request Body (JSON):JSON{
"name": "string",
"description": "string"
}
Response:
200 OK: Project updated.
404 Not Found: Project not found.
403 Forbidden: Unauthorized to update.

3. Delete Project

Endpoint: DELETE /api/projects/{id}
Description: Deletes a project.
Path Parameter: id (int) - Project ID.
Response:
204 No Content: Project deleted.
404 Not Found: Project not found.
403 Forbidden: Unauthorized.

4. Assign Member to Project

Endpoint: POST /api/projects/{id}/members
Description: Assigns a user to the project.
Path Parameter: id (int) - Project ID.
Request Body (JSON):JSON{
"userId": "int"
}
Response:
200 OK: Member assigned.
404 Not Found: Project or user not found.

5. Remove Member from Project

Endpoint: DELETE /api/projects/{id}/members/{userId}
Description: Removes a user from the project.
Path Parameters:
id (int) - Project ID.
userId (int) - User ID.

Response:
204 No Content: Member removed.
404 Not Found: Project or user not found.

6. Get Projects (with Pagination)

Endpoint: GET /api/projects?page={page}&pageSize={pageSize}
Description: Retrieves a paginated list of projects.
Query Parameters:
page (int, optional, default: 1)
pageSize (int, optional, default: 10)

Response:
200 OK: Paginated list of Project DTOs (includes total count, current page, etc.).

Tasks
Tasks are associated with projects. CRUD operations with status, priority, due date, and assignment.

1. Create Task

Endpoint: POST /api/tasks
Description: Creates a new task in a project.
Request Body (JSON):JSON{
"projectId": "int",
"title": "string",
"description": "string",
"status": "Backlog" | "InProgress" | "Done",
"priority": "Low" | "Medium" | "High",
"dueDate": "YYYY-MM-DD",
"assignedUserId": "int" // Optional
}
Response:
201 Created: Task created (returns Task DTO).

2. Update Task

Endpoint: PUT /api/tasks/{id}
Description: Updates an existing task.
Path Parameter: id (int) - Task ID.
Request Body (JSON): Same as create, excluding projectId.
Response:
200 OK: Task updated.
404 Not Found: Task not found.

3. Delete Task

Endpoint: DELETE /api/tasks/{id}
Description: Deletes a task.
Path Parameter: id (int) - Task ID.
Response:
204 No Content: Task deleted.
404 Not Found: Task not found.

4. Get Task by ID

Endpoint: GET /api/tasks/{id}
Description: Retrieves a single task.
Path Parameter: id (int) - Task ID.
Response:
200 OK: Task DTO.
404 Not Found: Task not found.

5. Get Tasks (with Pagination and Filtering)

Endpoint: GET /api/tasks?page={page}&pageSize={pageSize}&status={status}&projectId={projectId}&assignedUserId={assignedUserId}&dueDate={dueDate}
Description: Retrieves a paginated and filtered list of tasks.
Query Parameters:
page (int, optional, default: 1)
pageSize (int, optional, default: 10)
status (string, optional): "Backlog" | "InProgress" | "Done"
projectId (int, optional)
assignedUserId (int, optional)
dueDate (string, optional): "YYYY-MM-DD" (filters tasks due on or before this date)

Response:
200 OK: Paginated list of Task DTOs.

6. Assign Task to User

Endpoint: PUT /api/tasks/{id}/assign
Description: Assigns a task to a user.
Path Parameter: id (int) - Task ID.
Request Body (JSON):JSON{
"userId": "int"
}
Response:
200 OK: Task assigned.
404 Not Found: Task or user not found.

Error Handling
All responses use the ServiceResult<> pattern:

Success: { "success": true, "data": {...}, "message": "string" }
Failure: { "success": false, "errors": ["string"], "message": "string" }

Swagger Integration
Run the API and navigate to /swagger for interactive testing. Authentication is supported via Swagger's authorize button.
Notes

Roles: Admins can access all resources; Users are limited to their own or assigned projects/tasks.
Database: Uses Sqlite for development; configure connection string in appsettings.json.
AutoMapper: Configured directly in the application layer for DTO-entity mapping.
Pagination: All list endpoints support pagination for efficient data retrieval.
