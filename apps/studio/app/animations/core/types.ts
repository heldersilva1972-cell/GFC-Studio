import type { ComponentType } from "react";

export type GfcWebsiteType =
  | "hero-banner"
  | "cta-button"
  | "card-tile"
  | "section-layout"
  | "banner-status"
  | "branding-impact"
  | "calendar"
  | "calendar-event-creation"
  | "flags-patriotic";

export interface AnimationEditorMeta {
  sourceCode?: string;
  filePathHint?: string;
  notes?: string;
  suggestedUse?: string;
}

export interface AnimationConfig {
  id: string;
  name: string;
  category: string;
  complexity: number;
  previewSize: "sm" | "md" | "lg";
  component: ComponentType<AnimationProps>;
  code: string;
  editor?: AnimationEditorMeta;
  gfcType?: GfcWebsiteType;
}

export interface AnimationProps {
  className?: string;
  size?: number;
  speed?: number;
  colors?: string[];
  [key: string]: any;
}

export interface AnimationEngineState {
  selectedAnimationId: string | null;
  speed: number;
  size: number;
  colors: string[];
  theme: "light" | "dark";
}

export interface AnimationEngineActions {
  setSelectedAnimation: (id: string | null) => void;
  setSpeed: (speed: number) => void;
  setSize: (size: number) => void;
  setColors: (colors: string[]) => void;
  setTheme: (theme: "light" | "dark") => void;
  reset: () => void;
}

export interface AnimationEngineContextValue {
  state: AnimationEngineState;
  actions: AnimationEngineActions;
}

