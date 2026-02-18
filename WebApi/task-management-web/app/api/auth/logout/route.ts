import { NextRequest, NextResponse } from 'next/server';
import { cookies } from 'next/headers';
import { methodNotAllowed } from '@/lib/api/errors';

const BACKEND_URL = process.env.BACKEND_API_URL!;

export async function POST(req: NextRequest) {
  try {
    const cookieStore = cookies();
    const refreshToken = (await cookieStore).get('refresh_token')?.value;

    if (!refreshToken) {
      (await cookieStore).delete('access_token');
      (await cookieStore).delete('refresh_token');

      return NextResponse.json({
        success: true,
        message: 'Logout berhasil',
      });
    }

    const backendRes = await fetch(`${BACKEND_URL}/api/auth/logout`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refreshToken }),
    });

    (await cookieStore).delete('access_token');
    (await cookieStore).delete('refresh_token');

    if (!backendRes.ok) {
      const data = await backendRes.json().catch(() => ({}));
      console.warn('Logout warning from backend:', data.message || 'Unknown error');
    }

    return NextResponse.json({
      success: true,
      message: 'Logout success',
    });
  } catch (error) {
    console.error('Error in logout:', error);

    const cookieStore = cookies();
    (await cookieStore).delete('access_token');
    (await cookieStore).delete('refresh_token');

    return NextResponse.json(
      {
        success: false,
        message: 'There is something wrong when logout'
      },
      { status: 500 }
    );
  }
}

export async function GET() {
  return methodNotAllowed(['POST']);
}
export async function PUT() {
  return methodNotAllowed(['POST']);
}
export async function DELETE() {
  return methodNotAllowed(['POST']);
}
export async function PATCH() {
  return methodNotAllowed(['POST']);
}