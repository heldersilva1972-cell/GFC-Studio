"use client";

import React, { useState, useMemo, useEffect, useCallback } from "react";
import { motion, AnimatePresence } from "framer-motion";
import {
  Sparkles,
  MonitorPlay,
  Code2,
  LayoutTemplate,
  Wand2,
  Copy,
  Check,
  Home,
  BookOpen,
  ArrowLeft,
  Trash2,
  Info,
  ChevronDown,
  ChevronUp,
} from "lucide-react";
import Link from "next/link";
// Phase 6.xA: Use new centralized registry
import { 
  getAllAnimations, 
  getAnimationById as getRegistryAnimationById 
} from "./animations/core/registry/animationsRegistry";
import { getAnimationById as getCoreAnimationById } from "./animations/core/AnimationRegistry"; // Keep for backward compatibility
import { animations as animationMetas } from "./animations/registry"; // Keep for backward compatibility during migration
import { useFilteredPresets, type UsageCategory } from "./hooks/useFilteredPresets";
import type { GfcWebsiteType } from "./animations/core/types";
import { loadNewSettings, saveNewSettings, DEFAULT_NEW_SETTINGS, type NewSettings } from "./lib/newSettings";
import { isPresetNew } from "./lib/isPresetNew";
import { Settings } from "lucide-react";
import { AnimationEditorPanel } from "@/app/components/editor/AnimationEditorPanel";
import { ConfirmDeleteModal } from "@/components/ConfirmDeleteModal";
import { PlaygroundSettingsDialog } from "@/components/Settings/PlaygroundSettingsDialog";
import {
  deleteAnimation,
  getDeletedAnimationIds,
} from "@/utils/deleteAnimation";
import GfcCalendarInteractivePanel, { type CalendarSelection, type CalendarDayState } from "@/components/gfc/GfcCalendarInteractivePanel";
import { getInitialCalendarContext, CALENDAR_CONTEXTS, type CalendarContext } from "./lib/calendarState";
import { useCalendarAnimation } from "./hooks/useCalendarAnimation";
import CalendarAnimationAdminPanel from "@/components/admin/CalendarAnimationAdminPanel";
import {
  getCalendarTemplate,
  getTemplatesForCalendar,
  getFavoriteTemplates,
  type CalendarTemplate,
  isAnimationEnabledInTemplate,
  shouldForceNewInTemplate,
  shouldNeverNewInTemplate,
} from "./lib/calendarTemplateConfig";

type Difficulty = "Beginner" | "Intermediate" | "Advanced";

type AnimationPresetId =
  | "pulseCircle"
  | "slidingSquare"
  | "floatingText"
  | "morphingShapeFlow"
  | "hologramGridWarp"
  | "particleSwarmRibbon"
  | "fractalPulseTunnel"
  | "quantumPulseGrid"
  | "liquidNeonFlux"
  | "hypercubeWarpTunnel"
  | "liquidMetalMorph"
  | "singleLinePathMorph"
  | "particleCloudMorph"
  | "origamiFoldMorph"
  | "fireIceDualMorph"
  | "compressionMorph"
  | "neon-pulse-glow"
  | "digital-flip-clock"
  | "typewriter-ink-bleed"
  | "liquid-wave-text"
  | "particle-burst-reveal"
  | "floating-balloon-letters"
  | "metallic-shine-sweep"
  | "explode-reform"
  | "flame-text-morph"
  | "laser-beam-writing"
  | "glitch-scramble-decode"
  | "origami-fold-text"
  | "rotation-carousel-3d"
  | "ice-freeze-crack"
  | "bubble-pop-reveal"
  | "warp-speed-stretch"
  | "ribbon-text-unfold"
  | "chromatic-rgb-split"
  | "electric-arc-stitching"
  | "ink-calligraphy-stroke"
  | "button-liquid-ripple"
  | "button-glass-morph-slide"
  | "button-neon-pulse-glow"
  | "button-magnetic-attraction"
  | "button-soft-body-gooey"
  | "button-shockwave-click"
  | "button-shimmer-sweep"
  | "button-3d-tilt-parallax"
  | "button-color-bloom"
  | "button-particle-burst"
  | "button-laser-border-trace"
  | "button-aurora-gradient"
  | "button-morphing-shape"
  | "button-dynamic-shadow"
  | "button-inner-ripple-glow"
  | "button-firefly-sparkle"
  | "button-split-reveal"
  | "button-3d-pop-out-layers"
  | "button-aura-pressure"
  | "button-holographic-glitch"
  | "button-icon-envelope-plane"
  | "button-icon-envelope-flap-burst"
  | "button-icon-plane-runway"
  | "button-icon-inbox-zero-sweep"
  | "button-icon-chat-rocket"
  | "button-icon-typing-blast"
  | "button-icon-folder-arrow"
  | "button-icon-cloud-reverse"
  | "button-icon-heart-confetti"
  | "button-icon-star-supernova"
  | "button-icon-music-equalizer"
  | "button-icon-cart-checkout"
  | "button-icon-wand-sparkle"
  | "button-icon-portal-morph"
  | "button-icon-trash-vacuum"
  | "button-icon-lock-unlock"
  | "button-icon-camera-photo"
  | "button-icon-envelope-sequence"
  | "button-icon-flip-card"
  | "button-icon-particle-transform"
  | "gfc-hero-simple-fade"
  | "gfc-hero-diagonal-split"
  | "gfc-announcement-ribbon"
  | "gfc-section-header-gold-underline"
  | "gfc-cta-rent-hall"
  | "gfc-cta-join"
  | "gfc-cta-email"
  | "gfc-cta-donate"
  | "gfc-cta-learn-more"
  | "gfc-cta-book-now"
  | "gfc-cta-contact"
  | "gfc-american-flag-wave"
  | "gfc-american-flag-webgl"
  | "gfc-card-membership-stats"
  | "gfc-card-hall-feature-strip"
  | "gfc-card-floorplan-reveal"
  | "gfc-card-photo-mosaic"
  | "gfc-card-event-hover"
  | "gfc-card-team-member"
  | "gfc-section-parallax-hero"
  | "gfc-section-history-timeline"
  | "gfc-section-rental-steps"
  | "gfc-section-wave-divider"
  | "gfc-section-sticky-mini-nav"
  | "gfc-banner-event-countdown"
  | "gfc-chip-open-rentals"
  | "gfc-footer-cta-bar"
  | "gfc-strip-contact-info"
  | "gfc-brand-donation-impact-meter"
  | "gfc-brand-crest-highlight"
  | "gfc-brand-perfect-for-badges"
  | "gfc-brand-testimonials-slider"
  | "gfc-brand-membership-type-flip-cards"
  | "gfc-calendar-available-tile"
  | "gfc-calendar-booked-tile"
  | "gfc-calendar-pending-tile"
  | "gfc-calendar-header-slide"
  | "gfc-calendar-month-switch"
  | "gfc-calendar-range-highlight"
  | "gfc-calendar-available-day-pulse"
  | "gfc-calendar-available-hover-lift"
  | "gfc-calendar-soft-availability-ripple"
  | "gfc-calendar-availability-gradient-sweep"
  | "gfc-calendar-faint-checkmark-reveal"
  | "gfc-calendar-morning-evening-color-split"
  | "gfc-calendar-priority-date-glow"
  | "gfc-calendar-booked-day-lock-in"
  | "gfc-calendar-conflict-shake-red"
  | "gfc-calendar-full-day-flash-warning"
  | "gfc-calendar-blocked-date-stripe"
  | "gfc-calendar-gap-rule-violation-pulse"
  | "gfc-calendar-event-overlap-slide-alert"
  | "gfc-calendar-holiday-seal-stamp"
  | "gfc-calendar-add-event-slide-up-form"
  | "gfc-calendar-add-event-target-highlight"
  | "gfc-calendar-event-created-success-burst"
  | "gfc-calendar-event-denied-shake"
  | "gfc-calendar-event-saved-ribbon"
  | "gfc-calendar-admin-override-glow"
  | "gfc-calendar-admin-edit-tile-flip"
  | "gfc-calendar-admin-bulk-blocking-sweep"
  | "gfc-calendar-month-slide-left"
  | "gfc-calendar-month-slide-right"
  | "gfc-calendar-month-fade-reveal"
  | "password-strength-meter"
  | "glass-login-switcher"
  | "magic-play-button"
  | "liquid-distortion-text"
  | "neon-border-card"
  | "product-configurator-card"
  | "hover-info-reveal-card"
  | "dashboard-metrics-tiles"
  | "image-uploader-gallery"
  | "calendar-page-flip"
  | "calendar-date-pulse"
  | "calendar-day-glow"
  | "calendar-week-slide"
  | "calendar-pop-in-event"
  | "calendar-booked-stamp"
  | "calendar-available-fade"
  | "calendar-drag-add-event"
  | "calendar-bounce-day-marker"
  | "calendar-swipe-month"
  | "calendar-peel-back-reveal"
  | "calendar-highlight-range"
  | "calendar-dot-pop"
  | "calendar-event-ribbon"
  | "calendar-month-carousel"
  | "calendar-fog-unavailable"
  | "calendar-flash-available"
  | "calendar-event-ping"
  | "calendar-event-drop-in"
  | "calendar-availability-checker"
  | "calendar-tap-to-add"
  | "calendar-heat-map-glow"
  | "calendar-soft-wave-transition"
  | "calendar-event-flag-rise"
  | "calendar-booked-lock-in"
  | "calendar-create-event-pop"
  | "calendar-drag-to-create-slot"
  | "calendar-event-expand-form"
  | "calendar-day-press-popup"
  | "calendar-slide-in-event-creator"
  | "calendar-tap-add-marker"
  | "calendar-event-grow-highlight"
  | "calendar-drop-event-into-day"
  | "calendar-select-window-pulse"
  | "calendar-event-bubble-rise"
  | "flags_dual_ribbon_weave"
  | "flags_star_to_shield_morph"
  | "flags_wind_wave_sync"
  | "flags_split_screen_merge"
  | "flags_particle_burst_reveal"
  | "flags_heart_dual_flag"
  | "flags_crossed_flagpoles_3d"
  | "flags_flip_card_transition"
  | "flags_split_stripes_transform"
  | "flags_firework_ignite_reveal"
  | "flags_holiday_new_year"
  | "flags_holiday_us_independence"
  | "flags_holiday_pt_dia_de_portugal"
  | "flags_holiday_thanksgiving"
  | "flags_holiday_memorial_day"
  | "flags_holiday_christmas"
  | "flags_holiday_easter"
  | "flags_holiday_labor_day"
  | "american-flag-advanced"
  | "portuguese-flag-realistic-wave";

