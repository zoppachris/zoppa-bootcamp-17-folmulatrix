import { NextResponse } from "next/server";

export function methodNotAllowed(allowedMethods: string[]) {
  return NextResponse.json(
    {
      success: false,
      message: `Method not allowed. Allowed methods: ${allowedMethods.join(", ")}`,
    },
    {
      status: 405,
      headers: {
        Allow: allowedMethods.join(", "),
      },
    },
  );
}
