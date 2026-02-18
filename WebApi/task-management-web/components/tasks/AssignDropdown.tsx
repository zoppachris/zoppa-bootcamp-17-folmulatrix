'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { UserPlus, Loader2 } from 'lucide-react';
import { ProjectMember } from '@/types';

interface AssignDropdownProps {
  taskId: string;
  members: ProjectMember[];
  currentAssigneeId?: string;
  onAssign?: () => void;
}

export default function AssignDropdown({ taskId, members, currentAssigneeId, onAssign }: AssignDropdownProps) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleAssign = async (userId: string | null) => {
    setLoading(true);
    setError('');
    try {
      if (userId) {
        const res = await fetch(`/api/tasks/${taskId}/assign/${userId}`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
        });

        if (!res.ok) {
          const data = await res.json();
          throw new Error(data.message || 'Failed to assign');
        }
      } else {
        const res = await fetch(`/api/tasks/${taskId}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ assignedUserId: null }),
        });

        if (!res.ok) {
          const data = await res.json();
          throw new Error(data.message || 'Failed to remove assign');
        }
      }

      if (onAssign) onAssign();
    } catch (err: any) {
      setError(err instanceof Error ? err.message : 'Failed to assign task');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <DropdownMenu>
        <DropdownMenuTrigger asChild>
          <Button variant="ghost" size="sm" disabled={loading}>
            {loading ? <Loader2 className="h-4 w-4 mr-2 animate-spin" /> : <UserPlus className="h-4 w-4 mr-2" />}
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
              {member.userName} {member.userId === currentAssigneeId && '(current)'}
            </DropdownMenuItem>
          ))}
        </DropdownMenuContent>
      </DropdownMenu>
      {error && <p className="text-xs text-red-500 mt-1">{error}</p>}
    </div>
  );
}