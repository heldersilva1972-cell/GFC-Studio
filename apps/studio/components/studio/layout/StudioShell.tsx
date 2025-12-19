"use client";

import React from "react";
import DrawerHost from "@/components/studio/drawers/DrawerHost";
import StudioSidebar from "@/components/studio/navigation/StudioSidebar";
import StudioTopBar from "@/components/studio/layout/StudioTopBar";
import useStudioLayoutState from "@/lib/layout/layoutStateStore";
import { defaultStudioLayoutConfig, getRegionStyles } from "@/lib/layout/studioLayoutEngine";
import { getZIndex } from "@/lib/layout/zIndexMap";

type StudioShellProps = {
  children?: React.ReactNode;
};

const StudioShell: React.FC<StudioShellProps> = ({ children }) => {
  const { isSidebarCollapsed } = useStudioLayoutState();
  const regionStyles = getRegionStyles(defaultStudioLayoutConfig);

  const sidebarWidth = isSidebarCollapsed ? 64 : defaultStudioLayoutConfig.sidebarWidth;

  const stageContent =
    children ??
    (
      <div className="h-full w-full flex items-center justify-center text-sm text-[color:var(--studio-color-text-muted)]">
        <div className="rounded-xl border border-[color:var(--studio-color-border-subtle)] px-4 py-3 bg-[color:var(--studio-color-bg-elevated)]">
          Studio stage placeholder — animations and editors will appear here.
        </div>
      </div>
    );

  return (
    <div className="min-h-screen bg-[color:var(--studio-color-bg-canvas)] text-[color:var(--studio-color-text-strong)] flex flex-col">
      <StudioTopBar />
      <div className="flex flex-1 overflow-hidden">
        <div
          className="flex-shrink-0 h-full"
          style={{
            ...regionStyles.sidebarStyle,
            width: `${sidebarWidth}px`,
            zIndex: getZIndex("sidebar"),
          }}
        >
          <StudioSidebar />
        </div>

        <main
          className="flex-1 overflow-hidden relative"
          style={{
            ...regionStyles.stageStyle,
            zIndex: getZIndex("stage"),
          }}
        >
          {stageContent}
        </main>

        <DrawerHost />
      </div>
    </div>
  );
};

export default StudioShell;


