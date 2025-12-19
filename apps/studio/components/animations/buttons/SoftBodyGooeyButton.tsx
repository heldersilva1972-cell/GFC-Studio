"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function SoftBodyGooeyButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative rounded-2xl border border-emerald-500 bg-emerald-600 px-8 py-4 text-base font-semibold text-white"
        whileHover={{
          scaleX: 1.1,
          scaleY: 0.95,
          borderRadius: "1rem",
        }}
        whileTap={{
          scaleX: 0.9,
          scaleY: 1.1,
          borderRadius: "2rem",
        }}
        transition={{
          type: "spring",
          stiffness: 200,
          damping: 15,
        }}
      >
        Gooey
      </motion.button>
    </div>
  );
}

export default SoftBodyGooeyButton;

