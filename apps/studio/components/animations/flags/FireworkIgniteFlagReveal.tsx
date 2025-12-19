"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { USFlag, PortugueseFlag } from "./RealisticFlag";

/**
 * Fireworks Ignite Reveal
 * Fireworks explode on left (US colors) and right (PT colors), revealing flags behind
 */
export const FireworkIgniteFlagReveal: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const [phase, setPhase] = useState<"dark" | "fireworks" | "reveal" | "fade">("dark");
  const [fireworks, setFireworks] = useState<Array<{ id: number; x: number; y: number; color: string; delay: number }>>([]);
  const flagWidth = size * 0.35;
  const flagHeight = size * 0.5;
  const flagX1 = size * 0.1;
  const flagX2 = size * 0.55;
  const flagY = size * 0.25;

  useEffect(() => {
    const timer1 = setTimeout(() => {
      setPhase("fireworks");
      // Generate fireworks
      const newFireworks: Array<{ id: number; x: number; y: number; color: string; delay: number }> = [];
      for (let i = 0; i < 8; i++) {
        newFireworks.push({
          id: i,
          x: i < 4 ? size * 0.25 : size * 0.75,
          y: size * 0.3 + (i % 4) * (size * 0.2),
          color: i < 4 ? (i % 3 === 0 ? "#B22234" : i % 3 === 1 ? "#FFFFFF" : "#3C3B6E") : (i % 3 === 0 ? "#006600" : i % 3 === 1 ? "#FF0000" : "#FFD700"),
          delay: i * 0.3,
        });
      }
      setFireworks(newFireworks);
    }, 500 / speed);
    const timer2 = setTimeout(() => setPhase("reveal"), 4000 / speed);
    const timer3 = setTimeout(() => setPhase("fade"), 7000 / speed);
    const timer4 = setTimeout(() => setPhase("dark"), 8000 / speed);
    return () => {
      clearTimeout(timer1);
      clearTimeout(timer2);
      clearTimeout(timer3);
      clearTimeout(timer4);
    };
  }, [speed, size]);

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        {/* Background flags (revealed after fireworks) */}
        <motion.g
          initial={{ opacity: 0 }}
          animate={{ opacity: phase === "reveal" || phase === "fade" ? 1 : 0 }}
          transition={{ duration: 1 / speed }}
        >
          <g transform={`translate(${flagX1}, ${flagY})`}>
            <USFlag x={0} y={0} width={flagWidth} height={flagHeight} />
          </g>
          <g transform={`translate(${flagX2}, ${flagY})`}>
            <PortugueseFlag x={0} y={0} width={flagWidth} height={flagHeight} />
          </g>
        </motion.g>

        {/* Fireworks */}
        {fireworks.map((fw) => (
          <motion.g key={fw.id} initial={{ opacity: 0 }}>
            {Array.from({ length: 12 }, (_, i) => {
              const angle = (i / 12) * Math.PI * 2;
              const distance = size * 0.15;
              return (
                <motion.circle
                  key={i}
                  r={size * 0.01}
                  fill={fw.color}
                  initial={{ cx: fw.x, cy: fw.y, opacity: 0 }}
                  animate={{
                    cx: fw.x + Math.cos(angle) * distance,
                    cy: fw.y + Math.sin(angle) * distance,
                    opacity: phase === "fireworks" ? [0, 1, 0] : 0,
                    scale: [0, 1.5, 0],
                  }}
                  transition={{
                    duration: 1 / speed,
                    delay: fw.delay / speed,
                    repeat: phase === "fireworks" ? 1 : 0,
                    ease: "easeOut",
                  }}
                />
              );
            })}
          </motion.g>
        ))}
      </svg>
    </div>
  );
};

export default FireworkIgniteFlagReveal;
