"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Metallic Shine Sweep - A diagonal shine passes across the text in a loop
 */
export const MetallicShineSweep: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#eab308",
  speed = 1,
}) => {
  return (
    <AnimatedTextBase>
      <div className="relative inline-block">
        <motion.h1
          className="text-6xl font-bold bg-gradient-to-r from-slate-300 via-white to-slate-300 bg-clip-text text-transparent"
          style={{
            backgroundSize: "200% 100%",
            WebkitBackgroundClip: "text",
            WebkitTextFillColor: "transparent",
          }}
        >
          {text || "Animation Playground"}
        </motion.h1>
        <motion.div
          className="absolute inset-0 bg-gradient-to-r from-transparent via-white/60 to-transparent"
          style={{
            clipPath: "polygon(0 0, 30% 0, 35% 100%, 0 100%)",
          }}
          animate={{
            x: ["-100%", "200%"],
          }}
          transition={{
            duration: 2 / speed,
            repeat: Infinity,
            ease: "linear",
          }}
        />
      </div>
    </AnimatedTextBase>
  );
};

export default MetallicShineSweep;

