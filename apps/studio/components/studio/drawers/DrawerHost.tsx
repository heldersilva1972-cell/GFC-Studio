"use client";

import React, { useEffect, useMemo, useState } from "react";
import useStudioLayoutState from "@/lib/layout/layoutStateStore";
import { defaultStudioLayoutConfig } from "@/lib/layout/studioLayoutEngine";
import { getZIndex } from "@/lib/layout/zIndexMap";
import { isNarrow } from "@/lib/layout/breakpoints";
import DrawerPanel from "./DrawerPanel";

const useOverlayMode = () => {
  const [isOverlay, setIsOverlay] = useState(() =>
    typeof window !== "undefined" ? isNarrow(window.innerWidth) : false
  );

  useEffect(() => {
    const handleResize = () => {
      if (typeof window === "undefined") return;
      setIsOverlay(isNarrow(window.innerWidth));
    };

    handleResize();
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  return isOverlay;
};

const DrawerHost: React.FC = () => {
  const { openDrawerId, closeDrawer } = useStudioLayoutState();
  const isOverlay = useOverlayMode();
  const isOpen = !!openDrawerId;

  const drawerTitle = useMemo(() => {
    switch (openDrawerId) {
      case "inspector":
        return "Inspector";
      case "browser":
        return "Component Browser";
      case "settings":
        return "Studio Settings";
      default:
        return "Drawer";
    }
  }, [openDrawerId]);

  const renderDrawerContent = () => {
    switch (openDrawerId) {
      case "inspector":
        return <p>Inspector panel coming soon.</p>;
      case "browser":
        return <p>Component browser coming soon.</p>;
      case "settings":
        return <p>Studio settings coming soon.</p>;
      default:
        return null;
    }
  };

  if (!isOpen) return null;

  const widthPx = `${defaultStudioLayoutConfig.drawerWidth}px`;

  const overlayClasses = isOverlay ? "fixed inset-y-0 right-0 shadow-2xl" : "relative";

  return (
    <aside
      className={`drawer-host ${overlayClasses} h-full transform bg-[color:var(--studio-color-bg-elevated)] transition-transform duration-200 ease-out`}
      style={{
        width: widthPx,
        zIndex: getZIndex("drawers"),
      }}
    >
      <DrawerPanel id={openDrawerId!} title={drawerTitle} isActive onClose={closeDrawer}>
        {renderDrawerContent()}
      </DrawerPanel>
    </aside>
  );
};

export default DrawerHost;


