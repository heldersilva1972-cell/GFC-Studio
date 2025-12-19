import type { AnimationConfig } from "./types";
import { PasswordStrengthMeter } from "@/components/animations/forms/PasswordStrengthMeter";
import { GlassLoginSwitcher } from "@/components/animations/forms/GlassLoginSwitcher";
import { MagicPlayButton } from "@/components/animations/buttons/MagicPlayButton";
import { LiquidDistortionText } from "@/components/animations/text/LiquidDistortionText";
import { NeonBorderCard } from "@/components/animations/cards/NeonBorderCard";
import { ProductConfiguratorCard } from "@/components/animations/cards/ProductConfiguratorCard";
import { HoverInfoRevealCard } from "@/components/animations/cards/HoverInfoRevealCard";
import { DashboardMetricsTiles } from "@/components/animations/dashboards/DashboardMetricsTiles";
import { ImageUploaderGallery } from "@/components/animations/media/ImageUploaderGallery";

const defaultCode = `"use client";

import React from "react";
import { motion } from "framer-motion";

// UI Animation Component
export default function UIAnimation() {
  return (
    <div className="flex h-full w-full items-center justify-center">
      {/* Animation content */}
    </div>
  );
}
`;

export const uiAnimations: AnimationConfig[] = [
  {
    id: "password-strength-meter",
    name: "Password Strength Meter",
    category: "intermediate",
    complexity: 2,
    previewSize: "md",
    component: PasswordStrengthMeter,
    code: defaultCode,
  },
  {
    id: "glass-login-switcher",
    name: "Glass Login Switcher",
    category: "advanced",
    complexity: 3,
    previewSize: "md",
    component: GlassLoginSwitcher,
    code: defaultCode,
  },
  {
    id: "magic-play-button",
    name: "Magic Play Button",
    category: "intermediate",
    complexity: 2,
    previewSize: "md",
    component: MagicPlayButton,
    code: defaultCode,
  },
  {
    id: "liquid-distortion-text",
    name: "Liquid Distortion Text",
    category: "advanced",
    complexity: 3,
    previewSize: "md",
    component: LiquidDistortionText,
    code: defaultCode,
  },
  {
    id: "neon-border-card",
    name: "Neon Border Card",
    category: "intermediate",
    complexity: 2,
    previewSize: "sm",
    component: NeonBorderCard,
    code: defaultCode,
  },
  {
    id: "product-configurator-card",
    name: "Product Configurator Card",
    category: "advanced",
    complexity: 3,
    previewSize: "md",
    component: ProductConfiguratorCard,
    code: defaultCode,
  },
  {
    id: "hover-info-reveal-card",
    name: "Hover Info Reveal Card",
    category: "intermediate",
    complexity: 2,
    previewSize: "sm",
    component: HoverInfoRevealCard,
    code: defaultCode,
  },
  {
    id: "dashboard-metrics-tiles",
    name: "Dashboard Metrics Tiles",
    category: "intermediate",
    complexity: 2,
    previewSize: "md",
    component: DashboardMetricsTiles,
    code: defaultCode,
  },
  {
    id: "image-uploader-gallery",
    name: "Image Uploader Gallery",
    category: "intermediate",
    complexity: 2,
    previewSize: "md",
    component: ImageUploaderGallery,
    code: defaultCode,
  },
];

