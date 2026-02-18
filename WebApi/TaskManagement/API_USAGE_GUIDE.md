# Todo Web API - Usage Guide

## API Information

- **Base URL**: `http://localhost:5079`
- **Swagger UI**: `http://localhost:5079/swagger`
- **Database**: SQLite (`taskmanagement.db`)
- **Authentication**: JWT Bearer Token

## Default Test Accounts

### Admin Account

- **Email**: `admin@example.com`
- **Password**: `Admin@123`
- **Role**: Admin

## API Endpoints

### Authentication

#### 1. Register New User

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!",
  "userName": "johndoe",
  "fullName": "John Doe"
}
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "a1b2c3d4...",
    "expiration": "2025-03-17T12:00:00Z"
  }
}
```

#### 2. Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "a1b2c3d4...",
    "expiration": "2025-03-17T12:00:00Z"
  }
}
```

#### 3. Logout

```http
POST /api/auth/logout
Content-Type: application/json

{
  "refreshToken": "a1b2c3d4..."
}
```

**Response:**

```json
{
  "success": true,
  "message": "Logged out successfully",
  "data": true,
  "errors": null
}
```

#### 4. Refresh Token

```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "a1b2c3d4..."
}
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "new_refresh_token",
    "expiration": "2025-03-17T12:00:00Z"
  }
}
```

### User Management

#### 1. Get Current User Profile

```http
Get /api/users/me
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userName": "johndoe",
    "email": "user@example.com",
    "fullName": "John Doe"
  }
}
```

#### 2. Update Current User Profile

```http
Put /api/users/me
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "userName": "newusername",
  "email": "newemail@example.com",
  "fullName": "John Updated"
}
```

**Response:**

```json
{
  "success": true,
  "message": "Profile updated successfully",
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "userName": "newusername",
    "email": "newemail@example.com",
    "fullName": "John Updated"
  }
}
```

#### 3. Get List of Users

```http
Get /api/users
Authorization: Bearer YOUR_JWT_TOKEN

Filter :
- searchTerm (optional): Filter by username or full name (case-insensitive).
- SortBy (optional): Sorting by userName or fullName.
- SortOrder (optional): asc or desc. Default: asc.
- pageNumber (default: 1)
- pageSize (default: 10, max: 100)

example :
/api/users?searchTerm=john&SortBy=userName&SortOrder=asc&pageNumber=1&pageSize=10
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userName": "johndoe",
        "fullName": "John Doe"
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1
  }
}
```

### Projects

#### 1. Get User Projects

```http
Get /api/projects
Authorization: Bearer YOUR_JWT_TOKEN

Filter :
- searchTerm (optional): Filter by project name or description.
- SortBy (optional): Field to sort by (name, createdAt). Default: createdAt.
- SortOrder (optional): asc or desc. Default: asc.
- pageNumber (default: 1)
- pageSize (default: 10, max: 100)

example :
/api/projects?searchTerm=api&sortBy=name&sortOrder=asc&pageNumber=1&pageSize=10
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [
      {
        "id": "b7f3b2a1-1234-5678-9abc-def012345678",
        "name": "API Development",
        "description": "Build the REST API",
        "createdAt": "2025-02-01T10:00:00Z",
        "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "ownerName": "johndoe",
        "members": [
          {
            "userId": "a1b2c3d4-...",
            "userName": "janedoe",
            "email": "jane@example.com"
          }
        ]
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1
  }
}
```

#### 2. Get Project by ID

```http
Get /api/projects/{id}
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": "b7f3b2a1-1234-5678-9abc-def012345678",
    "name": "API Development",
    "description": "Build the REST API",
    "createdAt": "2025-02-01T10:00:00Z",
    "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ownerName": "johndoe",
    "members": [...]
  }
}
```

#### 3. Create Project

```http
Post /api/projects
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "name": "New Project",
  "description": "Project description"
}
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": "c8d4e5f6-...",
    "name": "New Project",
    "description": "Project description",
    "createdAt": "2025-02-17T10:00:00Z",
    "ownerId": "...",
    "ownerName": "johndoe",
    "members": []
  }
}
```

#### 4. Update Project

```http
Put /api/projects
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "id": "b7f3b2a1-...",
  "name": "Updated Name",
  "description": "Updated description"
}
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    ... updated project ...
  }
}
```

#### 5. Delete Project

