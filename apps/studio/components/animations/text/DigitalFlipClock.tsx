"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Digital Flip Clock - Characters appear via vertical flip-card motion
 * like an old digital clock, staggered per character
 */
export const DigitalFlipClock: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#10b981",
  speed = 1,
}) => {
  const letters = text.split("");

  return (
    <AnimatedTextBase>
      <div className="flex items-center justify-center gap-1">
        {letters.map((letter, index) => (
          <motion.div
            key={index}
            className="relative overflow-hidden"
            style={{
              perspective: "1000px",
            }}
          >
            <motion.span
              className="block text-6xl font-mono font-bold"
              style={{ color: accentColor }}
              initial={{ rotateX: -90, opacity: 0 }}
              animate={{ rotateX: 0, opacity: 1 }}
              transition={{
                delay: index * 0.15 / speed,
                duration: 0.6 / speed,
                ease: "easeOut",
              }}
            >
              {letter === " " ? "\u00A0" : letter}
            </motion.span>
          </motion.div>
        ))}
      </div>
    </AnimatedTextBase>
  );
};

export default DigitalFlipClock;

