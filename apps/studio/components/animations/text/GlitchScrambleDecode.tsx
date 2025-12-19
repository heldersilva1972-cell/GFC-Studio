"use client";

import React, { useState, useEffect, useRef } from "react";
import { motion } from "framer-motion";
import { AnimatedTextContainer, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Glitch Decode - Characters rapidly scramble through random letters/symbols
 * before locking into the target text with RGB offset jitter
 */
export const GlitchScrambleDecode: React.FC<AnimatedTextProps> = ({
  text = "GLITCH DECODE",
  accentColor = "#10b981",
  speed = 1,
}) => {
  const letters = text.split("");
  const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
  const [displayed, setDisplayed] = useState<string[]>(
    new Array(letters.length).fill("")
  );
  const [decoded, setDecoded] = useState<boolean[]>(
    new Array(letters.length).fill(false)
  );
  const intervalsRef = useRef<NodeJS.Timeout[]>([]);

  useEffect(() => {
    // Clear any existing intervals
    intervalsRef.current.forEach((interval) => clearInterval(interval));
    intervalsRef.current = [];

    const letterArray = text.split("");
    // Reset state
    setDisplayed(new Array(letterArray.length).fill(""));
    setDecoded(new Array(letterArray.length).fill(false));

    // Create intervals for each letter
    letterArray.forEach((targetLetter, index) => {
      let iterations = 0;
      const interval = setInterval(() => {
        setDisplayed((prev) => {
          const next = [...prev];
          if (iterations < 10 + index * 3) {
            next[index] = chars[Math.floor(Math.random() * chars.length)];
          } else {
            next[index] = targetLetter === " " ? "\u00A0" : targetLetter;
            setDecoded((prevDecoded) => {
              const nextDecoded = [...prevDecoded];
              nextDecoded[index] = true;
              return nextDecoded;
            });
            clearInterval(interval);
            // Remove from ref array
            intervalsRef.current = intervalsRef.current.filter((i) => i !== interval);
          }
          iterations++;
          return next;
        });
      }, 50 / speed);
      intervalsRef.current.push(interval);
    });

    // Cleanup function
    return () => {
      intervalsRef.current.forEach((interval) => clearInterval(interval));
      intervalsRef.current = [];
    };
  }, [text, speed]);

  return (
    <AnimatedTextContainer>
      <div className="flex items-center justify-center gap-1">
        {letters.map((letter, index) => (
          <motion.span
            key={`${text}-${index}`}
            className="text-4xl md:text-6xl font-bold font-mono"
            style={{ color: accentColor }}
            animate={
              !decoded[index]
                ? {
                    x: [0, -2, 2, -1, 1, 0],
                    opacity: [1, 0.8, 1, 0.9, 1],
                  }
                : {
                    x: 0,
                    opacity: 1,
                  }
            }
            transition={{
              duration: 0.1,
              repeat: decoded[index] ? 0 : Infinity,
              ease: "easeInOut",
            }}
          >
            {displayed[index] || (letter === " " && !displayed[index]) ? displayed[index] || "\u00A0" : displayed[index]}
          </motion.span>
        ))}
      </div>
    </AnimatedTextContainer>
  );
};

export default GlitchScrambleDecode;

