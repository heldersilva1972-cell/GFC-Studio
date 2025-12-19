"use client";

import React from "react";
import { motion } from "framer-motion";

export function MagicPlayButton() {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <div className="relative">
        {/* Glow blobs */}
        {[0, 1, 2].map((i) => (
          <motion.div
            key={i}
            className="absolute inset-0 rounded-full blur-3xl"
            style={{
              background: `radial-gradient(circle, ${
                i === 0 ? "rgba(251, 191, 36, 0.4)" : i === 1 ? "rgba(139, 92, 246, 0.3)" : "rgba(59, 130, 246, 0.3)"
              }, transparent)`,
            }}
            animate={{
              x: [0, Math.cos(i * 2) * 30, 0],
              y: [0, Math.sin(i * 2) * 30, 0],
              scale: [1, 1.2, 1],
            }}
            transition={{
              duration: 4 + i,
              repeat: Infinity,
              ease: "easeInOut",
            }}
          />
        ))}

        {/* Button */}
        <motion.button
          className="relative z-10 rounded-full bg-gradient-to-r from-amber-500 via-violet-500 to-blue-500 px-12 py-4 text-lg font-bold text-white shadow-2xl"
          initial={{ scale: 0.9, opacity: 0 }}
          animate={{ scale: 1, opacity: 1 }}
          transition={{ duration: 0.5, type: "spring" }}
          whileHover={{
            scale: 1.05,
            y: -4,
            boxShadow: "0 20px 40px rgba(251, 191, 36, 0.4)",
          }}
          whileTap={{ scale: 0.95, y: 0 }}
        >
          <motion.div
            className="flex items-center gap-3"
            whileHover={{ x: 2 }}
          >
            <motion.svg
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="currentColor"
              animate={{ rotate: [0, 5, -5, 0] }}
              transition={{ duration: 2, repeat: Infinity, ease: "easeInOut" }}
            >
              <path d="M8 5v14l11-7z" />
            </motion.svg>
            <span>Get Started</span>
          </motion.div>
        </motion.button>
      </div>
    </div>
  );
}

