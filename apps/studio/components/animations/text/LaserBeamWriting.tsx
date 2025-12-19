"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Laser Write - A horizontal laser bar sweeps from left to right;
 * letters appear as the beam passes
 */
export const LaserBeamWriting: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#8b5cf6",
  speed = 1,
}) => {
  const letters = text.split("");
  const [revealed, setRevealed] = useState<number>(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setRevealed((prev) => {
        if (prev >= letters.length) {
          setTimeout(() => setRevealed(0), 1000 / speed);
          return prev;
        }
        return prev + 1;
      });
    }, 100 / speed);

    return () => clearInterval(interval);
  }, [letters.length, speed]);

  return (
    <AnimatedTextBase>
      <div className="relative flex items-center justify-center">
        <div className="flex">
          {letters.map((letter, index) => (
            <motion.span
              key={index}
              className="text-6xl font-bold"
              style={{ color: accentColor }}
              initial={{ opacity: 0 }}
              animate={{
                opacity: index < revealed ? 1 : 0,
                textShadow: [
                  `0 0 5px ${accentColor}`,
                  `0 0 20px ${accentColor}`,
                  `0 0 5px ${accentColor}`,
                ],
              }}
              transition={{
                opacity: { duration: 0.1 },
                textShadow: {
                  duration: 0.3,
                  repeat: Infinity,
                  delay: index * 0.1,
                },
              }}
            >
              {letter === " " ? "\u00A0" : letter}
            </motion.span>
          ))}
        </div>
        <motion.div
          className="absolute h-full w-1"
          style={{
            background: `linear-gradient(to right, transparent, ${accentColor}, transparent)`,
            boxShadow: `0 0 20px ${accentColor}`,
          }}
          animate={{
            x: ["-50px", `${letters.length * 40}px`],
          }}
          transition={{
            duration: (letters.length * 0.1) / speed,
            repeat: Infinity,
            ease: "linear",
          }}
        />
      </div>
    </AnimatedTextBase>
  );
};

export default LaserBeamWriting;

