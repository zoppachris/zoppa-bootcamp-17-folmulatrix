"use client";

import { useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Label } from "@/components/ui/label";
import { TaskFilter } from "@/types";
import { Search, X } from "lucide-react";

interface TaskFiltersProps {
  onFilterChange: (filters: TaskFilter) => void;
  initialFilters?: TaskFilter;
}

export default function TaskFilters({
  onFilterChange,
  initialFilters = {},
}: TaskFiltersProps) {
  const [filters, setFilters] = useState<TaskFilter>({
    searchTerm: initialFilters.searchTerm || "",
    status: initialFilters.status,
    priority: initialFilters.priority,
    projectId: initialFilters.projectId,
    assignedUserId: initialFilters.assignedUserId,
    sortBy: initialFilters.sortBy || "dueDate",
    sortOrder: initialFilters.sortOrder || "asc",
  });

  const handleChange = (key: keyof TaskFilter, value: any) => {
    setFilters((prev) => ({ ...prev, [key]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const cleanFilters = Object.fromEntries(
      Object.entries(filters).filter(
        ([_, v]) => v !== undefined && v !== null && v !== "",
      ),
    );
    onFilterChange(cleanFilters);
  };

  const handleReset = () => {
    setFilters({
      searchTerm: "",
      status: undefined,
      priority: undefined,
      projectId: undefined,
      assignedUserId: undefined,
      sortBy: "dueDate",
      sortOrder: "asc",
    });
    onFilterChange({});
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="space-y-4 bg-gray-50 p-4 rounded-lg mb-6"
    >
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <div className="space-y-2">
          <Label htmlFor="search">Search</Label>
          <div className="relative">
            <Search className="absolute left-2 top-2.5 h-4 w-4 text-gray-500" />
            <Input
              id="search"
              placeholder="Search tasks..."
              value={filters.searchTerm}
              onChange={(e) => handleChange("searchTerm", e.target.value)}
              className="pl-8"
            />
          </div>
        </div>

        <div className="space-y-2">
          <Label htmlFor="status">Status</Label>
          <Select
            value={filters.status?.toString() || ""}
            onValueChange={(val) =>
              handleChange("status", val ? parseInt(val) : undefined)
            }
          >
            <SelectTrigger>
              <SelectValue placeholder="All statuses" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="">All</SelectItem>
              <SelectItem value="0">Backlog</SelectItem>
              <SelectItem value="1">In Progress</SelectItem>
              <SelectItem value="2">Done</SelectItem>
            </SelectContent>
          </Select>
        </div>

        <div className="space-y-2">
          <Label htmlFor="priority">Priority</Label>
          <Select
            value={filters.priority?.toString() || ""}
            onValueChange={(val) =>
              handleChange("priority", val ? parseInt(val) : undefined)
            }
          >
            <SelectTrigger>
              <SelectValue placeholder="All priorities" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="">All</SelectItem>
              <SelectItem value="0">Low</SelectItem>
              <SelectItem value="1">Medium</SelectItem>
              <SelectItem value="2">High</SelectItem>
            </SelectContent>
          </Select>
        </div>

        <div className="space-y-2">
          <Label htmlFor="sortBy">Sort By</Label>
          <Select
            value={filters.sortBy || "dueDate"}
            onValueChange={(val) => handleChange("sortBy", val)}
          >
            <SelectTrigger>
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="dueDate">Due Date</SelectItem>
              <SelectItem value="title">Title</SelectItem>
              <SelectItem value="priority">Priority</SelectItem>
              <SelectItem value="status">Status</SelectItem>
              <SelectItem value="createdAt">Created At</SelectItem>
            </SelectContent>
          </Select>
        </div>

        <div className="space-y-2">
          <Label htmlFor="sortOrder">Order</Label>
          <Select
            value={filters.sortOrder || "asc"}
            onValueChange={(val) =>
              handleChange("sortOrder", val as "asc" | "desc")
            }
          >
            <SelectTrigger>
              <SelectValue />
            </SelectTrigger>
            <SelectContent>
              <SelectItem value="asc">Ascending</SelectItem>
              <SelectItem value="desc">Descending</SelectItem>
            </SelectContent>
          </Select>
        </div>
      </div>

      <div className="flex justify-end gap-2">
        <Button type="button" variant="outline" onClick={handleReset}>
          <X className="mr-2 h-4 w-4" /> Reset
        </Button>
        <Button type="submit">
          <Search className="mr-2 h-4 w-4" /> Apply Filters
        </Button>
      </div>
    </form>
  );
}
