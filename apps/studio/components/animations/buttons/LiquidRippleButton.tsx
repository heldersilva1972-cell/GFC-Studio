"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

interface Ripple {
  id: number;
  x: number;
  y: number;
}

export function LiquidRippleButton() {
  const [ripples, setRipples] = useState<Ripple[]>([]);

  const handleClick = (e: React.MouseEvent<HTMLButtonElement>) => {
    const rect = e.currentTarget.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;
    const newRipple: Ripple = {
      id: Date.now(),
      x,
      y,
    };
    setRipples((prev) => [...prev, newRipple]);
    setTimeout(() => {
      setRipples((prev) => prev.filter((r) => r.id !== newRipple.id));
    }, 600);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border border-slate-700 bg-slate-800 px-8 py-4 text-base font-semibold text-slate-100"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
      >
        <span className="relative z-10">Liquid Ripple</span>
        <AnimatePresence>
          {ripples.map((ripple) => (
            <motion.div
              key={ripple.id}
              className="absolute rounded-full border-2 border-cyan-400/60"
              style={{
                left: ripple.x,
                top: ripple.y,
                width: 0,
                height: 0,
                x: "-50%",
                y: "-50%",
              }}
              initial={{ width: 0, height: 0, opacity: 1 }}
              animate={{ width: 200, height: 200, opacity: 0 }}
              exit={{ opacity: 0 }}
              transition={{ duration: 0.6, ease: "easeOut" }}
            />
          ))}
        </AnimatePresence>
      </motion.button>
    </div>
  );
}

export default LiquidRippleButton;

