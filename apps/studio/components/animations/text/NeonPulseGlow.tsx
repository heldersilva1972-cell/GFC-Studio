"use client";

import React from "react";
import { motion } from "framer-motion";
import {
  AnimatedTextContainer,
  type AnimatedTextProps,
} from "./AnimatedTextBase";

/**
 * Neon Pulse Glow - Text with outer glow, looping soft pulse and subtle flicker
 */
export function NeonPulseGlow({
  text = "Animation Playground",
  subText,
  accentColor = "#f97316",
}: AnimatedTextProps) {
  return (
    <AnimatedTextContainer>
      <motion.div
        initial={{ opacity: 0, scale: 0.9 }}
        animate={{
          opacity: [0.4, 1, 0.7, 1],
          scale: [0.96, 1.04, 0.98, 1],
        }}
        transition={{
          duration: 1.8,
          repeat: Infinity,
          ease: "easeInOut",
        }}
        className="drop-shadow-[0_0_30px_rgba(249,115,22,0.9)]"
        style={{ color: accentColor }}
      >
        <div className="text-3xl md:text-5xl font-extrabold tracking-wide uppercase">
          {text || "Animation Playground"}
        </div>
        {subText && (
          <div className="mt-3 text-sm md:text-base text-slate-200/80">
            {subText}
          </div>
        )}
      </motion.div>
    </AnimatedTextContainer>
  );
}

export default NeonPulseGlow;

