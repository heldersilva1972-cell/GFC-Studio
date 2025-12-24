# GFC Studio - Import & Export System

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Content Migration & Portability Specs

---

## 📥 Import Capabilities

GFC Studio supports multi-source ingestion to help migrate the old website and bring in new external designs.

### 1. HTML File Import
- **User Action:** Upload a `.html` file.
- **Process:** Studio parses the DOM tree.
- **Mapping:** It tries to identify headers, footers, and main sections.
- **Result:** A new Studio page is created with "HTML Raw" blocks that can be converted into standard Studio components.

### 2. URL Scraping (Live Website)
- **Tool:** Backend scraper using Playwright/Puppeteer.
- **Process:** 
  1. Fetches the live URL.
  2. Captures all styles and computed CSS.
  3. Downloads all images to the Studio Media Library.
  4. Reconstructs the page as Studio editable components.

### 3. JSON Import
- **Standard:** Use for moving pages between different Studio environments (local vs production).
- **Format:** The `PageSchema v2.0` specification.

---

## 📤 Export Capabilities

### 1. Studio Bundle (.zip)
Includes:
- `page.json` (The structure)
- `/images` (All assets used)
- `theme.json` (Global CSS tokens)

### 2. Static HTML/CSS
- Generates a standalone, dependency-free HTML version of the page.
- Useful for archiving or hosting on simple static servers.

### 3. Markdown
- Exports just the textual content, headings, and images.
- Useful for documentation or blog migrations.

---

## 🤖 Smart Conversion (AI Detection)
While the user opted out of "paid AI support" for daily editing, the **Import System** uses a **built-in heuristic engine** (Rule-based AI) to perform:

- **Component Identification:** Tagging a `<div>` with `class="hero"` as a "Hero Component".
- **Color Extraction:** Automatically adding a website's primary colors to the Studio "Global Theme".
- **Asset Deduplication:** Ensuring the same image isn't uploaded twice during a bulk site import.

---

**Next:** 08_FORM_BUILDER.md ➜
