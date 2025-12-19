import { OrigamiFoldText } from "../OrigamiFoldText";
import type { AnimationConfig } from "@/app/animations/core/types";

const origamiFoldTextCode = `"use client";
import { motion } from "framer-motion";
// Origami Fold Text animation code`;

export const origamiFoldTextConfig: AnimationConfig = {
  id: "origami-fold-text",
  name: "Origami Fold",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: OrigamiFoldText,
  code: origamiFoldTextCode,
};

export { OrigamiFoldText };
export default origamiFoldTextConfig;

