"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";

/**
 * CompressionMorph - Advanced compression-based morphing animation
 * 
 * Title: Compression Morph – Bars Squeeze "I LOVE YOU" into "I ❤️ U" and then a Heart
 * 
 * Description:
 * A unique compression animation where two horizontal bars squeeze the text "I LOVE YOU" from the sides,
 * compressing it into "I ❤️ U", and then further compressing into a single heart symbol. The animation
 * simulates physical compression with text morphing and bar movement.
 * 
 * Technical Breakdown:
 * - Text elements that morph between "I LOVE YOU", "I ❤️ U", and heart symbol
 * - Two animated compression bars (left and right) that move inward
 * - Text scaling and opacity changes during compression
 * - Smooth transitions between text states
 * - Visual feedback with bar shadows and text glow
 * 
 * SVG Path Plan:
 * - Text rendering using SVG text elements
 * - Compression bars as animated rectangles
 * - Heart symbol as SVG path
 * 
 * Framer Motion Plan:
 * - Animate bar positions (x translation)
 * - Animate text scale and opacity
 * - Animate text content changes
 * - 8-second cycle: expand → compress → expand
 */
interface CompressionMorphProps {
  className?: string;
  size?: number;
}

const TEXT_STATES = [
  { text: "I LOVE YOU", scale: 1, opacity: 1 },
  { text: "I ❤️ U", scale: 0.7, opacity: 0.9 },
  { text: "❤️", scale: 0.5, opacity: 0.8 },
];

export default function CompressionMorph({
  className = "",
  size = 400,
}: CompressionMorphProps) {
  const [stateIndex, setStateIndex] = useState(0);
  const [isCompressing, setIsCompressing] = useState(false);

  useEffect(() => {
    const sequence = async () => {
      // Start compression
      setIsCompressing(true);
      await new Promise((resolve) => setTimeout(resolve, 2000));
      setStateIndex(1);
      await new Promise((resolve) => setTimeout(resolve, 2000));
      setStateIndex(2);
      await new Promise((resolve) => setTimeout(resolve, 2000));
      // Expand back
      setIsCompressing(false);
      setStateIndex(1);
      await new Promise((resolve) => setTimeout(resolve, 2000));
      setStateIndex(0);
    };

    sequence();
    const interval = setInterval(sequence, 8000);
    return () => clearInterval(interval);
  }, []);

  const currentState = TEXT_STATES[stateIndex];
  const barOffset = isCompressing ? 30 : 0;

  return (
    <div
      className={`flex items-center justify-center ${className}`}
      style={{ width: size, height: size }}
    >
      <motion.svg
        width={size}
        height={size}
        viewBox="0 0 100 100"
        className="overflow-visible"
      >
        <defs>
          <linearGradient id="textGradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stopColor="#ec4899" />
            <stop offset="50%" stopColor="#f472b6" />
            <stop offset="100%" stopColor="#ec4899" />
          </linearGradient>
          <filter id="textGlow">
            <feGaussianBlur stdDeviation="1" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
          <filter id="barShadow">
            <feDropShadow dx="0" dy="2" stdDeviation="3" floodOpacity="0.5" />
          </filter>
        </defs>

        {/* Compression bars */}
        <motion.rect
          x={0}
          y={0}
          width={10}
          height={100}
          fill="url(#textGradient)"
          filter="url(#barShadow)"
          animate={{
            x: [0, barOffset, barOffset, 0],
          }}
          transition={{
            duration: 2,
            ease: "easeInOut",
          }}
        />
        <motion.rect
          x={90}
          y={0}
          width={10}
          height={100}
          fill="url(#textGradient)"
          filter="url(#barShadow)"
          animate={{
            x: [90, 90 - barOffset, 90 - barOffset, 90],
          }}
          transition={{
            duration: 2,
            ease: "easeInOut",
          }}
        />

        {/* Text content */}
        <AnimatePresence mode="wait">
          <motion.text
            key={stateIndex}
            x="50"
            y="50"
            textAnchor="middle"
            dominantBaseline="middle"
            fontSize="12"
            fill="url(#textGradient)"
            filter="url(#textGlow)"
            fontWeight="bold"
            initial={{ scale: 0.8, opacity: 0 }}
            animate={{
              scale: currentState.scale,
              opacity: currentState.opacity,
            }}
            exit={{ scale: 0.8, opacity: 0 }}
            transition={{
              duration: 0.5,
              ease: "easeInOut",
            }}
          >
            {currentState.text}
          </motion.text>
        </AnimatePresence>

        {/* Heart symbol (when fully compressed) */}
        {stateIndex === 2 && (
          <motion.path
            d="M 50,30 C 50,25 45,20 40,20 C 35,20 30,25 30,30 C 30,35 35,40 40,45 C 45,50 50,55 50,60 C 50,55 55,50 60,45 C 65,40 70,35 70,30 C 70,25 65,20 60,20 C 55,20 50,25 50,30 Z"
            fill="url(#textGradient)"
            filter="url(#textGlow)"
            initial={{ scale: 0, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            transition={{
              duration: 0.5,
              ease: "easeOut",
            }}
          />
        )}
      </motion.svg>
    </div>
  );
}

