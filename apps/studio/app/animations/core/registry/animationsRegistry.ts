/**
 * REV: AnimationPlayground/Phase-6.xA/TemplateSystem
 * 
 * Central Animation Registry
 * 
 * This is the SINGLE SOURCE OF TRUTH for all animations in the system.
 * All animations must be registered here with their complete metadata.
 * 
 * DOCUMENTATION OF PREVIOUS LOCATIONS:
 * ====================================
 * 
 * 1. Animation Metadata (AnimationMeta[]):
 *    - Location: app/animations/registry.ts
 *    - Contains: id, name, description, category, difficulty, usageCategory, gfcType, addedAt
 *    - Used by: AnimationPlaygroundClient.getPresets()
 * 
 * 2. Animation Components (AnimationConfig[]):
 *    - Location: app/animations/core/AnimationRegistry.ts
 *    - Contains: id, component, code, category, complexity
 *    - Used by: getAnimationById() in AnimationPlaygroundClient
 * 
 * 3. Category Definitions:
 *    - Location: app/hooks/useFilteredPresets.ts
 *    - Type: UsageCategory
 *    - Values: "all" | "new" | "hero" | "section-transition" | "card-tile" | "button-cta" | "buttons" | "text-headline" | "text-fx" | "background-ambient" | "interactive" | "forms-auth" | "dashboards-metrics" | "media-galleries" | "gfc-website" | "admin"
 * 
 * 4. NEW Indicator Logic:
 *    - Location: app/lib/isPresetNew.ts
 *    - Function: isPresetNew()
 *    - Uses: addedAt date, NewSettings, CalendarTemplate overrides
 * 
 * 5. Preset ID List:
 *    - Location: app/AnimationPlaygroundClient.tsx (lines 52-234)
 *    - Type: AnimationPresetId (union of all animation IDs)
 *    - Used for: Type safety and validation
 * 
 * 6. Tag Generation:
 *    - Location: app/AnimationPlaygroundClient.tsx (lines 272-308)
 *    - Function: getTags()
 *    - Generates tags based on category and name patterns
 * 
 * MIGRATION STRATEGY:
 * ==================
 * This registry combines metadata from registry.ts with components from AnimationRegistry.ts
 * into a single unified AnimationRegistryEntry[] array. The old files will remain temporarily
 * for backward compatibility but will eventually be removed after full migration.
 */

import type { AnimationRegistryEntry } from "../templates/types";
import { getAnimationById as getCoreAnimationById } from "../AnimationRegistry";
import { animations as animationMetas } from "../../registry";
import type { UsageCategory } from "@/app/hooks/useFilteredPresets";

/**
 * Helper function to determine template type from usage category and ID
 */
function inferTemplateType(
  usageCategory: UsageCategory,
  id: string,
  gfcType?: string
): AnimationRegistryEntry["templateType"] {
  if (id.startsWith("button-")) return "button";
  if (id.startsWith("gfc-cta-")) return "button";
  if (id.startsWith("gfc-hero-")) return "hero";
  if (id.startsWith("gfc-section-")) return "section";
  if (id.startsWith("gfc-banner-") || id.startsWith("gfc-chip-") || id.startsWith("gfc-footer-") || id.startsWith("gfc-strip-")) return "banner";
  if (id.startsWith("gfc-card-")) return "card";
  if (id.startsWith("gfc-calendar-") || id.startsWith("calendar-")) return "calendar-tile";
  if (id.startsWith("gfc-brand-")) return "card";
  
  if (usageCategory === "text-fx" || usageCategory === "text-headline") return "text";
  if (usageCategory === "buttons" || usageCategory === "button-cta") return "button";
  if (usageCategory === "card-tile") return "card";
  if (usageCategory === "forms-auth") return "form";
  if (usageCategory === "dashboards-metrics") return "dashboard";
  if (usageCategory === "media-galleries") return "media";
  if (usageCategory === "background-ambient") return "background";
  if (usageCategory === "interactive") return "interactive";
  if (usageCategory === "hero") return "hero";
  if (usageCategory === "section-transition") return "section";
  
  return "generic";
}

/**
 * Helper function to generate tags from category and name
 * (Replicates logic from AnimationPlaygroundClient.getTags)
 */
