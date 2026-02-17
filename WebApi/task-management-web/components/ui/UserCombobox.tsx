"use client";

import { useState } from "react";
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
import { ProjectMember } from "@/types";

interface UserComboboxProps {
  members: ProjectMember[];
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

export function UserCombobox({
  members,
  value,
  onChange,
  placeholder = "Select user...",
}: UserComboboxProps) {
  const [open, setOpen] = useState(false);

  const selectedMember = members.find((m) => m.userId === value);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between"
        >
          {selectedMember ? selectedMember.userName : placeholder}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-full p-0">
        <Command>
          <CommandInput placeholder="Search user..." />
          <CommandList>
            <CommandEmpty>User not found.</CommandEmpty>
            <CommandGroup>
              <CommandItem
                key="none"
                value="none"
                onSelect={() => {
                  onChange("none");
                  setOpen(false);
                }}
              >
                <Check
                  className={cn(
                    "mr-2 h-4 w-4",
                    value === "none" ? "opacity-100" : "opacity-0",
                  )}
                />
                Unassigned
              </CommandItem>
              {members.map((member) => (
                <CommandItem
                  key={member.userId}
                  value={member.userId}
                  onSelect={() => {
                    onChange(member.userId);
                    setOpen(false);
                  }}
                >
                  <Check
                    className={cn(
                      "mr-2 h-4 w-4",
                      value === member.userId ? "opacity-100" : "opacity-0",
                    )}
                  />
                  {member.userName} ({member.email})
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
}
