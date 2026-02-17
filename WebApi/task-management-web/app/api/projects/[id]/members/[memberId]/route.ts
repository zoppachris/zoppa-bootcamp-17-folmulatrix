import { NextRequest, NextResponse } from "next/server";
import { fetchWithAuth } from "@/lib/api/backendClient";

export async function DELETE(
  req: NextRequest,
  { params }: { params: Promise<{ id: string; memberId: string }> },
) {
  try {
    const { id, memberId } = await params;

    const backendRes = await fetchWithAuth(
      req,
      `/api/projects/${id}/members/${memberId}`,
      { method: "DELETE" },
    );
    const data = await backendRes.json();

    if (!backendRes.ok) {
      return NextResponse.json(
        { message: data.message },
        { status: backendRes.status },
      );
    }

    return NextResponse.json(data);
  } catch (error) {
    return NextResponse.json(
      { message: "Internal server error" },
      { status: 500 },
    );
  }
}
