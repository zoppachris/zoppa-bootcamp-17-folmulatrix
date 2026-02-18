"use client";

import { useState } from "react";
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
import { apiPost, apiPut } from "@/lib/api/client";
import { Project } from "@/types";

interface ProjectFormProps {
  initialData?: Partial<Project>;
  projectId?: string;
  onSuccess?: () => void;
}

export default function ProjectForm({
  initialData = {},
  projectId,
  onSuccess,
}: ProjectFormProps) {
  const router = useRouter();
  const [formData, setFormData] = useState({
    name: initialData.name || "",
    description: initialData.description || "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const isEdit = !!projectId;

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      if (isEdit) {
        await apiPut<Project>(`/projects/${projectId}`, {
          id: projectId,
          ...formData,
        });
      } else {
        await apiPost<Project>("/projects", formData);
      }

      if (onSuccess) {
        onSuccess();
      } else {
        router.push("/projects");
        router.refresh();
      }
    } catch (err: any) {
      setError(err instanceof Error ? err.message : "Failed to save project");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card className="w-full max-w-2xl mx-auto">
      <CardHeader>
        <CardTitle>{isEdit ? "Edit Project" : "Create New Project"}</CardTitle>
      </CardHeader>
      <form onSubmit={handleSubmit} className="space-y-4">
        <CardContent className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">
              Project Name <span className="text-red-500">*</span>
            </Label>
            <Input
              id="name"
              name="name"
              value={formData.name}
              onChange={handleChange}
              placeholder="Ex: Development Machine"
              required
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="description">Description</Label>
            <Textarea
              id="description"
              name="description"
              value={formData.description}
              onChange={handleChange}
              placeholder="Describe this project..."
              rows={4}
            />
          </div>
          {error && <p className="text-sm text-red-500">{error}</p>}
        </CardContent>
        <CardFooter className="flex justify-between">
          <Button type="button" variant="outline" onClick={() => router.back()}>
            Cancel
          </Button>
          <Button type="submit" disabled={loading}>
            {loading ? "Menyimpan..." : isEdit ? "Update" : "Create Project"}
          </Button>
        </CardFooter>
      </form>
    </Card>
  );
}
