"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * Split Stripes Transform
 * US flag stripes peel downward and recolor to form Portuguese flag layout
 */
export const SplitStripesTransformFlag: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const [phase, setPhase] = useState<"us" | "peeling" | "portugal" | "reverse">("us");
  const stripes = Array.from({ length: 13 }, (_, i) => ({
    id: i,
    isRed: i % 2 === 0,
    y: (size * 0.2) + (i * (size * 0.6 / 13)),
    height: size * 0.6 / 13,
  }));

  useEffect(() => {
    const timer1 = setTimeout(() => setPhase("peeling"), 2000 / speed);
    const timer2 = setTimeout(() => setPhase("portugal"), 5000 / speed);
    const timer3 = setTimeout(() => setPhase("reverse"), 8000 / speed);
    const timer4 = setTimeout(() => setPhase("us"), 11000 / speed);
    return () => {
      clearTimeout(timer1);
      clearTimeout(timer2);
      clearTimeout(timer3);
      clearTimeout(timer4);
    };
  }, [speed]);

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        <defs>
          <pattern id="usStars" x="0" y="0" width="8" height="11.5" patternUnits="userSpaceOnUse">
            <rect width="8" height="11.5" fill="#3C3B6E" />
            <circle cx="2" cy="2" r="0.8" fill="#FFFFFF" />
            <circle cx="6" cy="2" r="0.8" fill="#FFFFFF" />
            <circle cx="4" cy="4" r="0.8" fill="#FFFFFF" />
          </pattern>
        </defs>

        {/* US Stars area morphing to Portuguese emblem */}
        <motion.rect
          x={size * 0.15}
          y={size * 0.2}
          width={size * 0.2}
          height={size * 0.3}
          fill={phase === "portugal" ? "#FFFFFF" : "url(#usStars)"}
          animate={{
            x: phase === "portugal" ? size * 0.4 : size * 0.15,
            y: phase === "portugal" ? size * 0.35 : size * 0.2,
            width: phase === "portugal" ? size * 0.2 : size * 0.2,
            height: phase === "portugal" ? size * 0.3 : size * 0.3,
          }}
          transition={{ duration: 2 / speed, ease: "easeInOut" }}
        />

        {/* Stripes peeling and transforming */}
        {stripes.map((stripe, i) => {
          const targetColor = phase === "portugal" || phase === "reverse"
            ? i < 6.5 ? "#006600" : "#FF0000"
            : stripe.isRed ? "#B22234" : "#FFFFFF";

          return (
            <motion.rect
              key={stripe.id}
              x={phase === "portugal" ? (i < 6.5 ? size * 0.15 : size * 0.35) : size * 0.15}
              y={stripe.y}
              width={phase === "portugal" ? size * 0.2 : size * 0.7}
              height={stripe.height}
              fill={targetColor}
              animate={{
                x: phase === "peeling"
                  ? [size * 0.15, size * 0.15 + Math.sin(i) * 20, phase === "portugal" ? (i < 6.5 ? size * 0.15 : size * 0.35) : size * 0.15]
                  : phase === "portugal"
                  ? (i < 6.5 ? size * 0.15 : size * 0.35)
                  : size * 0.15,
                rotate: phase === "peeling" ? [0, i % 2 === 0 ? 5 : -5, 0] : 0,
                y: phase === "peeling" ? [stripe.y, stripe.y + 30, stripe.y] : stripe.y,
              }}
              transition={{
                duration: phase === "peeling" ? 2 / speed : 1.5 / speed,
                delay: i * 0.1 / speed,
                ease: "easeInOut",
              }}
            />
          );
        })}
      </svg>
    </div>
  );
};

export default SplitStripesTransformFlag;

