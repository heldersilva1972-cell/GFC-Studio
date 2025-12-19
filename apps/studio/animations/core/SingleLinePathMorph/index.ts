import SingleLinePathMorph from "./SingleLinePathMorph";
import { singleLinePathMorphCode } from "./code";
import type { AnimationConfig } from "@/app/animations/core/types";

export const singleLinePathMorphConfig: AnimationConfig = {
  id: "singleLinePathMorph",
  name: "Single-Line Path Morph",
  category: "Advanced",
  complexity: 3,
  previewSize: "lg",
  component: SingleLinePathMorph,
  code: singleLinePathMorphCode,
};

export { SingleLinePathMorph };
export default singleLinePathMorphConfig;

