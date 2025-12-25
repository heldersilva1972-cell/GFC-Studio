# GFC Studio - UI/UX Specifications

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Detailed User Interface Specifications

---

## 🎨 Overview

This document provides pixel-perfect specifications for the GFC Studio user interface, including layouts, interactions, visual design, and user workflows.

---

## 📐 Layout Specifications

### Overall Dimensions

```
Full Editor Window: 100vw × 100vh (full screen)

Layout Breakdown:
├── Top Command Bar: 100% × 60px (fixed)
├── Workspace: 100% × calc(100vh - 60px)
    ├── Left Panel: 280px × 100% (collapsible)
    ├── Center Canvas: flex-grow × 100%
    └── Right Panel: 320px × 100% (collapsible)
```

### Responsive Breakpoints

```
Minimum Studio Resolution: 1366px × 768px
Recommended: 1920px × 1080px or higher

Note: Studio is desktop-only, no mobile editing
```

---

## 🎯 Top Command Bar

### Layout

```
┌─────────────────────────────────────────────────────────────┐
│ [Logo] [Page Selector] [Actions] [Devices] [History] [Exit] │
└─────────────────────────────────────────────────────────────┘
Height: 60px
Background: #1f2937 (dark gray)
Border Bottom: 1px solid #374151
```

### Components (Left to Right)

#### 1. Studio Logo/Brand
```
Position: Left (16px padding)
Content: "GFC Studio" text + icon
Font: Outfit, 20px, Bold
Color: #ffffff
Width: 180px
```

#### 2. Page Selector
```
Position: After logo
Component: Dropdown button
Width: 250px
Height: 40px
Background: #374151
Border: 1px solid #4b5563
Border Radius: 6px
Padding: 0 12px

Content:
┌──────────────────────────┐
│ Page: Home ▼             │
└──────────────────────────┘

On Click:
┌──────────────────────────┐
│ 🏠 Home          ✓       │
│ 🏛️ Hall Rentals          │
│ 📅 Events                │
│ 📞 Contact Us            │
│ ℹ️ About                 │
├──────────────────────────┤
│ ➕ New Page              │
│ ⚙️ Manage Pages          │
└──────────────────────────┘
Max Height: 400px (scrollable)
```

#### 3. Action Buttons
```
Position: Center-left
Buttons: [Save Draft] [Publish]
Spacing: 12px gap

Save Draft Button:
- Width: 120px
- Height: 40px
- Background: #3b82f6 (blue)
- Color: #ffffff
- Border Radius: 6px
- Font: Inter, 14px, Medium
- Hover: #2563eb

Publish Button:
- Width: 100px
- Height: 40px
- Background: #10b981 (green)
- Color: #ffffff
- Border Radius: 6px
- Font: Inter, 14px, Medium
- Hover: #059669
```

#### 4. Device Toggle
```
Position: Center-right
Component: Button group
Width: 180px
Height: 40px

Buttons:
[💻 Desktop] [📱 Tablet] [📱 Mobile]

Each Button:
- Width: 60px
- Height: 40px
- Background: #374151 (inactive)
- Background: #3b82f6 (active)
- Border: 1px solid #4b5563
- Border Radius: 6px (first/last), 0 (middle)
- Font Size: 20px (emoji)
```

#### 5. History Button
```
Position: Right side
Width: 40px
Height: 40px
Background: #374151
Border: 1px solid #4b5563
Border Radius: 6px
Icon: Clock/History icon
Tooltip: "Version History"
```

#### 6. Exit Studio Button
```
Position: Far right (16px padding)
Width: 100px
Height: 40px
Background: transparent
Border: 1px solid #ef4444 (red)
Color: #ef4444
Border Radius: 6px
Text: "Exit Studio"
Hover: Background #ef4444, Color #ffffff
```

---

## 📦 Left Panel: Component Library

### Panel Specifications

```
Width: 280px (default)
Min Width: 240px
Max Width: 400px
Height: 100%
Background: #111827 (dark)
Border Right: 1px solid #374151
Resizable: Yes (drag right edge)
Collapsible: Yes (arrow button)
```

### Collapse Button

```
Position: Top right of panel
Width: 24px
Height: 24px
Background: #374151
Border Radius: 4px
Icon: ◀ (when open), ▶ (when closed)
Hover: #4b5563
```

### Panel Header

```
Height: 50px
Padding: 16px
Background: #1f2937
Border Bottom: 1px solid #374151

Content:
┌────────────────────────┐
│ Components             │
└────────────────────────┘
Font: Outfit, 16px, Semibold
Color: #ffffff
```

