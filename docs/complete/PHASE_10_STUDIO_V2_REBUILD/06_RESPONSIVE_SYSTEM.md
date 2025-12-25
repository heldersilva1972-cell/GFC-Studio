# GFC Studio - Responsive System

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Device Viewports & Breakpoint Logic

---

## 📱 Overview

Studio implements a **Mobile-First** responsive architecture. Every property in the Studio (padding, font-size, layout) can be overridden for specific screen sizes.

---

## 📐 Breakpoints

Studio uses standard Tailwind-compatible breakpoints:

| Label | Device Type | Width |
|-------|-------------|-------|
| **Base** | Mobile (Portrait) | < 640px |
| **SM** | Mobile (Landscape) | 640px |
| **MD** | Tablet | 768px |
| **LG** | Laptop / Desktop | 1024px |
| **XL** | Large Screens | 1280px |
| **2XL** | Ultra-Wide | 1536px |

---

## 🛠️ Property Overrides

### How it Works
When editing a component, the right panel detects the active "Device View" selected in the top bar.

1. **Inheritance:** If a value is set on "Desktop" (LG), it trickles down to "Mobile" unless explicitly changed.
2. **Visual Feedback:** Studio icons indicate if a property has a device-specific override.
   - 💻 *Inherited from Desktop*
   - 📱 *Customized for Mobile*

### Example JSON Payload
```json
{
  "fontSize": "3rem",
  "responsive": {
    "md": { "fontSize": "2rem" },
    "sm": { "fontSize": "1.5rem" }
  }
}
```

---

## 🖼️ Preview System

### 1. View Toggles
Located in the Top Command Bar:
- **💻 Desktop:** 100% width canvas.
- **📱 Tablet:** 768px centered container with frame.
- **📱 Mobile:** 375px centered container with frame and notch mockup.

### 2. Auto-Zoom
If the preview device is larger than the available Studio workspace, an "Auto-Zoom" feature scales the frame down (e.g., 50%) so the entire height/width remains visible for editing.

---

## 🎨 Layout Switching
- **Stacking:** Toggle "Direction" from `Row` (Desktop) to `Column` (Mobile) with one click.
- **Hidden Elements:** Mark any section as "Hidden on Mobile" or "Hidden on Desktop" to create unique experiences for different users.

---

**Next:** 07_IMPORT_EXPORT_SYSTEM.md ➜
