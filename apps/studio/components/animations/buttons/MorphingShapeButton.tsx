"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function MorphingShapeButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative rounded-xl border border-indigo-500 bg-indigo-600 px-8 py-4 text-base font-semibold text-white"
        whileHover={{
          borderRadius: [
            "0.75rem",
            "9999px",
            "1.5rem",
            "9999px",
            "0.75rem",
          ],
          scaleX: [1, 1.1, 0.95, 1.1, 1],
        }}
        transition={{
          duration: 2,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      >
        Morphing
      </motion.button>
    </div>
  );
}

export default MorphingShapeButton;

