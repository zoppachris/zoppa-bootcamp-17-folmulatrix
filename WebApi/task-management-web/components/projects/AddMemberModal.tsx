'use client';

import { useState, useEffect } from 'react';
import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
} from '@/components/ui/dialog';
import { Input } from '@/components/ui/input';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Avatar, AvatarFallback } from '@/components/ui/avatar';
import { Check, Loader2, Search, UserPlus } from 'lucide-react';
import { apiGet } from '@/lib/api/client';
import { User, PaginatedResponse } from '@/types';
import { useDebounce } from '@/hooks/useDebounce';

interface AddMemberModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  projectId: string;
  existingMemberIds: string[];
  onMemberAdded: () => void;
}

export default function AddMemberModal({
  open,
  onOpenChange,
  projectId,
  existingMemberIds,
  onMemberAdded,
}: AddMemberModalProps) {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearch = useDebounce(searchTerm, 500);
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!open) return;

    const fetchUsers = async () => {
      setLoading(true);
      try {
        const params: Record<string, any> = {
          pageNumber: page,
          pageSize: 10,
        };
        if (debouncedSearch) params.searchTerm = debouncedSearch;

        const data = await apiGet<PaginatedResponse<User>>('/users', params);
        const filtered = data.items.filter((u) => !existingMemberIds.includes(u.id));
        setUsers(filtered);
        setTotalPages(data.totalPages);
      } catch (err: any) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, [debouncedSearch, page, open, existingMemberIds]);

  const handleAddMember = async () => {
    if (!selectedUserId) return;
    setSubmitting(true);
    setError('');
    try {
      const res = await fetch('/api/projects/assign-member', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          projectId,
          userId: selectedUserId,
        }),
      });

      const data = await res.json();

      if (!res.ok || !data.success) {
        throw new Error(data.message || 'Failed to add member');
      }

      onMemberAdded();
      onOpenChange(false);
    } catch (err: any) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  };

  const resetState = () => {
    setSearchTerm('');
    setPage(1);
    setSelectedUserId(null);
    setError('');
  };

  return (
    <Dialog
      open={open}
      onOpenChange={(open) => {
        if (!open) resetState();
        onOpenChange(open);
      }}
    >
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Add member to project</DialogTitle>
        </DialogHeader>
        <div className="space-y-4 py-4">
          <div className="relative">
            <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
            <Input
              placeholder="Search by fullname or username..."
              value={searchTerm}
              onChange={(e) => {
                setSearchTerm(e.target.value);
                setPage(1);
              }}
              className="pl-8"
            />
          </div>

          <ScrollArea className="h-72 border rounded-md p-2">
            {loading ? (
              <div className="flex justify-center items-center h-full">
                <Loader2 className="h-6 w-6 animate-spin" />
              </div>
            ) : users.length === 0 ? (
              <p className="text-center text-gray-500 py-4">
                {searchTerm ? 'No user matched' : 'No user available'}
              </p>
            ) : (
              <div className="space-y-2">
                {users.map((user) => (
                  <div
                    key={user.id}
                    className={`flex items-center justify-between p-2 rounded-md cursor-pointer hover:bg-gray-100 ${selectedUserId === user.id ? 'bg-gray-200' : ''
                      }`}
                    onClick={() => setSelectedUserId(user.id)}
                  >
                    <div className="flex items-center gap-3">
                      <Avatar className="h-8 w-8">
                        <AvatarFallback>
                          {user.fullName?.charAt(0) || user.userName?.charAt(0)}
                        </AvatarFallback>
                      </Avatar>
                      <div>
                        <p className="font-medium">{user.fullName}</p>
                        <p className="text-sm text-gray-500">{user.email}</p>
                      </div>
                    </div>
                    {selectedUserId === user.id && <Check className="h-4 w-4 text-green-600" />}
                  </div>
                ))}
              </div>
            )}
          </ScrollArea>

          {totalPages > 1 && (
            <div className="flex justify-center gap-2">
              <Button
                variant="outline"
                size="sm"
                disabled={page === 1}
                onClick={() => setPage((p) => Math.max(1, p - 1))}
              >
                Previous
              </Button>
              <span className="text-sm py-2">
                Page {page} of {totalPages}
              </span>
              <Button
                variant="outline"
                size="sm"
                disabled={page === totalPages}
                onClick={() => setPage((p) => p + 1)}
              >
                Next
              </Button>
            </div>
          )}

          {error && <p className="text-sm text-red-500">{error}</p>}
        </div>
        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)}>
            Batal
          </Button>
          <Button onClick={handleAddMember} disabled={!selectedUserId || submitting}>
            {submitting ? (
              <Loader2 className="h-4 w-4 animate-spin mr-2" />
            ) : (
              <UserPlus className="h-4 w-4 mr-2" />
            )}
            Add
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}