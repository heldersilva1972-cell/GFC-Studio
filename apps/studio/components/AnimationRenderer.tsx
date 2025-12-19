"use client";

import React from "react";

// Animation map for rendering different animation types
const animationMap: Record<string, React.ComponentType<any>> = {};

interface AnimationRendererProps {
  animationId: string;
  [key: string]: any;
}

export const AnimationRenderer: React.FC<AnimationRendererProps> = ({
  animationId,
  ...props
}) => {
  const AnimationComponent = animationMap[animationId];

  if (!AnimationComponent) {
    return (
      <div className="flex items-center justify-center p-8 text-slate-400">
        Animation "{animationId}" not found
      </div>
    );
  }

  return <AnimationComponent {...props} />;
};

export default AnimationRenderer;

