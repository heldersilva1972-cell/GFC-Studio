# GFC Website - Master Plan (Revision 4)

**Version:** 1.2.0 (R5)  
**Date:** December 26, 2025  
**Status:** ✅ Infrastructure Complete | 🛠️ Data Integration Active  

## 📜 REVISION HISTORY

| Date | Version | Author | Description |
|:---|:---|:---|:---|
| 2025-12-25 | 1.0.0 | Helder Silva | Initial master plan creation |
| 2025-12-26 | 1.1.0 | Jules (AI Agent) | Updated status for Hall Rental Flow, Moderation, and Document Library |
| 2025-12-26 | 1.2.0 | Antigravity | Updated progress for Imagery, Dynamic Pricing, and Club Status |

---

## 🎯 Executive Summary
The new GFC Website is designed to be a **"Modern Legacy"** platform. It combines a premium, sleek aesthetic with robust operational features that are controlled directly from the GFC Web App.

---

## 🚦 Phase 11 Status Tracker

### ✅ COMPLETED
1. **Core Infrastructure**: 
    - Next.js 14+ `apps/website` environment installed.
    - TypeScript & ESLint configuration.
2. **Visual Implementation**:
    - **Midnight Blue / Charcoal** background system.
    - **Burnished Gold** accent markers.
    - **Animated Hero** with 3-step value proposition.
    - **Feature Grid** for club amenities (Full Bar, Parking, etc.).
    - **High-Resolution Imagery**: Hero photo and Hall Rental venue photos integrated with optimized styling.
3. **Operational Shell**:
    - Header with sticky navigation.
    - Footer with business hours and social links.
    - **Status Indicator** UI component (Open/Closed).
    - **Update Bar** UI component (Scrolling news).
4. **Data Integration**:
    - [x] Hall Rental Flow (3-step application wizard).
    - [x] Dynamic Hall Rental Pricing (Connected to SQL via WebsiteSettings).
    - [x] Hall Rental Inquiry backend submission.
    - [x] Review Moderation dashboard.
    - [x] Document Library management.

### 🛠️ REMAINING (The "Missing Links")
1. **Live Data Integration**:
    - [x] Replace `getClubStatusFromWebApp` mock with real Fetch call to `http://localhost:5207/api/WebsiteSettings`.
    - [ ] Connect `Announcements` API to the SQL `SystemNotifications` table.
    - [ ] Connect `Events` API to the SQL `AvailabilityCalendars` table.
3. **Social Proof Moderation**:
    - [x] Implement the "Approve/Reject" logic for member reviews in the Web App dashboard.

---

## ⚙️ Operational Control (Factual Integration Map)

| Feature | Data Source (SQL Table) | Current Status |
| :--- | :--- | :--- |
| **Club Status** | `SystemSettings` | ✅ Live (Syncs with Web App) |
| **News Ticker** | `SystemNotifications` | 🏗️ Mocked (Hardcoded) |
| **Hall Availability** | `AvailabilityCalendars` | 🏗️ Logic Pending |
| **Hall Pricing** | `SystemSettings` | ✅ Live (Dynamic) |
| **Event Cards** | `EventPromotions` | 🏗️ UI Built / Data Mocked |

---

## 🛠️ Technology Stack
- **Framework:** Next.js 14.
- **Styling:** CSS Modules & Design Tokens (Refined V2).
- **Animations:** Framer Motion (Slow reveals).
- **Bridge:** API Proxy to Blazor Server (Port 5207).
