import { NextRequest } from 'next/server';
import { cookies } from 'next/headers';

const BACKEND_URL = process.env.BACKEND_API_URL!;

interface FetchOptions extends RequestInit {
  params?: Record<string, string>;
}

async function refreshTokens(): Promise<{ accessToken: string; refreshToken: string } | null> {
  const cookieStore = cookies();
  const refreshToken = (await cookieStore).get('refresh_token')?.value;

  if (!refreshToken) return null;

  try {
    const baseUrl = process.env.NEXTAUTH_URL || 'http://localhost:3000';
    const res = await fetch(`${baseUrl}/api/auth/refresh`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
    });

    if (!res.ok) return null;

    const data = await res.json();
    if (!data.success) return null;

    const newCookieStore = cookies();
    return {
      accessToken: (await newCookieStore).get('access_token')?.value || '',
      refreshToken: (await newCookieStore).get('refresh_token')?.value || '',
    };
  } catch {
    return null;
  }
}

export async function fetchWithAuth(
  req: NextRequest,
  path: string,
  options: FetchOptions = {}
): Promise<Response> {
  const cookieStore = cookies();
  let accessToken = (await cookieStore).get('access_token')?.value;

  const makeRequest = async (token?: string) => {
    const url = new URL(path, BACKEND_URL);
    if (options.params) {
      Object.entries(options.params).forEach(([key, value]) => {
        url.searchParams.append(key, value);
      });
    }

    const headers = new Headers(options.headers);
    headers.set('Content-Type', 'application/json');
    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    return fetch(url.toString(), {
      ...options,
      headers,
      body: options.body,
    });
  };

  let response = await makeRequest(accessToken);

  if (response.status === 401) {
    console.log('Token expired, trying refresh...');
    const newTokens = await refreshTokens();

    if (newTokens) {
      const newCookieStore = cookies();
      const newAccessToken = (await newCookieStore).get('access_token')?.value;

      if (newAccessToken) {
        console.log('Refresh success, load request...');
        response = await makeRequest(newAccessToken);
      }
    } else {
      console.log('Refresh failed, deleting cookie...');
      (await cookieStore).delete('access_token');
      (await cookieStore).delete('refresh_token');
    }
  }

  return response;
}