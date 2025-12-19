"use client";

import React from "react";
import { motion } from "framer-motion";

export function NeonBorderCard() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <div className="relative w-full max-w-sm overflow-hidden rounded-xl p-[2px]">
        {/* Animated border */}
        <motion.div
          className="absolute inset-0"
          style={{
            background: "conic-gradient(from 0deg, transparent, #fbbf24, #8b5cf6, #3b82f6, transparent)",
          }}
          animate={{ rotate: 360 }}
          transition={{ duration: 3, repeat: Infinity, ease: "linear" }}
        />

        {/* Card content */}
        <motion.div
          className="relative rounded-xl bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-8 shadow-2xl"
          initial={{ opacity: 0, scale: 0.9 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.5 }}
          whileHover={{ scale: 1.05, boxShadow: "0 20px 40px rgba(251, 191, 36, 0.3)" }}
        >
          <h3 className="mb-3 text-2xl font-bold text-slate-100">Premium Feature</h3>
          <p className="mb-6 text-slate-400">
            Unlock advanced capabilities with our premium plan. Get access to exclusive features and priority support.
          </p>
          <motion.button
            className="w-full rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-6 py-3 font-semibold text-slate-900 shadow-lg"
            whileHover={{ scale: 1.02 }}
            whileTap={{ scale: 0.98 }}
          >
            Upgrade Now
          </motion.button>
        </motion.div>
      </div>
    </div>
  );
}