function generateTags(category: string, name: string): readonly string[] {
  const lowerName = name.toLowerCase();
  const lowerCategory = category.toLowerCase();
  
  // Button-specific tags
  if (lowerName.includes("button") || lowerCategory.includes("button")) {
    if (lowerName.includes("ripple")) return ["Button", "Ripple", "Click"];
    if (lowerName.includes("glass") || lowerName.includes("morph")) return ["Button", "Glass", "Hover"];
    if (lowerName.includes("neon") || lowerName.includes("glow")) return ["Button", "Neon", "Glow"];
    if (lowerName.includes("magnetic")) return ["Button", "Magnetic", "Interactive"];
    if (lowerName.includes("gooey") || lowerName.includes("soft")) return ["Button", "Morph", "Spring"];
    if (lowerName.includes("shockwave")) return ["Button", "Shockwave", "Click"];
    if (lowerName.includes("shimmer")) return ["Button", "Shimmer", "Hover"];
    if (lowerName.includes("3d") || lowerName.includes("tilt") || lowerName.includes("parallax")) return ["Button", "3D", "Parallax"];
    if (lowerName.includes("bloom") || lowerName.includes("color")) return ["Button", "Color", "Hover"];
    if (lowerName.includes("particle")) return ["Button", "Particles", "Click"];
    if (lowerName.includes("laser") || lowerName.includes("trace")) return ["Button", "Laser", "Border"];
    if (lowerName.includes("aurora")) return ["Button", "Gradient", "Aurora"];
    if (lowerName.includes("morphing") || lowerName.includes("shape")) return ["Button", "Morph", "Shape"];
    if (lowerName.includes("shadow")) return ["Button", "Shadow", "Dynamic"];
    if (lowerName.includes("firefly") || lowerName.includes("sparkle")) return ["Button", "Particles", "Sparkle"];
    if (lowerName.includes("split") || lowerName.includes("reveal")) return ["Button", "Split", "Reveal"];
    if (lowerName.includes("layered") || lowerName.includes("pop")) return ["Button", "3D", "Layers"];
    if (lowerName.includes("aura") || lowerName.includes("pressure")) return ["Button", "Aura", "Glow"];
    if (lowerName.includes("holographic") || lowerName.includes("glitch")) return ["Button", "Glitch", "Holographic"];
    if (lowerName.includes("icon")) return ["Button", "Icon", "Story"];
    return ["Button", "Hover", "Click"];
  }
  
  // Text-specific tags
  if (lowerName.includes("text") || lowerCategory.includes("text")) {
    if (lowerName.includes("neon")) return ["Text", "Neon", "Glow"];
    if (lowerName.includes("typewriter") || lowerName.includes("typing")) return ["Text", "Typewriter", "Reveal"];
    if (lowerName.includes("liquid") || lowerName.includes("wave")) return ["Text", "Liquid", "Morph"];
    if (lowerName.includes("particle")) return ["Text", "Particles", "Reveal"];
    if (lowerName.includes("glitch")) return ["Text", "Glitch", "Effect"];
    if (lowerName.includes("3d") || lowerName.includes("carousel")) return ["Text", "3D", "Rotation"];
    return ["Text", "Animation", "Effect"];
  }
  
  // Calendar-specific tags
  if (lowerName.includes("calendar") || lowerCategory.includes("calendar")) {
    if (lowerName.includes("available")) return ["Calendar", "Availability", "Indicator"];
    if (lowerName.includes("booked") || lowerName.includes("blocked")) return ["Calendar", "Booked", "Status"];
    if (lowerName.includes("event") || lowerName.includes("creation")) return ["Calendar", "Event", "Creation"];
    if (lowerName.includes("navigation") || lowerName.includes("month")) return ["Calendar", "Navigation", "Transition"];
    return ["Calendar", "Tile", "Animation"];
  }
  
  // Flag-specific tags
  if (lowerName.includes("flag") || lowerCategory.includes("flag")) {
    if (lowerName.includes("holiday")) return ["Flag", "Holiday", "Celebration"];
    if (lowerName.includes("ribbon") || lowerName.includes("weave")) return ["Flag", "Ribbon", "Weave"];
    if (lowerName.includes("morph") || lowerName.includes("transform")) return ["Flag", "Morph", "Transform"];
    if (lowerName.includes("particle") || lowerName.includes("burst")) return ["Flag", "Particles", "Reveal"];
    if (lowerName.includes("firework") || lowerName.includes("ignite")) return ["Flag", "Fireworks", "Reveal"];
    if (lowerName.includes("heart")) return ["Flag", "Heart", "Spiral"];
    if (lowerName.includes("3d") || lowerName.includes("pole")) return ["Flag", "3D", "Flagpole"];
    if (lowerName.includes("flip") || lowerName.includes("card")) return ["Flag", "Flip", "Transition"];
    if (lowerName.includes("wind") || lowerName.includes("wave")) return ["Flag", "Wind", "Wave"];
    if (lowerName.includes("split") || lowerName.includes("screen")) return ["Flag", "Split", "Merge"];
    return ["Flag", "Animation", "Dual"];
  }
  
  // Generic patterns
  if (lowerName.includes("pulse") && !lowerName.includes("fractal") && !lowerName.includes("quantum")) return ["Scale", "Loop", "Ease"];
  if (lowerName.includes("sliding")) return ["Position", "Repeat", "EaseInOut"];
  if (lowerName.includes("floating")) return ["Opacity", "Y motion", "Hero"];
  if (lowerName.includes("morph")) return ["SVG", "Morphing", "Gradient"];
  if (lowerName.includes("hologram") || lowerName.includes("grid")) return ["Interactive", "3D", "Mouse"];
  if (lowerName.includes("particle") || lowerName.includes("swarm")) return ["Canvas", "Particles", "Noise"];
  if (lowerName.includes("fractal") || lowerName.includes("tunnel")) return ["Recursive", "Depth", "Pulse"];
  if (lowerName.includes("quantum")) return ["Multi-layer", "Waves", "Energy"];
  if (lowerName.includes("liquid") || lowerName.includes("neon")) return ["Morphing", "Gradient", "Flux"];
  if (lowerName.includes("hypercube") || lowerName.includes("warp")) return ["4D", "Rotation", "Tunnel"];
  
  return ["Animation"];
}

