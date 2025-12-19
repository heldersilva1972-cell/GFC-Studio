import type { AnimationConfig } from "./types";

// Import all flag animation components
import DualFlagRibbonWeave from "@/components/animations/flags/DualFlagRibbonWeave";
import StarToShieldMorph from "@/components/animations/flags/StarToShieldMorph";
import DualFlagWindWaveSync from "@/components/animations/flags/DualFlagWindWaveSync";
import SplitScreenFlagMerge from "@/components/animations/flags/SplitScreenFlagMerge";
import DualFlagParticleBurstReveal from "@/components/animations/flags/DualFlagParticleBurstReveal";
import DualFlagHeartSpiral from "@/components/animations/flags/DualFlagHeartSpiral";
import CrossedFlagpoles3D from "@/components/animations/flags/CrossedFlagpoles3D";
import FlipCardFlagTransition from "@/components/animations/flags/FlipCardFlagTransition";
import SplitStripesTransformFlag from "@/components/animations/flags/SplitStripesTransformFlag";
import FireworkIgniteFlagReveal from "@/components/animations/flags/FireworkIgniteFlagReveal";
import DualFlagHolidayWave from "@/components/animations/flags/DualFlagHolidayWave";
import AmericanFlagWebGL from "../components/AmericanFlagWebGL";
import AmericanFlagAdvancedAnimation from "@/components/animations/AmericanFlagAdvancedAnimation";
import PortugueseFlagRealisticWave from "@/components/animations/PortugueseFlagRealisticWave";
import React from "react";

// Placeholder code for flag animations
const defaultFlagCode = `"use client";

import React from "react";
import { motion } from "framer-motion";

// Flag animation component code here`;

// Wrapper components for holiday variants
const NewYearFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "new_year", ...props });
const USIndependenceFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "us_independence", ...props });
const PortugalDayFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "pt_dia_de_portugal", ...props });
const ThanksgivingFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "thanksgiving", ...props });
const MemorialDayFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "memorial_day", ...props });
const ChristmasFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "christmas", ...props });
const EasterFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "easter", ...props });
const LaborDayFlag = (props: any) => React.createElement(DualFlagHolidayWave, { holidayKey: "labor_day", ...props });

export const flagsAnimations: AnimationConfig[] = [
  // Core dual-flag animations
  {
    id: "flags_dual_ribbon_weave",
    name: "Dual-Flag Ribbon Weave",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: DualFlagRibbonWeave,
    code: defaultFlagCode,
  },
  {
    id: "flags_star_to_shield_morph",
    name: "Star-to-Shield Morph",
    category: "flags",
    complexity: 3,
    previewSize: "md",
    component: StarToShieldMorph,
    code: defaultFlagCode,
  },
  {
    id: "flags_wind_wave_sync",
    name: "Wind-Wave Sync",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: DualFlagWindWaveSync,
    code: defaultFlagCode,
  },
  {
    id: "flags_split_screen_merge",
    name: "Split-Screen Merge",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: SplitScreenFlagMerge,
    code: defaultFlagCode,
  },
  {
    id: "flags_particle_burst_reveal",
    name: "Particle Burst Flag Reveal",
    category: "flags",
    complexity: 3,
    previewSize: "md",
    component: DualFlagParticleBurstReveal,
    code: defaultFlagCode,
  },
  {
    id: "flags_heart_dual_flag",
    name: "Heart-Shape Dual Flag",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: DualFlagHeartSpiral,
    code: defaultFlagCode,
  },
  {
    id: "flags_crossed_flagpoles_3d",
    name: "3D Crossing Flagpoles",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: CrossedFlagpoles3D,
    code: defaultFlagCode,
  },
  {
    id: "flags_flip_card_transition",
    name: "Flip-Card Flag Transition",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: FlipCardFlagTransition,
    code: defaultFlagCode,
  },
  {
    id: "flags_split_stripes_transform",
    name: "Split Stripes Transform",
    category: "flags",
    complexity: 3,
    previewSize: "md",
    component: SplitStripesTransformFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_firework_ignite_reveal",
    name: "Fireworks Ignite Reveal",
    category: "flags",
    complexity: 3,
    previewSize: "md",
    component: FireworkIgniteFlagReveal,
    code: defaultFlagCode,
  },
  // Holiday variants
  {
    id: "flags_holiday_new_year",
    name: "New Year's Dual Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: NewYearFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_holiday_us_independence",
    name: "US Independence Day Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: USIndependenceFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_holiday_pt_dia_de_portugal",
    name: "Portugal Day Dual Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: PortugalDayFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_holiday_thanksgiving",
    name: "Thanksgiving Dual Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: ThanksgivingFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_holiday_memorial_day",
    name: "Memorial Day Dual Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: MemorialDayFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_holiday_christmas",
    name: "Christmas Dual Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: ChristmasFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_holiday_easter",
    name: "Easter Dual Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: EasterFlag,
    code: defaultFlagCode,
  },
  {
    id: "flags_holiday_labor_day",
    name: "Labor Day Dual Flags",
    category: "flags",
    complexity: 2,
    previewSize: "md",
    component: LaborDayFlag,
    code: defaultFlagCode,
  },
  {
    id: "gfc-american-flag-webgl",
    name: "American Flag – WebGL Wave",
    category: "flags",
    complexity: 2,
    previewSize: "lg",
    component: AmericanFlagWebGL,
    code: "",
  },
  {
    id: "american-flag-advanced",
    name: "American Flag – Advanced Wave",
    category: "flags",
    complexity: 3,
    previewSize: "lg",
    component: AmericanFlagAdvancedAnimation,
    code: defaultFlagCode,
  },
  {
    id: "portuguese-flag-realistic-wave",
    name: "Portuguese Flag – Realistic Wave",
    category: "flags",
    complexity: 3,
    previewSize: "lg",
    component: PortugueseFlagRealisticWave,
    code: defaultFlagCode,
  },
];

