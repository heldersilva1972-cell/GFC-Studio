"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * FireIceDualMorph - Advanced dual-element morphing animation
 * 
 * Title: Fire & Ice Dual Morph – Two-Element Morph
 * 
 * Description:
 * A dramatic dual-element animation featuring two contrasting elements: fire (red/orange) and ice (blue/cyan).
 * The fire element forms "I", the ice element forms "U", and they merge together to create a heart in the middle.
 * The animation showcases the contrast between hot and cold, with particle effects and color gradients.
 * 
 * Technical Breakdown:
 * - Two separate SVG paths for fire and ice elements
 * - Fire: Red/orange gradient with flickering animation
 * - Ice: Blue/cyan gradient with crystalline effects
 * - Both elements morph independently then merge
 * - Particle overlay effects for fire sparks and ice crystals
 * - Synchronized timing for dramatic effect
 * 
 * SVG Path Plan:
 * - Fire path: Forms "I" shape, then morphs to left half of heart
 * - Ice path: Forms "U" shape, then morphs to right half of heart
 * - Combined: Full heart shape when merged
 * 
 * Framer Motion Plan:
 * - Animate two separate path elements
 * - Fire: Animate path + color gradient + flicker effect
 * - Ice: Animate path + color gradient + shimmer effect
 * - Synchronized morphing with 6-second cycle
 */
interface FireIceDualMorphProps {
  className?: string;
  size?: number;
}

const FIRE_PATHS = [
  // I shape
  "M 30,20 L 70,20 L 70,30 L 55,30 L 55,70 L 70,70 L 70,80 L 30,80 L 30,70 L 45,70 L 45,30 L 30,30 Z",
  // Left half of heart
  "M 50,30 C 50,25 45,20 40,20 C 35,20 30,25 30,30 C 30,35 35,40 40,45 C 45,50 50,55 50,60 L 50,30 Z",
];

const ICE_PATHS = [
  // U shape
  "M 30,20 L 30,50 C 30,60 35,65 40,65 C 45,65 50,60 50,50 L 50,20 L 60,20 L 60,50 C 60,65 50,75 40,75 C 30,75 20,65 20,50 L 20,20 Z",
  // Right half of heart
  "M 50,30 C 50,25 55,20 60,20 C 65,20 70,25 70,30 C 70,35 65,40 60,45 C 55,50 50,55 50,60 L 50,30 Z",
];

export default function FireIceDualMorph({
  className = "",
  size = 400,
}: FireIceDualMorphProps) {
  const [phase, setPhase] = useState<"separate" | "merge">("separate");

  useEffect(() => {
    const interval = setInterval(() => {
      setPhase((prev) => (prev === "separate" ? "merge" : "separate"));
    }, 3000);
    return () => clearInterval(interval);
  }, []);

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
          {/* Fire gradient */}
          <linearGradient id="fireGradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <motion.stop
              offset="0%"
              stopColor="#f97316"
              animate={{
                stopColor: ["#f97316", "#ef4444", "#dc2626", "#f97316"],
              }}
              transition={{
                duration: 1,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
            <motion.stop
              offset="50%"
              stopColor="#fb923c"
              animate={{
                stopColor: ["#fb923c", "#f97316", "#ef4444", "#fb923c"],
              }}
              transition={{
                duration: 1,
                repeat: Infinity,
                ease: "easeInOut",
                delay: 0.2,
              }}
            />
            <motion.stop
              offset="100%"
              stopColor="#fbbf24"
              animate={{
                stopColor: ["#fbbf24", "#fb923c", "#f97316", "#fbbf24"],
              }}
              transition={{
                duration: 1,
                repeat: Infinity,
                ease: "easeInOut",
                delay: 0.4,
              }}
            />
          </linearGradient>
          {/* Ice gradient */}
          <linearGradient id="iceGradient" x1="0%" y1="0%" x2="100%" y2="100%">
            <motion.stop
              offset="0%"
              stopColor="#0ea5e9"
              animate={{
                stopColor: ["#0ea5e9", "#06b6d4", "#0891b2", "#0ea5e9"],
              }}
              transition={{
                duration: 1.5,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
            <motion.stop
              offset="50%"
              stopColor="#38bdf8"
              animate={{
                stopColor: ["#38bdf8", "#22d3ee", "#0ea5e9", "#38bdf8"],
              }}
              transition={{
                duration: 1.5,
                repeat: Infinity,
                ease: "easeInOut",
                delay: 0.3,
              }}
            />
            <motion.stop
              offset="100%"
              stopColor="#e0f2fe"
              animate={{
                stopColor: ["#e0f2fe", "#bae6fd", "#7dd3fc", "#e0f2fe"],
              }}
              transition={{
                duration: 1.5,
                repeat: Infinity,
                ease: "easeInOut",
                delay: 0.6,
              }}
            />
          </linearGradient>
          <filter id="fireGlow">
            <feGaussianBlur stdDeviation="2" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
          <filter id="iceGlow">
            <feGaussianBlur stdDeviation="2" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>
        {/* Fire element */}
        <motion.path
          d={phase === "separate" ? FIRE_PATHS[0] : FIRE_PATHS[1]}
          fill="url(#fireGradient)"
          filter="url(#fireGlow)"
          animate={{
            d: phase === "separate" ? FIRE_PATHS[0] : FIRE_PATHS[1],
            opacity: [1, 0.9, 1],
          }}
          transition={{
            d: {
              duration: 1.5,
              ease: "easeInOut",
            },
            opacity: {
              duration: 0.5,
              repeat: Infinity,
              ease: "easeInOut",
            },
          }}
        />
        {/* Ice element */}
        <motion.path
          d={phase === "separate" ? ICE_PATHS[0] : ICE_PATHS[1]}
          fill="url(#iceGradient)"
          filter="url(#iceGlow)"
          animate={{
            d: phase === "separate" ? ICE_PATHS[0] : ICE_PATHS[1],
            opacity: [1, 0.95, 1],
          }}
          transition={{
            d: {
              duration: 1.5,
              ease: "easeInOut",
            },
            opacity: {
              duration: 0.8,
              repeat: Infinity,
              ease: "easeInOut",
            },
          }}
        />
      </motion.svg>
    </div>
  );
}

