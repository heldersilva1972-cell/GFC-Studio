# GFC Web Application: Members Info User Manual & Standard Operating Procedure

---

## Document Control & Purpose
* **Module:** Membership Operations
* **Primary URL:** `/members`
* **Target Audience:** Club Executive Officers, Membership Secretaries, Office Staff
* **Purpose:** This manual provides an exhaustive, non-technical, step-by-step procedure for searching, viewing, adding, editing, and managing member records. It covers membership status management, contact updates, member ID assignment, and roster generation.

---

## 1. How to Navigate to the Members Info Page

Follow these exact steps from anywhere inside the web application:

1. **Locate the Left Navigation Sidebar:**
   Look at the dark sidebar menu running down the left side of your browser.
2. **Find the Operations Section:**
   Scroll to the **OPERATIONS** workspace grouping.
3. **Open Membership:**
   Click directly on the header labeled **MEMBERSHIP** to expand its items.
4. **Click "Members Info":**
   Click on the item labeled **Members Info** (accompanied by the outline icon of two people <i class="bi bi-people"></i>).
5. **Confirmation:**
   The page loads with the title **Members** and a live roster table.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the left sidebar showing the "MEMBERSHIP" menu expanded with a red box or arrow pointing to "Members Info".*

---

## 2. Visual Layout & Screen Tour

```
+-----------------------------------------------------------------------------------------------+
| Page Title: Members                                       [ + Add Member btn ] [ Export v ]   |
+-----------------------------------------------------------------------------------------------+
| [ Total: 420 ]    [ Regular: 250 ]    [ Reg-NP: 50 ]    [ Life: 70 ]    [ Inactive: 50 ]      |
+-----------------------------------------------------------------------------------------------+
| [ All ] [ Regular ] [ Regular-NP ] [ Life ] [ Social ] [ Inactive ]   | Search: [ John Silva ]|
+-----------------------------------------------------------------------------------------------+
| MEMBER #  | NAME               | STATUS     | PHONE          | JOIN DATE   | ACTIONS          |
+-----------+--------------------+------------+----------------+-------------+------------------+
| #1042     | Silva, John M.     | Regular    | (508) 555-0123 | Oct 12 2014 | [View] [Edit]    |
| #1043     | Costa, Paul A.     | Regular-NP | (508) 555-0456 | Jan 15 2018 | [View] [Edit]    |
| #0512     | Santos, Manuel     | Life       | (508) 555-0789 | Jun 01 1995 | [View] [Edit]    |
+-----------------------------------------------------------------------------------------------+
```

### Zone 1: Summary Statistics Bar
Across the top of the page, metric cards summarize current club demographics:
* **Total Members:** Overall count of all living and historical profiles in the database.
* **Regular & Regular-NP:** The core voting membership counts.
* **Life & Honorary Life:** Senior members who have completed requisite tenure.
* **Social / Inactive:** Auxiliary or historical accounts.

### Zone 2: Filter Tabs & Real-Time Search Bar
* **Status Filter Tabs:** Click any tab (*All*, *Regular*, *Regular-NP*, *Life*, *Social*, *Inactive*) to filter the table to that specific group instantly.
* **Instant Search Input:** Located at the top right above the table. Type any name, number, or phone number to filter rows on the fly without refreshing the page.

### Zone 3: The Members Table
* **Member # Column:** Displays the unique club badge/membership number.
* **Name Column:** Displayed as `Last Name, First Name Middle Initial`.
* **Status Badge:** Color-coded status badge (Green for Regular/Life, Orange for Social, Gray for Inactive).
* **Phone & Join Date:** Primary contact number and date joined.
* **Actions Column:** Action buttons to **View** full profile or **Edit** directly.

> 📸 **[SCREENSHOT SUGGESTION]:** *Wide-angle screenshot of the full Members Info page showing the metrics bar, filter tabs, search bar, and the members list table.*

---

## 3. Step-by-Step Operating Procedures

