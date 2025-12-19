"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function AuraPressureButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative rounded-xl border border-emerald-500 bg-emerald-600 px-8 py-4 text-base font-semibold text-white"
        whileHover="hover"
        whileTap="tap"
        initial="initial"
      >
        <span className="relative z-10">Aura Pressure</span>
        <motion.div
          className="absolute inset-0 rounded-xl bg-emerald-400 blur-xl"
          variants={{
            initial: { scale: 1, opacity: 0.3 },
            hover: { scale: 1.2, opacity: 0.6 },
            tap: { scale: 0.8, opacity: 0.2 },
          }}
          transition={{ type: "spring", stiffness: 300, damping: 20 }}
        />
        <motion.div
          className="absolute inset-0 rounded-xl bg-emerald-300 blur-2xl"
          variants={{
            initial: { scale: 1, opacity: 0.2 },
            hover: { scale: 1.3, opacity: 0.4 },
            tap: { scale: 0.7, opacity: 0.1 },
          }}
          transition={{ type: "spring", stiffness: 300, damping: 20 }}
        />
      </motion.button>
    </div>
  );
}

export default AuraPressureButton;

