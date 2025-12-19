import FireIceDualMorph from "./FireIceDualMorph";
import { fireIceDualMorphCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const fireIceDualMorphConfig: AnimationConfig = {
  id: "fireIceDualMorph",
  name: "Fire & Ice Dual Morph",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: FireIceDualMorph,
  code: fireIceDualMorphCode,
};

export { FireIceDualMorph };
export default fireIceDualMorphConfig;

