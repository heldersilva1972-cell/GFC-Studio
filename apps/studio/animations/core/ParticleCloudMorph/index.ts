import ParticleCloudMorph from "./ParticleCloudMorph";
import { particleCloudMorphCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const particleCloudMorphConfig: AnimationConfig = {
  id: "particleCloudMorph",
  name: "Particle Cloud Morph",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: ParticleCloudMorph,
  code: particleCloudMorphCode,
};

export { ParticleCloudMorph };
export default particleCloudMorphConfig;

