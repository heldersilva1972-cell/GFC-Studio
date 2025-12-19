"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function PopOut3DLayeredButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8" style={{ perspective: "1000px" }}>
      <div className="relative">
        {/* Background layer */}
        <motion.div
          className="absolute inset-0 rounded-xl bg-slate-700"
          whileHover={{ scale: 1.05, y: 4, z: -10 }}
          transition={{ type: "spring", stiffness: 300, damping: 20 }}
        />
        {/* Main face */}
        <motion.button
          type="button"
          className="relative rounded-xl border border-cyan-500 bg-cyan-600 px-8 py-4 text-base font-semibold text-white"
          whileHover={{ scale: 1.05, y: -4, z: 10 }}
          whileTap={{ scale: 0.98, y: 0, z: 0 }}
          transition={{ type: "spring", stiffness: 300, damping: 20 }}
        >
          3D Layers
        </motion.button>
        {/* Highlight edge */}
        <motion.div
          className="absolute inset-0 rounded-xl border-2 border-cyan-300 opacity-50"
          whileHover={{ scale: 1.08, y: -6, z: 15, opacity: 0.8 }}
          transition={{ type: "spring", stiffness: 300, damping: 20 }}
        />
      </div>
    </div>
  );
}

export default PopOut3DLayeredButton;

