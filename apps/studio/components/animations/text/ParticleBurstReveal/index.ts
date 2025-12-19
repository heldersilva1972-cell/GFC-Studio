import { ParticleBurstReveal } from "../ParticleBurstReveal";
import type { AnimationConfig } from "@/app/animations/core/types";

const particleBurstRevealCode = `"use client";
import { motion } from "framer-motion";
// Particle Burst Reveal animation code`;

export const particleBurstRevealConfig: AnimationConfig = {
  id: "particle-burst-reveal",
  name: "Particle Burst",
  category: "Text FX",
  complexity: 3,
  previewSize: "md",
  component: ParticleBurstReveal,
  code: particleBurstRevealCode,
};

export { ParticleBurstReveal };
export default particleBurstRevealConfig;

