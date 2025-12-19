"use client";

import React, { useRef } from "react";
import { motion, useMotionValue, useSpring } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function Tilt3DParallaxButton() {
  const buttonRef = useRef<HTMLButtonElement>(null);
  const rotateX = useMotionValue(0);
  const rotateY = useMotionValue(0);
  const springRotateX = useSpring(rotateX, { stiffness: 180, damping: 20 });
  const springRotateY = useSpring(rotateY, { stiffness: 180, damping: 20 });

  const handleMouseMove = (e: React.MouseEvent<HTMLButtonElement>) => {
    if (!buttonRef.current) return;
    const rect = buttonRef.current.getBoundingClientRect();
    const centerX = rect.left + rect.width / 2;
    const centerY = rect.top + rect.height / 2;
    const width = rect.width;
    const height = rect.height;
    
    // Calculate normalized offsets in [-1, 1]
    let dx = (e.clientX - centerX) / (width / 2);
    let dy = (e.clientY - centerY) / (height / 2);
    
    // Clamp to [-1, 1]
    dx = Math.max(-1, Math.min(1, dx));
    dy = Math.max(-1, Math.min(1, dy));
    
    // Map to rotation degrees (maxTilt = 12 degrees)
    const maxTilt = 12;
    const targetRotateY = dx * maxTilt;
    const targetRotateX = -dy * maxTilt; // Negative so top → tilt toward viewer
    
    rotateY.set(targetRotateY);
    rotateX.set(targetRotateX);
  };

  const handleMouseLeave = () => {
    rotateX.set(0);
    rotateY.set(0);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8" style={{ perspective: "800px" }}>
      <motion.button
        ref={buttonRef}
        type="button"
        className="relative rounded-xl border border-blue-500 bg-blue-600 px-8 py-4 text-base font-semibold text-white"
        style={{
          rotateX: springRotateX,
          rotateY: springRotateY,
          transformStyle: "preserve-3d",
        }}
        onMouseMove={handleMouseMove}
        onMouseLeave={handleMouseLeave}
        whileHover={{ scale: 1.03 }}
        whileTap={{ scale: 0.95 }}
      >
        3D Tilt
      </motion.button>
    </div>
  );
}

export default Tilt3DParallaxButton;

