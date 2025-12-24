# GFC Studio - Asset Manager

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Media Library & File Optimization System

---

## 📂 Overview

The Asset Manager is the central repository for all images, videos, and documents used across the GFC Website and Studio.

---

## 🏗️ Organization

### 1. Folder System
- **Virtual Folders:** Stored in the database, allowing for deep nesting without moving files on the physical disk.
- **Search:** Instant indexing based on filenames and metadata tags.

### 2. File Tags
Automatically applied tags:
- `PageUse:home`
- `Type:hero`
- `Date:Dec-2024`

---

## ⚡ Image Optimization (Auto-Process)

To ensure the GFC Website remains extremely fast, every upload undergoes an automated pipeline:

1. **Format Conversion:** Converts `JPG/PNG` to `WebP` and `AVIF` for maximum compression.
2. **Responsive Sizing:** Generates 4 variants of every image:
   - `xl` (1920px)
   - `lg` (1280px)
   - `md` (768px)
   - `sm` (375px)
3. **Lazy Loading:** Studio automatically appends `loading="lazy"` to all images unless marked as "Above the Fold".

---

## 🎞️ Video Handling
- **Streaming:** Support for large background videos via fragmented streaming.
- **Thumnails:** Automatic frame-capture to create poster images for `<video>` tags.

---

## 📊 Usage Tracking
- **"Where Is This Used?"**: Ability to see which pages or components are currently referencing an asset before deleting it.
- **Dangling Assets:** Identify files that aren't used anywhere to save storage space.

---

**Next:** 10_PERFORMANCE_SEO.md ➜
