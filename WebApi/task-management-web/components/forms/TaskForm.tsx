"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Label } from "@/components/ui/label";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { apiGet, apiPost, apiPut } from "@/lib/api/client";
import { Project, ProjectMember, Task } from "@/types";
import { ProjectCombobox } from "@/components/ui/ProjectCombobox";
import { UserCombobox } from "@/components/ui/UserCombobox";

interface TaskFormProps {
  taskId?: string;
  onSuccess?: () => void;
}

export default function TaskForm({ taskId, onSuccess }: TaskFormProps) {
  const router = useRouter();
  const isEdit = !!taskId;

  const [loading, setLoading] = useState(false);
  const [initialLoading, setInitialLoading] = useState(isEdit);
  const [error, setError] = useState("");
  const [selectedProjectId, setSelectedProjectId] = useState("");
  const [projectMembers, setProjectMembers] = useState<ProjectMember[]>([]);
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    status: "0",
    priority: "1",
    dueDate: "",
    assignedUserId: "none",
  });

  useEffect(() => {
    if (!isEdit) return;

    const fetchTask = async () => {
      try {
        const task = await apiGet<Task>(`/tasks/${taskId}`);
        setSelectedProjectId(task.projectId);
        setFormData({
          title: task.title || "",
          description: task.description || "",
          status: task.status?.toString() || "0",
          priority: task.priority?.toString() || "1",
          dueDate: task.dueDate ? task.dueDate.split("T")[0] : "",
          assignedUserId: task.assignedUserId || "none",
        });
      } catch (err: any) {
        setError(err instanceof Error ? err.message : "Failed to load task");
      } finally {
        setInitialLoading(false);
      }
    };
    fetchTask();
  }, [taskId, isEdit]);

  useEffect(() => {
    if (!selectedProjectId) {
      setProjectMembers([]);
      return;
    }

    const fetchProjectMembers = async () => {
      try {
        const project = await apiGet<Project>(`/projects/${selectedProjectId}`);
        setProjectMembers(project.members || []);
      } catch (err) {
        console.error("Failed load member to project:", err);
      }
    };
    fetchProjectMembers();
  }, [selectedProjectId]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSelectChange = (name: string, value: string) => {
    setFormData({ ...formData, [name]: value });
  };

  const handleProjectChange = (projectId: string, projectName: string) => {
    setSelectedProjectId(projectId);
    setFormData((prev) => ({ ...prev, assignedUserId: "none" }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      if (!selectedProjectId) throw new Error('Need select project');
      if (!formData.title) throw new Error('Need fill title');

      const payload = {
        title: formData.title,
        description: formData.description,
        status: parseInt(formData.status),
        priority: parseInt(formData.priority),
        dueDate: formData.dueDate ? new Date(formData.dueDate).toISOString() : null,
        projectId: selectedProjectId,
        assignedUserId: formData.assignedUserId === 'none' ? null : formData.assignedUserId,
      };

      if (isEdit) {
        await apiPut<Task>(`/tasks/${taskId}`, { ...payload, id: taskId });
      } else {
        await apiPost<Task>('/tasks', payload);
      }

      if (onSuccess) {
        onSuccess();
      } else {
        router.push('/tasks');
        router.refresh();
      }
    } catch (err: any) {
      setError(err instanceof Error ? err.message : 'Failed to save task');

      console.error('Task form error:', err);
    } finally {
      setLoading(false);
    }
  };

  if (initialLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  return (
    <Card className="w-full max-w-2xl mx-auto">
      <CardHeader>
        <CardTitle>{isEdit ? "Edit Task" : "Create New Task"}</CardTitle>
      </CardHeader>
      <form onSubmit={handleSubmit} className="space-y-4">
        <CardContent className="space-y-4">
          {/* Project */}
          <div className="space-y-2">
            <Label>
              Project <span className="text-red-500">*</span>
            </Label>
            <ProjectCombobox
              value={selectedProjectId}
              onChange={handleProjectChange}
              placeholder="Select project"
            />
          </div>

          {/* Title */}
          <div className="space-y-2">
            <Label htmlFor="title">
              Title <span className="text-red-500">*</span>
            </Label>
            <Input
              id="title"
              name="title"
              value={formData.title}
              onChange={handleChange}
              required
            />
          </div>

          {/* Description */}
          <div className="space-y-2">
            <Label htmlFor="description">Description</Label>
            <Textarea
              id="description"
              name="description"
              value={formData.description}
              onChange={handleChange}
              rows={3}
            />
          </div>

          {/* Status & Priority */}
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="status">Status</Label>
              <Select
                value={formData.status}
                onValueChange={(val) => handleSelectChange("status", val)}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="0">Backlog</SelectItem>
                  <SelectItem value="1">In Progress</SelectItem>
                  <SelectItem value="2">Done</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-2">
              <Label htmlFor="priority">Priority</Label>
              <Select
                value={formData.priority}
                onValueChange={(val) => handleSelectChange("priority", val)}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="0">Low</SelectItem>
                  <SelectItem value="1">Medium</SelectItem>
                  <SelectItem value="2">High</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          {/* Due Date & Assignee */}
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="dueDate">Due Date</Label>
              <Input
                id="dueDate"
                name="dueDate"
                type="date"
                value={formData.dueDate}
                onChange={handleChange}
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="assignedUserId">Assignee</Label>
              <UserCombobox
                members={projectMembers}
                value={formData.assignedUserId}
                onChange={(val) => handleSelectChange("assignedUserId", val)}
                placeholder="Select assignee"
              />
              {!selectedProjectId && (
                <p className="text-xs text-gray-500">Select project first</p>
              )}
            </div>
          </div>

          {error && <p className="text-sm text-red-500">{error}</p>}
        </CardContent>
        <CardFooter className="flex justify-between">
          <Button type="button" variant="outline" onClick={() => router.back()}>
            Cancel
          </Button>
          <Button type="submit" disabled={loading || !selectedProjectId}>
            {loading ? "Menyimpan..." : isEdit ? "Update" : "Create Task"}
          </Button>
        </CardFooter>
      </form>
    </Card>
  );
}
