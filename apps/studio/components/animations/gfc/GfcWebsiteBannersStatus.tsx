"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { MapPin, Clock, Phone, Calendar } from "lucide-react";

// 1. Event Countdown Banner
export function GfcBannerEventCountdown() {
  const [days, setDays] = useState(10);
  const [hours, setHours] = useState(4);
  const [minutes, setMinutes] = useState(32);

  // Simple demo countdown (optional - can be static)
  useEffect(() => {
    const interval = setInterval(() => {
      setMinutes((prev) => {
        if (prev > 0) return prev - 1;
        setHours((prev) => {
          if (prev > 0) return prev - 1;
          setDays((prev) => (prev > 0 ? prev - 1 : 0));
          return 23;
        });
        return 59;
      });
    }, 60000); // Update every minute
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <motion.div
        className="relative w-full max-w-4xl overflow-hidden rounded-xl border border-slate-700/50 bg-gradient-to-r from-slate-900/95 via-slate-800/95 to-slate-900/95 p-6 shadow-xl"
        initial={{ opacity: 0, y: -16 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5, ease: "easeOut" }}
      >
        {/* Subtle glow on left */}
        <motion.div
          className="absolute left-0 top-0 h-full w-1 bg-gradient-to-b from-amber-400 to-amber-600"
          animate={{ opacity: [0.5, 1, 0.5] }}
          transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
        />

        <div className="flex items-center justify-between gap-6">
          <div className="flex-1">
            <h3 className="mb-1 text-lg font-bold text-slate-100">Holiday Dinner at the GFC</h3>
            <p className="text-sm text-slate-400">Join us for a festive evening of food and community</p>
          </div>

          {/* Countdown */}
          <div className="flex items-center gap-4">
            <div className="text-center">
              <motion.div
                key={days}
                initial={{ scale: 1.2, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                className="text-2xl font-bold text-amber-400"
              >
                {days}
              </motion.div>
              <div className="text-xs text-slate-500">Days</div>
            </div>
            <div className="text-amber-500">:</div>
            <div className="text-center">
              <motion.div
                key={hours}
                initial={{ scale: 1.2, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                className="text-2xl font-bold text-amber-400"
              >
                {hours}
              </motion.div>
              <div className="text-xs text-slate-500">Hours</div>
            </div>
            <div className="text-amber-500">:</div>
            <div className="text-center">
              <motion.div
                key={minutes}
                initial={{ scale: 1.2, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                className="text-2xl font-bold text-amber-400"
              >
                {minutes}
              </motion.div>
              <div className="text-xs text-slate-500">Minutes</div>
            </div>
          </div>

          {/* CTA */}
          <motion.button
            className="rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-4 py-2 text-sm font-semibold text-slate-900 shadow-lg"
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
          >
            View Event
          </motion.button>
        </div>
      </motion.div>
    </div>
  );
}

// 2. Status Chip – "Open for Rentals"
export function GfcChipOpenRentals() {
  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <motion.div
        className="relative inline-flex items-center gap-2 rounded-full border border-amber-500/30 bg-gradient-to-r from-amber-500/20 to-amber-600/20 px-4 py-2 shadow-lg"
        initial={{ opacity: 0, scale: 0.9 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 0.4 }}
        whileHover={{ scale: 1.05, boxShadow: "0 10px 25px rgba(251, 191, 36, 0.3)" }}
      >
        {/* Pulsing ring */}
        <motion.div
          className="absolute inset-0 rounded-full border-2 border-amber-400"
          animate={{ scale: [1, 1.2, 1], opacity: [0.5, 0, 0.5] }}
          transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
        />

        <motion.div
          animate={{ rotate: [0, 10, -10, 0] }}
          transition={{ duration: 2, repeat: Infinity, repeatDelay: 1 }}
        >
          <Calendar className="h-4 w-4 text-amber-400" />
        </motion.div>
        <span className="text-sm font-semibold text-amber-300">Now Accepting Bookings</span>
      </motion.div>
    </div>
  );
}

// 3. Footer CTA Bar
export function GfcFooterCtaBar() {
  return (
    <div className="flex h-full w-full items-end justify-center p-4">
      <motion.div
        className="relative w-full max-w-4xl overflow-hidden rounded-t-xl border-t border-l border-r border-slate-700/50 bg-gradient-to-r from-slate-900/95 via-slate-800/95 to-slate-900/95 p-6 shadow-xl"
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5, ease: "easeOut" }}
      >
        <div className="flex items-center justify-between gap-6">
          <div>
            <h3 className="text-lg font-semibold text-slate-100">Thinking about renting the hall?</h3>
            <p className="text-sm text-slate-400">Check our availability and book your event today</p>
          </div>

          <motion.button
            className="flex items-center gap-2 rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-6 py-3 font-semibold text-slate-900 shadow-lg"
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
          >
            <span>Check Availability</span>
            <motion.span
              animate={{ x: [0, 4, 0] }}
              transition={{ duration: 1.5, repeat: Infinity, ease: "easeInOut" }}
            >
              →
            </motion.span>
          </motion.button>
        </div>
      </motion.div>
    </div>
  );
}

// 4. Multi-Icon Contact Strip
export function GfcStripContactInfo() {
  const contactItems = [
    {
      icon: MapPin,
      label: "Address",
      value: "123 Main St, Gloucester",
      hoverAnimation: { y: [0, -4, 0] },
    },
    {
      icon: Clock,
      label: "Hours",
      value: "Mon-Fri: 9am-5pm",
      hoverAnimation: { rotate: [0, 15, -15, 0] },
    },
    {
      icon: Phone,
      label: "Phone",
      value: "(555) 123-4567",
      hoverAnimation: { x: [0, 2, -2, 0] },
    },
  ];

  return (
    <div className="flex h-full w-full items-center justify-center p-4">
      <div className="w-full max-w-4xl">
        <div className="grid gap-4 md:grid-cols-3">
          {contactItems.map((item, index) => {
            const Icon = item.icon;
            return (
              <motion.div
                key={item.label}
                className="group relative rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-4 shadow-lg"
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.4, delay: index * 0.1 }}
                whileHover={{ y: -4, boxShadow: "0 10px 25px rgba(0,0,0,0.3)" }}
              >
                <div className="flex items-start gap-3">
                  <motion.div
                    className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-amber-500/20"
                    whileHover={item.hoverAnimation}
                    transition={{ duration: 0.3 }}
                  >
                    <Icon className="h-5 w-5 text-amber-400" />
                  </motion.div>
                  <div className="flex-1 min-w-0">
                    <div className="mb-1 text-xs font-medium text-slate-400">{item.label}</div>
                    <div className="text-sm font-semibold text-slate-100">{item.value}</div>
                  </div>
                </div>
              </motion.div>
            );
          })}
        </div>
      </div>
    </div>
  );
}

