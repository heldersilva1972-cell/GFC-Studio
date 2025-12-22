# GFC STUDIO & ECOSYSTEM – LIVING SPECIFICATION
**STATUS:** DRAFT / LOCKED CONCEPT
**LAST UPDATED:** 2025-12-22
**PURPOSE:** The authoritative source of truth for the GFC Ecosystem (Studio, Web App, Website, Mobile App). Updated based on user discussions to defining the complete system vision.

---

## 1. CORE ARCHITECTURE & PHILOSOPHY
**"Design in Studio, Command in Web App, Action in Mobile App"**

*   **Studio's Role (The Architect):** The creative engine. Used to build visual layouts, templates, and animations.
*   **Web App's Role (Mission Control):** The central brain. Used to schedule content, manage business data (Rentals, Staff, Inventory), and control the website's behavior.
*   **Mobile App's Role (Operations):** The "Boots on the Ground" tool. Used by staff for reporting, scheduling, and approvals.
*   **Website's Role (The Face):** The public display. Read-Only display of data provided by the other systems.

---

## 2. STUDIO EDITOR (Visual Builder)
*   **Interface:**
    *   **Left Toolbox:** Drag & Drop Blocks (Hero, Grid, Text).
    *   **Center Canvas:** Live preview with Desktop/Tablet/Mobile toggles.
    *   **Right Inspector:** Properties (Colors, Text) + Animation Settings.
    *   **Bottom Timeline:** Animation Orchestrator for sequencing (e.g., "Wait 0.5s then slide").
*   **Interaction Logic:**
    *   **Vertical Stacking:** Blocks stack vertically to ensure mobile robustness.
    *   **Shared Content:** Text updates apply to all device views.
    *   **Independent Layout:** Mobile can use different layouts (Stack) vs Desktop (Grid).
*   **The "Wizard" (AI Assistant):**
    *   **Layout Doctor:** Detects broken mobile layouts and offers visual fixes (Stack vs Carousel).
    *   **Mobile Condenser:** Offers to summarize long text for phones.
    *   **Smart Focus:** Ensures image crops keep faces visible.
    *   **Brand Guard:** Restricts visual choices to the global theme.
    *   **SEO/Accessibility:** Real-time checking for contrast and metadata.

---

## 3. WEB APP (Mission Control & Data)
The "Business Logic" center that feeds the website.

*   **Content Scheduling (The "Calendar"):**
    *   Dashboard to view "What is live on the site?".
    *   Control Start/End dates for Banners, Alerts, and Pages devised in Studio.
    *   **Conflict Detection:** Warns if two banners compete for the same slot.
*   **Event Tiles Manager:**
    *   Create "Event Cards" (Title, Date, Image).
    *   Apply Studio-designed "Frames" (Holiday, Gold, Standard).
    *   Set Countdowns/Expirations.
*   **Hall Rental Manager:**
    *   **Calendar:** Admin view of all requests.
    *   **Workflow:** Approve/Deny requests.
    *   **Configuration:** Set Block-out dates and Pricing Tiers (Member/Non-Member).
*   **Notification Hub:**
    *   Central dispatcher for Alerts.
    *   **Channels:** Web App Bell, Mobile Push, Email, SMS (Toggleable).
    *   **Safety:** "Master Kill Switch" for external comms.
*   **Theme & Menu Manager:**
    *   Set Global Colors/Fonts (Studio inherits these).
    *   Build the Navigation Tree (Drag and drop menu items).
*   **Asset Managers:**
    *   **Photo Gallery:** Bulk upload photos -> Fills Studio-designed "Smart Gallery" blocks.
    *   **Documents:** Upload Minutes/Bylaws with member-only gating.
    *   **Menus:** Manage Bar/Kitchen prices and "Out of Stock" status.
    *   **Staff Schedule:** Drag-and-drop weekly calendar for bartenders.

---

## 4. MOBILE APP (Operations - PWA)
Role-based Progressive Web App for internal staff.

*   **Bartender Mode:**
    *   **Shift Entry:** Pick Shift (Day/Night) -> Enter Cash/Lotto Sales -> Submit.
    *   **Schedule View:** See upcoming shifts.
*   **Director Mode ("Pocket Command"):**
    *   **Approvals:** Approve Hall Rentals or Op Reports on the go.
    *   **Reports:** View real-time sales/financials.
    *   **Schedule Manager:** Edit/Swap staff shifts.
    *   **Member Lookup:** Access Vendor list and Member Directory.
*   **Alerts System:**
    *   **Missing Report Watchdog:** Alerts Directors if a shift report is not submitted by the deadline.

---

## 5. PUBLIC WEBSITE (The Display)
*   **Hall Rentals:**
    *   "Fancy Calendar" with interactive availability.
    *   Application Wizard: Validates rules (Capacity, Pricing) based on "Original GFC Site" logic.
*   **Digital Box Office:**
    *   Reserve tickets/RSVP for events.
    *   Generate QR/PDF tickets.
*   **Smart Features:**
    *   Live "Open/Closed" status.
    *   Smart Galleries (Masonry/Grid).

---

## 6. IMPLEMENTATION ROADMAP (Phases)

*   **PHASE 1: FOUNDATION (Database)**
    *   Create DB Schemas for *all* systems: `Rentals`, `Shifts`, `Schedules`, `Studio_Content`, `Notifications`.

*   **PHASE 2: OPERATIONS (High Value)**
    *   Build **Web App Managers** for Rentals & Staff.
    *   Build **Mobile Op App** (Bartender Reporting & Director Approvals).

*   **PHASE 3: PUBLIC SITE (The Face)**
    *   Connect Website to DB for Rentals, Events, and Galleries.
    *   Implement "Application Wizard" logic.

*   **PHASE 4: STUDIO EDITOR (The Creative)**
    *   Build the Visual Builder, Timeline, and Wizard tools.

*   **PHASE 5: EXTRAS**
    *   TV Digital Signage.
    *   Inventory Tracking.
    *   Advanced Ticketing Payments.

---

## 7. OPEN ITEMS FOR FUTURE DISCUSSION
*   Specific data fields for the Hall Rental Contract.
*   Details of the TV Signage "Playlist" logic.
*   Specific rules for Member vs. Non-Member pricing algorithms.
