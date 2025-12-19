import LiquidNeonFlux from "./LiquidNeonFlux";
import { liquidNeonFluxCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const liquidNeonFluxConfig: AnimationConfig = {
  id: "liquidNeonFlux",
  name: "Liquid Neon Flux",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: LiquidNeonFlux,
  code: liquidNeonFluxCode,
};

export { LiquidNeonFlux };
export default liquidNeonFluxConfig;

