import { LiquidWaveText } from "../LiquidWaveText";
import type { AnimationConfig } from "@/app/animations/core/types";

const liquidWaveTextCode = `"use client";
import { motion } from "framer-motion";
// Liquid Wave Text animation code`;

export const liquidWaveTextConfig: AnimationConfig = {
  id: "liquid-wave-text",
  name: "Liquid Wave",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: LiquidWaveText,
  code: liquidWaveTextCode,
};

export { LiquidWaveText };
export default liquidWaveTextConfig;

