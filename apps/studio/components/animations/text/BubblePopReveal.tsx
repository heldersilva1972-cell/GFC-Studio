"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextContainer, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Bubble Pop Reveal - Letters pop in with a bubbly expanding motion
 * Each character starts small, expands beyond normal size, then settles
 */
export const BubblePopReveal: React.FC<AnimatedTextProps> = ({
  text = "Bubble Pop",
  accentColor = "#a855f7",
  speed = 1,
}) => {
  const letters = text.split("");

  return (
    <AnimatedTextContainer>
      <div className="flex items-center justify-center gap-1">
        {letters.map((letter, index) => (
          <motion.span
            key={`${text}-${index}`}
            className="text-4xl md:text-6xl font-bold"
            style={{ color: accentColor }}
            initial={{
              scale: 0,
              opacity: 0,
            }}
            animate={{
              scale: [0, 1.3, 0.95, 1],
              opacity: [0, 1, 1, 1],
            }}
            transition={{
              delay: index * 0.08 / speed,
              duration: 0.6 / speed,
              type: "spring",
              stiffness: 400,
              damping: 15,
              repeat: Infinity,
              repeatDelay: (letters.length * 0.08 + 1.5) / speed,
            }}
          >
            {letter === " " ? "\u00A0" : letter}
          </motion.span>
        ))}
      </div>
    </AnimatedTextContainer>
  );
};

export default BubblePopReveal;

