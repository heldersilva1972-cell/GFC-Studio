import { RibbonTextUnfold } from "../RibbonTextUnfold";
import type { AnimationConfig } from "@/app/animations/core/types";

const ribbonTextUnfoldCode = `"use client";
import { motion } from "framer-motion";
// Ribbon Text Unfold animation code`;

export const ribbonTextUnfoldConfig: AnimationConfig = {
  id: "ribbon-text-unfold",
  name: "Ribbon Unfold",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: RibbonTextUnfold,
  code: ribbonTextUnfoldCode,
};

export { RibbonTextUnfold };
export default ribbonTextUnfoldConfig;

