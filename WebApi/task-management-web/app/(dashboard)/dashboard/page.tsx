"use client";

import { useAuth } from "@/lib/auth/AuthContext";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function DashboardPage() {
  const { user } = useAuth();

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold mb-6">Dashboard</h1>
      <Card>
        <CardHeader>
          <CardTitle>Welcome, {user?.fullName}!</CardTitle>
        </CardHeader>
        <CardContent>
          <p>Email: {user?.email}</p>
          <p>Username: {user?.userName}</p>
        </CardContent>
      </Card>
    </div>
  );
}