```http
DELETE /api/projects/{id}
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**

```json
{
  "success": true,
  "message": "Project deleted successfully",
  "data": true
}
```

#### 6. Assign Member to Project

```http
POST /assign-member
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "projectId": "b7f3b2a1-...",
  "userId": "a1b2c3d4-..."
}
```

**Response:**

```json
{
  "success": true,
  "message": "Member assigned successfully",
  "data": true
}
```

#### 7. Remove Member from Project

```http
DELETE /{projectId}/members/{memberId}
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**

```json
{
  "success": true,
  "message": "Member removed successfully",
  "data": true
}
```

### Tasks

#### 1. Get Tasks

```http
Get /api/task
Authorization: Bearer YOUR_JWT_TOKEN

Filter :
- status (optional): 0 = Backlog, 1 = InProgress, 2 = Done
- priority (optional): 0 = Low, 1 = Medium, 2 = High
- projectId (optional): Filter by project.
- assignedUserId (optional): Filter by assigned user.
- dueDateFrom (optional): Start of due date range (ISO 8601).
- dueDateTo (optional): End of due date range.
- searchTerm (optional): Search in title and description.
- sortBy (optional): Field to sort by (title, status, priority, dueDate, createdAt). Default: dueDate.
- sortOrder (optional): asc or desc. Default: asc.
- pageNumber (default: 1)
- pageSize (default: 10)

example :
GET /api/tasks?status=1&priority=2&projectId=b7f3b2a1-...&searchTerm=bug&sortBy=dueDate&sortOrder=desc&pageNumber=1&pageSize=10
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [
      {
        "id": "d9e6f7g8-...",
        "title": "Fix login bug",
        "description": "Users cannot log in",
        "status": 1,
        "priority": 2,
        "dueDate": "2025-02-20T00:00:00Z",
        "createdAt": "2025-02-17T10:00:00Z",
        "projectId": "b7f3b2a1-...",
        "assignedUserId": "a1b2c3d4-...",
        "assignedUserName": "janedoe"
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1
  }
}
```

#### 2. Get Task by ID

```http
Get /api/tasks/{id}
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": "d9e6f7g8-...",
    "title": "Fix login bug",
    "description": "Users cannot log in",
    "status": 1,
    "priority": 2,
    "dueDate": "2025-02-20T00:00:00Z",
    "createdAt": "2025-02-17T10:00:00Z",
    "projectId": "b7f3b2a1-...",
    "assignedUserId": "a1b2c3d4-...",
    "assignedUserName": "janedoe"
  }
}
```

#### 3. Create Task

```http
Post /api/tasks
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "title": "Implement search",
  "description": "Add search to tasks",
  "status": 0,
  "priority": 1,
  "dueDate": "2025-02-25T00:00:00Z",
  "projectId": "b7f3b2a1-...",
  "assignedUserId": "a1b2c3d4-..."
}
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    ... created task ...
  }
}
```

#### 4. Update Taks

```http
Put /api/taks
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "id": "d9e6f7g8-...",
  "title": "Updated title",
  "description": "Updated description",
  "status": 1,
  "priority": 2,
  "dueDate": "2025-02-28T00:00:00Z",
  "projectId": "b7f3b2a1-...",
  "assignedUserId": "a1b2c3d4-..."
}
```

**Response:**

```json
{
  "success": true,
  "message": "Success",
  "data": {
    ... updated task ...
  }
}
```

#### 5. Delete Task

```http
DELETE /api/task/{id}
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**

```json
{
  "success": true,
  "message": "Task deleted successfully",
  "data": true
}
```

#### 6. Assign Task to User

```http
POST /{taskId}/assign/{userId}
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**

```json
{
  "success": true,
  "message": "Task assigned successfully",
  "data": true
}
```

### Notes on Filtering, Pagination, and Sorting

- All filter parameters are optional.
- Pagination parameters pageNumber and pageSize apply to all list endpoints.
- Sorting is case-insensitive; use the exact property names as defined in the DTO (e.g., dueDate, createdAt).
- For sortBy, you can use dot notation for nested properties (e.g., project.name) if needed, but check allowed fields per endpoint.
- Date filters (dueDateFrom, dueDateTo) should be provided in ISO 8601 format (e.g., 2025-02-20T00:00:00Z).

### Error Handling

When a request fails, the API returns an appropriate HTTP status code and a response body with success: false and an error message.

Example (400 Bad Request):

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": ["Title is required", "Due date must be in the future"]
}
```

#### Common status codes:

- 200 – OK
- 201 – Created
- 400 – Bad Request (validation error)
- 401 – Unauthorized (missing or invalid token)
- 403 – Forbidden (insufficient permissions)
- 404 – Not Found
- 500 – Internal Server Error
