export const hologramGridWarpCode = `"use client";

import React, { useState, useEffect, useRef } from "react";
import { motion } from "framer-motion";

/**
 * HologramGridWarp - Advanced grid animation with mouse interaction
 * 
 * Features:
 * - Glowing neon grid that reacts to mouse movement
 * - Perspective skew and z-depth motion for 3D effect
 * - Motion blur lines that follow the cursor
 * - Smooth interpolation for responsive feel
 */
interface HologramGridWarpProps {
  className?: string;
  size?: number;
}

export default function HologramGridWarp({
  className = "",
  size = 400,
}: HologramGridWarpProps) {
  const containerRef = useRef<HTMLDivElement>(null);
  const [mousePos, setMousePos] = useState({ x: 0.5, y: 0.5 });
  const [isHovering, setIsHovering] = useState(false);

  // Grid configuration
  const gridSize = 20; // Number of grid cells
  const cellSize = size / gridSize;

  // Handle mouse movement for interactive grid warping
  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      if (!containerRef.current) return;
      
      const rect = containerRef.current.getBoundingClientRect();
      const x = (e.clientX - rect.left) / rect.width;
      const y = (e.clientY - rect.top) / rect.height;
      
      setMousePos({ x: Math.max(0, Math.min(1, x)), y: Math.max(0, Math.min(1, y)) });
    };

    const container = containerRef.current;
    if (container) {
      container.addEventListener("mousemove", handleMouseMove);
      container.addEventListener("mouseenter", () => setIsHovering(true));
      container.addEventListener("mouseleave", () => setIsHovering(false));
    }

    return () => {
      if (container) {
        container.removeEventListener("mousemove", handleMouseMove);
        container.removeEventListener("mouseenter", () => setIsHovering(true));
        container.removeEventListener("mouseleave", () => setIsHovering(false));
      }
    };
  }, []);

  // Calculate grid cell transform based on distance from mouse
  const getCellTransform = (row: number, col: number) => {
    const cellX = (col + 0.5) / gridSize;
    const cellY = (row + 0.5) / gridSize;
    
    // Distance from mouse position (0 to 1)
    const dx = cellX - mousePos.x;
    const dy = cellY - mousePos.y;
    const distance = Math.sqrt(dx * dx + dy * dy);
    
    // Warp effect: cells closer to mouse get pushed back (z-depth)
    const maxDistance = Math.sqrt(2); // Diagonal distance
    const normalizedDistance = Math.min(distance / maxDistance, 1);
    
    // Z-depth calculation: closer cells have more depth
    const zDepth = isHovering ? (1 - normalizedDistance) * 30 : 0;
    
    // Perspective skew based on mouse position
    const skewX = isHovering ? (mousePos.x - cellX) * 15 : 0;
    const skewY = isHovering ? (mousePos.y - cellY) * 15 : 0;
    
    // Scale effect for depth perception
    const scale = 1 + (zDepth / 200);
    
    return {
      transform: \`perspective(500px) translateZ(\${zDepth}px) scale(\${scale}) skew(\${skewX}deg, \${skewY}deg)\`,
      opacity: 0.3 + (normalizedDistance * 0.7), // Fade based on distance
    };
  };

  return (
    <div
      ref={containerRef}
      className={\`flex items-center justify-center relative \${className}\`}
      style={{ width: size, height: size }}
    >
      {/* Animated background glow */}
      <motion.div
        className="absolute inset-0 rounded-lg"
        style={{
          background: \`radial-gradient(circle at \${mousePos.x * 100}% \${mousePos.y * 100}%, rgba(139, 92, 246, 0.3), transparent 70%)\`,
        }}
        animate={{
          opacity: isHovering ? 1 : 0.5,
        }}
        transition={{ duration: 0.3 }}
      />

      {/* Grid container with perspective */}
      <div
        className="relative"
        style={{
          width: size,
          height: size,
          transformStyle: "preserve-3d",
        }}
      >
        {/* Grid cells */}
        {Array.from({ length: gridSize }).map((_, row) =>
          Array.from({ length: gridSize }).map((_, col) => {
            const cellStyle = getCellTransform(row, col);
            const isEven = (row + col) % 2 === 0;
            
            return (
              <motion.div
                key={\`\${row}-\${col}\`}
                className="absolute border border-cyan-400/40"
                style={{
                  left: col * cellSize,
                  top: row * cellSize,
                  width: cellSize,
                  height: cellSize,
                  backgroundColor: isEven 
                    ? "rgba(139, 92, 246, 0.1)" 
                    : "rgba(34, 211, 238, 0.1)",
                  boxShadow: isHovering 
                    ? \`0 0 \${cellStyle.opacity * 10}px rgba(139, 92, 246, \${cellStyle.opacity * 0.5})\`
                    : "none",
                  ...cellStyle,
                }}
                animate={{
                  borderColor: isHovering
                    ? \`rgba(139, 92, 246, \${0.4 + cellStyle.opacity * 0.6})\`
                    : "rgba(34, 211, 238, 0.4)",
                }}
                transition={{ duration: 0.2 }}
              />
            );
          })
        )}
      </div>

      {/* Motion blur trail effect */}
      {isHovering && (
        <motion.div
          className="absolute pointer-events-none"
          style={{
            left: mousePos.x * size - 2,
            top: mousePos.y * size - 2,
            width: 4,
            height: 4,
            borderRadius: "50%",
            background: "radial-gradient(circle, rgba(139, 92, 246, 1), transparent)",
            boxShadow: "0 0 20px rgba(139, 92, 246, 0.8)",
          }}
          animate={{
            scale: [1, 1.5, 1],
            opacity: [1, 0.8, 0],
          }}
          transition={{
            duration: 0.5,
            repeat: Infinity,
            ease: "easeOut",
          }}
        />
      )}
    </div>
  );
}`;

