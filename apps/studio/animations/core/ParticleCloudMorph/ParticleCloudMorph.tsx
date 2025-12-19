"use client";

import React, { useEffect, useRef, useState } from "react";
import { motion } from "framer-motion";

/**
 * ParticleCloudMorph - Advanced particle-based morphing animation
 * 
 * Title: Particle Cloud Morph – Swarm → Heart → U
 * 
 * Description:
 * Hundreds of particles that swarm together to form the shapes "I", a heart, and "U" in sequence.
 * Particles flow organically between formations, creating a dynamic, living animation effect.
 * Each particle follows a smooth path to its target position in the current shape.
 * 
 * Technical Breakdown:
 * - Canvas-based particle system for performance
 * - 200-300 particles per shape
 * - Each particle has target positions for I, Heart, and U shapes
 * - Smooth interpolation between target positions
 * - Particle trails for organic flow effect
 * - Color gradients based on particle position
 * 
 * SVG Path Plan:
 * - Define target positions for particles in each shape
 * - I: Vertical line formation
 * - Heart: Curved heart shape formation
 * - U: U-shaped formation
 * 
 * Framer Motion Plan:
 * - Use canvas for particle rendering
 * - Animate particle positions using requestAnimationFrame
 * - Smooth transitions between shape formations
 * - 6-second cycle for complete morph sequence
 */
interface ParticleCloudMorphProps {
  className?: string;
  size?: number;
}

interface Particle {
  x: number;
  y: number;
  targetX: number;
  targetY: number;
  vx: number;
  vy: number;
  hue: number;
}

export default function ParticleCloudMorph({
  className = "",
  size = 400,
}: ParticleCloudMorphProps) {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const animationFrameRef = useRef<number>();
  const particlesRef = useRef<Particle[]>([]);
  const [shapeIndex, setShapeIndex] = useState(0);
  const timeRef = useRef(0);

  // Target positions for each shape
  const shapeTargets = [
    // I shape - vertical line
    Array.from({ length: 200 }, (_, i) => {
      const progress = i / 200;
      return { x: 50, y: 20 + progress * 60 };
    }),
    // Heart shape
    Array.from({ length: 200 }, (_, i) => {
      const angle = (i / 200) * Math.PI * 2;
      const radius = 15 + Math.sin(angle * 2) * 5;
      const x = 50 + Math.cos(angle) * radius;
      const y = 50 + Math.sin(angle) * radius * 0.8;
      return { x, y };
    }),
    // U shape
    Array.from({ length: 200 }, (_, i) => {
      const progress = i / 200;
      if (progress < 0.33) {
        return { x: 30, y: 20 + (progress / 0.33) * 50 };
      } else if (progress < 0.66) {
        const uProgress = (progress - 0.33) / 0.33;
        const angle = Math.PI * uProgress;
        return { x: 30 + Math.sin(angle) * 20, y: 70 + Math.cos(angle) * 10 };
      } else {
        const sideProgress = (progress - 0.66) / 0.34;
        return { x: 70, y: 70 - sideProgress * 50 };
      }
    }),
  ];

  useEffect(() => {
    const interval = setInterval(() => {
      setShapeIndex((prev) => (prev + 1) % shapeTargets.length);
    }, 6000);
    return () => clearInterval(interval);
  }, []);

  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    canvas.width = size;
    canvas.height = size;

    // Initialize particles
    if (particlesRef.current.length === 0) {
      particlesRef.current = shapeTargets[0].map((target, i) => ({
        x: Math.random() * 100,
        y: Math.random() * 100,
        targetX: target.x,
        targetY: target.y,
        vx: 0,
        vy: 0,
        hue: (i * 2) % 360,
      }));
    }

    const animate = () => {
      timeRef.current += 0.016;
      const targets = shapeTargets[shapeIndex];

      ctx.fillStyle = "rgba(15, 23, 42, 0.1)";
      ctx.fillRect(0, 0, size, size);

      particlesRef.current.forEach((particle, i) => {
        const target = targets[i % targets.length];
        particle.targetX = target.x;
        particle.targetY = target.y;

        const dx = particle.targetX - particle.x;
        const dy = particle.targetY - particle.y;
        const distance = Math.sqrt(dx * dx + dy * dy);

        if (distance > 0.5) {
          particle.vx += dx * 0.05;
          particle.vy += dy * 0.05;
          particle.vx *= 0.9;
          particle.vy *= 0.9;
          particle.x += particle.vx;
          particle.y += particle.vy;
        }

        const alpha = Math.min(1, distance / 10);
        ctx.fillStyle = `hsla(${particle.hue}, 70%, 60%, ${alpha})`;
        ctx.beginPath();
        ctx.arc(
          (particle.x / 100) * size,
          (particle.y / 100) * size,
          2,
          0,
          Math.PI * 2
        );
        ctx.fill();
      });

      animationFrameRef.current = requestAnimationFrame(animate);
    };

    animate();

    return () => {
      if (animationFrameRef.current) {
        cancelAnimationFrame(animationFrameRef.current);
      }
    };
  }, [size, shapeIndex]);

  return (
    <div
      className={`flex items-center justify-center ${className}`}
      style={{ width: size, height: size }}
    >
      <canvas
        ref={canvasRef}
        className="rounded-lg"
        style={{
          width: size,
          height: size,
          background: "radial-gradient(circle, rgba(15, 23, 42, 0.9), rgba(15, 23, 42, 1))",
        }}
      />
    </div>
  );
}