### Search Bar

```
Position: Below header
Height: 40px
Margin: 12px
Background: #374151
Border: 1px solid #4b5563
Border Radius: 6px
Padding: 0 12px

Placeholder: "Search components..."
Font: Inter, 14px
Color: #9ca3af (placeholder)
Icon: 🔍 (left side)
```

### Category Tabs

```
Height: 44px
Padding: 0 12px
Background: #1f2937
Border Bottom: 1px solid #374151

Tabs:
[All] [Layout] [Content] [Cards] [Interactive] [Media] [GFC]

Each Tab:
- Padding: 8px 12px
- Font: Inter, 13px, Medium
- Color: #9ca3af (inactive)
- Color: #3b82f6 (active)
- Border Bottom: 2px solid #3b82f6 (active)
- Cursor: pointer
- Hover: #d1d5db (text color)
```

### Component List

```
Height: calc(100% - 134px)
Padding: 12px
Overflow-Y: Scroll
Scrollbar: Custom styled (8px wide, dark theme)

Component Item:
┌────────────────────────┐
│ 📄 Hero Section        │
│ Full-width header...   │
└────────────────────────┘

Each Item:
- Height: 72px
- Margin Bottom: 8px
- Background: #1f2937
- Border: 1px solid #374151
- Border Radius: 6px
- Padding: 12px
- Cursor: grab
- Draggable: true

Hover State:
- Border: 1px solid #3b82f6
- Background: #1e3a5f

Drag State:
- Opacity: 0.7
- Cursor: grabbing
- Shadow: 0 4px 12px rgba(0,0,0,0.3)

Content Layout:
Icon (24px) + Text (flex-grow)
- Icon: Emoji or SVG, 24px
- Title: Inter, 14px, Semibold, #ffffff
- Description: Inter, 12px, Regular, #9ca3af
```

---

## 🖼️ Center Canvas: Live Preview

### Canvas Container

```
Width: flex-grow (fills remaining space)
Height: 100%
Background: #0f172a (dark blue-gray)
Padding: 24px
Display: flex
Justify Content: center
Align Items: flex-start
Overflow: auto
```

### Device Frame

```
Desktop View (1920px):
- Width: 100% (max 1920px)
- Height: auto
- Border: none
- Shadow: 0 10px 40px rgba(0,0,0,0.3)

Tablet View (768px):
- Width: 768px
- Height: 1024px
- Border: 8px solid #1f2937
- Border Radius: 12px
- Shadow: 0 10px 40px rgba(0,0,0,0.3)

Mobile View (375px):
- Width: 375px
- Height: 667px
- Border: 12px solid #1f2937
- Border Radius: 24px
- Shadow: 0 10px 40px rgba(0,0,0,0.3)
```

### Preview Iframe

```
Width: 100%
Height: 100%
Border: none
Background: #ffffff
Pointer Events: all (interactive)

URL: http://localhost:3000/preview?pageId={pageId}
```

### Selection Overlay

```
When component selected:
- Outline: 2px dashed #3b82f6
- Outline Offset: 2px
- Background: rgba(59, 130, 246, 0.1)

Selection Label:
┌─────────────────┐
│ Hero Section ✕  │
└─────────────────┘
Position: Top left of selected element
Background: #3b82f6
Color: #ffffff
Padding: 4px 8px
Border Radius: 4px 4px 0 0
Font: Inter, 11px, Medium
```

### Zoom Controls

```
Position: Bottom right of canvas
Width: 120px
Height: 40px
Background: #1f2937
Border: 1px solid #374151
Border Radius: 6px
Padding: 4px

Buttons:
[−] [100%] [+]

Each Button:
- Width: 36px
- Height: 32px
- Background: transparent
- Color: #ffffff
- Font: Inter, 14px
- Hover: #374151
- Active: #3b82f6

Zoom Levels: 25%, 50%, 75%, 100%, 125%, 150%, 200%
```

---

## ⚙️ Right Panel: Properties Inspector

### Panel Specifications

```
Width: 320px (default)
Min Width: 280px
Max Width: 480px
Height: 100%
Background: #111827 (dark)
Border Left: 1px solid #374151
Resizable: Yes (drag left edge)
Collapsible: Yes (arrow button)
```

### Collapse Button

```
Position: Top left of panel
Width: 24px
Height: 24px
Background: #374151
Border Radius: 4px
Icon: ▶ (when open), ◀ (when closed)
Hover: #4b5563
```

### Panel Header