### Procedure A: Searching & Finding a Member Rapidly
1. Click into the **Search** box located directly above the table on the right.
2. Type any search criteria:
   * **By Name:** Type either last name or first name (e.g., *"Silva"*).
   * **By Member Number:** Type the badge number (e.g., *"1042"*).
   * **By Phone Number:** Type the last 4 digits or area code (e.g., *"0123"*).
3. The table filters automatically as you type.
4. To reset your search, click the small **"X"** inside the search box or delete your text.

> 📸 **[SCREENSHOT SUGGESTION]:** *Close-up of the Search Bar with the name "Silva" typed in, showing filtered search results below.*

---

### Procedure B: Adding a New Member to the Club
When an applicant is approved for membership:

1. Click the blue **"+ Add Member"** button in the upper right corner of the page.
2. A window titled **"Add New Member"** will appear.
3. Fill in the required fields:
   * **First Name & Last Name:** (Required).
   * **Middle Name / Suffix:** *(Optional)* (e.g., Jr., III).
   * **Membership Status:** Choose from the dropdown:
     * *Regular* (Portuguese heritage voting member)
     * *Regular-NP* (Non-Portuguese quota voting member)
     * *Social* (Non-voting social privileges)
   * **Member Number:** The system automatically suggests the next available number, or you can enter a specific physical badge number.
   * **Join Date:** Enter the official date of induction/approval.
   * **Date of Birth:** (Highly recommended for tracking Life membership age rules).
4. Fill in the contact details:
   * **Street Address, City, State, Zip Code.**
   * **Primary Mobile Phone & Email Address.**
5. Click **"Save Member"** at the bottom of the window.
6. The window closes, a green notification confirms creation, and the new member appears in the roster.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Add New Member" popup form with sample information filled in, showing the "Save Member" button.*

---

### Procedure C: Viewing & Editing a Member's Profile
1. Search for the member in the main table.
2. Click on the member's row, or click the **"View"** button in the Actions column.
3. The **Member Detail** page opens, displaying:
   * Full contact information.
   * Door access key card status.
   * Multi-year dues payment history.
   * Complete audit trail of profile changes.
4. To make changes, click the **"Edit Profile"** button.
5. Update any phone numbers, mailing addresses, or contact information.
6. Click **"Save Changes"**.
7. The system logs the exact change, your username, and the timestamp in the member's permanent history.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the Member Detail page showing the contact overview card, key card badge, and "Edit Profile" button.*

---

### Procedure D: Changing a Member's Status (e.g., Promotion to Life, Inactivation)
1. Navigate to the member's detail page.
2. Click the **"Edit Status"** button beside their current status badge.
3. Select the new status from the dropdown list:
   * **Regular $\rightarrow$ Life:** If promoted to senior Life status.
   * **Active $\rightarrow$ Inactive:** If the member has resigned or allowed membership to lapse.
   * **Active $\rightarrow$ Deceased:** If the member has passed away.
4. Enter the **Effective Date** and any reference notes.
5. Click **"Update Status"**.
   * **Important System Reaction:** If changed to Inactive or Deceased, the system automatically deactivates their door key card and terminates building access.

---

### Procedure E: Exporting Member Rosters & Mailing Lists
1. In the top right corner of the page, click the **"Export"** button.
2. Choose your preferred format from the dropdown:
   * **Export to Excel (.xlsx):** Generates a spreadsheet suitable for mail merges or official records.
   * **Export to CSV:** Plain text data export.
   * **Printable Directory (PDF):** Cleanly formatted membership roster ready for physical printing.
3. Your browser will download the file immediately.

> 📸 **[SCREENSHOT SUGGESTION]:** *Dropdown menu showing "Export to Excel", "Export to CSV", and "Printable Directory".*

---

## 4. Key Rules & Best Practices

1. **Never Delete Records:**
   * Always set departing members to **Inactive** or **Deceased** rather than deleting them. Retaining the record preserves historical financial audits, dues receipts, and door access event logs.
2. **Birth Dates are Critical:**
   * Entering accurate birth dates enables the system to calculate **Life Eligibility** automatically, saving officers hours of manual research.
3. **Audit History:**
   * Every edit made to a member is logged under their profile history. Administrators can always see who changed an address or status and when.
