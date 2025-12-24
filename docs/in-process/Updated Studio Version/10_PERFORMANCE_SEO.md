# GFC Studio - Performance & SEO

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Search Optimization & Speed Strategy

---

## 🚀 Performance Strategy

Studio ensures the GFC Website hits **95+ Lighthouse Scores** by default.

### 1. Technical Implementation
- **ISR (Incremental Static Regeneration):** Pages are pre-rendered at build time but updated in the background as you change them in Studio.
- **Critical CSS:** Studio extracts only the CSS used on the page and inlines it for instant first-paint.
- **Minification:** Automated removal of unneeded code and comments during the publish phase.

### 2. Monitoring
The Studio Dashboard displays a "Health Score" for each page, calculating:
- Total page weight (MB).
- Number of network requests.
- First Contentful Paint (FCP) estimates.

---

## 🔍 SEO Features

### 1. On-Page Basics
Every page in Studio has an "SEO Panel" with:
- **Title Tag:** Real-time character counter (Target: 50–60 chars).
- **Meta Description:** Real-time character counter (Target: 150–160 chars).
- **Canonical URL:** Automatically generated to prevent duplicate content issues.

### 2. Structured Data (Schema.org)
Studio automatically injects JSON-LD for:
- **Organizations:** GFC club info, location, phone.
- **Events:** Calendar events with dates, prices, and locations.
- **Local Business:** Hall rental services and hours.

### 3. Image SEO
- **Alt Text Reminder:** Studio flags images missing alt-text during the "Final Review" before publishing.

---

## 🗺️ Indexing
- **Sitemap.xml:** Automatically updated whenever a page is published or deleted.
- **Robots.txt:** Configurable via Studio Settings to manage search crawler access.

---

**Next:** 11_IMPLEMENTATION_GUIDE.md ➜
