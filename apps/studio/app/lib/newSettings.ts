import type { NewIndicatorMode } from "@/app/hooks/useFilteredPresets";

export interface NewSettings {
  mode: NewIndicatorMode; // "duration" | "latest" | "duration-or-latest"
  durationDays: number; // # of days a preset is considered new (for duration mode)
}

export const DEFAULT_NEW_SETTINGS: NewSettings = {
  mode: "duration-or-latest",
  durationDays: 30,
};

const STORAGE_KEY = "animationPlaygroundNewSettings";

export function loadNewSettings(): NewSettings {
  if (typeof window === "undefined") {
    return DEFAULT_NEW_SETTINGS;
  }

  try {
    const stored = localStorage.getItem(STORAGE_KEY);
    if (!stored) {
      return DEFAULT_NEW_SETTINGS;
    }

    const parsed = JSON.parse(stored) as Partial<NewSettings>;
    const settings: NewSettings = {
      mode: parsed.mode && ["duration", "latest", "duration-or-latest"].includes(parsed.mode)
        ? parsed.mode
        : DEFAULT_NEW_SETTINGS.mode,
      durationDays:
        typeof parsed.durationDays === "number" &&
        parsed.durationDays >= 1 &&
        parsed.durationDays <= 365
          ? parsed.durationDays
          : DEFAULT_NEW_SETTINGS.durationDays,
    };

    return settings;
  } catch (error) {
    console.warn("Failed to load new settings from localStorage:", error);
    return DEFAULT_NEW_SETTINGS;
  }
}

export function saveNewSettings(settings: NewSettings): void {
  if (typeof window === "undefined") {
    return;
  }

  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(settings));
  } catch (error) {
    console.warn("Failed to save new settings to localStorage:", error);
  }
}

