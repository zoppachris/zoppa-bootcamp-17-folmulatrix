import { NextRequest, NextResponse } from 'next/server';
import { cookies } from 'next/headers';
import { methodNotAllowed } from '@/lib/api/errors';

const BACKEND_URL = process.env.BACKEND_API_URL!;

export async function POST(req: NextRequest) {
    try {
        const body = await req.json();

        if (!body.email || !body.password || !body.userName || !body.fullName) {
            return NextResponse.json(
                {
                    success: false,
                    message: 'All field need to be fill: email, password, userName, fullName'
                },
                { status: 400 }
            );
        }

        const backendRes = await fetch(`${BACKEND_URL}/api/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body),
        });

        const data = await backendRes.json();

        if (!backendRes.ok || !data.success) {
            return NextResponse.json(
                {
                    success: false,
                    message: data.message || 'Registrasi failed',
                    errors: data.errors
                },
                { status: backendRes.status || 400 }
            );
        }

        if (data.data?.token && data.data?.refreshToken) {
            const cookieStore = cookies();

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
        }

        return NextResponse.json({
            success: true,
            message: 'Registrasi success',
        });
    } catch (error) {
        console.error('Error in register:', error);
        return NextResponse.json(
            {
                success: false,
                message: 'Internal server error'
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