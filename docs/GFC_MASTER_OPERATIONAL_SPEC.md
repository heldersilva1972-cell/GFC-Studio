# GFC ECOSYSTEM – MASTER OPERATIONAL SPECIFICATION
**STATUS:** FINALIZED / AUTHORITATIVE
**DOCUMENT TYPE:** Full Implementation Guide
**LAST UPDATED:** 2025-12-22

---

## 1. CORE ARCHITECTURE
**"Design in Studio, Command in Web App, Action in Mobile App"**
The system is built as a unified ecosystem sharing a single database brain. All modules must respect the "Safety Air-Gap"—the Public Website never touches private Member data; it only reads from the Studio/Content tables.

---

## 2. GFC STUDIO (The Visual Builder)
A professional-grade editor with built-in design intelligence.

### **Editor Workspace**
*   **Structured Canvas:** Drag-and-drop builder using "Horizontal Strips" (Sections) to ensure mobile friendliness.
*   **Animation Orchestrator:** A video-editor style timeline at the bottom to sequence fades/slides/parallax.
    *   **Stagger Button:** One-click to make grid items appear sequentially (Entry Delay per item).
*   **Skeleton Preview Mode:** Toggle to view "Small/Medium/Large" loading shapes to verify the premium feel during page loads.
*   **Collaboration Mode:** "Sticky Note" system to place comments on the canvas for other Directors to review.
*   **The Recycle Bin:** 30-day "Trash Can" allowing recovery of deleted sections or entire pages.

### **The "Wizard" Suite (AI Assistant)**
*   **Layout Doctor:** Identifies broken grids/tables on mobile and offers: *Stack, Carousel, or Grid* with live previews.
*   **Touch Target Sizer:** Automatically highlights buttons in **Red** if they are too close together for mobile fingers.
*   **Wall-of-Text Condenser:** Warns about long mobile paragraphs and offers AI summarization or "Read More" expanders.
*   **Smart Focus Dot:** Allows the user to place a "Focus Point" on an image (e.g., a person's face) so auto-cropping always keeps it centered.
*   **Load Time Estimator:** Real-time "Speed Meter" that warns if high-res assets will slow the site.
*   **SEO Health Bar:** A 0-100 real-time score auditing Title tags, Descriptions, and Heading (H1, H2) hierarchy.

---

## 3. WEB APP (Mission Control & Command Center)
The operational heart of the club where data is managed without design complexity.

### **Site Management (Content & Rules)**
*   **Conflict Detection:** Web App warns if two Hero Banners or Alerts are scheduled for the same time/slot.
*   **Recurring Schedules:** Set menus/banners to show on patterns (e.g., "Every Friday at 3 PM").
*   **Emergency Kill Switch:** Instant removal of any scheduled banner/alert with a single click.
*   **Theme Manager:** Global control of "Club Gold" and "Navy Blue." Studio inherits these instantly.
*   **Global Variables:** Centralized `{ClubPhone}`, `{ClubAddress}`, and `{ClubEmail}` tags used site-wide.
*   **Tile Manager:** Assign predefined "Skins" (designed in Studio) to Event Tiles via simple dropdown.

### **Hall Rental Module**
*   **Command Calendar:** Admin view showing Approved (Red), Pending (Yellow), and Internal (Private Notes) dates.
*   **Pricing Engine:** Automatically applies Member vs. Non-Member rates based on database lookup.
*   **Rule Enforcement:** Digitized logic from the "Original GFC Site" (Capacity: 180, Cleaning Deposit required, etc.).
*   **Automated In Memoriam:** A toggle on Member Profiles that instantly adds names to the public "Deceased" list.

### **Unified Notification Hub**
*   **Multi-Channel Dispatcher:** Choose "Bell Icon", "Push", "Email", or "SMS".
*   **Adapter Architecture:** Built so that any SMS/Email provider can be plugged in later without code changes.
*   **Master Silence Switch:** Instantly disable all outgoing automated communications.

---

## 4. MOBILE OPS APP (PWA - Directors & Staff)
Role-specific tool for daily club tasks.

### **Bartender Role (The "Data Entry")**
*   **Simple Shift Selection:** Large buttons for "1st Shift (Day)" or "2nd Shift (Night)." No time-clocking.
*   **Reporting:** Instant Bar Sales ($) and Lottery Sales ($) entry.
*   **Digital Receipt:** Generates a "Net Cash Drop" summary for the bartender's records.
*   **Late Entry Flag:** If report is submitted after the deadline, it is marked for audit.

### **Director Role (The "Pocket Manager")**
*   **Rental Approver:** View full rental applications and click Approve/Deny from the phone.
*   **The Watchdog Alert:** Automatic notification if a Bartender fails to submit a report by the deadline (e.g. 3:01 AM).
*   **Staff Scheduler:** Simple weekly view with the ability to "Swap" staff for sick days.
*   **QR Check-In:** Built-in scanner to verify Digital Tickets/Reservations at the door.
*   **Vendor Directory:** Quick-tap contact list for Plumbers, Distributors, and Electricians.

---

## 5. REFINEMENTS & ASSETS
*   **Smart Gallery:** Web App handles "Bulk Uploads" and "Captions"; Studio handles "Masonry/Lightbox" display.
*   **Lobby/Member Bar TV:** A specific hidden URL (`/tv/lobby`) that plays the "Signage Playlist" of slides and events.
*   **Digital Box Office:** Reservation system with logic to generate PDF tickets with Unique IDs.

---

## 6. IMPLEMENTATION PHASES
1.  **Phase 1:** Database Schema (The Foundation for all systems).
2.  **Phase 2:** Website & Web App Integration (Rentals, Memorials, Events Sync, and Admin Dashboards).
3.  **Phase 3:** Studio Builder (Visual Canvas, Wizards, Timeline).
4.  **Phase 4:** Advanced Extras (TV Signage, Ticketing, and Payment Prep).
5.  **Phase 5:** Mobile Operations App (Bartender Entry, Director Mobile Approvals, and Watchdog).
