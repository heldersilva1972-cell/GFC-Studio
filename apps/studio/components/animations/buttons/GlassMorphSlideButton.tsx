"use client";

import React from "react";
import { motion } from "framer-motion";
import { ButtonEffectBase } from "./ButtonEffectBase";

export function GlassMorphSlideButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border border-white/20 bg-white/10 px-8 py-4 text-base font-semibold text-white backdrop-blur-md shadow-lg"
        whileHover={{
          scale: 1.05,
          y: -4,
          boxShadow: "0 20px 40px rgba(255, 255, 255, 0.2)",
        }}
        whileTap={{ scale: 0.98 }}
        transition={{ type: "spring", stiffness: 300, damping: 20 }}
      >
        <span className="relative z-10">Glass Morph</span>
        <motion.div
          className="absolute inset-0 bg-gradient-to-br from-white/20 to-transparent"
          initial={{ opacity: 0 }}
          whileHover={{ opacity: 1 }}
        />
      </motion.button>
    </div>
  );
}

export default GlassMorphSlideButton;

