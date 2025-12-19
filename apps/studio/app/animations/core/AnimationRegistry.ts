import type { AnimationConfig } from "./types";

// Import placeholder animations
import PulseCircle from "./placeholders/PulseCircle";
import SlidingSquare from "./placeholders/SlidingSquare";
import FloatingText from "./placeholders/FloatingText";

// Import existing animations
import { morphingShapeFlowConfig } from "@/animations/advanced/MorphingShapeFlow";
import { hologramGridWarpConfig } from "@/animations/advanced/HologramGridWarp";
import { particleSwarmRibbonConfig } from "@/animations/advanced/ParticleSwarmRibbon";
import { fractalPulseTunnelConfig } from "@/animations/advanced/FractalPulseTunnel";
import { quantumPulseGridConfig } from "@/animations/advanced/QuantumPulseGrid";
import { liquidNeonFluxConfig } from "@/animations/advanced/LiquidNeonFlux";
import { hypercubeWarpTunnelConfig } from "@/animations/advanced/HypercubeWarpTunnel";

// Import new I ❤️ U morphing animations
import { liquidMetalMorphConfig } from "@/animations/core/LiquidMetalMorph";
import { singleLinePathMorphConfig } from "@/animations/core/SingleLinePathMorph";
import { particleCloudMorphConfig } from "@/animations/core/ParticleCloudMorph";
import { origamiFoldMorphConfig } from "@/animations/core/OrigamiFoldMorph";
import { fireIceDualMorphConfig } from "@/animations/core/FireIceDualMorph";
import { compressionMorphConfig } from "@/animations/core/CompressionMorph";

// Import Text FX animations - all 20 with distinct components
import { textFxAnimations } from "./textFxAnimations";

// Import Button animations - all 20 with distinct components
import { buttonAnimations } from "./buttonAnimations";

// Import GFC Website animations
import { gfcWebsiteAnimations } from "./gfcWebsiteAnimations";

// Import UI animations
import { uiAnimations } from "./uiAnimations";

// Import Flag animations
import { flagsAnimations } from "./flagsAnimations";


// Placeholder animation codes
const pulseCircleCode = `"use client";

import { motion } from "framer-motion";

export default function PulseCircle({ size = 200, speed = 1.0, colors = ["#3b82f6"] }) {
  return (
    <div style={{ width: size, height: size }} className="flex items-center justify-center">
      <motion.div
        className="rounded-full"
        style={{
          width: size * 0.6,
          height: size * 0.6,
          backgroundColor: colors[0],
        }}
        animate={{
          scale: [1, 1.2, 1],
          opacity: [1, 0.7, 1],
        }}
        transition={{
          duration: 2 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />
    </div>
  );
}`;

const slidingSquareCode = `"use client";

import { motion } from "framer-motion";

export default function SlidingSquare({ size = 200, speed = 1.0, colors = ["#8b5cf6"] }) {
  const squareSize = size * 0.3;
  const containerSize = size;

  return (
    <div style={{ width: containerSize, height: containerSize }} className="flex items-center justify-center overflow-hidden">
      <motion.div
        style={{
          width: squareSize,
          height: squareSize,
          backgroundColor: colors[0],
        }}
        animate={{
          x: [
            -containerSize / 2 + squareSize / 2,
            containerSize / 2 - squareSize / 2,
            -containerSize / 2 + squareSize / 2,
          ],
        }}
        transition={{
          duration: 3 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />
    </div>
  );
}`;

const floatingTextCode = `"use client";

import { motion } from "framer-motion";

export default function FloatingText({ size = 200, speed = 1.0, colors = ["#ec4899"] }) {
  return (
    <div style={{ width: size, height: size }} className="flex items-center justify-center">
      <motion.div
        style={{
          color: colors[0],
          fontSize: size * 0.15,
          fontWeight: "bold",
        }}
        animate={{
          y: [-10, 10, -10],
          opacity: [0.7, 1, 0.7],
        }}
        transition={{
          duration: 2.5 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      >
        Float
      </motion.div>
    </div>
  );
}`;

// Placeholder animation configs
const pulseCircleConfig: AnimationConfig = {
  id: "pulseCircle",
  name: "Pulse Circle",
  category: "Basic",
  complexity: 1,
  previewSize: "md",
  component: PulseCircle,
  code: pulseCircleCode,
  editor: {
    filePathHint: "app/animations/core/placeholders/PulseCircle.tsx",
    sourceCode: `"use client";

import { motion } from "framer-motion";

export default function PulseCircle({ size = 200, speed = 1.0, colors = ["#3b82f6"] }) {
  return (
    <div style={{ width: size, height: size }} className="flex items-center justify-center">
      <motion.div
        className="rounded-full"
        style={{
          width: size * 0.6,
          height: size * 0.6,
          backgroundColor: colors[0],
        }}
        animate={{
          scale: [1, 1.2, 1],
          opacity: [1, 0.7, 1],
        }}
        transition={{
          duration: 2 / speed,
          repeat: Infinity,
          ease: "easeInOut",
        }}
      />
    </div>
  );
}`,
    notes: "Looks best on dark backgrounds.",
    suggestedUse: "Good for hero sections or featured banners.",
  },
};

const slidingSquareConfig: AnimationConfig = {
  id: "slidingSquare",
  name: "Sliding Square",
  category: "Basic",
  complexity: 1,
  previewSize: "md",
  component: SlidingSquare,
  code: slidingSquareCode,
};

const floatingTextConfig: AnimationConfig = {
  id: "floatingText",
  name: "Floating Text",
  category: "Basic",
  complexity: 2,
  previewSize: "md",
  component: FloatingText,
  code: floatingTextCode,
};

// Registry of all animations
// Build as flat array and filter out any undefined/null entries
export const animationRegistry: AnimationConfig[] = [
  // GFC Website animations
  ...gfcWebsiteAnimations,
  // Core animations
  pulseCircleConfig,
  slidingSquareConfig,
  floatingTextConfig,
  // Advanced animations
  morphingShapeFlowConfig,
  hologramGridWarpConfig,
  particleSwarmRibbonConfig,
  fractalPulseTunnelConfig,
  quantumPulseGridConfig,
  liquidNeonFluxConfig,
  hypercubeWarpTunnelConfig,
  // I ❤️ U morphing animations
  liquidMetalMorphConfig,
  singleLinePathMorphConfig,
  particleCloudMorphConfig,
  origamiFoldMorphConfig,
  fireIceDualMorphConfig,
  compressionMorphConfig,
  // Text FX animations (all 20 with distinct components)
  ...textFxAnimations,
  // Button animations (all 20 with distinct components)
  ...buttonAnimations,
  // UI animations (Forms, Cards, Dashboards, Media)
  ...uiAnimations,
  // Flag animations (Flags & Nations)
  ...flagsAnimations,
  // GFC Website animations
  ...gfcWebsiteAnimations,
].filter(
  (anim): anim is AnimationConfig =>
    !!anim && !!anim.id && typeof anim.component === "function"
);

// Helper function to get animation by ID
export function getAnimationById(id: string): AnimationConfig | undefined {
  if (!id) return undefined;
  return animationRegistry.find((anim) => anim && anim.id === id);
}

// Helper function to get animations by category
export function getAnimationsByCategory(category: string): AnimationConfig[] {
  return animationRegistry.filter(
    (anim) => anim && anim.category === category
  );
}

// Get all unique categories
export function getCategories(): string[] {
  return Array.from(new Set(animationRegistry.map((anim) => anim.category)));
}

// Export all configs
export {
  pulseCircleConfig,
  slidingSquareConfig,
  floatingTextConfig,
};

