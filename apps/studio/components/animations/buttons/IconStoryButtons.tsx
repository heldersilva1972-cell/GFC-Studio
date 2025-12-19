"use client";

import React, { useState } from "react";
import { motion, AnimatePresence } from "framer-motion";

export interface IconStoryButtonProps {
  label?: string;
  onComplete?: () => void;
}

// 1. Envelope → Paper Plane
export function EnvelopeToPaperPlaneButton({ label = "Send Email", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "opening" | "flying" | "sent">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("opening");
    setTimeout(() => setPhase("flying"), 400);
    setTimeout(() => {
      setPhase("sent");
      onComplete?.();
      setTimeout(() => {
        setPhase("idle");
        setIsAnimating(false);
      }, 800);
    }, 1200);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="envelope"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
              </motion.svg>
            )}
            {phase === "opening" && (
              <motion.svg
                key="opening"
                initial={{ rotateX: 0 }}
                animate={{ rotateX: -45 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
                style={{ transformStyle: "preserve-3d" }}
              >
                <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
              </motion.svg>
            )}
            {phase === "flying" && (
              <motion.svg
                key="plane"
                initial={{ x: 0, y: 0, rotate: 0, scale: 0.8 }}
                animate={{ x: 100, y: -30, rotate: 15, scale: 1 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8 absolute"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z" />
              </motion.svg>
            )}
            {phase === "sent" && (
              <motion.div
                key="sent"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                className="flex items-center gap-2"
              >
                <svg className="w-6 h-6 text-emerald-400" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
                </svg>
                <span className="text-sm">Sent!</span>
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 2. Envelope Flap Bounce + Paper Burst
export function EnvelopeFlapBurstButton({ label = "Send", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "bouncing" | "burst" | "check">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("bouncing");
    setTimeout(() => setPhase("burst"), 300);
    setTimeout(() => {
      setPhase("check");
      onComplete?.();
      setTimeout(() => {
        setPhase("idle");
        setIsAnimating(false);
      }, 600);
    }, 800);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence>
            {phase === "idle" && (
              <motion.svg
                key="envelope"
                initial={{ scale: 1 }}
                animate={{ scale: 1 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
              </motion.svg>
            )}
            {phase === "bouncing" && (
              <motion.svg
                key="bouncing"
                initial={{ scale: 1 }}
                animate={{ scale: [1, 1.2, 1] }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
              </motion.svg>
            )}
            {phase === "burst" && (
              <motion.div
                key="paper"
                initial={{ y: 0, opacity: 1, scale: 1 }}
                animate={{ y: -30, opacity: 0, scale: 0.5 }}
                className="absolute w-4 h-4 bg-slate-300 rounded"
              />
            )}
            {phase === "check" && (
              <motion.svg
                key="check"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 3. Paper Plane Runway Takeoff
export function PaperPlaneRunwayButton({ label = "Take Off", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "rolling" | "flying">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("rolling");
    setTimeout(() => setPhase("flying"), 600);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1500);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-16 h-12">
          {phase === "idle" && (
            <>
              <div className="absolute bottom-0 left-0 right-0 h-0.5 bg-slate-600" />
              <motion.svg
                className="w-8 h-8 relative z-10"
                fill="currentColor"
                viewBox="0 0 24 24"
                initial={{ x: -20 }}
                animate={{ x: 0 }}
              >
                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z" />
              </motion.svg>
            </>
          )}
          {phase === "rolling" && (
            <motion.svg
              className="w-8 h-8"
              fill="currentColor"
              viewBox="0 0 24 24"
              animate={{ x: [0, 20, 0], y: [0, -2, 0] }}
              transition={{ duration: 0.6, repeat: Infinity }}
            >
              <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z" />
            </motion.svg>
          )}
          {phase === "flying" && (
            <motion.svg
              className="w-8 h-8 absolute"
              fill="currentColor"
              viewBox="0 0 24 24"
              initial={{ x: 0, y: 0, rotate: 0 }}
              animate={{ x: 80, y: -40, rotate: 15 }}
            >
              <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z" />
            </motion.svg>
          )}
        </div>
      </motion.button>
    </div>
  );
}

// 4. Inbox Zero Sweep
export function InboxZeroSweepButton({ label = "Clear Inbox", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "sweeping" | "empty">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("sweeping");
    setTimeout(() => {
      setPhase("empty");
      onComplete?.();
      setTimeout(() => {
        setPhase("idle");
        setIsAnimating(false);
      }, 1000);
    }, 1000);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="inbox"
                initial={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 16H5v-3h3.56c.69 1.19 1.97 2 3.45 2s2.75-.81 3.45-2H19v3zm0-5h-4.99c0 1.1-.9 2-2 2s-2-.9-2-2H5V5h14v9z" />
              </motion.svg>
            )}
            {phase === "sweeping" && (
              <motion.div
                key="swirl"
                className="absolute w-12 h-12"
                animate={{ rotate: 360 }}
                transition={{ duration: 1, ease: "easeInOut" }}
              >
                <div className="w-full h-0.5 bg-emerald-400" />
              </motion.div>
            )}
            {phase === "empty" && (
              <motion.svg
                key="check"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 5. Bubble → Rocket Bubble
export function ChatBubbleRocketButton({ label = "Send Message", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "morphing" | "launching">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("morphing");
    setTimeout(() => setPhase("launching"), 500);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1200);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="bubble"
                initial={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2z" />
              </motion.svg>
            )}
            {phase === "morphing" && (
              <motion.div
                key="rocket"
                initial={{ scaleX: 1, scaleY: 1 }}
                animate={{ scaleX: 0.6, scaleY: 1.2 }}
                className="w-8 h-8 bg-emerald-400 rounded-full"
              />
            )}
            {phase === "launching" && (
              <motion.svg
                key="rocket-svg"
                initial={{ y: 0 }}
                animate={{ y: -60, rotate: -45 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M2.81 14.12L5.64 11.3l-1.42-1.42L1.39 12.7c-.39.39-.39 1.02 0 1.42zm5.64 0l5.66-5.66-1.41-1.41-5.66 5.66-1.42 1.41zm11.31-1.42l-1.41-1.41-5.66 5.66 1.41 1.41 5.66-5.66zm-7.07 7.07l-1.41-1.41 5.66-5.66 1.41 1.41-5.66 5.66z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 6. Typing Dots → Message Blast
export function TypingDotsBlastButton({ label = "Typing...", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "blasting" | "sent">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("blasting");
    setTimeout(() => {
      setPhase("sent");
      onComplete?.();
      setTimeout(() => {
        setPhase("idle");
        setIsAnimating(false);
      }, 800);
    }, 600);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-16 h-12">
          {phase === "idle" && (
            <div className="flex gap-1">
              {[0, 1, 2].map((i) => (
                <motion.div
                  key={i}
                  className="w-2 h-2 bg-slate-400 rounded-full"
                  animate={{ y: [0, -8, 0] }}
                  transition={{ duration: 0.6, delay: i * 0.1, repeat: Infinity }}
                />
              ))}
            </div>
          )}
          {phase === "blasting" && (
            <motion.div
              initial={{ scale: 1, x: 0 }}
              animate={{ scale: 0.3, x: 60 }}
              className="w-4 h-4 bg-emerald-400 rounded-full"
            />
          )}
          {phase === "sent" && (
            <motion.svg
              initial={{ scale: 0 }}
              animate={{ scale: 1 }}
              className="w-6 h-6 text-emerald-400"
              fill="currentColor"
              viewBox="0 0 24 24"
            >
              <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
            </motion.svg>
          )}
        </div>
      </motion.button>
    </div>
  );
}

// 7. Folder → Arrow Launch
export function FolderArrowLaunchButton({ label = "Open", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "opening" | "launching">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("opening");
    setTimeout(() => setPhase("launching"), 400);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1200);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="folder"
                initial={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M10 4H4c-1.11 0-2 .89-2 2v12c0 1.11.89 2 2 2h16c1.11 0 2-.89 2-2V8c0-1.11-.89-2-2-2h-8l-2-2z" />
              </motion.svg>
            )}
            {phase === "opening" && (
              <motion.svg
                key="open-folder"
                initial={{ rotateX: 0 }}
                animate={{ rotateX: -20 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M10 4H4c-1.11 0-2 .89-2 2v12c0 1.11.89 2 2 2h16c1.11 0 2-.89 2-2V8c0-1.11-.89-2-2-2h-8l-2-2z" />
              </motion.svg>
            )}
            {phase === "launching" && (
              <motion.svg
                key="arrow"
                initial={{ y: 0, scale: 0.8 }}
                animate={{ y: -40, scale: 1.2 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M7.41 8.59L12 13.17l4.59-4.58L18 10l-6 6-6-6 1.41-1.41z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 8. Cloud Upload Reverse Raindrops
export function CloudUploadReverseButton({ label = "Upload", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "uploading" | "complete">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("uploading");
    setTimeout(() => {
      setPhase("complete");
      onComplete?.();
      setTimeout(() => {
        setPhase("idle");
        setIsAnimating(false);
      }, 800);
    }, 1000);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence>
            <motion.svg
              key="cloud"
              animate={{ scale: phase === "complete" ? [1, 1.1, 1] : 1 }}
              className="w-8 h-8"
              fill="currentColor"
              viewBox="0 0 24 24"
            >
              <path d="M19.35 10.04C18.67 6.59 15.64 4 12 4 9.11 4 6.6 5.64 5.35 8.04 2.34 8.36 0 10.91 0 14c0 3.31 2.69 6 6 6h13c2.76 0 5-2.24 5-5 0-2.64-2.05-4.78-4.65-4.96zM14 13v4h-4v-4H7l5-5 5 5h-3z" />
            </motion.svg>
            {phase === "uploading" && (
              <motion.div
                key="drops"
                className="absolute bottom-0"
                initial={{ y: 10 }}
                animate={{ y: -20 }}
              >
                {[0, 1, 2].map((i) => (
                  <motion.div
                    key={i}
                    className="w-1 h-3 bg-blue-400 rounded-full absolute"
                    style={{ left: `${i * 4}px` }}
                    animate={{ y: [-10, -30] }}
                    transition={{ duration: 0.6, delay: i * 0.1, repeat: Infinity }}
                  />
                ))}
              </motion.div>
            )}
            {phase === "complete" && (
              <motion.svg
                key="check"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                className="absolute w-6 h-6 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 9. Heart Inflate → Confetti Pop
export function HeartConfettiPopButton({ label = "Like", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "inflating" | "popping">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("inflating");
    setTimeout(() => setPhase("popping"), 400);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1200);
  };

  const particles = Array.from({ length: 8 }, (_, i) => ({
    id: i,
    angle: (i * 360) / 8,
  }));

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence>
            {phase === "idle" && (
              <motion.svg
                key="heart"
                initial={{ scale: 1 }}
                animate={{ scale: 1 }}
                className="w-8 h-8 text-red-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
              </motion.svg>
            )}
            {phase === "inflating" && (
              <motion.svg
                key="inflating"
                initial={{ scale: 1 }}
                animate={{ scale: [1, 1.4, 1.2] }}
                className="w-8 h-8 text-red-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
              </motion.svg>
            )}
            {phase === "popping" && (
              <>
                {particles.map((p) => {
                  const rad = (p.angle * Math.PI) / 180;
                  return (
                    <motion.div
                      key={p.id}
                      className="absolute w-2 h-2 bg-red-400 rounded-full"
                      initial={{ x: 0, y: 0, scale: 1, opacity: 1 }}
                      animate={{
                        x: Math.cos(rad) * 40,
                        y: Math.sin(rad) * 40,
                        scale: 0,
                        opacity: 0,
                      }}
                      transition={{ duration: 0.6 }}
                    />
                  );
                })}
                <motion.svg
                  initial={{ scale: 0 }}
                  animate={{ scale: 1 }}
                  className="w-8 h-8 text-red-400"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z" />
                </motion.svg>
              </>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 10. Star Spin → Supernova
export function StarSupernovaButton({ label = "Star", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "spinning" | "burst">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("spinning");
    setTimeout(() => setPhase("burst"), 600);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1400);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <motion.svg
            className="w-8 h-8 text-yellow-400"
            fill="currentColor"
            viewBox="0 0 24 24"
            animate={{
              rotate: phase === "spinning" ? 360 : 0,
              scale: phase === "burst" ? [1, 1.5, 1.2] : 1,
            }}
            transition={{ duration: 0.6, repeat: phase === "spinning" ? Infinity : 0 }}
          >
            <path d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z" />
          </motion.svg>
          {phase === "burst" && (
            <motion.div
              className="absolute inset-0"
              initial={{ scale: 0, opacity: 0.8 }}
              animate={{ scale: 2, opacity: 0 }}
              style={{
                background: "radial-gradient(circle, rgba(255,215,0,0.8) 0%, transparent 70%)",
              }}
            />
          )}
        </div>
      </motion.button>
    </div>
  );
}

// 11. Note → Equalizer Bars
export function MusicEqualizerButton({ label = "Play", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "equalizing" | "note">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("equalizing");
    setTimeout(() => {
      setPhase("note");
      onComplete?.();
      setTimeout(() => {
        setPhase("idle");
        setIsAnimating(false);
      }, 600);
    }, 1500);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-16 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="note"
                initial={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M12 3v10.55c-.59-.34-1.27-.55-2-.55-2.21 0-4 1.79-4 4s1.79 4 4 4 4-1.79 4-4V7h4V3h-6z" />
              </motion.svg>
            )}
            {phase === "equalizing" && (
              <motion.div
                key="bars"
                className="flex items-end gap-1"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                exit={{ opacity: 0 }}
              >
                {[0, 1, 2, 3].map((i) => (
                  <motion.div
                    key={i}
                    className="w-2 bg-emerald-400"
                    animate={{ height: [8, 20, 12, 24, 8] }}
                    transition={{ duration: 0.8, delay: i * 0.1, repeat: Infinity }}
                  />
                ))}
              </motion.div>
            )}
            {phase === "note" && (
              <motion.svg
                key="note-return"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M12 3v10.55c-.59-.34-1.27-.55-2-.55-2.21 0-4 1.79-4 4s1.79 4 4 4 4-1.79 4-4V7h4V3h-6z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 12. Cart → Checkout Arrow
export function CartCheckoutArrowButton({ label = "Checkout", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "shaking" | "arrow">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("shaking");
    setTimeout(() => setPhase("arrow"), 500);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1200);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="cart"
                animate={{ x: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M7 18c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96 0 1.1.9 2 2 2h12v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12L8.1 13h7.45c.75 0 1.41-.41 1.75-1.03L21.7 4H5.21l-.94-2H1zm16 16c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z" />
              </motion.svg>
            )}
            {phase === "shaking" && (
              <motion.svg
                key="shaking"
                animate={{ x: [0, -4, 4, -4, 4, 0] }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M7 18c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96 0 1.1.9 2 2 2h12v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12L8.1 13h7.45c.75 0 1.41-.41 1.75-1.03L21.7 4H5.21l-.94-2H1zm16 16c-1.1 0-2 .9-2 2s.9 2 2 2 2-.9 2-2-.9-2-2-2z" />
              </motion.svg>
            )}
            {phase === "arrow" && (
              <motion.svg
                key="arrow"
                initial={{ x: 0 }}
                animate={{ x: 40 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M12 4l-1.41 1.41L16.17 11H4v2h12.17l-5.58 5.59L12 20l8-8z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 13. Wand → Sparkle Swirl
export function MagicWandSparkleButton({ label = "Magic", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "sparkling" | "check">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("sparkling");
    setTimeout(() => {
      setPhase("check");
      onComplete?.();
      setTimeout(() => {
        setPhase("idle");
        setIsAnimating(false);
      }, 800);
    }, 1200);
  };

  const sparkles = Array.from({ length: 6 }, (_, i) => ({
    id: i,
    angle: (i * 360) / 6,
  }));

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence>
            {phase === "idle" && (
              <motion.svg
                key="wand"
                animate={{ rotate: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M7.5 5.6L10 7 8.6 4.5 10 2 7.5 3.4 5 2l1.4 2.5L5 7l2.5-1.4zm12 9.8L17 14l1.4 2.5L17 19l2.5-1.4L22 19l-1.4-2.5L22 14l-2.5 1.4zM22 2l-2.5 1.4L17 2l1.4 2.5L17 7l2.5-1.4L22 7l-1.4-2.5L22 2zM11.5 8.5l-1-1-1 1-1-1 1 1-1 1 1-1 1 1 1-1zm-1 7l-1-1-1 1-1-1 1 1-1 1 1-1 1 1 1-1zM9 13l-1-1-1 1-1-1 1 1-1 1 1-1 1 1 1-1zm8 0l-1-1-1 1-1-1 1 1-1 1 1-1 1 1 1-1z" />
              </motion.svg>
            )}
            {phase === "sparkling" && (
              <>
                {sparkles.map((s) => {
                  const rad = (s.angle * Math.PI) / 180;
                  return (
                    <motion.div
                      key={s.id}
                      className="absolute w-1.5 h-1.5 bg-yellow-400 rounded-full"
                      initial={{ x: 0, y: 0, scale: 0 }}
                      animate={{
                        x: Math.cos(rad) * 30,
                        y: Math.sin(rad) * 30,
                        scale: [0, 1, 0],
                      }}
                      transition={{ duration: 1, delay: s.id * 0.1 }}
                    />
                  );
                })}
              </>
            )}
            {phase === "check" && (
              <motion.svg
                key="check"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 14. Portal Button
export function PortalMorphButton({ label = "Portal", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "opening" | "morphing">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("opening");
    setTimeout(() => setPhase("morphing"), 600);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1400);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.div
                key="ring"
                initial={{ scale: 1, opacity: 1 }}
                exit={{ scale: 0.8, opacity: 0 }}
                className="w-12 h-12 rounded-full border-4 border-purple-400"
              />
            )}
            {phase === "opening" && (
              <motion.div
                key="portal"
                initial={{ scale: 0.8 }}
                animate={{ scale: [0.8, 1.3, 1] }}
                className="w-12 h-12 rounded-full border-4 border-purple-400"
                style={{
                  boxShadow: "0 0 20px rgba(168, 85, 247, 0.6)",
                }}
              />
            )}
            {phase === "morphing" && (
              <motion.svg
                key="check"
                initial={{ scale: 0, y: 20 }}
                animate={{ scale: 1, y: 0 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 15. Trash Can Vacuum
export function TrashVacuumButton({ label = "Delete", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "opening" | "sucking" | "closed">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("opening");
    setTimeout(() => setPhase("sucking"), 300);
    setTimeout(() => setPhase("closed"), 800);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1200);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence>
            <motion.svg
              key="trash"
              className="w-8 h-8"
              fill="currentColor"
              viewBox="0 0 24 24"
              animate={{
                rotateX: phase === "opening" ? -20 : 0,
              }}
            >
              <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z" />
            </motion.svg>
            {phase === "sucking" && (
              <motion.div
                key="doc"
                className="absolute -top-4 w-3 h-4 bg-slate-300 rounded-sm"
                animate={{ y: [0, 20], scale: [1, 0.5], opacity: [1, 0] }}
                transition={{ duration: 0.5 }}
              />
            )}
            {phase === "closed" && (
              <motion.div
                key="shockwave"
                className="absolute inset-0 rounded-full border-2 border-red-400"
                initial={{ scale: 0.5, opacity: 0.8 }}
                animate={{ scale: 1.5, opacity: 0 }}
                transition={{ duration: 0.3 }}
              />
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 16. Lock → Unlock → Slide
export function LockUnlockSlideButton({ label = "Unlock", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "unlocking" | "unlocked">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("unlocking");
    setTimeout(() => setPhase("unlocked"), 600);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 2000);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-hidden rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
        animate={{
          backgroundColor: phase === "unlocked" ? "rgba(34, 197, 94, 0.2)" : undefined,
        }}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="lock"
                initial={{ opacity: 1 }}
                exit={{ opacity: 0, x: -20 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M18 8h-1V6c0-2.76-2.24-5-5-5S7 3.24 7 6v2H6c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V10c0-1.1-.9-2-2-2zm-6 9c-1.1 0-2-.9-2-2s.9-2 2-2 2 .9 2 2-.9 2-2 2zm3.1-9H8.9V6c0-1.71 1.39-3.1 3.1-3.1 1.71 0 3.1 1.39 3.1 3.1v2z" />
              </motion.svg>
            )}
            {phase === "unlocking" && (
              <motion.svg
                key="unlocking"
                initial={{ y: 0 }}
                animate={{ y: -8, rotate: 15 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M12 17c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2zm6-9h-1V6c0-2.76-2.24-5-5-5S7 3.24 7 6v2H6c-1.1 0-2 .9-2 2v10c0 1.1.9 2 2 2h12c1.1 0 2-.9 2-2V10c0-1.1-.9-2-2-2zM8.9 6c0-1.71 1.39-3.1 3.1-3.1 1.71 0 3.1 1.39 3.1 3.1v2H8.9V6z" />
              </motion.svg>
            )}
            {phase === "unlocked" && (
              <motion.div
                key="unlocked"
                initial={{ opacity: 0, x: 20 }}
                animate={{ opacity: 1, x: 0 }}
                className="text-emerald-400 text-sm font-bold"
              >
                Welcome
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 17. Camera → Photo Pop
export function CameraPhotoPopButton({ label = "Capture", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "capturing" | "photo">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("capturing");
    setTimeout(() => setPhase("photo"), 400);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1500);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence>
            <motion.svg
              key="camera"
              animate={{
                scale: phase === "capturing" ? [1, 0.95, 1] : 1,
                x: phase === "capturing" ? [0, -2, 0] : 0,
              }}
              className="w-8 h-8"
              fill="currentColor"
              viewBox="0 0 24 24"
            >
              <path d="M12 12m-3.2 0a3.2 3.2 0 1 1 6.4 0a3.2 3.2 0 1 1-6.4 0" />
              <path d="M9 2L7.17 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2h-3.17L15 2H9zm3 15c-2.76 0-5-2.24-5-5s2.24-5 5-5 5 2.24 5 5-2.24 5-5 5z" />
            </motion.svg>
            {phase === "capturing" && (
              <motion.div
                key="flash"
                className="absolute inset-0 bg-white rounded-full"
                initial={{ scale: 0, opacity: 0.8 }}
                animate={{ scale: 2, opacity: 0 }}
                transition={{ duration: 0.2 }}
              />
            )}
            {phase === "photo" && (
              <motion.div
                key="photo"
                initial={{ x: 20, y: 0, scale: 0, rotate: -10 }}
                animate={{ x: 30, y: -10, scale: 1, rotate: 5 }}
                className="absolute w-6 h-8 bg-slate-200 rounded-sm border-2 border-slate-400"
              />
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 18. Multi-step Envelope Sequence
export function EnvelopeMultiMorphButton({ label = "Send", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "open" | "paper" | "plane" | "sent">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("open");
    setTimeout(() => setPhase("paper"), 400);
    setTimeout(() => setPhase("plane"), 800);
    setTimeout(() => setPhase("sent"), 1200);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 2000);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="envelope"
                initial={{ opacity: 1 }}
                exit={{ opacity: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
              </motion.svg>
            )}
            {phase === "open" && (
              <motion.svg
                key="open"
                initial={{ rotateX: 0 }}
                animate={{ rotateX: -45 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
              </motion.svg>
            )}
            {phase === "paper" && (
              <motion.div
                key="paper"
                initial={{ scale: 0, y: 0 }}
                animate={{ scale: 1, y: -10 }}
                className="w-6 h-8 bg-slate-200 rounded-sm"
              />
            )}
            {phase === "plane" && (
              <motion.svg
                key="plane"
                initial={{ x: 0, y: 0, rotate: 0 }}
                animate={{ x: 60, y: -30, rotate: 15 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z" />
              </motion.svg>
            )}
            {phase === "sent" && (
              <motion.div
                key="sent"
                initial={{ scale: 0 }}
                animate={{ scale: 1 }}
                className="flex flex-col items-center gap-1"
              >
                <svg className="w-6 h-6 text-emerald-400" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
                </svg>
                <span className="text-xs text-emerald-400">Sent!</span>
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

// 19. 3D Flip Icon Card
export function FlipIconCardButton({ label = "Flip", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "flipping" | "flipped">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("flipping");
    setTimeout(() => setPhase("flipped"), 600);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 2000);
  };

  return (
    <div className="flex h-full w-full items-center justify-center p-8" style={{ perspective: "1000px" }}>
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12" style={{ transformStyle: "preserve-3d" }}>
          <motion.div
            animate={{
              rotateY: phase === "flipping" ? 180 : phase === "flipped" ? 180 : 0,
            }}
            transition={{ duration: 0.6 }}
            className="relative w-8 h-8"
            style={{ transformStyle: "preserve-3d" }}
          >
            <div className="absolute inset-0" style={{ backfaceVisibility: "hidden" }}>
              <svg className="w-8 h-8" fill="currentColor" viewBox="0 0 24 24">
                <path d="M20 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 4l-8 5-8-5V6l8 5 8-5v2z" />
              </svg>
            </div>
            <div className="absolute inset-0" style={{ backfaceVisibility: "hidden", transform: "rotateY(180deg)" }}>
              <svg className="w-8 h-8 text-emerald-400" fill="currentColor" viewBox="0 0 24 24">
                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z" />
              </svg>
            </div>
          </motion.div>
        </div>
      </motion.button>
    </div>
  );
}

// 20. Particle Transformation
export function ParticleTransformButton({ label = "Transform", onComplete }: IconStoryButtonProps) {
  const [phase, setPhase] = useState<"idle" | "dissolving" | "forming">("idle");
  const [isAnimating, setIsAnimating] = useState(false);

  const handleClick = () => {
    if (isAnimating) return;
    setIsAnimating(true);
    setPhase("dissolving");
    setTimeout(() => setPhase("forming"), 600);
    setTimeout(() => {
      onComplete?.();
      setPhase("idle");
      setIsAnimating(false);
    }, 1500);
  };

  const particles = Array.from({ length: 12 }, (_, i) => ({
    id: i,
    angle: (i * 360) / 12,
  }));

  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className="relative overflow-visible rounded-xl border border-slate-700 bg-gradient-to-br from-slate-800 to-slate-900 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg"
        onClick={handleClick}
        whileTap={{ scale: 0.98 }}
        disabled={isAnimating}
      >
        <div className="relative flex items-center justify-center w-12 h-12">
          <AnimatePresence mode="wait">
            {phase === "idle" && (
              <motion.svg
                key="arrow"
                initial={{ opacity: 1, scale: 1 }}
                exit={{ opacity: 0, scale: 0 }}
                className="w-8 h-8"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M12 4l-1.41 1.41L16.17 11H4v2h12.17l-5.58 5.59L12 20l8-8z" />
              </motion.svg>
            )}
            {phase === "dissolving" && (
              <>
                {particles.map((p) => {
                  const rad = (p.angle * Math.PI) / 180;
                  return (
                    <motion.div
                      key={p.id}
                      className="absolute w-1.5 h-1.5 bg-slate-300 rounded-full"
                      initial={{ x: 0, y: 0, scale: 1, opacity: 1 }}
                      animate={{
                        x: Math.cos(rad) * 30,
                        y: Math.sin(rad) * 30,
                        scale: 0,
                        opacity: 0,
                      }}
                      transition={{ duration: 0.6 }}
                    />
                  );
                })}
              </>
            )}
            {phase === "forming" && (
              <motion.svg
                key="check"
                initial={{ scale: 0, opacity: 0 }}
                animate={{ scale: 1, opacity: 1 }}
                className="w-8 h-8 text-emerald-400"
                fill="currentColor"
                viewBox="0 0 24 24"
              >
                <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
              </motion.svg>
            )}
          </AnimatePresence>
        </div>
      </motion.button>
    </div>
  );
}

