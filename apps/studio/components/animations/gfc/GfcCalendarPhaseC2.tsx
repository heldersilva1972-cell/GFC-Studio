"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Lock, Calendar, Flag, Check, X, AlertCircle, Clock } from "lucide-react";

// 1. CalendarPageFlip
export function CalendarPageFlip() {
  const [flipped, setFlipped] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setFlipped((prev) => !prev);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Page Flip</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative h-32 w-24 rounded-lg border-2 border-amber-500/50 bg-gradient-to-br from-amber-900/30 to-amber-700/20"
            animate={{ rotateY: flipped ? 180 : 0 }}
            transition={{ duration: 0.6, ease: "easeInOut" }}
            style={{ transformStyle: "preserve-3d" }}
          >
            <div className="absolute inset-0 flex items-center justify-center text-amber-300">
              <span className="text-2xl font-bold">Nov</span>
            </div>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 2. CalendarDatePulse
export function CalendarDatePulse() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Date Pulse</h3>
        <div className="flex justify-center gap-2">
          {[15, 16, 17].map((day) => (
            <motion.div
              key={day}
              className="flex h-12 w-12 items-center justify-center rounded-lg border border-blue-500/50 bg-blue-500/10"
              animate={{
                scale: [1, 1.1, 1],
                boxShadow: [
                  "0 0 0px rgba(59, 130, 246, 0.4)",
                  "0 0 15px rgba(59, 130, 246, 0.6)",
                  "0 0 0px rgba(59, 130, 246, 0.4)",
                ],
              }}
              transition={{ duration: 1.5, repeat: Infinity, delay: day * 0.1 }}
            >
              <span className="text-sm font-bold text-blue-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 3. CalendarDayGlow
export function CalendarDayGlow() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Day Glow</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-purple-500/50 bg-purple-500/10"
            animate={{
              boxShadow: [
                "0 0 10px rgba(168, 85, 247, 0.4)",
                "0 0 30px rgba(168, 85, 247, 0.8)",
                "0 0 10px rgba(168, 85, 247, 0.4)",
              ],
            }}
            transition={{ duration: 2, repeat: Infinity }}
          >
            <span className="text-lg font-bold text-purple-300">20</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 4. CalendarWeekSlide
export function CalendarWeekSlide() {
  const [week, setWeek] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setWeek((prev) => (prev + 1) % 4);
    }, 2000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Week Slide</h3>
        <div className="flex justify-center gap-1 overflow-hidden">
          {[1, 2, 3, 4].map((w) => (
            <motion.div
              key={w}
              className="flex h-10 w-10 items-center justify-center rounded border border-slate-600 bg-slate-800"
              animate={{ x: week * -44 }}
              transition={{ duration: 0.5, ease: "easeInOut" }}
            >
              <span className="text-xs text-slate-300">W{w}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 5. CalendarPopInEvent
export function CalendarPopInEvent() {
  const [showEvent, setShowEvent] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setShowEvent(true);
      setTimeout(() => setShowEvent(false), 1500);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Pop In Event</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">22</span>
            <AnimatePresence>
              {showEvent && (
                <motion.div
                  className="absolute -top-2 -right-2 h-4 w-4 rounded-full bg-red-500"
                  initial={{ scale: 0, opacity: 0 }}
                  animate={{ scale: 1, opacity: 1 }}
                  exit={{ scale: 0, opacity: 0 }}
                  transition={{ type: "spring", stiffness: 300 }}
                />
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 6. CalendarBookedStamp
export function CalendarBookedStamp() {
  const [stamped, setStamped] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setStamped(true);
      setTimeout(() => setStamped(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Booked Stamp</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">23</span>
            <AnimatePresence>
              {stamped && (
                <motion.div
                  className="absolute inset-0 flex items-center justify-center rounded-lg bg-red-500/20"
                  initial={{ scale: 0, rotate: -10 }}
                  animate={{ scale: 1, rotate: 0 }}
                  exit={{ scale: 0, rotate: 10 }}
                  transition={{ type: "spring", stiffness: 200 }}
                >
                  <Lock className="h-6 w-6 text-red-400" />
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 7. CalendarAvailableFade
export function CalendarAvailableFade() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Available Fade</h3>
        <div className="flex justify-center gap-2">
          {[24, 25, 26].map((day, i) => (
            <motion.div
              key={day}
              className="flex h-12 w-12 items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/10"
              initial={{ opacity: 0 }}
              animate={{ opacity: [0, 1, 0.7, 1] }}
              transition={{ duration: 2, repeat: Infinity, delay: i * 0.3 }}
            >
              <span className="text-sm font-bold text-emerald-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 8. CalendarDragAddEvent
export function CalendarDragAddEvent() {
  const [dragging, setDragging] = useState(false);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Drag Add Event</h3>
        <div className="flex justify-center gap-2">
          <motion.div
            className="flex h-12 w-12 cursor-grab items-center justify-center rounded-lg border-2 border-dashed border-blue-500/50 bg-blue-500/10"
            drag
            dragConstraints={{ left: 0, right: 0, top: 0, bottom: 0 }}
            onDragStart={() => setDragging(true)}
            onDragEnd={() => setDragging(false)}
            animate={{ scale: dragging ? 1.1 : 1 }}
          >
            <Calendar className="h-5 w-5 text-blue-300" />
          </motion.div>
          <motion.div
            className="flex h-12 w-12 items-center justify-center rounded-lg border border-slate-600 bg-slate-800"
            animate={{ scale: dragging ? 1.05 : 1, borderColor: dragging ? "rgba(59, 130, 246, 0.5)" : undefined }}
          >
            <span className="text-sm font-bold text-slate-300">27</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 9. CalendarBounceDayMarker
export function CalendarBounceDayMarker() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Bounce Day Marker</h3>
        <div className="flex justify-center">
          <motion.div
            className="flex h-16 w-16 items-center justify-center rounded-lg border-2 border-cyan-500/50 bg-cyan-500/10"
            animate={{ y: [0, -10, 0] }}
            transition={{ duration: 0.6, repeat: Infinity, ease: "easeInOut" }}
          >
            <span className="text-lg font-bold text-cyan-300">28</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 10. CalendarSwipeMonth
export function CalendarSwipeMonth() {
  const [month, setMonth] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setMonth((prev) => (prev + 1) % 2);
    }, 2500);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Swipe Month</h3>
        <div className="relative flex h-20 items-center justify-center overflow-hidden">
          <AnimatePresence mode="wait">
            <motion.div
              key={month}
              className="absolute"
              initial={{ x: month === 0 ? -100 : 100, opacity: 0 }}
              animate={{ x: 0, opacity: 1 }}
              exit={{ x: month === 0 ? 100 : -100, opacity: 0 }}
              transition={{ duration: 0.4 }}
            >
              <span className="text-2xl font-bold text-slate-200">{month === 0 ? "November" : "December"}</span>
            </motion.div>
          </AnimatePresence>
        </div>
      </div>
    </div>
  );
}

// 11. CalendarPeelBackReveal
export function CalendarPeelBackReveal() {
  const [peeled, setPeeled] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setPeeled(true);
      setTimeout(() => setPeeled(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Peel Back Reveal</h3>
        <div className="flex justify-center">
          <div className="relative h-20 w-20">
            <motion.div
              className="absolute inset-0 flex items-center justify-center rounded-lg border border-slate-600 bg-slate-800"
              animate={{ rotateX: peeled ? -90 : 0 }}
              transition={{ duration: 0.6 }}
              style={{ transformStyle: "preserve-3d", transformOrigin: "top" }}
            >
              <span className="text-lg font-bold text-slate-300">29</span>
            </motion.div>
            {peeled && (
              <motion.div
                className="absolute inset-0 flex items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/10"
                initial={{ rotateX: 90 }}
                animate={{ rotateX: 0 }}
                transition={{ duration: 0.6, delay: 0.3 }}
                style={{ transformStyle: "preserve-3d", transformOrigin: "top" }}
              >
                <Check className="h-6 w-6 text-emerald-400" />
              </motion.div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

// 12. CalendarHighlightRange
export function CalendarHighlightRange() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Highlight Range</h3>
        <div className="flex justify-center gap-1">
          {[30, 1, 2, 3, 4].map((day, i) => (
            <motion.div
              key={`${day}-${i}`}
              className="flex h-12 w-12 items-center justify-center rounded-lg border"
              initial={{ backgroundColor: "rgba(30, 41, 59, 0.5)", borderColor: "rgba(71, 85, 105, 0.5)" }}
              animate={{
                backgroundColor: i >= 1 && i <= 3 ? "rgba(59, 130, 246, 0.2)" : "rgba(30, 41, 59, 0.5)",
                borderColor: i >= 1 && i <= 3 ? "rgba(59, 130, 246, 0.5)" : "rgba(71, 85, 105, 0.5)",
                scale: i >= 1 && i <= 3 ? 1.05 : 1,
              }}
              transition={{ duration: 0.3, delay: i * 0.1 }}
            >
              <span className={`text-sm font-bold ${i >= 1 && i <= 3 ? "text-blue-300" : "text-slate-300"}`}>{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 13. CalendarDotPop
export function CalendarDotPop() {
  const [popped, setPopped] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setPopped(true);
      setTimeout(() => setPopped(false), 800);
    }, 2500);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Dot Pop</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">5</span>
            <AnimatePresence>
              {popped && (
                <motion.div
                  className="absolute -top-1 -right-1 h-3 w-3 rounded-full bg-orange-500"
                  initial={{ scale: 0 }}
                  animate={{ scale: [0, 1.5, 1] }}
                  exit={{ scale: 0 }}
                  transition={{ duration: 0.4, type: "spring" }}
                />
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 14. CalendarEventRibbon
export function CalendarEventRibbon() {
  const [showRibbon, setShowRibbon] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setShowRibbon(true);
      setTimeout(() => setShowRibbon(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Ribbon</h3>
        <div className="flex justify-center">
          <div className="relative h-16 w-16 overflow-hidden rounded-lg border border-slate-600 bg-slate-800">
            <span className="absolute inset-0 flex items-center justify-center text-lg font-bold text-slate-300">6</span>
            <AnimatePresence>
              {showRibbon && (
                <motion.div
                  className="absolute top-0 right-0 h-6 w-16 origin-top-right bg-gradient-to-br from-red-500 to-red-600"
                  initial={{ scaleX: 0, scaleY: 0 }}
                  animate={{ scaleX: 1, scaleY: 1 }}
                  exit={{ scaleX: 0, scaleY: 0 }}
                  transition={{ duration: 0.4 }}
                  style={{ clipPath: "polygon(0 0, 100% 0, 100% 100%)" }}
                >
                  <span className="absolute top-1 right-1 text-[8px] font-bold text-white">EVENT</span>
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 15. CalendarMonthCarousel
export function CalendarMonthCarousel() {
  const [index, setIndex] = useState(0);
  const months = ["Oct", "Nov", "Dec"];

  useEffect(() => {
    const interval = setInterval(() => {
      setIndex((prev) => (prev + 1) % months.length);
    }, 2000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Month Carousel</h3>
        <div className="relative flex h-12 items-center justify-center overflow-hidden">
          <AnimatePresence mode="wait">
            <motion.div
              key={index}
              initial={{ x: 50, opacity: 0 }}
              animate={{ x: 0, opacity: 1 }}
              exit={{ x: -50, opacity: 0 }}
              transition={{ duration: 0.4 }}
            >
              <span className="text-xl font-bold text-slate-200">{months[index]}</span>
            </motion.div>
          </AnimatePresence>
        </div>
      </div>
    </div>
  );
}

// 16. CalendarFogUnavailable
export function CalendarFogUnavailable() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Fog Unavailable</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">7</span>
            <motion.div
              className="absolute inset-0 rounded-lg bg-slate-900/60 backdrop-blur-sm"
              animate={{ opacity: [0.4, 0.7, 0.4] }}
              transition={{ duration: 2, repeat: Infinity }}
            />
          </div>
        </div>
      </div>
    </div>
  );
}

// 17. CalendarFlashAvailable
export function CalendarFlashAvailable() {
  const [flash, setFlash] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setFlash(true);
      setTimeout(() => setFlash(false), 300);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Flash Available</h3>
        <div className="flex justify-center">
          <motion.div
            className="flex h-16 w-16 items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/10"
            animate={flash ? { backgroundColor: "rgba(16, 185, 129, 0.4)", scale: 1.1 } : {}}
            transition={{ duration: 0.3 }}
          >
            <span className="text-lg font-bold text-emerald-300">8</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 18. CalendarEventPing
export function CalendarEventPing() {
  const [ping, setPing] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setPing(true);
      setTimeout(() => setPing(false), 1000);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Ping</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="relative z-10 text-lg font-bold text-slate-300">9</span>
            {ping && (
              <motion.div
                className="absolute inset-0 rounded-lg border-2 border-red-500"
                initial={{ scale: 1, opacity: 1 }}
                animate={{ scale: 2, opacity: 0 }}
                transition={{ duration: 0.8 }}
              />
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

// 19. CalendarEventDropIn
export function CalendarEventDropIn() {
  const [dropped, setDropped] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setDropped(false);
      setTimeout(() => setDropped(true), 100);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Drop In</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">10</span>
            <AnimatePresence>
              {dropped && (
                <motion.div
                  className="absolute -top-2 -right-2 h-5 w-5 rounded-full bg-red-500"
                  initial={{ y: -20, opacity: 0, scale: 0 }}
                  animate={{ y: 0, opacity: 1, scale: 1 }}
                  exit={{ y: 10, opacity: 0, scale: 0 }}
                  transition={{ type: "spring", stiffness: 300, damping: 20 }}
                />
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 20. CalendarAvailabilityChecker
export function CalendarAvailabilityChecker() {
  const [checking, setChecking] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setChecking(true);
      setTimeout(() => setChecking(false), 1500);
    }, 3500);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Availability Checker</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">11</span>
            {checking && (
              <motion.div
                className="absolute inset-0 flex items-center justify-center rounded-lg bg-emerald-500/20"
                initial={{ scale: 0.8, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                exit={{ scale: 0.8, opacity: 0 }}
              >
                <Check className="h-6 w-6 text-emerald-400" />
              </motion.div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

// 21. CalendarTapToAdd
export function CalendarTapToAdd() {
  const [tapped, setTapped] = useState(false);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Tap To Add</h3>
        <div className="flex justify-center">
          <motion.div
            className="flex h-16 w-16 cursor-pointer items-center justify-center rounded-lg border-2 border-dashed border-blue-500/50 bg-blue-500/10"
            onClick={() => {
              setTapped(true);
              setTimeout(() => setTapped(false), 1000);
            }}
            animate={{ scale: tapped ? 0.9 : 1 }}
            whileTap={{ scale: 0.85 }}
          >
            <span className="text-lg font-bold text-blue-300">12</span>
            {tapped && (
              <motion.div
                className="absolute inset-0 flex items-center justify-center rounded-lg bg-blue-500/30"
                initial={{ scale: 0 }}
                animate={{ scale: 1.2, opacity: 0 }}
                transition={{ duration: 0.4 }}
              />
            )}
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 22. CalendarHeatMapGlow
export function CalendarHeatMapGlow() {
  const intensities = [0.3, 0.6, 0.9, 0.5, 0.8];

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Heat Map Glow</h3>
        <div className="flex justify-center gap-1">
          {intensities.map((intensity, i) => (
            <motion.div
              key={i}
              className="flex h-12 w-12 items-center justify-center rounded-lg border"
              animate={{
                backgroundColor: `rgba(239, 68, 68, ${intensity * 0.3})`,
                borderColor: `rgba(239, 68, 68, ${intensity * 0.5})`,
                boxShadow: `0 0 ${intensity * 15}px rgba(239, 68, 68, ${intensity * 0.6})`,
              }}
              transition={{ duration: 2, repeat: Infinity, delay: i * 0.2 }}
            >
              <span className="text-sm font-bold text-red-300">{13 + i}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 23. CalendarSoftWaveTransition
export function CalendarSoftWaveTransition() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Soft Wave Transition</h3>
        <div className="flex justify-center">
          <motion.div
            className="flex h-16 w-16 items-center justify-center rounded-lg border border-indigo-500/50 bg-indigo-500/10"
            animate={{
              y: [0, -5, 0],
              boxShadow: [
                "0 0 0px rgba(99, 102, 241, 0.4)",
                "0 5px 20px rgba(99, 102, 241, 0.6)",
                "0 0 0px rgba(99, 102, 241, 0.4)",
              ],
            }}
            transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
          >
            <span className="text-lg font-bold text-indigo-300">18</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 24. CalendarEventFlagRise
export function CalendarEventFlagRise() {
  const [flagUp, setFlagUp] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setFlagUp(true);
      setTimeout(() => setFlagUp(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Flag Rise</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">19</span>
            <AnimatePresence>
              {flagUp && (
                <motion.div
                  className="absolute -top-3 right-0"
                  initial={{ y: 10, opacity: 0 }}
                  animate={{ y: 0, opacity: 1 }}
                  exit={{ y: 10, opacity: 0 }}
                  transition={{ duration: 0.4 }}
                >
                  <Flag className="h-5 w-5 text-red-500" />
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 25. CalendarBookedLockIn
export function CalendarBookedLockIn() {
  const [locked, setLocked] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setLocked(true);
      setTimeout(() => setLocked(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Booked Lock In</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800"
            animate={locked ? { borderColor: "rgba(239, 68, 68, 0.5)", backgroundColor: "rgba(239, 68, 68, 0.1)" } : {}}
            transition={{ duration: 0.3 }}
          >
            <span className="text-lg font-bold text-slate-300">20</span>
            <AnimatePresence>
              {locked && (
                <motion.div
                  className="absolute inset-0 flex items-center justify-center"
                  initial={{ scale: 0, rotate: -90 }}
                  animate={{ scale: 1, rotate: 0 }}
                  exit={{ scale: 0, rotate: 90 }}
                  transition={{ type: "spring", stiffness: 200 }}
                >
                  <Lock className="h-6 w-6 text-red-400" />
                </motion.div>
              )}
            </AnimatePresence>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

