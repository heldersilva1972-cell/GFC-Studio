import { NeonPulseGlow } from "../NeonPulseGlow";
import { neonPulseGlowCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const neonPulseGlowConfig: AnimationConfig = {
  id: "neon-pulse-glow",
  name: "Neon Pulse Glow",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: NeonPulseGlow,
  code: neonPulseGlowCode,
};

export { NeonPulseGlow };
export default neonPulseGlowConfig;

