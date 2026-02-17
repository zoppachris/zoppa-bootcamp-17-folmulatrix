import { NextRequest } from "next/server";
import { cookies } from "next/headers";

const BACKEND_URL = process.env.BACKEND_API_URL!;

interface FetchOptions extends RequestInit {
  params?: Record<string, string>;
}

async function refreshTokens(): Promise<{
  accessToken: string;
  refreshToken: string;
} | null> {
  const cookieStore = cookies();
  const refreshToken = (await cookieStore).get("refresh_token")?.value;

  if (!refreshToken) return null;

  try {
    const res = await fetch(`${BACKEND_URL}/api/auth/refresh-token`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ refreshToken }),
    });

    if (!res.ok) return null;

    const data = await res.json();
    if (!data.success) return null;

    return {
      accessToken: data.data.token,
      refreshToken: data.data.refreshToken,
    };
  } catch {
    return null;
  }
}

export async function fetchWithAuth(
  req: NextRequest,
  path: string,
  options: FetchOptions = {},
): Promise<Response> {
  const cookieStore = cookies();
  let accessToken = (await cookieStore).get("access_token")?.value;

  const makeRequest = async (token?: string) => {
    const url = new URL(path, BACKEND_URL);
    if (options.params) {
      Object.entries(options.params).forEach(([key, value]) => {
        url.searchParams.append(key, value);
      });
    }

    const headers = new Headers(options.headers);
    headers.set("Content-Type", "application/json");
    if (token) {
      headers.set("Authorization", `Bearer ${token}`);
    }

    return fetch(url.toString(), {
      ...options,
      headers,
      body: options.body,
    });
  };

  let response = await makeRequest(accessToken);

  if (response.status === 401) {
    const newTokens = await refreshTokens();
    if (newTokens) {
      (await cookieStore).set("access_token", newTokens.accessToken, {
        httpOnly: true,
        secure: process.env.NODE_ENV === "production",
        sameSite: "lax",
        path: "/",
      });
      (await cookieStore).set("refresh_token", newTokens.refreshToken, {
        httpOnly: true,
        secure: process.env.NODE_ENV === "production",
        sameSite: "lax",
        path: "/",
      });

      response = await makeRequest(newTokens.accessToken);
    }
  }

  return response;
}