// Import AnimationPreset type from hook to ensure consistency
import type { AnimationPreset as HookAnimationPreset } from "./hooks/useFilteredPresets";

// Local AnimationPreset extends the hook type with AnimationPresetId
interface AnimationPreset extends Omit<HookAnimationPreset, "id"> {
  id: AnimationPresetId;
}

// UsageCategory is now imported from useFilteredPresets hook

const USAGE_CATEGORIES: { id: UsageCategory; label: string; icon: string }[] = [
  { id: "all", label: "All", icon: "✨" },
  { id: "new", label: "New", icon: "🆕" },
  { id: "hero", label: "Hero", icon: "🎯" },
  { id: "section-transition", label: "Transitions", icon: "🔄" },
  { id: "text-fx", label: "Text FX", icon: "🅰️" },
  { id: "card-tile", label: "Cards", icon: "🎴" },
  { id: "button-cta", label: "CTAs", icon: "🔘" },
  { id: "buttons", label: "Buttons", icon: "🎨" },
  { id: "text-headline", label: "Text", icon: "📝" },
  { id: "background-ambient", label: "Background", icon: "🌌" },
  { id: "interactive", label: "Interactive", icon: "🖱️" },
  { id: "forms-auth", label: "Forms & Auth", icon: "🔐" },
  { id: "dashboards-metrics", label: "Dashboards", icon: "📊" },
  { id: "media-galleries", label: "Media", icon: "🖼️" },
  { id: "gfc-website", label: "GFC Website", icon: "🏛️" },
  { id: "flags", label: "Flags & Nations", icon: "🏳️" },
  { id: "admin", label: "Admin", icon: "⚙️" },
];

function getLevel(difficulty?: number): Difficulty {
  if (!difficulty) return "Beginner";
  if (difficulty === 1) return "Beginner";
  if (difficulty === 2) return "Intermediate";
  return "Advanced";
}

