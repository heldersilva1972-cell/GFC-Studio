export const liquidMetalMorphCode = `"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * LiquidMetalMorph - Advanced morphing animation
 * 
 * Title: Liquid Metal Morph – I → Heart → U
 * 
 * Description:
 * A fluid, metallic morphing animation that smoothly transitions between three shapes:
 * the letter "I", a heart symbol, and the letter "U". The animation uses SVG path morphing
 * with a liquid metal aesthetic, featuring reflective gradients and smooth bezier transitions.
 */
interface LiquidMetalMorphProps {
  className?: string;
  size?: number;
}

const SHAPES = [
  // Letter "I" - vertical line with serifs
  "M 30,20 L 70,20 L 70,30 L 55,30 L 55,70 L 70,70 L 70,80 L 30,80 L 30,70 L 45,70 L 45,30 L 30,30 Z",
  // Heart shape
  "M 50,30 C 50,25 45,20 40,20 C 35,20 30,25 30,30 C 30,35 35,40 40,45 C 45,50 50,55 50,60 C 50,55 55,50 60,45 C 65,40 70,35 70,30 C 70,25 65,20 60,20 C 55,20 50,25 50,30 Z",
  // Letter "U"
  "M 30,20 L 30,50 C 30,60 35,65 40,65 C 45,65 50,60 50,50 L 50,20 L 60,20 L 60,50 C 60,65 50,75 40,75 C 30,75 20,65 20,50 L 20,20 Z",
];

export default function LiquidMetalMorph({
  className = "",
  size = 400,
}: LiquidMetalMorphProps) {
  const [currentShapeIndex, setCurrentShapeIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentShapeIndex((prev) => (prev + 1) % SHAPES.length);
    }, 4000);
    return () => clearInterval(interval);
  }, []);

  return (
    <div
      className={\`flex items-center justify-center \${className}\`}
      style={{ width: size, height: size }}
    >
      <motion.svg
        width={size}
        height={size}
        viewBox="0 0 100 100"
        className="overflow-visible"
      >
        <defs>
          <linearGradient id="liquidMetalGradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <motion.stop
              offset="0%"
              stopColor="#cbd5e1"
              animate={{
                stopColor: ["#cbd5e1", "#94a3b8", "#64748b", "#475569", "#cbd5e1"],
              }}
              transition={{
                duration: 6,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
            <motion.stop
              offset="50%"
              stopColor="#f1f5f9"
              animate={{
                stopColor: ["#f1f5f9", "#e2e8f0", "#cbd5e1", "#94a3b8", "#f1f5f9"],
              }}
              transition={{
                duration: 6,
                repeat: Infinity,
                ease: "easeInOut",
                delay: 0.2,
              }}
            />
            <motion.stop
              offset="100%"
              stopColor="#64748b"
              animate={{
                stopColor: ["#64748b", "#475569", "#334155", "#1e293b", "#64748b"],
              }}
              transition={{
                duration: 6,
                repeat: Infinity,
                ease: "easeInOut",
                delay: 0.4,
              }}
            />
          </linearGradient>
          <filter id="metalGlow">
            <feGaussianBlur stdDeviation="2" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>
        <motion.path
          d={SHAPES[currentShapeIndex]}
          fill="url(#liquidMetalGradient)"
          filter="url(#metalGlow)"
          animate={{
            d: SHAPES,
          }}
          transition={{
            d: {
              duration: 4,
              repeat: Infinity,
              ease: "easeInOut",
              times: [0, 0.33, 0.66, 1],
            },
          }}
        />
      </motion.svg>
    </div>
  );
}`;

