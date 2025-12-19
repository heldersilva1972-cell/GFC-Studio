import { ChromaticRGBSplit } from "../ChromaticRGBSplit";
import type { AnimationConfig } from "@/app/animations/core/types";

const chromaticRGBSplitCode = `"use client";
import { motion } from "framer-motion";
// Chromatic RGB Split animation code`;

export const chromaticRGBSplitConfig: AnimationConfig = {
  id: "chromatic-rgb-split",
  name: "Chromatic Split",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: ChromaticRGBSplit,
  code: chromaticRGBSplitCode,
};

export { ChromaticRGBSplit };
export default chromaticRGBSplitConfig;

