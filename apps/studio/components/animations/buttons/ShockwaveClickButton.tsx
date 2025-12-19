"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

interface Shockwave {
  id: number;
}

export function ShockwaveClickButton() {
  const [shockwaves, setShockwaves] = useState<Shockwave[]>([]);

  const handleClick = () => {
    const newShockwave: Shockwave = { id: Date.now() };
    setShockwaves((prev) => [...prev, newShockwave]);
    setTimeout(() => {
      setShockwaves((prev) => prev.filter((s) => s.id !== newShockwave.id));
    }, 800);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative rounded-xl border border-purple-500 bg-purple-600 px-8 py-4 text-base font-semibold text-white"
        onClick={handleClick}
        whileTap={{ scale: 0.95 }}
      >
        <span className="relative z-10">Shockwave</span>
        <AnimatePresence>
          {shockwaves.map((wave) => (
            <motion.div
              key={wave.id}
              className="absolute inset-0 rounded-xl border-2 border-purple-400"
              initial={{ scale: 1, opacity: 1 }}
              animate={{ scale: 2.5, opacity: 0 }}
              exit={{ opacity: 0 }}
              transition={{ duration: 0.8, ease: "easeOut" }}
            />
          ))}
        </AnimatePresence>
      </motion.button>
    </div>
  );
}

export default ShockwaveClickButton;

