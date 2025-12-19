"use client";

import React from "react";
import { motion } from "framer-motion";

export function LiquidDistortionText() {
  const text = "Liquid Distortion";

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <div className="relative w-full max-w-4xl overflow-hidden rounded-2xl border border-slate-700/50 bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 p-12">
        <motion.h1
          className="text-center text-5xl font-bold text-slate-100 md:text-6xl lg:text-7xl"
          style={{
            filter: "url(#liquid-distortion)",
          }}
        >
          {text.split("").map((char, i) => (
            <motion.span
              key={i}
              className="inline-block"
              animate={{
                y: [
                  0,
                  Math.sin(i * 0.5) * 8,
                  Math.sin(i * 0.5 + Math.PI) * 8,
                  0,
                ],
                skewY: [
                  0,
                  Math.sin(i * 0.3) * 3,
                  Math.sin(i * 0.3 + Math.PI) * 3,
                  0,
                ],
                scaleY: [
                  1,
                  1 + Math.sin(i * 0.4) * 0.1,
                  1 + Math.sin(i * 0.4 + Math.PI) * 0.1,
                  1,
                ],
              }}
              transition={{
                duration: 3 + i * 0.1,
                repeat: Infinity,
                ease: "easeInOut",
                delay: i * 0.05,
              }}
            >
              {char === " " ? "\u00A0" : char}
            </motion.span>
          ))}
        </motion.h1>

        <svg className="absolute h-0 w-0">
          <defs>
            <filter id="liquid-distortion">
              <feTurbulence
                type="fractalNoise"
                baseFrequency="0.01 0.02"
                numOctaves="3"
                result="noise"
              >
                <animate
                  attributeName="baseFrequency"
                  values="0.01 0.02;0.02 0.03;0.01 0.02"
                  dur="4s"
                  repeatCount="indefinite"
                />
              </feTurbulence>
              <feDisplacementMap
                in="SourceGraphic"
                in2="noise"
                scale="5"
              />
            </filter>
          </defs>
        </svg>
      </div>
    </div>
  );
}

