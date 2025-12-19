"use client";

import React from "react";
import { motion } from "framer-motion";
import type { AnimationProps } from "../types";

export default function SlidingSquare({
  className = "",
  size = 200,
  speed = 1.0,
  colors = ["#8b5cf6"],
}: AnimationProps) {
  const squareSize = size * 0.3;
  const containerSize = size;

  return (
    <div
      className={`flex items-center justify-center overflow-hidden ${className}`}
      style={{ width: containerSize, height: containerSize }}
    >
      <motion.div
        style={{
          width: squareSize,
          height: squareSize,
          backgroundColor: colors[0] || "#8b5cf6",
        }}
        animate={{
          x: [
            -containerSize / 2 + squareSize / 2,
            containerSize / 2 - squareSize / 2,
            -containerSize / 2 + squareSize / 2,
          ],
        }}
        transition={{
          duration: 3 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />
    </div>
  );
}

