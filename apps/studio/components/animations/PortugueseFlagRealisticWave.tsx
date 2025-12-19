"use client";

import React from "react";

type Props = {
  width?: number;
  height?: number;
};

const PortugueseFlagRealisticWave: React.FC<Props> = ({
  width = 520,
  height = 340,
}) => {
  return (
    <>
      <div className="flex h-full w-full items-center justify-center">
        <div
          className="relative flex items-center justify-center rounded-3xl bg-gradient-to-br from-slate-950 via-slate-900 to-slate-950 p-4 shadow-2xl shadow-black/65"
          style={{ width, height }}
        >
          {/* outer glow */}
          <div className="pointer-events-none absolute inset-2 rounded-3xl bg-gradient-to-br from-white/10 via-transparent to-black/50" />

          {/* flag frame */}
          <div className="relative isolate overflow-hidden rounded-2xl border border-slate-800/80 bg-slate-950/95 backdrop-blur-sm w-[95%] h-[90%]">
            {/* base flag (static bicolor) */}
            <div
              className="absolute inset-0 rounded-2xl"
              style={{
                background:
                  "linear-gradient(to right, #006600 0%, #006600 40%, #c1121f 40%, #c1121f 100%)",
              }}
            />

            {/* wave lighting layer */}
            <div className="pt-flag-wave-layer absolute inset-0 rounded-2xl mix-blend-soft-light opacity-80" />

            {/* emblem */}
            <div
              className="pointer-events-none absolute"
              style={{
                left: "40%",
                top: "50%",
                transform: "translate(-50%, -50%)",
                width: "26%",
                height: "30%",
                borderRadius: "999px",
                background:
                  "radial-gradient(circle at 30% 20%, #facc15 0%, #eab308 45%, #7c2d12 100%)",
                boxShadow: "0 0 16px rgba(0,0,0,0.65)",
                border: "2px solid rgba(255,255,255,0.95)",
              }}
            >
              {/* inner shield */}
              <div
                style={{
                  position: "absolute",
                  inset: "18% 24%",
                  borderRadius: "18px",
                  background:
                    "linear-gradient(to bottom, #f9fafb 0%, #e5e7eb 40%, #cbd5f5 100%)",
                  boxShadow: "0 0 8px rgba(0,0,0,0.45) inset",
                }}
              >
                {/* blue central shield with white dots */}
                <div
                  style={{
                    position: "absolute",
                    inset: "24% 32%",
                    borderRadius: "10px",
                    backgroundColor: "#1e3a8a",
                    backgroundImage:
                      "radial-gradient(circle, #eff6ff 0.8px, transparent 1.4px)",
                    backgroundSize: "22% 30%",
                    boxShadow: "0 0 4px rgba(0,0,0,0.4) inset",
                  }}
                />
              </div>
            </div>

            {/* right-edge shadow for depth */}
            <div className="pointer-events-none absolute inset-y-0 right-0 w-14 bg-gradient-to-l from-black/55 via-transparent to-transparent" />
          </div>
        </div>
      </div>

      {/* local CSS for wave animation */}
      <style jsx global>{`
        @keyframes portuguese-flag-wave-motion {
          0% {
            transform: translateY(0px) skewX(0deg);
            background-position: 0% 50%;
          }
          50% {
            transform: translateY(4px) skewX(-2deg);
            background-position: 100% 50%;
          }
          100% {
            transform: translateY(0px) skewX(0deg);
            background-position: 0% 50%;
          }
        }

        .pt-flag-wave-layer {
          background-image: linear-gradient(
            135deg,
            rgba(255, 255, 255, 0.25) 0%,
            transparent 40%,
            rgba(15, 23, 42, 0.75) 70%,
            transparent 100%
          );
          background-size: 220% 220%;
          animation: portuguese-flag-wave-motion 4.4s ease-in-out infinite;
          transform-origin: center;
        }
      `}</style>
    </>
  );
};

export default PortugueseFlagRealisticWave;
