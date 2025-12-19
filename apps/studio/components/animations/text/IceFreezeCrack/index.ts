import { IceFreezeCrack } from "../IceFreezeCrack";
import type { AnimationConfig } from "@/app/animations/core/types";

const iceFreezeCrackCode = `"use client";
import { motion } from "framer-motion";
// Ice Freeze Crack animation code`;

export const iceFreezeCrackConfig: AnimationConfig = {
  id: "ice-freeze-crack",
  name: "Ice Crack Reveal",
  category: "Text FX",
  complexity: 2,
  previewSize: "md",
  component: IceFreezeCrack,
  code: iceFreezeCrackCode,
};

export { IceFreezeCrack };
export default iceFreezeCrackConfig;

