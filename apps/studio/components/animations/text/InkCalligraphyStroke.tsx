"use client";

import React, { useState, useEffect } from "react";
import { Allura } from "next/font/google";
import { type AnimatedTextProps } from "./AnimatedTextBase";

const allura = Allura({ subsets: ["latin"], weight: "400" });

/**
 * Calligraphy Stroke - A moving stroke reveals each letter as if hand-written
 */
export const InkCalligraphyStroke: React.FC<AnimatedTextProps> = ({ text = "Gloucester Fraternity Club" }) => {
  const safeText = text || "";
  const [visibleLetters, setVisibleLetters] = useState(0);

  useEffect(() => {
    if (visibleLetters < safeText.length) {
      const delay = safeText[visibleLetters] === " " ? 80 : 280;
      const timer = setTimeout(() => {
        setVisibleLetters((prev) => prev + 1);
      }, delay);
      return () => clearTimeout(timer);
    }
  }, [visibleLetters, safeText]);

  // Reset on text change
  useEffect(() => {
    setVisibleLetters(0);
  }, [safeText]);

  return (
    <div className="relative flex items-center justify-center w-full h-full min-h-[220px] rounded-2xl bg-slate-950/95 shadow-xl overflow-hidden px-6">
      <svg
        viewBox="0 0 1000 200"
        className="w-full"
        role="img"
        aria-label={safeText}
        preserveAspectRatio="xMidYMid meet"
      >
        <defs>
          <linearGradient id="calligraphyStrokeGradient" x1="0%" y1="0%" x2="100%" y2="0%">
            <stop offset="0%" stopColor="#d4af37" />
            <stop offset="30%" stopColor="#c9a961" />
            <stop offset="60%" stopColor="#b8860b" />
            <stop offset="100%" stopColor="#daa520" />
          </linearGradient>
        </defs>

        {/* Soft glow layer (subtle background) */}
        <text
          x="50%"
          y="110"
          textAnchor="middle"
          dominantBaseline="middle"
          className={`${allura.className} calligraphy-stroke-text calligraphy-glow-layer`}
          style={{
            fontSize: '96px',
            fontWeight: 400,
            fill: 'transparent',
            stroke: 'rgba(212, 175, 55, 0.3)',
            strokeWidth: 2.1,
            strokeLinecap: 'round',
            strokeLinejoin: 'round',
          }}
        >
          {safeText.split("").map((char, index) => {
            const isVisible = index < visibleLetters;
            if (!isVisible) return null;
            return (
              <tspan key={index}>
                {char === " " ? "\u00A0" : char}
              </tspan>
            );
          })}
        </text>

        {/* Main calligraphy text */}
        <text
          x="50%"
          y="110"
          textAnchor="middle"
          dominantBaseline="middle"
          className={`${allura.className} calligraphy-stroke-text`}
          style={{
            fontSize: '96px',
            fontWeight: 400,
            fill: 'transparent',
            stroke: 'url(#calligraphyStrokeGradient)',
            strokeWidth: 2.1,
            strokeLinecap: 'round',
            strokeLinejoin: 'round',
          }}
        >
          {safeText.split("").map((char, index) => {
            const isVisible = index < visibleLetters;
            const isWriting = index === visibleLetters - 1;
            
            if (!isVisible) return null;
            
            return (
              <tspan
                key={index}
                className={`calligraphy-stroke-letter ${isWriting ? "writing" : "written"}`}
              >
                {char === " " ? "\u00A0" : char}
              </tspan>
            );
          })}
        </text>
      </svg>

      <style jsx>{`
        .calligraphy-stroke-text {
          font-size: 96px;
          font-weight: 400;
          fill: transparent;
          stroke: url(#calligraphyStrokeGradient);
          stroke-width: 2.1;
          stroke-linecap: round;
          stroke-linejoin: round;
        }

        .calligraphy-glow-layer {
          filter: blur(1px);
          opacity: 0.4;
        }

        .calligraphy-stroke-letter {
          stroke-dasharray: 1100;
          stroke-dashoffset: 1100;
          opacity: 0;
        }

        .calligraphy-stroke-letter.writing {
          animation: calligraphy-stroke-draw 3s ease-out forwards;
        }

        .calligraphy-stroke-letter.written {
          stroke-dashoffset: 0;
          opacity: 1;
          animation: calligraphy-stroke-glow 3s ease-in-out infinite alternate;
        }

        @keyframes calligraphy-stroke-draw {
          0% {
            stroke-dashoffset: 1100;
            opacity: 0;
            filter: blur(1.5px);
          }
          20% {
            opacity: 1;
          }
          100% {
            stroke-dashoffset: 0;
            opacity: 1;
            filter: blur(0);
          }
        }

        @keyframes calligraphy-stroke-glow {
          0% {
            stroke-width: 2.1;
          }
          100% {
            stroke-width: 2.6;
          }
        }

        @media (max-width: 768px) {
          .calligraphy-stroke-text {
            font-size: 64px !important;
          }
        }
      `}</style>
    </div>
  );
};

export default InkCalligraphyStroke;

