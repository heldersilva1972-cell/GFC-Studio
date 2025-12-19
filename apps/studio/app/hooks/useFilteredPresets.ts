import { useMemo } from "react";
import type { GfcWebsiteType } from "@/app/animations/core/types";

export type NewIndicatorMode = "duration" | "latest" | "duration-or-latest";

export type UsageCategory =
  | "all"
  | "new"
  | "hero"
  | "section-transition"
  | "card-tile"
  | "button-cta"
  | "buttons"
  | "text-headline"
  | "text-fx"
  | "background-ambient"
  | "interactive"
  | "forms-auth"
  | "dashboards-metrics"
  | "media-galleries"
  | "gfc-website"
  | "flags"
  | "admin";

export interface AnimationPreset {
  id: string;
  name: string;
  difficulty: "Beginner" | "Intermediate" | "Advanced";
  shortDescription: string;
  longDescription: string;
  tags: readonly string[];
  code: string;
  Component: React.ComponentType<any>;
  usageCategory: UsageCategory;
  gfcType?: GfcWebsiteType;
  addedAt?: string; // ISO date string "YYYY-MM-DD" when this preset was introduced
}

export function useFilteredPresets(
  presets: readonly AnimationPreset[],
  selectedCategory: UsageCategory
) {
  // SINGLE SOURCE OF TRUTH: This is the ONLY place filtering happens
  const visiblePresets = useMemo(() => {
    if (!selectedCategory || selectedCategory === "all") {
      // Always return a new array copy to prevent mutations
      return [...presets];
    }
    // Filter creates a new array, which is correct
    // This ensures React detects the change and re-renders
    return presets.filter((p) => p.usageCategory === selectedCategory);
  }, [presets, selectedCategory]);

  return { visiblePresets };
}

