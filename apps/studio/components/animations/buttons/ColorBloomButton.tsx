"use client";

import React, { useState, useRef } from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function ColorBloomButton() {
  const [bloomPosition, setBloomPosition] = useState({ x: 50, y: 50 });
  const buttonRef = useRef<HTMLButtonElement>(null);

  const handleMouseMove = (e: React.MouseEvent<HTMLButtonElement>) => {
    if (!buttonRef.current) return;
    const rect = buttonRef.current.getBoundingClientRect();
    const x = ((e.clientX - rect.left) / rect.width) * 100;
    const y = ((e.clientY - rect.top) / rect.height) * 100;
    setBloomPosition({ x, y });
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        ref={buttonRef}
        type="button"
        className="relative overflow-hidden rounded-xl border border-pink-500 bg-slate-800 px-8 py-4 text-base font-semibold text-white"
        onMouseMove={handleMouseMove}
        whileHover="hover"
        initial="initial"
      >
        <span className="relative z-10">Color Bloom</span>
        <motion.div
          className="absolute rounded-full bg-gradient-to-r from-pink-500 via-purple-500 to-cyan-500"
          style={{
            left: `${bloomPosition.x}%`,
            top: `${bloomPosition.y}%`,
            x: "-50%",
            y: "-50%",
            width: 0,
            height: 0,
          }}
          variants={{
            initial: { width: 0, height: 0, opacity: 0 },
            hover: { width: 300, height: 300, opacity: 0.6 },
          }}
          transition={{ duration: 0.4, ease: "easeOut" }}
        />
      </motion.button>
    </div>
  );
}

export default ColorBloomButton;

