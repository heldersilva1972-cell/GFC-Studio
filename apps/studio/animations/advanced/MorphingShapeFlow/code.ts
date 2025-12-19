export const morphingShapeFlowCode = `"use client";

import { motion } from "framer-motion";

// SVG path definitions for different shapes
const SHAPES = [
  // Circle
  "M 50,50 m -40,0 a 40,40 0 1,0 80,0 a 40,40 0 1,0 -80,0",
  // Squircle (rounded square)
  "M 30,20 L 70,20 Q 80,20 80,30 L 80,70 Q 80,80 70,80 L 30,80 Q 20,80 20,70 L 20,30 Q 20,20 30,20 Z",
  // Blob shape 1
  "M 50,20 Q 70,20 80,40 Q 85,60 70,75 Q 50,85 30,75 Q 15,60 20,40 Q 30,20 50,20 Z",
  // Star
  "M 50,15 L 55,35 L 75,35 L 60,48 L 65,68 L 50,55 L 35,68 L 40,48 L 25,35 L 45,35 Z",
  // Blob shape 2
  "M 50,25 Q 65,15 75,35 Q 80,55 65,70 Q 50,80 35,70 Q 20,55 25,35 Q 35,15 50,25 Z",
  // Hexagon
  "M 50,15 L 70,25 L 75,45 L 65,65 L 50,70 L 35,65 L 25,45 L 30,25 Z",
];

interface MorphingShapeFlowProps {
  className?: string;
  size?: number;
}

export default function MorphingShapeFlow({
  className = "",
  size = 200,
}: MorphingShapeFlowProps) {
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
        initial={{ rotate: 0 }}
        animate={{ rotate: 360 }}
        transition={{
          duration: 20,
          repeat: Infinity,
          ease: "linear",
        }}
      >
        <defs>
          <linearGradient
            id="morphingGradient"
            x1="0%"
            y1="0%"
            x2="100%"
            y2="100%"
            gradientUnits="userSpaceOnUse"
          >
            <motion.stop
              offset="0%"
              stopColor="#3b82f6"
              animate={{
                stopColor: [
                  "#3b82f6", // blue-500
                  "#8b5cf6", // violet-500
                  "#ec4899", // pink-500
                  "#f59e0b", // amber-500
                  "#10b981", // emerald-500
                  "#3b82f6", // back to blue-500
                ],
              }}
              transition={{
                duration: 8,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
            <motion.stop
              offset="100%"
              stopColor="#8b5cf6"
              animate={{
                stopColor: [
                  "#8b5cf6", // violet-500
                  "#ec4899", // pink-500
                  "#f59e0b", // amber-500
                  "#10b981", // emerald-500
                  "#3b82f6", // blue-500
                  "#8b5cf6", // back to violet-500
                ],
              }}
              transition={{
                duration: 8,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
          </linearGradient>
        </defs>
        <motion.path
          fill="url(#morphingGradient)"
          animate={{
            d: SHAPES,
          }}
          transition={{
            d: {
              duration: 12,
              repeat: Infinity,
              ease: "easeInOut",
              times: [0, 0.166, 0.333, 0.5, 0.666, 0.833, 1],
            },
          }}
        />
      </motion.svg>
    </div>
  );
}`;

