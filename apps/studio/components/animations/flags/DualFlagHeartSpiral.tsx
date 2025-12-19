"use client";

import React from "react";
import { motion } from "framer-motion";

/**
 * Heart-Shape Dual Flag
 * Ribbons spiral inward to form a heart outline, then fill with blended flag pattern
 */
export const DualFlagHeartSpiral: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const heartPath = `M ${size / 2},${size * 0.6} 
    C ${size / 2},${size * 0.5} ${size * 0.35},${size * 0.35} ${size * 0.35},${size * 0.45}
    C ${size * 0.35},${size * 0.55} ${size / 2},${size * 0.6} ${size / 2},${size * 0.6}
    C ${size / 2},${size * 0.6} ${size * 0.65},${size * 0.55} ${size * 0.65},${size * 0.45}
    C ${size * 0.65},${size * 0.35} ${size / 2},${size * 0.5} ${size / 2},${size * 0.6} Z`;

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900">
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        <defs>
          <pattern id="usHeart" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect width="20" height="11.5" fill="#B22234" />
            <rect y="11.5" width="20" height="8.5" fill="#FFFFFF" />
            <rect width="8" height="11.5" fill="#3C3B6E" />
          </pattern>
          <pattern id="ptHeart" x="0" y="0" width="40" height="60" patternUnits="userSpaceOnUse">
            <rect width="20" height="60" fill="#006600" />
            <rect x="20" width="20" height="60" fill="#FF0000" />
          </pattern>
          <clipPath id="heartClip">
            <path d={heartPath} />
          </clipPath>
        </defs>

        {/* Heart outline with spiral ribbons */}
        <motion.path
          d={heartPath}
          fill="none"
          stroke="#FFD700"
          strokeWidth="3"
          initial={{ pathLength: 0, opacity: 0 }}
          animate={{
            pathLength: [0, 1, 1],
            opacity: [0, 1, 1],
          }}
          transition={{
            pathLength: { duration: 2 / speed },
            opacity: { duration: 0.5 / speed },
          }}
        />

        {/* Filled heart - left half US, right half PT */}
        <motion.g clipPath="url(#heartClip)">
          <motion.rect
            x={size * 0.15}
            y={size * 0.3}
            width={size * 0.35}
            height={size * 0.3}
            fill="url(#usHeart)"
            initial={{ opacity: 0 }}
            animate={{ opacity: [0, 0, 1] }}
            transition={{ duration: 3 / speed, times: [0, 0.6, 1] }}
          />
          <motion.rect
            x={size * 0.5}
            y={size * 0.3}
            width={size * 0.35}
            height={size * 0.3}
            fill="url(#ptHeart)"
            initial={{ opacity: 0 }}
            animate={{ opacity: [0, 0, 1] }}
            transition={{ duration: 3 / speed, times: [0, 0.6, 1] }}
          />
        </motion.g>

        {/* Pulsing effect */}
        <motion.g
          animate={{
            scale: [1, 1.05, 1],
          }}
          transition={{
            duration: 2 / speed,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        >
          <path
            d={heartPath}
            fill="none"
            stroke="rgba(255, 215, 0, 0.3)"
            strokeWidth="5"
          />
        </motion.g>
      </svg>
    </div>
  );
};

export default DualFlagHeartSpiral;

