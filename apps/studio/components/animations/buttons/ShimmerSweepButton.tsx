"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function ShimmerSweepButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border border-slate-700 bg-gradient-to-r from-slate-800 via-slate-700 to-slate-800 px-8 py-4 text-base font-semibold text-slate-100"
        whileHover="hover"
        initial="initial"
      >
        <span className="relative z-10">Shimmer</span>
        <motion.div
          className="absolute inset-0 bg-gradient-to-r from-transparent via-white/30 to-transparent"
          variants={{
            initial: { x: "-100%" },
            hover: { x: "200%" },
          }}
          transition={{ duration: 0.6, ease: "easeInOut" }}
        />
      </motion.button>
    </div>
  );
}

export default ShimmerSweepButton;

