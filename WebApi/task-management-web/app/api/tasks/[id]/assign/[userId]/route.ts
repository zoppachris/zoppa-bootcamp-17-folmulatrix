import { NextRequest, NextResponse } from 'next/server';
import { fetchWithAuth } from '@/lib/api/backendClient';
import { methodNotAllowed } from '@/lib/api/errors';

export async function POST(
  req: NextRequest,
  { params }: { params: Promise<{ id: string; userId: string }> }
) {
  try {
    const { id, userId } = await params;

    const backendRes = await fetchWithAuth(
      req,
      `/api/tasks/${id}/assign/${userId}`,
      { method: 'POST' }
    );

    const data = await backendRes.json();

    if (!backendRes.ok) {
      return NextResponse.json(
        {
          success: false,
          message: data.message || 'Failed to assign task'
        },
        { status: backendRes.status }
      );
    }

    return NextResponse.json(data);
  } catch (error) {
    console.error('Error in assign task:', error);
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