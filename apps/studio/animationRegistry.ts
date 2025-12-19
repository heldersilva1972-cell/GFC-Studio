import { morphingShapeFlowConfig } from "./animations/advanced/MorphingShapeFlow";
import type { ComponentType } from "react";

// Animation Registry
// This file registers all available animations in the system

export interface AnimationConfig {
  id: string;
  name: string;
  category: string;
  complexity: number;
  previewSize: "sm" | "md" | "lg";
  component: ComponentType<any>;
  code: string;
}

// Registry of all animations
export const animationRegistry: AnimationConfig[] = [
  morphingShapeFlowConfig,
];

// Helper function to get animation by ID
export function getAnimationById(id: string): AnimationConfig | undefined {
  return animationRegistry.find((anim) => anim.id === id);
}

// Helper function to get animations by category
export function getAnimationsByCategory(category: string): AnimationConfig[] {
  return animationRegistry.filter((anim) => anim.category === category);
}

// Get all unique categories
export function getCategories(): string[] {
  return Array.from(new Set(animationRegistry.map((anim) => anim.category)));
}

