"use client";

import React from "react";
import SidebarModuleList from "@/components/studio/navigation/SidebarModuleList";

const StudioSidebar: React.FC = () => {
  return (
    <div className="w-64 border-r border-[color:var(--studio-color-border-subtle)] bg-[color:var(--studio-color-bg-elevated)] flex flex-col overflow-y-auto">
      <SidebarModuleList />
    </div>
  );
};

export default StudioSidebar;


