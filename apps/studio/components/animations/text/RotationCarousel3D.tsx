"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * 3D Carousel Text - Whole text rotates in 3D around Y axis like on a cylinder
 */
export const RotationCarousel3D: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#14b8a6",
  speed = 1,
}) => {
  return (
    <AnimatedTextBase>
      <div style={{ perspective: "1000px" }}>
        <motion.h1
          className="text-6xl font-bold"
          style={{
            color: accentColor,
            transformStyle: "preserve-3d",
          }}
          animate={{
            rotateY: [0, 360],
          }}
          transition={{
            duration: 4 / speed,
            repeat: Infinity,
            ease: "linear",
          }}
        >
          {text || "Animation Playground"}
        </motion.h1>
      </div>
    </AnimatedTextBase>
  );
};

export default RotationCarousel3D;

