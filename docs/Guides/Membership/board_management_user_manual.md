# GFC Web Application: Board Management User Manual & Standard Operating Procedure

---

## Document Control & Purpose
* **Module:** Membership Operations
* **Primary URL:** `/directors` (or `/board`)
* **Target Audience:** Club Executive Officers, Board Members, IT/System Administrators
* **Purpose:** This manual provides an exhaustive, non-technical, step-by-step procedure for managing officer and director assignments for any club term year. It covers election slate setup, mid-term vacancy handling, confirmation procedures, and the automatic system workflows triggered upon saving assignments.

---

## 1. How to Navigate to the Board Management Page

Follow these exact steps from anywhere inside the web application:

1. **Locate the Left Navigation Bar:**
   On the left side of the screen, you will find the main navigation menu sidebar.
2. **Expand the Operations Workspace:**
   Under the primary **OPERATIONS** grouping, look for the **MEMBERSHIP** dropdown section.
3. **Open the Membership Group:**
   If the menu is collapsed, click directly on the word **MEMBERSHIP** (or the small arrow <i class="bi bi-chevron-right"></i> beside it). The menu will expand downward to reveal all membership tools.
4. **Click "Directors":**
   Click on the item labeled **Directors** (accompanied by the people icon <i class="bi bi-people-fill"></i>).
5. **Confirmation:**
   The page will load, displaying the title **Board Management** at the top of the main window.

> 📸 **[SCREENSHOT SUGGESTION]:** *Capture the full browser window highlighting the left navigation sidebar with "OPERATIONS" -> "MEMBERSHIP" expanded, showing a red callout circle around the "Directors" menu link.*

---

## 2. Comprehensive Visual Layout & Screen Tour

The Board Management page is divided into five functional zones:

```
+-----------------------------------------------------------------------------------------------+
| Page Title: Board Management                             [ Term Year: 2027 v ] [ Current btn ]|
+-----------------------------------------------------------------------------------------------+
| [ 6 Positions ]        [ 6 Filled ]            [ 0 Vacant ]           [ 142 Eligible ]        |
+-----------------------------------------------------------------------------------------------+
|  Alert Banner: [ ! ] Board Not Confirmed / [ √ ] Board Confirmed            [ Confirm Now btn ]|
+-----------------------------------------------------------------------------------------------+
| +-------------------------+ +-------------------------+ +-------------------------+          |
| | President      [Filled] | | Vice President [Filled] | | Treasurer      [Filled] |          |
| | Currently: Silva, John  | | Currently: Santos, Paul | | Currently: Costa, Mark  |          |
| | Assign Member: [Select] | | Assign Member: [Select] | | Assign Member: [Select] |          |
| | [ Save ]      [ Clear ] | | [ Save ]      [ Clear ] | | [ Save ]      [ Clear ] |          |
| +-------------------------+ +-------------------------+ +-------------------------+          |
| +-------------------------+ +-------------------------+ +-------------------------+          |
| | Director #1    [Filled] | | Director #2    [Vacant] | | Director #3    [Filled] |          |
| | ...                     | | ...                     | | ...                     |          |
| +-------------------------+ +-------------------------+ +-------------------------+          |
+-----------------------------------------------------------------------------------------------+
```

### Zone 1: Page Header & Term Year Selector
Located in the upper right corner:
* **Term Year Dropdown:** Allows you to switch between past historical boards, the current year, or future election years (e.g., `2027`).
* **"Current" Button:** Only appears when you are inspecting a year other than today's calendar year. Clicking this instantly snaps the page back to the current active term.

### Zone 2: Live Metrics Counter Bar
Four summary stat cards provide an instant tally:
1. **Positions (Blue):** Total statutory board positions defined in club bylaws (President, Vice President, Treasurer, Collector of Dues, Recording Secretary, plus numbered Director seats).
2. **Filled (Green):** How many seats currently have an active member assigned.
3. **Vacant (Amber):** How many seats remain unassigned or open.
4. **Eligible (Cyan):** Total living members in the club whose membership status permits them to hold office under club bylaws.