```
Height: 50px
Padding: 16px
Background: #1f2937
Border Bottom: 1px solid #374151

Content (when component selected):
┌────────────────────────┐
│ Hero Section           │
│ Properties             │
└────────────────────────┘
Font: Outfit, 16px, Semibold
Color: #ffffff

Content (when nothing selected):
┌────────────────────────┐
│ No Selection           │
│ Click element to edit  │
└────────────────────────┘
Font: Inter, 14px, Regular
Color: #9ca3af
```

### Properties Tabs

```
Height: 44px
Padding: 0 12px
Background: #1f2937
Border Bottom: 1px solid #374151

Tabs:
[Content] [Style] [Layout] [Animation] [Advanced]

Each Tab:
- Padding: 8px 12px
- Font: Inter, 13px, Medium
- Color: #9ca3af (inactive)
- Color: #3b82f6 (active)
- Border Bottom: 2px solid #3b82f6 (active)
```

### Properties Content Area

```
Height: calc(100% - 94px)
Padding: 16px
Overflow-Y: Scroll
Scrollbar: Custom styled (8px wide, dark theme)
```

### Property Field Types

#### Text Input
```
Label:
- Font: Inter, 12px, Medium
- Color: #d1d5db
- Margin Bottom: 6px

Input:
- Width: 100%
- Height: 36px
- Background: #374151
- Border: 1px solid #4b5563
- Border Radius: 6px
- Padding: 0 12px
- Font: Inter, 14px
- Color: #ffffff
- Focus: Border #3b82f6

Example:
Title
[Welcome to GFC_____________]
```

#### Textarea
```
Same as Text Input, but:
- Height: 80px (default)
- Resize: vertical
- Padding: 8px 12px
```

#### Color Picker
```
Label + Preview + Input

Color Preview:
- Width: 36px
- Height: 36px
- Border: 2px solid #4b5563
- Border Radius: 6px
- Background: Selected color
- Cursor: pointer

Input:
- Width: calc(100% - 44px)
- Height: 36px
- Displays hex value
- Click preview to open color picker modal

Example:
Background Color
[🎨] [#1e3a8a_______________]
```

#### Slider
```
Label + Value + Slider

Value Display:
- Font: Inter, 12px, Medium
- Color: #3b82f6
- Position: Right of label

Slider:
- Width: 100%
- Height: 6px
- Background: #374151
- Border Radius: 3px
- Thumb: 16px circle, #3b82f6
- Track (filled): #3b82f6

Example:
Font Size                    2rem
[═══════●═══════════════════]
```

#### Dropdown
```
Label + Select

Select:
- Width: 100%
- Height: 36px
- Background: #374151
- Border: 1px solid #4b5563
- Border Radius: 6px
- Padding: 0 12px
- Font: Inter, 14px
- Color: #ffffff
- Arrow: ▼ (right side)

Example:
Font Family
[Inter                    ▼]
```

#### Toggle Switch
```
Label + Switch

Switch:
- Width: 44px
- Height: 24px
- Background: #4b5563 (off), #3b82f6 (on)
- Border Radius: 12px
- Thumb: 20px circle, #ffffff
- Transition: 0.2s ease

Example:
Show on Mobile          [○──]
```

#### Image Upload
```
Label + Upload Area

Upload Area:
- Width: 100%
- Height: 120px
- Background: #374151
- Border: 2px dashed #4b5563
- Border Radius: 6px
- Display: flex, center
- Cursor: pointer

Content (empty):
📷 Click to upload
or drag image here

Content (with image):
[Image Preview]
[Change] [Remove]

Example:
Background Image
┌────────────────────────┐
│                        │
│   📷 Click to upload   │
│   or drag image here   │
│                        │
└────────────────────────┘
```

---

## 🎬 Animation Timeline (Bottom Panel)

### Panel Specifications

```
Position: Bottom of canvas (when animation tab active)
Width: 100%
Height: 280px (default)
Min Height: 200px
Max Height: 400px
Background: #111827
Border Top: 1px solid #374151
Resizable: Yes (drag top edge)
Collapsible: Yes (minimize button)
```

### Timeline Header

```
Height: 44px
Padding: 12px 16px
Background: #1f2937
Border Bottom: 1px solid #374151

Content:
[▶ Play] [⏸ Pause] [⏹ Stop] [Timeline: 0.0s - 5.0s]

Buttons:
- Width: 32px
- Height: 32px
- Background: #374151
- Border: 1px solid #4b5563
- Border Radius: 4px
- Color: #ffffff
- Margin Right: 8px
```

### Timeline Ruler

