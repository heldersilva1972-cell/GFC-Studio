# GFC Website - Web App Control Integration

**Version:** 1.0.0  
**Date:** December 24, 2024  
**Status:** Mandatory Specification  
**Purpose:** Defining the bridge between internal club operations (Web App) and the public-facing website.

---

## 🔗 The "Control" Philosophy

The GFC Website is designed to be **Static for Speed** but **Dynamic for Operations**. Administrators should never have to use the "Studio" for daily updates. All operational data is served via an API from the GFC Web App.

---

## 📡 1. Live Club Status (Open/Closed)

A real-time indicator on the website header/home page.

### Web App Control:
- **Location:** Admin Dashboard / Club Settings.
- **Control:** Simple Toggle (ON/OFF).
- **Secondary Control:** "Temporary Close" timer (e.g., "Closed until 5 PM today").

### Website behavior:
- **Green Indicator:** "Currently Open - Come visit us!"
- **Red/Yellow Indicator:** "Currently Closed - See you at [Next Open Time]."

---

## 📢 2. Announcements & News Engine

Handling the "Update Bar" and "Featured News Card."

### Web App Control:
- **Management Interface:** "News & Updates" Table.
- **Fields:**
  - `Title`: Short headline.
  - `Content`: Detailed text.
  - `Priority`: (Alert Bar vs. Homepage Card).
  - `Start Date / End Date`: Automatic scheduling (Self-cleaning).
- **Action:** If an announcement is labeled "Alert," it automatically appears in the thin bar at the top of the website.

---

## 📅 3. Calendar & Hall Availability

This is the most critical integration. It prevents double-booking and informs the public.

### Operational Logic:
1. **Public View:** The website pulls from the `Events` and `Bookings` tables.
2. **Availability Rule:** If a date has a "Confirmed" or "Reserved" status in the Web App, the website calendar displays that date as **Booked/Reserved**.
3. **No Overlap:** The Rental Form on the website automatically blocks users from selecting a date that is already Reserved in the database.

### Web App Management:
- **Approvals:** When a new application comes in from the website, it appears in the Web App "Pending Approvals" list.
- **Conversion:** Once an admin hits "Approve," the website calendar updates instantly to show the date is taken.

---

## ⭐ 4. Social Proof (Review Moderation)

A system to collect public feedback and display only the best highlights.

### The Workflow:
1. **Submission:** Public site has a "Share Your Experience" form.
2. **Storage:** Submissions enter a `PendingReviews` table (Hidden from public).
3. **Moderation (Web App):**
   - Admin views the list of reviews.
   - Admin clicks a **"Featured" toggle** on the best ones.
4. **Display:** The Website "Social Proof" section only queries reviews where `IsFeatured = true`.

---

## 📂 5. History & Media Updates

Allows the club to update the "Impact" gallery easily.

### Web App Control:
- **Media Gallery Manager:** Upload photos of recent events (Children's parties, etc.).
- **Categorization:** Tagging photos as "Community Impact" causes them to automatically populate the History/Impact section of the website.

---

## 🔗 6. Legacy Route Manager (301 Redirects)

Ensures that old search engine results for `gloucesterfraternityclub.com` do not break.

### Web App Control:
- **Interface:** Simple "Source -> Destination" mapping table.
- **Example:**
  - `hall-rental-application.html` → `/hall-rentals`
  - `gfc-history.php` → `/our-impact`
  - `contact-us/` → `/contact`

### Website Behavior:
Next.js checks this table at the middleware layer. If a user hits an old URL, they are instantly and permanently redirected to the correct new page.

---

## 🛠️ Technical Flow

```
[ GFC WEB APP ] (Data Owner)
      ↓
[ SQL DATABASE ] (Central Truth)
      ↓
[ NEXT.JS API ROUTE ] (Data Fetcher)
      ↓
[ GFC WEBSITE ] (Data Displayer)
```

**Key Requirement:** The Website will use **On-Demand Revalidation**. When an Admin updates one of these items in the Web App, the Web App sends a "web-hook" to the Website to refresh that specific data in the background.
