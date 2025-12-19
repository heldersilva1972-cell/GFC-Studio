/**
 * Calendar State Management Utilities
 * Phase C5: Multi-calendar support and state management
 */

import type { CalendarDayState, CalendarSelection } from "@/components/gfc/GfcCalendarInteractivePanel";

export interface CalendarContext {
  id: string;
  name: string;
  currentMonth: Date;
  selection: CalendarSelection;
  dayStates: Map<string, CalendarDayState>;
}

// Mock calendar data - stored in memory only
const MOCK_CALENDAR_DATA: Record<string, Map<string, CalendarDayState>> = {
  "main-hall": new Map([
    // Example: Some booked days
    ["2025-01-15", "booked"],
    ["2025-01-16", "booked"],
    ["2025-01-20", "blocked"],
    ["2025-01-25", "pending"],
    ["2025-01-30", "booked"],
  ]),
  "lower-hall": new Map([
    ["2025-01-10", "booked"],
    ["2025-01-18", "blocked"],
    ["2025-01-22", "booked"],
  ]),
  "billiards-room": new Map([
    ["2025-01-12", "booked"],
    ["2025-01-19", "pending"],
    ["2025-01-28", "blocked"],
  ]),
};

export const CALENDAR_CONTEXTS = [
  { id: "main-hall", name: "Main Hall" },
  { id: "lower-hall", name: "Lower Hall" },
  { id: "billiards-room", name: "Billiards Room" },
];

export function getInitialCalendarContext(calendarId: string): CalendarContext {
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  
  return {
    id: calendarId,
    name: CALENDAR_CONTEXTS.find((c) => c.id === calendarId)?.name || calendarId,
    currentMonth: new Date(today.getFullYear(), today.getMonth(), 1),
    selection: {
      startDate: null,
      endDate: null,
      selectedDates: new Set(),
    },
    dayStates: new Map(MOCK_CALENDAR_DATA[calendarId] || []),
  };
}

export function getDayState(calendarId: string, dateKey: string): CalendarDayState {
  return MOCK_CALENDAR_DATA[calendarId]?.get(dateKey) || "available";
}

export function setDayState(
  calendarId: string,
  dateKey: string,
  state: CalendarDayState
): void {
  if (!MOCK_CALENDAR_DATA[calendarId]) {
    MOCK_CALENDAR_DATA[calendarId] = new Map();
  }
  MOCK_CALENDAR_DATA[calendarId].set(dateKey, state);
}

