# GFC Studio - Phase 10: Studio V2 Rebuild Master Plan

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Status:** Planning Phase  
**Context:** This project (Phases 10-15) represents the full rebuild of the GFC Studio editor, accounting for the completion of legacy Phase 6A/6B and the ongoing work in Phases 7 (Mobile), 8 (Automation), and 9 (Polish).

---

## 🎯 Executive Summary

GFC Studio is being completely redesigned as a **professional-grade visual page builder** specifically optimized for editing Next.js/React websites. The V2 Rebuild fixes architectural issues from the previous project attempt, providing a full-page editing experience with live preview, drag-and-drop components, and advanced animations.

---

## 🏗️ Technical Architecture

- **Studio:** Blazor Server (ASP.NET Core 8).
- **Website:** Next.js 14+ (React).
- **Communication:** SignalR (Real-time syncing) + Iframe postMessage.
- **Database:** SQL Server (Existing GFC Context).

---

## 🚀 Rebuild Implementation Phases

### Phase 10: Foundation (Current)
**Goal:** Basic editor infrastructure and communication loop.
- Full-page Studio layout (removing app-shell clutter).
- Iframe integration for Next.js preview.
- Device viewport toggles (Desktop/Tablet/Mobile).
- Page switcher and basic database connectivity.

### Phase 11: Component System
**Goal:** Visual drag-and-drop mechanics.
- Component library UI.
- Property Inspector with visual controls (Color pickers, sliders).
- Next.js dynamic rendering engine.

### Phase 12: Page Management & Workflow
**Goal:** Handling multi-pane and versions.
- Auto-save system.
- Version history and rollback.
- One-click "Publish" to production.

### Phase 13: Advanced Animation Timeline
**Goal:** Professional-grade Motion Orchestrator.
- Timeline UI (Adobe-style layers).
- Framer Motion & GSAP integration.
- Scroll-triggered reveal sequences.

### Phase 14: Utility Suite
**Goal:** Completing the management tools.
- Form Builder with GFC Wizard logic.
- Asset Manager (Image optimization pipeline).
- SEO & Performance scoring dashboard.

### Phase 15: Migration & Import
**Goal:** Content ingestion from existing sources.
- URL Scraper (Legacy site ingestion).
- Format conversion (HTML to Studio Blocks).

---

## 🏁 Success Criteria
- **Architecture:** Decoupled frontend (Next.js) and editor (Blazor).
- **Performance:** 95+ Lighthouse score on generated pages.
- **Simplicity:** Zero-code daily updates for Club Directors.
