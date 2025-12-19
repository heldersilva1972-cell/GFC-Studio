"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function LaserBeamBorderTraceButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative rounded-xl border-2 border-cyan-400 bg-slate-900 px-8 py-4 text-base font-semibold text-cyan-400"
        whileHover="hover"
        initial="initial"
      >
        <span className="relative z-10">Laser Trace</span>
        {/* Top edge */}
        <motion.div
          className="absolute left-0 top-0 h-1 bg-cyan-400"
          variants={{
            initial: { width: 0, x: 0 },
            hover: { width: "100%", transition: { duration: 0.3, delay: 0 } },
          }}
        />
        {/* Right edge */}
        <motion.div
          className="absolute right-0 top-0 w-1 bg-cyan-400"
          variants={{
            initial: { height: 0, y: 0 },
            hover: { height: "100%", transition: { duration: 0.3, delay: 0.3 } },
          }}
        />
        {/* Bottom edge */}
        <motion.div
          className="absolute bottom-0 right-0 h-1 bg-cyan-400"
          variants={{
            initial: { width: 0, x: 0 },
            hover: { width: "100%", transition: { duration: 0.3, delay: 0.6 } },
          }}
        />
        {/* Left edge */}
        <motion.div
          className="absolute bottom-0 left-0 w-1 bg-cyan-400"
          variants={{
            initial: { height: 0, y: 0 },
            hover: { height: "100%", transition: { duration: 0.3, delay: 0.9 } },
          }}
        />
      </motion.button>
    </div>
  );
}

export default LaserBeamBorderTraceButton;