function getTags(category: string, name: string): string[] {
  const lowerName = name.toLowerCase();
  if (lowerName.includes("pulse") && !lowerName.includes("fractal") && !lowerName.includes("quantum"))
    return ["Scale", "Loop", "Ease"];
  if (lowerName.includes("sliding")) return ["Position", "Repeat", "EaseInOut"];
  if (lowerName.includes("floating")) return ["Opacity", "Y motion", "Hero"];
  if (lowerName.includes("morph")) return ["SVG", "Morphing", "Gradient"];
  if (lowerName.includes("hologram") || lowerName.includes("grid")) return ["Interactive", "3D", "Mouse"];
  if (lowerName.includes("particle") || lowerName.includes("swarm")) return ["Canvas", "Particles", "Noise"];
  if (lowerName.includes("fractal") || lowerName.includes("tunnel")) return ["Recursive", "Depth", "Pulse"];
  if (lowerName.includes("quantum")) return ["Multi-layer", "Waves", "Energy"];
  if (lowerName.includes("liquid") || lowerName.includes("neon")) return ["Morphing", "Gradient", "Flux"];
  if (lowerName.includes("hypercube") || lowerName.includes("warp")) return ["4D", "Rotation", "Tunnel"];
  if (lowerName.includes("button") || category.toLowerCase().includes("button")) {
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
    return ["Button", "Hover", "Click"];
  }
  if (lowerName.includes("flag") || category.toLowerCase().includes("flag")) {
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
  return ["Animation"];
}

// Phase 6.xA: Updated to use centralized registry
// Helper function to get presets (filters out deleted animations)
function getPresets(): readonly AnimationPreset[] {
  const deletedIds = typeof window !== "undefined" ? getDeletedAnimationIds() : [];
  
  // Phase C6: Import calendar config helpers (dynamic import to avoid SSR issues)
  let getCalendarAnimationConfig: ((id: string) => any) | null = null;
  try {
    if (typeof window !== "undefined") {
      const calendarConfigModule = require("./lib/calendarAnimationConfig");
      getCalendarAnimationConfig = calendarConfigModule.getCalendarAnimationConfig;
    }
  } catch {
    // Ignore if module not available
  }
  
  // Phase 6.xA: Use new centralized registry as single source of truth
  const registryEntries = getAllAnimations();
  
  return Object.freeze(
    registryEntries
      .filter((entry) => {
        // Filter out deleted animations
        if (deletedIds.includes(entry.id)) return false;
        
        // Phase C6: Filter out disabled calendar animations
        if (getCalendarAnimationConfig) {
          const calendarConfig = getCalendarAnimationConfig(entry.id);
          if (calendarConfig && !calendarConfig.enabled) {
            return false;
          }
        }
        
        return true;
      })
      .map((entry) => {
        // Phase C6: Use calendar config metadata if available (overrides registry)
        let displayName = entry.name;
        let description = entry.longDescription;
        let tags = entry.tags;
        
        if (getCalendarAnimationConfig) {
          const calendarConfig = getCalendarAnimationConfig(entry.id);
          if (calendarConfig) {
            displayName = calendarConfig.displayName || displayName;
            description = calendarConfig.description || description;
            tags = calendarConfig.tags || tags;
          }
        }
        
        return Object.freeze({
          id: entry.id as AnimationPresetId,
          name: displayName,
          difficulty: entry.difficulty,
          shortDescription: entry.shortDescription,
          longDescription: description,
          tags: Object.freeze(tags) as readonly string[],
          code: entry.code,
          Component: entry.component,
          usageCategory: entry.usageCategory,
          gfcType: entry.gfcType,
          addedAt: entry.addedAt,
        });
      })
  );
}

export default function AnimationPlaygroundClient() {
  const [ready, setReady] = useState(false);
  const [deletedPresetIds, setDeletedPresetIds] = useState<string[]>([]);

  useEffect(() => {
    try {
      const raw =
        typeof window !== "undefined"
          ? window.localStorage.getItem("ap_deletedPresetIds_v1")
          : null;
      if (raw) {
        const parsed = JSON.parse(raw);
        if (Array.isArray(parsed)) {
          setDeletedPresetIds(parsed.filter((id): id is string => typeof id === "string"));
        }
      }
    } catch {
      // Ignore localStorage errors
    }
  }, []);

  // PRESET FILTER STATE - Independent from animation selection
  const [selectedCategory, setSelectedCategory] = useState<UsageCategory>("all");
  
  // GFC Website second-level filter state
  const [gfcTypeFilter, setGfcTypeFilter] = useState<GfcWebsiteType | "all">("all");
  
  // GFC Calendar subcategory filter state (for calendar-specific filtering)
  type CalendarSubcategory = 
    | "all"
    | "availability"
    | "booked-days"
    | "event-creation"
    | "status-indicators"
    | "navigation"
    | "loading-feedback";
  const [calendarSubcategoryFilter, setCalendarSubcategoryFilter] = useState<CalendarSubcategory>("all");
  
  // Store last selected GFC filter for state restoration
  const [lastGfcTypeFilter, setLastGfcTypeFilter] = useState<GfcWebsiteType | "all">("all");
  const [lastCalendarSubcategoryFilter, setLastCalendarSubcategoryFilter] = useState<CalendarSubcategory>("all");

  // Calendar interactive panel state (Phase C5)
  const [selectedCalendarId, setSelectedCalendarId] = useState<string>(CALENDAR_CONTEXTS[0].id);
  const [calendarContexts, setCalendarContexts] = useState<Map<string, CalendarContext>>(() => {
    const contexts = new Map<string, CalendarContext>();
    CALENDAR_CONTEXTS.forEach((cal) => {
      contexts.set(cal.id, getInitialCalendarContext(cal.id));
    });
    return contexts;
  });
  
  // Phase C7: Template state - map calendar ID to template ID
  const [calendarTemplates, setCalendarTemplates] = useState<Map<string, string>>(() => {
    const templates = new Map<string, string>();
    // Set default template for each calendar
    CALENDAR_CONTEXTS.forEach((cal) => {
      templates.set(cal.id, "gfc-default-calendar");
    });
    return templates;
  });
  
  const currentCalendarContext = useMemo(() => {
    return calendarContexts.get(selectedCalendarId) || getInitialCalendarContext(selectedCalendarId);
  }, [calendarContexts, selectedCalendarId]);

  // Phase C7: Get active template for current calendar
  const activeTemplate = useMemo(() => {
    const templateId = calendarTemplates.get(selectedCalendarId);
    if (templateId) {
      return getCalendarTemplate(templateId) || null;
    }
    return null;
  }, [selectedCalendarId, calendarTemplates]);

  const { getAnimationForDayState, getAnimationForNavigation, getAnimationForWeek, getAnimationForEventCreation, getAnimationForHover } = useCalendarAnimation(activeTemplate);

  // Generate usage instructions based on preset type
  const getUsageInstructions = (preset: AnimationPreset): string => {
    const id = preset.id.toLowerCase();
    const name = preset.name.toLowerCase();

    // Calendar animations
    if (id.includes("calendar") || name.includes("calendar")) {
      if (id.includes("event") || id.includes("creation") || id.includes("add")) {
        return "Select the 'Calendar Add Event' filter, then click on an available day in the calendar to see this animation. It shows when creating a new event.";
      }
      if (id.includes("available") || id.includes("availability")) {
        return "Hover over or click available days in the calendar panel to see this animation. It indicates the day is free for booking.";
      }
      if (id.includes("booked") || id.includes("blocked")) {
        return "Click on booked or blocked days in the calendar to see this animation. It shows the day is unavailable.";
      }
      if (id.includes("navigation") || id.includes("month") || id.includes("header")) {
        return "Use the previous/next month buttons in the calendar panel to see this animation. It triggers when navigating between months.";
      }
      if (id.includes("range") || id.includes("week") || id.includes("select")) {
        return "Click on a week number or select a range of days in the calendar to see this animation. It highlights selected periods.";
      }
      if (id.includes("pending") || id.includes("status")) {
        return "This animation appears on days with pending status. Click on pending days in the calendar to see it.";
      }
      return "Interact with the calendar panel below to see this animation. Click on days, hover over dates, or navigate months.";
    }

    // Button animations
    if (id.includes("button") || name.includes("button")) {
      return "Click the button in the preview panel to see the animation. The animation plays on click and resets automatically.";
    }

    // Form animations
    if (id.includes("form") || id.includes("input") || id.includes("password") || id.includes("login")) {
      return "Interact with the form in the preview panel. Type in inputs, toggle password visibility, or switch between login/signup modes to see the animation.";
    }

    // Card animations
    if (id.includes("card") || name.includes("card")) {
      return "Hover over the card in the preview panel to see the animation. Some cards may have click interactions as well.";
    }

    // Text animations
    if (id.includes("text") || name.includes("text") || id.includes("typing") || id.includes("typewriter")) {
      return "The animation plays automatically when the component loads in the preview panel. Watch the preview to see the effect.";
    }

    // Dashboard/Metric animations
    if (id.includes("dashboard") || id.includes("metric") || id.includes("tile")) {
      return "Hover over metric tiles or dashboard elements in the preview to see the animation. Some animations trigger on mount.";
    }

    // Media/Gallery animations
    if (id.includes("image") || id.includes("gallery") || id.includes("upload") || id.includes("media")) {
      return "Interact with the upload area or gallery items in the preview. Drag and drop files, click to upload, or hover over thumbnails.";
    }

    // Default
    return "Click this tile to see the animation in the preview panel. Hover, click, or watch for automatic animations depending on the component type.";
  };

  // Helper function to categorize calendar animations into subcategories
  // Pure function - doesn't depend on component state, so it's stable
  // Phase C7: Use calendar animation config as source of truth when available
  const getCalendarSubcategory = (presetId: string, presetName: string): CalendarSubcategory | null => {
    // First, try to get category from calendar animation config (most accurate)
    try {
      if (typeof window !== "undefined") {
        const calendarConfigModule = require("./lib/calendarAnimationConfig");
        const config = calendarConfigModule.getCalendarAnimationConfig(presetId);
        if (config && config.category) {
          // Map config category to subcategory
          const categoryMap: Record<string, CalendarSubcategory> = {
            "Calendar Availability": "availability",
            "Calendar Booked Days": "booked-days",
            "Calendar Add Event": "event-creation",
            "Calendar Status Indicators": "status-indicators",
            "Calendar Navigation": "navigation",
            "Calendar Loading & Feedback": "loading-feedback",
          };
          const mapped = categoryMap[config.category];
          if (mapped) return mapped;
        }
      }
    } catch {
      // Fall back to pattern matching if config not available
    }
    
    // Fallback: Pattern-based categorization
    const id = presetId.toLowerCase();
    const name = presetName.toLowerCase();
    
    // Event Creation animations
    if (
      id.includes("event-creation") ||
      id.includes("create-event") ||
      id.includes("add-event") ||
      id.includes("event-expand") ||
      id.includes("event-grow") ||
      id.includes("drag-to-create") ||
      id.includes("tap-add") ||
      id.includes("day-press-popup") ||
      id.includes("slide-in-event-creator") ||
      id.includes("drop-event-into-day") ||
      id.includes("select-window-pulse") ||
      id.includes("event-bubble-rise")
    ) {
      return "event-creation";
    }
    
    // Availability animations
    if (
      id.includes("available") ||
      id.includes("availability") ||
      id.includes("faint-checkmark") ||
      id.includes("priority-date-glow") ||
      id.includes("morning-evening")
    ) {
      return "availability";
    }
    
    // Booked Days animations
    if (
      id.includes("booked") ||
      id.includes("conflict") ||
      id.includes("blocked") ||
      id.includes("full-day") ||
      id.includes("gap-rule") ||
      id.includes("overlap") ||
      id.includes("holiday-seal")
    ) {
      return "booked-days";
    }
    
    // Status Indicators
    if (
      id.includes("status") ||
      id.includes("indicator") ||
      id.includes("pending") ||
      id.includes("event-created") ||
      id.includes("event-denied") ||
      id.includes("event-saved")
    ) {
      return "status-indicators";
    }
    
    // Navigation animations
    if (
      id.includes("month") ||
      id.includes("navigation") ||
      id.includes("header-slide") ||
      id.includes("range-highlight") ||
      id.includes("week") ||
      id.includes("swipe")
    ) {
      return "navigation";
    }
    
    // Loading & Feedback - only match if explicitly loading/feedback related
    // Don't match generic pulse/fade/slide/pop as those are too broad
    if (
      id.includes("loading") ||
      id.includes("feedback")
    ) {
      return "loading-feedback";
    }
    
    return null;
  };

  // DELETE MODAL STATE
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [animationToDelete, setAnimationToDelete] = useState<{
    id: string;
    name: string;
  } | null>(null);
  const [presetsKey, setPresetsKey] = useState(0); // Force re-render when presets change

  // Get presets (filters out deleted animations) - Re-compute when presetsKey changes
  const currentPresets = useMemo(() => getPresets(), [presetsKey]);

  // ANIMATION SELECTION STATE - Independent from preset filter
  const [selectedId, setSelectedId] = useState<AnimationPresetId>(
    currentPresets.length > 0 ? currentPresets[currentPresets.length - 1].id : "pulseCircle"
  );

  // UI STATE - Independent from both above
  const [showCode, setShowCode] = useState(false);
  const [copied, setCopied] = useState(false);
  const [newSettings, setNewSettings] = useState<NewSettings>(DEFAULT_NEW_SETTINGS);
  const [showSettings, setShowSettings] = useState(false);
  
  // Expanded description state (tracks which preset has description expanded)
  const [expandedDescriptionId, setExpandedDescriptionId] = useState<string | null>(null);
  
  // Calendar panel collapse state
  const [isCalendarPanelCollapsed, setIsCalendarPanelCollapsed] = useState(false);

  // Load new settings on mount
  useEffect(() => {
    const loaded = loadNewSettings();
    setNewSettings(loaded);
    setReady(true);
  }, []);

  // SINGLE SOURCE OF TRUTH: Use the shared hook for filtering
  // This is the ONLY place where filtering happens
  const { visiblePresets: categoryFilteredPresets } = useFilteredPresets(
    currentPresets,
    selectedCategory === "new" ? "all" : selectedCategory
  );
  
  // Apply GFC Website second-level filter if GFC Website tab is active
  // OR apply "new" filter if "new" tab is active
  const isGfcWebsiteTab = selectedCategory === "gfc-website";
  const isNewTab = selectedCategory === "new";
  const isCalendarFilter = gfcTypeFilter === "calendar" || gfcTypeFilter === "calendar-event-creation";
  // Hide calendar panel when viewing event creation animations (they don't need the interactive calendar)
  const shouldShowCalendarPanel = isCalendarFilter && calendarSubcategoryFilter !== "event-creation";
  
  const visiblePresets = useMemo(() => {
    let basePresets: HookAnimationPreset[];

    // Handle "new" tab filtering
    if (isNewTab) {
      basePresets = currentPresets.filter((p) => isPresetNew(p, currentPresets, newSettings, activeTemplate));
    } else if (isGfcWebsiteTab && gfcTypeFilter !== "all") {
      // Handle GFC Website second-level filter
      basePresets = categoryFilteredPresets.filter((p) => p.gfcType === gfcTypeFilter);
      
      // Apply calendar subcategory filter if calendar type is selected
      if (isCalendarFilter && calendarSubcategoryFilter !== "all") {
        basePresets = basePresets.filter((p) => {
          const subcat = getCalendarSubcategory(p.id, p.name);
          // Only include presets that match the selected subcategory
          return subcat === calendarSubcategoryFilter;
        });
      }
      
      // Phase C7: Apply template-enabled filter for calendar animations
      if (isCalendarFilter && activeTemplate) {
        basePresets = basePresets.filter((p) => {
          // Only filter calendar animations (gfcType === "calendar" or "calendar-event-creation")
          if (p.gfcType === "calendar" || p.gfcType === "calendar-event-creation") {
            return isAnimationEnabledInTemplate(activeTemplate, p.id);
          }
          // Non-calendar animations are not affected by templates
          return true;
        });
      }
    } else {
      basePresets = categoryFilteredPresets;
    }

    // Always filter out deleted presets (client-side persistent deletion)
    return basePresets.filter((p) => !deletedPresetIds.includes(p.id));
  }, [
    categoryFilteredPresets, 
    isGfcWebsiteTab, 
    isNewTab, 
    gfcTypeFilter, 
    calendarSubcategoryFilter,
    isCalendarFilter,
    currentPresets, 
    newSettings, 
    deletedPresetIds,
    activeTemplate
    // Note: getCalendarSubcategory is a pure function (doesn't depend on state/props),
    // so it doesn't need to be in the dependency array
  ]);
  
  // Reset GFC type filter when switching away from GFC Website tab
  // Restore last selected filter when returning to GFC Website tab
  useEffect(() => {
    if (!isGfcWebsiteTab) {
      // Save current filter state before leaving
      setLastGfcTypeFilter(gfcTypeFilter);
      setLastCalendarSubcategoryFilter(calendarSubcategoryFilter);
      setGfcTypeFilter("all");
      setCalendarSubcategoryFilter("all");
    } else {
      // Restore last selected filter when returning
      setGfcTypeFilter(lastGfcTypeFilter);
      if (gfcTypeFilter === "calendar" || gfcTypeFilter === "calendar-event-creation") {
        setCalendarSubcategoryFilter(lastCalendarSubcategoryFilter);
      }
    }
  }, [isGfcWebsiteTab]);

  // Phase 6.xA: Get animation from new registry (fallback to old system for compatibility)
  // AnimationEditorPanel expects AnimationConfig type, so we need to convert or use the old system
  const selectedAnimation = getCoreAnimationById(selectedId) ?? null;

  const selectedPreset = useMemo(() => {
    // Always use ID-based lookup - never rely on array position
    // First check if selectedId exists in visible presets (current category)
    const inVisible = visiblePresets.find((p) => p.id === selectedId);
    if (inVisible) return inVisible;
    
    // If not in visible, check if it exists in all presets (might be in different category)
    const inAllPresets = currentPresets.find((p) => p.id === selectedId);
    if (inAllPresets) {
      // Selected animation exists but is not in current category
      // Return first visible preset instead
      return visiblePresets[0] || null;
    }
    
    // Selected ID doesn't exist at all (might be deleted or invalid)
    // Return first visible preset or null
    return visiblePresets[0] || null;
  }, [selectedId, visiblePresets, currentPresets]);

  // Handle delete button click
  const handleDeleteClick = (e: React.MouseEvent, preset: HookAnimationPreset) => {
    e.stopPropagation(); // Prevent tile selection
    setAnimationToDelete({ id: preset.id, name: preset.name });
    setDeleteModalOpen(true);
  };

  // Handle delete confirmation
  const handleDeleteConfirm = async () => {
    if (!animationToDelete) return;

    const result = await deleteAnimation(animationToDelete.id);
    
    if (result.success) {
      // Persist deleted preset ID locally so it stays deleted across reloads
      setDeletedPresetIds((prev) => {
        if (prev.includes(animationToDelete.id)) return prev;
        const next = [...prev, animationToDelete.id];
        try {
          if (typeof window !== "undefined") {
            window.localStorage.setItem("ap_deletedPresetIds_v1", JSON.stringify(next));
          }
        } catch {
          // Ignore storage errors
        }
        return next;
      });

      // Force re-computation of presets (this will filter out the deleted animation)
      setPresetsKey((prev) => prev + 1);
      
      // If deleted animation was selected, select a different one from current category
      // The useEffect watching visiblePresets will handle the selection update
      // But we can proactively clear it here to prevent stale preview
      if (selectedId === animationToDelete.id) {
        // Clear selection - useEffect will set it to first visible preset
        setSelectedId("" as AnimationPresetId);
      }
      
      setDeleteModalOpen(false);
      setAnimationToDelete(null);
    } else {
      console.error("Failed to delete animation:", result.message);
      // You could show an error toast here
    }
  };

  // Handle delete cancel
  const handleDeleteCancel = () => {
    setDeleteModalOpen(false);
    setAnimationToDelete(null);
  };

  // Reset selectedId when category changes OR when visible presets change
  // This ensures the viewer always shows an animation from the current category
  // and prevents stale selections from previous categories
  useEffect(() => {
    // Always reset to first visible preset when category/filter changes
    // This prevents color bleed and ensures clean state transitions
    if (visiblePresets.length > 0) {
      const firstVisibleId = visiblePresets[0].id as AnimationPresetId;
      // Only update if current selection is not in visible list or is invalid/temporary
      const isInvalid = !selectedId || selectedId.toString().startsWith("__temp_");
      if (isInvalid || !visiblePresets.some((p) => p.id === selectedId)) {
        setSelectedId(firstVisibleId);
      }
    }
    // Don't auto-select from all presets when visiblePresets is empty
    // This allows the empty state UI to show with navigation buttons
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedCategory, gfcTypeFilter, calendarSubcategoryFilter, visiblePresets.length]); // Reset on category/filter changes

  // Calendar interaction handlers (Phase C5)
  const handleCalendarMonthChange = useCallback((month: Date) => {
    setCalendarContexts((prev) => {
      const updated = new Map(prev);
      const context = updated.get(selectedCalendarId);
      if (context) {
        updated.set(selectedCalendarId, {
          ...context,
          currentMonth: month,
          selection: {
            startDate: null,
            endDate: null,
            selectedDates: new Set(),
          },
        });
      }
      return updated;
    });
    
    // Trigger navigation animation
    const navAnimation = getAnimationForNavigation();
    if (navAnimation) {
      setSelectedId(navAnimation);
    }
  }, [selectedCalendarId, getAnimationForNavigation]);

  const handleCalendarSelectionChange = useCallback((selection: CalendarSelection) => {
    setCalendarContexts((prev) => {
      const updated = new Map(prev);
      const context = updated.get(selectedCalendarId);
      if (context) {
        updated.set(selectedCalendarId, {
          ...context,
          selection,
        });
      }
      return updated;
    });
  }, [selectedCalendarId]);

  const handleCalendarDayClick = useCallback((date: Date, state: CalendarDayState) => {
    // If "event-creation" filter is active, use event creation animation
    if (calendarSubcategoryFilter === "event-creation" && state === "available") {
      const animation = getAnimationForEventCreation();
      if (animation) {
        setSelectedId(animation);
        return;
      }
    }
    
    // Otherwise use the default day state animation
    const animation = getAnimationForDayState(state);
    if (animation) {
      setSelectedId(animation);
    }
  }, [getAnimationForDayState, getAnimationForEventCreation, calendarSubcategoryFilter]);

  const handleCalendarDayHover = useCallback((date: Date | null, state: CalendarDayState | null) => {
    // Optional: Could trigger hover animations here
  }, []);

  const handleCalendarWeekClick = useCallback((weekStart: Date) => {
    const animation = getAnimationForWeek();
    if (animation) {
      setSelectedId(animation);
    }
  }, [getAnimationForWeek]);

  const handleCalendarChange = useCallback((calendarId: string) => {
    setSelectedCalendarId(calendarId);
    // Reset selection when switching calendars
    const context = calendarContexts.get(calendarId);
    if (context) {
      setCalendarContexts((prev) => {
        const updated = new Map(prev);
        const ctx = updated.get(calendarId);
        if (ctx) {
          updated.set(calendarId, {
            ...ctx,
            selection: {
              startDate: null,
              endDate: null,
              selectedDates: new Set(),
            },
          });
        }
        return updated;
      });
    }
  }, [calendarContexts]);

  // Phase C7: Handle template change for a calendar
  const handleTemplateChange = useCallback((calendarId: string, templateId: string) => {
    setCalendarTemplates((prev) => {
      const updated = new Map(prev);
      updated.set(calendarId, templateId);
      return updated;
    });
  }, []);

  // Helper to navigate back to a safe category when stuck in empty state
  const navigateToSafeCategory = useCallback(() => {
    // Try to find a category with animations
    const categoriesWithAnimations = USAGE_CATEGORIES.filter((cat) => {
      if (cat.id === "all") return currentPresets.length > 0;
      if (cat.id === "new") {
        return currentPresets.some((p) => isPresetNew(p, currentPresets, newSettings, activeTemplate));
      }
      return currentPresets.some((p) => p.usageCategory === cat.id);
    });

    if (categoriesWithAnimations.length > 0) {
      const safeCategory = categoriesWithAnimations[0].id;
      setSelectedCategory(safeCategory);
      // Reset filters
      setGfcTypeFilter("all");
      setCalendarSubcategoryFilter("all");
      // Select first preset from safe category
      const safePresets = currentPresets.filter((p) => {
        if (safeCategory === "all") return true;
        if (safeCategory === "new") return isPresetNew(p, currentPresets, newSettings, activeTemplate);
        return p.usageCategory === safeCategory;
      });
      if (safePresets.length > 0) {
        setSelectedId(safePresets[0].id as AnimationPresetId);
      }
    }
  }, [currentPresets, newSettings, activeTemplate]);

  const handleCopy = async () => {
    if (!selectedPreset || !selectedPreset.code) return;
    try {
      await navigator.clipboard.writeText(selectedPreset.code);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error("Failed to copy code:", err);
    }
  };

  if (!ready) {
    return (
      <main className="flex min-h-screen flex-col bg-slate-950 text-slate-100">
        <div className="flex flex-1 items-center justify-center text-sm text-slate-400">
          Loading Animation Playground…
        </div>
      </main>
    );
  }

  // Don't early return - let the empty state UI handle it with navigation buttons

  return (
    <main className="flex h-screen w-full overflow-hidden bg-gradient-to-br from-slate-950 via-slate-900 to-slate-950 text-slate-100">
      {/* Animated background glow effects */}
      <div className="pointer-events-none fixed inset-0 -z-10">
        <motion.div
          className="absolute -left-20 top-10 h-96 w-96 rounded-full bg-indigo-500/20 blur-3xl"
          animate={{
            x: [0, 50, 0],
            y: [0, 30, 0],
            scale: [1, 1.1, 1],
          }}
          transition={{
            duration: 20,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        />
        <motion.div
          className="absolute bottom-0 right-0 h-96 w-96 rounded-full bg-cyan-500/15 blur-3xl"
          animate={{
            x: [0, -30, 0],
            y: [0, -20, 0],
            scale: [1, 1.15, 1],
          }}
          transition={{
            duration: 25,
            repeat: Infinity,
            ease: "easeInOut",
          }}
        />
        <div className="absolute left-1/2 top-1/2 h-[600px] w-[600px] -translate-x-1/2 -translate-y-1/2 rounded-full bg-violet-500/5 blur-3xl" />
      </div>

      {/* Slim top navigation bar */}
      <motion.header
        initial={{ y: -20, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        transition={{ duration: 0.5 }}
        className="absolute top-0 left-0 right-0 z-50 border-b border-slate-800/50 bg-slate-950/80 backdrop-blur-xl"
      >
        <div className="mx-auto flex max-w-full items-center justify-between gap-4 px-4 py-3 sm:px-6 lg:px-8">
          <div className="flex items-center gap-3">
            <Link
              href="/"
              className="flex items-center gap-2 rounded-lg px-2.5 py-1.5 text-sm font-semibold text-slate-100 transition-all hover:bg-slate-800/50 hover:text-violet-300 group"
            >
              <ArrowLeft className="h-4 w-4 transition-transform group-hover:-translate-x-0.5" />
              <Sparkles className="h-4 w-4" />
              <span className="hidden sm:inline">Animation Playground</span>
              <span className="sm:hidden">Home</span>
            </Link>
          </div>
          <nav className="flex items-center gap-1">
            <Link
              href="/"
              className="flex items-center gap-1.5 rounded-lg px-3 py-1.5 text-xs font-medium text-slate-300 transition-colors hover:bg-slate-800/50 hover:text-slate-100"
            >
              <Home className="h-3.5 w-3.5" />
              <span className="hidden sm:inline">Home</span>
            </Link>
            <Link
              href="/animations"
              className="flex items-center gap-1.5 rounded-lg px-3 py-1.5 text-xs font-medium text-slate-300 transition-colors hover:bg-slate-800/50 hover:text-slate-100"
            >
              <BookOpen className="h-3.5 w-3.5" />
              <span className="hidden sm:inline">Library</span>
            </Link>
          </nav>
        </div>
      </motion.header>

      {/* Main content area - Fixed height, no scroll */}
      <div className="flex flex-1 flex-col overflow-hidden pt-16">
        <div className="mx-auto flex w-full max-w-full flex-1 gap-4 overflow-hidden px-4 py-6 sm:px-6 lg:px-8">
          {/* Grid layout: Left (presets/admin) + Right (viewer + editor) */}
          <div className={`grid w-full flex-1 gap-4 min-h-0 ${
            selectedCategory === "admin" 
              ? "lg:grid-cols-1" 
              : "lg:grid-cols-[minmax(0,1.4fr)_minmax(0,2fr)] xl:grid-cols-[minmax(0,1.4fr)_minmax(0,2.2fr)]"
          }`}>
            {/* LEFT: Presets + tiles OR Admin Panel */}
            <div className="flex flex-col gap-3 min-h-0">
              {/* Admin Panel - shown when Admin tab is selected */}
              {selectedCategory === "admin" ? (
                <motion.div
                  initial={{ opacity: 0, x: -20 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ duration: 0.6, delay: 0.3 }}
                  className="flex h-full flex-col overflow-hidden rounded-2xl border border-slate-800/80 bg-gradient-to-br from-slate-900/90 to-slate-950/90 shadow-2xl shadow-black/50 backdrop-blur-sm"
                >
                  <CalendarAnimationAdminPanel
                    onConfigChange={() => {
                      // Force re-computation of presets when config changes
                      setPresetsKey((prev) => prev + 1);
                    }}
                  />
                </motion.div>
              ) : (
                /* Preset selector - Fixed width, category buttons fixed, list scrolls */
                <motion.aside
                  initial={{ opacity: 0, x: -20 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ duration: 0.6, delay: 0.3 }}
                  className="flex h-full flex-col overflow-hidden"
                >
            <div className="flex h-full flex-col rounded-2xl border border-slate-800/80 bg-gradient-to-br from-slate-900/90 to-slate-950/90 shadow-2xl shadow-black/50 backdrop-blur-sm">
              {/* Compact Header - FIXED, NO SCROLL */}
              <div className="flex-shrink-0 border-b border-slate-800/50 bg-slate-900/50">
                {/* Top bar with title and settings */}
                <div className="flex items-center justify-between px-4 py-2.5">
                  <div className="flex items-center gap-2.5">
                    <div className="flex h-7 w-7 items-center justify-center rounded-md border border-slate-700/50 bg-slate-800/50">
                      <LayoutTemplate className="h-3.5 w-3.5 text-slate-300" />
                    </div>
                    <div>
                      <h3 className="text-xs font-semibold text-slate-100 leading-tight">Animation Presets</h3>
                      <p className="text-[10px] text-slate-500 leading-tight">
                        {ready
                          ? `${visiblePresets.length} of ${currentPresets.length}`
                          : "…"}
                      </p>
                    </div>
                  </div>
                  <button
                    onClick={() => setShowSettings(true)}
                    className="flex h-7 w-7 items-center justify-center rounded-md border border-slate-700/50 bg-slate-800/50 text-slate-400 transition-all hover:bg-slate-700/50 hover:text-slate-200 hover:border-slate-600/50"
                    title="Settings"
                  >
                    <Settings className="h-3.5 w-3.5" />
                  </button>
                </div>

                {/* Category filter tabs - Compact design */}
                <div className="flex flex-wrap gap-1.5 px-4 pb-3">
                  {USAGE_CATEGORIES.map((category) => {
                    const isActive = selectedCategory === category.id;
                    // Compute count from currentPresets - filter creates new array, no mutation
                    const count =
                      category.id === "all"
                        ? currentPresets.length
                        : category.id === "new"
                        ? currentPresets.filter((p) => isPresetNew(p, currentPresets, newSettings, activeTemplate)).length
                        : currentPresets.filter((p) => p.usageCategory === category.id).length;

                    if (count === 0 && category.id !== "all") return null;

                    return (
                      <button
                        key={category.id}
                        onClick={() => {
                          // Pure handler - only updates category filter state
                          // Does NOT affect selectedId or any other state
                          setSelectedCategory(category.id);
                        }}
                        className={`group relative flex items-center gap-1 rounded-md border px-2.5 py-1 text-[11px] font-medium transition-all ${
                          isActive
                            ? "border-violet-500/60 bg-violet-500/20 text-violet-200 shadow-sm shadow-violet-900/20"
                            : "border-slate-700/40 bg-slate-800/40 text-slate-400 hover:border-slate-600/50 hover:bg-slate-800/60 hover:text-slate-300"
                        }`}
                      >
                        <span className="text-xs leading-none">{category.icon}</span>
                        <span className="leading-tight">{category.label}</span>
                        <span
                          className={`rounded-full px-1.5 py-0.5 text-[9px] font-semibold leading-none ${
                            isActive
                              ? "bg-violet-500/30 text-violet-200"
                              : "bg-slate-700/40 text-slate-500"
                          }`}
                        >
                          {ready ? count : "–"}
                        </span>
                      </button>
                    );
                  })}
                </div>

                {/* GFC Website second-level filter - only visible when GFC Website tab is active */}
                {isGfcWebsiteTab && (
                  <div className="border-t border-slate-800/50 bg-slate-900/30">
                    {/* Primary GFC type filters */}
                    <div className="flex flex-wrap gap-1.5 overflow-x-auto px-4 py-2.5">
                      {[
                        { id: "all" as const, label: "All", icon: "✨" },
                        { id: "hero-banner" as const, label: "Heroes & Banners", icon: "🎯" },
                        { id: "cta-button" as const, label: "CTA Buttons", icon: "🔘" },
                        { id: "card-tile" as const, label: "Cards & Tiles", icon: "🎴" },
                        { id: "section-layout" as const, label: "Sections & Layout", icon: "📐" },
                        { id: "banner-status" as const, label: "Banners & Status", icon: "📢" },
                        { id: "branding-impact" as const, label: "Branding & Impact", icon: "💎" },
                        { id: "flags-patriotic" as const, label: "Flags & Patriotic", icon: "🏳️" },
                        { id: "calendar" as const, label: "Calendar", icon: "📅" },
                        { id: "calendar-event-creation" as const, label: "Event Creation", icon: "➕" },
                      ]
                        .filter((type) => {
                          // Always show "all" and "calendar-event-creation"
                          if (type.id === "all" || type.id === "calendar-event-creation") return true;
                          // For other types, only show if count > 0
                          const count =
                            categoryFilteredPresets.filter((p) => p.gfcType === type.id).length;
                          return count > 0;
                        })
                        .map((type) => {
                          const isActive = gfcTypeFilter === type.id;
                          // Count presets for this type (only count those with gfcType set)
                          const count =
                            type.id === "all"
                              ? categoryFilteredPresets.length
                              : categoryFilteredPresets.filter((p) => p.gfcType === type.id).length;

                        return (
                          <button
                            key={type.id}
                            onClick={(e) => {
                              e.preventDefault();
                              e.stopPropagation();
                              setGfcTypeFilter(type.id);
                              // Reset calendar subcategory when switching away from calendar
                              if (type.id !== "calendar" && type.id !== "calendar-event-creation") {
                                setCalendarSubcategoryFilter("all");
                              } else if (type.id === "calendar") {
                                // When switching to calendar, reset to "all" subcategory
                                setCalendarSubcategoryFilter("all");
                              }
                            }}
                            className={`group relative flex items-center gap-1 rounded-md border px-2.5 py-1 text-[11px] font-medium transition-all ${
                              isActive
                                ? "border-amber-500/60 bg-amber-500/20 text-amber-200 shadow-sm shadow-amber-900/20"
                                : "border-slate-700/40 bg-slate-800/40 text-slate-400 hover:border-slate-600/50 hover:bg-slate-800/60 hover:text-slate-300"
                            }`}
                          >
                            <span className="text-xs leading-none">{type.icon}</span>
                            <span className="leading-tight">{type.label}</span>
                            <span
                              className={`rounded-full px-1.5 py-0.5 text-[9px] font-semibold leading-none ${
                                isActive
                                  ? "bg-amber-500/30 text-amber-200"
                                  : "bg-slate-700/40 text-slate-500"
                              }`}
                            >
                              {count}
                            </span>
                          </button>
                        );
                      })}
                    </div>

                    {/* Multi-calendar selector - only visible when calendar type is selected, but hidden for event creation */}
                    {shouldShowCalendarPanel && (
                      <>
                        <div className="flex items-center gap-2 px-4 py-2 border-t border-slate-800/50 bg-slate-900/20">
                          <span className="text-[10px] font-medium text-slate-500 uppercase tracking-wide">Calendar:</span>
                          <select
                            value={selectedCalendarId}
                            onChange={(e) => handleCalendarChange(e.target.value)}
                            className="flex-1 rounded-md border border-slate-700/50 bg-slate-800/50 px-2.5 py-1 text-[11px] font-medium text-slate-200 transition-colors hover:bg-slate-800/70 focus:border-cyan-500/60 focus:outline-none"
                          >
                            {CALENDAR_CONTEXTS.map((cal) => (
                              <option key={cal.id} value={cal.id}>
                                {cal.name}
                              </option>
                            ))}
                          </select>
                        </div>
                        
                        {/* Phase C7: Template selector */}
                        <div className="px-4 py-2 border-t border-slate-800/50 bg-slate-900/20">
                          <div className="mb-2">
                            <span className="text-[10px] font-medium text-slate-500 uppercase tracking-wide">Template:</span>
                          </div>
                          {(() => {
                            const favorites = getFavoriteTemplates();
                            const applicableTemplates = getTemplatesForCalendar(selectedCalendarId);
                            const currentTemplateId = calendarTemplates.get(selectedCalendarId) || "";
                            
                            return (
                              <div className="space-y-2">
                                {favorites.length > 0 && favorites.some(t => applicableTemplates.some(at => at.id === t.id)) && (
                                  <div className="flex flex-wrap gap-1.5">
                                    {favorites
                                      .filter(t => applicableTemplates.some(at => at.id === t.id))
                                      .map((template) => (
                                        <button
                                          key={template.id}
                                          onClick={(e) => {
                                            e.preventDefault();
                                            e.stopPropagation();
                                            handleTemplateChange(selectedCalendarId, template.id);
                                          }}
                                          className={`flex items-center gap-1 rounded-md border px-2 py-1 text-[10px] font-medium transition-all ${
                                            currentTemplateId === template.id
                                              ? "border-amber-500/60 bg-amber-500/20 text-amber-200"
                                              : "border-slate-700/40 bg-slate-800/40 text-slate-400 hover:border-slate-600/50 hover:bg-slate-800/60 hover:text-slate-300"
                                          }`}
                                        >
                                          <span className="text-xs">⭐</span>
                                          <span>{template.name}</span>
                                        </button>
                                      ))}
                                  </div>
                                )}
                                {/* All templates dropdown */}
                                <select
                                  value={currentTemplateId}
                                  onChange={(e) => {
                                    handleTemplateChange(selectedCalendarId, e.target.value);
                                  }}
                                  className="w-full rounded-md border border-slate-700/50 bg-slate-800/50 px-2.5 py-1 text-[11px] font-medium text-slate-200 transition-colors hover:bg-slate-800/70 focus:border-cyan-500/60 focus:outline-none"
                                >
                                  {applicableTemplates.map((template) => (
                                    <option key={template.id} value={template.id}>
                                      {template.isFavorite ? "⭐ " : ""}{template.name}
                                    </option>
                                  ))}
                                </select>
                              </div>
                            );
                          })()}
                        </div>
                      </>
                    )}
                    
                    {/* Calendar subcategory filters - only visible when calendar type is selected */}
                    {isCalendarFilter && (
                      <div className="flex flex-wrap gap-1.5 overflow-x-auto px-4 py-2.5 border-t border-slate-800/50 bg-slate-900/20">
                        {[
                          { id: "all" as const, label: "All", icon: "✨" },
                          { id: "availability" as const, label: "Calendar Availability", icon: "✅" },
                          { id: "booked-days" as const, label: "Calendar Booked Days", icon: "🔒" },
                          { id: "event-creation" as const, label: "Calendar Add Event", icon: "➕" },
                          { id: "status-indicators" as const, label: "Calendar Status Indicators", icon: "📊" },
                          { id: "navigation" as const, label: "Calendar Navigation", icon: "🧭" },
                          { id: "loading-feedback" as const, label: "Calendar Loading & Feedback", icon: "⏳" },
                        ]
                          .map((subcat) => {
                            const isActive = calendarSubcategoryFilter === subcat.id;
                            // Count presets for this subcategory
                            let count: number;
                            if (subcat.id === "all") {
                              count = categoryFilteredPresets.filter((p) => 
                                p.gfcType === gfcTypeFilter
                              ).length;
                            } else {
                              count = categoryFilteredPresets.filter((p) => {
                                if (p.gfcType !== gfcTypeFilter) return false;
                                const cat = getCalendarSubcategory(p.id, p.name);
                                return cat === subcat.id;
                              }).length;
                            }
                            return { ...subcat, count, isActive };
                          })
                          .filter((subcat) => {
                            // Always show "All" button, even if count is 0
                            if (subcat.id === "all") return true;
                            // Always show subcategory buttons when calendar filter is active (even if count is 0)
                            // This allows users to see all available categories
                            return true;
                          })
                          .map((subcat) => {
                          return (
                            <button
                              key={subcat.id}
                              onClick={(e) => {
                                e.preventDefault();
                                e.stopPropagation();
                                setCalendarSubcategoryFilter(subcat.id);
                              }}
                              className={`group relative flex items-center gap-1 rounded-md border px-2.5 py-1 text-[11px] font-medium transition-all ${
                                subcat.isActive
                                  ? "border-cyan-500/60 bg-cyan-500/20 text-cyan-200 shadow-sm shadow-cyan-900/20"
                                  : "border-slate-700/40 bg-slate-800/40 text-slate-400 hover:border-slate-600/50 hover:bg-slate-800/60 hover:text-slate-300"
                              }`}
                            >
                              <span className="text-xs leading-none">{subcat.icon}</span>
                              <span className="leading-tight">{subcat.label}</span>
                              <span
                                className={`rounded-full px-1.5 py-0.5 text-[9px] font-semibold leading-none ${
                                  subcat.isActive
                                    ? "bg-cyan-500/30 text-cyan-200"
                                    : "bg-slate-700/40 text-slate-500"
                                }`}
                              >
                                {subcat.count}
                              </span>
                            </button>
                          );
                        })}
                      </div>
                    )}
                  </div>
                )}

              </div>

              {/* Scrollable preset list - ONLY THIS SCROLLS */}
              {/* CRITICAL: This div ONLY renders visiblePresets from the hook - no other source */}
              <div className="flex-1 overflow-y-auto p-4 min-h-0 pr-2 scroll-smooth" style={{ scrollBehavior: 'smooth' }}>
                {ready ? (
                  <div className="space-y-1.5">
                    {/* AnimatePresence ensures smooth transitions when items are added/removed */}
                    <AnimatePresence mode="popLayout" initial={false}>
                      {/* ONLY render presets from visiblePresets - this is the single source of truth */}
                      {visiblePresets.map((preset) => {
                        const isActive = preset.id === selectedId;
                        // Tile colors are ONLY based on selection state, not difficulty/category

                        return (
                          <motion.div
                            key={preset.id}
                            role="button"
                            tabIndex={0}
                            initial={{ opacity: 0, x: -10 }}
                            animate={{ opacity: 1, x: 0 }}
                            exit={{ opacity: 0, x: 10, height: 0, marginBottom: 0 }}
                            transition={{ duration: 0.2 }}
                            onClick={() => {
                              // Pure handler - only updates selected animation
                              // Does NOT affect selectedCategory or visiblePresets
                              setSelectedId(preset.id as AnimationPresetId);
                            }}
                            onKeyDown={(e) => {
                              if (e.key === "Enter" || e.key === " ") {
                                e.preventDefault();
                                setSelectedId(preset.id as AnimationPresetId);
                              }
                            }}
                            className={`group relative w-full text-left transition-all cursor-pointer ${
                              isActive ? "scale-[1.02]" : "hover:scale-[1.01]"
                            }`}
                            style={{ minHeight: '100px' }}
                          >
                            {/* Tile container - standardized states: normal, hovered, selected */}
                            <div
                              className={`relative overflow-hidden rounded-lg border p-3 transition-all duration-200 ${
                                isActive
                                  ? "border-violet-400/60 bg-violet-600/25 shadow-md shadow-violet-900/30"
                                  : "border-slate-700/40 bg-slate-800/30 hover:border-slate-600/50 hover:bg-slate-800/40"
                              }`}
                            >
                              {/* NEW badge */}
                              {isPresetNew(preset, currentPresets, newSettings, activeTemplate) && (
                                <motion.div
                                  initial={{ scale: 0, opacity: 0 }}
                                  animate={{ scale: 1, opacity: 1 }}
                                  className="absolute right-2 top-2 z-10 rounded-full bg-emerald-500 px-2 py-0.5 text-xs font-bold text-slate-900 shadow-lg"
                                >
                                  NEW
                                </motion.div>
                              )}

                              {/* Active indicator overlay */}
                              {isActive && (
                                <motion.div
                                  layoutId="activePreset"
                                  className="absolute inset-0 rounded-xl bg-gradient-to-br from-violet-500/10 to-transparent"
                                  transition={{ type: "spring", bounce: 0.2, duration: 0.6 }}
                                />
                              )}

                              <div className="relative flex items-start gap-3">
                                {/* Icon container - consistent styling */}
                                <div
                                  className={`mt-0.5 flex h-9 w-9 shrink-0 items-center justify-center rounded-lg border transition-colors ${
                                    isActive
                                      ? "border-violet-500/50 bg-violet-500/20"
                                      : "border-slate-700/50 bg-slate-800/50 group-hover:border-slate-600/50"
                                  }`}
                                >
                                  <MonitorPlay
                                    className={`h-4 w-4 transition-colors ${
                                      isActive ? "text-violet-300" : "text-slate-400"
                                    }`}
                                  />
                                </div>
                                <div className="flex-1 min-w-0">
                                  <div className="flex items-start justify-between gap-2">
                                    <h4
                                      className={`text-sm font-semibold transition-colors ${
                                        isActive ? "text-violet-100" : "text-slate-100"
                                      }`}
                                    >
                                    {preset.name ?? "Untitled"}
                                    </h4>
                                    <div className="flex items-center gap-1.5 shrink-0">
                                      {/* Difficulty badge - small pill, doesn't affect tile background */}
                                      <span
                                        className={`rounded-full border px-2 py-0.5 text-[10px] font-semibold uppercase tracking-wider ${
                                          isActive
                                            ? "border-violet-500/50 bg-violet-500/20 text-violet-200"
                                            : "border-slate-700/50 bg-slate-800/50 text-slate-400"
                                        }`}
                                      >
                                      {preset.difficulty}
                                      </span>
                                      
                                      {/* Action buttons - appear on hover, positioned next to difficulty badge */}
                                      <div className="flex gap-1 opacity-0 transition-all group-hover:opacity-100">
                                        {/* Info/Description toggle button */}
                                        <button
                                          type="button"
                                          onClick={(e) => {
                                            e.stopPropagation();
                                            setExpandedDescriptionId(
                                              expandedDescriptionId === preset.id ? null : preset.id
                                            );
                                          }}
                                          className="rounded-lg p-1.5 text-slate-400 transition-all hover:bg-cyan-500/20 hover:text-cyan-400"
                                          title={expandedDescriptionId === preset.id ? "Hide description" : "Show description"}
                                        >
                                          {expandedDescriptionId === preset.id ? (
                                            <ChevronUp className="h-3.5 w-3.5" />
                                          ) : (
                                            <Info className="h-3.5 w-3.5" />
                                          )}
                                        </button>
                                        
                                        {/* Delete button */}
                                        <button
                                          type="button"
                                          onClick={(e) => {
                                            e.stopPropagation();
                                            handleDeleteClick(e, preset);
                                          }}
                                          className="rounded-md p-1 text-slate-400 transition-all hover:bg-red-500/20 hover:text-red-400"
                                          title="Delete animation"
                                        >
                                          <Trash2 className="h-3 w-3" />
                                        </button>
                                      </div>
                                    </div>
                                  </div>
                                  <p
                                    className={`mt-1 line-clamp-2 text-xs ${
                                      isActive ? "text-violet-300/80" : "text-slate-400"
                                    }`}
                                  >
                                  {preset.shortDescription}
                                  </p>
                                  <div className="mt-2 flex flex-wrap gap-1.5">
                                    {preset.tags.map((tag) => (
                                      <span
                                        key={tag}
                                        className={`rounded-full border px-2 py-0.5 text-[10px] font-medium ${
                                          isActive
                                            ? "border-slate-700/50 bg-slate-800/50 text-slate-300"
                                            : "border-slate-800/50 bg-slate-900/50 text-slate-500"
                                        }`}
                                      >
                                        {tag}
                                      </span>
                                    ))}
                                  </div>
                                  
                                  {/* Expanded description section */}
                                  {expandedDescriptionId === preset.id && (
                                    <motion.div
                                      initial={{ opacity: 0, height: 0 }}
                                      animate={{ opacity: 1, height: "auto" }}
                                      exit={{ opacity: 0, height: 0 }}
                                      transition={{ duration: 0.2 }}
                                      className="mt-3 space-y-2 border-t border-slate-700/50 pt-3"
                                    >
                                      <div>
                                        <h5 className="mb-1 text-xs font-semibold text-slate-300">Description</h5>
                                        <p className="text-xs leading-relaxed text-slate-400">
                                          {preset.longDescription || preset.shortDescription}
                                        </p>
                                      </div>
                                      <div>
                                        <h5 className="mb-1 text-xs font-semibold text-slate-300">How to Use</h5>
                                        <p className="text-xs leading-relaxed text-slate-400">
                                          {getUsageInstructions(preset as AnimationPreset)}
                                        </p>
                                      </div>
                                    </motion.div>
                                  )}
                                </div>
                              </div>
                            </div>
                          </motion.div>
                        );
                      })}
                    </AnimatePresence>
                  </div>
                ) : (
                  <div className="space-y-2">
                    <div className="h-16 rounded-xl border border-slate-800/60 bg-slate-950/60" />
                    <div className="h-16 rounded-xl border border-slate-800/60 bg-slate-950/60" />
                    <div className="h-16 rounded-xl border border-slate-800/60 bg-slate-950/60" />
                  </div>
                )}
              </div>

            </div>
          </motion.aside>
              )}
            </div>

            {/* RIGHT: Viewer + Editor (hidden in admin mode) */}
            {selectedCategory !== "admin" && (
            <div className="flex flex-col gap-4 min-h-0">
              {/* Animation viewer */}
              <motion.section
                initial={{ opacity: 0, x: 20 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.6, delay: 0.2 }}
                className="relative flex min-h-[280px] flex-shrink-0 items-center justify-center rounded-xl border border-slate-800/80 bg-slate-950/90 p-4 md:min-h-[340px] md:p-6"
              >
                {/* Subtle grid pattern */}
                <div
                  className="pointer-events-none absolute inset-0 rounded-2xl opacity-[0.03]"
                  style={{
                    backgroundImage:
                      "linear-gradient(rgba(255,255,255,0.1) 1px, transparent 1px), linear-gradient(90deg, rgba(255,255,255,0.1) 1px, transparent 1px)",
                    backgroundSize: "20px 20px",
                  }}
                />
                {/* Glow effect */}
                <div className="pointer-events-none absolute inset-0 rounded-2xl bg-gradient-to-br from-violet-500/10 via-transparent to-transparent" />

                <AnimatePresence mode="wait">
                  {selectedPreset && selectedPreset.Component && visiblePresets.length > 0 ? (
                    <motion.div
                      key={selectedId}
                      initial={{ opacity: 0, scale: 0.95, y: 10 }}
                      animate={{ opacity: 1, scale: 1, y: 0 }}
                      exit={{ opacity: 0, scale: 0.95, y: -10 }}
                      transition={{ duration: 0.4, ease: "easeOut" }}
                      className="relative z-10 h-full w-full"
                    >
                      <selectedPreset.Component />
                    </motion.div>
                  ) : (
                    <motion.div
                      key="fallback"
                      initial={{ opacity: 0 }}
                      animate={{ opacity: 1 }}
                      exit={{ opacity: 0 }}
                      className="relative z-10 flex h-full w-full items-center justify-center"
                    >
                      <div className="text-center text-slate-400 text-sm md:text-base px-4 w-full max-w-md">
                        {visiblePresets.length === 0 || !selectedPreset ? (
                          <div className="space-y-4">
                            <div>
                              <p className="text-slate-300 font-medium mb-1">No animations available in this category</p>
                              <p className="text-xs text-slate-500">Try selecting a different category or filter</p>
                            </div>
                            <div className="flex flex-col gap-2 items-center">
                              {/* Always show "All Categories" button as primary escape */}
                              <button
                                onClick={() => {
                                  setSelectedCategory("all");
                                  setGfcTypeFilter("all");
                                  setCalendarSubcategoryFilter("all");
                                  // Reset to first available preset
                                  if (currentPresets.length > 0) {
                                    setSelectedId(currentPresets[0].id as AnimationPresetId);
                                  }
                                }}
                                className="rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2 text-sm font-medium text-slate-300 transition-colors hover:bg-slate-800/70 hover:text-slate-100 hover:border-slate-600/50"
                              >
                                ← Back to All Categories
                              </button>
                              
                              {/* Calendar-specific navigation */}
                              {isCalendarFilter && calendarSubcategoryFilter !== "all" && (
                                <button
                                  onClick={() => {
                                    setCalendarSubcategoryFilter("all");
                                    // Also reset to first available preset
                                    const calendarPresets = categoryFilteredPresets.filter((p) => p.gfcType === gfcTypeFilter);
                                    if (calendarPresets.length > 0) {
                                      setSelectedId(calendarPresets[0].id as AnimationPresetId);
                                    }
                                  }}
                                  className="rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2 text-sm font-medium text-slate-300 transition-colors hover:bg-slate-800/70 hover:text-slate-100 hover:border-slate-600/50"
                                >
                                  ← Back to All Calendar Animations
                                </button>
                              )}
                              
                              {/* GFC Website navigation */}
                              {isGfcWebsiteTab && gfcTypeFilter !== "all" && (
                                <button
                                  onClick={() => {
                                    setGfcTypeFilter("all");
                                    setCalendarSubcategoryFilter("all");
                                    // Reset to first available preset in GFC Website
                                    const gfcPresets = categoryFilteredPresets.filter((p) => p.usageCategory === "gfc-website");
                                    if (gfcPresets.length > 0) {
                                      setSelectedId(gfcPresets[0].id as AnimationPresetId);
                                    }
                                  }}
                                  className="rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2 text-sm font-medium text-slate-300 transition-colors hover:bg-slate-800/70 hover:text-slate-100 hover:border-slate-600/50"
                                >
                                  ← Back to All GFC Website Animations
                                </button>
                              )}
                              
                              {/* Fallback: navigate to any safe category */}
                              <button
                                onClick={navigateToSafeCategory}
                                className="rounded-lg border border-cyan-700/50 bg-cyan-900/30 px-4 py-2 text-sm font-medium text-cyan-300 transition-colors hover:bg-cyan-900/50 hover:text-cyan-200 hover:border-cyan-600/50"
                              >
                                Find Category with Animations
                              </button>
                            </div>
                          </div>
                        ) : (
                          <div className="space-y-3">
                            <p className="text-slate-300 font-medium">No animation component found</p>
                            <p className="text-xs text-slate-500">
                              ID: <span className="font-mono">{selectedId || "(none)"}</span>
                            </p>
                            <div className="flex flex-col gap-2 items-center">
                              {visiblePresets.length > 0 && (
                                <button
                                  onClick={() => {
                                    // Select first visible preset
                                    setSelectedId(visiblePresets[0].id as AnimationPresetId);
                                  }}
                                  className="rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2 text-sm font-medium text-slate-300 transition-colors hover:bg-slate-800/70 hover:text-slate-100"
                                >
                                  Select First Available Animation
                                </button>
                              )}
                              {selectedCategory !== "all" && (
                                <button
                                  onClick={() => {
                                    setSelectedCategory("all");
                                    if (currentPresets.length > 0) {
                                      setSelectedId(currentPresets[0].id as AnimationPresetId);
                                    }
                                  }}
                                  className="rounded-lg border border-slate-700/50 bg-slate-800/50 px-4 py-2 text-sm font-medium text-slate-300 transition-colors hover:bg-slate-800/70 hover:text-slate-100"
                                >
                                  ← Back to All Categories
                                </button>
                              )}
                            </div>
                          </div>
                        )}
                      </div>
                    </motion.div>
                  )}
                </AnimatePresence>
              </motion.section>

              {/* Interactive Calendar Panel - positioned below viewer, above editor */}
              {shouldShowCalendarPanel && (
                <motion.div
                  initial={{ opacity: 0, y: 10 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ duration: 0.4, delay: 0.3 }}
                  className="flex-shrink-0 rounded-xl border border-slate-800/80 bg-slate-900/50"
                >
                  <div className="flex items-center justify-between p-4 border-b border-slate-800/50">
                    <div className="flex items-center gap-2">
                      <div className="text-xs font-semibold text-slate-300 uppercase tracking-wide">Interactive Calendar</div>
                      <div className="text-xs text-slate-500">Click days to preview animations</div>
                    </div>
                    <button
                      onClick={() => setIsCalendarPanelCollapsed(!isCalendarPanelCollapsed)}
                      className="flex items-center gap-1 rounded-md px-2 py-1 text-xs text-slate-400 transition-colors hover:bg-slate-800/50 hover:text-slate-200"
                      title={isCalendarPanelCollapsed ? "Expand calendar" : "Collapse calendar"}
                    >
                      {isCalendarPanelCollapsed ? (
                        <>
                          <ChevronDown className="h-3.5 w-3.5" />
                          <span>Show</span>
                        </>
                      ) : (
                        <>
                          <ChevronUp className="h-3.5 w-3.5" />
                          <span>Hide</span>
                        </>
                      )}
                    </button>
                  </div>
                  {!isCalendarPanelCollapsed && (
                    <motion.div
                      initial={{ height: 0, opacity: 0 }}
                      animate={{ height: "auto", opacity: 1 }}
                      exit={{ height: 0, opacity: 0 }}
                      transition={{ duration: 0.2 }}
                      className="overflow-hidden"
                    >
                      <div className="p-4 max-h-[280px] overflow-y-auto">
                        <GfcCalendarInteractivePanel
                          currentMonth={currentCalendarContext.currentMonth}
                          onMonthChange={handleCalendarMonthChange}
                          selection={currentCalendarContext.selection}
                          onSelectionChange={handleCalendarSelectionChange}
                          onDayClick={handleCalendarDayClick}
                          onDayHover={handleCalendarDayHover}
                          onWeekClick={handleCalendarWeekClick}
                          dayStates={currentCalendarContext.dayStates}
                          calendarId={selectedCalendarId}
                        />
                      </div>
                    </motion.div>
                  )}
                </motion.div>
              )}

              {/* Editor panel */}
              <AnimationEditorPanel animation={selectedAnimation} />
            </div>
            )}
          </div>
        </div>
      </div>

      {/* Delete Confirmation Modal */}
      <ConfirmDeleteModal
        isOpen={deleteModalOpen}
        animationName={animationToDelete?.name || ""}
        onConfirm={handleDeleteConfirm}
        onCancel={handleDeleteCancel}
      />

      {/* Settings Dialog */}
      <PlaygroundSettingsDialog
        isOpen={showSettings}
        onClose={() => setShowSettings(false)}
        newSettings={newSettings}
        onChangeNewSettings={(settings) => {
          setNewSettings(settings);
          saveNewSettings(settings);
        }}
      />
    </main>
  );
}
