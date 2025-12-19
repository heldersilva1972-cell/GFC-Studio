"use client";

import React from "react";
import { motion } from "framer-motion";
import type { AnimationProps } from "../types";

export default function PulseCircle({
  className = "",
  size = 200,
  speed = 1.0,
  colors = ["#3b82f6"],
}: AnimationProps) {
  return (
    <div
      className={`flex items-center justify-center ${className}`}
      style={{ width: size, height: size }}
    >
      <motion.div
        className="rounded-full"
        style={{
          width: size * 0.6,
          height: size * 0.6,
          backgroundColor: colors[0] || "#3b82f6",
        }}
        animate={{
          scale: [1, 1.2, 1],
          opacity: [1, 0.7, 1],
        }}
        transition={{
          duration: 2 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />
    </div>
  );
}

