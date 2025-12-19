/**
 * Calendar Template Configuration Metadata
 * Phase C7: Template-based animation sets for calendar contexts
 */

import type { AnimationPresetId } from "../AnimationPlaygroundClient";
import { CALENDAR_CONTEXTS } from "./calendarState";

export interface CalendarTemplateAnimationDefaults {
  hoverDay?: AnimationPresetId;
  clickDay?: AnimationPresetId;
  selectWeek?: AnimationPresetId;
  monthChange?: AnimationPresetId;
  bookedDay?: AnimationPresetId;
  blockedDay?: AnimationPresetId;
  availableDay?: AnimationPresetId;
  loadingState?: AnimationPresetId;
  statusIndicator?: AnimationPresetId;
}

export interface CalendarTemplateNewIndicatorOverrides {
  forceNewAnimationIds?: string[];
  neverNewAnimationIds?: string[];
}

export interface CalendarTemplate {
  id: string; // Stable key
  name: string; // Display name
  description: string;
  isFavorite: boolean;
  applicableCalendars?: string[]; // Calendar IDs, empty means "all"
  animationDefaults: CalendarTemplateAnimationDefaults;
  enabledAnimationIds: string[]; // Which animations are enabled in tiles
  newIndicatorOverrides: CalendarTemplateNewIndicatorOverrides;
}

// Central metadata for calendar templates
export const calendarTemplates: CalendarTemplate[] = [
  {
    id: "gfc-default-calendar",
    name: "GFC Default Calendar",
    description: "Standard calendar template with balanced animations for general use.",
    isFavorite: true,
    applicableCalendars: [], // Applies to all calendars
    animationDefaults: {
      hoverDay: "gfc-calendar-available-hover-lift",
      clickDay: "gfc-calendar-available-tile",
      selectWeek: "gfc-calendar-range-highlight",
      monthChange: "gfc-calendar-header-slide",
      bookedDay: "gfc-calendar-booked-tile",
      blockedDay: "gfc-calendar-blocked-date-stripe",
      availableDay: "gfc-calendar-available-tile",
      loadingState: "gfc-calendar-available-day-pulse",
      statusIndicator: "gfc-calendar-pending-tile",
    },
    enabledAnimationIds: [
      "gfc-calendar-available-tile",
      "gfc-calendar-booked-tile",
      "gfc-calendar-pending-tile",
      "gfc-calendar-header-slide",
      "gfc-calendar-month-switch",
      "gfc-calendar-range-highlight",
      "gfc-calendar-available-day-pulse",
      "gfc-calendar-available-hover-lift",
      "gfc-calendar-soft-availability-ripple",
      "gfc-calendar-booked-day-lock-in",
      "gfc-calendar-conflict-shake-red",
      "gfc-calendar-month-slide-left",
      "gfc-calendar-month-slide-right",
    ],
    newIndicatorOverrides: {
      forceNewAnimationIds: [],
      neverNewAnimationIds: [],
    },
  },
  {
    id: "wedding-private-event",
    name: "Wedding / Private Event",
    description: "Elegant, subtle animations perfect for formal events and weddings.",
    isFavorite: true,
    applicableCalendars: ["main-hall", "lower-hall"],
    animationDefaults: {
      hoverDay: "gfc-calendar-soft-availability-ripple",
      clickDay: "gfc-calendar-available-tile",
      selectWeek: "gfc-calendar-range-highlight",
      monthChange: "gfc-calendar-month-fade-reveal",
      bookedDay: "gfc-calendar-booked-day-lock-in",
      blockedDay: "gfc-calendar-blocked-date-stripe",
      availableDay: "gfc-calendar-faint-checkmark-reveal",
      loadingState: "gfc-calendar-available-day-pulse",
      statusIndicator: "gfc-calendar-pending-tile",
    },
    enabledAnimationIds: [
      "gfc-calendar-available-tile",
      "gfc-calendar-booked-tile",
      "gfc-calendar-pending-tile",
      "gfc-calendar-header-slide",
      "gfc-calendar-month-fade-reveal",
      "gfc-calendar-range-highlight",
      "gfc-calendar-soft-availability-ripple",
      "gfc-calendar-faint-checkmark-reveal",
      "gfc-calendar-booked-day-lock-in",
      "gfc-calendar-holiday-seal-stamp",
    ],
    newIndicatorOverrides: {
      forceNewAnimationIds: ["gfc-calendar-faint-checkmark-reveal"],
      neverNewAnimationIds: [],
    },
  },
  {
    id: "club-night-busy-mode",
    name: "Club Night / Busy Mode",
    description: "High-energy, attention-grabbing animations for busy event periods.",
    isFavorite: false,
    applicableCalendars: ["main-hall", "billiards-room"],
    animationDefaults: {
      hoverDay: "gfc-calendar-availability-gradient-sweep",
      clickDay: "gfc-calendar-priority-date-glow",
      selectWeek: "gfc-calendar-range-highlight",
      monthChange: "gfc-calendar-month-slide-left",
      bookedDay: "gfc-calendar-conflict-shake-red",
      blockedDay: "gfc-calendar-full-day-flash-warning",
      availableDay: "gfc-calendar-priority-date-glow",
      loadingState: "gfc-calendar-available-day-pulse",
      statusIndicator: "gfc-calendar-pending-tile",
    },
    enabledAnimationIds: [
      "gfc-calendar-available-tile",
      "gfc-calendar-booked-tile",
      "gfc-calendar-pending-tile",
      "gfc-calendar-header-slide",
      "gfc-calendar-month-slide-left",
      "gfc-calendar-month-slide-right",
      "gfc-calendar-range-highlight",
      "gfc-calendar-availability-gradient-sweep",
      "gfc-calendar-priority-date-glow",
      "gfc-calendar-conflict-shake-red",
      "gfc-calendar-full-day-flash-warning",
      "gfc-calendar-event-overlap-slide-alert",
    ],
    newIndicatorOverrides: {
      forceNewAnimationIds: ["gfc-calendar-priority-date-glow", "gfc-calendar-conflict-shake-red"],
      neverNewAnimationIds: [],
    },
  },
];

// Helper functions
export function getCalendarTemplate(templateId: string): CalendarTemplate | undefined {
  return calendarTemplates.find((t) => t.id === templateId);
}

export function getTemplatesForCalendar(calendarId: string): CalendarTemplate[] {
  return calendarTemplates.filter(
    (t) => !t.applicableCalendars || t.applicableCalendars.length === 0 || t.applicableCalendars.includes(calendarId)
  );
}

export function getFavoriteTemplates(): CalendarTemplate[] {
  return calendarTemplates.filter((t) => t.isFavorite);
}

export function getAllTemplates(): CalendarTemplate[] {
  return [...calendarTemplates];
}

// Template application helpers
export function getAnimationIdForUsage(
  template: CalendarTemplate | null,
  usageKey: keyof CalendarTemplateAnimationDefaults
): string | null {
  if (!template) return null;
  return template.animationDefaults[usageKey] || null;
}

export function isAnimationEnabledInTemplate(template: CalendarTemplate | null, animationId: string): boolean {
  if (!template) return true; // If no template, default to enabled
  return template.enabledAnimationIds.includes(animationId);
}

export function shouldForceNewInTemplate(template: CalendarTemplate | null, animationId: string): boolean {
  if (!template) return false;
  return template.newIndicatorOverrides.forceNewAnimationIds?.includes(animationId) || false;
}

export function shouldNeverNewInTemplate(template: CalendarTemplate | null, animationId: string): boolean {
  if (!template) return false;
  return template.newIndicatorOverrides.neverNewAnimationIds?.includes(animationId) || false;
}


