"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function NeonPulseEdgeGlowButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative rounded-xl border-2 border-cyan-400 bg-slate-900 px-8 py-4 text-base font-semibold text-cyan-400"
        animate={{
          boxShadow: [
            "0 0 10px rgba(34, 211, 238, 0.5), 0 0 20px rgba(34, 211, 238, 0.3)",
            "0 0 20px rgba(34, 211, 238, 0.8), 0 0 40px rgba(34, 211, 238, 0.5)",
            "0 0 10px rgba(34, 211, 238, 0.5), 0 0 20px rgba(34, 211, 238, 0.3)",
          ],
        }}
        whileHover={{
          boxShadow: "0 0 30px rgba(34, 211, 238, 1), 0 0 60px rgba(34, 211, 238, 0.6)",
        }}
        whileTap={{
          boxShadow: "0 0 50px rgba(34, 211, 238, 1), 0 0 100px rgba(34, 211, 238, 0.8)",
          scale: 0.95,
        }}
        transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
      >
        Neon Pulse
      </motion.button>
    </div>
  );
}

export default NeonPulseEdgeGlowButton;

