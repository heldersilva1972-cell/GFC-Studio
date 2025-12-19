import { ExplodeReform } from "../ExplodeReform";
import type { AnimationConfig } from "@/app/animations/core/types";

const explodeReformCode = `"use client";
import { motion } from "framer-motion";
// Explode & Reform animation code`;

export const explodeReformConfig: AnimationConfig = {
  id: "explode-reform",
  name: "Explode & Reform",
  category: "Text FX",
  complexity: 3,
  previewSize: "md",
  component: ExplodeReform,
  code: explodeReformCode,
};

export { ExplodeReform };
export default explodeReformConfig;