```
Height: 40px
Background: #1f2937
Border Bottom: 1px solid #374151
Padding: 0 16px

Markers:
0s    1s    2s    3s    4s    5s
│─────│─────│─────│─────│─────│

Playhead:
- Width: 2px
- Height: 100%
- Background: #ef4444 (red)
- Position: Absolute
- Draggable: Yes
```

### Animation Layers

```
Height: calc(100% - 84px)
Padding: 12px 16px
Overflow-Y: Scroll

Layer Item:
┌────────────────────────────────────┐
│ Background                         │
│ [═══════════════════════════]     │
│ Fade In (0s - 1s)                 │
└────────────────────────────────────┘

Each Layer:
- Height: 60px
- Margin Bottom: 8px
- Background: #1f2937
- Border: 1px solid #374151
- Border Radius: 6px
- Padding: 8px

Layer Name:
- Font: Inter, 13px, Semibold
- Color: #ffffff

Animation Bar:
- Height: 24px
- Background: #3b82f6
- Border Radius: 4px
- Position: Relative to timeline
- Draggable: Yes (move/resize)
- Resizable: Yes (handles on edges)

Animation Label:
- Font: Inter, 11px, Regular
- Color: #ffffff
- Position: Below bar
```

---

## 🎨 Color Picker Modal

### Modal Specifications

```
Position: Center of screen
Width: 320px
Height: 420px
Background: #1f2937
Border: 1px solid #374151
Border Radius: 8px
Shadow: 0 20px 60px rgba(0,0,0,0.5)
```

### Modal Header

```
Height: 50px
Padding: 16px
Border Bottom: 1px solid #374151

Title: "Select Color"
Font: Outfit, 16px, Semibold
Color: #ffffff

Close Button: ✕ (top right)
```

### Color Picker Content

```
Padding: 16px

Components:
1. Color Gradient Square (280px × 200px)
2. Hue Slider (280px × 12px)
3. Opacity Slider (280px × 12px)
4. Hex Input (280px × 36px)
5. Color Presets (swatches)
6. [Cancel] [Apply] buttons
```

---

## 📱 Responsive Behavior

### Panel Collapsing

```
Screen Width > 1600px:
- Both panels open by default
- Maximum workspace

Screen Width 1366px - 1600px:
- Right panel open
- Left panel collapsed
- Good balance

Screen Width < 1366px:
- Warning: "Studio requires minimum 1366px width"
- Suggest increasing window size
```

---

## 🎯 Interaction States

### Buttons

```
Default:
- Background: #374151
- Border: 1px solid #4b5563
- Color: #ffffff

Hover:
- Background: #4b5563
- Border: 1px solid #6b7280
- Cursor: pointer

Active/Pressed:
- Background: #1f2937
- Border: 1px solid #3b82f6
- Transform: scale(0.98)

Disabled:
- Background: #1f2937
- Border: 1px solid #374151
- Color: #6b7280
- Cursor: not-allowed
- Opacity: 0.5
```

### Input Fields

```
Default:
- Background: #374151
- Border: 1px solid #4b5563
- Color: #ffffff

Focus:
- Border: 1px solid #3b82f6
- Box Shadow: 0 0 0 3px rgba(59, 130, 246, 0.1)

Error:
- Border: 1px solid #ef4444
- Box Shadow: 0 0 0 3px rgba(239, 68, 68, 0.1)

Success:
- Border: 1px solid #10b981
```

### Drag & Drop

```
Dragging Component:
- Opacity: 0.7
- Cursor: grabbing
- Shadow: 0 4px 12px rgba(0,0,0,0.3)
- Z-index: 1000

Drop Zone (valid):
- Border: 2px dashed #3b82f6
- Background: rgba(59, 130, 246, 0.05)

Drop Zone (invalid):
- Border: 2px dashed #ef4444
- Background: rgba(239, 68, 68, 0.05)
- Cursor: not-allowed
```

---

## 🔔 Notifications & Feedback

### Toast Notifications

```
Position: Top right
Width: 320px
Height: auto (min 60px)
Background: #1f2937
Border: 1px solid #374151
Border Radius: 8px
Shadow: 0 10px 30px rgba(0,0,0,0.3)
Padding: 16px
Animation: Slide in from right

Types:
Success: Green accent (#10b981)
Error: Red accent (#ef4444)
Warning: Yellow accent (#f59e0b)
Info: Blue accent (#3b82f6)

Auto-dismiss: 5 seconds
Close button: ✕ (top right)
```

### Loading States

```
Spinner:
- Size: 24px
- Color: #3b82f6
- Animation: Rotate 360deg, 1s, infinite

Skeleton Loaders:
- Background: #374151
- Shimmer: Linear gradient animation
- Border Radius: 6px
```

