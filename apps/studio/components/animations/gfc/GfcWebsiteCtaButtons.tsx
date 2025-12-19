"use client";

import React, { useState } from "react";
import { motion } from "framer-motion";

export interface GfcCtaButtonProps {
  label?: string;
  onClickOverride?: () => void;
}

// 1. Rent Hall CTA
export function GfcCtaRentHallButton({
  label = "Rent the Hall",
  onClickOverride,
}: GfcCtaButtonProps) {
  const [clicked, setClicked] = useState(false);

  const handleClick = () => {
    setClicked(true);
    setTimeout(() => setClicked(false), 600);
    onClickOverride?.();
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="group relative flex items-center gap-3 rounded-full bg-gradient-to-r from-slate-800 via-slate-700 to-amber-600 px-8 py-3 font-semibold text-slate-50 shadow-lg transition-all"
        onClick={handleClick}
        whileHover={{ scale: 1.05 }}
        whileTap={{ scale: 0.98 }}
        animate={{
          background: clicked
            ? "linear-gradient(to right, rgb(30 41 59), rgb(51 65 85), rgb(217 119 6))"
            : "linear-gradient(to right, rgb(30 41 59), rgb(51 65 85), rgb(234 179 8))",
        }}
      >
        <span>{label}</span>
        <motion.svg
          className="h-5 w-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
          animate={{
            x: clicked ? [0, 8, 0] : 0,
          }}
          transition={{
            duration: 0.3,
            ease: "easeInOut",
          }}
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M13 7l5 5m0 0l-5 5m5-5H6"
          />
        </motion.svg>
      </motion.button>
    </div>
  );
}

// 2. Join CTA
export function GfcCtaJoinButton({
  label = "Join the Club",
  onClickOverride,
}: GfcCtaButtonProps) {
  const [clicked, setClicked] = useState(false);

  const handleClick = () => {
    setClicked(true);
    setTimeout(() => setClicked(false), 400);
    onClickOverride?.();
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-lg border-2 border-slate-600 bg-slate-800 px-8 py-3 font-semibold text-slate-50 shadow-lg transition-all"
        onClick={handleClick}
        whileHover={{
          borderColor: "rgb(234 179 8)",
          boxShadow: "0 0 20px rgba(234, 179, 8, 0.4)",
        }}
        animate={{
          x: clicked ? [0, -4, 4, -4, 4, 0] : 0,
        }}
        transition={{
          duration: 0.3,
          ease: "easeInOut",
        }}
      >
        <motion.div
          className="absolute inset-0 bg-amber-500/30"
          initial={{ opacity: 0 }}
          animate={{ opacity: clicked ? [0, 0.6, 0] : 0 }}
          transition={{ duration: 0.4 }}
        />
        <div className="relative flex items-center gap-2">
          <svg
            className="h-5 w-5"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"
            />
          </svg>
          <span>{label}</span>
        </div>
      </motion.button>
    </div>
  );
}

// 3. Email CTA
export function GfcCtaEmailButton({
  label = "Email Us",
  onClickOverride,
}: GfcCtaButtonProps) {
  const [clicked, setClicked] = useState(false);

  const handleClick = () => {
    setClicked(true);
    setTimeout(() => setClicked(false), 600);
    onClickOverride?.();
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="group relative flex items-center gap-3 rounded-lg bg-gradient-to-r from-slate-700 to-slate-800 px-8 py-3 font-semibold text-slate-50 shadow-lg"
        onClick={handleClick}
        whileHover={{ scale: 1.05 }}
        whileTap={{ scale: 0.98 }}
        animate={{
          scale: clicked ? [1, 1.1, 1] : 1,
        }}
        transition={{
          duration: 0.4,
          ease: "easeOut",
        }}
      >
        <motion.svg
          className="h-5 w-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
          animate={{
            rotateX: clicked ? [0, -20, 0] : 0,
          }}
          transition={{
            duration: 0.4,
            ease: "easeOut",
          }}
          style={{ transformStyle: "preserve-3d" }}
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
          />
        </motion.svg>
        <span>{label}</span>
        {clicked && (
          <motion.div
            className="absolute left-8 top-2 h-3 w-4 rounded-sm bg-slate-200"
            initial={{ y: 0, opacity: 0 }}
            animate={{ y: -8, opacity: [0, 1, 0] }}
            transition={{ duration: 0.4 }}
          />
        )}
      </motion.button>
    </div>
  );
}

// 4. Donate CTA
export function GfcCtaDonateButton({
  label = "Donate",
  onClickOverride,
}: GfcCtaButtonProps) {
  const [clicked, setClicked] = useState(false);

  const handleClick = () => {
    setClicked(true);
    setTimeout(() => setClicked(false), 600);
    onClickOverride?.();
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="group relative flex items-center gap-3 rounded-lg bg-gradient-to-r from-red-600 to-red-700 px-8 py-3 font-semibold text-white shadow-lg"
        onClick={handleClick}
        whileHover={{ scale: 1.05 }}
        whileTap={{ scale: 0.98 }}
      >
        <motion.svg
          className="h-5 w-5"
          fill="currentColor"
          viewBox="0 0 24 24"
          animate={{
            scale: clicked
              ? [1, 1.3, 1.1, 1]
              : [1, 1.1, 1, 1.1, 1],
          }}
          transition={{
            duration: clicked ? 0.5 : 1.5,
            repeat: clicked ? 0 : Infinity,
            ease: "easeInOut",
          }}
        >
          <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
        </motion.svg>
        <span>{label}</span>
        {clicked && (
          <motion.div
            className="absolute inset-0 rounded-lg bg-amber-400/40"
            initial={{ scale: 0, opacity: 0.8 }}
            animate={{ scale: 2, opacity: 0 }}
            transition={{ duration: 0.5 }}
          />
        )}
      </motion.button>
    </div>
  );
}

