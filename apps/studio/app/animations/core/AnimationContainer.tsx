"use client";

import { useMemo } from "react";
import { getAnimationById } from "./AnimationRegistry";
import type { AnimationProps } from "./types";

interface AnimationContainerProps {
  animationId: string | null;
  className?: string;
  size?: number;
  speed?: number;
  colors?: string[];
  [key: string]: any;
}

export default function AnimationContainer({
  animationId,
  className = "",
  size,
  speed,
  colors,
  ...props
}: AnimationContainerProps) {
  const animation = useMemo(() => {
    if (!animationId) return null;
    return getAnimationById(animationId);
  }, [animationId]);

  if (!animation) {
    return (
      <div className={`flex items-center justify-center ${className}`}>
        <div className="text-center p-8">
          <p className="text-gray-500 text-lg">No animation selected</p>
          <p className="text-gray-400 text-sm mt-2">
            Select an animation from the list to preview it here
          </p>
        </div>
      </div>
    );
  }

  const AnimationComponent = animation.component;
  const animationProps: AnimationProps = {
    className,
    size: size ?? 200,
    speed: speed ?? 1.0,
    colors: colors ?? ["#3b82f6", "#8b5cf6", "#ec4899"],
    ...props,
  };

  return (
    <div className="flex items-center justify-center w-full h-full">
      <AnimationComponent {...animationProps} />
    </div>
  );
}

