"use client";

import { useEffect, useState } from "react";
import { apiGet } from "@/lib/api/client";
import { Task, PaginatedResponse } from "@/types";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import Link from "next/link";
import { format } from "date-fns";
import { id } from "date-fns/locale";

const statusMap: Record<
  number,
  { label: string; variant: "default" | "secondary" | "outline" }
> = {
  0: { label: "Backlog", variant: "outline" },
  1: { label: "In Progress", variant: "secondary" },
  2: { label: "Done", variant: "default" },
};

const priorityMap: Record<number, { label: string; color: string }> = {
  0: { label: "Low", color: "bg-gray-200" },
  1: { label: "Medium", color: "bg-yellow-200" },
  2: { label: "High", color: "bg-red-200" },
};

export default function TaskList() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchTasks();
  }, []);

  const fetchTasks = async () => {
    try {
      setLoading(true);
      const data = await apiGet<PaginatedResponse<Task>>("/tasks");
      setTasks(data.items);
      setError("");
    } catch (err: any) {
      setError(err.message || "Failed to load task");
      console.error("Error fetching tasks:", err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center p-8">
        <p className="text-red-500 mb-4">{error}</p>
        <Button onClick={fetchTasks}>Coba Lagi</Button>
      </div>
    );
  }

  if (tasks.length === 0) {
    return (
      <div className="text-center p-8">
        <p className="text-gray-500 mb-4">Belum ada task</p>
      </div>
    );
  }

  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
      {tasks.map((task) => (
        <Card key={task.id} className="hover:shadow-lg transition-shadow">
          <CardHeader>
            <CardTitle className="text-lg">{task.title}</CardTitle>
            <div className="flex gap-2 mt-2">
              <Badge variant={statusMap[task.status]?.variant || "outline"}>
                {statusMap[task.status]?.label || "Unknown"}
              </Badge>
              <Badge className={priorityMap[task.priority]?.color}>
                {priorityMap[task.priority]?.label || "Unknown"}
              </Badge>
            </div>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-gray-600 line-clamp-2">
              {task.description}
            </p>
            <div className="mt-4 text-xs text-gray-400">
              <p>
                Due:{" "}
                {task.dueDate
                  ? format(new Date(task.dueDate), "dd MMM yyyy", {
                      locale: id,
                    })
                  : "-"}
              </p>
              <p>Assigned to: {task.assignedUserName || "Unassigned"}</p>
            </div>
            <Button asChild className="mt-4 w-full">
              <Link href={`/tasks/${task.id}`}>Detail</Link>
            </Button>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
