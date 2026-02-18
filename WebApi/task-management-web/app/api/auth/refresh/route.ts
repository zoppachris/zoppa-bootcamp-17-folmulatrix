import { NextRequest, NextResponse } from 'next/server';
import { cookies } from 'next/headers';
import { methodNotAllowed } from '@/lib/api/errors';

const BACKEND_URL = process.env.BACKEND_API_URL!;

export async function POST(req: NextRequest) {
    try {
        const cookieStore = cookies();
        const refreshToken = (await cookieStore).get('refresh_token')?.value;

        if (!refreshToken) {
            return NextResponse.json(
                { success: false, message: 'Refresh token not found' },
                { status: 401 }
            );
        }

        const backendRes = await fetch(`${BACKEND_URL}/api/auth/refresh-token`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ refreshToken }),
        });

        const data = await backendRes.json();

        if (!backendRes.ok || !data.success) {
            (await cookieStore).delete('access_token');
            (await cookieStore).delete('refresh_token');

            return NextResponse.json(
                { success: false, message: data.message || 'Refresh token failed' },
                { status: backendRes.status || 401 }
            );
        }

        (await cookieStore).set('access_token', data.data.token, {
            httpOnly: true,
            secure: process.env.NODE_ENV === 'production',
            sameSite: 'lax',
            path: '/',
            maxAge: 60 * 60 * 24,
        });

        (await cookieStore).set('refresh_token', data.data.refreshToken, {
            httpOnly: true,
            secure: process.env.NODE_ENV === 'production',
            sameSite: 'lax',
            path: '/',
            maxAge: 60 * 60 * 24 * 7,
        });

        return NextResponse.json({
            success: true,
            message: 'Token updated',
            data: {
                token: data.data.token,
                refreshToken: data.data.refreshToken,
                expiration: data.data.expiration,
            },
        });
    } catch (error) {
        console.error('Error in refresh token:', error);
        return NextResponse.json(
            { success: false, message: 'Internal server error' },
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