> 📸 **[SCREENSHOT SUGGESTION]:** *Close-up of the Top Header and 4 Metric Cards showing "Positions", "Filled", "Vacant", and "Eligible".*

### Zone 3: Board Confirmation Banner
An alert bar running across the screen:
* **Amber Alert ("Board Not Confirmed"):** Displayed when the lineup for the selected year is still tentative or in progress. A button labeled **"Confirm Now"** is available on the right.
* **Green Alert ("Board Confirmed"):** Displayed once an authorized officer confirms the lineup. Shows the confirmation timestamp and the administrator's username.

### Zone 4: The Positions Grid
A responsive grid of position cards ordered by rank:
1. President
2. Vice President
3. Treasurer
4. Collector of Dues
5. Recording Secretary
6. Director #1 through Director #N (based on bylaws maximum seats)

Each card features:
* **Position Title & Status Badge:** Displays the title and a badge indicating **Filled** (green/solid) or **Vacant** (gray/dashed border).
* **Current Occupant Summary:** Displays the name of the member currently holding the seat.
* **"Assign Member" Dropdown:** A searchable list of all eligible members.
* **Save Button (Blue):** Lights up as soon as a change is made.
* **Clear Button (Red Outline):** Removes the current member and vacates the seat.

---

## 3. Step-by-Step Operating Procedures

### Procedure A: Selecting or Planning an Upcoming Term Year (e.g., 2027)
When club elections conclude in the fall, follow these steps to set up the new board before the new calendar year starts:

1. Look at the top right of the page for the **Term Year** selector box.
2. Click the dropdown menu.
3. Scroll and select **2027** (or the target year).
4. The screen will display a brief message: *"Synchronizing board data..."*.
5. The page refreshes showing the seats for 2027.
   * If this is the first time opening 2027, seats will typically show as **Vacant**.
   * Notice that the **"Current"** button now appears next to the dropdown, allowing you to return to the active year at any time.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the Term Year dropdown open with "2027" highlighted, and the "Current" button visible beside it.*

---

### Procedure B: Assigning a Member to an Open / Vacant Seat
To fill a vacant position with an elected or appointed member:

1. Locate the position card you want to fill (e.g., **Treasurer**).
2. Check that the badge reads **Vacant**.
3. Click the dropdown box labeled **"Assign Member"** (it initially reads *"— Select a member —"*).
4. Scroll or begin typing the member's last name.
   * The list displays names in `Last Name, First Name (Status)` format (e.g., *"Silva, John (Regular)"*).
   * **Important:** If a member is already assigned to another seat in this term, they will not appear in the list.
5. Click on the desired member's name.
6. The card detects the change:
   * The blue **Save** button (<i class="bi bi-cloud-arrow-up-fill"></i> **Save**) becomes active and clickable.
7. Click the **Save** button.
   * A small loading spinner will appear inside the button.
8. When complete, a green banner appears: *"Treasurer updated for 2027."*
9. The card badge updates to **Filled**, and the **Filled** metric card at the top increments by 1.

> 📸 **[SCREENSHOT SUGGESTION]:** *Image showing a single Position Card with a member selected in the dropdown, the blue "Save" button highlighted with an arrow pointing to it.*

---

### Procedure C: Replacing a Board Member (Mid-Term Resignation or Succession)
If an officer steps down during the term and another member is appointed to replace them:

