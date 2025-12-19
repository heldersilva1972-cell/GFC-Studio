"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";

/**
 * Star-to-Shield Morph
 * US flag stars detach and cluster, morphing into Portuguese coat-of-arms shield
 * while background transitions from stripes to green/red
 */
export const StarToShieldMorph: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const [phase, setPhase] = useState<"us" | "morphing" | "portugal">("us");
  const flagWidth = size * 0.7;
  const flagHeight = size * 0.8;
  const stripeHeight = flagHeight / 13;
  const cantonWidth = flagWidth * 0.3846;
  const cantonHeight = flagHeight * 0.5385;

  useEffect(() => {
    const timer = setTimeout(() => setPhase("morphing"), 2000 / speed);
    const timer2 = setTimeout(() => setPhase("portugal"), 4000 / speed);
    const timer3 = setTimeout(() => setPhase("us"), 8000 / speed);
    return () => {
      clearTimeout(timer);
      clearTimeout(timer2);
      clearTimeout(timer3);
    };
  }, [speed]);

  // Generate 50 stars for US flag canton
  const generateStars = () => {
    const stars: Array<{ id: number; x: number; y: number }> = [];
    const starRows = [6, 5, 6, 5, 6, 5, 6, 5, 6];
    starRows.forEach((count, row) => {
      const offset = count === 5 ? cantonWidth / 12 : 0;
      for (let col = 0; col < count; col++) {
        const x = size * 0.15 + offset + (col * cantonWidth / 6) + (count === 6 ? cantonWidth / 12 : 0);
        const y = size * 0.1 + (row + 1) * cantonHeight / 10;
        stars.push({ id: row * 10 + col, x, y });
      }
    });
    return stars;
  };

  const stars = generateStars();

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        <defs>
          {/* US Flag - Stripes pattern */}
          <pattern id="usStripesMorph" x="0" y="0" width={flagWidth} height={flagHeight} patternUnits="userSpaceOnUse">
            <rect width={flagWidth} height={stripeHeight} y={0} fill="#B22234" />
            <rect width={flagWidth} height={stripeHeight} y={stripeHeight * 2} fill="#B22234" />
            <rect width={flagWidth} height={stripeHeight} y={stripeHeight * 4} fill="#B22234" />
            <rect width={flagWidth} height={stripeHeight} y={stripeHeight * 6} fill="#B22234" />
            <rect width={flagWidth} height={stripeHeight} y={stripeHeight * 8} fill="#B22234" />
            <rect width={flagWidth} height={stripeHeight} y={stripeHeight * 10} fill="#B22234" />
            <rect width={flagWidth} height={stripeHeight} y={stripeHeight * 12} fill="#B22234" />
          </pattern>
        </defs>

        {/* Background transition */}
        <motion.rect
          width={size}
          height={size}
          fill="url(#usStripesMorph)"
          animate={{
            fill: phase === "portugal" ? "#006600" : phase === "morphing" ? "#4a7c59" : undefined,
          }}
          transition={{ duration: 2 / speed }}
        />
        <motion.rect
          x={size / 2}
          width={size / 2}
          height={size}
          fill={phase === "portugal" ? "#FF0000" : "transparent"}
          initial={{ opacity: 0 }}
          animate={{ opacity: phase === "portugal" ? 1 : 0 }}
          transition={{ duration: 2 / speed }}
        />

        {/* US Stars - 50 stars in canton */}
        <AnimatePresence>
          {phase === "us" && (
            <g>
              {stars.map((star) => {
                const starSize = cantonWidth * 0.012;
                const angle = (Math.PI * 2) / 5;
                const points = Array.from({ length: 5 }, (_, i) => {
                  const a = i * angle - Math.PI / 2;
                  const r = i % 2 === 0 ? starSize : starSize * 0.4;
                  return `${star.x + Math.cos(a) * r},${star.y + Math.sin(a) * r}`;
                }).join(" ");
                return (
                  <motion.polygon
                    key={star.id}
                    points={points}
                    fill="#FFFFFF"
                    initial={{ opacity: 1, scale: 1 }}
                    exit={{
                      x: size / 2,
                      y: size / 2,
                      scale: 0.3,
                      opacity: 0,
                    }}
                    transition={{ duration: 1.5 / speed }}
                  />
                );
              })}
            </g>
          )}
        </AnimatePresence>

        {/* Clustered stars morphing into shield */}
        {phase === "morphing" && (
          <motion.g
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
          >
            {stars.map((star, i) => {
              const starSize = cantonWidth * 0.012;
              const angle = (Math.PI * 2) / 5;
              const points = Array.from({ length: 5 }, (_, j) => {
                const a = j * angle - Math.PI / 2;
                const r = j % 2 === 0 ? starSize : starSize * 0.4;
                return `${star.x + Math.cos(a) * r},${star.y + Math.sin(a) * r}`;
              }).join(" ");
              return (
                <motion.polygon
                  key={star.id}
                  points={points}
                  fill="#FFFFFF"
                  animate={{
                    x: size / 2,
                    y: size / 2,
                    scale: 0.2,
                    rotate: i * 27.7,
                  }}
                  transition={{ duration: 1.5 / speed, delay: i * 0.05 }}
                />
              );
            })}
          </motion.g>
        )}

        {/* Portuguese Shield */}
        {phase === "portugal" && (
          <motion.g
            transform={`translate(${size / 2}, ${size / 2})`}
            initial={{ opacity: 0, scale: 0.5 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ duration: 1 / speed }}
          >
            {/* Shield shape */}
            <path
              d={`M 0,-${size * 0.15} 
                  L -${size * 0.15},-${size * 0.05} 
                  L -${size * 0.15},${size * 0.15} 
                  Q -${size * 0.15},${size * 0.2} 0,${size * 0.2} 
                  Q ${size * 0.15},${size * 0.2} ${size * 0.15},${size * 0.15} 
                  L ${size * 0.15},-${size * 0.05} Z`}
              fill="#FFFFFF"
              stroke="#FFD700"
              strokeWidth="4"
            />
            {/* Simplified coat of arms elements */}
            <rect x={-size * 0.08} y={-size * 0.05} width={size * 0.06} height={size * 0.08} fill="#006600" />
            <rect x={size * 0.02} y={-size * 0.05} width={size * 0.06} height={size * 0.08} fill="#006600" />
            <rect x={-size * 0.03} y={-size * 0.08} width={size * 0.06} height={size * 0.06} fill="#006600" />
            <circle cx={0} cy={size * 0.05} r={size * 0.08} fill="none" stroke="#FFD700" strokeWidth="2" />
            <line x1={0} y1={size * 0.01} x2={0} y2={size * 0.09} stroke="#FFD700" strokeWidth="1.5" />
            <line x1={-size * 0.04} y1={size * 0.05} x2={size * 0.04} y2={size * 0.05} stroke="#FFD700" strokeWidth="1.5" />
          </motion.g>
        )}
      </svg>
    </div>
  );
};

export default StarToShieldMorph;
