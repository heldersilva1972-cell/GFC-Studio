"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * OrigamiFoldMorph - Advanced origami-style folding morphing animation
 * 
 * Title: Origami Fold Morph – Folding Paper Transforms Shapes
 * 
 * Description:
 * An origami-inspired animation where paper-like shapes fold and unfold to transform between
 * "I", a heart, and "U". The animation uses 3D perspective transforms to simulate paper folding,
 * with crease lines and shadow effects that enhance the illusion of depth.
 * 
 * Technical Breakdown:
 * - Multiple SVG polygons representing paper segments
 * - 3D transform animations using perspective and rotateX/rotateY
 * - Animated crease lines that appear during folding
 * - Shadow effects for depth perception
 * - Sequential folding/unfolding animations
 * 
 * SVG Path Plan:
 * - I shape: Vertical rectangle segments
 * - Heart shape: Curved segments that fold into heart
 * - U shape: Curved bottom with vertical sides
 * 
 * Framer Motion Plan:
 * - Animate transform-style: preserve-3d
 * - Animate rotateX and rotateY for folding effect
 * - Animate opacity for crease lines
 * - 5-second cycle with folding/unfolding phases
 */
interface OrigamiFoldMorphProps {
  className?: string;
  size?: number;
}

const SHAPE_SEGMENTS = [
  // I shape - vertical segments
  [
    { points: "30,20 70,20 70,30 30,30", transform: "translateZ(0)" },
    { points: "30,30 70,30 70,70 30,70", transform: "translateZ(0)" },
    { points: "30,70 70,70 70,80 30,80", transform: "translateZ(0)" },
  ],
  // Heart shape - folded segments
  [
    { points: "40,20 50,30 50,40 40,50", transform: "translateZ(0) rotateX(0deg)" },
    { points: "50,30 60,20 60,40 50,50", transform: "translateZ(0) rotateX(0deg)" },
    { points: "40,50 50,50 50,60 45,65 40,60", transform: "translateZ(0) rotateX(0deg)" },
    { points: "50,50 60,50 60,60 55,65 50,60", transform: "translateZ(0) rotateX(0deg)" },
  ],
  // U shape - curved segments
  [
    { points: "30,20 30,50 35,55 40,55", transform: "translateZ(0)" },
    { points: "40,55 50,55 50,50 50,20", transform: "translateZ(0)" },
    { points: "50,20 70,20 70,50 65,55 60,55", transform: "translateZ(0)" },
    { points: "60,55 50,55 50,60 55,65 60,60", transform: "translateZ(0)" },
  ],
];

export default function OrigamiFoldMorph({
  className = "",
  size = 400,
}: OrigamiFoldMorphProps) {
  const [currentShapeIndex, setCurrentShapeIndex] = useState(0);
  const [isFolding, setIsFolding] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      setIsFolding(true);
      setTimeout(() => {
        setCurrentShapeIndex((prev) => (prev + 1) % SHAPE_SEGMENTS.length);
        setIsFolding(false);
      }, 1000);
    }, 5000);
    return () => clearInterval(interval);
  }, []);

  const currentSegments = SHAPE_SEGMENTS[currentShapeIndex];

  return (
    <div
      className={`flex items-center justify-center ${className}`}
      style={{ width: size, height: size, perspective: "1000px" }}
    >
      <motion.svg
        width={size}
        height={size}
        viewBox="0 0 100 100"
        className="overflow-visible"
        style={{ transformStyle: "preserve-3d" }}
      >
        <defs>
          <linearGradient id="origamiGradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <stop offset="0%" stopColor="#fef3c7" />
            <stop offset="50%" stopColor="#fde68a" />
            <stop offset="100%" stopColor="#f59e0b" />
          </linearGradient>
          <filter id="paperShadow">
            <feDropShadow dx="2" dy="2" stdDeviation="3" floodOpacity="0.3" />
          </filter>
        </defs>
        {currentSegments.map((segment, index) => (
          <motion.polygon
            key={`${currentShapeIndex}-${index}`}
            points={segment.points}
            fill="url(#origamiGradient)"
            stroke="#d97706"
            strokeWidth="0.5"
            filter="url(#paperShadow)"
            initial={{ opacity: 0, rotateX: isFolding ? 90 : 0 }}
            animate={{
              opacity: 1,
              rotateX: isFolding ? [0, 90, 0] : 0,
              rotateY: isFolding ? [0, 45, 0] : 0,
            }}
            transition={{
              duration: 1,
              delay: index * 0.1,
              ease: "easeInOut",
            }}
            style={{
              transform: segment.transform,
              transformStyle: "preserve-3d",
            }}
          />
        ))}
        {/* Crease lines */}
        {isFolding && (
          <motion.line
            x1="50"
            y1="0"
            x2="50"
            y2="100"
            stroke="#d97706"
            strokeWidth="0.5"
            strokeDasharray="2,2"
            initial={{ opacity: 0 }}
            animate={{ opacity: [0, 1, 0] }}
            transition={{ duration: 1 }}
          />
        )}
      </motion.svg>
    </div>
  );
}

