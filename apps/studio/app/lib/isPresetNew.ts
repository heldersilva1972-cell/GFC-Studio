import type { AnimationPreset } from "@/app/hooks/useFilteredPresets";
import type { NewSettings } from "@/app/AnimationPlaygroundClient";
import { getCalendarAnimationConfig } from "./calendarAnimationConfig";
import { shouldForceNewInTemplate, shouldNeverNewInTemplate, type CalendarTemplate } from "./calendarTemplateConfig";

export function isPresetNew(
  preset: AnimationPreset,
  allPresets: readonly AnimationPreset[],
  settings: NewSettings,
  activeTemplate?: CalendarTemplate | null
): boolean {
  // Phase C7: Check template-level overrides first (highest precedence)
  if (activeTemplate) {
    if (shouldForceNewInTemplate(activeTemplate, preset.id)) {
      return true;
    }
    if (shouldNeverNewInTemplate(activeTemplate, preset.id)) {
      return false;
    }
  }
  
  // Phase C6: Check for animation-level overrides (from admin config)
  const calendarConfig = getCalendarAnimationConfig(preset.id);
  if (calendarConfig?.newIndicatorOverride) {
    if (calendarConfig.newIndicatorOverride.forceNew) {
      return true;
    }
    if (calendarConfig.newIndicatorOverride.neverNew) {
      return false;
    }
  }

  // Continue with global rules if no override
  if (!preset.addedAt) return false;

  const added = new Date(preset.addedAt + "T00:00:00Z").getTime();
  const now = Date.now();
  const msPerDay = 1000 * 60 * 60 * 24;
  const ageDays = (now - added) / msPerDay;

  const isWithinDuration =
    settings.mode === "duration" || settings.mode === "duration-or-latest"
      ? ageDays <= settings.durationDays
      : false;

  let isLatest = false;
  if (settings.mode === "latest" || settings.mode === "duration-or-latest") {
    const maxAdded = allPresets
      .filter((p) => p.addedAt)
      .reduce<number>((max, p) => {
        const t = new Date(p.addedAt! + "T00:00:00Z").getTime();
        return t > max ? t : max;
      }, 0);

    if (maxAdded > 0) {
      isLatest = added === maxAdded;
    }
  }

  switch (settings.mode) {
    case "duration":
      return isWithinDuration;
    case "latest":
      return isLatest;
    case "duration-or-latest":
    default:
      return isWithinDuration || isLatest;
  }
}

