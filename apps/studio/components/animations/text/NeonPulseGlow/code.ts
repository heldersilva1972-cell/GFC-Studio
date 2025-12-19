export const neonPulseGlowCode = `"use client";

import React from "react";
import { motion } from "framer-motion";

/**
 * Neon Pulse Glow - Text with outer glow, per-letter sequential fade-in,
 * looping soft pulse and subtle flicker
 */
export const NeonPulseGlow = ({ text = "Animation Playground", accentColor = "#a855f7", speed = 1 }) => {
  const letters = text.split("");

  return (
    <div className="flex h-full w-full items-center justify-center">
      <motion.div
        className="flex items-center justify-center"
        animate={{
          filter: [
            \`drop-shadow(0 0 10px \${accentColor})\`,
            \`drop-shadow(0 0 20px \${accentColor})\`,
            \`drop-shadow(0 0 10px \${accentColor})\`,
          ],
        }}
        transition={{
          duration: 2 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      >
        {letters.map((letter, index) => (
          <motion.span
            key={index}
            className="text-6xl font-bold text-white"
            initial={{ opacity: 0 }}
            animate={{
              opacity: [0.8, 1, 0.9, 1],
              filter: [
                \`drop-shadow(0 0 5px \${accentColor})\`,
                \`drop-shadow(0 0 15px \${accentColor})\`,
                \`drop-shadow(0 0 5px \${accentColor})\`,
              ],
            }}
            transition={{
              opacity: {
                delay: index * 0.1,
                duration: 0.5,
              },
              filter: {
                duration: 1.5 / speed,
                repeat: Infinity,
                delay: index * 0.05,
                ease: "easeInOut",
              },
            }}
            style={{ color: accentColor }}
          >
            {letter === " " ? "\\u00A0" : letter}
          </motion.span>
        ))}
      </motion.div>
    </div>
  );
};`;

