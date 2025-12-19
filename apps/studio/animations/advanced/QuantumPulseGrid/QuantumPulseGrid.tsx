"use client";

import React, { useState, useEffect } from "react";
import { motion } from "framer-motion";

/**
 * QuantumPulseGrid - Ultra-advanced multi-layer grid animation
 * 
 * Features:
 * - Pulsating multi-layer grid with quantum-style energy waves
 * - Multiple depth layers creating 3D effect
 * - Synchronized pulse waves propagating across grid
 * - Dynamic color shifts with neon glow effects
 */
interface QuantumPulseGridProps {
  className?: string;
  size?: number;
}

export default function QuantumPulseGrid({
  className = "",
  size = 500,
}: QuantumPulseGridProps) {
  const [time, setTime] = useState(0);
  const gridSize = 15; // Number of grid cells
  const cellSize = size / gridSize;
  const layers = 3; // Number of depth layers

  // Animation loop
  useEffect(() => {
    const interval = setInterval(() => {
      setTime((prev) => prev + 0.02);
    }, 16);
    return () => clearInterval(interval);
  }, []);

  // Calculate pulse wave for a grid cell
  const getPulseValue = (x: number, y: number, layer: number, time: number) => {
    // Create wave propagation from center
    const centerX = gridSize / 2;
    const centerY = gridSize / 2;
    const dx = x - centerX;
    const dy = y - centerY;
    const distance = Math.sqrt(dx * dx + dy * dy);
    
    // Wave frequency varies by layer
    const frequency = 0.3 + layer * 0.2;
    const wave = Math.sin(distance * 0.5 - time * frequency * 2);
    
    // Add quantum-style interference pattern
    const interference = Math.sin((x + y) * 0.8 + time * 1.5) * 0.3;
    
    return (wave + interference) * 0.5 + 0.5; // Normalize to 0-1
  };

  return (
    <div
      className={`flex items-center justify-center relative ${className}`}
      style={{ width: size, height: size }}
    >
      {/* Background glow */}
      <motion.div
        className="absolute inset-0 rounded-lg"
        style={{
          background: `radial-gradient(circle, rgba(139, 92, 246, 0.15), transparent 70%)`,
        }}
        animate={{
          opacity: [0.5, 0.8, 0.5],
          scale: [1, 1.05, 1],
        }}
        transition={{
          duration: 3,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />

      {/* Multiple depth layers */}
      {Array.from({ length: layers }).map((_, layerIndex) => {
        const layerOffset = (layerIndex - layers / 2) * 15;
        const layerOpacity = 0.3 + (1 - layerIndex / layers) * 0.7;
        const layerScale = 1 - layerIndex * 0.1;

        return (
          <div
            key={layerIndex}
            className="absolute"
            style={{
              width: size,
              height: size,
              transform: `translateZ(${layerOffset}px) scale(${layerScale})`,
              transformStyle: "preserve-3d",
              opacity: layerOpacity,
            }}
          >
            {/* Grid cells */}
            {Array.from({ length: gridSize }).map((_, row) =>
              Array.from({ length: gridSize }).map((_, col) => {
                const pulseValue = getPulseValue(col, row, layerIndex, time);
                const isEven = (row + col) % 2 === 0;
                
                // Color shifts based on pulse and position
                const hue = (time * 30 + (row + col) * 10 + layerIndex * 40) % 360;
                const brightness = 40 + pulseValue * 40;
                
                return (
                  <motion.div
                    key={`${layerIndex}-${row}-${col}`}
                    className="absolute border"
                    style={{
                      left: col * cellSize,
                      top: row * cellSize,
                      width: cellSize,
                      height: cellSize,
                      borderColor: `hsl(${hue}, 70%, ${brightness}%)`,
                      borderWidth: 1 + pulseValue * 2,
                      backgroundColor: isEven
                        ? `hsla(${hue}, 60%, ${brightness * 0.3}%, ${pulseValue * 0.3})`
                        : "transparent",
                      boxShadow: `0 0 ${pulseValue * 15}px hsla(${hue}, 80%, 60%, ${pulseValue * 0.6})`,
                    }}
                    animate={{
                      scale: [1, 1 + pulseValue * 0.1, 1],
                    }}
                    transition={{
                      duration: 0.5,
                      repeat: Infinity,
                      ease: "easeInOut",
                    }}
                  />
                );
              })
            )}
          </div>
        );
      })}

      {/* Energy wave overlay */}
      <motion.div
        className="absolute inset-0 rounded-lg pointer-events-none"
        style={{
          background: `conic-gradient(from ${time * 50}deg, transparent, rgba(139, 92, 246, 0.1), transparent)`,
        }}
        animate={{
          rotate: [0, 360],
        }}
        transition={{
          duration: 20,
          repeat: Infinity,
          ease: "linear",
        }}
      />
    </div>
  );
}

