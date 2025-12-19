import { GlitchScrambleDecode } from "../GlitchScrambleDecode";
import type { AnimationConfig } from "@/app/animations/core/types";

const glitchScrambleDecodeCode = `"use client";
import { motion } from "framer-motion";
// Glitch Scramble Decode animation code`;

export const glitchScrambleDecodeConfig: AnimationConfig = {
  id: "glitch-scramble-decode",
  name: "Glitch Decode",
  category: "Text FX",
  complexity: 3,
  previewSize: "md",
  component: GlitchScrambleDecode,
  code: glitchScrambleDecodeCode,
};

export { GlitchScrambleDecode };
export default glitchScrambleDecodeConfig;

