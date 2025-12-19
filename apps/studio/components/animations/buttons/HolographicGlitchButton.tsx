"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function HolographicGlitchButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border border-fuchsia-500 bg-gradient-to-r from-fuchsia-600 via-purple-600 to-cyan-600 px-8 py-4 text-base font-semibold text-white"
        animate={{
          backgroundPosition: ["0% 0%", "100% 100%", "0% 0%"],
        }}
        whileHover={{
          x: [0, -2, 2, -1, 1, 0],
        }}
        transition={{
          backgroundPosition: { duration: 3, repeat: Infinity, ease: "linear" },
          x: { duration: 0.3 },
        }}
        style={{
          backgroundSize: "200% 200%",
        }}
      >
        <span className="relative z-10">Holographic</span>
        <motion.div
          className="absolute inset-0 bg-gradient-to-r from-transparent via-white/20 to-transparent"
          animate={{
            x: ["-100%", "200%"],
          }}
          transition={{
            duration: 2,
            repeat: Infinity,
            ease: "linear",
          }}
        />
      </motion.button>
    </div>
  );
}

export default HolographicGlitchButton;

