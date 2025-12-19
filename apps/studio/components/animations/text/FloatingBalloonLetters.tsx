"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Floating Balloon Letters - Each letter gently floats upward and bobs,
 * with soft scale-in from below
 */
export const FloatingBalloonLetters: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#ec4899",
  speed = 1,
}) => {
  const letters = text.split("");

  return (
    <AnimatedTextBase>
      <div className="flex items-center justify-center">
        {letters.map((letter, index) => (
          <motion.span
            key={index}
            className="text-6xl font-bold"
            style={{ color: accentColor }}
            initial={{ y: 100, scale: 0, opacity: 0 }}
            animate={{
              y: [0, -15, 0],
              scale: 1,
              opacity: 1,
            }}
            transition={{
              y: {
                duration: 2 / speed,
                repeat: Infinity,
                ease: "easeInOut",
                delay: index * 0.1,
              },
              scale: {
                delay: index * 0.1 / speed,
                duration: 0.5 / speed,
                ease: "easeOut",
              },
              opacity: {
                delay: index * 0.1 / speed,
                duration: 0.5 / speed,
              },
            }}
          >
            {letter === " " ? "\u00A0" : letter}
          </motion.span>
        ))}
      </div>
    </AnimatedTextBase>
  );
};

export default FloatingBalloonLetters;

