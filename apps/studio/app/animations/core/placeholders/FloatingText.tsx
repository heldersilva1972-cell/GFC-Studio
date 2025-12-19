"use client";

import React from "react";
import { motion } from "framer-motion";
import type { AnimationProps } from "../types";

export default function FloatingText({
  className = "",
  size = 200,
  speed = 1.0,
  colors = ["#ec4899"],
}: AnimationProps) {
  return (
    <div
      className={`flex items-center justify-center ${className}`}
      style={{ width: size, height: size }}
    >
      <motion.div
        style={{
          color: colors[0] || "#ec4899",
          fontSize: size * 0.15,
          fontWeight: "bold",
        }}
        animate={{
          y: [-10, 10, -10],
          opacity: [0.7, 1, 0.7],
        }}
        transition={{
          duration: 2.5 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      >
        Float
      </motion.div>
    </div>
  );
}