// 5. Learn More CTA
export function GfcCtaLearnMoreButton({
  label = "Learn More",
  onClickOverride,
}: GfcCtaButtonProps) {
  const [clicked, setClicked] = useState(false);

  const handleClick = () => {
    setClicked(true);
    setTimeout(() => setClicked(false), 400);
    onClickOverride?.();
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="group relative flex items-center gap-2 rounded-lg border-2 border-slate-600 bg-slate-800 px-8 py-3 font-semibold text-slate-50 shadow-lg transition-colors hover:text-amber-400"
        onClick={handleClick}
        whileHover={{ scale: 1.05 }}
        whileTap={{ scale: 0.98 }}
        animate={{
          borderWidth: clicked ? [2, 4, 2] : 2,
        }}
        transition={{
          duration: 0.3,
          ease: "easeInOut",
        }}
      >
        <span>{label}</span>
        <motion.svg
          className="h-5 w-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
          animate={{
            x: clicked ? [0, 4, -2, 0] : 0,
          }}
          transition={{
            duration: 0.3,
            ease: "easeInOut",
          }}
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M9 5l7 7-7 7"
          />
        </motion.svg>
      </motion.button>
    </div>
  );
}

// 6. Book Now CTA
export function GfcCtaBookNowButton({
  label = "Book Now",
  onClickOverride,
}: GfcCtaButtonProps) {
  const [clicked, setClicked] = useState(false);

  const handleClick = () => {
    setClicked(true);
    setTimeout(() => setClicked(false), 600);
    onClickOverride?.();
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="group relative flex items-center gap-3 rounded-lg bg-gradient-to-r from-amber-500 to-amber-600 px-8 py-3 font-semibold text-slate-900 shadow-lg"
        onClick={handleClick}
        whileHover={{ scale: 1.05 }}
        whileTap={{ scale: 0.98 }}
        animate={{
          y: clicked ? [0, -4, 0] : 0,
        }}
        transition={{
          duration: 0.3,
          ease: "easeOut",
        }}
      >
        <motion.div
          className="relative"
          animate={{
            rotate: clicked ? [0, -5, 5, 0] : 0,
          }}
          transition={{
            duration: 0.4,
            ease: "easeInOut",
          }}
        >
          <svg
            className="h-5 w-5"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"
            />
          </svg>
          <motion.div
            className="absolute left-0 top-0 h-2 w-full bg-slate-900/20"
            animate={{
              y: clicked ? [0, -2, 0] : 0,
            }}
            transition={{
              duration: 0.3,
              ease: "easeInOut",
            }}
          />
        </motion.div>
        <span>{label}</span>
        {clicked && (
          <motion.div
            className="absolute inset-0 rounded-lg bg-white/20"
            initial={{ rotateY: 0 }}
            animate={{ rotateY: 180 }}
            transition={{ duration: 0.3 }}
          />
        )}
      </motion.button>
    </div>
  );
}

// 7. Contact CTA
export function GfcCtaContactButton({
  label = "Contact Us",
  onClickOverride,
}: GfcCtaButtonProps) {
  const [clicked, setClicked] = useState(false);

  const handleClick = () => {
    setClicked(true);
    setTimeout(() => {
      setClicked(false);
    }, 1500);
    onClickOverride?.();
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="group relative flex items-center gap-3 rounded-lg bg-gradient-to-r from-blue-600 to-blue-700 px-8 py-3 font-semibold text-white shadow-lg"
        onClick={handleClick}
        whileHover={{ scale: 1.05, y: -2 }}
        whileTap={{ scale: 0.98 }}
        animate={{
          boxShadow: clicked
            ? "0 10px 25px rgba(37, 99, 235, 0.4)"
            : "0 4px 6px rgba(0, 0, 0, 0.1)",
        }}
      >
        <motion.div
          className="relative"
          animate={{
            y: clicked ? [0, -2, 0] : 0,
          }}
          transition={{
            duration: 0.3,
            ease: "easeInOut",
          }}
        >
          <svg
            className="h-5 w-5"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z"
            />
          </svg>
          {clicked && (
            <div className="absolute left-1/2 top-1/2 flex -translate-x-1/2 -translate-y-1/2 gap-1">
              {[0, 1, 2].map((i) => (
                <motion.div
                  key={i}
                  className="h-1.5 w-1.5 rounded-full bg-white"
                  initial={{ scale: 0, opacity: 0 }}
                  animate={{
                    scale: [0, 1, 1, 0],
                    opacity: [0, 1, 1, 0],
                  }}
                  transition={{
                    duration: 0.6,
                    delay: i * 0.15,
                    ease: "easeInOut",
                  }}
                />
              ))}
            </div>
          )}
        </motion.div>
        <span>{label}</span>
      </motion.button>
    </div>
  );
}

