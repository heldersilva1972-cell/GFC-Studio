"use client";

import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";

/**
 * Flip-Card Flag Transition
 * US flag card flips, mid-flip swaps to Portuguese flag, then both appear side-by-side
 */
export const FlipCardFlagTransition: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const [phase, setPhase] = useState<"us" | "flipping" | "portugal" | "both">("us");

  useEffect(() => {
    const timer1 = setTimeout(() => setPhase("flipping"), 2000 / speed);
    const timer2 = setTimeout(() => setPhase("portugal"), 3500 / speed);
    const timer3 = setTimeout(() => setPhase("both"), 5500 / speed);
    const timer4 = setTimeout(() => setPhase("us"), 9000 / speed);
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
          <pattern id="usFlagFlip" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect width="20" height="11.5" fill="#B22234" />
            <rect y="11.5" width="20" height="8.5" fill="#FFFFFF" />
            <rect width="8" height="11.5" fill="#3C3B6E" />
          </pattern>
          <pattern id="ptFlagFlip" x="0" y="0" width="40" height="60" patternUnits="userSpaceOnUse">
            <rect width="20" height="60" fill="#006600" />
            <rect x="20" width="20" height="60" fill="#FF0000" />
          </pattern>
        </defs>

        <AnimatePresence mode="wait">
          {phase === "us" && (
            <motion.rect
              key="us"
              x={size * 0.25}
              y={size * 0.2}
              width={size * 0.5}
              height={size * 0.6}
              fill="url(#usFlagFlip)"
              initial={{ rotateY: 0, opacity: 1 }}
              exit={{ rotateY: 90, opacity: 0 }}
              transition={{ duration: 0.5 / speed }}
            />
          )}

          {phase === "flipping" && (
            <motion.rect
              key="flip"
              x={size * 0.25}
              y={size * 0.2}
              width={size * 0.5}
              height={size * 0.6}
              fill="url(#ptFlagFlip)"
              initial={{ rotateY: 90, opacity: 0 }}
              animate={{ rotateY: 0, opacity: 1 }}
              exit={{ rotateY: -90, opacity: 0 }}
              transition={{ duration: 0.5 / speed }}
            />
          )}

          {phase === "portugal" && (
            <motion.rect
              key="pt"
              x={size * 0.25}
              y={size * 0.2}
              width={size * 0.5}
              height={size * 0.6}
              fill="url(#ptFlagFlip)"
              initial={{ rotateY: 0, opacity: 1 }}
              animate={{ rotateY: [0, 180, 360] }}
              transition={{ duration: 2 / speed, times: [0, 0.5, 1] }}
            />
          )}

          {phase === "both" && (
            <motion.g key="both">
              <motion.rect
                x={size * 0.15}
                y={size * 0.2}
                width={size * 0.35}
                height={size * 0.6}
                fill="url(#usFlagFlip)"
                initial={{ x: size * 0.25, opacity: 0 }}
                animate={{ x: size * 0.15, opacity: 1 }}
                transition={{ duration: 0.8 / speed }}
              />
              <motion.rect
                x={size * 0.5}
                y={size * 0.2}
                width={size * 0.35}
                height={size * 0.6}
                fill="url(#ptFlagFlip)"
                initial={{ x: size * 0.25, opacity: 0 }}
                animate={{ x: size * 0.5, opacity: 1 }}
                transition={{ duration: 0.8 / speed }}
              />
            </motion.g>
          )}
        </AnimatePresence>
      </svg>
    </div>
  );
};

export default FlipCardFlagTransition;

