"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { UserPlus } from "lucide-react";
import { ProjectMember } from "@/types";
import { apiPost, apiPut } from "@/lib/api/client";

interface AssignDropdownProps {
  taskId: string;
  members: ProjectMember[];
  currentAssigneeId?: string;
  onAssign?: () => void;
}

export default function AssignDropdown({
  taskId,
  members,
  currentAssigneeId,
  onAssign,
}: AssignDropdownProps) {
  const [loading, setLoading] = useState(false);

  const handleAssign = async (userId: string | null) => {
    setLoading(true);
    try {
      if (userId) {
        await apiPost(`/tasks/${taskId}/assign/${userId}`, {});
      } else {
        // Unassign dengan mengirim null via update task
        await apiPut(`/tasks/${taskId}`, { assignedUserId: null });
      }
      if (onAssign) onAssign();
    } catch (error) {
      console.error("Failed to assign task:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="sm" disabled={loading}>
          <UserPlus className="h-4 w-4 mr-2" />
          Assign
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end">
        <DropdownMenuLabel>Assign to member</DropdownMenuLabel>
        <DropdownMenuSeparator />
        <DropdownMenuItem onClick={() => handleAssign(null)}>
          Unassigned
        </DropdownMenuItem>
        {members.map((member) => (
          <DropdownMenuItem
            key={member.userId}
            onClick={() => handleAssign(member.userId)}
            disabled={member.userId === currentAssigneeId}
          >
            {member.userName}{" "}
            {member.userId === currentAssigneeId && "(current)"}
          </DropdownMenuItem>
        ))}
      </DropdownMenuContent>
    </DropdownMenu>
  );
}
