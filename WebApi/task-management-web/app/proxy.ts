import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";
import { jwtVerify } from "jose";

const JWT_SECRET = new TextEncoder().encode(
  process.env.JWT_SECRET || "your-secret-key",
);

export async function proxy(request: NextRequest) {
  const path = request.nextUrl.pathname;

  if (path === "/login" || path === "/register" || path === "/") {
    return NextResponse.next();
  }

  const accessToken = request.cookies.get("access_token")?.value;

  if (path === "/login" || path === "/register") {
    if (accessToken) {
      return NextResponse.redirect(new URL("/dashboard", request.url));
    }
    return NextResponse.next();
  }

  if (!accessToken) {
    return NextResponse.redirect(new URL("/login", request.url));
  }

  try {
    const { payload } = await jwtVerify(accessToken, JWT_SECRET);
    const role = payload.role as string;

    if (path.startsWith("/admin") && role !== "Admin") {
      return NextResponse.redirect(new URL("/dashboard", request.url));
    }

    return NextResponse.next();
  } catch (error) {
    return NextResponse.redirect(new URL("/login", request.url));
  }
}

export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico).*)"],
};
