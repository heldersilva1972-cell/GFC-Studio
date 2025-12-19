import { TypewriterInkBleed } from "../TypewriterInkBleed";
import type { AnimationConfig } from "@/app/animations/core/types";

const typewriterInkBleedCode = `"use client";
import { motion } from "framer-motion";
// Typewriter Ink Bleed animation code`;

export const typewriterInkBleedConfig: AnimationConfig = {
  id: "typewriter-ink-bleed",
  name: "Typewriter Ink Bleed",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: TypewriterInkBleed,
  code: typewriterInkBleedCode,
};

export { TypewriterInkBleed };
export default typewriterInkBleedConfig;

