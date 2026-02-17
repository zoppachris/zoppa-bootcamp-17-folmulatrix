"use client";

import { useState, useEffect } from "react";
import { Check, ChevronsUpDown } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "@/components/ui/command";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { apiGet } from "@/lib/api/client";
import { Project, PaginatedResponse } from "@/types";
import { useDebounce } from "@/hooks/useDebounce";

interface ProjectComboboxProps {
  value: string;
  onChange: (projectId: string, projectName: string) => void;
  placeholder?: string;
}

export function ProjectCombobox({
  value,
  onChange,
  placeholder = "Select project...",
}: ProjectComboboxProps) {
  const [open, setOpen] = useState(false);
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search, 500);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const selectedProject = projects.find((p) => p.id === value);

  useEffect(() => {
    const fetchProjects = async () => {
      setLoading(true);
      try {
        const params: Record<string, any> = {
          pageNumber: page,
          pageSize: 10,
        };
        if (debouncedSearch) params.searchTerm = debouncedSearch;

        const data = await apiGet<PaginatedResponse<Project>>(
          "/projects",
          params,
        );
        setProjects(data.items);
        setTotalPages(data.totalPages);
      } catch (error) {
        console.error("Gagal memuat projects:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchProjects();
  }, [debouncedSearch, page]);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between"
        >
          {selectedProject ? selectedProject.name : placeholder}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-full p-0">
        <Command>
          <CommandInput
            placeholder="Cari project..."
            onValueChange={setSearch}
          />
          <CommandList>
            <CommandEmpty>
              {loading ? "Memuat..." : "Project tidak ditemukan."}
            </CommandEmpty>
            <CommandGroup>
              {projects.map((project) => (
                <CommandItem
                  key={project.id}
                  value={project.id}
                  onSelect={() => {
                    onChange(project.id, project.name);
                    setOpen(false);
                  }}
                >
                  <Check
                    className={cn(
                      "mr-2 h-4 w-4",
                      value === project.id ? "opacity-100" : "opacity-0",
                    )}
                  />
                  {project.name}
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
          {totalPages > 1 && (
            <div className="flex justify-center gap-2 p-2 border-t">
              <Button
                variant="outline"
                size="sm"
                disabled={page === 1}
                onClick={() => setPage((p) => Math.max(1, p - 1))}
              >
                Previous
              </Button>
              <span className="text-sm py-2">
                {page}/{totalPages}
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
        </Command>
      </PopoverContent>
    </Popover>
  );
}
