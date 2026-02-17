"use client";

import { useEffect, useState } from "react";
import { apiGet } from "@/lib/api/client";
import { User } from "@/types";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function AdminPage() {
  const [users, setUsers] = useState<User[]>([]);

  useEffect(() => {
    apiGet<{ items: User[] }>("/users").then((data) => setUsers(data.items));
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-3xl font-bold mb-6">Admin Panel</h1>
      <Card>
        <CardHeader>
          <CardTitle>All Users</CardTitle>
        </CardHeader>
        <CardContent>
          <ul className="space-y-2">
            {users.map((user) => (
              <li key={user.id} className="border-b pb-2">
                {user.fullName} - {user.userName}
              </li>
            ))}
          </ul>
        </CardContent>
      </Card>
    </div>
  );
}
