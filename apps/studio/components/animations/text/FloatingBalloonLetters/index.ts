import { FloatingBalloonLetters } from "../FloatingBalloonLetters";
import type { AnimationConfig } from "@/app/animations/core/types";

const floatingBalloonLettersCode = `"use client";
import { motion } from "framer-motion";
// Floating Balloon Letters animation code`;

export const floatingBalloonLettersConfig: AnimationConfig = {
  id: "floating-balloon-letters",
  name: "Balloon Float",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: FloatingBalloonLetters,
  code: floatingBalloonLettersCode,
};

export { FloatingBalloonLetters };
export default floatingBalloonLettersConfig;

