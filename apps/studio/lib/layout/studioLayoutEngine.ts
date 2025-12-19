import type { CSSProperties } from "react";

export type StudioRegion = "sidebar" | "topBar" | "stage" | "drawerArea";

export interface StudioLayoutConfig {
  sidebarWidth: number;
  topBarHeight: number;
  drawerWidth: number;
  minStageWidth: number;
}

export interface RegionStyles {
  sidebarStyle: CSSProperties;
  topBarStyle: CSSProperties;
  stageStyle: CSSProperties;
  drawerAreaStyle: CSSProperties;
}

export const defaultStudioLayoutConfig: StudioLayoutConfig = {
  sidebarWidth: 280,
  topBarHeight: 56,
  drawerWidth: 360,
  minStageWidth: 720,
};

const toPx = (value: number) => `${value}px`;

export const getRegionStyles = (config: StudioLayoutConfig = defaultStudioLayoutConfig): RegionStyles => {
  const { sidebarWidth, topBarHeight, drawerWidth, minStageWidth } = config;

  const sidebarStyle: CSSProperties = {
    width: toPx(sidebarWidth),
    flexShrink: 0,
  };

  const topBarStyle: CSSProperties = {
    height: toPx(topBarHeight),
    flexShrink: 0,
    width: "100%",
  };

  const stageStyle: CSSProperties = {
    flex: 1,
    minWidth: toPx(minStageWidth),
    width: "100%",
  };

  const drawerAreaStyle: CSSProperties = {
    width: toPx(drawerWidth),
    flexShrink: 0,
  };

  return {
    sidebarStyle,
    topBarStyle,
    stageStyle,
    drawerAreaStyle,
  };
};

