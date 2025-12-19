"use client";

import React from "react";

/**
 * American Flag – Photographic Wave
 * A realistic American flag using a photographic image with cloth-like ripple effect
 */
export default function AmericanFlagWave({
  size = 600,
  speed = 1,
}: {
  size?: number;
  speed?: number;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center bg-slate-950">
      <svg
        className="w-[80%] max-w-5xl rounded-xl shadow-xl"
        viewBox="0 0 1920 1080"
        preserveAspectRatio="xMidYMid meet"
      >
        <defs>
          <filter id="usa-flag-wave-filter">
            <feTurbulence
              type="fractalNoise"
              baseFrequency="0.01 0.03"
              numOctaves="2"
              seed="2"
              result="noise"
            >
              <animate
                attributeName="baseFrequency"
                dur={`${6 / speed}s`}
                values="0.01 0.03; 0.015 0.04; 0.01 0.03"
                repeatCount="indefinite"
              />
            </feTurbulence>
            <feDisplacementMap
              in="SourceGraphic"
              in2="noise"
              scale="18"
              xChannelSelector="R"
              yChannelSelector="G"
            />
          </filter>
        </defs>

        <image
          href="/flags/us-real.png"
          x="0"
          y="0"
          width="1920"
          height="1080"
          preserveAspectRatio="xMidYMid meet"
          filter="url(#usa-flag-wave-filter)"
        />
      </svg>
    </div>
  );
}
