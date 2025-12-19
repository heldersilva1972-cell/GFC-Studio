"use client";

import React, { useMemo } from "react";
import { motion } from "framer-motion";
import { AnimatedTextContainer, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Particle Burst Reveal - Letters appear with a popping burst effect
 * Each character starts small and offset, then bursts into place
 */
export const ParticleBurstReveal: React.FC<AnimatedTextProps> = ({
  text = "Burst Reveal",
  accentColor = "#f59e0b",
  speed = 1,
}) => {
  const letters = text.split("");

  // Calculate offsets deterministically based on index
  const letterOffsets = useMemo(() => {
    return letters.map((_, index) => {
      // Create circular burst pattern - letters come from different angles
      const angle = (Math.PI * 2 * index) / Math.max(letters.length, 1);
      // Vary distance slightly based on index for more organic feel
      const distance = 50 + (index % 3) * 10;
      return {
        x: Math.cos(angle) * distance,
        y: Math.sin(angle) * distance,
      };
    });
  }, [letters]);

  return (
    <AnimatedTextContainer>
      <div className="flex items-center justify-center gap-1">
        {letters.map((letter, index) => {
          const offset = letterOffsets[index];

          return (
            <motion.span
              key={`${text}-${index}`}
              className="text-4xl md:text-6xl font-bold"
              style={{ color: accentColor }}
              initial={{
                scale: 0,
                opacity: 0,
                x: offset.x,
                y: offset.y,
              }}
              animate={{
                scale: 1,
                opacity: 1,
                x: 0,
                y: 0,
              }}
              transition={{
                delay: index * 0.08 / speed,
                duration: 0.5 / speed,
                type: "spring",
                stiffness: 300,
                damping: 20,
              }}
            >
              {letter === " " ? "\u00A0" : letter}
            </motion.span>
          );
        })}
      </div>
    </AnimatedTextContainer>
  );
};

export default ParticleBurstReveal;

