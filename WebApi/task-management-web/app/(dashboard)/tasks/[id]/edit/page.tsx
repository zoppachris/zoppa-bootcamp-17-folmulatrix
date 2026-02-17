"use client";

import { useParams } from "next/navigation";
import TaskForm from "@/components/forms/TaskForm";

export default function EditTaskPage() {
  const params = useParams();
  const taskId = params.id as string;

  return (
    <div className="p-6">
      <TaskForm taskId={taskId} />
    </div>
  );
}
