"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextContainer, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Ribbon Text Unfold - A horizontal ribbon sweeps across, revealing text as it passes
 * The ribbon acts as a reveal mask, unwrapping the text from left to right
 */
export const RibbonTextUnfold: React.FC<AnimatedTextProps> = ({
  text = "Ribbon Unfold",
  accentColor = "#ec4899",
  speed = 1,
}) => {
  const letters = text.split("");

  return (
    <AnimatedTextContainer>
      <div className="relative flex items-center justify-center overflow-hidden">
        {/* Text - fully visible */}
        <div className="relative flex items-center justify-center z-10">
          {letters.map((letter, index) => (
            <span
              key={`${text}-${index}`}
              className="text-4xl md:text-6xl font-bold"
              style={{ color: accentColor }}
            >
              {letter === " " ? "\u00A0" : letter}
            </span>
          ))}
        </div>

        {/* Reveal mask that sweeps from left to right */}
        <motion.div
          className="absolute inset-0 bg-slate-950 z-20"
          style={{
            clipPath: "inset(0 100% 0 0)",
          }}
          animate={{
            clipPath: [
              "inset(0 100% 0 0)",
              "inset(0 0% 0 0)",
              "inset(0 0% 0 0)",
              "inset(0 100% 0 0)",
            ],
          }}
          transition={{
            duration: 2.5 / speed,
            times: [0, 0.4, 0.6, 1],
            ease: "easeInOut",
            repeat: Infinity,
            repeatDelay: 0.8 / speed,
          }}
        />

        {/* Visible ribbon bar that sweeps behind the text */}
        <motion.div
          className="absolute h-16 rounded-full z-5"
          style={{
            width: "300px",
            background: `linear-gradient(90deg, transparent, ${accentColor}60, ${accentColor}, ${accentColor}60, transparent)`,
            boxShadow: `0 0 30px ${accentColor}50`,
            filter: "blur(2px)",
          }}
          animate={{
            x: ["-350px", "calc(100% + 350px)"],
          }}
          transition={{
            duration: 2.5 / speed,
            ease: "easeInOut",
            repeat: Infinity,
            repeatDelay: 0.8 / speed,
          }}
        />
      </div>
    </AnimatedTextContainer>
  );
};

export default RibbonTextUnfold;

