"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Typewriter Ink Bleed - Typewriter typing effect with ink-bleed/ripple
 * expansion on each letter as it appears
 */
export const TypewriterInkBleed: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#3b82f6",
  speed = 1,
}) => {
  const letters = text.split("");

  return (
    <AnimatedTextBase>
      <div className="flex items-center justify-center">
        {letters.map((letter, index) => (
          <motion.span
            key={index}
            className="relative text-6xl font-mono font-bold text-white"
            initial={{ opacity: 0, scale: 0.5 }}
            animate={{
              opacity: 1,
              scale: [0.5, 1.2, 1],
            }}
            transition={{
              delay: index * 0.1 / speed,
              duration: 0.4 / speed,
              ease: "easeOut",
            }}
          >
            {letter === " " ? "\u00A0" : letter}
            <motion.span
              className="absolute inset-0 rounded-full"
              style={{
                background: `radial-gradient(circle, ${accentColor}40, transparent)`,
              }}
              initial={{ scale: 0, opacity: 0.8 }}
              animate={{
                scale: [0, 2, 0],
                opacity: [0.8, 0.4, 0],
              }}
              transition={{
                delay: index * 0.1 / speed,
                duration: 0.6 / speed,
                ease: "easeOut",
              }}
            />
          </motion.span>
        ))}
      </div>
    </AnimatedTextBase>
  );
};

export default TypewriterInkBleed;

