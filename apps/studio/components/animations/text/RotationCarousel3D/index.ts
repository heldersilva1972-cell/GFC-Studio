import { RotationCarousel3D } from "../RotationCarousel3D";
import type { AnimationConfig } from "@/app/animations/core/types";

const rotationCarousel3DCode = `"use client";
import { motion } from "framer-motion";
// 3D Rotation Carousel animation code`;

export const rotationCarousel3DConfig: AnimationConfig = {
  id: "rotation-carousel-3d",
  name: "3D Carousel Text",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: RotationCarousel3D,
  code: rotationCarousel3DCode,
};

export { RotationCarousel3D };
export default rotationCarousel3DConfig;

