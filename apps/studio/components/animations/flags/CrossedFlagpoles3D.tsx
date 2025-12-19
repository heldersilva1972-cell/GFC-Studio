"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * 3D Crossing Flagpoles
 * Two poles rotate in from offscreen and cross in X formation, flags unfurl from top
 */
export const CrossedFlagpoles3D: React.FC<{
  size?: number;
  speed?: number;
}> = ({ size = 600, speed = 1 }) => {
  const [phase, setPhase] = useState<"entering" | "crossing" | "unfurling" | "waving">("entering");

  useEffect(() => {
    const timer1 = setTimeout(() => setPhase("crossing"), 1000 / speed);
    const timer2 = setTimeout(() => setPhase("unfurling"), 2000 / speed);
    const timer3 = setTimeout(() => setPhase("waving"), 3500 / speed);
    return () => {
      clearTimeout(timer1);
      clearTimeout(timer2);
      clearTimeout(timer3);
    };
  }, [speed]);

  const poleLength = size * 0.7;
  const flagHeight = size * 0.3;

  return (
    <div className="relative flex items-center justify-center w-full h-full overflow-hidden bg-gradient-to-br from-slate-950 to-slate-900" style={{ perspective: "1000px" }}>
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        <defs>
          <pattern id="usFlagPole" x="0" y="0" width="20" height="20" patternUnits="userSpaceOnUse">
            <rect width="20" height="11.5" fill="#B22234" />
            <rect y="11.5" width="20" height="8.5" fill="#FFFFFF" />
            <rect width="8" height="11.5" fill="#3C3B6E" />
          </pattern>
          <pattern id="ptFlagPole" x="0" y="0" width="40" height="60" patternUnits="userSpaceOnUse">
            <rect width="20" height="60" fill="#006600" />
            <rect x="20" width="20" height="60" fill="#FF0000" />
          </pattern>
        </defs>

        {/* Left pole (US) */}
        <motion.g
          initial={{ x: -size * 0.3, y: size * 0.2, rotate: -45, opacity: 0 }}
          animate={{
            x: phase === "entering" ? size * 0.2 : size * 0.35,
            y: phase === "entering" ? size * 0.2 : size * 0.15,
            rotate: phase === "entering" ? -45 : phase === "crossing" ? -20 : -20,
            opacity: 1,
          }}
          transition={{ duration: 1 / speed, ease: "easeOut" }}
        >
          <line
            x1={0}
            y1={0}
            x2={0}
            y2={poleLength}
            stroke="#8B4513"
            strokeWidth="8"
          />
          <motion.rect
            x={-size * 0.15}
            y={0}
            width={size * 0.3}
            height={flagHeight}
            fill="url(#usFlagPole)"
            initial={{ height: 0, opacity: 0 }}
            animate={{
              height: phase === "unfurling" || phase === "waving" ? flagHeight : 0,
              opacity: phase === "unfurling" || phase === "waving" ? 1 : 0,
              x: phase === "waving" ? [-size * 0.15, -size * 0.12, -size * 0.15] : -size * 0.15,
            }}
            transition={{
              height: { duration: 0.8 / speed, delay: 1.5 / speed },
              opacity: { duration: 0.3 / speed, delay: 1.5 / speed },
              x: {
                duration: 2 / speed,
                repeat: phase === "waving" ? Infinity : 0,
                ease: "easeInOut",
              },
            }}
          />
        </motion.g>

        {/* Right pole (PT) */}
        <motion.g
          initial={{ x: size * 1.3, y: size * 0.2, rotate: 45, opacity: 0 }}
          animate={{
            x: phase === "entering" ? size * 0.8 : size * 0.65,
            y: phase === "entering" ? size * 0.2 : size * 0.15,
            rotate: phase === "entering" ? 45 : phase === "crossing" ? 20 : 20,
            opacity: 1,
          }}
          transition={{ duration: 1 / speed, ease: "easeOut" }}
        >
          <line
            x1={0}
            y1={0}
            x2={0}
            y2={poleLength}
            stroke="#8B4513"
            strokeWidth="8"
          />
          <motion.rect
            x={-size * 0.15}
            y={0}
            width={size * 0.3}
            height={flagHeight}
            fill="url(#ptFlagPole)"
            initial={{ height: 0, opacity: 0 }}
            animate={{
              height: phase === "unfurling" || phase === "waving" ? flagHeight : 0,
              opacity: phase === "unfurling" || phase === "waving" ? 1 : 0,
              x: phase === "waving" ? [-size * 0.15, -size * 0.18, -size * 0.15] : -size * 0.15,
            }}
            transition={{
              height: { duration: 0.8 / speed, delay: 1.5 / speed },
              opacity: { duration: 0.3 / speed, delay: 1.5 / speed },
              x: {
                duration: 2 / speed,
                repeat: phase === "waving" ? Infinity : 0,
                ease: "easeInOut",
              },
            }}
          />
        </motion.g>
      </svg>
    </div>
  );
};

export default CrossedFlagpoles3D;

