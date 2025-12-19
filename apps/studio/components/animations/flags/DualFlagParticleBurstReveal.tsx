"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * Particle Burst Flag Reveal
 * Particles burst from center, then fall back to assemble into two flags
 */
export const DualFlagParticleBurstReveal: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const [phase, setPhase] = useState<"burst" | "assemble" | "glow">("burst");
  const flagWidth = size * 0.35;
  const flagHeight = size * 0.6;
  const stripeHeight = flagHeight / 13;
  const cantonWidth = flagWidth * 0.3846;
  const cantonHeight = flagHeight * 0.5385;

  // Create particles that will form the flags
  const particles = Array.from({ length: 200 }, (_, i) => {
    // US flag particles (first 100)
    if (i < 100) {
      const row = Math.floor(i / 10);
      const col = i % 10;
      const isStripe = row < 7 || (row >= 7 && col < 4); // First 7 rows or canton area
      const isRedStripe = row % 2 === 0;
      const isCanton = row < 7 && col < 4;
      return {
        id: i,
        color: isCanton ? "#3C3B6E" : isRedStripe ? "#B22234" : "#FFFFFF",
        targetX: size * 0.15 + (col * flagWidth / 10),
        targetY: size * 0.2 + (row * flagHeight / 10),
        size: isCanton ? size * 0.008 : size * 0.01,
      };
    } else {
      // PT flag particles (last 100)
      const row = Math.floor((i - 100) / 10);
      const col = (i - 100) % 10;
      const isGreen = col < 5;
      const isCoatOfArms = row >= 3 && row <= 6 && col >= 4 && col <= 6;
      return {
        id: i,
        color: isCoatOfArms ? "#FFFFFF" : isGreen ? "#006600" : "#FF0000",
        targetX: size * 0.5 + (col * flagWidth / 10),
        targetY: size * 0.2 + (row * flagHeight / 10),
        size: isCoatOfArms ? size * 0.006 : size * 0.01,
      };
    }
  });

  useEffect(() => {
    const timer1 = setTimeout(() => setPhase("assemble"), 1500 / speed);
    const timer2 = setTimeout(() => setPhase("glow"), 4000 / speed);
    const timer3 = setTimeout(() => setPhase("burst"), 8000 / speed);
    return () => {
      clearTimeout(timer1);
      clearTimeout(timer2);
      clearTimeout(timer3);
    };
  }, [speed]);

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        {particles.map((particle) => {
          const angle = (particle.id / particles.length) * Math.PI * 2;
          const burstDistance = size * 0.45;
          const burstX = size / 2 + Math.cos(angle) * burstDistance;
          const burstY = size / 2 + Math.sin(angle) * burstDistance;

          return (
            <motion.circle
              key={particle.id}
              r={particle.size}
              fill={particle.color}
              initial={{ cx: size / 2, cy: size / 2, opacity: 0 }}
              animate={{
                cx: phase === "burst" ? burstX : particle.targetX,
                cy: phase === "burst" ? burstY : particle.targetY,
                opacity: phase === "glow" ? [0.8, 1, 0.8] : 1,
                scale: phase === "glow" ? [1, 1.2, 1] : 1,
              }}
              transition={{
                duration: phase === "burst" ? 1.5 / speed : phase === "assemble" ? 2 / speed : 2 / speed,
                repeat: phase === "glow" ? Infinity : 0,
                ease: phase === "burst" ? "easeOut" : "easeInOut",
                delay: phase === "assemble" ? (particle.id % 20) * 0.02 / speed : 0,
              }}
            />
          );
        })}
      </svg>
    </div>
  );
};

export default DualFlagParticleBurstReveal;
