/**
 * REV: AnimationPlayground/Phase-6.xA/TemplateSystem
 * 
 * Template Renderer
 * 
 * This file provides a unified renderer that maps registry entries to React components.
 * The renderer handles all template types and provides a consistent interface.
 */

import React from "react";
import type { AnimationRegistryEntry } from "../templates/types";

/**
 * Render an animation from a registry entry
 * 
 * This is the main renderer function that takes a registry entry
 * and returns the appropriate React component.
 */
export function renderAnimation(entry: AnimationRegistryEntry): React.ReactElement {
  const Component = entry.component;
  
  // For now, we just render the component directly
  // In the future, we can add template-specific wrappers here
  return <Component />;
}

/**
 * Get the component for a registry entry
 * 
 * This is a helper function that returns the component directly
 * without rendering it. Useful for cases where you need the component
 * itself rather than a rendered element.
 */
export function getAnimationComponent(entry: AnimationRegistryEntry): React.ComponentType<any> {
  return entry.component;
}

