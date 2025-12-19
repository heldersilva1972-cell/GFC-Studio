/**
 * Calendar Animation Configuration Metadata
 * Phase C6: Admin-editable metadata for calendar animations
 */

export type CalendarAnimationCategory =
  | "Calendar Availability"
  | "Calendar Booked Days"
  | "Calendar Add Event"
  | "Calendar Status Indicators"
  | "Calendar Navigation"
  | "Calendar Loading & Feedback";

export interface CalendarAnimationDefaultUsage {
  hoverDay?: boolean;
  clickDay?: boolean;
  selectWeek?: boolean;
  monthChange?: boolean;
  bookedDay?: boolean;
  blockedDay?: boolean;
  availableDay?: boolean;
  loadingState?: boolean;
  statusIndicator?: boolean;
}

export interface NewIndicatorOverride {
  forceNew?: boolean;
  neverNew?: boolean;
}

export interface CalendarAnimationConfig {
  id: string; // Stable key, matches existing animation ID
  internalName: string; // Component/registry name
  displayName: string;
  description: string;
  category: CalendarAnimationCategory;
  tags: string[];
  enabled: boolean; // Controls visibility in tiles
  isDefaultFor: CalendarAnimationDefaultUsage;
  newIndicatorOverride?: NewIndicatorOverride;
}

