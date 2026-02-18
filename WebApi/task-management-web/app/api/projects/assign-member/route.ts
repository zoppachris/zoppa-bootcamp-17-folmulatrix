import { NextRequest, NextResponse } from 'next/server';
import { fetchWithAuth } from '@/lib/api/backendClient';
import { methodNotAllowed } from '@/lib/api/errors';

export async function POST(req: NextRequest) {
  try {
    const body = await req.json();

    if (!body.projectId || !body.userId) {
      return NextResponse.json(
        {
          success: false,
          message: 'projectId and userId must fill in'
        },
        { status: 400 }
      );
    }

    const backendRes = await fetchWithAuth(req, '/api/projects/assign-member', {
      method: 'POST',
      body: JSON.stringify(body),
    });

    const data = await backendRes.json();

    if (!backendRes.ok) {
      return NextResponse.json(
        {
          success: false,
          message: data.message || 'Failed to add member'
        },
        { status: backendRes.status }
      );
    }

    return NextResponse.json(data);
  } catch (error) {
    console.error('Error in assign-member:', error);
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