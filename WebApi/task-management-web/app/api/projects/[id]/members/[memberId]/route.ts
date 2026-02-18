import { NextRequest, NextResponse } from 'next/server';
import { fetchWithAuth } from '@/lib/api/backendClient';
import { methodNotAllowed } from '@/lib/api/errors';

export async function DELETE(
  req: NextRequest,
  { params }: { params: Promise<{ id: string; memberId: string }> }
) {
  try {
    const { id, memberId } = await params;

    const backendRes = await fetchWithAuth(
      req,
      `/api/projects/${id}/members/${memberId}`,
      { method: 'DELETE' }
    );

    const data = await backendRes.json();

    if (!backendRes.ok) {
      return NextResponse.json(
        {
          success: false,
          message: data.message || 'Failed to remove member'
        },
        { status: backendRes.status }
      );
    }

    return NextResponse.json(data);
  } catch (error) {
    console.error('Error in remove member:', error);
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
  return methodNotAllowed(['DELETE']);
}
export async function POST() {
  return methodNotAllowed(['DELETE']);
}
export async function PUT() {
  return methodNotAllowed(['DELETE']);
}
export async function PATCH() {
  return methodNotAllowed(['DELETE']);
}