import CompressionMorph from "./CompressionMorph";
import { compressionMorphCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const compressionMorphConfig: AnimationConfig = {
  id: "compressionMorph",
  name: "Compression Morph",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: CompressionMorph,
  code: compressionMorphCode,
};

export { CompressionMorph };
export default compressionMorphConfig;

