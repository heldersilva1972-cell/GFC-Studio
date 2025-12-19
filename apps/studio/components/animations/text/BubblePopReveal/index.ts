import { BubblePopReveal } from "../BubblePopReveal";
import type { AnimationConfig } from "@/app/animations/core/types";

const bubblePopRevealCode = `"use client";
import { motion } from "framer-motion";
// Bubble Pop Reveal animation code`;

export const bubblePopRevealConfig: AnimationConfig = {
  id: "bubble-pop-reveal",
  name: "Bubble Pop",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: BubblePopReveal,
  code: bubblePopRevealCode,
};

export { BubblePopReveal };
export default bubblePopRevealConfig;

