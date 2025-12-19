import { MetallicShineSweep } from "../MetallicShineSweep";
import type { AnimationConfig } from "@/app/animations/core/types";

const metallicShineSweepCode = `"use client";
import { motion } from "framer-motion";
// Metallic Shine Sweep animation code`;

export const metallicShineSweepConfig: AnimationConfig = {
  id: "metallic-shine-sweep",
  name: "Metallic Shine",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: MetallicShineSweep,
  code: metallicShineSweepCode,
};

export { MetallicShineSweep };
export default metallicShineSweepConfig;

