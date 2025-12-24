# GFC Studio - Animation System

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Animation Orchestrator & Timeline Specs

---

## 🎬 Overview

The GFC Studio Animation System is powered by **Framer Motion** and **GSAP**. It allows users to create high-end, professional animations through a visual "After Effects" style timeline without writing code.

---

## ⚙️ Core Architecture

### 1. The Interaction Model
Components are wrapped in `motion` components. The Studio provides a UI to map database values to motion props:

- **Initial State:** Transform, opacity, scale before enter.
- **Animate State:** Final visible state.
- **Exit State:** Transition out state.
- **Transition Settings:** Duration, delay, easing (Spring, Tween, Inertia).

---

## 🎞️ Timeline Visual Editor

### Data Structure
Animations are stored in a `sequence` array within the `AnimationJson` column:

```json
[
  {
    "target": "headline",
    "effect": "slideUp",
    "startTime": 0.5,
    "duration": 1.2,
    "easing": "easeOutExpo"
  },
  {
    "target": "image",
    "effect": "zoomIn",
    "startTime": 0.8,
    "duration": 1.5,
    "easing": "easeInOut"
  }
]
```

### UI Controls
- **Playhead:** 0 to 10 seconds scrub bar.
- **Layers:** Each sub-element (Title, Subtitle, Graphic) is a row.
- **Handles:** Drag handles to adjust start/end times.
- **Easing Bezier:** Visual curve editor for custom easing paths.

---

## ⚡ Effect Library

### Entry Effects
- **Reveal:** Clip-path reveal (Left to Right, Center Out).
- **Stagger:** Sequential item entrance for lists.
- **Glitch:** Cyber-punk style flicker.

### Scroll Triggers (GSAP ScrollTrigger)
- **Parallax:** Offset Y position based on scroll progress.
- **Pinning:** Keep element in view while scrolling through a long section.
- **Progressive Reveal:** Character-by-character text reveal on scroll.

---

## 🛠️ Technical Integration

1. **Studio Side (Blazor):**
   - The UI generates the JSON payload.
   - It sends the payload via WebSocket to the Next.js preview.

2. **Website Side (Next.js):**
   - Receives the JSON.
   - Updates the `@framer-motion` context.
   - Uses `AnimatePresence` to handle element switching.

---

## ✅ Performance Optimization
- **Hardware Acceleration:** Animations focus on `transform` and `opacity` to avoid layout thrashing.
- **Prefers Reduced Motion:** Automatically respects user system settings to disable intense animations for accessibility.

---

**Next:** 06_RESPONSIVE_SYSTEM.md ➜
