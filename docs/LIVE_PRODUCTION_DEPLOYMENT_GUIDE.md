# GFC Studio: Live Production Calendar & Website Embed Deployment Guide

This document contains step-by-step instructions for transitioning the **Gloucester Fraternity Club Hall Rental System** from testing/sandbox to full **Live Production**.

---

## 1. System Architecture: How It Works in Production

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                       PUBLIC APPLICANT / WEBSITE VISITOR                    │
│                 Visits: https://gloucesterfraternityclub.com                │
└──────────────────────────────────────┬──────────────────────────────────────┘
                                       │
                                       ▼ (Fills out embedded form)
┌─────────────────────────────────────────────────────────────────────────────┐
│                       EMBEDDED GFC RENTAL FORM (iFrame)                      │
│             Live dynamic pricing calculator + date availability check       │
└──────────────────────────────────────┬──────────────────────────────────────┘
                                       │
                                       ▼ (Instant submission POST)
┌─────────────────────────────────────────────────────────────────────────────┐
│                             GFC STUDIO ENGINE                               │
│  1. Saves Rental Application & Ledger record in Database (Status: Pending)  │
│  2. Dispatches Notification Email to Club Management Distribution List      │
│  3. Sends Confirmation Receipt to Applicant Email Address                   │
└──────────────────────────────────────┬──────────────────────────────────────┘
                                       │
                                       ▼ (2-Way Write API)
┌─────────────────────────────────────────────────────────────────────────────┐
│                         LIVE GOOGLE CALENDAR                                │
│       Automatically creates event: "PENDING: [Applicant] (Wedding)"         │
│          Contains full breakdown, guest count, phone, and total quote       │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 2. Part 1: Setting Up the Official Live Google Calendar

