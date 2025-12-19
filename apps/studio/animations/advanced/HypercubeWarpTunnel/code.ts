export const hypercubeWarpTunnelCode = `"use client";

import React, { useState, useEffect, useRef } from "react";
import { motion } from "framer-motion";

/**
 * HypercubeWarpTunnel - Ultra-advanced 4D hypercube animation
 * 
 * Features:
 * - Rotating 4D hypercube warping through space
 * - Multiple cube layers creating tunnel effect
 * - Perspective projection for 3D visualization
 * - Dynamic rotation and scaling for depth perception
 */
interface HypercubeWarpTunnelProps {
  className?: string;
  size?: number;
}

interface Point3D {
  x: number;
  y: number;
  z: number;
}

export default function HypercubeWarpTunnel({
  className = "",
  size = 500,
}: HypercubeWarpTunnelProps) {
  const [time, setTime] = useState(0);
  const canvasRef = useRef<HTMLCanvasElement>(null);

  // Animation loop
  useEffect(() => {
    const interval = setInterval(() => {
      setTime((prev) => prev + 0.02);
    }, 16);
    return () => clearInterval(interval);
  }, []);

  // Draw hypercube layers
  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    canvas.width = size;
    canvas.height = size;

    const centerX = size / 2;
    const centerY = size / 2;
    const cubeSize = size * 0.15;
    const layers = 8; // Number of cube layers
    const perspective = 400; // Perspective distance

    // Clear canvas
    ctx.fillStyle = "rgba(15, 23, 42, 0.1)";
    ctx.fillRect(0, 0, size, size);

    // Draw multiple cube layers
    for (let layer = 0; layer < layers; layer++) {
      const layerProgress = (layer / layers + time * 0.3) % 1;
      const z = -perspective + layerProgress * perspective * 2;
      const scale = perspective / (perspective + z);
      const layerSize = cubeSize * scale;
      const layerOpacity = 1 - layerProgress * 0.8;

      // Rotation angles
      const rotX = time * 0.5 + layer * 0.2;
      const rotY = time * 0.7 + layer * 0.15;
      const rotZ = time * 0.3 + layer * 0.1;

      // Define cube vertices (8 vertices of a cube)
      const vertices: Point3D[] = [
        { x: -1, y: -1, z: -1 },
        { x: 1, y: -1, z: -1 },
        { x: 1, y: 1, z: -1 },
        { x: -1, y: 1, z: -1 },
        { x: -1, y: -1, z: 1 },
        { x: 1, y: -1, z: 1 },
        { x: 1, y: 1, z: 1 },
        { x: -1, y: 1, z: 1 },
      ];

      // Rotate vertices
      const rotateX = (p: Point3D, angle: number): Point3D => {
        const cos = Math.cos(angle);
        const sin = Math.sin(angle);
        return {
          x: p.x,
          y: p.y * cos - p.z * sin,
          z: p.y * sin + p.z * cos,
        };
      };

      const rotateY = (p: Point3D, angle: number): Point3D => {
        const cos = Math.cos(angle);
        const sin = Math.sin(angle);
        return {
          x: p.x * cos + p.z * sin,
          y: p.y,
          z: -p.x * sin + p.z * cos,
        };
      };

      const rotateZ = (p: Point3D, angle: number): Point3D => {
        const cos = Math.cos(angle);
        const sin = Math.sin(angle);
        return {
          x: p.x * cos - p.y * sin,
          y: p.x * sin + p.y * cos,
          z: p.z,
        };
      };

      // Project 3D to 2D
      const project = (p: Point3D): { x: number; y: number } => {
        const rotated = rotateZ(rotateY(rotateX(p, rotX), rotY), rotZ);
        const projectedZ = rotated.z + z;
        const projectedX = (rotated.x * layerSize * scale) / (1 + projectedZ / perspective);
        const projectedY = (rotated.y * layerSize * scale) / (1 + projectedZ / perspective);
        return {
          x: centerX + projectedX,
          y: centerY + projectedY,
        };
      };

      // Project all vertices
      const projected = vertices.map(project);

      // Define cube edges (12 edges of a cube)
      const edges = [
        [0, 1], [1, 2], [2, 3], [3, 0], // Front face
        [4, 5], [5, 6], [6, 7], [7, 4], // Back face
        [0, 4], [1, 5], [2, 6], [3, 7], // Connecting edges
      ];

      // Draw edges
      ctx.strokeStyle = \`rgba(139, 92, 246, \${layerOpacity * 0.8})\`;
      ctx.lineWidth = 2 * scale;
      ctx.shadowBlur = 10 * scale;
      ctx.shadowColor = \`rgba(139, 92, 246, \${layerOpacity * 0.6})\`;

      edges.forEach(([start, end]) => {
        const p1 = projected[start];
        const p2 = projected[end];

        ctx.beginPath();
        ctx.moveTo(p1.x, p1.y);
        ctx.lineTo(p2.x, p2.y);
        ctx.stroke();
      });

      // Draw vertices
      projected.forEach((p) => {
        ctx.fillStyle = \`rgba(139, 92, 246, \${layerOpacity})\`;
        ctx.beginPath();
        ctx.arc(p.x, p.y, 3 * scale, 0, Math.PI * 2);
        ctx.fill();
      });
    }
  }, [time, size]);

  return (
    <div
      className={\`flex items-center justify-center relative \${className}\`}
      style={{ width: size, height: size }}
    >
      {/* Background gradient */}
      <motion.div
        className="absolute inset-0 rounded-lg"
        style={{
          background: \`radial-gradient(circle, rgba(15, 23, 42, 0.95), rgba(15, 23, 42, 1))\`,
        }}
      />

      {/* Canvas for hypercube */}
      <canvas
        ref={canvasRef}
        className="absolute rounded-lg"
        style={{
          width: size,
          height: size,
        }}
      />

      {/* Animated glow overlay */}
      <motion.div
        className="absolute inset-0 rounded-lg pointer-events-none"
        style={{
          background: \`radial-gradient(circle at \${size / 2}px \${size / 2}px, rgba(139, 92, 246, 0.2), transparent 70%)\`,
        }}
        animate={{
          opacity: [0.3, 0.6, 0.3],
          scale: [1, 1.1, 1],
        }}
        transition={{
          duration: 3,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />

      {/* Rotating energy rings */}
      {Array.from({ length: 3 }).map((_, i) => (
        <motion.div
          key={i}
          className="absolute rounded-full border"
          style={{
            width: size * (0.3 + i * 0.2),
            height: size * (0.3 + i * 0.2),
            left: size / 2 - (size * (0.3 + i * 0.2)) / 2,
            top: size / 2 - (size * (0.3 + i * 0.2)) / 2,
            borderColor: \`rgba(139, 92, 246, \${0.2 - i * 0.05})\`,
            borderWidth: 1,
            boxShadow: \`0 0 \${20 + i * 10}px rgba(139, 92, 246, \${0.3 - i * 0.1})\`,
          }}
          animate={{
            rotate: [0, 360],
            scale: [1, 1.1, 1],
            opacity: [0.3, 0.6, 0.3],
          }}
          transition={{
            duration: 4 + i * 2,
            repeat: Infinity,
            ease: "linear",
            delay: i * 0.5,
          }}
        />
      ))}
    </div>
  );
}`;

