"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { apiGet, apiDelete } from "@/lib/api/client";
import { Task } from "@/types";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { format } from "date-fns";
import { id as idLocale } from "date-fns/locale";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";

const statusMap: Record<
  number,
  { label: string; variant: "default" | "secondary" | "outline" }
> = {
  0: { label: "Backlog", variant: "outline" },
  1: { label: "In Progress", variant: "secondary" },
  2: { label: "Done", variant: "default" },
};

const priorityMap: Record<number, { label: string; className: string }> = {
  0: { label: "Low", className: "bg-gray-200" },
  1: { label: "Medium", className: "bg-yellow-200" },
  2: { label: "High", className: "bg-red-200" },
};

export default function TaskDetailPage() {
  const params = useParams();
  const router = useRouter();
  const id = params.id as string;

  const [task, setTask] = useState<Task | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [deleteLoading, setDeleteLoading] = useState(false);

  const fetchTask = async () => {
    try {
      setLoading(true);
      const data = await apiGet<Task>(`/tasks/${id}`);
      setTask(data);
    } catch (err: any) {
      setError(err.message || "Failed to load task");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTask();
  }, [id]);

  const handleDelete = async () => {
    try {
      setDeleteLoading(true);
      await apiDelete(`/tasks/${id}`);
      router.push("/tasks");
      router.refresh();
    } catch (err: any) {
      setError(err.message || "Failed to delete task");
      setDeleteLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  if (error || !task) {
    return (
      <div className="text-center p-8">
        <p className="text-red-500 mb-4">{error || "Task not found"}</p>
        <Button onClick={() => router.back()}>Back</Button>
      </div>
    );
  }

  return (
    <div className="p-6 max-w-4xl mx-auto">
      <div className="flex justify-between items-start mb-6">
        <h1 className="text-3xl font-bold">Detail Task</h1>
        <div className="space-x-2">
          <Button
            variant="outline"
            onClick={() => router.push(`/tasks/${id}/edit`)}
          >
            Edit
          </Button>
          <AlertDialog>
            <AlertDialogTrigger asChild>
              <Button variant="destructive">Delete</Button>
            </AlertDialogTrigger>
            <AlertDialogContent>
              <AlertDialogHeader>
                <AlertDialogTitle>Delete Task?</AlertDialogTitle>
                <AlertDialogDescription>
                  This action cannot be cancelled. The task will be deleted
                  automatically permanent.
                </AlertDialogDescription>
              </AlertDialogHeader>
              <AlertDialogFooter>
                <AlertDialogCancel>Cancel</AlertDialogCancel>
                <AlertDialogAction
                  onClick={handleDelete}
                  disabled={deleteLoading}
                >
                  {deleteLoading ? "Deleting..." : "Delete"}
                </AlertDialogAction>
              </AlertDialogFooter>
            </AlertDialogContent>
          </AlertDialog>
        </div>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>{task.title}</CardTitle>
          <div className="flex gap-2 mt-2">
            <Badge variant={statusMap[task.status]?.variant || "outline"}>
              {statusMap[task.status]?.label || "Unknown"}
            </Badge>
            <Badge
              className={priorityMap[task.priority]?.className || "bg-gray-200"}
            >
              {priorityMap[task.priority]?.label || "Unknown"}
            </Badge>
          </div>
        </CardHeader>
        <CardContent className="space-y-4">
          <div>
            <h3 className="font-semibold">Description</h3>
            <p className="text-gray-700 whitespace-pre-wrap">
              {task.description || "No Description"}
            </p>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <h3 className="font-semibold">Project ID</h3>
              <p>{task.projectId}</p>
            </div>
            <div>
              <h3 className="font-semibold">Assignee</h3>
              <p>{task.assignedUserName || "Unassigned"}</p>
            </div>
            <div>
              <h3 className="font-semibold">Due Date</h3>
              <p>
                {task.dueDate
                  ? format(new Date(task.dueDate), "dd MMMM yyyy", {
                      locale: idLocale,
                    })
                  : "-"}
              </p>
            </div>
            <div>
              <h3 className="font-semibold">Created At</h3>
              <p>
                {task.createdAt
                  ? format(new Date(task.createdAt), "dd MMMM yyyy HH:mm", {
                      locale: idLocale,
                    })
                  : "-"}
              </p>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
