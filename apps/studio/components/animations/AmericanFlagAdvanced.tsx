"use client";

import React, { useEffect, useRef } from "react";

export type AmericanFlagAdvancedProps = {
  width?: number;
  height?: number;
  slices?: number;
  waveSpeed?: number;
  waveAmplitude?: number;
  brightnessAmplitude?: number;
};

export const AmericanFlagAdvanced: React.FC<AmericanFlagAdvancedProps> = ({
  width = 480,
  height = 320,
  slices = 32,
  waveSpeed = 2.2,
  waveAmplitude = 16,
  brightnessAmplitude = 0.28,
}) => {
  const containerRef = useRef<HTMLDivElement | null>(null);
  const frameRef = useRef<number | null>(null);
  const startTimeRef = useRef<number | null>(null);

  useEffect(() => {
    const container = containerRef.current;
    if (!container) return;

    const sliceEls = Array.from(
      container.querySelectorAll<HTMLDivElement>("[data-flag-slice]")
    );
    if (!sliceEls.length) return;

    const phaseOffset = (Math.PI * 2) / sliceEls.length;

    const animate = (timestamp: number) => {
      if (startTimeRef.current == null) {
        startTimeRef.current = timestamp;
      }
      const elapsed = (timestamp - startTimeRef.current) / 1000;

      sliceEls.forEach((el, index) => {
        const phase = elapsed * waveSpeed + index * phaseOffset;
        const wave = Math.sin(phase);
        const angle = wave * waveAmplitude;
        const depth = wave * 8;

        const brightness = 1 + wave * brightnessAmplitude;
        const saturate = 1 + Math.abs(wave) * 0.25;

        el.style.transform = `
          translateZ(0)
          translateY(${wave * 4}px)
          translateX(${depth * 0.4}px)
          rotateY(${angle}deg)
        `;
        el.style.filter = `
          brightness(${brightness.toFixed(3)})
          saturate(${saturate.toFixed(3)})
          contrast(1.03)
        `;
      });

      frameRef.current = window.requestAnimationFrame(animate);
    };

    frameRef.current = window.requestAnimationFrame(animate);

    return () => {
      if (frameRef.current != null) {
        window.cancelAnimationFrame(frameRef.current);
      }
    };
  }, [waveSpeed, waveAmplitude, brightnessAmplitude]);

  const sliceWidthPercent = 100 / slices;

  return (
    <div
      className="relative flex items-center justify-center"
      style={{ width, height }}
    >
      <div className="absolute inset-0 rounded-3xl bg-gradient-to-br from-slate-900 via-slate-950 to-slate-900 shadow-2xl shadow-sky-900/30" />
      <div
        ref={containerRef}
        className="relative isolate overflow-hidden rounded-2xl border border-slate-800/70 bg-slate-900/70 backdrop-blur-sm"
        style={{
          width: width * 0.94,
          height: height * 0.9,
          transformStyle: "preserve-3d",
          perspective: "800px",
        }}
      >
        <div className="pointer-events-none absolute inset-0 bg-gradient-to-br from-white/8 via-transparent to-black/35 mix-blend-soft-light" />

        <div className="relative h-full w-full">
          {Array.from({ length: slices }).map((_, i) => {
            const leftPercent = i * sliceWidthPercent;
            return (
              <div
                key={i}
                data-flag-slice
                className="absolute top-0 h-full origin-center will-change-transform will-change-filter"
                style={{
                  left: `${leftPercent}%`,
                  width: `${sliceWidthPercent + 0.5}%`,
                  transformStyle: "preserve-3d",
                  backgroundImage: `
                    repeating-linear-gradient(
                      to bottom,
                      #c1121f 0px,
                      #c1121f 20px,
                      #ffffff 20px,
                      #ffffff 40px
                    ),
                    radial-gradient(circle at 25% 20%, #1d4ed8 0%, #020617 60%)
                  `,
                  backgroundSize: `
                    100% 100%,
                    65% 55%
                  `,
                  backgroundPosition: `
                    0 0,
                    0 0
                  `,
                  backgroundRepeat: "no-repeat",
                  maskImage: `
                    linear-gradient(to bottom, black 0, black 55%, transparent 55%, transparent 100%)
                  `,
                  WebkitMaskImage: `
                    linear-gradient(to bottom, black 0, black 55%, transparent 55%, transparent 100%)
                  `,
                  boxShadow:
                    "0 0 18px rgba(15,23,42,0.6), inset 0 0 12px rgba(15,23,42,0.85)",
                }}
              />
            );
          })}
          <div
            className="pointer-events-none absolute"
            style={{
              left: 0,
              top: 0,
              width: "38%",
              height: "45%",
              backgroundImage: "radial-gradient(circle, white 0.6px, transparent 1px)",
              backgroundSize: "6% 12%",
              opacity: 0.85,
              mixBlendMode: "screen",
            }}
          />
        </div>

        <div className="pointer-events-none absolute inset-y-0 right-0 w-10 bg-gradient-to-l from-black/40 via-transparent to-transparent" />
      </div>
    </div>
  );
};

export default AmericanFlagAdvanced;

