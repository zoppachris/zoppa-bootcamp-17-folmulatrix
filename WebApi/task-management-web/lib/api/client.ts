const API_BASE = '/api';

interface ErrorResponse {
  type?: string;
  title?: string;
  status?: number;
  errors?: Record<string, string[]>;
  traceId?: string;
  message?: string;
}

async function handleResponse<T>(response: Response, throwError: boolean = true): Promise<T> {
  if (!response.ok) {
    let errorMessage = `HTTP error ${response.status}`;

    try {
      const errorData: ErrorResponse = await response.json();

      if (errorData.title) {
        errorMessage = errorData.title;
      }
      else if (errorData.errors) {
        const firstErrorKey = Object.keys(errorData.errors)[0];
        if (firstErrorKey && errorData.errors[firstErrorKey].length > 0) {
          errorMessage = errorData.errors[firstErrorKey][0];
        }
      }
      else if (errorData.message) {
        errorMessage = errorData.message;
      }

    } catch (e) {
      const text = await response.text().catch(() => '');
      if (text) {
        errorMessage = text;
      }
    }

    if (throwError) {
      throw new Error(errorMessage);
    }
    return Promise.reject(new Error(errorMessage));
  }

  const json = await response.json();
  return json.data as T;
}

export async function apiGet<T>(path: string, params?: Record<string, any>, throwError: boolean = true): Promise<T> {
  const url = new URL(`${API_BASE}${path}`, window.location.origin);
  if (params) {
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        url.searchParams.append(key, String(value));
      }
    });
  }
  const res = await fetch(url.toString(), {
    credentials: 'include',
  });
  return handleResponse<T>(res, throwError);
}

export async function apiPost<T>(path: string, data?: any, throwError: boolean = true): Promise<T> {
  const res = await fetch(`${API_BASE}${path}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    credentials: 'include',
    body: data ? JSON.stringify(data) : undefined,
  });
  return handleResponse<T>(res, throwError);
}

export async function apiPut<T>(path: string, data?: any, throwError: boolean = true): Promise<T> {
  const res = await fetch(`${API_BASE}${path}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    credentials: 'include',
    body: data ? JSON.stringify(data) : undefined,
  });
  return handleResponse<T>(res, throwError);
}

export async function apiDelete<T>(path: string, throwError: boolean = true): Promise<T> {
  const res = await fetch(`${API_BASE}${path}`, {
    method: 'DELETE',
    credentials: 'include',
  });
  return handleResponse<T>(res, throwError);
}

export async function checkAuth(): Promise<boolean> {
  try {
    await apiGet('/users/me', {}, false);
    return true;
  } catch {
    return false;
  }
}