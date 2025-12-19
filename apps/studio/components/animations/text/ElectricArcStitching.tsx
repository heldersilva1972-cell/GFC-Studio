"use client";

import React, { useState, useEffect, useRef } from "react";
import { motion } from "framer-motion";
import { AnimatedTextBase, type AnimatedTextProps } from "./AnimatedTextBase";

/**
 * Electric Stitch - Electric arcs travel along baseline; letters appear as arcs pass
 */
export const ElectricArcStitching: React.FC<AnimatedTextProps> = ({
  text = "Animation Playground",
  accentColor = "#fbbf24",
  speed = 1,
}) => {
  const letters = text.split("");
  const [revealed, setRevealed] = useState<number>(0);
  const [arcPosition, setArcPosition] = useState(0);
  const containerRef = useRef<HTMLDivElement>(null);
  const letterRefs = useRef<(HTMLSpanElement | null)[]>([]);
  const [textWidth, setTextWidth] = useState(0);
  const [textStartX, setTextStartX] = useState(0);

  // Measure text width and position
  useEffect(() => {
    const measureText = () => {
      if (containerRef.current) {
        const container = containerRef.current;
        const textContainer = container.querySelector('.text-container') as HTMLElement;
        if (textContainer) {
          const rect = textContainer.getBoundingClientRect();
          const containerRect = container.getBoundingClientRect();
          setTextWidth(rect.width);
          setTextStartX(rect.left - containerRect.left);
        }
      }
    };
    
    measureText();
    // Re-measure on window resize
    window.addEventListener('resize', measureText);
    return () => window.removeEventListener('resize', measureText);
  }, [text, letters]);

  useEffect(() => {
    const interval = setInterval(() => {
      setArcPosition((prev) => {
        const step = 8 * speed;
        const next = prev + step;
        const totalWidth = textWidth || letters.length * 40;
        
        // Reset when arc completes
        if (next > totalWidth + 100) {
          setRevealed(0);
          return -100; // Start slightly before text
        }
        
        // Calculate which letters should be revealed based on arc position
        let newRevealed = 0;
        const containerRect = containerRef.current?.getBoundingClientRect();
        if (containerRect) {
          const arcCenter = textStartX + next;
          
          letterRefs.current.forEach((ref, index) => {
            if (ref) {
              const rect = ref.getBoundingClientRect();
              const letterLeft = rect.left - containerRect.left;
              const letterRight = rect.right - containerRect.left;
              const letterCenter = letterLeft + (letterRight - letterLeft) / 2;
              
              // Reveal letter when arc passes its center
              if (arcCenter >= letterCenter - 10) {
                newRevealed = Math.max(newRevealed, index + 1);
              }
            }
          });
        }
        
        if (newRevealed > revealed) {
          setRevealed(newRevealed);
        }
        
        return next;
      });
    }, 16); // ~60fps

    return () => clearInterval(interval);
  }, [letters.length, revealed, speed, textWidth, textStartX]);

  return (
    <AnimatedTextBase>
      <div ref={containerRef} className="relative flex items-center justify-center w-full">
        <div className="text-container flex items-center justify-center">
          {letters.map((letter, index) => (
            <motion.span
              key={index}
              ref={(el) => (letterRefs.current[index] = el)}
              className="text-6xl font-bold inline-block"
              style={{ 
                color: accentColor,
                minWidth: letter === " " ? "0.3em" : "auto",
              }}
              initial={{ opacity: 0 }}
              animate={{
                opacity: index < revealed ? 1 : 0,
                textShadow: index < revealed 
                  ? `0 0 10px ${accentColor}, 0 0 20px ${accentColor}40` 
                  : "none",
                filter: index < revealed ? "brightness(1.2)" : "brightness(0.3)",
              }}
              transition={{ duration: 0.15 }}
            >
              {letter === " " ? "\u00A0" : letter}
            </motion.span>
          ))}
        </div>
        
        {/* Electric arc */}
        <motion.div
          className="absolute top-1/2 -translate-y-1/2 h-2 w-24 pointer-events-none"
          style={{
            left: `${textStartX}px`,
            background: `linear-gradient(90deg, transparent, ${accentColor}, transparent)`,
            boxShadow: `0 0 20px ${accentColor}, 0 0 40px ${accentColor}80`,
            clipPath: "polygon(0 50%, 15% 0, 25% 50%, 50% 100%, 75% 50%, 85% 0, 100% 50%)",
            filter: "blur(0.5px)",
          }}
          animate={{
            x: arcPosition,
            opacity: [0.6, 1, 0.6],
            scale: [1, 1.1, 1],
          }}
          transition={{
            opacity: {
              duration: 0.3,
              repeat: Infinity,
              ease: "easeInOut",
            },
            scale: {
              duration: 0.3,
              repeat: Infinity,
              ease: "easeInOut",
            },
          }}
        />
      </div>
    </AnimatedTextBase>
  );
};

export default ElectricArcStitching;

