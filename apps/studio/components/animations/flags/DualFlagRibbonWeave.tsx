"use client";

import React from "react";
import { motion } from "framer-motion";
import { USFlag, PortugueseFlag } from "./RealisticFlag";

/**
 * Dual-Flag Ribbon Weave
 * Both flags start as horizontal ribbons off-screen, weave over/under each other,
 * then snap into full flags side-by-side with gentle swaying
 */
export const DualFlagRibbonWeave: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  // Realistic flag proportions: US flag is 10:19, Portuguese is 2:3
  // Using 2:3 for both for consistency - make flags larger and more visible
  const flagWidth = size * 0.45;
  const flagHeight = (flagWidth * 3) / 2; // 2:3 ratio
  const ribbonHeight = size * 0.2;
  const flagX1 = size * 0.05;
  const flagX2 = size * 0.5;
  const flagY = (size - flagHeight) / 2; // Center vertically

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        {/* Phase 1: Ribbons weaving */}
        <motion.g
          initial={{ opacity: 1 }}
          animate={{
            opacity: [1, 1, 0],
          }}
          transition={{
            duration: 4 / speed,
            times: [0, 0.7, 1],
          }}
        >
          {/* US Ribbon from left */}
          <motion.g 
            initial={{ x: -size * 0.3, y: size * 0.4 }} 
            animate={{
              x: [size * 0.1, size * 0.3, size * 0.2, size * 0.35],
              y: [size * 0.4, size * 0.35, size * 0.45, size * 0.4],
            }} 
            transition={{
              duration: 3 / speed,
              times: [0, 0.33, 0.66, 1],
              ease: "easeInOut",
            }}
          >
            <USFlag x={0} y={0} width={size * 0.6} height={ribbonHeight} />
          </motion.g>
          
          {/* PT Ribbon from right */}
          <motion.g 
            initial={{ x: size * 0.7, y: size * 0.4 }} 
            animate={{
              x: [size * 0.3, size * 0.1, size * 0.2, size * 0.15],
              y: [size * 0.4, size * 0.45, size * 0.35, size * 0.4],
            }} 
            transition={{
              duration: 3 / speed,
              times: [0, 0.33, 0.66, 1],
              ease: "easeInOut",
            }}
          >
            <PortugueseFlag x={0} y={0} width={size * 0.6} height={ribbonHeight} />
          </motion.g>
        </motion.g>

        {/* Phase 2: Full flags side-by-side */}
        <motion.g
          initial={{ opacity: 0 }}
          animate={{
            opacity: [0, 0, 1],
          }}
          transition={{
            duration: 4 / speed,
            times: [0, 0.7, 1],
          }}
        >
          {/* US Flag */}
          <motion.g
            transform={`translate(${flagX1}, ${flagY})`}
            initial={{ scale: 0.8, opacity: 0 }}
            animate={{
              scale: [0.8, 1.05, 1],
              opacity: [0, 1, 1],
              rotate: [0, 2, -1, 0.5, -0.5, 0],
            }}
            transition={{
              scale: {
                duration: 0.5 / speed,
                delay: 3 / speed,
              },
              opacity: {
                duration: 0.3 / speed,
                delay: 3 / speed,
              },
              rotate: {
                duration: 4 / speed,
                delay: 3.5 / speed,
                repeat: Infinity,
                ease: "easeInOut",
              },
          }}>
            <USFlag x={0} y={0} width={flagWidth} height={flagHeight} />
          </motion.g>
          
          {/* PT Flag */}
          <motion.g
            transform={`translate(${flagX2}, ${flagY})`}
            initial={{ scale: 0.8, opacity: 0 }}
            animate={{
              scale: [0.8, 1.05, 1],
              opacity: [0, 1, 1],
              rotate: [0, -2, 1, -0.5, 0.5, 0],
            }}
            transition={{
              scale: {
                duration: 0.5 / speed,
                delay: 3 / speed,
              },
              opacity: {
                duration: 0.3 / speed,
                delay: 3 / speed,
              },
              rotate: {
                duration: 4 / speed,
                delay: 3.5 / speed,
                repeat: Infinity,
                ease: "easeInOut",
              },
          }}>
            <PortugueseFlag x={0} y={0} width={flagWidth} height={flagHeight} />
          </motion.g>
        </motion.g>
      </svg>
    </div>
  );
};

export default DualFlagRibbonWeave;