1. Ensure you are viewing the correct **Term Year**.
2. Locate the card of the position being replaced (e.g., **Director #2**).
   * The card currently displays the departing member's name under *Currently: [Member Name]*.
3. Click the **"Assign Member"** dropdown on that card.
4. Select the new member from the list.
5. Click the blue **Save** button.
6. The system updates the seat immediately:
   * The previous member is recorded as removed in the club history log.
   * The new member is recorded as active.
   * All building access, dues waivers, and workstation permissions transfer to the new member automatically.

---

### Procedure D: Clearing / Vacating a Seat (Leaving it Unfilled)
If an officer resigns and the position will remain temporarily vacant:

1. Locate the position card.
2. Notice the red outline button at the bottom right labeled **Clear** (<i class="bi bi-person-x-fill"></i>).
3. Click **Clear**.
4. The system clears the assignment:
   * The dropdown resets to *"— Select a member —"*.
   * The badge switches to **Vacant**.
   * The occupant status displays *Currently: Unassigned*.
   * A confirmation message appears: *"[Position] updated for [Year]."*

> 📸 **[SCREENSHOT SUGGESTION]:** *Position card showing the red "Clear" button being clicked, with a tooltip or note pointing to the "Vacant" badge result.*

---

### Procedure E: Confirming the Final Board Lineup
Once all officer and director positions have been assigned and verified:

1. Scroll toward the top of the page and locate the amber **"Board Not Confirmed"** banner.
2. Click the yellow button on the right side labeled **"Confirm Now"**.
3. A modal window titled **"Confirm Board Term"** will appear in the center of your screen.
4. **Notes Field (Optional):**
   * You can enter an optional note for the club's permanent archives (e.g., *"Elected at Annual General Meeting on November 20, 2026. Certified by Recording Secretary."*).
5. Click the green button labeled **"Confirm Board"**.
6. The modal closes, and the banner instantly transforms into a green **"Board Confirmed"** bar:
   * Displays: *"Confirmed on [Date] by [Your Username]"*.
   * If changes ever need to be made later, an **"Update"** button remains available on the green bar.

> 📸 **[SCREENSHOT SUGGESTION]:** *The "Confirm Board Term" popup window showing the Notes text area, "Cancel" button, and green "Confirm Board" button.*

---

## 4. What the System Does Automatically Behind the Scenes

When you assign a member and click **Save**, the system executes the following background automations:

```
[ Board Assignment Saved ]
        │
        ├──> 1. User Account: Auto-provisions staff login in User Management
        ├──> 2. Permissions: Auto-grants full Membership page access
        ├──> 3. Workstations: Adds UserId to all shared POS/kiosk whitelists
        ├──> 4. Key Card: Evaluates RFID card and syncs access to physical door controllers
        ├──> 5. Dues Waiver: Tags member for annual board service dues exemption
        └──> 6. Audit Trail: Logs date, position, and admin username in Security Audit Log
```

1. **Staff User Account Provisioning:**
   * The system checks if the member already has a login account in **User Management** (`/users`).
   * If not, it automatically creates an account with their member name and links their profile.
2. **Immediate Membership Permissions:**
   * The newly elected director is automatically granted access permissions to all pages in the **Membership** section (Members Info, Dues, Key Cards, Life Eligibility, etc.).
3. **Shared Workstation Terminal Access:**
   * The member's user ID is added to the authorized operator whitelist of all active shared workstations (bar POS tablets, office computers). They can immediately sign in on club terminals.
4. **Door Key Card & Controller Synchronization:**
   * The system evaluates their physical RFID card and transmits an activation command down to the physical door controllers, ensuring building access privileges are active.
5. **Dues Exemption:**
   * In the **Dues & Payments** module, the member is flagged as exempt for that term year under the bylaws board service rule (`WAIVED - Board Service`).
6. **Audit & Member History:**
   * A permanent record is stored under the member's profile change history, and an entry is written to the system audit log.

---

## 5. Troubleshooting & Frequently Asked Questions

### Q1: Why is a member's name not showing up in the "Assign Member" dropdown?
* **Reason 1:** The member is already assigned to another board position for this same year. (The system strictly enforces the bylaws rule that no person can hold multiple seats simultaneously).
* **Reason 2:** The member's membership status does not permit board service under club bylaws. Only members with active statuses (*Regular*, *Regular-NP*, *Life*, *Honorary Life*) are eligible. Social or Inactive members are filtered out automatically.

### Q2: What happens if I make a mistake and assign the wrong person?
* Simply click the dropdown on that same card, pick the correct person, and click **Save**. The software will seamlessly update the record.

### Q3: When should the board be confirmed?
* Confirmation is typically done right after the annual general meeting or election meeting once all seats are filled and verified by the election committee.
