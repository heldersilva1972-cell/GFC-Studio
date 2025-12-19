"use client";

import React from "react";
import { motion } from "framer-motion";
import { USFlag, PortugueseFlag } from "./RealisticFlag";

type HolidayKey =
  | "new_year"
  | "us_independence"
  | "pt_dia_de_portugal"
  | "thanksgiving"
  | "memorial_day"
  | "christmas"
  | "easter"
  | "labor_day";

interface HolidayConfig {
  accentColors: string[];
  particles: string[];
  background: string;
  icon?: string;
}

const HOLIDAY_CONFIGS: Record<HolidayKey, HolidayConfig> = {
  new_year: {
    accentColors: ["#FFD700", "#FFA500", "#FF6B6B"],
    particles: ["✨", "🎉", "⭐"],
    background: "radial-gradient(circle, rgba(255,215,0,0.1), transparent)",
  },
  us_independence: {
    accentColors: ["#B22234", "#FFFFFF", "#3C3B6E"],
    particles: ["🎆", "⭐", "🇺🇸"],
    background: "radial-gradient(circle, rgba(178,34,52,0.15), transparent)",
  },
  pt_dia_de_portugal: {
    accentColors: ["#006600", "#FF0000", "#FFD700"],
    particles: ["🎆", "⭐", "🇵🇹"],
    background: "radial-gradient(circle, rgba(0,102,0,0.15), transparent)",
  },
  thanksgiving: {
    accentColors: ["#D2691E", "#8B4513", "#FF8C00"],
    particles: ["🍂", "🦃", "🍁"],
    background: "radial-gradient(circle, rgba(210,105,30,0.1), transparent)",
  },
  memorial_day: {
    accentColors: ["#B22234", "#3C3B6E", "#FFFFFF"],
    particles: ["⭐", "🕊️"],
    background: "radial-gradient(circle, rgba(60,59,110,0.1), transparent)",
  },
  christmas: {
    accentColors: ["#DC143C", "#228B22", "#FFD700"],
    particles: ["❄️", "🎄", "⭐"],
    background: "radial-gradient(circle, rgba(220,20,60,0.1), transparent)",
  },
  easter: {
    accentColors: ["#FFB6C1", "#E6E6FA", "#FFE4E1"],
    particles: ["🌸", "🐰", "🥚"],
    background: "radial-gradient(circle, rgba(255,182,193,0.1), transparent)",
  },
  labor_day: {
    accentColors: ["#B22234", "#FFFFFF", "#3C3B6E"],
    particles: ["⭐", "🎉"],
    background: "radial-gradient(circle, rgba(178,34,52,0.1), transparent)",
  },
};

/**
 * Dual Flag Holiday Wave
 * Both flags side-by-side with holiday-themed accents, particles, and colors
 */
export const DualFlagHolidayWave: React.FC<{
  holidayKey: HolidayKey;
  size?: number;
  speed?: number;
}> = ({ holidayKey, size = 600, speed = 1 }) => {
  const config = HOLIDAY_CONFIGS[holidayKey];
  // Realistic flag proportions: 2:3 ratio - make flags larger and more visible
  const flagWidth = size * 0.4;
  const flagHeight = (flagWidth * 3) / 2; // 2:3 ratio
  const flagX1 = size * 0.05;
  const flagX2 = size * 0.5;
  const flagY = (size - flagHeight) / 2; // Center vertically

  return (
    <div
      className="relative flex items-center justify-center w-full h-full overflow-hidden"
      style={{
        background: config.background,
      }}
    >
      <svg width={size} height={size} viewBox={`0 0 ${size} ${size}`} className="absolute">
        {/* Holiday particles */}
        {Array.from({ length: 15 }, (_, i) => (
          <motion.text
            key={i}
            x={Math.random() * size}
            y={Math.random() * size}
            fontSize={size * 0.04}
            fill={config.accentColors[i % config.accentColors.length]}
            initial={{ opacity: 0, y: -20 }}
            animate={{
              opacity: [0, 1, 0],
              y: [Math.random() * size, Math.random() * size + size],
              x: [Math.random() * size, Math.random() * size + (Math.random() - 0.5) * size * 0.3],
            }}
            transition={{
              duration: (3 + Math.random() * 2) / speed,
              repeat: Infinity,
              delay: Math.random() * 2,
              ease: "linear",
            }}
          >
            {config.particles[i % config.particles.length]}
          </motion.text>
        ))}

        {/* US Flag with wave */}
        <motion.g
          transform={`translate(${flagX1}, ${flagY})`}
          animate={{
            x: [0, size * 0.01, -size * 0.01, 0],
            rotate: [0, 1, -1, 0.5, -0.5, 0],
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
            x: [0, size * 0.01, -size * 0.01, 0],
            rotate: [0, -1, 1, -0.5, 0.5, 0],
          }}
          transition={{
            duration: 3 / speed,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        >
          <PortugueseFlag x={0} y={0} width={flagWidth} height={flagHeight} />
        </motion.g>

        {/* Accent glow */}
        <motion.circle
          cx={size / 2}
          cy={size / 2}
          r={size * 0.4}
          fill="none"
          stroke={config.accentColors[0]}
          strokeWidth="2"
          opacity={0.3}
          animate={{
            scale: [1, 1.2, 1],
            opacity: [0.3, 0.5, 0.3],
          }}
          transition={{
            duration: 2 / speed,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        />
      </svg>
    </div>
  );
};

export default DualFlagHolidayWave;
