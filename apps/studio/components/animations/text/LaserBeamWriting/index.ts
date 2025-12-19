import { LaserBeamWriting } from "../LaserBeamWriting";
import type { AnimationConfig } from "@/app/animations/core/types";

const laserBeamWritingCode = `"use client";
import { motion } from "framer-motion";
// Laser Beam Writing animation code`;

export const laserBeamWritingConfig: AnimationConfig = {
  id: "laser-beam-writing",
  name: "Laser Write",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: LaserBeamWriting,
  code: laserBeamWritingCode,
};

export { LaserBeamWriting };
export default laserBeamWritingConfig;

