export const liquidNeonFluxCode = `"use client";

import React, { useState, useEffect, useRef } from "react";
import { motion } from "framer-motion";

/**
 * LiquidNeonFlux - Ultra-advanced liquid neon simulation
 * 
 * Features:
 * - Flowing neon liquid simulation with morphing energy shapes
 * - Smooth blob morphing using SVG path animation
 * - Dynamic gradient colors shifting through spectrum
 * - Glowing neon effects with multiple layers
 */
interface LiquidNeonFluxProps {
  className?: string;
  size?: number;
}

export default function LiquidNeonFlux({
  className = "",
  size = 500,
}: LiquidNeonFluxProps) {
  const [time, setTime] = useState(0);
  const svgRef = useRef<SVGSVGElement>(null);

  // Animation loop
  useEffect(() => {
    const interval = setInterval(() => {
      setTime((prev) => prev + 0.015);
    }, 16);
    return () => clearInterval(interval);
  }, []);

  // Generate morphing blob path using noise-like functions
  const generateBlobPath = (points: number, time: number, offset: number = 0) => {
    const centerX = size / 2;
    const centerY = size / 2;
    const baseRadius = size * 0.35;
    
    const pathPoints = Array.from({ length: points }).map((_, i) => {
      const angle = (i / points) * Math.PI * 2;
      const radiusVariation = 
        Math.sin(angle * 3 + time * 0.8 + offset) * 0.3 +
        Math.cos(angle * 5 + time * 1.2 + offset) * 0.2 +
        Math.sin(angle * 7 + time * 0.5 + offset) * 0.1;
      
      const radius = baseRadius * (1 + radiusVariation);
      const x = centerX + Math.cos(angle) * radius;
      const y = centerY + Math.sin(angle) * radius;
      
      return { x, y };
    });

    // Create smooth SVG path
    let path = \`M \${pathPoints[0].x} \${pathPoints[0].y}\`;
    for (let i = 1; i < pathPoints.length; i++) {
      const prev = pathPoints[i - 1];
      const curr = pathPoints[i];
      const next = pathPoints[(i + 1) % pathPoints.length];
      
      const cp1x = prev.x + (curr.x - prev.x) * 0.5;
      const cp1y = prev.y + (curr.y - prev.y) * 0.5;
      const cp2x = curr.x + (next.x - curr.x) * 0.5;
      const cp2y = curr.y + (next.y - curr.y) * 0.5;
      
      path += \` C \${cp1x} \${cp1y}, \${cp2x} \${cp2y}, \${curr.x} \${curr.y}\`;
    }
    path += " Z";
    
    return path;
  };

  // Dynamic color calculation
  const getHue = (time: number, offset: number = 0) => {
    return (time * 20 + offset * 60) % 360;
  };

  const blob1Path = generateBlobPath(16, time, 0);
  const blob2Path = generateBlobPath(12, time, Math.PI);
  const blob3Path = generateBlobPath(14, time, Math.PI * 0.5);

  return (
    <div
      className={\`flex items-center justify-center relative \${className}\`}
      style={{ width: size, height: size }}
    >
      {/* Background gradient */}
      <motion.div
        className="absolute inset-0 rounded-lg"
        style={{
          background: \`radial-gradient(circle, rgba(15, 23, 42, 0.9), rgba(15, 23, 42, 1))\`,
        }}
      />

      {/* SVG container */}
      <svg
        ref={svgRef}
        width={size}
        height={size}
        viewBox={\`0 0 \${size} \${size}\`}
        className="absolute"
      >
        <defs>
          {/* Gradient definitions */}
          <linearGradient id="liquidGradient1" x1="0%" y1="0%" x2="100%" y2="100%">
            <motion.stop
              offset="0%"
              stopColor={\`hsl(\${getHue(time, 0)}, 80%, 60%)\`}
              animate={{
                stopColor: [
                  \`hsl(\${getHue(time, 0)}, 80%, 60%)\`,
                  \`hsl(\${getHue(time, 0) + 60}, 90%, 70%)\`,
                  \`hsl(\${getHue(time, 0)}, 80%, 60%)\`,
                ],
              }}
              transition={{
                duration: 4,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
            <motion.stop
              offset="100%"
              stopColor={\`hsl(\${getHue(time, 0.3)}, 70%, 50%)\`}
              animate={{
                stopColor: [
                  \`hsl(\${getHue(time, 0.3)}, 70%, 50%)\`,
                  \`hsl(\${getHue(time, 0.3) + 60}, 80%, 60%)\`,
                  \`hsl(\${getHue(time, 0.3)}, 70%, 50%)\`,
                ],
              }}
              transition={{
                duration: 4,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
          </linearGradient>

          <linearGradient id="liquidGradient2" x1="0%" y1="0%" x2="100%" y2="100%">
            <motion.stop
              offset="0%"
              stopColor={\`hsl(\${getHue(time, 0.5)}, 75%, 55%)\`}
              animate={{
                stopColor: [
                  \`hsl(\${getHue(time, 0.5)}, 75%, 55%)\`,
                  \`hsl(\${getHue(time, 0.5) + 60}, 85%, 65%)\`,
                  \`hsl(\${getHue(time, 0.5)}, 75%, 55%)\`,
                ],
              }}
              transition={{
                duration: 5,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
            <motion.stop
              offset="100%"
              stopColor={\`hsl(\${getHue(time, 0.8)}, 65%, 45%)\`}
              animate={{
                stopColor: [
                  \`hsl(\${getHue(time, 0.8)}, 65%, 45%)\`,
                  \`hsl(\${getHue(time, 0.8) + 60}, 75%, 55%)\`,
                  \`hsl(\${getHue(time, 0.8)}, 65%, 45%)\`,
                ],
              }}
              transition={{
                duration: 5,
                repeat: Infinity,
                ease: "easeInOut",
              }}
            />
          </linearGradient>

          {/* Glow filter */}
          <filter id="neonGlow">
            <feGaussianBlur stdDeviation="8" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>

        {/* Multiple blob layers */}
        <motion.path
          d={blob1Path}
          fill="url(#liquidGradient1)"
          filter="url(#neonGlow)"
          opacity={0.8}
          animate={{
            opacity: [0.7, 0.9, 0.7],
          }}
          transition={{
            duration: 3,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        />
        <motion.path
          d={blob2Path}
          fill="url(#liquidGradient2)"
          filter="url(#neonGlow)"
          opacity={0.6}
          animate={{
            opacity: [0.5, 0.7, 0.5],
          }}
          transition={{
            duration: 4,
            repeat: Infinity,
            ease: "easeInOut",
            delay: 0.5,
          }}
        />
        <motion.path
          d={blob3Path}
          fill={\`hsl(\${getHue(time, 1)}, 70%, 50%)\`}
          filter="url(#neonGlow)"
          opacity={0.4}
          animate={{
            opacity: [0.3, 0.5, 0.3],
            fill: [
              \`hsl(\${getHue(time, 1)}, 70%, 50%)\`,
              \`hsl(\${getHue(time, 1) + 60}, 80%, 60%)\`,
              \`hsl(\${getHue(time, 1)}, 70%, 50%)\`,
            ],
          }}
          transition={{
            duration: 5,
            repeat: Infinity,
            ease: "easeInOut",
            delay: 1,
          }}
        />
      </svg>

      {/* Animated particles overlay */}
      {Array.from({ length: 20 }).map((_, i) => {
        const angle = (i / 20) * Math.PI * 2 + time;
        const radius = size * 0.25 + Math.sin(time * 2 + i) * size * 0.1;
        const x = size / 2 + Math.cos(angle) * radius;
        const y = size / 2 + Math.sin(angle) * radius;

        return (
          <motion.div
            key={i}
            className="absolute rounded-full"
            style={{
              width: 4,
              height: 4,
              left: x - 2,
              top: y - 2,
              background: \`hsl(\${getHue(time, i * 0.1)}, 80%, 60%)\`,
              boxShadow: \`0 0 10px hsl(\${getHue(time, i * 0.1)}, 80%, 60%)\`,
            }}
            animate={{
              scale: [1, 1.5, 1],
              opacity: [0.5, 1, 0.5],
            }}
            transition={{
              duration: 2 + i * 0.1,
              repeat: Infinity,
              ease: "easeInOut",
              delay: i * 0.1,
            }}
          />
        );
      })}
    </div>
  );
}`;

