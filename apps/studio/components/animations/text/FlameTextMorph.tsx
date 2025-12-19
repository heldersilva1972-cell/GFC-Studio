"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Flame Flicker - Letters flicker vertically/opacity like fire with warm colors
 */
export const FlameTextMorph: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#f97316",
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
            animate={{
              y: [0, -3, 0, 3, 0],
              opacity: [0.8, 1, 0.9, 1, 0.85],
              scale: [1, 1.05, 0.98, 1.02, 1],
              filter: [
                `hue-rotate(0deg) brightness(1)`,
                `hue-rotate(10deg) brightness(1.2)`,
                `hue-rotate(-5deg) brightness(0.9)`,
                `hue-rotate(0deg) brightness(1)`,
              ],
            }}
            transition={{
              duration: 0.3 / speed,
              repeat: Infinity,
              ease: "easeInOut",
              delay: index * 0.05,
            }}
            style={{
              color: accentColor,
              textShadow: `0 0 10px ${accentColor}, 0 0 20px ${accentColor}80`,
            }}
          >
            {letter === " " ? "\u00A0" : letter}
          </motion.span>
        ))}
      </div>
    </AnimatedTextBase>
  );
};

export default FlameTextMorph;

