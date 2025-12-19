"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function SplitRevealButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border border-red-500 bg-red-600 px-8 py-4 text-base font-semibold text-white"
        whileHover="hover"
        initial="initial"
      >
        <motion.div
          className="absolute inset-0 flex"
          variants={{
            initial: { x: 0 },
            hover: { x: -10 },
          }}
          transition={{ type: "spring", stiffness: 300, damping: 20 }}
        >
          <div className="flex-1 bg-red-600" />
        </motion.div>
        <motion.div
          className="absolute inset-0 flex"
          variants={{
            initial: { x: 0 },
            hover: { x: 10 },
          }}
          transition={{ type: "spring", stiffness: 300, damping: 20 }}
        >
          <div className="flex-1 bg-red-600" />
        </motion.div>
        <motion.div
          className="absolute inset-0 bg-gradient-to-r from-transparent via-yellow-400/50 to-transparent"
          variants={{
            initial: { opacity: 0, scaleX: 0 },
            hover: { opacity: 1, scaleX: 1 },
          }}
          transition={{ duration: 0.3 }}
        />
        <span className="relative z-10">Split Reveal</span>
      </motion.button>
    </div>
  );
}

export default SplitRevealButton;

