# GFC Studio - UI Specifications

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Visual Interface & Experience Design

---

## 🎨 Theme & Appearance

### Color Palette
- **Background:** `#0F172A` (Slate 900)
- **Panels:** `#1E293B` (Slate 800)
- **Active Accent:** `#3B82F6` (Blue 500)
- **Text:** `#F1F5F9` (Primary), `#94A3B8` (Secondary)

### Workspace Layout
The editor occupies 100% of the viewport width and height.

1. **Top Bar (60px):** Global commands, device toggles.
2. **Left Panel (280px):** Component Library (Collapsible).
3. **Right Panel (320px):** Properties Inspector (Collapsible).
4. **Canvas (Center):** Iframe rendering showing `localhost:3000/preview`.

---

## 🕹️ Interaction Model

### Drag and Drop
- Components from the left panel can be dragged into the "Canvas" iframe.
- "Visual drop indicators" appear in the Next.js preview to show insertion points.

### Property Editing
- Clicking a component in the preview selects it.
- Right panel updates instantly to show relevant properties (Padding, Colors, Text, etc.).

---

## 📱 Device Preview Logic

The canvas scales the iframe to simulate different breakpoints:
- **Desktop:** 100% width.
- **Tablet:** 768px width (centered with device frame).
- **Mobile:** 375px width (centered with device frame).
- **Zoom:** Auto-scales the preview frame to fit the available viewport space.
