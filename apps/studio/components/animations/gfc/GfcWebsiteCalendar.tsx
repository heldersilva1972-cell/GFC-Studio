"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Lock, ChevronLeft, ChevronRight } from "lucide-react";

// 1. Available Day Tile Glow
export function GfcCalendarAvailableTile() {
  const days = [10, 11, 12, 13, 14];
  const availableDay = 12;

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Available Dates</h3>
        <div className="flex justify-center gap-2">
          {days.map((day) => (
            <motion.div
              key={day}
              className={`relative flex h-16 w-16 flex-col items-center justify-center rounded-lg border transition-all ${
                day === availableDay
                  ? "border-emerald-500/50 bg-emerald-500/10"
                  : "border-slate-700/50 bg-slate-800/50"
              }`}
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, delay: days.indexOf(day) * 0.05 }}
              whileHover={
                day === availableDay
                  ? {
                      scale: 1.05,
                      boxShadow: "0 0 20px rgba(16, 185, 129, 0.4)",
                      borderColor: "rgba(16, 185, 129, 0.8)",
                    }
                  : {}
              }
            >
              {day === availableDay && (
                <motion.div
                  className="absolute inset-0 rounded-lg bg-emerald-500/20"
                  animate={{
                    scale: [1, 1.04, 1],
                    opacity: [0.3, 0.6, 0.3],
                  }}
                  transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
                />
              )}
              <span className="relative z-10 text-lg font-bold text-slate-100">{day}</span>
              {day === availableDay && (
                <span className="relative z-10 text-[10px] font-medium text-emerald-400">Available</span>
              )}
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 2. Booked Day Tile Lock Fade
export function GfcCalendarBookedTile() {
  const days = [15, 16, 17, 18, 19];
  const bookedDay = 17;

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Calendar Status</h3>
        <div className="flex justify-center gap-2">
          {days.map((day) => (
            <motion.div
              key={day}
              className={`relative flex h-16 w-16 flex-col items-center justify-center rounded-lg border transition-all ${
                day === bookedDay
                  ? "border-red-500/50 bg-red-900/30"
                  : "border-slate-700/50 bg-slate-800/50"
              }`}
              initial={{ opacity: 0, backgroundColor: "rgba(51, 65, 85, 0.5)" }}
              animate={{
                opacity: 1,
                backgroundColor: day === bookedDay ? "rgba(127, 29, 29, 0.3)" : "rgba(30, 41, 59, 0.5)",
              }}
              transition={{ duration: 0.5, delay: days.indexOf(day) * 0.1 }}
              whileHover={
                day === bookedDay
                  ? {
                      scale: 1.05,
                    }
                  : {}
              }
            >
              {day === bookedDay && (
                <>
                  <motion.div
                    initial={{ scale: 0, opacity: 0 }}
                    animate={{ scale: 1, opacity: 1 }}
                    transition={{ duration: 0.4, delay: 0.3 }}
                    whileHover={{
                      scale: [1, 1.1, 1],
                      rotate: [0, -5, 5, 0],
                    }}
                    transition={{ duration: 0.3 }}
                  >
                    <Lock className="h-5 w-5 text-red-400" />
                  </motion.div>
                  <motion.span
                    className="mt-1 text-[10px] font-medium text-red-400"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    transition={{ delay: 0.5 }}
                  >
                    Booked
                  </motion.span>
                </>
              )}
              {day !== bookedDay && (
                <span className="text-lg font-bold text-slate-100">{day}</span>
              )}
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 3. Pending Day Tile Pulse
export function GfcCalendarPendingTile() {
  const days = [20, 21, 22, 23, 24];
  const pendingDay = 22;

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Booking Status</h3>
        <div className="flex justify-center gap-2">
          {days.map((day) => (
            <motion.div
              key={day}
              className={`relative flex h-16 w-16 flex-col items-center justify-center rounded-lg border-2 transition-all ${
                day === pendingDay
                  ? "border-amber-500/70 bg-amber-500/10"
                  : "border-slate-700/50 bg-slate-800/50"
              }`}
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, delay: days.indexOf(day) * 0.05 }}
              whileHover={
                day === pendingDay
                  ? {
                      y: -4,
                    }
                  : {}
              }
            >
              {day === pendingDay && (
                <>
                  <motion.div
                    className="absolute inset-0 rounded-lg"
                    animate={{
                      borderWidth: ["2px", "3px", "2px"],
                      borderColor: [
                        "rgba(245, 158, 11, 0.7)",
                        "rgba(245, 158, 11, 1)",
                        "rgba(245, 158, 11, 0.7)",
                      ],
                      backgroundColor: [
                        "rgba(245, 158, 11, 0.1)",
                        "rgba(251, 191, 36, 0.15)",
                        "rgba(245, 158, 11, 0.1)",
                      ],
                    }}
                    transition={{ duration: 1.5, repeat: Infinity, ease: "easeInOut" }}
                  />
                  <motion.div
                    className="absolute -bottom-6 left-1/2 -translate-x-1/2 whitespace-nowrap rounded-full bg-amber-500/90 px-2 py-0.5 text-[10px] font-medium text-slate-900 opacity-0"
                    whileHover={{ opacity: 1, y: -2 }}
                    transition={{ duration: 0.2 }}
                  >
                    Pending Approval
                  </motion.div>
                </>
              )}
              <span className="relative z-10 text-lg font-bold text-slate-100">{day}</span>
              {day === pendingDay && (
                <span className="relative z-10 text-[10px] font-medium text-amber-400">Pending</span>
              )}
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 4. Calendar Header Slide
export function GfcCalendarHeaderSlide() {
  const weekdays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        {/* Header */}
        <motion.div
          className="mb-4 flex items-center justify-between"
          initial={{ opacity: 0, y: -12 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.4 }}
          whileHover={{ backgroundColor: "rgba(30, 41, 59, 0.5)" }}
        >
          <motion.button
            className="rounded-lg p-1 text-slate-400 transition-colors hover:text-slate-200"
            whileHover={{ x: -2 }}
            whileTap={{ scale: 0.95 }}
          >
            <ChevronLeft className="h-5 w-5" />
          </motion.button>
          <h3 className="text-lg font-semibold text-slate-100">March 2026</h3>
          <motion.button
            className="rounded-lg p-1 text-slate-400 transition-colors hover:text-slate-200"
            whileHover={{ x: 2 }}
            whileTap={{ scale: 0.95 }}
          >
            <ChevronRight className="h-5 w-5" />
          </motion.button>
        </motion.div>

        {/* Weekday labels */}
        <div className="grid grid-cols-7 gap-1">
          {weekdays.map((day, index) => (
            <motion.div
              key={day}
              className="py-2 text-center text-xs font-medium text-slate-400"
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, delay: index * 0.05 }}
            >
              {day}
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 5. Month Switch Animation
export function GfcCalendarMonthSwitch() {
  const [currentIndex, setCurrentIndex] = useState(0);
  const months = [
    { name: "March 2026", days: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14] },
    { name: "April 2026", days: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14] },
  ];

  const currentMonth = months[currentIndex];

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        {/* Month header with arrows */}
        <div className="mb-4 flex items-center justify-between">
          <motion.button
            onClick={() => setCurrentIndex((prev) => (prev === 0 ? 1 : 0))}
            className="rounded-lg p-1 text-slate-400 transition-colors hover:text-slate-200"
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
          >
            <ChevronLeft className="h-5 w-5" />
          </motion.button>

          <AnimatePresence mode="wait">
            <motion.h3
              key={currentIndex}
              className="text-lg font-semibold text-slate-100"
              initial={{ opacity: 0, y: -10 }}
              animate={{ opacity: 1, y: 0 }}
              exit={{ opacity: 0, y: 10 }}
              transition={{ duration: 0.3 }}
            >
              {currentMonth.name}
            </motion.h3>
          </AnimatePresence>

          <motion.button
            onClick={() => setCurrentIndex((prev) => (prev === 0 ? 1 : 0))}
            className="rounded-lg p-1 text-slate-400 transition-colors hover:text-slate-200"
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
          >
            <ChevronRight className="h-5 w-5" />
          </motion.button>
        </div>

        {/* Calendar grid */}
        <div className="relative min-h-[200px]">
          <AnimatePresence mode="wait">
            <motion.div
              key={currentIndex}
              className="grid grid-cols-7 gap-1"
              initial={{ opacity: 0, x: 20 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -20 }}
              transition={{ duration: 0.4 }}
            >
              {currentMonth.days.map((day) => (
                <div
                  key={day}
                  className="flex h-8 items-center justify-center rounded border border-slate-700/30 bg-slate-800/50 text-xs text-slate-300"
                >
                  {day}
                </div>
              ))}
            </motion.div>
          </AnimatePresence>
        </div>
      </div>
    </div>
  );
}

// 6. Highlighted Range Sweep
export function GfcCalendarRangeHighlight() {
  const days = [5, 6, 7, 8, 9, 10, 11, 12, 13, 14];
  const rangeStart = 7;
  const rangeEnd = 11;
  const rangeDays = days.filter((d) => d >= rangeStart && d <= rangeEnd);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-lg rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Multi-Day Rental</h3>
        <div className="relative flex justify-center gap-2">
          {/* Day tiles */}
          {days.map((day, index) => {
            const isInRange = day >= rangeStart && day <= rangeEnd;
            const isRangeStart = day === rangeStart;
            const isRangeEnd = day === rangeEnd;
            
            return (
              <div key={day} className="relative">
                {/* Highlight bar - only on range start */}
                {isRangeStart && (
                  <motion.div
                    className="absolute top-0 h-16 rounded-lg bg-amber-500/30 -z-0"
                    initial={{ width: 0 }}
                    animate={{
                      width: `${((rangeEnd - rangeStart + 1) * 64 + (rangeEnd - rangeStart) * 8)}px`,
                    }}
                    transition={{ duration: 0.8, ease: "easeOut" }}
                    style={{ left: 0 }}
                  />
                )}
                
                <motion.div
                  className={`relative z-10 flex h-16 w-16 flex-col items-center justify-center rounded-lg border transition-all ${
                    isInRange
                      ? "border-amber-500/50 bg-amber-500/20"
                      : "border-slate-700/50 bg-slate-800/50"
                  }`}
                  initial={{ opacity: 0, y: 10 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ duration: 0.3, delay: index * 0.05 }}
                  whileHover={
                    isInRange
                      ? {
                          y: -4,
                          backgroundColor: "rgba(245, 158, 11, 0.3)",
                          boxShadow: "0 10px 25px rgba(245, 158, 11, 0.3)",
                        }
                      : {}
                  }
                >
                  <span className="text-lg font-bold text-slate-100">{day}</span>
                </motion.div>
              </div>
            );
          })}
        </div>

        {/* Range label */}
        <motion.div
          className="mt-6 text-center"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.8 }}
        >
          <motion.span
            className="inline-block rounded-full bg-amber-500/90 px-3 py-1 text-xs font-medium text-slate-900"
            whileHover={{ scale: 1.05 }}
          >
            Rental Range: {rangeStart} - {rangeEnd}
          </motion.span>
        </motion.div>
      </div>
    </div>
  );
}

