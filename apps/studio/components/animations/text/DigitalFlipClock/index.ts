import { DigitalFlipClock } from "../DigitalFlipClock";
import type { AnimationConfig } from "@/app/animations/core/types";

const digitalFlipClockCode = `"use client";
import { motion } from "framer-motion";
// Digital Flip Clock animation code`;

export const digitalFlipClockConfig: AnimationConfig = {
  id: "digital-flip-clock",
  name: "Digital Flip Clock",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: DigitalFlipClock,
  code: digitalFlipClockCode,
};

export { DigitalFlipClock };
export default digitalFlipClockConfig;

