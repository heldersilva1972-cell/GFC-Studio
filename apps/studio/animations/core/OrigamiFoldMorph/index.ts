import OrigamiFoldMorph from "./OrigamiFoldMorph";
import { origamiFoldMorphCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const origamiFoldMorphConfig: AnimationConfig = {
  id: "origamiFoldMorph",
  name: "Origami Fold Morph",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: OrigamiFoldMorph,
  code: origamiFoldMorphCode,
};

export { OrigamiFoldMorph };
export default origamiFoldMorphConfig;

