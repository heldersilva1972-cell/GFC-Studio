import ParticleSwarmRibbon from "./ParticleSwarmRibbon";
import { particleSwarmRibbonCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const particleSwarmRibbonConfig: AnimationConfig = {
  id: "particleSwarmRibbon",
  name: "Particle Swarm Ribbon",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: ParticleSwarmRibbon,
  code: particleSwarmRibbonCode,
};

export { ParticleSwarmRibbon };
export default particleSwarmRibbonConfig;

