"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

const fireflies = Array.from({ length: 5 }, (_, i) => ({
  id: i,
  x: Math.random() * 100,
  y: Math.random() * 100,
  delay: Math.random() * 2,
  duration: 3 + Math.random() * 2,
}));

export function FireflySparkleButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-yellow-500 bg-slate-900 px-8 py-4 text-base font-semibold text-yellow-400"
        whileHover={{ filter: "brightness(1.2)" }}
        whileTap={{ scale: 0.95 }}
      >
        <span className="relative z-10">Firefly</span>
        {fireflies.map((firefly) => (
          <motion.div
            key={firefly.id}
            className="absolute h-2 w-2 rounded-full bg-yellow-300 blur-sm"
            style={{
              left: `${firefly.x}%`,
              top: `${firefly.y}%`,
            }}
            animate={{
              x: [0, 20, -20, 0],
              y: [0, -20, 20, 0],
              opacity: [0.3, 1, 0.8, 0.3],
              scale: [0.8, 1.2, 1, 0.8],
            }}
            transition={{
              duration: firefly.duration,
              delay: firefly.delay,
              repeat: Infinity,
              ease: "easeInOut",
            }}
          />
        ))}
      </motion.button>
    </div>
  );
}

export default FireflySparkleButton;

