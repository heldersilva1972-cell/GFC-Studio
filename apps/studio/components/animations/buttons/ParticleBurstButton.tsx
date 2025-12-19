"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

interface Particle {
  id: number;
  x: number;
  y: number;
  vx: number;
  vy: number;
}

export function ParticleBurstButton() {
  const [particles, setParticles] = useState<Particle[]>([]);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    const rect = e.currentTarget.getBoundingClientRect();
    const centerX = rect.width / 2;
    const centerY = rect.height / 2;
    const newParticles: Particle[] = Array.from({ length: 40 }, (_, i) => {
      const angle = (Math.PI * 2 * i) / 40;
      const speed = 2 + Math.random() * 3;
      return {
        id: Date.now() + i,
        x: centerX,
        y: centerY,
        vx: Math.cos(angle) * speed,
        vy: Math.sin(angle) * speed,
      };
    });
    setParticles((prev) => [...prev, ...newParticles]);
    setTimeout(() => {
      setParticles((prev) => prev.filter((p) => !newParticles.includes(p)));
    }, 1000);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-orange-500 bg-orange-600 px-8 py-4 text-base font-semibold text-white"
        onClick={handleClick}
        whileTap={{ scale: 0.95 }}
      >
        <span className="relative z-10">Particle Burst</span>
        <AnimatePresence>
          {particles.map((particle) => (
            <motion.div
              key={particle.id}
              className="absolute h-2 w-2 rounded-full bg-orange-300"
              style={{
                left: particle.x,
                top: particle.y,
                x: "-50%",
                y: "-50%",
              }}
              initial={{ opacity: 1, scale: 1 }}
              animate={{
                x: particle.vx * 50,
                y: particle.vy * 50,
                opacity: 0,
                scale: 0,
              }}
              exit={{ opacity: 0 }}
              transition={{ duration: 1, ease: "easeOut" }}
            />
          ))}
        </AnimatePresence>
      </motion.button>
    </div>
  );
}

export default ParticleBurstButton;

