/**
 * REV: AnimationPlayground/Phase-6.xA/TemplateSystem
 * 
 * Template Type Definitions
 * 
 * This file defines the base and specialized template configurations
 * for the centralized animation template system.
 */

import type { ComponentType } from "react";
import type { UsageCategory } from "@/app/hooks/useFilteredPresets";
import type { GfcWebsiteType } from "../types";

/**
 * Base template configuration that all templates extend
 */
export interface BaseTemplateConfig {
  /** Unique identifier for this animation */
  id: string;
  /** Display name */
  name: string;
  /** Short description (first sentence) */
  shortDescription: string;
  /** Full description */
  longDescription: string;
  /** Difficulty level */
  difficulty: "Beginner" | "Intermediate" | "Advanced";
  /** Usage category for filtering */
  usageCategory: UsageCategory;
  /** Tags for search and filtering */
  tags: readonly string[];
  /** ISO date string when animation was added (YYYY-MM-DD) */
  addedAt?: string;
  /** Whether this animation is marked as "new" */
  isNew?: boolean;
  /** Whether animation is heavy/complex (performance consideration) */
  heavy?: boolean;
  /** Whether animation is mobile-safe */
  mobileSafe?: boolean;
  /** GFC Website type (if applicable) */
  gfcType?: GfcWebsiteType;
  /** Template type identifier */
  templateType: TemplateType;
  /** Template name/identifier for renderer lookup */
  templateName: string;
  /** Template-specific options/config */
  templateOptions?: Record<string, unknown>;
}

/**
 * Template type identifiers
 */
export type TemplateType =
  | "button"
  | "text"
  | "header"
  | "card"
  | "calendar-tile"
  | "background"
  | "interactive"
  | "form"
  | "dashboard"
  | "media"
  | "hero"
  | "section"
  | "banner"
  | "generic";

/**
 * Button template configuration
 */
export interface ButtonTemplateConfig extends BaseTemplateConfig {
  templateType: "button";
  templateName: string;
  templateOptions?: {
    /** Button text */
    text?: string;
    /** Button variant */
    variant?: "primary" | "secondary" | "outline" | "ghost";
    /** Icon name (if applicable) */
    icon?: string;
    /** Size */
    size?: "sm" | "md" | "lg";
  };
}

/**
 * Text template configuration
 */
export interface TextTemplateConfig extends BaseTemplateConfig {
  templateType: "text";
  templateName: string;
  templateOptions?: {
    /** Text content */
    text?: string;
    /** Font size */
    fontSize?: "sm" | "md" | "lg" | "xl" | "2xl";
    /** Font weight */
    fontWeight?: "normal" | "medium" | "semibold" | "bold";
  };
}

/**
 * Header template configuration
 */
export interface HeaderTemplateConfig extends BaseTemplateConfig {
  templateType: "header";
  templateName: string;
  templateOptions?: {
    /** Header text */
    text?: string;
    /** Header level */
    level?: 1 | 2 | 3 | 4 | 5 | 6;
    /** Subtitle */
    subtitle?: string;
  };
}

/**
 * Card template configuration
 */
export interface CardTemplateConfig extends BaseTemplateConfig {
  templateType: "card";
  templateName: string;
  templateOptions?: {
    /** Card title */
    title?: string;
    /** Card content */
    content?: string;
    /** Card image URL */
    imageUrl?: string;
  };
}

/**
 * Calendar tile template configuration
 */
export interface CalendarTileTemplateConfig extends BaseTemplateConfig {
  templateType: "calendar-tile";
  templateName: string;
  templateOptions?: {
    /** Day state */
    dayState?: "available" | "booked" | "blocked" | "pending";
    /** Date */
    date?: string;
  };
}

/**
 * Union type of all template configs
 */
export type TemplateConfig =
  | ButtonTemplateConfig
  | TextTemplateConfig
  | HeaderTemplateConfig
  | CardTemplateConfig
  | CalendarTileTemplateConfig
  | BaseTemplateConfig;

/**
 * Animation registry entry
 * This is the single source of truth for all animations
 */
export interface AnimationRegistryEntry extends BaseTemplateConfig {
  /** React component for this animation */
  component: ComponentType<any>;
  /** Source code for this animation */
  code: string;
  /** Route path (if different from default) */
  route?: string;
}

