"use client";

import React from "react";
import { motion } from "framer-motion";
import { USFlag, PortugueseFlag } from "./RealisticFlag";

/**
 * Wind-Wave Sync
 * Two flags side-by-side with synchronized wind motion simulating cloth waves
 */
export const DualFlagWindWaveSync: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  // Realistic flag proportions: 2:3 ratio - make flags larger and more visible
  const flagWidth = size * 0.4;
  const flagHeight = (flagWidth * 3) / 2; // 2:3 ratio
  const flagX1 = size * 0.05;
  const flagX2 = size * 0.5;
  const flagY = (size - flagHeight) / 2; // Center vertically

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        <motion.g
          animate={{
            x: [0, 2, -2, 1, -1, 0],
            y: [0, 1, -1, 0.5, -0.5, 0],
          }}
          transition={{
            duration: 4 / speed,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        >
          {/* US Flag with wave effect */}
          <motion.g
            transform={`translate(${flagX1}, ${flagY})`}
            animate={{
              rotate: [0, 1, -1, 0.5, -0.5, 0],
              scaleY: [1, 1.02, 0.98, 1.01, 0.99, 1],
            }}
            transition={{
              duration: 3 / speed,
              repeat: Infinity,
              ease: "easeInOut",
            }}
          >
            <USFlag x={0} y={0} width={flagWidth} height={flagHeight} />
          </motion.g>

          {/* PT Flag with synchronized wave */}
          <motion.g
            transform={`translate(${flagX2}, ${flagY})`}
            animate={{
              rotate: [0, -1, 1, -0.5, 0.5, 0],
              scaleY: [1, 1.02, 0.98, 1.01, 0.99, 1],
            }}
            transition={{
              duration: 3 / speed,
              repeat: Infinity,
              ease: "easeInOut",
            }}
          >
            <PortugueseFlag x={0} y={0} width={flagWidth} height={flagHeight} />
          </motion.g>
        </motion.g>
      </svg>
    </div>
  );
};

export default DualFlagWindWaveSync;
