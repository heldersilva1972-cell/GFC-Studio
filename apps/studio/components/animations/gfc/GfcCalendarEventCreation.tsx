"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Plus, Calendar, X, Check, Clock, MapPin } from "lucide-react";

// 1. CalendarCreateEventPop
export function CalendarCreateEventPop() {
  const [popped, setPopped] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setPopped(true);
      setTimeout(() => setPopped(false), 1000);
    }, 3000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Create Event Pop</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">15</span>
            <AnimatePresence>
              {popped && (
                <motion.div
                  className="absolute inset-0 flex items-center justify-center rounded-lg bg-blue-500/20"
                  initial={{ scale: 0, opacity: 0 }}
                  animate={{ scale: 1.2, opacity: 1 }}
                  exit={{ scale: 0, opacity: 0 }}
                  transition={{ type: "spring", stiffness: 300, damping: 20 }}
                >
                  <Plus className="h-6 w-6 text-blue-400" />
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 2. CalendarDragToCreateSlot
export function CalendarDragToCreateSlot() {
  const [dragging, setDragging] = useState(false);
  const [dropped, setDropped] = useState(false);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Drag To Create Slot</h3>
        <div className="flex justify-center gap-4">
          <motion.div
            className="flex h-12 w-12 cursor-grab items-center justify-center rounded-lg border-2 border-dashed border-blue-500/50 bg-blue-500/10"
            drag
            dragConstraints={{ left: 0, right: 0, top: 0, bottom: 0 }}
            onDragStart={() => setDragging(true)}
            onDragEnd={() => {
              setDragging(false);
              setDropped(true);
              setTimeout(() => setDropped(false), 1500);
            }}
            animate={{ scale: dragging ? 1.1 : 1, opacity: dragging ? 0.7 : 1 }}
          >
            <Calendar className="h-5 w-5 text-blue-300" />
          </motion.div>
          <motion.div
            className="flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800"
            animate={{
              borderColor: dropped ? "rgba(59, 130, 246, 0.5)" : undefined,
              backgroundColor: dropped ? "rgba(59, 130, 246, 0.1)" : undefined,
            }}
            transition={{ duration: 0.3 }}
          >
            <span className="text-lg font-bold text-slate-300">16</span>
            {dropped && (
              <motion.div
                className="absolute inset-0 flex items-center justify-center"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                exit={{ scale: 0 }}
              >
                <Check className="h-6 w-6 text-blue-400" />
              </motion.div>
            )}
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 3. CalendarEventExpandForm
export function CalendarEventExpandForm() {
  const [expanded, setExpanded] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setExpanded(true);
      setTimeout(() => setExpanded(false), 2500);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Expand Form</h3>
        <div className="flex justify-center">
          <motion.div
            className="overflow-hidden rounded-lg border border-slate-600 bg-slate-800"
            animate={{ height: expanded ? 120 : 48, width: expanded ? 200 : 48 }}
            transition={{ duration: 0.4, ease: "easeInOut" }}
          >
            <div className="flex h-full items-center justify-center p-2">
              {expanded ? (
                <motion.div
                  className="flex flex-col gap-2"
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  transition={{ delay: 0.2 }}
                >
                  <div className="h-2 w-32 rounded bg-slate-600" />
                  <div className="h-2 w-24 rounded bg-slate-600" />
                  <div className="h-6 w-20 rounded bg-blue-500" />
                </motion.div>
              ) : (
                <span className="text-lg font-bold text-slate-300">17</span>
              )}
            </div>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 4. CalendarDayPressPopup
export function CalendarDayPressPopup() {
  const [showPopup, setShowPopup] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setShowPopup(true);
      setTimeout(() => setShowPopup(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Day Press Popup</h3>
        <div className="flex justify-center">
          <div className="relative">
            <div className="flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
              <span className="text-lg font-bold text-slate-300">18</span>
            </div>
            <AnimatePresence>
              {showPopup && (
                <motion.div
                  className="absolute -top-20 left-1/2 w-32 -translate-x-1/2 rounded-lg border border-slate-600 bg-slate-800 p-2 shadow-lg"
                  initial={{ y: 10, opacity: 0, scale: 0.8 }}
                  animate={{ y: 0, opacity: 1, scale: 1 }}
                  exit={{ y: 10, opacity: 0, scale: 0.8 }}
                  transition={{ type: "spring", stiffness: 300 }}
                >
                  <div className="text-center text-xs text-slate-300">Create Event</div>
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 5. CalendarSlideInEventCreator
export function CalendarSlideInEventCreator() {
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setVisible(true);
      setTimeout(() => setVisible(false), 2500);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="relative w-full max-w-md overflow-hidden rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Slide In Event Creator</h3>
        <div className="flex justify-center">
          <div className="flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">19</span>
          </div>
        </div>
        <AnimatePresence>
          {visible && (
            <motion.div
              className="absolute bottom-0 left-0 right-0 rounded-t-lg border-t border-slate-600 bg-slate-800 p-4"
              initial={{ y: "100%" }}
              animate={{ y: 0 }}
              exit={{ y: "100%" }}
              transition={{ type: "spring", stiffness: 300, damping: 30 }}
            >
              <div className="space-y-2">
                <div className="h-2 w-full rounded bg-slate-600" />
                <div className="h-2 w-3/4 rounded bg-slate-600" />
                <div className="h-6 w-20 rounded bg-blue-500" />
              </div>
            </motion.div>
          )}
        </AnimatePresence>
      </div>
    </div>
  );
}

// 6. CalendarTapAddMarker
export function CalendarTapAddMarker() {
  const [marked, setMarked] = useState(false);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Tap Add Marker</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 cursor-pointer items-center justify-center rounded-lg border border-slate-600 bg-slate-800"
            onClick={() => {
              setMarked(true);
              setTimeout(() => setMarked(false), 1500);
            }}
            whileTap={{ scale: 0.9 }}
          >
            <span className="text-lg font-bold text-slate-300">20</span>
            <AnimatePresence>
              {marked && (
                <motion.div
                  className="absolute -top-1 -right-1 h-4 w-4 rounded-full bg-green-500"
                  initial={{ scale: 0 }}
                  animate={{ scale: [0, 1.3, 1] }}
                  exit={{ scale: 0 }}
                  transition={{ type: "spring", stiffness: 300 }}
                />
              )}
            </AnimatePresence>
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 7. CalendarEventGrowHighlight
export function CalendarEventGrowHighlight() {
  const [highlighted, setHighlighted] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setHighlighted(true);
      setTimeout(() => setHighlighted(false), 1500);
    }, 3500);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Grow Highlight</h3>
        <div className="flex justify-center">
          <motion.div
            className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800"
            animate={{
              scale: highlighted ? 1.15 : 1,
              borderColor: highlighted ? "rgba(59, 130, 246, 0.8)" : undefined,
              boxShadow: highlighted ? "0 0 20px rgba(59, 130, 246, 0.5)" : undefined,
            }}
            transition={{ duration: 0.3 }}
          >
            <span className="text-lg font-bold text-slate-300">21</span>
            {highlighted && (
              <motion.div
                className="absolute inset-0 rounded-lg bg-blue-500/20"
                initial={{ scale: 0.8, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                exit={{ scale: 0.8, opacity: 0 }}
              />
            )}
          </motion.div>
        </div>
      </div>
    </div>
  );
}

// 8. CalendarDropEventIntoDay
export function CalendarDropEventIntoDay() {
  const [dropping, setDropping] = useState(false);
  const [dropped, setDropped] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setDropping(true);
      setTimeout(() => {
        setDropping(false);
        setDropped(true);
        setTimeout(() => setDropped(false), 1500);
      }, 800);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Drop Event Into Day</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">22</span>
            <AnimatePresence>
              {dropping && (
                <motion.div
                  className="absolute -top-8 flex h-8 w-8 items-center justify-center rounded-lg bg-blue-500"
                  initial={{ y: -20, opacity: 0 }}
                  animate={{ y: 0, opacity: 1 }}
                  exit={{ y: 0, opacity: 0 }}
                  transition={{ duration: 0.4 }}
                >
                  <Calendar className="h-4 w-4 text-white" />
                </motion.div>
              )}
              {dropped && (
                <motion.div
                  className="absolute inset-0 flex items-center justify-center rounded-lg bg-blue-500/30"
                  initial={{ scale: 0 }}
                  animate={{ scale: 1 }}
                  exit={{ scale: 0 }}
                >
                  <Check className="h-6 w-6 text-blue-400" />
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

// 9. CalendarSelectWindowPulse
export function CalendarSelectWindowPulse() {
  const [pulsing, setPulsing] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setPulsing(true);
      setTimeout(() => setPulsing(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Select Window Pulse</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">23</span>
            {pulsing && (
              <motion.div
                className="absolute inset-0 rounded-lg border-2 border-blue-500"
                animate={{
                  scale: [1, 1.2, 1],
                  opacity: [1, 0.5, 1],
                }}
                transition={{ duration: 1, repeat: Infinity }}
              />
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

// 10. CalendarEventBubbleRise
export function CalendarEventBubbleRise() {
  const [bubbling, setBubbling] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setBubbling(true);
      setTimeout(() => setBubbling(false), 2000);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-md rounded-xl border border-slate-700/50 bg-slate-900/80 p-6 shadow-xl">
        <h3 className="mb-4 text-center text-sm font-semibold text-slate-300">Event Bubble Rise</h3>
        <div className="flex justify-center">
          <div className="relative flex h-16 w-16 items-center justify-center rounded-lg border border-slate-600 bg-slate-800">
            <span className="text-lg font-bold text-slate-300">24</span>
            <AnimatePresence>
              {bubbling && (
                <motion.div
                  className="absolute -top-2 left-1/2 h-6 w-6 -translate-x-1/2 rounded-full bg-blue-500"
                  initial={{ y: 0, scale: 0, opacity: 0 }}
                  animate={{ y: -30, scale: 1, opacity: [0, 1, 0] }}
                  exit={{ y: -30, scale: 0, opacity: 0 }}
                  transition={{ duration: 1.5, ease: "easeOut" }}
                >
                  <Plus className="absolute inset-0 m-auto h-3 w-3 text-white" />
                </motion.div>
              )}
            </AnimatePresence>
          </div>
        </div>
      </div>
    </div>
  );
}

