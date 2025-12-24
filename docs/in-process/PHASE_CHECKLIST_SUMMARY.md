# GFC Ecosystem Rebuild - Master Phase Checklist

This document provides a high-level summary of the **Studio V2** and **New GFC Website** implementation phases. Use this as a punch-list for tracking overall project progress.

---

## 🏗️ Part 1: GFC Studio V2 Rebuild (The Tool)

### ✅ Phase 10: Foundation & Infrastructure
*Goal: Establish the shell and real-time preview link.*
- [ ] Full-page editor layout (100vw/100vh).
- [ ] Next.js Iframe preview canvas.
- [ ] Device Viewport toggles (Mobile/Tablet/Desktop).
- [ ] Core database schema (Pages, Sections, Drafts).

### 🛠️ Phase 11: Component System & Visual Editing
*Goal: Turn the shell into a functional builder.*
- [ ] Drag-and-drop from sidebar to iframe.
- [ ] Properties Inspector with color pickers and sliders.
- [ ] "Big Four" blocks: Hero, Text, Button, Grid.
- [ ] Real-time property syncing (WebSockets/SignalR).

### 💾 Phase 12: Page Management & Workflow
*Goal: Multi-page handling and data safety.*
- [ ] Page switcher and creation wizard.
- [ ] Debounced Auto-save to Drafts.
- [ ] Version history with named snapshots and rollback.
- [ ] One-click "Publish" with Next.js cache purging.

### 🎬 Phase 13: Animation Timeline & Global Styles
*Goal: Professional motion and site-wide branding.*
- [ ] Layer-based Animation Timeline UI.
- [ ] Framer Motion & GSAP entrance/scroll triggers.
- [ ] Global Design Tokens (One-click color/font updates site-wide).
- [ ] High-Accessibility Mode for senior readability.

### 🧰 Phase 14: Utility Suite (Forms, Assets & SEO)
*Goal: Completing the daily management toolkit.*
- [ ] Asset Manager with automatic WebP optimization.
- [ ] Visual Form Builder for rentals and contact.
- [ ] "Save & Resume" logic for incomplete rental forms.
- [ ] SEO & Performance scoring dashboard (per page).

### 📥 Phase 15: Migration & Portability
*Goal: Ingesting the old site and backing up the new one.*
- [ ] URL Scraper for legacy content harvesting.
- [ ] HTML-to-Block conversion engine.
- [ ] Full-site Backup (.ZIP) and Static HTML export.
- [ ] Redirect Manager for preserving old Google links.

---

## 🌐 Part 2: New GFC Website (The Face)

### 🚀 Phase 11 Integration: "Modern Legacy" Design
*Goal: Launching the high-performance public frontend.*
- [ ] Midnight Blue & Burnished Gold aesthetic.
- [ ] **Web App Controls:**
    - [ ] Live "Open/Closed" toggle.
    - [ ] Automated Update Bar (News ticker).
    - [ ] Real-time Booking Calendar (Pulls from Web App).
    - [ ] Review Moderation Queue (Approval workflow).
- [ ] **Operational Intelligence:**
    - [ ] Smart Alert Routing (Choosing which Director gets notifications).
    - [ ] High-Accessibility toggle for members.
    - [ ] Secure Document Library (Role-based access).

---

## 📈 Success Summary
- **Admin Control:** No code required for daily club updates.
- **Performance:** 95+ score targets on all pages.
- **Operations:** Integrated with Staff Shifts, Hall Rentals, and Member Dues.
