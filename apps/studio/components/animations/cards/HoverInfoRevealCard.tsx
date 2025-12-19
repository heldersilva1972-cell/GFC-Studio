"use client";

import React, { useState } from "react";
import { motion } from "framer-motion";

export function HoverInfoRevealCard() {
  const [isHovered, setIsHovered] = useState(false);

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.div
        className="relative h-64 w-full max-w-sm cursor-pointer"
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => setIsHovered(false)}
        style={{ perspective: "1000px" }}
      >
        <motion.div
          className="relative h-full w-full rounded-xl border border-slate-700/50 bg-gradient-to-br from-slate-900/95 to-slate-800/95 p-8 shadow-xl"
          animate={{
            rotateY: isHovered ? 180 : 0,
          }}
          transition={{ duration: 0.6, ease: "easeInOut" }}
          style={{ transformStyle: "preserve-3d" }}
        >
          {/* Front */}
          <motion.div
            className="absolute inset-0 flex flex-col items-center justify-center rounded-xl p-8"
            style={{ backfaceVisibility: "hidden" }}
            animate={{ opacity: isHovered ? 0 : 1 }}
          >
            <h2 className="mb-2 text-4xl font-bold text-slate-100">Hello</h2>
            <p className="text-slate-400">Hover to learn more</p>
          </motion.div>

          {/* Back */}
          <motion.div
            className="absolute inset-0 flex flex-col items-center justify-center rounded-xl p-8"
            style={{ backfaceVisibility: "hidden", transform: "rotateY(180deg)" }}
            animate={{ opacity: isHovered ? 1 : 0 }}
          >
            <h3 className="mb-3 text-2xl font-bold text-slate-100">More Info</h3>
            <p className="mb-6 text-center text-sm text-slate-300">
              Discover amazing features and unlock your potential with our premium services.
            </p>
            <motion.button
              className="rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-6 py-2.5 font-semibold text-slate-900 shadow-lg"
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
            >
              Learn More
            </motion.button>
          </motion.div>
        </motion.div>
      </motion.div>
    </div>
  );
}

