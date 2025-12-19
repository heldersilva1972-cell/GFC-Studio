"use client";

import React, { useState, useRef } from "react";
import { motion, useMotionValue, useSpring } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function MagneticAttractionButton() {
  const [isHovered, setIsHovered] = useState(false);
  const buttonRef = useRef<HTMLButtonElement>(null);
  const x = useMotionValue(0);
  const y = useMotionValue(0);
  const springX = useSpring(x, { stiffness: 300, damping: 30 });
  const springY = useSpring(y, { stiffness: 300, damping: 30 });

  const handleMouseMove = (e: React.MouseEvent<HTMLButtonElement>) => {
    if (!buttonRef.current) return;
    const rect = buttonRef.current.getBoundingClientRect();
    const centerX = rect.left + rect.width / 2;
    const centerY = rect.top + rect.height / 2;
    const deltaX = (e.clientX - centerX) * 0.2;
    const deltaY = (e.clientY - centerY) * 0.2;
    x.set(deltaX);
    y.set(deltaY);
  };

  const handleMouseLeave = () => {
    setIsHovered(false);
    x.set(0);
    y.set(0);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        ref={buttonRef}
        type="button"
        className="relative rounded-xl border border-slate-700 bg-slate-800 px-8 py-4 text-base font-semibold text-slate-100"
        style={{ x: springX, y: springY }}
        onMouseMove={handleMouseMove}
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={handleMouseLeave}
        whileTap={{ scale: 1.1 }}
        transition={{ type: "spring", stiffness: 300, damping: 30 }}
      >
        Magnetic
      </motion.button>
    </div>
  );
}

export default MagneticAttractionButton;

