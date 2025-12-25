# GFC Website - Design System

**Version:** 1.0.0  
**Date:** December 24, 2024  
**Theme:** "Modern Legacy"

---

## 🎨 Color Palette

The colors are chosen to reflect professional maturity, exclusivity, and the club's long history while maintaining a sleek, modern edge.

| Role | Color Name | Hex Code | Visual Target |
| :--- | :--- | :--- | :--- |
| **Primary Background** | Midnight Coal | `#0F172A` | Deep, dark charcoal-blue. |
| **Secondary Background** | Night Shadow | `#111827` | Subtle contrast for sections. |
| **Accent (Primary)** | Burnished Gold | `#D4AF37` | Sophisticated metallic gold. |
| **Accent (Secondary)** | Vintage Champagne| `#F7E7CE` | Light, elegant highlight. |
| **Primary Text** | Ivory Mist | `#F9FAFB` | Off-white for readability. |
| **Secondary Text** | Slate Silver | `#9CA3AF` | Supporting text/descriptions. |
| **Status (Success)** | Emerald | `#10B981` | Open indicators/Success. |

---

## ✍️ Typography

| Intent | Font Family | Weight | Style |
| :--- | :--- | :--- | :--- |
| **Headings (H1, H2)** | **Outfit** | 700 (Bold) | Modern, clean, geometric. |
| **Subheadings (H3)** | **Outfit** | 500 (Medium) | Elegant and spaced. |
| **Body Text** | **Inter** | 400 (Regular) | Maximum readability. |
| **Action Buttons** | **Inter** | 600 (Semibold) | Bold and clear. |

---

## ✨ UI Components & Aesthetics

### Buttons (The "Premium" Button)
- **Primary:** Solid "Burnished Gold" background with "Midnight Coal" text.
- **Secondary:** Transparent with a thin gold border (Outline).
- **Interaction:** A subtle "lift" effect (Shadow increases) and a gold glow on hover.

### Cards
- **Style:** Dark background (`Night Shadow`) with a very thin border (`#374151`).
- **Corner Radius:** 12px (Smooth, modern).
- **Shadow:** Deep, wide shadow to create a "floating" effect on the charcoal background.

### Sections
- **Spacing:** Large vertical padding (120px) to provide a "breathable" premium feel.
- **Reveal:** All sections use a **0.8s Fade-In Slide-Up** animation as they enter the viewport.

---

## 🖼️ Imagery & Icons

### Photography Direction
- **Exterior:** Sunset or "Blue Hour" photography of the GFC building.
- **Interior:** Crisp, wide-angle shots of the halls with warm lighting.
- **Lifestyle:** Natural, candit shots of club members and community events.

### Iconography
- **System:** Lucide React icons.
- **Weight:** Thin (2px) to match the elegant typography.
- **Color:** Burnished Gold.

---

## ♿ Accessibility & Readability
 
- **Mobile First:** Navigation collapses into a full-screen, sleek dark overlay menu.
- **Stacking:** Grid layouts switch to single columns with increased vertical spacing.
- **Touch Targets:** Buttons are increased to 48px height minimum for easier mobile taps.

### High-Accessibility Mode (Toggle)
The website includes an optional "Accessibility" toggle for members who need enhanced readability.
- **High Contrast:** Swaps Gold/Champagne text for High-Contrast White.
- **Text Scaling:** Increases all body text by 20% (16px → 20px).
- **Reduced Motion:** Disables all Framer Motion reveal animations.
- **Focus States:** Adds a 3px thick gold border around all active keyboard elements.
