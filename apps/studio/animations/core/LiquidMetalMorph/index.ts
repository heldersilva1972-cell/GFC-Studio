import LiquidMetalMorph from "./LiquidMetalMorph";
import { liquidMetalMorphCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const liquidMetalMorphConfig: AnimationConfig = {
  id: "liquidMetalMorph",
  name: "Liquid Metal Morph",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: LiquidMetalMorph,
  code: liquidMetalMorphCode,
};

export { LiquidMetalMorph };
export default liquidMetalMorphConfig;

