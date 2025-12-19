/**
 * Hook to map calendar interactions to appropriate animations
 * Phase C5: Connects calendar interactions to animation previews
 * Phase C6: Updated to use calendar animation config metadata
 * Phase C7: Updated to support template-based animation selection
 */

import { useMemo } from "react";
import type { CalendarDayState } from "@/components/gfc/GfcCalendarInteractivePanel";
import type { AnimationPresetId } from "../AnimationPlaygroundClient";
import {
  getDefaultAnimationForUsage,
  getCalendarAnimationConfig,
} from "@/app/lib/calendarAnimationConfig";
import {
  getAnimationIdForUsage,
  type CalendarTemplate,
} from "@/app/lib/calendarTemplateConfig";

export function useCalendarAnimation(activeTemplate: CalendarTemplate | null = null) {
  const getAnimationForDayState = useMemo(() => {
    return (state: CalendarDayState): AnimationPresetId | null => {
      // Phase C7: Check template first
      let usageKey: keyof import("@/app/lib/calendarTemplateConfig").CalendarTemplateAnimationDefaults | null = null;
      
      switch (state) {
        case "available":
          usageKey = "availableDay";
          break;
        case "booked":
          usageKey = "bookedDay";
          break;
        case "blocked":
          usageKey = "blockedDay";
          break;
        case "pending":
          usageKey = "statusIndicator";
          break;
        default:
          return null;
      }
      
      // Try template first
      if (usageKey && activeTemplate) {
        const templateAnimationId = getAnimationIdForUsage(activeTemplate, usageKey);
        if (templateAnimationId) {
          return templateAnimationId as AnimationPresetId;
        }
      }
      
      // Fallback to config metadata
      let configUsageKey: keyof import("@/app/lib/calendarAnimationConfig").CalendarAnimationDefaultUsage | null = null;
      switch (state) {
        case "available":
          configUsageKey = "availableDay";
          break;
        case "booked":
          configUsageKey = "bookedDay";
          break;
        case "blocked":
          configUsageKey = "blockedDay";
          break;
        case "pending":
          configUsageKey = "statusIndicator";
          break;
        default:
          return null;
      }
      
      if (configUsageKey) {
        const config = getDefaultAnimationForUsage(configUsageKey);
        if (config) {
          return config.id as AnimationPresetId;
        }
      }
      
      // Fallback to hardcoded defaults if no config found
      const fallbacks: Record<CalendarDayState, AnimationPresetId | null> = {
        available: "gfc-calendar-available-tile",
        booked: "gfc-calendar-booked-tile",
        blocked: "gfc-calendar-blocked-date-stripe",
        pending: "gfc-calendar-pending-tile",
      };
      
      return fallbacks[state] || null;
    };
  }, [activeTemplate]);

  const getAnimationForNavigation = useMemo(() => {
    return (): AnimationPresetId | null => {
      // Phase C7: Check template first
      if (activeTemplate) {
        const templateAnimationId = getAnimationIdForUsage(activeTemplate, "monthChange");
        if (templateAnimationId) {
          return templateAnimationId as AnimationPresetId;
        }
      }
      
      const config = getDefaultAnimationForUsage("monthChange");
      if (config) {
        return config.id as AnimationPresetId;
      }
      // Fallback
      return "gfc-calendar-header-slide";
    };
  }, [activeTemplate]);

  const getAnimationForWeek = useMemo(() => {
    return (): AnimationPresetId | null => {
      // Phase C7: Check template first
      if (activeTemplate) {
        const templateAnimationId = getAnimationIdForUsage(activeTemplate, "selectWeek");
        if (templateAnimationId) {
          return templateAnimationId as AnimationPresetId;
        }
      }
      
      const config = getDefaultAnimationForUsage("selectWeek");
      if (config) {
        return config.id as AnimationPresetId;
      }
      // Fallback
      return "gfc-calendar-range-highlight";
    };
  }, [activeTemplate]);

  const getAnimationForEventCreation = useMemo(() => {
    return (): AnimationPresetId | null => {
      // Phase C7: Check template first
      if (activeTemplate) {
        const templateAnimationId = getAnimationIdForUsage(activeTemplate, "clickDay");
        if (templateAnimationId) {
          return templateAnimationId as AnimationPresetId;
        }
      }
      
      const config = getDefaultAnimationForUsage("clickDay");
      if (config) {
        return config.id as AnimationPresetId;
      }
      // Fallback
      return "gfc-calendar-add-event-slide-up-form";
    };
  }, [activeTemplate]);

  const getAnimationForHover = useMemo(() => {
    return (): AnimationPresetId | null => {
      // Phase C7: Check template first
      if (activeTemplate) {
        const templateAnimationId = getAnimationIdForUsage(activeTemplate, "hoverDay");
        if (templateAnimationId) {
          return templateAnimationId as AnimationPresetId;
        }
      }
      
      const config = getDefaultAnimationForUsage("hoverDay");
      if (config) {
        return config.id as AnimationPresetId;
      }
      // Fallback
      return "gfc-calendar-available-hover-lift";
    };
  }, [activeTemplate]);

  return {
    getAnimationForDayState,
    getAnimationForNavigation,
    getAnimationForWeek,
    getAnimationForEventCreation,
    getAnimationForHover,
  };
}

