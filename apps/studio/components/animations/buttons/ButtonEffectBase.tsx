"use client";

import React from "react";
import { motion, type HTMLMotionProps } from "framer-motion";

export interface ButtonEffectProps {
  label?: string;
  className?: string;
  children?: React.ReactNode;
  motionProps?: HTMLMotionProps<"button">;
}

/**
 * Base component for all button effect animations
 * Provides consistent layout, styling, and structure
 */
export function ButtonEffectBase({
  label = "Click Me",
  className = "",
  children,
  motionProps = {},
}: ButtonEffectProps) {
  return (
    <div className="flex h-full w-full items-center justify-center p-8">
      <motion.button
        type="button"
        className={`relative overflow-hidden rounded-xl border border-slate-700/50 bg-slate-800/50 px-8 py-4 text-base font-semibold text-slate-100 shadow-lg transition-colors hover:bg-slate-700/50 ${className}`}
        whileHover={{ scale: 1.02 }}
        whileTap={{ scale: 0.98 }}
        {...motionProps}
      >
        {children || <span>{label}</span>}
      </motion.button>
    </div>
  );
}