### Progress Bars

```
Width: 100%
Height: 4px
Background: #374151
Border Radius: 2px

Progress:
- Background: #3b82f6
- Height: 4px
- Border Radius: 2px
- Transition: width 0.3s ease
```

---

## 🎨 Design Tokens

### Colors

```css
/* Primary Colors */
--color-primary-50: #eff6ff;
--color-primary-500: #3b82f6;
--color-primary-600: #2563eb;
--color-primary-700: #1d4ed8;

/* Gray Scale */
--color-gray-50: #f9fafb;
--color-gray-100: #f3f4f6;
--color-gray-200: #e5e7eb;
--color-gray-300: #d1d5db;
--color-gray-400: #9ca3af;
--color-gray-500: #6b7280;
--color-gray-600: #4b5563;
--color-gray-700: #374151;
--color-gray-800: #1f2937;
--color-gray-900: #111827;
--color-gray-950: #0f172a;

/* Semantic Colors */
--color-success: #10b981;
--color-warning: #f59e0b;
--color-error: #ef4444;
--color-info: #3b82f6;
```

### Typography

```css
/* Font Families */
--font-heading: 'Outfit', sans-serif;
--font-body: 'Inter', sans-serif;
--font-mono: 'Fira Code', monospace;

/* Font Sizes */
--text-xs: 0.75rem;    /* 12px */
--text-sm: 0.875rem;   /* 14px */
--text-base: 1rem;     /* 16px */
--text-lg: 1.125rem;   /* 18px */
--text-xl: 1.25rem;    /* 20px */
--text-2xl: 1.5rem;    /* 24px */

/* Font Weights */
--font-regular: 400;
--font-medium: 500;
--font-semibold: 600;
--font-bold: 700;
```

### Spacing

```css
--spacing-1: 0.25rem;  /* 4px */
--spacing-2: 0.5rem;   /* 8px */
--spacing-3: 0.75rem;  /* 12px */
--spacing-4: 1rem;     /* 16px */
--spacing-5: 1.25rem;  /* 20px */
--spacing-6: 1.5rem;   /* 24px */
--spacing-8: 2rem;     /* 32px */
--spacing-10: 2.5rem;  /* 40px */
--spacing-12: 3rem;    /* 48px */
```

### Shadows

```css
--shadow-sm: 0 1px 2px rgba(0, 0, 0, 0.05);
--shadow-md: 0 4px 6px rgba(0, 0, 0, 0.1);
--shadow-lg: 0 10px 15px rgba(0, 0, 0, 0.1);
--shadow-xl: 0 20px 25px rgba(0, 0, 0, 0.15);
--shadow-2xl: 0 25px 50px rgba(0, 0, 0, 0.25);
```

### Border Radius

```css
--radius-sm: 0.25rem;  /* 4px */
--radius-md: 0.375rem; /* 6px */
--radius-lg: 0.5rem;   /* 8px */
--radius-xl: 0.75rem;  /* 12px */
--radius-2xl: 1rem;    /* 16px */
--radius-full: 9999px;
```

### Transitions

```css
--transition-fast: 0.15s ease;
--transition-base: 0.3s ease;
--transition-slow: 0.5s ease;
```

---

## ✅ Accessibility

### Keyboard Navigation

```
Tab: Navigate between focusable elements
Shift+Tab: Navigate backwards
Enter: Activate button/link
Space: Toggle checkbox/switch
Escape: Close modal/dropdown
Arrow Keys: Navigate lists/menus
Ctrl+S: Save
Ctrl+Z: Undo
Ctrl+Y: Redo
Ctrl+P: Page switcher
```

### Screen Reader Support

```
All interactive elements have:
- aria-label or aria-labelledby
- Proper role attributes
- Focus indicators
- Semantic HTML
```

### Focus Indicators

```
All focusable elements:
- Outline: 2px solid #3b82f6
- Outline Offset: 2px
- Visible on keyboard focus
- Hidden on mouse click (but still accessible)
```

---

## 📏 Grid System

### Layout Grid

```
12-column grid system
Gutter: 24px
Max Width: 1920px
Padding: 24px
```

---

## 🎯 Component States

### Loading
- Show skeleton loaders
- Disable interactions
- Display spinner

### Empty
- Show empty state illustration
- Helpful message
- Call-to-action button

### Error
- Show error message
- Suggest solutions
- Retry button

### Success
- Show success message
- Green checkmark icon
- Auto-dismiss or close button

---

**This document provides the complete UI/UX specifications for GFC Studio. All measurements, colors, and interactions are precisely defined for implementation.**
