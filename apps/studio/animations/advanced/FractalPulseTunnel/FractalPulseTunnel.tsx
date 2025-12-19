"use client";

import React, { useEffect, useRef, useState } from "react";
import { motion } from "framer-motion";

/**
 * FractalPulseTunnel - Advanced recursive ring animation
 * 
 * Features:
 * - Animated recursive rings creating tunnel effect
 * - Depth zoom traveling through tunnel
 * - Pulse sync glow with synchronized animations
 * - Hue shifting for dynamic color changes
 */
interface FractalPulseTunnelProps {
  className?: string;
  size?: number;
}

export default function FractalPulseTunnel({
  className = "",
  size = 400,
}: FractalPulseTunnelProps) {
  const svgRef = useRef<SVGSVGElement>(null);
  const [time, setTime] = useState(0);

  // Configuration
  const ringCount = 20; // Number of recursive rings
  const centerX = size / 2;
  const centerY = size / 2;
  const maxRadius = size * 0.45;

  // Animation loop for continuous updates
  useEffect(() => {
    const interval = setInterval(() => {
      setTime((prev) => prev + 0.016); // ~60fps
    }, 16);

    return () => clearInterval(interval);
  }, []);

  // Calculate ring properties based on depth and time
  const getRingProperties = (index: number, time: number) => {
    // Depth calculation: rings move from center outward
    // Using modulo to create infinite tunnel effect
    const depth = (index / ringCount + time * 0.1) % 1;
    
    // Radius increases with depth (tunnel perspective)
    // Using exponential scaling for realistic perspective
    const radius = maxRadius * (0.2 + depth * 0.8);
    
    // Scale factor for size variation
    const scale = 0.3 + depth * 0.7;
    
    // Opacity fades with depth
    const opacity = 0.2 + (1 - depth) * 0.8;
    
    // Pulse effect: synchronized pulsing
    const pulsePhase = (time * 2 + index * 0.1) % (Math.PI * 2);
    const pulseScale = 1 + Math.sin(pulsePhase) * 0.15;
    
    // Hue shifting: color changes over time and depth
    const baseHue = (time * 30 + depth * 120) % 360;
    
    return {
      radius: radius * pulseScale,
      scale,
      opacity: opacity * (0.7 + Math.sin(pulsePhase) * 0.3),
      hue: baseHue,
      strokeWidth: 2 + depth * 3,
    };
  };

  return (
    <div
      className={`flex items-center justify-center relative ${className}`}
      style={{ width: size, height: size }}
    >
      {/* Background glow */}
      <motion.div
        className="absolute inset-0 rounded-full"
        style={{
          background: "radial-gradient(circle, rgba(139, 92, 246, 0.2), transparent 70%)",
        }}
        animate={{
          scale: [1, 1.1, 1],
          opacity: [0.5, 0.8, 0.5],
        }}
        transition={{
          duration: 3,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />

      {/* SVG for rings */}
      <svg
        ref={svgRef}
        width={size}
        height={size}
        viewBox={`0 0 ${size} ${size}`}
        className="absolute"
      >
        <defs>
          {/* Radial gradient for glow effect */}
          <radialGradient id="ringGradient">
            <stop offset="0%" stopColor="currentColor" stopOpacity="1" />
            <stop offset="100%" stopColor="currentColor" stopOpacity="0.3" />
          </radialGradient>
          
          {/* Glow filter */}
          <filter id="glow">
            <feGaussianBlur stdDeviation="3" result="coloredBlur" />
            <feMerge>
              <feMergeNode in="coloredBlur" />
              <feMergeNode in="SourceGraphic" />
            </feMerge>
          </filter>
        </defs>

        {/* Render recursive rings */}
        {Array.from({ length: ringCount }).map((_, index) => {
          const props = getRingProperties(index, time);
          
          return (
            <motion.circle
              key={index}
              cx={centerX}
              cy={centerY}
              r={props.radius}
              fill="none"
              stroke={`hsl(${props.hue}, 70%, 60%)`}
              strokeWidth={props.strokeWidth}
              opacity={props.opacity}
              filter="url(#glow)"
              animate={{
                r: [
                  props.radius * 0.95,
                  props.radius * 1.05,
                  props.radius * 0.95,
                ],
                opacity: [
                  props.opacity * 0.8,
                  props.opacity,
                  props.opacity * 0.8,
                ],
                stroke: [
                  `hsl(${props.hue}, 70%, 60%)`,
                  `hsl(${(props.hue + 30) % 360}, 80%, 70%)`,
                  `hsl(${props.hue}, 70%, 60%)`,
                ],
              }}
              transition={{
                duration: 2 + index * 0.1,
                repeat: Infinity,
                ease: "easeInOut",
                delay: index * 0.05,
              }}
            />
          );
        })}
      </svg>

      {/* Center core pulse */}
      <motion.div
        className="absolute rounded-full"
        style={{
          width: size * 0.1,
          height: size * 0.1,
          left: centerX - size * 0.05,
          top: centerY - size * 0.05,
          background: "radial-gradient(circle, rgba(139, 92, 246, 1), rgba(139, 92, 246, 0))",
          boxShadow: "0 0 40px rgba(139, 92, 246, 0.8)",
        }}
        animate={{
          scale: [1, 1.5, 1],
          opacity: [0.8, 1, 0.8],
        }}
        transition={{
          duration: 2,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />
    </div>
  );
}

