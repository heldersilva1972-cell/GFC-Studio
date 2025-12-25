# GFC Website - Master Plan (Revision 3)

**Version:** 1.0.0 (R3)  
**Date:** December 25, 2025  
**Status:** ✅ Infrastructure Complete | 🛠️ Data Integration Active  

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
3. **Operational Shell**:
    - Header with sticky navigation.
    - Footer with business hours and social links.
    - **Status Indicator** UI component (Open/Closed).
    - **Update Bar** UI component (Scrolling news).

### 🛠️ REMAINING (The "Missing Links")
1. **Live Data Integration**:
    - [ ] Replace `getClubStatusFromWebApp` mock with real Fetch call to `http://localhost:5207/api/WebsiteSettings`.
    - [ ] Connect `Announcements` API to the SQL `SystemNotifications` table.
    - [ ] Connect `Events` API to the SQL `AvailabilityCalendars` table.
2. **Hall Rental Flow**:
    - [ ] Build `/hall-rentals` multi-step page.
    - [ ] Implement the `HallRentalInquiry` submission to the backend.
3. **Social Proof Moderation**:
    - [ ] Implement the "Approve/Reject" logic for member reviews in the Web App dashboard.

---

## ⚙️ Operational Control (Factual Integration Map)

| Feature | Data Source (SQL Table) | Current Status |
| :--- | :--- | :--- |
| **Club Status** | `SystemSettings` | 🏗️ Mocked (Morning=Open) |
| **News Ticker** | `SystemNotifications` | 🏗️ Mocked (Hardcoded) |
| **Hall Availability** | `AvailabilityCalendars` | 🏗️ Logic Pending |
| **Event Cards** | `EventPromotions` | 🏗️ UI Built / Data Mocked |

---

## 🛠️ Technology Stack
- **Framework:** Next.js 14.
- **Styling:** CSS Modules & Design Tokens (Refined V2).
- **Animations:** Framer Motion (Slow reveals).
- **Bridge:** API Proxy to Blazor Server (Port 5207).
