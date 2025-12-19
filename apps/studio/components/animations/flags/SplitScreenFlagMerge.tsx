"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";
import { USFlag, PortugueseFlag } from "./RealisticFlag";

/**
 * Split-Screen Merge
 * Left half US flag, right half Portuguese flag, they slide together to merge in center
 */
export const SplitScreenFlagMerge: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const [phase, setPhase] = useState<"split" | "merged" | "swap">("split");
  const flagWidth = size * 0.5;
  const flagHeight = size * 0.5;

  useEffect(() => {
    const timer1 = setTimeout(() => setPhase("merged"), 2000 / speed);
    const timer2 = setTimeout(() => setPhase("swap"), 5000 / speed);
    const timer3 = setTimeout(() => setPhase("split"), 7000 / speed);
    return () => {
      clearTimeout(timer1);
      clearTimeout(timer2);
      clearTimeout(timer3);
    };
  }, [speed]);

  const usX = phase === "merged" ? size * 0.25 : phase === "swap" ? size * 0.5 : 0;
  const ptX = phase === "merged" ? size * 0.25 : phase === "swap" ? 0 : size * 0.5;
  const flagY = size * 0.25;

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        {/* US Flag - Left half */}
        <motion.g
          transform={`translate(${usX}, ${flagY})`}
          animate={{
            scaleY: [1, 1.05, 1],
          }}
          transition={{
            scaleY: {
              duration: 2 / speed,
              repeat: Infinity,
              ease: "easeInOut",
            },
          }}
        >
          <USFlag x={0} y={0} width={flagWidth} height={flagHeight} />
        </motion.g>

        {/* PT Flag - Right half */}
        <motion.g
          transform={`translate(${ptX}, ${flagY})`}
          animate={{
            scaleY: [1, 1.05, 1],
          }}
          transition={{
            scaleY: {
              duration: 2 / speed,
              repeat: Infinity,
              ease: "easeInOut",
            },
          }}
        >
          <PortugueseFlag x={0} y={0} width={flagWidth} height={flagHeight} />
        </motion.g>
      </svg>
    </div>
  );
};

export default SplitScreenFlagMerge;
