"use client";

import { useState, useEffect } from "react";
import { apiGet, apiDelete } from "@/lib/api/client";
import { Task, PaginatedResponse, ProjectMember } from "@/types";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { format } from "date-fns";
import { id } from "date-fns/locale";
import { Pencil, Trash2, Plus } from "lucide-react";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import AssignDropdown from "@/components/tasks/AssignDropdown";
import TaskModal from "../tasks/TaskModal";

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

interface ProjectTaskListProps {
  projectId: string;
  members: ProjectMember[];
}

export default function ProjectTaskList({
  projectId,
  members,
}: ProjectTaskListProps) {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [editingTask, setEditingTask] = useState<Task | null>(null);
  const [deleteTaskId, setDeleteTaskId] = useState<string | null>(null);

  const fetchTasks = async () => {
    try {
      setLoading(true);
      const data = await apiGet<PaginatedResponse<Task>>("/tasks", {
        projectId,
        pageSize: 100,
      });
      setTasks(data.items || []);
    } catch (err: any) {
      setError(err.message || "Failed to load task");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, [projectId]);

  const handleDelete = async () => {
    if (!deleteTaskId) return;
    try {
      await apiDelete(`/tasks/${deleteTaskId}`);
      setTasks(tasks.filter((t) => t.id !== deleteTaskId));
      setDeleteTaskId(null);
    } catch (err: any) {
      setError(err.message || "Failed to delete task");
    }
  };

  const handleEdit = (task: Task) => {
    setEditingTask(task);
    setModalOpen(true);
  };

  const handleModalSuccess = () => {
    fetchTasks();
    setEditingTask(null);
  };

  if (loading) return <div className="text-center py-8">Memuat tasks...</div>;

  return (
    <div className="mt-8">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-2xl font-semibold">Tasks</h2>
        <Button onClick={() => setModalOpen(true)} size="sm">
          <Plus className="h-4 w-4 mr-2" /> Add Task
        </Button>
      </div>

      {error && <p className="text-red-500 mb-4">{error}</p>}

      {tasks.length === 0 ? (
        <p className="text-gray-500 text-center py-8">
          There is no task yet.
        </p>
      ) : (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {tasks.map((task) => (
            <Card key={task.id} className="relative">
              <CardHeader className="pb-2">
                <div className="flex justify-between items-start">
                  <CardTitle className="text-lg line-clamp-1">
                    {task.title}
                  </CardTitle>
                  <div className="flex gap-1">
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => handleEdit(task)}
                    >
                      <Pencil className="h-4 w-4" />
                    </Button>
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => setDeleteTaskId(task.id)}
                    >
                      <Trash2 className="h-4 w-4" />
                    </Button>
                  </div>
                </div>
                <div className="flex gap-2 flex-wrap">
                  <Badge variant={statusMap[task.status]?.variant || "outline"}>
                    {statusMap[task.status]?.label}
                  </Badge>
                  <Badge className={priorityMap[task.priority]?.className}>
                    {priorityMap[task.priority]?.label}
                  </Badge>
                </div>
              </CardHeader>
              <CardContent>
                <p className="text-sm text-gray-600 line-clamp-2 mb-2">
                  {task.description || "-"}
                </p>
                <div className="text-xs text-gray-400 space-y-1">
                  <p>
                    Due:{" "}
                    {task.dueDate
                      ? format(new Date(task.dueDate), "dd MMM yyyy", {
                          locale: id,
                        })
                      : "-"}
                  </p>
                  <p>Assignee: {task.assignedUserName || "Unassigned"}</p>
                </div>
                <div className="mt-4 flex justify-between items-center">
                  <AssignDropdown
                    taskId={task.id}
                    members={members}
                    currentAssigneeId={task.assignedUserId}
                    onAssign={fetchTasks}
                  />
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}

      <TaskModal
        open={modalOpen}
        onOpenChange={(open) => {
          setModalOpen(open);
          if (!open) setEditingTask(null);
        }}
        projectId={projectId}
        members={members}
        task={editingTask}
        onSuccess={handleModalSuccess}
      />

      <AlertDialog
        open={!!deleteTaskId}
        onOpenChange={(open) => !open && setDeleteTaskId(null)}
      >
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Task?</AlertDialogTitle>
            <AlertDialogDescription>
              The task will be deleted permanently. This action cannot be done
              cancelled.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={handleDelete}>
              Confirm
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}
