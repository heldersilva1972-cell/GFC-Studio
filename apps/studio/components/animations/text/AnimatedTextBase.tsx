"use client";

import React from "react";

/**
 * Shared props for all text animation components
 */
export interface AnimatedTextProps {
  text?: string;
  subText?: string;
  accentColor?: string;
  className?: string;
  size?: number;
  speed?: number;
  colors?: string[];
  [key: string]: any;
}

/**
 * Base wrapper component for text animations
 * Provides consistent layout and centering
 */
export const AnimatedTextBase: React.FC<{
  children: React.ReactNode;
  className?: string;
}> = ({ children, className = "" }) => {
  return (
    <div
      className={`flex h-full w-full items-center justify-center ${className}`}
    >
      {children}
    </div>
  );
};

/**
 * Container component that ensures text is centered and visible
 */
export function AnimatedTextContainer({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center">
      <div className="px-4 text-center">{children}</div>
    </div>
  );
}