/**
 * Helper function to convert difficulty number to string
 */
function getDifficultyLevel(difficulty?: number): "Beginner" | "Intermediate" | "Advanced" {
  if (!difficulty) return "Beginner";
  if (difficulty === 1) return "Beginner";
  if (difficulty === 2) return "Intermediate";
  return "Advanced";
}

/**
 * Initialize the registry by combining metadata and components
 * This function should be called once at module load time
 */
function initializeRegistry(): AnimationRegistryEntry[] {
  const entries: AnimationRegistryEntry[] = [];
  
  // Iterate through all animation metadata
  for (const meta of animationMetas) {
    // Get the component from the core registry
    const coreAnim = getCoreAnimationById(meta.id);
    
    if (!coreAnim) {
      // Skip if component not found (might be disabled or not yet implemented)
      console.warn(`[Registry] Component not found for animation: ${meta.id}`);
      continue;
    }
    
    // Determine template type
    const templateType = inferTemplateType(
      (meta.usageCategory || "background-ambient") as UsageCategory,
      meta.id,
      meta.gfcType
    );
    
    // Generate tags
    const tags = generateTags(meta.category || "Basic", meta.name);
    
    // Create registry entry
    const entry: AnimationRegistryEntry = {
      id: meta.id,
      name: meta.name,
      shortDescription: meta.description.split(".")[0] + ".",
      longDescription: meta.description,
      difficulty: getDifficultyLevel(meta.difficulty),
      usageCategory: (meta.usageCategory || "background-ambient") as UsageCategory,
      tags: Object.freeze(tags),
      addedAt: meta.addedAt,
      gfcType: meta.gfcType,
      templateType,
      templateName: meta.id, // Use ID as template name for now (can be customized later)
      component: coreAnim.component,
      code: coreAnim.code || "// Code not available",
      route: meta.route,
    };
    
    entries.push(entry);
  }
  
  return Object.freeze(entries);
}

/**
 * Central animation registry
 * 
 * This array contains ALL animations in the system.
 * Each entry combines metadata from registry.ts with components from AnimationRegistry.ts
 * Initialized at module load time.
 */
export const animationsRegistry: readonly AnimationRegistryEntry[] = initializeRegistry();

/**
 * Get animation by ID
 */
export function getAnimationById(id: string): AnimationRegistryEntry | undefined {
  return animationsRegistry.find((entry) => entry.id === id);
}

/**
 * Get all animations
 */
export function getAllAnimations(): readonly AnimationRegistryEntry[] {
  return animationsRegistry;
}

/**
 * Get animations by usage category
 */
export function getAnimationsByCategory(category: UsageCategory): AnimationRegistryEntry[] {
  if (category === "all") {
    return [...animationsRegistry];
  }
  return animationsRegistry.filter((entry) => entry.usageCategory === category);
}

/**
 * Get animations by template type
 */
export function getAnimationsByTemplateType(templateType: AnimationRegistryEntry["templateType"]): AnimationRegistryEntry[] {
  return animationsRegistry.filter((entry) => entry.templateType === templateType);
}

/**
 * Get animations by GFC type
 */
export function getAnimationsByGfcType(gfcType: string): AnimationRegistryEntry[] {
  return animationsRegistry.filter((entry) => entry.gfcType === gfcType);
}

