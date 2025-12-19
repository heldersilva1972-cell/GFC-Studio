"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Chromatic Split - Continuous RGB split offset on text; intensifies on loop moments
 */
export const ChromaticRGBSplit: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#10b981",
  speed = 1,
}) => {
  return (
    <AnimatedTextBase>
      <div className="relative flex items-center justify-center">
        <motion.h1
          className="text-6xl font-bold absolute"
          style={{
            color: "#ff0000",
            mixBlendMode: "screen",
          }}
          animate={{
            x: [-2, 2, -2],
          }}
          transition={{
            duration: 0.1 / speed,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        >
          {text || "Animation Playground"}
        </motion.h1>
        <motion.h1
          className="text-6xl font-bold absolute"
          style={{
            color: "#00ff00",
            mixBlendMode: "screen",
          }}
          animate={{
            x: [0, 0, 0],
          }}
        >
          {text || "Animation Playground"}
        </motion.h1>
        <motion.h1
          className="text-6xl font-bold absolute"
          style={{
            color: "#0000ff",
            mixBlendMode: "screen",
          }}
          animate={{
            x: [2, -2, 2],
          }}
          transition={{
            duration: 0.1 / speed,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        >
          {text || "Animation Playground"}
        </motion.h1>
        <h1
          className="text-6xl font-bold relative"
          style={{ color: accentColor, opacity: 0.3 }}
        >
          {text || "Animation Playground"}
        </h1>
      </div>
    </AnimatedTextBase>
  );
};

export default ChromaticRGBSplit;

