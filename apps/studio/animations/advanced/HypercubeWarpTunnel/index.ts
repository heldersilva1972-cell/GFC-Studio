import HypercubeWarpTunnel from "./HypercubeWarpTunnel";
import { hypercubeWarpTunnelCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const hypercubeWarpTunnelConfig: AnimationConfig = {
  id: "hypercubeWarpTunnel",
  name: "Hypercube Warp Tunnel",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: HypercubeWarpTunnel,
  code: hypercubeWarpTunnelCode,
};

export { HypercubeWarpTunnel };
export default hypercubeWarpTunnelConfig;

