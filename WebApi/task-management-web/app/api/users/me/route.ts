import { NextRequest, NextResponse } from "next/server";
import { fetchWithAuth } from "@/lib/api/backendClient";
import { methodNotAllowed } from "@/lib/api/errors";

export async function GET(req: NextRequest) {
  try {
    const backendRes = await fetchWithAuth(req, "/api/users/me");
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

export async function PUT(req: NextRequest) {
  try {
    const body = await req.json();
    const backendRes = await fetchWithAuth(req, "/api/users/me", {
      method: "PUT",
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
  } catch (error) {
    return NextResponse.json(
      { message: "Internal server error" },
      { status: 500 },
    );
  }
}

export async function POST() {
  return methodNotAllowed(["GET", "PUT"]);
}
export async function DELETE() {
  return methodNotAllowed(["GET", "PUT"]);
}
export async function PATCH() {
  return methodNotAllowed(["GET", "PUT"]);
}
