"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Origami Fold - Each letter folds open from a flat line via 3D rotation
 */
export const OrigamiFoldText: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#6366f1",
  speed = 1,
}) => {
  const letters = text.split("");

  return (
    <AnimatedTextBase>
      <div className="flex items-center justify-center" style={{ perspective: "1000px" }}>
        {letters.map((letter, index) => (
          <motion.span
            key={index}
            className="text-6xl font-bold inline-block"
            style={{
              color: accentColor,
              transformStyle: "preserve-3d",
            }}
            initial={{ rotateX: -90, opacity: 0 }}
            animate={{ rotateX: 0, opacity: 1 }}
            transition={{
              delay: index * 0.1 / speed,
              duration: 0.6 / speed,
              ease: "easeOut",
            }}
          >
            {letter === " " ? "\u00A0" : letter}
          </motion.span>
        ))}
      </div>
    </AnimatedTextBase>
  );
};

export default OrigamiFoldText;

