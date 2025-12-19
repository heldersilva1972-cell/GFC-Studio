import { InkCalligraphyStroke } from "../InkCalligraphyStroke";
import type { AnimationConfig } from "@/app/animations/core/types";

const inkCalligraphyStrokeCode = `"use client";
import { motion } from "framer-motion";
// Ink Calligraphy Stroke animation code`;

export const inkCalligraphyStrokeConfig: AnimationConfig = {
  id: "ink-calligraphy-stroke",
  name: "Calligraphy Stroke",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: InkCalligraphyStroke,
  code: inkCalligraphyStrokeCode,
};

export { InkCalligraphyStroke };
export default inkCalligraphyStrokeConfig;

