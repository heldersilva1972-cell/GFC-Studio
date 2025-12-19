import { FlameTextMorph } from "../FlameTextMorph";
import type { AnimationConfig } from "@/app/animations/core/types";

const flameTextMorphCode = `"use client";
import { motion } from "framer-motion";
// Flame Flicker animation code`;

export const flameTextMorphConfig: AnimationConfig = {
  id: "flame-text-morph",
  name: "Flame Flicker",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: FlameTextMorph,
  code: flameTextMorphCode,
};

export { FlameTextMorph };
export default flameTextMorphConfig;

