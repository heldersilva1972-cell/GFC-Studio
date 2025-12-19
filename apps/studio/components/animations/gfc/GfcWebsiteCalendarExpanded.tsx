"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Lock, AlertCircle, X, Check, Calendar, Edit } from "lucide-react";

// ============================================
// AVAILABILITY INDICATORS (7)
// ============================================

// 1. AvailableDayPulse
export function GfcCalendarAvailableDayPulse() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Available Days</h3>
        <div className="flex justify-center gap-2">
          {[12, 13, 14].map((day) => (
            <motion.div
              key={day}
              className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-emerald-500/50 bg-emerald-500/10"
              animate={{
                boxShadow: [
                  "0 0 0px rgba(16, 185, 129, 0.4)",
                  "0 0 20px rgba(16, 185, 129, 0.6)",
                  "0 0 0px rgba(16, 185, 129, 0.4)",
                ],
                scale: [1, 1.02, 1],
              }}
              transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
            >
              <span className="text-lg font-bold text-emerald-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 2. AvailableHoverLift
export function GfcCalendarAvailableHoverLift() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Hover to Lift</h3>
        <div className="flex justify-center gap-2">
          {[15, 16, 17].map((day) => (
            <motion.div
              key={day}
              className="flex h-16 w-16 items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/10"
              whileHover={{
                y: -8,
                boxShadow: "0 10px 25px rgba(16, 185, 129, 0.4)",
                scale: 1.05,
              }}
              transition={{ type: "spring", stiffness: 300, damping: 20 }}
            >
              <span className="text-lg font-bold text-emerald-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 3. SoftAvailabilityRipple
export function GfcCalendarSoftAvailabilityRipple() {
  const [ripple, setRipple] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setRipple(true);
      setTimeout(() => setRipple(false), 1000);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Availability Ripple</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/10">
            {ripple && (
              <motion.div
                className="absolute inset-0 rounded-lg border-2 border-emerald-400"
                initial={{ scale: 1, opacity: 0.8 }}
                animate={{ scale: 2, opacity: 0 }}
                transition={{ duration: 1, ease: "easeOut" }}
              />
            )}
            <span className="relative z-10 text-lg font-bold text-emerald-300">18</span>
          </div>
        </div>
      </div>
    </div>
  );
}

// 4. AvailabilityGradientSweep
export function GfcCalendarAvailabilityGradientSweep() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Gradient Sweep</h3>
        <div className="flex justify-center gap-2">
          {[19, 20, 21].map((day) => (
            <motion.div
              key={day}
              className="relative flex h-16 w-16 items-center justify-center overflow-hidden rounded-lg border border-emerald-500/50 bg-slate-800/50"
            >
              <motion.div
                className="absolute inset-0 bg-gradient-to-br from-emerald-500/0 via-emerald-500/40 to-emerald-500/0"
                animate={{
                  x: ["-100%", "200%"],
                }}
                transition={{ duration: 2, repeat: Infinity, ease: "linear" }}
              />
              <span className="relative z-10 text-lg font-bold text-emerald-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 5. FaintCheckmarkReveal
export function GfcCalendarFaintCheckmarkReveal() {
  const [showCheck, setShowCheck] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setShowCheck(true);
      setTimeout(() => setShowCheck(false), 1500);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Checkmark Reveal</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/10">
            <AnimatePresence>
              {showCheck && (
                <motion.div
                  initial={{ scale: 0, opacity: 0 }}
                  animate={{ scale: 1, opacity: 0.6 }}
                  exit={{ scale: 0.5, opacity: 0 }}
                  transition={{ duration: 0.3 }}
                  className="absolute"
                >
                  <Check className="h-6 w-6 text-emerald-400" />
                </motion.div>
              )}
            </AnimatePresence>
            <span className="text-lg font-bold text-emerald-300">22</span>
          </div>
        </div>
      </div>
    </div>
  );
}

// 6. MorningEveningColorSplit
export function GfcCalendarMorningEveningColorSplit() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">AM/PM Split</h3>
        <div className="flex justify-center gap-2">
          {[23, 24, 25].map((day) => (
            <motion.div
              key={day}
              className="relative flex h-16 w-16 flex-col overflow-hidden rounded-lg border border-emerald-500/50"
            >
              <div className="h-1/2 bg-emerald-500/20" />
              <div className="h-1/2 bg-blue-500/20" />
              <span className="absolute inset-0 flex items-center justify-center text-lg font-bold text-slate-100">
                {day}
              </span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 7. PriorityDateGlow
export function GfcCalendarPriorityDateGlow() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Priority Dates</h3>
        <div className="flex justify-center gap-2">
          {[26, 27, 28].map((day) => (
            <motion.div
              key={day}
              className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-amber-500/50 bg-amber-500/10"
              animate={{
                boxShadow: [
                  "0 0 10px rgba(245, 158, 11, 0.3)",
                  "0 0 25px rgba(245, 158, 11, 0.6)",
                  "0 0 10px rgba(245, 158, 11, 0.3)",
                ],
              }}
              transition={{ duration: 2.5, repeat: Infinity, ease: "easeInOut" }}
            >
              <span className="text-lg font-bold text-amber-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// ============================================
// BOOKED / CONFLICT STATES (7)
// ============================================

// 8. BookedDayLockIn
export function GfcCalendarBookedDayLockIn() {
  const [locked, setLocked] = useState(false);

  useEffect(() => {
    const timer = setTimeout(() => setLocked(true), 500);
    return () => clearTimeout(timer);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Booked Day</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-red-500/50 bg-red-900/30"
            initial={{ backgroundColor: "rgba(51, 65, 85, 0.5)" }}
            animate={{ backgroundColor: locked ? "rgba(127, 29, 29, 0.3)" : "rgba(51, 65, 85, 0.5)" }}
            transition={{ duration: 0.5 }}
          >
            <motion.div
              initial={{ scale: 0, rotate: -90 }}
              animate={{ scale: locked ? 1 : 0, rotate: locked ? 0 : -90 }}
              transition={{ duration: 0.4, delay: 0.2 }}
            >
              <Lock className="h-6 w-6 text-red-400" />
            </motion.div>
            <span className="absolute bottom-1 text-[10px] font-medium text-red-400">29</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 9. ConflictShakeRed
export function GfcCalendarConflictShakeRed() {
  const [shake, setShake] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setShake(true);
      setTimeout(() => setShake(false), 600);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Conflict Detected</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-red-500 bg-red-900/40"
            animate={shake ? { x: [0, -8, 8, -8, 8, 0] } : {}}
            transition={{ duration: 0.3 }}
          >
            <motion.div
              animate={shake ? { opacity: [1, 0.3, 1] } : { opacity: 1 }}
              transition={{ duration: 0.3 }}
              className="absolute inset-0 rounded-lg bg-red-500/50"
            />
            <span className="relative z-10 text-lg font-bold text-red-200">30</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 10. FullDayFlashWarning
export function GfcCalendarFullDayFlashWarning() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Full Day Warning</h3>
        <div className="flex justify-center gap-2">
          {[1, 2, 3].map((day) => (
            <motion.div
              key={day}
              className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-orange-500/50 bg-orange-900/30"
              animate={{
                backgroundColor: [
                  "rgba(154, 52, 18, 0.3)",
                  "rgba(251, 146, 60, 0.5)",
                  "rgba(154, 52, 18, 0.3)",
                ],
                boxShadow: [
                  "0 0 0px rgba(251, 146, 60, 0)",
                  "0 0 20px rgba(251, 146, 60, 0.8)",
                  "0 0 0px rgba(251, 146, 60, 0)",
                ],
              }}
              transition={{ duration: 1.5, repeat: Infinity, ease: "easeInOut" }}
            >
              <span className="text-lg font-bold text-orange-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 11. BlockedDateStripe
export function GfcCalendarBlockedDateStripe() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Blocked Date</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center overflow-hidden rounded-lg border-2 border-red-500/50 bg-red-900/30">
            <motion.div
              className="absolute inset-0"
              style={{
                background: "repeating-linear-gradient(45deg, transparent, transparent 4px, rgba(239, 68, 68, 0.3) 4px, rgba(239, 68, 68, 0.3) 8px)",
              }}
              animate={{
                x: ["-100%", "100%"],
              }}
              transition={{ duration: 1.5, repeat: Infinity, ease: "linear" }}
            />
            <span className="relative z-10 text-lg font-bold text-red-200">4</span>
          </div>
        </div>
      </div>
    </div>
  );
}

// 12. GapRuleViolationPulse
export function GfcCalendarGapRuleViolationPulse() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Gap Rule Violation</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-yellow-500/50 bg-yellow-900/30"
            animate={{
              scale: [1, 1.1, 1],
              boxShadow: [
                "0 0 0px rgba(234, 179, 8, 0)",
                "0 0 20px rgba(234, 179, 8, 0.6)",
                "0 0 0px rgba(234, 179, 8, 0)",
              ],
            }}
            transition={{ duration: 1.2, repeat: Infinity, ease: "easeInOut" }}
          >
            <AlertCircle className="absolute -top-1 -right-1 h-5 w-5 text-yellow-400" />
            <span className="text-lg font-bold text-yellow-300">5</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 13. EventOverlapSlideAlert
export function GfcCalendarEventOverlapSlideAlert() {
  const [overlap, setOverlap] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setOverlap(true);
      setTimeout(() => setOverlap(false), 1000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Overlap</h3>
        <div className="relative flex justify-center gap-1">
          <motion.div
            className="flex h-16 w-16 items-center justify-center rounded-lg border-2 border-red-500/50 bg-red-900/30"
            animate={overlap ? { x: [0, 8, 0] } : {}}
            transition={{ duration: 0.5, ease: "easeInOut" }}
          >
            <span className="text-lg font-bold text-red-200">6</span>
          </motion.div>
          <motion.div
            className="flex h-16 w-16 items-center justify-center rounded-lg border-2 border-red-500/50 bg-red-900/30"
            animate={overlap ? { x: [0, -8, 0] } : {}}
            transition={{ duration: 0.5, ease: "easeInOut" }}
          >
            <span className="text-lg font-bold text-red-200">7</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 14. HolidaySealStamp
export function GfcCalendarHolidaySealStamp() {
  const [stamped, setStamped] = useState(false);

  useEffect(() => {
    const timer = setTimeout(() => setStamped(true), 500);
    return () => clearTimeout(timer);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Holiday Closure</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-amber-500/50 bg-amber-900/20"
          >
            <motion.div
              className="absolute inset-0 flex items-center justify-center"
              initial={{ scale: 0, rotate: -15 }}
              animate={stamped ? { scale: 1, rotate: 5 } : { scale: 0, rotate: -15 }}
              transition={{ type: "spring", stiffness: 300, damping: 15 }}
            >
              <div className="rounded-full border-2 border-amber-400 bg-amber-500/20 px-2 py-1 text-[8px] font-bold text-amber-200">
                HOLIDAY
              </div>
            </motion.div>
            <span className="text-lg font-bold text-slate-300">25</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// ============================================
// EVENT CREATION FLOW (5)
// ============================================

// 15. AddEventSlideUpForm
export function GfcCalendarAddEventSlideUpForm() {
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setShowForm(true);
      setTimeout(() => setShowForm(false), 2500);
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="relative w-full max-w-md overflow-hidden rounded-xl border border-slate-700/50 bg-slate-900/80 shadow-xl">
        <div className="p-6">
          <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Add Event</h3>
          <div className="flex justify-center">
            <div className="flex h-16 w-16 items-center justify-center rounded-lg border border-slate-700/50 bg-slate-800/50">
              <span className="text-lg font-bold text-slate-300">8</span>
            </div>
          </div>
        </div>
        <AnimatePresence>
          {showForm && (
            <motion.div
              className="absolute bottom-0 left-0 right-0 border-t border-slate-700/50 bg-slate-800/95 p-4"
              initial={{ y: "100%" }}
              animate={{ y: 0 }}
              exit={{ y: "100%" }}
              transition={{ type: "spring", stiffness: 300, damping: 30 }}
            >
              <div className="space-y-2">
                <div className="h-8 rounded bg-slate-700/50" />
                <div className="h-8 rounded bg-slate-700/50" />
                <div className="h-8 rounded bg-emerald-500/80" />
              </div>
            </motion.div>
          )}
        </AnimatePresence>
      </div>
    </div>
  );
}

// 16. AddEventTargetHighlight
export function GfcCalendarAddEventTargetHighlight() {
  const [highlighted, setHighlighted] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setHighlighted(true);
      setTimeout(() => setHighlighted(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Target Highlight</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-slate-700/50 bg-slate-800/50"
            animate={highlighted ? { scale: 1.15 } : { scale: 1 }}
            transition={{ duration: 0.3 }}
          >
            {highlighted && (
              <motion.div
                className="absolute inset-0 rounded-lg border-2 border-emerald-400"
                initial={{ scale: 1, opacity: 0.8 }}
                animate={{ scale: 1.3, opacity: 0 }}
                transition={{ duration: 0.6, repeat: Infinity }}
              />
            )}
            <span className="relative z-10 text-lg font-bold text-slate-300">9</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 17. EventCreatedSuccessBurst
export function GfcCalendarEventCreatedSuccessBurst() {
  const [burst, setBurst] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setBurst(true);
      setTimeout(() => setBurst(false), 1000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Created</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/20">
            {burst && (
              <>
                {[...Array(8)].map((_, i) => (
                  <motion.div
                    key={i}
                    className="absolute h-2 w-2 rounded-full bg-emerald-400"
                    initial={{ x: 0, y: 0, scale: 0, opacity: 1 }}
                    animate={{
                      x: Math.cos((i * Math.PI * 2) / 8) * 30,
                      y: Math.sin((i * Math.PI * 2) / 8) * 30,
                      scale: [0, 1, 0],
                      opacity: [1, 1, 0],
                    }}
                    transition={{ duration: 0.8, ease: "easeOut" }}
                  />
                ))}
              </>
            )}
            <span className="relative z-10 text-lg font-bold text-emerald-300">10</span>
          </div>
        </div>
      </div>
    </div>
  );
}

// 18. EventDeniedShake
export function GfcCalendarEventDeniedShake() {
  const [denied, setDenied] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setDenied(true);
      setTimeout(() => setDenied(false), 1000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Denied</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-red-500/50 bg-red-900/30"
            animate={denied ? { y: [0, -10, 0], x: [0, -4, 4, -4, 4, 0] } : {}}
            transition={{ duration: 0.5 }}
          >
            <AnimatePresence>
              {denied && (
                <motion.div
                  initial={{ scale: 0, y: -10 }}
                  animate={{ scale: 1, y: 0 }}
                  exit={{ scale: 0, y: 10 }}
                  transition={{ duration: 0.3 }}
                  className="absolute"
                >
                  <X className="h-8 w-8 text-red-400" />
                </motion.div>
              )}
            </AnimatePresence>
            <span className="text-lg font-bold text-red-200">11</span>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 19. EventSavedRibbon
export function GfcCalendarEventSavedRibbon() {
  const [saved, setSaved] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setSaved(true);
      setTimeout(() => setSaved(false), 2000);
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Saved</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-emerald-500/50 bg-emerald-500/20">
            <AnimatePresence>
              {saved && (
                <motion.div
                  className="absolute -right-2 -top-2 rounded-full bg-emerald-500 px-2 py-0.5 text-[10px] font-bold text-slate-900 shadow-lg"
                  initial={{ x: 20, y: -20, opacity: 0, rotate: -15 }}
                  animate={{ x: 0, y: 0, opacity: 1, rotate: 0 }}
                  exit={{ x: 20, y: -20, opacity: 0, rotate: -15 }}
                  transition={{ type: "spring", stiffness: 300, damping: 20 }}
                >
                  Saved
                </motion.div>
              )}
            </AnimatePresence>
            <span className="text-lg font-bold text-emerald-300">12</span>
          </div>
        </div>
      </div>
    </div>
  );
}

// ============================================
// ADMIN-ONLY VISUALS (3)
// ============================================

// 20. AdminOverrideGlow
export function GfcCalendarAdminOverrideGlow() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Admin Override</h3>
        <div className="flex justify-center gap-2">
          {[13, 14, 15].map((day) => (
            <motion.div
              key={day}
              className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-purple-500/50 bg-purple-900/30"
              animate={{
                boxShadow: [
                  "0 0 10px rgba(168, 85, 247, 0.4)",
                  "0 0 25px rgba(168, 85, 247, 0.7)",
                  "0 0 10px rgba(168, 85, 247, 0.4)",
                ],
              }}
              transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
            >
              <span className="text-lg font-bold text-purple-300">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// 21. AdminEditTileFlip
export function GfcCalendarAdminEditTileFlip() {
  const [flipped, setFlipped] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setFlipped(true);
      setTimeout(() => setFlipped(false), 2000);
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Admin Edit Mode</h3>
        <div className="flex justify-center">
          <div className="relative h-16 w-16" style={{ perspective: "1000px" }}>
            <motion.div
              className="relative h-full w-full"
              animate={{ rotateY: flipped ? 180 : 0 }}
              transition={{ duration: 0.5 }}
              style={{ transformStyle: "preserve-3d" }}
            >
              <div
                className="absolute inset-0 flex items-center justify-center rounded-lg border-2 border-purple-500/50 bg-purple-900/30"
                style={{ backfaceVisibility: "hidden" }}
              >
                <span className="text-lg font-bold text-purple-300">16</span>
              </div>
              <div
                className="absolute inset-0 flex flex-col items-center justify-center rounded-lg border-2 border-purple-500/50 bg-purple-800/40"
                style={{ backfaceVisibility: "hidden", transform: "rotateY(180deg)" }}
              >
                <Edit className="h-5 w-5 text-purple-300" />
                <span className="mt-1 text-xs font-medium text-purple-300">Edit</span>
              </div>
            </motion.div>
          </div>
        </div>
      </div>
    </div>
  );
}

// 22. AdminBulkBlockingSweep
export function GfcCalendarAdminBulkBlockingSweep() {
  const [sweep, setSweep] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setSweep(true);
      setTimeout(() => setSweep(false), 1500);
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Bulk Blocking</h3>
        <div className="relative flex justify-center gap-2">
          {[17, 18, 19, 20].map((day, index) => (
            <motion.div
              key={day}
              className="relative flex h-16 w-16 items-center justify-center rounded-lg border-2 border-red-500/50 bg-red-900/30"
              animate={sweep && index <= 2 ? { backgroundColor: "rgba(127, 29, 29, 0.5)" } : {}}
              transition={{ delay: index * 0.1 }}
            >
              {sweep && index <= 2 && (
                <motion.div
                  className="absolute inset-0 rounded-lg bg-red-500/50"
                  initial={{ x: "-100%" }}
                  animate={{ x: "100%" }}
                  transition={{ duration: 0.5, delay: index * 0.1 }}
                />
              )}
              <span className="relative z-10 text-lg font-bold text-red-200">{day}</span>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}

// ============================================
// MONTH NAVIGATION & TRANSITIONS (3)
// ============================================

// 23. MonthSlideLeft
export function GfcCalendarMonthSlideLeft() {
  const [currentMonth, setCurrentMonth] = useState(0);
  const months = ["March 2026", "April 2026"];

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentMonth((prev) => (prev === 0 ? 1 : 0));
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Month Slide Left</h3>
        <div className="relative h-32 overflow-hidden">
          <AnimatePresence mode="wait">
            <motion.div
              key={currentMonth}
              className="absolute inset-0 flex items-center justify-center"
              initial={{ x: 100, opacity: 0 }}
              animate={{ x: 0, opacity: 1 }}
              exit={{ x: -100, opacity: 0 }}
              transition={{ duration: 0.4, ease: "easeInOut" }}
            >
              <div className="text-center">
                <div className="text-2xl font-bold text-slate-100">{months[currentMonth]}</div>
                <div className="mt-2 grid grid-cols-7 gap-1">
                  {[1, 2, 3, 4, 5, 6, 7].map((d) => (
                    <div
                      key={d}
                      className="flex h-8 items-center justify-center rounded border border-slate-700/30 bg-slate-800/50 text-xs text-slate-400"
                    >
                      {d}
                    </div>
                  ))}
                </div>
              </div>
            </motion.div>
          </AnimatePresence>
        </div>
      </div>
    </div>
  );
}

// 24. MonthSlideRight
export function GfcCalendarMonthSlideRight() {
  const [currentMonth, setCurrentMonth] = useState(0);
  const months = ["March 2026", "February 2026"];

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentMonth((prev) => (prev === 0 ? 1 : 0));
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Month Slide Right</h3>
        <div className="relative h-32 overflow-hidden">
          <AnimatePresence mode="wait">
            <motion.div
              key={currentMonth}
              className="absolute inset-0 flex items-center justify-center"
              initial={{ x: -100, opacity: 0 }}
              animate={{ x: 0, opacity: 1 }}
              exit={{ x: 100, opacity: 0 }}
              transition={{ duration: 0.4, ease: "easeInOut" }}
            >
              <div className="text-center">
                <div className="text-2xl font-bold text-slate-100">{months[currentMonth]}</div>
                <div className="mt-2 grid grid-cols-7 gap-1">
                  {[1, 2, 3, 4, 5, 6, 7].map((d) => (
                    <div
                      key={d}
                      className="flex h-8 items-center justify-center rounded border border-slate-700/30 bg-slate-800/50 text-xs text-slate-400"
                    >
                      {d}
                    </div>
                  ))}
                </div>
              </div>
            </motion.div>
          </AnimatePresence>
        </div>
      </div>
    </div>
  );
}

// 25. MonthFadeReveal
export function GfcCalendarMonthFadeReveal() {
  const [currentMonth, setCurrentMonth] = useState(0);
  const months = ["March 2026", "April 2026"];

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentMonth((prev) => (prev === 0 ? 1 : 0));
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Month Fade Reveal</h3>
        <div className="relative h-32 overflow-hidden">
          <AnimatePresence mode="wait">
            <motion.div
              key={currentMonth}
              className="absolute inset-0 flex items-center justify-center"
              initial={{ opacity: 0, scale: 0.9 }}
              animate={{ opacity: 1, scale: 1 }}
              exit={{ opacity: 0, scale: 0.9 }}
              transition={{ duration: 0.5, ease: "easeInOut" }}
            >
              <div className="text-center">
                <div className="text-2xl font-bold text-slate-100">{months[currentMonth]}</div>
                <div className="mt-2 grid grid-cols-7 gap-1">
                  {[1, 2, 3, 4, 5, 6, 7].map((d) => (
                    <div
                      key={d}
                      className="flex h-8 items-center justify-center rounded border border-slate-700/30 bg-slate-800/50 text-xs text-slate-400"
                    >
                      {d}
                    </div>
                  ))}
                </div>
              </div>
            </motion.div>
          </AnimatePresence>
        </div>
      </div>
    </div>
  );
}

