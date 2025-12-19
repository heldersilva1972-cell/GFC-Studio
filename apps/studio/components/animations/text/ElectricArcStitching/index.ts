import { ElectricArcStitching } from "../ElectricArcStitching";
import type { AnimationConfig } from "@/app/animations/core/types";

const electricArcStitchingCode = `"use client";
import { motion } from "framer-motion";
// Electric Arc Stitching animation code`;

export const electricArcStitchingConfig: AnimationConfig = {
  id: "electric-arc-stitching",
  name: "Electric Stitch",
  category: "Text FX",
  complexity: 3,
  previewSize: "md",
  component: ElectricArcStitching,
  code: electricArcStitchingCode,
};

export { ElectricArcStitching };
export default electricArcStitchingConfig;

