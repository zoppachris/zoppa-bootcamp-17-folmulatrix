"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { apiGet, apiDelete } from "@/lib/api/client";
import { Project } from "@/types";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { format } from "date-fns";
import { id as idLocal } from "date-fns/locale";
import { Pencil, Trash2, UserPlus, X } from "lucide-react";
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
import ProjectTaskList from "@/components/projects/ProjectTaskList";
import AddMemberModal from "@/components/projects/AddMemberModal";

export default function ProjectDetailPage() {
  const params = useParams();
  const router = useRouter();
  const id = params.id as string;

  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [addMemberModalOpen, setAddMemberModalOpen] = useState(false);
  const [removeMemberId, setRemoveMemberId] = useState<string | null>(null);

  const fetchProject = async () => {
    try {
      setLoading(true);
      const data = await apiGet<Project>(`/projects/${id}`);
      setProject(data);
    } catch (err: any) {
      setError(err.message || "Failed to load project");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProject();
  }, [id]);

  const handleRemoveMember = async () => {
    if (!removeMemberId || !project) return;
    try {
      await apiDelete(`/projects/${project.id}/members/${removeMemberId}`);
      setProject({
        ...project,
        members: project.members.filter((m) => m.userId !== removeMemberId),
      });
      setRemoveMemberId(null);
    } catch (err: any) {
      setError(err.message || "Failed to delete member");
    }
  };

  if (loading)
    return (
      <div className="flex justify-center items-center h-64">Loading...</div>
    );
  if (error || !project) {
    return (
      <div className="p-6 text-center">
        <p className="text-red-500 mb-4">{error || "Project not found"}</p>
        <Button onClick={() => router.back()}>Back</Button>
      </div>
    );
  }

  return (
    <div className="p-6 max-w-6xl mx-auto">
      <div className="flex justify-between items-start mb-6">
        <div>
          <h1 className="text-3xl font-bold">{project.name}</h1>
          <p className="text-gray-600 mt-1">{project.description}</p>
          <p className="text-sm text-gray-400 mt-2">
            Created At :{" "}
            {format(new Date(project.createdAt), "dd MMMM yyyy", {
              locale: idLocal,
            })}{" "}
            By {project.ownerName}
          </p>
        </div>
        <div className="flex gap-2">
          <Button
            variant="outline"
            onClick={() => router.push(`/projects/${id}/edit`)}
          >
            <Pencil className="h-4 w-4 mr-2" /> Edit
          </Button>
          <Button
            variant="destructive"
            onClick={() => setDeleteDialogOpen(true)}
          >
            <Trash2 className="h-4 w-4 mr-2" /> Delete
          </Button>
        </div>
      </div>

      <Card className="mb-6">
        <CardHeader className="flex flex-row items-center justify-between">
          <CardTitle className="text-lg">Project Members</CardTitle>
          <Button size="sm" onClick={() => setAddMemberModalOpen(true)}>
            <UserPlus className="h-4 w-4 mr-2" /> Add member
          </Button>
        </CardHeader>
        <CardContent>
          {project.members && project.members.length > 0 ? (
            <div className="flex flex-wrap gap-2">
              {project.members.map((member) => (
                <Badge key={member.userId} variant="secondary" className="pr-1">
                  {member.userName} ({member.email})
                  {member.userId !== project.ownerId && (
                    <Button
                      variant="ghost"
                      size="sm"
                      className="h-5 w-5 p-0 ml-1"
                      onClick={() => setRemoveMemberId(member.userId)}
                    >
                      <X className="h-3 w-3" />
                    </Button>
                  )}
                </Badge>
              ))}
            </div>
          ) : (
            <p className="text-gray-500">Belum ada anggota.</p>
          )}
        </CardContent>
      </Card>

      <ProjectTaskList projectId={project.id} members={project.members || []} />

      <AddMemberModal
        open={addMemberModalOpen}
        onOpenChange={setAddMemberModalOpen}
        projectId={project.id}
        existingMemberIds={project.members.map((m) => m.userId)}
        onMemberAdded={fetchProject}
      />

      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Project?</AlertDialogTitle>
            <AlertDialogDescription>
              The project will be permanently deleted along with all tasks in it
              inside. This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={() => router.push("/projects")}>
              Confirm
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>

      <AlertDialog
        open={!!removeMemberId}
        onOpenChange={(open) => !open && setRemoveMemberId(null)}
      >
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Member?</AlertDialogTitle>
            <AlertDialogDescription>
              This member will be removed from the project. This action can
              canceled by adding it back.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={handleRemoveMember}>
              Confirm
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}
