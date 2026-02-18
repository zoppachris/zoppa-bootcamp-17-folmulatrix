"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { apiGet } from "@/lib/api/client";
import { Project } from "@/types";
import ProjectForm from "@/components/forms/ProjectForm";
import { Loader2 } from "lucide-react";

export default function EditProjectPage() {
  const params = useParams();
  const router = useRouter();
  const id = params.id as string;

  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchProject = async () => {
      try {
        const data = await apiGet<Project>(`/projects/${id}`);
        setProject(data);
      } catch (err: any) {
        setError(err instanceof Error ? err.message : "Failed to load project");
      } finally {
        setLoading(false);
      }
    };
    fetchProject();
  }, [id]);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Loader2 className="h-8 w-8 animate-spin" />
      </div>
    );
  }

  if (error || !project) {
    return (
      <div className="p-6 text-center">
        <p className="text-red-500 mb-4">
          {error || "Project tidak ditemukan"}
        </p>
        <button
          className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
          onClick={() => router.back()}
        >
          Back
        </button>
      </div>
    );
  }

  return (
    <div className="p-6">
      <ProjectForm
        initialData={{ name: project.name, description: project.description }}
        projectId={id}
        onSuccess={() => router.push(`/projects/${id}`)}
      />
    </div>
  );
}
