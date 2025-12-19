"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Ice Crack Reveal - Text appears frosted, then cracking animation reveals sharp clear text
 */
export const IceFreezeCrack: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#06b6d4",
  speed = 1,
}) => {
  const [phase, setPhase] = useState<"frozen" | "cracking" | "clear">("frozen");

  useEffect(() => {
    const timer1 = setTimeout(() => setPhase("cracking"), 1000 / speed);
    const timer2 = setTimeout(() => setPhase("clear"), 2000 / speed);
    const timer3 = setTimeout(() => setPhase("frozen"), 4000 / speed);

    return () => {
      clearTimeout(timer1);
      clearTimeout(timer2);
      clearTimeout(timer3);
    };
  }, [speed]);

  return (
    <AnimatedTextBase>
      <motion.h1
        className="text-6xl font-bold"
        style={{ color: accentColor }}
        animate={{
          filter: [
            phase === "frozen"
              ? "blur(4px) brightness(1.5)"
              : phase === "cracking"
              ? "blur(2px) brightness(1.2)"
              : "blur(0px) brightness(1)",
          ],
          opacity: phase === "cracking" ? [1, 0.7, 1, 0.8, 1] : 1,
          scale: phase === "cracking" ? [1, 1.02, 0.98, 1.01, 1] : 1,
        }}
        transition={{
          filter: { duration: 0.5 / speed },
          opacity: {
            duration: 0.1,
            repeat: phase === "cracking" ? Infinity : 0,
          },
          scale: {
            duration: 0.1,
            repeat: phase === "cracking" ? Infinity : 0,
          },
        }}
      >
        {text || "Animation Playground"}
      </motion.h1>
    </AnimatedTextBase>
  );
};

export default IceFreezeCrack;

