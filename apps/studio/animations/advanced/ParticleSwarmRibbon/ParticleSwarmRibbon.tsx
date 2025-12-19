"use client";

import React, { useEffect, useRef, useState } from "react";
import { motion } from "framer-motion";

/**
 * ParticleSwarmRibbon - Advanced particle system animation
 * 
 * Features:
 * - 500-1500 particles following a noise field
 * - Particles form a long flowing ribbon path
 * - Smooth camera drift using Framer Motion
 * - Optimized canvas rendering for performance
 */
interface ParticleSwarmRibbonProps {
  className?: string;
  size?: number;
}

interface Particle {
  x: number;
  y: number;
  vx: number;
  vy: number;
  life: number;
  size: number;
  hue: number;
}

export default function ParticleSwarmRibbon({
  className = "",
  size = 500,
}: ParticleSwarmRibbonProps) {
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const animationFrameRef = useRef<number>();
  const particlesRef = useRef<Particle[]>([]);
  const timeRef = useRef(0);

  // Configuration
  const particleCount = Math.min(1000, Math.floor(size * 2)); // Scale particle count with size
  const noiseScale = 0.01; // Noise field scale
  const particleSpeed = 0.5; // Base particle speed
  const ribbonWidth = size * 0.3; // Width of the ribbon

  // Initialize particles
  useEffect(() => {
    const particles: Particle[] = [];
    const centerX = size / 2;
    const centerY = size / 2;

    for (let i = 0; i < particleCount; i++) {
      // Distribute particles along a curved path
      const t = i / particleCount;
      const angle = t * Math.PI * 4; // Multiple loops
      const radius = size * 0.2;
      
      particles.push({
        x: centerX + Math.cos(angle) * radius,
        y: centerY + Math.sin(angle) * radius,
        vx: 0,
        vy: 0,
        life: Math.random(),
        size: 1 + Math.random() * 2,
        hue: (t * 360 + Math.random() * 60) % 360, // Color variation
      });
    }

    particlesRef.current = particles;
  }, [particleCount, size]);

  // Simple 2D noise function (simplified Perlin-like noise)
  const noise = (x: number, y: number, time: number): number => {
    // Simplified noise using sine waves
    return (
      Math.sin(x * noiseScale + time * 0.5) *
      Math.cos(y * noiseScale + time * 0.3) *
      0.5 +
      0.5
    );
  };

  // Get noise field gradient for particle direction
  const getNoiseGradient = (x: number, y: number, time: number) => {
    const delta = 1;
    const nx = noise(x + delta, y, time);
    const ny = noise(x, y + delta, time);
    const n = noise(x, y, time);
    
    return {
      angle: Math.atan2(ny - n, nx - n),
      strength: Math.sqrt((nx - n) ** 2 + (ny - n) ** 2),
    };
  };

  // Animation loop
  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    canvas.width = size;
    canvas.height = size;

    const animate = () => {
      timeRef.current += 0.016; // ~60fps
      const time = timeRef.current;

      // Clear canvas with fade effect for trails
      ctx.fillStyle = "rgba(15, 23, 42, 0.1)"; // slate-950 with low opacity
      ctx.fillRect(0, 0, size, size);

      const particles = particlesRef.current;
      const centerX = size / 2;
      const centerY = size / 2;

      // Update and draw particles
      particles.forEach((particle, index) => {
        // Get noise field influence
        const gradient = getNoiseGradient(
          particle.x,
          particle.y,
          time
        );

        // Update velocity based on noise field
        const force = gradient.strength * particleSpeed;
        particle.vx += Math.cos(gradient.angle) * force * 0.1;
        particle.vy += Math.sin(gradient.angle) * force * 0.1;

        // Apply damping
        particle.vx *= 0.95;
        particle.vy *= 0.95;

        // Update position
        particle.x += particle.vx;
        particle.y += particle.vy;

        // Wrap around edges
        if (particle.x < 0) particle.x = size;
        if (particle.x > size) particle.x = 0;
        if (particle.y < 0) particle.y = size;
        if (particle.y > size) particle.y = 0;

        // Update life cycle
        particle.life += 0.01;
        if (particle.life > 1) particle.life = 0;

        // Calculate distance from center for ribbon effect
        const dx = particle.x - centerX;
        const dy = particle.y - centerY;
        const distance = Math.sqrt(dx * dx + dy * dy);
        const normalizedDistance = distance / (size * 0.4);

        // Draw particle with gradient based on position in ribbon
        const alpha = Math.max(0, 1 - normalizedDistance * 2);
        const brightness = 0.5 + Math.sin(particle.life * Math.PI * 2) * 0.5;

        ctx.save();
        ctx.globalAlpha = alpha * brightness;
        ctx.fillStyle = `hsl(${particle.hue}, 70%, ${50 + brightness * 30}%)`;
        ctx.beginPath();
        ctx.arc(particle.x, particle.y, particle.size, 0, Math.PI * 2);
        ctx.fill();
        ctx.restore();
      });

      animationFrameRef.current = requestAnimationFrame(animate);
    };

    animate();

    return () => {
      if (animationFrameRef.current) {
        cancelAnimationFrame(animationFrameRef.current);
      }
    };
  }, [size, particleSpeed, noiseScale]);

  return (
    <motion.div
      className={`flex items-center justify-center relative ${className}`}
      style={{ width: size, height: size }}
      animate={{
        scale: [1, 1.02, 1],
      }}
      transition={{
        duration: 4,
        repeat: Infinity,
        ease: "easeInOut",
      }}
    >
      <canvas
        ref={canvasRef}
        className="rounded-lg"
        style={{
          width: size,
          height: size,
          background: "radial-gradient(circle, rgba(15, 23, 42, 0.8), rgba(15, 23, 42, 1))",
        }}
      />
    </motion.div>
  );
}

