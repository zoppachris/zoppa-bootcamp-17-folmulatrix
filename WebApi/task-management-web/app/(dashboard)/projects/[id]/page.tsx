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
import { ArrowLeft, Loader2, Pencil, Trash2, UserPlus, X } from "lucide-react";
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
import { toast } from "sonner";

export default function ProjectDetailPage() {
  const params = useParams();
  const router = useRouter();
  const id = params.id as string;

  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [deleteLoading, setDeleteLoading] = useState(false);
  const [addMemberModalOpen, setAddMemberModalOpen] = useState(false);
  const [removeMemberId, setRemoveMemberId] = useState<string | null>(null);

  const fetchProject = async () => {
    try {
      setLoading(true);
      const data = await apiGet<Project>(`/projects/${id}`);
      setProject(data);
    } catch (err: any) {
      setError(err.message || 'Failed to load project');
      toast.error("Failed to load project");
    } finally {
      setLoading(false);
    }


  }
  useEffect(() => {
    fetchProject();
  }, [id]);

  const handleDelete = async () => {
    if (!project) return;

    setDeleteLoading(true);
    try {
      await apiDelete(`/projects/${project.id}`);

      toast.success('Project deleted');

      router.push('/projects');
      router.refresh();
    } catch (err: any) {
      toast.error("Failed to delete project");
      setDeleteLoading(false);
      setDeleteDialogOpen(false);
    }
  };

  const handleRemoveMember = async () => {
    if (!removeMemberId || !project) return;

    try {
      await apiDelete(`/projects/${project.id}/members/${removeMemberId}`);

      setProject({
        ...project,
        members: project.members.filter((m) => m.userId !== removeMemberId),
      });

      toast.success("Success to delete member from project");

      setRemoveMemberId(null);
    } catch (err: any) {
      toast.error("Failed to delete member");
    }
  };

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
        <p className="text-red-500 mb-4">{error || 'Project not found'}</p>
        <Button onClick={() => router.back()}>
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back
        </Button>
      </div>
    );
  }

  return (
    <div className="p-6 max-w-6xl mx-auto">
      {/* Header with back button */}
      <div className="mb-4">
        <Button variant="ghost" size="sm" onClick={() => router.back()}>
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back
        </Button>
      </div>

      {/* Project Info */}
      <div className="flex justify-between items-start mb-6">
        <div>
          <h1 className="text-3xl font-bold">{project.name}</h1>
          <p className="text-gray-600 mt-1">{project.description}</p>
          <p className="text-sm text-gray-400 mt-2">
            Created at : {format(new Date(project.createdAt), 'dd MMMM yyyy', { locale: idLocal })} by {project.ownerName}
          </p>
        </div>
        <div className="flex gap-2">
          <Button variant="outline" onClick={() => router.push(`/projects/${id}/edit`)}>
            <Pencil className="h-4 w-4 mr-2" /> Edit
          </Button>
          <Button variant="destructive" onClick={() => setDeleteDialogOpen(true)}>
            <Trash2 className="h-4 w-4 mr-2" /> Delete
          </Button>
        </div>
      </div>

      {/* Members Section */}
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
                      className="h-5 w-5 p-0 ml-1 hover:bg-red-100 hover:text-red-600"
                      onClick={() => setRemoveMemberId(member.userId)}
                    >
                      <X className="h-3 w-3" />
                    </Button>
                  )}
                </Badge>
              ))}
            </div>
          ) : (
            <p className="text-gray-500">No members yet.</p>
          )}
        </CardContent>
      </Card>

      {/* Tasks Section */}
      <ProjectTaskList projectId={project.id} members={project.members || []} />

      {/* Add Member Modal */}
      <AddMemberModal
        open={addMemberModalOpen}
        onOpenChange={setAddMemberModalOpen}
        projectId={project.id}
        existingMemberIds={project.members.map((m) => m.userId)}
        onMemberAdded={() => {
          fetchProject();
          toast.success("Added member success");
        }}
      />

      {/* Delete Project Confirmation Dialog */}
      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete Project?</AlertDialogTitle>
            <AlertDialogDescription>
              <p className="mb-2">Are you sure you want to delete the project? <strong>{project.name}</strong>?</p>
              <p className="text-red-600 font-semibold">
                Warning: All tasks associated with this project will also be permanently deleted.
              </p>
              <p className="mt-2 text-sm text-gray-500">
                This action cannot be undone.
              </p>
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel disabled={deleteLoading}>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={handleDelete}
              disabled={deleteLoading}
              className="bg-red-600 hover:bg-red-700 focus:ring-red-600"
            >
              {deleteLoading ? (
                <>
                  <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                  Deleting...
                </>
              ) : (
                'Delete'
              )}
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>

      {/* Remove Member Confirmation Dialog */}
      <AlertDialog open={!!removeMemberId} onOpenChange={(open) => !open && setRemoveMemberId(null)}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete member?</AlertDialogTitle>
            <AlertDialogDescription>
              This member will be removed from the project. They will no longer be able to view or manage tasks in this project.
              This action can be undone by adding them back.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={handleRemoveMember}>
              Delete
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  );
}