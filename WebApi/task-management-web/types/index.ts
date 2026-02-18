export interface User {
  id: string;
  userName: string;
  email: string;
  fullName: string;
  role: string;
}

export interface Project {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  ownerId: string;
  ownerName: string;
  members: ProjectMember[];
}

export interface ProjectMember {
  userId: string;
  userName: string;
  email: string;
}

export interface Task {
  id: string;
  title: string;
  description: string;
  status: number; // 0=Backlog,1=InProgress,2=Done
  priority: number; // 0=Low,1=Medium,2=High
  dueDate: string;
  createdAt: string;
  projectId: string;
  assignedUserId?: string;
  assignedUserName?: string;
}

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors?: string[];
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterData {
  email: string;
  password: string;
  userName: string;
  fullName: string;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiration: string;
}

export interface ProjectFilter {
  searchTerm?: string;
  sortBy?: string;
  sortOrder?: "asc" | "desc";
  pageNumber?: number;
  pageSize?: number;
}

export interface TaskFilter {
  status?: number;
  priority?: number;
  projectId?: string;
  assignedUserId?: string;
  dueDateFrom?: string;
  dueDateTo?: string;
  searchTerm?: string;
  sortBy?: string;
  sortOrder?: "asc" | "desc";
  pageNumber?: number;
  pageSize?: number;
}
