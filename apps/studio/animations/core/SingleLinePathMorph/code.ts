export const singleLinePathMorphCode = `"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * SingleLinePathMorph - Advanced single-stroke morphing animation
 * 
 * Title: Single-Line Path Morph – One Continuous Glowing Stroke
 * 
 * Description:
 * A mesmerizing single continuous stroke that morphs through the shapes of "I", heart, and "U"
 * without lifting the pen. The animation creates a flowing, calligraphic effect with a glowing
 * neon trail that follows the path as it transforms.
 */
interface SingleLinePathMorphProps {
  className?: string;
  size?: number;
}

const PATHS = [
  // I shape - single continuous stroke
  "M 50,20 L 50,30 M 35,20 L 65,20 M 50,30 L 50,70 M 35,80 L 65,80 M 50,70 L 50,80",
  // Heart shape - single continuous stroke
  "M 50,40 L 45,35 Q 40,30 35,35 Q 30,40 35,45 Q 40,50 45,55 L 50,60 L 55,55 Q 60,50 65,45 Q 70,40 65,35 Q 60,30 55,35 Z",
  // U shape - single continuous stroke
  "M 30,20 L 30,50 Q 30,65 40,65 Q 50,65 50,50 L 50,20 M 50,20 L 70,20 L 70,50 Q 70,65 60,65 Q 50,65 50,50",
];

export default function SingleLinePathMorph({
  className = "",
  size = 400,
}: SingleLinePathMorphProps) {
  const [pathLength, setPathLength] = useState(0);
  const pathRef = React.useRef<SVGPathElement>(null);

  useEffect(() => {
    if (pathRef.current) {
      const length = pathRef.current.getTotalLength();
      setPathLength(length);
    }
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
          <filter id="neonGlow">
            <feGaussianBlur stdDeviation="3" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>
        <motion.path
          ref={pathRef}
          d={PATHS[0]}
          fill="none"
          strokeWidth="3"
          strokeLinecap="round"
          strokeLinejoin="round"
          filter="url(#neonGlow)"
          animate={{
            d: PATHS,
            stroke: ["#3b82f6", "#ec4899", "#8b5cf6", "#3b82f6"],
            strokeDasharray: pathLength > 0 ? [pathLength, pathLength] : "none",
            strokeDashoffset: [0, -pathLength, 0],
          }}
          transition={{
            d: {
              duration: 5,
              repeat: Infinity,
              ease: "easeInOut",
              times: [0, 0.33, 0.66, 1],
            },
            stroke: {
              duration: 5,
              repeat: Infinity,
              ease: "easeInOut",
            },
            strokeDashoffset: {
              duration: 2,
              repeat: Infinity,
              ease: "linear",
            },
          }}
        />
      </motion.svg>
    </div>
  );
}`;

