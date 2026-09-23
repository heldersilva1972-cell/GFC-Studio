# Google Calendar & Service Account Integration Guide

This guide provides step-by-step instructions for connecting Google Calendar with **GFC Studio**. Following these steps enables:
1. **Real-time Event Sync (Reading)**: Pulling all club bookings and private events directly into GFC Studio.
2. **Automated Event Creation (Writing)**: Allowing GFC Studio to automatically write approved hall rentals and sandbox simulations directly into Google Calendar.

---

## Architecture Overview

```
 ┌───────────────────────────────────────────────────────────┐
 │                        GFC Studio                         │
 └─────────────┬───────────────────────────────┬─────────────┘
               │                               │
       (1-Way iCal Read)               (2-Way Write API)
   Pulls live event feeds         Pushes bookings & test events
               │                               │
               ▼                               ▼
 ┌───────────────────────────┐   ┌───────────────────────────┐
 │ Google Calendar iCal Feed │   │ Google Cloud Service Acct │
 └───────────────────────────┘   └───────────────────────────┘
```

- **iCal Feed (Read Only)**: Uses a secure calendar web address (`.ics`) to display events. **No Google Cloud account needed.**
- **Service Account (Read & Write)**: Uses a secure JSON key file from Google Cloud to add, update, and manage events programmatically. **100% free.**

---

## Phase 1: Google Cloud Console Setup (One-Time Setup)

> **Note**: This phase only needs to be completed once. The resulting JSON key file can be used for both your **Test/Sandbox Calendar** and your **Live Production Calendar**.

