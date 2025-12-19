# Animation Registry System

**REV: AnimationPlayground/Phase-6.xA/TemplateSystem**

## Overview

This directory contains the centralized animation registry system that serves as the **single source of truth** for all animations in the Animation Playground.

## Architecture

### Files

- **`animationsRegistry.ts`** - Main registry file that combines metadata and components
- **`../templates/types.ts`** - Type definitions for templates and registry entries
- **`../renderers/`** - Template renderers for rendering animations

### How It Works

1. **Registry Initialization**: On module load, the registry automatically:
   - Reads animation metadata from `app/animations/registry.ts`
   - Looks up components from `app/animations/core/AnimationRegistry.ts`
   - Combines them into unified `AnimationRegistryEntry` objects
   - Filters out animations without components

2. **Template System**: Each animation is assigned a template type:
   - `button` - Button animations
   - `text` - Text effects
   - `header` - Header animations
   - `card` - Card/tile animations
   - `calendar-tile` - Calendar-specific animations
   - `background` - Background effects
   - `interactive` - Interactive animations
   - `form` - Form animations
   - `dashboard` - Dashboard components
   - `media` - Media/gallery animations
   - `hero` - Hero section animations
   - `section` - Section layouts
   - `banner` - Banner/status animations
   - `generic` - Other animations

3. **Usage**: The `AnimationPlaygroundClient` uses `getAllAnimations()` to get the complete list of animations, which replaces the old `getPresets()` logic.

## Migration Status

✅ **Phase 6.xA Complete**: 
- Template types defined
- Registry created and initialized
- Renderers created
- Integrated into tile system
- All animations automatically registered

## Future Phases

- **Phase 6.xB**: Generator script to auto-generate registry entries
- **Phase 7.xA**: Section templates
- **Phase 7.xB**: Website builder UI
- **Phase 7.xC**: Content model
- **Phase 7.xD**: Export system

## Adding New Animations

To add a new animation:

1. Add metadata to `app/animations/registry.ts`
2. Add component to `app/animations/core/AnimationRegistry.ts`
3. The registry will automatically pick it up on next load

In the future (Phase 6.xB), a generator script will automate this process.

