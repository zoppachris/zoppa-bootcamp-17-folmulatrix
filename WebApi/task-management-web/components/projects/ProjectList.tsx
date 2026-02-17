"use client";

import { useEffect, useState } from "react";
import { apiGet } from "@/lib/api/client";
import { Project, PaginatedResponse } from "@/types";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import Link from "next/link";

export default function ProjectList() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    apiGet<PaginatedResponse<Project>>("/projects")
      .then((data) => setProjects(data.items))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <div>Loading...</div>;

  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
      {projects.map((project) => (
        <Card key={project.id}>
          <CardHeader>
            <CardTitle>{project.name}</CardTitle>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-gray-600">{project.description}</p>
            <p className="text-xs text-gray-400 mt-2">
              Owner: {project.ownerName} | Members: {project.members.length}
            </p>
            <Button asChild className="mt-4">
              <Link href={`/projects/${project.id}`}>View Details</Link>
            </Button>
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
