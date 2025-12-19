import { WarpSpeedStretch } from "../WarpSpeedStretch";
import type { AnimationConfig } from "@/app/animations/core/types";

const warpSpeedStretchCode = `"use client";
import { motion } from "framer-motion";
// Warp Speed Stretch animation code`;

export const warpSpeedStretchConfig: AnimationConfig = {
  id: "warp-speed-stretch",
  name: "Warp Speed",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: WarpSpeedStretch,
  code: warpSpeedStretchCode,
};

export { WarpSpeedStretch };
export default warpSpeedStretchConfig;

