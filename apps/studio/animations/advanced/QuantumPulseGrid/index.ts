import QuantumPulseGrid from "./QuantumPulseGrid";
import { quantumPulseGridCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const quantumPulseGridConfig: AnimationConfig = {
  id: "quantumPulseGrid",
  name: "Quantum Pulse Grid",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: QuantumPulseGrid,
  code: quantumPulseGridCode,
};

export { QuantumPulseGrid };
export default quantumPulseGridConfig;

