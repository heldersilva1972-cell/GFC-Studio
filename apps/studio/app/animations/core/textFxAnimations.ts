import type { AnimationConfig } from "./types";

// Import all 20 Text FX components
import { NeonPulseGlow } from "@/components/animations/text/NeonPulseGlow";
import { DigitalFlipClock } from "@/components/animations/text/DigitalFlipClock";
import { TypewriterInkBleed } from "@/components/animations/text/TypewriterInkBleed";
import { LiquidWaveText } from "@/components/animations/text/LiquidWaveText";
import { ParticleBurstReveal } from "@/components/animations/text/ParticleBurstReveal";
import { FloatingBalloonLetters } from "@/components/animations/text/FloatingBalloonLetters";
import { MetallicShineSweep } from "@/components/animations/text/MetallicShineSweep";
import { ExplodeReform } from "@/components/animations/text/ExplodeReform";
import { FlameTextMorph } from "@/components/animations/text/FlameTextMorph";
import { LaserBeamWriting } from "@/components/animations/text/LaserBeamWriting";
import { GlitchScrambleDecode } from "@/components/animations/text/GlitchScrambleDecode";
import { OrigamiFoldText } from "@/components/animations/text/OrigamiFoldText";
import { RotationCarousel3D } from "@/components/animations/text/RotationCarousel3D";
import { IceFreezeCrack } from "@/components/animations/text/IceFreezeCrack";
import { BubblePopReveal } from "@/components/animations/text/BubblePopReveal";
import { WarpSpeedStretch } from "@/components/animations/text/WarpSpeedStretch";
import { RibbonTextUnfold } from "@/components/animations/text/RibbonTextUnfold";
import { ChromaticRGBSplit } from "@/components/animations/text/ChromaticRGBSplit";
import { ElectricArcStitching } from "@/components/animations/text/ElectricArcStitching";
import { InkCalligraphyStroke } from "@/components/animations/text/InkCalligraphyStroke";

export const TEXT_FX_CATEGORY_ID = "text-fx";

// Placeholder code for Text FX animations (each will have its own code)
const defaultTextFxCode = `"use client";

import React from "react";
import { motion } from "framer-motion";
import { AnimatedTextContainer, AnimatedTextProps } from "./AnimatedTextBase";

// Text FX animation component code here`;

export const textFxAnimations: AnimationConfig[] = [
  {
    id: "neon-pulse-glow",
    name: "Neon Pulse Glow",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: NeonPulseGlow,
    code: defaultTextFxCode,
  },
  {
    id: "digital-flip-clock",
    name: "Digital Flip Clock",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: DigitalFlipClock,
    code: defaultTextFxCode,
  },
  {
    id: "typewriter-ink-bleed",
    name: "Typewriter Ink Bleed",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: TypewriterInkBleed,
    code: defaultTextFxCode,
  },
  {
    id: "liquid-wave-text",
    name: "Liquid Wave",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: LiquidWaveText,
    code: defaultTextFxCode,
  },
  {
    id: "particle-burst-reveal",
    name: "Particle Burst",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: ParticleBurstReveal,
    code: defaultTextFxCode,
  },
  {
    id: "floating-balloon-letters",
    name: "Balloon Float",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: FloatingBalloonLetters,
    code: defaultTextFxCode,
  },
  {
    id: "metallic-shine-sweep",
    name: "Metallic Shine",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: MetallicShineSweep,
    code: defaultTextFxCode,
  },
  {
    id: "explode-reform",
    name: "Explode & Reform",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: ExplodeReform,
    code: defaultTextFxCode,
  },
  {
    id: "flame-text-morph",
    name: "Flame Flicker",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: FlameTextMorph,
    code: defaultTextFxCode,
  },
  {
    id: "laser-beam-writing",
    name: "Laser Write",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: LaserBeamWriting,
    code: defaultTextFxCode,
  },
  {
    id: "glitch-scramble-decode",
    name: "Glitch Decode",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: GlitchScrambleDecode,
    code: defaultTextFxCode,
  },
  {
    id: "origami-fold-text",
    name: "Origami Fold",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: OrigamiFoldText,
    code: defaultTextFxCode,
  },
  {
    id: "rotation-carousel-3d",
    name: "3D Carousel Text",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: RotationCarousel3D,
    code: defaultTextFxCode,
  },
  {
    id: "ice-freeze-crack",
    name: "Ice Crack Reveal",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: IceFreezeCrack,
    code: defaultTextFxCode,
  },
  {
    id: "bubble-pop-reveal",
    name: "Bubble Pop",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: BubblePopReveal,
    code: defaultTextFxCode,
  },
  {
    id: "warp-speed-stretch",
    name: "Warp Speed",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: WarpSpeedStretch,
    code: defaultTextFxCode,
  },
  {
    id: "ribbon-text-unfold",
    name: "Ribbon Unfold",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: RibbonTextUnfold,
    code: defaultTextFxCode,
  },
  {
    id: "chromatic-rgb-split",
    name: "Chromatic Split",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: ChromaticRGBSplit,
    code: defaultTextFxCode,
  },
  {
    id: "electric-arc-stitching",
    name: "Electric Stitch",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: ElectricArcStitching,
    code: defaultTextFxCode,
  },
  {
    id: "ink-calligraphy-stroke",
    name: "Calligraphy Stroke",
    category: TEXT_FX_CATEGORY_ID,
    complexity: 2,
    previewSize: "md",
    component: InkCalligraphyStroke,
    code: defaultTextFxCode,
  },
];