### Step 1: Open Google Calendar
1. Log in to **[Google Calendar](https://calendar.google.com)** using the official club account (e.g. `gfcclub1927@gmail.com`).

---

### Step 2: Grant Write Permission to Your Service Account
Google requires you to give GFC Studio permission to write events to the live calendar:

1. On the left sidebar under **My calendars**, find the official calendar (e.g., **"GFC Hall Rentals"** or primary club calendar).
2. Hover over the calendar name → click the **three vertical dots (⋮)** → select **Settings and sharing**.
3. Scroll down to the section named **"Share with specific people or groups"**.
4. Click **+ Add people and groups**.
5. Paste your Service Account email:
   ```text
   gfc-test-rental-calendar@gfc-test-rental-calendar.iam.gserviceaccount.com
   ```
   *(Or your production service account email created in Google Cloud Console)*.
6. In the **Permissions** dropdown, choose:
   👉 **"Make changes to events"** (or *"Make changes and manage sharing"*).
7. Click **Send**.

---

### Step 3: Copy the Live Calendar ID
1. On that same **Settings and sharing** page, scroll down to the **"Integrate calendar"** section.
2. Find the **Calendar ID** field:
   - For primary calendars, this is: `gfcclub1927@gmail.com`
   - For secondary / group calendars, this looks like: `c_xxxxxxxxxxxxxxxxxxxxxx@group.calendar.google.com`
3. Copy this **Calendar ID**.

---

### Step 4: Copy the Live iCal Feed URL
1. In the same **"Integrate calendar"** section, scroll down slightly.
2. Locate the **Secret address in iCal format** (or *Public address in iCal format* if the calendar is public).
3. Click the copy icon to copy the link (starts with `https://calendar.google.com/calendar/ical/.../basic.ics`).

---

### Step 5: Configure GFC Studio Live Settings
1. Open GFC Studio → go to **Hall Rentals** → click the blue **Settings** button in the top-right corner.
2. Open the **Calendar Feeds & Sync** tab:
   - In the feed list, ensure your **Live Calendar iCal URL** is entered with name `Main Hall Rentals` and marked **Enabled**.
   - Under **Google Calendar API Credentials (2-Way Write Sync)**:
     - Ensure the Service Account JSON key is uploaded.
     - In **Primary Google Calendar ID**, paste your Live Calendar ID from Step 3.
     - Click **Test Write Permissions** to verify the green success checkmark.
3. Open the **Production Emails** tab:
   - Enter your official club notification recipient list (e.g., `manager@gloucesterfraternityclub.com, board@gloucesterfraternityclub.com`).
   - Check all triggers:
     - ☑️ *Send alert on New Application Submission*
     - ☑️ *Send Applicant Automatic Confirmation Receipt*
     - ☑️ *Send alert when Payment or Deposit is Recorded*
4. Open the **Pricing Matrix & Fees** tab:
   - Verify official rates: Function Hall, Coalition Room, Youth Organization, Bartender Fee, Kitchen Fee, A/V Fee, and Security Deposit Amount.
5. Click **Save All Settings**.

---

## 3. Part 2: Embedding the Application Form on the Club Website

### The Embed URL
The standalone, public application form is hosted at:
```text
https://YOUR-GFC-DOMAIN/rentals/apply?embed=true
```

> The `?embed=true` parameter automatically removes navigation headers, sidebars, and admin controls, leaving a clean, branded form.

---

### The HTML Embed Code (Copy & Paste)

Paste this HTML snippet into your WordPress page or website builder:

```html
<!-- Gloucester Fraternity Club - Hall Rental Application Form -->
<div style="width: 100%; max-width: 900px; margin: 0 auto; overflow: hidden;">
    <iframe 
        id="gfc-rental-form-iframe"
        src="https://YOUR-GFC-DOMAIN/rentals/apply?embed=true" 
        width="100%" 
        height="1300" 
        frameborder="0" 
        scrolling="auto"
        style="border: none; width: 100%; min-height: 1100px; display: block; border-radius: 8px; box-shadow: 0 4px 12px rgba(0,0,0,0.08);"
        title="Gloucester Fraternity Club Hall Rental Application">
    </iframe>
</div>
```

---

### Step-by-Step for WordPress

#### Method A: Using the WordPress Block Editor (Gutenberg)
1. In WordPress Admin, navigate to **Pages** → **Add New Page** (or edit your existing **"Hall Rental"** page).
2. Click the **+ (Add Block)** button.
3. Search for **Custom HTML** and add the block.
4. Paste the HTML Embed Code above into the box (replace `https://YOUR-GFC-DOMAIN` with your actual live server URL or domain).
5. Click **Publish** or **Update**.

#### Method B: Using Elementor
1. Edit your page with Elementor.
2. In the widget panel on the left, search for the **HTML** widget.
3. Drag the **HTML** widget onto the page where you want the form to appear.
4. Paste the HTML Embed Code.
5. Click **Update**.

#### Method C: Using WordPress Classic Editor
1. In the editor toolbar, switch from the **Visual** tab to the **Text (HTML)** tab in the top-right corner.
2. Paste the HTML embed snippet where you want the form.
3. Click **Update**.

---

## 4. Part 3: What the Customer Sees & How Quotes Are Calculated

When visitors fill out the form on your website:

1. **Room Selection**:
   - **Main Function Hall** (Capacity: 150): Applies Function Hall non-member or member rate.
   - **Coalition Room** (Capacity: 50): Applies Coalition Room rate.
   - **Youth Organization Rate**: Flat non-profit/youth rate.
2. **Club Member Discount Checkbox**:
   - Checking "I am an active GFC Club Member" applies the lower member rates automatically.
3. **Add-On Amenities**:
   - Bartender service ($100)
   - Kitchen access ($50)
   - A/V sound system ($25–$50)
4. **Live Dynamic Quote Box**:
   - Updates in real-time as the applicant changes selections:
     $$\text{Total Price} = \text{Base Room Rate} + \text{Add-ons}$$
     $$\text{Security Deposit} = \$200\text{ (Refundable)}$$
5. **Rules & Agreement**:
   - Requires applicant to review and accept the official GFC Hall Rental Rules before the submit button unlocks.

---

## 5. Part 4: The Live Intake Workflow (Day-to-Day Operation)

### When an Application is Submitted:
1. **Database Record**:
   - Appears immediately in GFC Studio under **Hall Rentals** marked with yellow badge **`PENDING`**.
2. **Google Calendar Event Created**:
   - Title: `PENDING: John Smith (Wedding Reception)`
   - Time: Date and times chosen by applicant.
   - Description: Full applicant details (Phone, Email, Room, Guest Count, Bar, Kitchen, Total Quoted).
3. **Email Notification**:
   - Sent to the club manager distribution list with all event details.
   - Confirmation email sent to the applicant.

---

### When the Hall Manager Reviews & Approves:
1. Open GFC Studio → **Hall Rentals**.
2. Click on the pending application from the table or calendar grid.
3. **Recording Deposits / Payments**:
   - Click **"Record Payment / Deposit"**.
   - Select payment type: *Check (with check #)*, *Cash*, *Credit Card*, or *Venmo/Zelle*.
   - Enter amount (e.g. `$200` security deposit).
   - Click **Save Payment**. The payment ledger updates automatically with timestamp and staff name.
4. **Approving the Event**:
   - Click **"Approve Booking"**.
   - The status changes to **Approved**.
   - The Google Calendar event automatically updates from `PENDING: ...` to confirmed.

---

## 6. Part 5: Production Go-Live Checklist

Before making the form live on the club website:

- [ ] **1. Turn Sandbox Mode OFF**:
  - In GFC Studio → **Settings** → **🧪 Sandbox & Testing Hub**, switch the **Sandbox Master Switch** to **OFF (Live Production Mode)**.
  - Click **Save All Settings**.
- [ ] **2. Verify Live Calendar ID & iCal**:
  - Ensure the **Calendar Feeds & Sync** tab has the official club calendar ID and iCal URL.
- [ ] **3. Verify Staff Notification Emails**:
  - Ensure the **Production Emails** tab has the correct club manager and board emails.
- [ ] **4. Test Embed on Website**:
  - Open the website page in an incognito window and on a mobile phone to confirm the form fits cleanly.
- [ ] **5. Run One Real Test Submission**:
  - Submit a test application through the website form.
  - Verify it shows up in GFC Studio with status `Pending`.
  - Verify it appears on your Google Calendar.
  - Verify the notification email arrived.
  - Delete or approve the test booking in GFC Studio.
