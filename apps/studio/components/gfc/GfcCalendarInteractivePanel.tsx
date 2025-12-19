"use client";

import React, { useMemo, useState, useCallback } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { ChevronLeft, ChevronRight } from "lucide-react";

export type CalendarDayState = "available" | "booked" | "blocked" | "pending";

export interface CalendarDay {
  date: Date;
  dayOfMonth: number;
  isCurrentMonth: boolean;
  state: CalendarDayState;
  isToday: boolean;
}

export interface CalendarSelection {
  startDate: Date | null;
  endDate: Date | null;
  selectedDates: Set<string>; // ISO date strings
}

interface GfcCalendarInteractivePanelProps {
  currentMonth: Date;
  onMonthChange: (month: Date) => void;
  selection: CalendarSelection;
  onSelectionChange: (selection: CalendarSelection) => void;
  onDayClick: (date: Date, state: CalendarDayState) => void;
  onDayHover: (date: Date | null, state: CalendarDayState | null) => void;
  onWeekClick: (weekStart: Date) => void;
  dayStates: Map<string, CalendarDayState>; // ISO date string -> state
  calendarId: string;
}

const DAYS_OF_WEEK = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];

export default function GfcCalendarInteractivePanel({
  currentMonth,
  onMonthChange,
  selection,
  onSelectionChange,
  onDayClick,
  onDayHover,
  onWeekClick,
  dayStates,
  calendarId,
}: GfcCalendarInteractivePanelProps) {
  const [hoveredDate, setHoveredDate] = useState<string | null>(null);
  const [hoveredWeek, setHoveredWeek] = useState<number | null>(null);

  // Generate calendar grid
  const calendarDays = useMemo(() => {
    const year = currentMonth.getFullYear();
    const month = currentMonth.getMonth();
    
    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    
    // Get first Monday of the calendar view (may be in previous month)
    const firstMonday = new Date(firstDay);
    const dayOfWeek = firstDay.getDay();
    const daysToSubtract = dayOfWeek === 0 ? 6 : dayOfWeek - 1;
    firstMonday.setDate(firstDay.getDate() - daysToSubtract);
    
    const days: CalendarDay[] = [];
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    
    for (let i = 0; i < 42; i++) {
      const date = new Date(firstMonday);
      date.setDate(firstMonday.getDate() + i);
      
      const dateKey = date.toISOString().split("T")[0];
      const state = dayStates.get(dateKey) || "available";
      const isToday = date.getTime() === today.getTime();
      
      days.push({
        date,
        dayOfMonth: date.getDate(),
        isCurrentMonth: date.getMonth() === month,
        state,
        isToday,
      });
    }
    
    return days;
  }, [currentMonth, dayStates]);

  // Group days into weeks
  const weeks = useMemo(() => {
    const weekArray: CalendarDay[][] = [];
    for (let i = 0; i < calendarDays.length; i += 7) {
      weekArray.push(calendarDays.slice(i, i + 7));
    }
    return weekArray;
  }, [calendarDays]);

  const handleDayClick = useCallback((day: CalendarDay, e: React.MouseEvent) => {
    e.stopPropagation();
    
    const dateKey = day.date.toISOString().split("T")[0];
    const newSelection = { ...selection };
    
    if (e.shiftKey && selection.startDate) {
      // Range selection
      const start = selection.startDate < day.date ? selection.startDate : day.date;
      const end = selection.startDate < day.date ? day.date : selection.startDate;
      
      newSelection.startDate = start;
      newSelection.endDate = end;
      newSelection.selectedDates = new Set();
      
      const current = new Date(start);
      while (current <= end) {
        newSelection.selectedDates.add(current.toISOString().split("T")[0]);
        current.setDate(current.getDate() + 1);
      }
    } else {
      // Single selection
      if (selection.selectedDates.has(dateKey)) {
        newSelection.selectedDates.delete(dateKey);
        if (newSelection.selectedDates.size === 0) {
          newSelection.startDate = null;
          newSelection.endDate = null;
        }
      } else {
        newSelection.selectedDates = new Set([dateKey]);
        newSelection.startDate = day.date;
        newSelection.endDate = day.date;
      }
    }
    
    onSelectionChange(newSelection);
    onDayClick(day.date, day.state);
  }, [selection, onSelectionChange, onDayClick]);

  const handleDayHover = useCallback((day: CalendarDay) => {
    const dateKey = day.date.toISOString().split("T")[0];
    setHoveredDate(dateKey);
    onDayHover(day.date, day.state);
  }, [onDayHover]);

  const handleWeekClick = useCallback((week: CalendarDay[], weekIndex: number) => {
    const weekStart = week[0].date;
    onWeekClick(weekStart);
  }, [onWeekClick]);

  const handlePrevMonth = useCallback(() => {
    const newMonth = new Date(currentMonth);
    newMonth.setMonth(newMonth.getMonth() - 1);
    onMonthChange(newMonth);
  }, [currentMonth, onMonthChange]);

  const handleNextMonth = useCallback(() => {
    const newMonth = new Date(currentMonth);
    newMonth.setMonth(newMonth.getMonth() + 1);
    onMonthChange(newMonth);
  }, [currentMonth, onMonthChange]);

  const monthName = currentMonth.toLocaleDateString("en-US", { month: "long", year: "numeric" });

  const getDayClassName = (day: CalendarDay, dateKey: string) => {
    const isSelected = selection.selectedDates.has(dateKey);
    const isHovered = hoveredDate === dateKey;
    const isInRange = selection.startDate && selection.endDate && 
      day.date >= selection.startDate && day.date <= selection.endDate;
    
    let baseClasses = "relative flex items-center justify-center h-10 w-10 rounded-lg text-sm font-medium transition-all cursor-pointer ";
    
    if (!day.isCurrentMonth) {
      baseClasses += "text-slate-600 opacity-40 ";
    } else {
      baseClasses += "text-slate-100 ";
    }
    
    if (day.isToday) {
      baseClasses += "ring-2 ring-cyan-400/50 ";
    }
    
    // State-based styling
    if (day.state === "booked") {
      baseClasses += "bg-red-500/20 border border-red-500/40 ";
    } else if (day.state === "blocked") {
      baseClasses += "bg-slate-700/50 border border-slate-600/50 ";
    } else if (day.state === "pending") {
      baseClasses += "bg-amber-500/20 border border-amber-500/40 ";
    } else {
      baseClasses += "bg-slate-800/30 border border-slate-700/50 ";
    }
    
    // Selection styling
    if (isSelected) {
      baseClasses += "bg-violet-500/40 border-violet-400/60 shadow-lg shadow-violet-900/40 ";
    } else if (isInRange) {
      baseClasses += "bg-violet-500/20 border-violet-400/40 ";
    }
    
    // Hover styling
    if (isHovered && day.isCurrentMonth) {
      baseClasses += "scale-110 border-cyan-400/60 shadow-md ";
    }
    
    return baseClasses;
  };

  return (
    <div className="w-full rounded-xl border border-slate-800/80 bg-gradient-to-br from-slate-900/90 to-slate-950/90 p-4">
      {/* Header with month navigation */}
      <div className="mb-4 flex items-center justify-between">
        <button
          onClick={handlePrevMonth}
          className="flex h-8 w-8 items-center justify-center rounded-lg border border-slate-700/50 bg-slate-800/50 text-slate-300 transition-colors hover:bg-slate-700/50 hover:text-slate-100"
        >
          <ChevronLeft className="h-4 w-4" />
        </button>
        <h3 className="text-sm font-bold text-slate-100">{monthName}</h3>
        <button
          onClick={handleNextMonth}
          className="flex h-8 w-8 items-center justify-center rounded-lg border border-slate-700/50 bg-slate-800/50 text-slate-300 transition-colors hover:bg-slate-700/50 hover:text-slate-100"
        >
          <ChevronRight className="h-4 w-4" />
        </button>
      </div>

      {/* Day headers */}
      <div className="mb-2 grid grid-cols-7 gap-1">
        {DAYS_OF_WEEK.map((day) => (
          <div
            key={day}
            className="flex items-center justify-center py-2 text-xs font-semibold text-slate-400"
          >
            {day}
          </div>
        ))}
      </div>

      {/* Calendar grid */}
      <div className="space-y-1">
        {weeks.map((week, weekIndex) => {
          const weekStartKey = week[0].date.toISOString().split("T")[0];
          const isWeekHovered = hoveredWeek === weekIndex;
          
          return (
            <div
              key={weekStartKey}
              className="grid grid-cols-7 gap-1"
              onMouseEnter={() => setHoveredWeek(weekIndex)}
              onMouseLeave={() => setHoveredWeek(null)}
              onClick={() => handleWeekClick(week, weekIndex)}
              role="button"
              tabIndex={0}
              onKeyDown={(e) => {
                if (e.key === "Enter" || e.key === " ") {
                  e.preventDefault();
                  handleWeekClick(week, weekIndex);
                }
              }}
              className={`rounded-lg transition-all ${
                isWeekHovered ? "bg-slate-800/30" : ""
              }`}
            >
              {week.map((day) => {
                const dateKey = day.date.toISOString().split("T")[0];
                return (
                  <motion.div
                    key={dateKey}
                    initial={false}
                    animate={{
                      scale: hoveredDate === dateKey && day.isCurrentMonth ? 1.1 : 1,
                    }}
                    transition={{ duration: 0.15 }}
                    onClick={(e) => handleDayClick(day, e)}
                    onMouseEnter={() => handleDayHover(day)}
                    onMouseLeave={() => {
                      setHoveredDate(null);
                      onDayHover(null, null);
                    }}
                    className={getDayClassName(day, dateKey)}
                  >
                    {day.dayOfMonth}
                    {day.state === "booked" && (
                      <div className="absolute inset-0 flex items-center justify-center">
                        <div className="h-1 w-1 rounded-full bg-red-400" />
                      </div>
                    )}
                    {day.state === "blocked" && (
                      <div className="absolute inset-0 flex items-center justify-center">
                        <div className="h-1 w-1 rounded-full bg-slate-500" />
                      </div>
                    )}
                    {day.state === "pending" && (
                      <div className="absolute inset-0 flex items-center justify-center">
                        <div className="h-1 w-1 rounded-full bg-amber-400" />
                      </div>
                    )}
                  </motion.div>
                );
              })}
            </div>
          );
        })}
      </div>
    </div>
  );
}

