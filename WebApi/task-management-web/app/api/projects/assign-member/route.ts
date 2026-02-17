import { NextRequest, NextResponse } from "next/server";
import { fetchWithAuth } from "@/lib/api/backendClient";

export async function POST(req: NextRequest) {
  try {
    const body = await req.json();
    const backendRes = await fetchWithAuth(req, `/api/projects/assign-member`, {
      method: "POST",
      body: JSON.stringify(body),
    });
    const data = await backendRes.json();
    if (!backendRes.ok) {
      return NextResponse.json(
        { message: data.message },
        { status: backendRes.status },
      );
    }
    return NextResponse.json(data);
  } catch {
    return NextResponse.json(
      { message: "Internal server error" },
      { status: 500 },
    );
  }
}