// Central metadata for all calendar animations
// This is the source of truth for admin editing
export const calendarAnimationConfigs: CalendarAnimationConfig[] = [
  // Availability animations
  {
    id: "gfc-calendar-available-tile",
    internalName: "GfcCalendarAvailableTile",
    displayName: "Available Day Tile Glow",
    description: "Glowing animation for available calendar days",
    category: "Calendar Availability",
    tags: ["availability", "glow", "tile"],
    enabled: true,
    isDefaultFor: { availableDay: true, hoverDay: true },
  },
  {
    id: "gfc-calendar-available-day-pulse",
    internalName: "GfcCalendarAvailableDayPulse",
    displayName: "Available Day Pulse",
    description: "Pulsing animation for available days",
    category: "Calendar Availability",
    tags: ["availability", "pulse"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-available-hover-lift",
    internalName: "GfcCalendarAvailableHoverLift",
    displayName: "Available Hover Lift",
    description: "Lift animation on hover for available days",
    category: "Calendar Availability",
    tags: ["availability", "hover", "lift"],
    enabled: true,
    isDefaultFor: { hoverDay: true },
  },
  {
    id: "gfc-calendar-soft-availability-ripple",
    internalName: "GfcCalendarSoftAvailabilityRipple",
    displayName: "Soft Availability Ripple",
    description: "Soft ripple effect for available days",
    category: "Calendar Availability",
    tags: ["availability", "ripple"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-availability-gradient-sweep",
    internalName: "GfcCalendarAvailabilityGradientSweep",
    displayName: "Availability Gradient Sweep",
    description: "Gradient sweep animation for available days",
    category: "Calendar Availability",
    tags: ["availability", "gradient"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-faint-checkmark-reveal",
    internalName: "GfcCalendarFaintCheckmarkReveal",
    displayName: "Faint Checkmark Reveal",
    description: "Subtle checkmark reveal for available days",
    category: "Calendar Availability",
    tags: ["availability", "checkmark"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-morning-evening-color-split",
    internalName: "GfcCalendarMorningEveningColorSplit",
    displayName: "Morning/Evening Color Split",
    description: "Color split animation for time-based availability",
    category: "Calendar Availability",
    tags: ["availability", "time", "color"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-priority-date-glow",
    internalName: "GfcCalendarPriorityDateGlow",
    displayName: "Priority Date Glow",
    description: "Glow effect for priority available dates",
    category: "Calendar Availability",
    tags: ["availability", "priority", "glow"],
    enabled: true,
    isDefaultFor: {},
  },
  // Booked Days animations
  {
    id: "gfc-calendar-booked-tile",
    internalName: "GfcCalendarBookedTile",
    displayName: "Booked Day Tile Lock",
    description: "Lock animation for booked calendar days",
    category: "Calendar Booked Days",
    tags: ["booked", "lock", "tile"],
    enabled: true,
    isDefaultFor: { bookedDay: true, clickDay: true },
  },
  {
    id: "gfc-calendar-booked-day-lock-in",
    internalName: "GfcCalendarBookedDayLockIn",
    displayName: "Booked Day Lock In",
    description: "Lock-in animation for booked days",
    category: "Calendar Booked Days",
    tags: ["booked", "lock"],
    enabled: true,
    isDefaultFor: { bookedDay: true },
  },
  {
    id: "gfc-calendar-conflict-shake-red",
    internalName: "GfcCalendarConflictShakeRed",
    displayName: "Conflict Shake Red",
    description: "Red shake animation for conflicts",
    category: "Calendar Booked Days",
    tags: ["booked", "conflict", "shake"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-full-day-flash-warning",
    internalName: "GfcCalendarFullDayFlashWarning",
    displayName: "Full Day Flash Warning",
    description: "Flash warning for fully booked days",
    category: "Calendar Booked Days",
    tags: ["booked", "warning", "flash"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-blocked-date-stripe",
    internalName: "GfcCalendarBlockedDateStripe",
    displayName: "Blocked Date Stripe",
    description: "Stripe pattern for blocked dates",
    category: "Calendar Booked Days",
    tags: ["blocked", "stripe"],
    enabled: true,
    isDefaultFor: { blockedDay: true },
  },
  {
    id: "gfc-calendar-gap-rule-violation-pulse",
    internalName: "GfcCalendarGapRuleViolationPulse",
    displayName: "Gap Rule Violation Pulse",
    description: "Pulse animation for gap rule violations",
    category: "Calendar Booked Days",
    tags: ["booked", "violation", "pulse"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-event-overlap-slide-alert",
    internalName: "GfcCalendarEventOverlapSlideAlert",
    displayName: "Event Overlap Slide Alert",
    description: "Slide alert for overlapping events",
    category: "Calendar Booked Days",
    tags: ["booked", "overlap", "alert"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-holiday-seal-stamp",
    internalName: "GfcCalendarHolidaySealStamp",
    displayName: "Holiday Seal Stamp",
    description: "Seal stamp animation for holidays",
    category: "Calendar Booked Days",
    tags: ["booked", "holiday", "stamp"],
    enabled: true,
    isDefaultFor: {},
  },
  // Event Creation animations
  {
    id: "gfc-calendar-add-event-slide-up-form",
    internalName: "GfcCalendarAddEventSlideUpForm",
    displayName: "Add Event Slide Up Form",
    description: "Slide-up form animation for adding events",
    category: "Calendar Add Event",
    tags: ["event-creation", "form", "slide"],
    enabled: true,
    isDefaultFor: { clickDay: true },
  },
  {
    id: "gfc-calendar-add-event-target-highlight",
    internalName: "GfcCalendarAddEventTargetHighlight",
    displayName: "Add Event Target Highlight",
    description: "Highlight animation for event creation target",
    category: "Calendar Add Event",
    tags: ["event-creation", "highlight"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-event-created-success-burst",
    internalName: "GfcCalendarEventCreatedSuccessBurst",
    displayName: "Event Created Success Burst",
    description: "Success burst animation when event is created",
    category: "Calendar Add Event",
    tags: ["event-creation", "success", "burst"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-event-denied-shake",
    internalName: "GfcCalendarEventDeniedShake",
    displayName: "Event Denied Shake",
    description: "Shake animation when event creation is denied",
    category: "Calendar Add Event",
    tags: ["event-creation", "denied", "shake"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-event-saved-ribbon",
    internalName: "GfcCalendarEventSavedRibbon",
    displayName: "Event Saved Ribbon",
    description: "Ribbon animation when event is saved",
    category: "Calendar Add Event",
    tags: ["event-creation", "saved", "ribbon"],
    enabled: true,
    isDefaultFor: {},
  },
  // Navigation animations
  {
    id: "gfc-calendar-header-slide",
    internalName: "GfcCalendarHeaderSlide",
    displayName: "Calendar Header Slide",
    description: "Slide animation for calendar header",
    category: "Calendar Navigation",
    tags: ["navigation", "header", "slide"],
    enabled: true,
    isDefaultFor: { monthChange: true },
  },
  {
    id: "gfc-calendar-month-switch",
    internalName: "GfcCalendarMonthSwitch",
    displayName: "Month Switch Animation",
    description: "Animation for switching months",
    category: "Calendar Navigation",
    tags: ["navigation", "month"],
    enabled: true,
    isDefaultFor: { monthChange: true },
  },
  {
    id: "gfc-calendar-range-highlight",
    internalName: "GfcCalendarRangeHighlight",
    displayName: "Highlighted Range Sweep",
    description: "Sweep animation for highlighted date ranges",
    category: "Calendar Navigation",
    tags: ["navigation", "range", "highlight"],
    enabled: true,
    isDefaultFor: { selectWeek: true },
  },
  {
    id: "gfc-calendar-month-slide-left",
    internalName: "GfcCalendarMonthSlideLeft",
    displayName: "Month Slide Left",
    description: "Slide left animation for month navigation",
    category: "Calendar Navigation",
    tags: ["navigation", "month", "slide"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-month-slide-right",
    internalName: "GfcCalendarMonthSlideRight",
    displayName: "Month Slide Right",
    description: "Slide right animation for month navigation",
    category: "Calendar Navigation",
    tags: ["navigation", "month", "slide"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-month-fade-reveal",
    internalName: "GfcCalendarMonthFadeReveal",
    displayName: "Month Fade Reveal",
    description: "Fade reveal animation for month changes",
    category: "Calendar Navigation",
    tags: ["navigation", "month", "fade"],
    enabled: true,
    isDefaultFor: {},
  },
  // Status Indicators
  {
    id: "gfc-calendar-pending-tile",
    internalName: "GfcCalendarPendingTile",
    displayName: "Pending Day Tile Pulse",
    description: "Pulse animation for pending calendar days",
    category: "Calendar Status Indicators",
    tags: ["status", "pending", "pulse"],
    enabled: true,
    isDefaultFor: { statusIndicator: true },
  },
  // Admin-only (if any)
  {
    id: "gfc-calendar-admin-override-glow",
    internalName: "GfcCalendarAdminOverrideGlow",
    displayName: "Admin Override Glow",
    description: "Glow effect for admin overrides",
    category: "Calendar Status Indicators",
    tags: ["admin", "override", "glow"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-admin-edit-tile-flip",
    internalName: "GfcCalendarAdminEditTileFlip",
    displayName: "Admin Edit Tile Flip",
    description: "Flip animation for admin edit tiles",
    category: "Calendar Status Indicators",
    tags: ["admin", "edit", "flip"],
    enabled: true,
    isDefaultFor: {},
  },
  {
    id: "gfc-calendar-admin-bulk-blocking-sweep",
    internalName: "GfcCalendarAdminBulkBlockingSweep",
    displayName: "Admin Bulk Blocking Sweep",
    description: "Sweep animation for bulk blocking",
    category: "Calendar Status Indicators",
    tags: ["admin", "bulk", "sweep"],
    enabled: true,
    isDefaultFor: {},
  },
];

// Helper functions to get calendar animation configs
export function getCalendarAnimationConfig(id: string): CalendarAnimationConfig | undefined {
  return calendarAnimationConfigs.find((config) => config.id === id);
}

export function getCalendarAnimationConfigsByCategory(
  category: CalendarAnimationCategory
): CalendarAnimationConfig[] {
  return calendarAnimationConfigs.filter((config) => config.category === category);
}

export function getEnabledCalendarAnimationConfigs(): CalendarAnimationConfig[] {
  return calendarAnimationConfigs.filter((config) => config.enabled);
}

export function getDefaultAnimationForUsage(
  usage: keyof CalendarAnimationDefaultUsage
): CalendarAnimationConfig | undefined {
  return calendarAnimationConfigs.find(
    (config) => config.enabled && config.isDefaultFor[usage] === true
  );
}

// Update function (for admin panel - updates in-memory config)
export function updateCalendarAnimationConfig(
  id: string,
  updates: Partial<CalendarAnimationConfig>
): void {
  const index = calendarAnimationConfigs.findIndex((config) => config.id === id);
  if (index !== -1) {
    calendarAnimationConfigs[index] = { ...calendarAnimationConfigs[index], ...updates };
  }
}

