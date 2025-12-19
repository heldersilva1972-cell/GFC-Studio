"use client";

import React, { useRef, useState, useEffect } from "react";
import { motion, useMotionValue, useSpring } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function DynamicShadowButton() {
  const [isPressed, setIsPressed] = useState(false);
  const [shadowStyle, setShadowStyle] = useState("0px 8px 24px rgba(0, 0, 0, 0.25)");
  const buttonRef = useRef<HTMLButtonElement>(null);
  const shadowX = useMotionValue(0);
  const shadowY = useMotionValue(0);
  const springShadowX = useSpring(shadowX, { stiffness: 200, damping: 25 });
  const springShadowY = useSpring(shadowY, { stiffness: 200, damping: 25 });

  useEffect(() => {
    const updateShadow = () => {
      const x = springShadowX.get();
      const y = springShadowY.get();
      
      if (isPressed) {
        // Pressed state: shadow closer, softer
        setShadowStyle("0px 2px 8px rgba(0, 0, 0, 0.4)");
      } else if (Math.abs(x) < 0.1 && Math.abs(y) < 0.1) {
        // Neutral state: centered shadow
        setShadowStyle("0px 8px 24px rgba(0, 0, 0, 0.25)");
      } else {
        // Dynamic shadow based on cursor position
        // Slightly increase blur when further from center
        const distance = Math.sqrt(x * x + y * y);
        const blur = 20 + distance * 0.5;
        setShadowStyle(`${x}px ${y}px ${blur}px rgba(0, 0, 0, 0.35)`);
      }
    };

    const unsubscribeX = springShadowX.on("change", updateShadow);
    const unsubscribeY = springShadowY.on("change", updateShadow);
    updateShadow(); // Initial update

    return () => {
      unsubscribeX();
      unsubscribeY();
    };
  }, [springShadowX, springShadowY, isPressed]);

  const handleMouseMove = (e: React.MouseEvent<HTMLButtonElement>) => {
    if (!buttonRef.current || isPressed) return;
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
    
    // Map to shadow offset (maxOffset = 18px)
    const maxOffset = 18;
    const targetShadowX = dx * maxOffset;
    const targetShadowY = dy * maxOffset;
    
    shadowX.set(targetShadowX);
    shadowY.set(targetShadowY);
  };

  const handleMouseLeave = () => {
    shadowX.set(0);
    shadowY.set(0);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        ref={buttonRef}
        type="button"
        className="relative rounded-xl border border-teal-500 bg-teal-600 px-8 py-4 text-base font-semibold text-white"
        style={{ boxShadow: shadowStyle }}
        onMouseMove={handleMouseMove}
        onMouseLeave={handleMouseLeave}
        onMouseDown={() => setIsPressed(true)}
        onMouseUp={() => setIsPressed(false)}
        whileTap={{ 
          scale: 0.98,
          y: 2, // Slight downward movement when pressed
        }}
      >
        Dynamic Shadow
      </motion.button>
    </div>
  );
}

export default DynamicShadowButton;

