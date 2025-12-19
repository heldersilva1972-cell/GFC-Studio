"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Explode & Reform - Text starts intact, explodes outward, then reassembles
 */
export const ExplodeReform: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#ef4444",
  speed = 1,
}) => {
  const letters = text.split("");
  const [phase, setPhase] = useState<"intact" | "exploded" | "reformed">(
    "intact"
  );

  useEffect(() => {
    const timer1 = setTimeout(() => setPhase("exploded"), 1000 / speed);
    const timer2 = setTimeout(() => setPhase("reformed"), 2000 / speed);
    const timer3 = setTimeout(() => setPhase("intact"), 3500 / speed);

    return () => {
      clearTimeout(timer1);
      clearTimeout(timer2);
      clearTimeout(timer3);
    };
  }, [speed]);

  return (
    <AnimatedTextBase>
      <div className="flex items-center justify-center">
        {letters.map((letter, index) => (
          <motion.span
            key={index}
            className="text-6xl font-bold"
            style={{ color: accentColor }}
            animate={
              phase === "exploded"
                ? {
                    x: (Math.random() - 0.5) * 200,
                    y: (Math.random() - 0.5) * 200,
                    rotate: (Math.random() - 0.5) * 360,
                    opacity: 0,
                    scale: 0,
                  }
                : {
                    x: 0,
                    y: 0,
                    rotate: 0,
                    opacity: 1,
                    scale: 1,
                  }
            }
            transition={{
              duration: 0.8 / speed,
              ease: phase === "exploded" ? "easeIn" : "easeOut",
              delay: phase === "reformed" ? index * 0.05 / speed : 0,
            }}
          >
            {letter === " " ? "\u00A0" : letter}
          </motion.span>
        ))}
      </div>
    </AnimatedTextBase>
  );
};

export default ExplodeReform;

