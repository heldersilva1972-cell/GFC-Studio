"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Warp Speed - Text streaks from center as stretched lines, then decelerates into clear letters
 */
export const WarpSpeedStretch: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#f59e0b",
  speed = 1,
}) => {
  const letters = text.split("");
  const [phase, setPhase] = useState<"streaking" | "settled">("streaking");

  useEffect(() => {
    setTimeout(() => setPhase("settled"), 1000 / speed);
    const timer = setTimeout(() => setPhase("streaking"), 3000 / speed);
    return () => clearTimeout(timer);
  }, [phase, speed]);

  return (
    <AnimatedTextBase>
      <div className="flex items-center justify-center">
        {letters.map((letter, index) => (
          <motion.span
            key={index}
            className="text-6xl font-bold"
            style={{ color: accentColor }}
            animate={
              phase === "streaking"
                ? {
                    scaleX: [0.1, 1],
                    scaleY: [3, 1],
                    x: [0, (index - letters.length / 2) * 10],
                    opacity: [0, 1],
                  }
                : {
                    scaleX: 1,
                    scaleY: 1,
                    x: 0,
                    opacity: 1,
                  }
            }
            transition={{
              duration: 0.8 / speed,
              delay: index * 0.05 / speed,
              ease: phase === "streaking" ? "easeOut" : "easeIn",
            }}
          >
            {letter === " " ? "\u00A0" : letter}
          </motion.span>
        ))}
      </div>
    </AnimatedTextBase>
  );
};

export default WarpSpeedStretch;

