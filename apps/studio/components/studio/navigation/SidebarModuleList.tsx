"use client";

import React, { useState } from "react";

type ModuleSection = {
  id: string;
  title: string;
  items: string[];
  initiallyExpanded?: boolean;
};

const moduleSections: ModuleSection[] = [
  { id: "animations", title: "Animations", items: ["Browse Animations", "Favorites", "Recent"], initiallyExpanded: true },
  { id: "components", title: "Components", items: ["Library", "Patterns", "Saved Sets"], initiallyExpanded: true },
  { id: "projects", title: "My Projects", items: ["Overview", "Active", "Archived"] },
  { id: "settings", title: "Settings", items: ["Profile", "Workspace"], initiallyExpanded: true },
];

const SidebarModuleList: React.FC = () => {
  const [expandedSections, setExpandedSections] = useState<Record<string, boolean>>(
    () =>
      moduleSections.reduce<Record<string, boolean>>((acc, section) => {
        acc[section.id] = section.initiallyExpanded ?? true;
        return acc;
      }, {}),
  );
  const [selectedItem, setSelectedItem] = useState<string | null>(null);

  const toggleSection = (sectionId: string) => {
    setExpandedSections((prev) => ({
      ...prev,
      [sectionId]: !prev[sectionId],
    }));
  };

  const handleSelect = (sectionId: string, itemLabel: string) => {
    setSelectedItem(`${sectionId}:${itemLabel}`);
  };

  return (
    <div className="p-3 space-y-2">
      {moduleSections.map((section, index) => {
        const isExpanded = expandedSections[section.id];

        return (
          <div key={section.id} className="space-y-1">
            <button
              type="button"
              onClick={() => toggleSection(section.id)}
              className="w-full text-left text-sm font-medium uppercase tracking-wide text-[color:var(--studio-color-text-muted)] py-2 px-3 flex items-center justify-between cursor-pointer"
            >
              <span>{section.title}</span>
              <span aria-hidden className="text-xs">
                {isExpanded ? "▾" : "▸"}
              </span>
            </button>

            {isExpanded && (
              <div className="pl-4 flex flex-col gap-1">
                {section.items.map((item) => {
                  const itemKey = `${section.id}:${item}`;
                  const isSelected = selectedItem === itemKey;

                  return (
                    <div
                      key={itemKey}
                      role="button"
                      tabIndex={0}
                      onClick={() => handleSelect(section.id, item)}
                      onKeyDown={(event) => {
                        if (event.key === "Enter" || event.key === " ") {
                          event.preventDefault();
                          handleSelect(section.id, item);
                        }
                      }}
                      className={`py-1.5 px-2 rounded-md cursor-pointer hover:bg-[color:var(--studio-color-bg-hover)] ${
                        isSelected
                          ? "bg-[color:var(--studio-color-bg-active)] text-[color:var(--studio-color-text-accent)]"
                          : "text-[color:var(--studio-color-text-muted)]"
                      }`}
                    >
                      {item}
                    </div>
                  );
                })}
              </div>
            )}

            {index < moduleSections.length - 1 && (
              <div className="my-2 border-b border-[color:var(--studio-color-border-subtle)]" />
            )}
          </div>
        );
      })}
    </div>
  );
};

export default SidebarModuleList;

