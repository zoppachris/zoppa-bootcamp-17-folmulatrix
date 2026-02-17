import { Suspense } from "react";
import TaskList from "@/components/tasks/TaskList";
import { Button } from "@/components/ui/button";
import Link from "next/link";

export default function TasksPage() {
  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Daftar Task</h1>
        <Button asChild>
          <Link href="/tasks/new">+ Create Task</Link>
        </Button>
      </div>
      <Suspense fallback={<div className="text-center">Memuat...</div>}>
        <TaskList />
      </Suspense>
    </div>
  );
}
