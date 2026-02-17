import { Task } from "@/types";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import Link from "next/link";
import { Calendar, User } from "lucide-react";
import { format } from "date-fns";

interface TaskCardProps {
  task: Task;
}

const statusMap = ["Backlog", "In Progress", "Done"];
const priorityMap = ["Low", "Medium", "High"];
const priorityColor = ["bg-gray-500", "bg-yellow-500", "bg-red-500"];

export default function TaskCard({ task }: TaskCardProps) {
  return (
    <Card>
      <CardHeader className="pb-2">
        <div className="flex justify-between items-start">
          <CardTitle className="text-lg">{task.title}</CardTitle>
          <Badge className={priorityColor[task.priority]}>
            {priorityMap[task.priority]}
          </Badge>
        </div>
      </CardHeader>
      <CardContent className="pb-2">
        <p className="text-sm text-gray-600 line-clamp-2">{task.description}</p>
        <div className="flex items-center gap-4 mt-4 text-sm text-gray-500">
          <div className="flex items-center gap-1">
            <Calendar className="h-4 w-4" />
            {task.dueDate
              ? format(new Date(task.dueDate), "dd MMM yyyy")
              : "No due date"}
          </div>
          <div className="flex items-center gap-1">
            <User className="h-4 w-4" />
            {task.assignedUserName || "Unassigned"}
          </div>
        </div>
        <Badge variant="outline" className="mt-2">
          {statusMap[task.status]}
        </Badge>
      </CardContent>
      <CardFooter>
        <Button asChild variant="outline" size="sm" className="w-full">
          <Link href={`/tasks/${task.id}`}>View Details</Link>
        </Button>
      </CardFooter>
    </Card>
  );
}