### Step 1: Open Google Cloud Console
1. In your web browser, navigate to: [https://console.cloud.google.com/](https://console.cloud.google.com/)
2. Sign in with the Google Account that manages (or has access to) the club’s calendars.

---

### Step 2: Create a Dedicated Project
1. In the top navigation bar, click the **Project Dropdown** (located next to the "Google Cloud" logo).
2. In the popup dialog, click **NEW PROJECT** in the top-right corner.
3. Enter the project details:
   - **Project name**: `GFC-Studio`
   - **Organization**: Leave as *No organization* (or select your organization if applicable).
4. Click **CREATE** and wait 5–10 seconds for Google Cloud to provision the project.
5. Click the project dropdown at the top again and select your newly created **GFC-Studio** project.

---

### Step 3: Enable the Google Calendar API
1. In the top search bar, type: `Google Calendar API` and press **Enter**.
2. From the search results, click on **Google Calendar API** (Marketplace).
3. Click the blue **ENABLE** button.
4. Wait a few seconds until the API status displays as **Enabled**.

---

### Step 4: Create a Service Account (The GFC Studio "Robot Account")
1. Click the main navigation menu icon (**☰** in the top-left corner).
2. Go to **IAM & Admin** → **Service Accounts** (or type `Service Accounts` in the top search bar).
3. Click the **+ CREATE SERVICE ACCOUNT** button at the top.
4. Fill in **Step 1 (Service account details)**:
   - **Service account name**: `gfc-calendar-sync`
   - **Service account ID**: Automatically generates (e.g. `gfc-calendar-sync@gfc-studio-XXXXX.iam.gserviceaccount.com`).
   - **Service account description**: `Automated calendar sync for GFC Studio Hall Rentals`.
5. Click **CREATE AND CONTINUE**.
6. **Step 2 & Step 3 (Grant roles/permissions)**: These are optional for Calendar API. Click **DONE** to finish creating the service account.

---

### Step 5: Generate & Download the Service Account JSON Key
1. In the Service Accounts list table, find the account you just created.
2. Click on the **Email address** of the service account (e.g. `gfc-calendar-sync@...`).
3. In the top tab bar, click on the **KEYS** tab.
4. Click **ADD KEY** → **Create new key**.
5. In the popup modal:
   - Select **JSON** (the default option).
   - Click **CREATE**.
6. A `.json` file will automatically download to your computer (e.g. `gfc-studio-XXXXX-XXXXXXXX.json`).
7. **Important**: Save this file in a safe location. This is your authentication key.

---

## Phase 2: Google Calendar Configuration (For Your Live or Test Calendar)

> **Important**: Google requires you to explicitly grant your Service Account permission on the specific Google Calendar you want it to write to.

### Step 1: Open Google Calendar
1. Navigate to [https://calendar.google.com/](https://calendar.google.com/) in your browser.
2. Ensure you are logged into the account that owns the calendar.

---

### Step 2: Grant Write Permissions to the Service Account
1. Look at the left sidebar under **My calendars**.
2. Hover over the calendar you want GFC Studio to write to (e.g., **"GFC Hall Rentals"** or your test calendar).
3. Click the **three vertical dots (⋮)** next to the calendar name and select **Settings and sharing**.
4. Scroll down to the **"Share with specific people or groups"** section.
5. Click **+ Add people and groups**.
6. In the modal:
   - **Email**: Paste the Service Account email address (e.g. `gfc-calendar-sync@gfc-studio-XXXXX.iam.gserviceaccount.com` from Phase 1).
   - **Permissions**: Change the dropdown from *"See all event details"* to **"Make changes to events"** (or *"Make changes and manage sharing"*).
7. Click **Send**.

---

### Step 3: Copy the Calendar ID
1. On that same calendar settings page, scroll down to the **"Integrate calendar"** section.
2. Locate the **Calendar ID** field.
   - **For Secondary / Test / Group Calendars**: This will look like:
     ```text
     c_abc123456789xyz...@group.calendar.google.com
     ```
     *(Always copy this exact ID for secondary or club calendars!)*
   - **For your Primary Personal Calendar**: This looks like your Google email (e.g. `your-email@gmail.com`).
3. Copy this Calendar ID to your clipboard.

---

### Step 4: Copy the iCal Feed URL (For Read Sync)
1. In the same **"Integrate calendar"** section, scroll down slightly.
2. Locate:
   - **Secret address in iCal format** (recommended for private/internal calendars), OR
   - **Public address in iCal format** (if your calendar is set to public).
3. Click the copy icon next to the URL (starts with `https://calendar.google.com/calendar/ical/.../basic.ics`).

---

## Phase 3: GFC Studio Configuration

### A. Configuring the Live Production Calendar
1. In GFC Studio, navigate to **Hall Rentals** → click the blue **Settings** button in the top-right header.
2. Go to the **Calendar Feeds & Sync** tab:
   - Under **Configured Calendar Feeds**, click **Edit** or **Add Another Calendar**.
   - Paste your **Live Calendar iCal URL** into the URL field.
   - Give it a name (e.g., `Main Hall Rentals`) and select a badge color.
3. Under **Google Calendar API Credentials (2-Way Write Sync)**:
   - Upload your downloaded `.json` key file from Phase 1.
   - Enter your **Primary Google Calendar ID** (from Phase 2, Step 3).
   - Click **Test Write Permissions** to verify the green success checkmark.
4. Click **Save All Settings**.

---

### B. Configuring the Sandbox & Testing Calendar
1. In GFC Studio, open **Hall Rentals** → click **Settings** → go to the **🧪 Sandbox & Testing Hub** tab.
2. Ensure the **Sandbox Master Switch** is toggled **ON**.
3. In **Section 1 (Connect Your Test Google Calendar)**:
   - **iCal Feed URL**: Paste your Test Calendar's iCal URL.
   - **Test Google Calendar ID**: Paste your Test Calendar's Calendar ID (`c_...` or email).
   - **Service Account Key**: Upload the `.json` key file if not already uploaded.
4. Click **Test Write Permissions** to verify access.
5. In **Section 2 (Interactive Form Simulator)**:
   - Click **Simulate Wedding**, **Simulate Birthday**, or submit a custom test booking.
   - Open Google Calendar in your browser: the test event will appear on your calendar marked with `[TEST] PENDING: ...`.

---

## Troubleshooting & Verification Matrix

| Issue / Error Message | Root Cause | Solution |
| :--- | :--- | :--- |
| **"Google API error (404 Not Found)"** | 1. The **Calendar ID** is blank in GFC Studio.<br>2. You copied the iCal link instead of the Calendar ID.<br>3. The Service Account email was not invited on that calendar. | 1. In Google Calendar → *Integrate calendar*, copy the **Calendar ID** (looks like `c_...@group.calendar.google.com` or `...@group.calendar.google.com`) and paste into GFC Studio.<br>2. Ensure the Service Account email is added under *Share with specific people*. |
| **"Google API error (403 Forbidden)"** | Service Account permission is set to "See details" instead of "Make changes". | In Google Calendar → Settings → Share with specific people, change permissions to **"Make changes to events"**. |
| **"Could not generate OAuth2 access token"** | The `.json` key file is corrupt or expired. | Go back to Google Cloud Console → Service Accounts → Keys → Generate a new JSON key. |
| **"Events appear in GFC Studio but not on Google"** | Read sync is working via iCal, but write API is disabled. | Upload the JSON key and enter the Calendar ID in Settings. |
