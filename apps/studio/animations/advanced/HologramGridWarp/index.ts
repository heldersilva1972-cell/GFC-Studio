import HologramGridWarp from "./HologramGridWarp";
import { hologramGridWarpCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const hologramGridWarpConfig: AnimationConfig = {
  id: "hologramGridWarp",
  name: "Hologram Grid Warp",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: HologramGridWarp,
  code: hologramGridWarpCode,
};

export { HologramGridWarp };
export default hologramGridWarpConfig;

