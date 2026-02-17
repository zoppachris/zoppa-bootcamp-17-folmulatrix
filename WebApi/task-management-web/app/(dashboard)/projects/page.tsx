import ProjectList from "@/components/projects/ProjectList";
import { Button } from "@/components/ui/button";
import Link from "next/link";

export default function ProjectsPage() {
  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Projects</h1>
        <Button asChild>
          <Link href="/projects/new">Create Project</Link>
        </Button>
      </div>
      <ProjectList />
    </div>
  );
}
