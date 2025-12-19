"use client";

import React from "react";
import PortugueseFlagRealisticWave from "./PortugueseFlagRealisticWave";

const PortugueseFlagRealisticWaveAnimation: React.FC<{
  className?: string;
  size?: number;
  speed?: number;
  [key: string]: any;
}> = ({ className, size, speed, ...props }) => {
  return (
    <div className={`flex h-full w-full items-center justify-center ${className || ""}`}>
      <PortugueseFlagRealisticWave width={520} height={340} />
    </div>
  );
};

export default PortugueseFlagRealisticWaveAnimation;

