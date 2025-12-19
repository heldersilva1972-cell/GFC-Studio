"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

interface Ripple {
  id: number;
}

export function InnerRippleGlowButton() {
  const [ripples, setRipples] = useState<Ripple[]>([]);

  const handleClick = () => {
    const newRipples: Ripple[] = Array.from({ length: 3 }, (_, i) => ({
      id: Date.now() + i,
    }));
    setRipples((prev) => [...prev, ...newRipples]);
    setTimeout(() => {
      setRipples((prev) => prev.filter((r) => !newRipples.includes(r)));
    }, 1200);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border-2 border-violet-400 bg-slate-900 px-8 py-4 text-base font-semibold text-violet-400"
        onClick={handleClick}
        whileTap={{ scale: 0.95 }}
        style={{
          boxShadow: "0 0 20px rgba(139, 92, 246, 0.5)",
        }}
      >
        <span className="relative z-10">Inner Ripple</span>
        <AnimatePresence>
          {ripples.map((ripple, index) => (
            <motion.div
              key={ripple.id}
              className="absolute left-1/2 top-1/2 rounded-full border-2 border-violet-400"
              style={{
                x: "-50%",
                y: "-50%",
                width: 0,
                height: 0,
              }}
              initial={{ width: 0, height: 0, opacity: 0.8 }}
              animate={{
                width: 200 - index * 30,
                height: 200 - index * 30,
                opacity: 0,
              }}
              exit={{ opacity: 0 }}
              transition={{
                duration: 0.6,
                delay: index * 0.1,
                ease: "easeOut",
              }}
            />
          ))}
        </AnimatePresence>
      </motion.button>
    </div>
  );
}

export default InnerRippleGlowButton;

