"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function AuroraGradientButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border border-slate-700 bg-gradient-to-br from-purple-600 via-pink-600 to-cyan-600 px-8 py-4 text-base font-semibold text-white"
        animate={{
          backgroundPosition: ["0% 0%", "100% 100%", "0% 0%"],
        }}
        whileHover={{
          scale: 1.05,
          filter: "brightness(1.2)",
        }}
        transition={{
          backgroundPosition: { duration: 8, repeat: Infinity, ease: "linear" },
        }}
        style={{
          backgroundSize: "200% 200%",
        }}
      >
        <span className="relative z-10">Aurora</span>
      </motion.button>
    </div>
  );
}

export default AuroraGradientButton;

