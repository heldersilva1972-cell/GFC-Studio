import FractalPulseTunnel from "./FractalPulseTunnel";
import { fractalPulseTunnelCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const fractalPulseTunnelConfig: AnimationConfig = {
  id: "fractalPulseTunnel",
  name: "Fractal Pulse Tunnel",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: FractalPulseTunnel,
  code: fractalPulseTunnelCode,
};

export { FractalPulseTunnel };
export default fractalPulseTunnelConfig;

