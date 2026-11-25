# GFC Web Application: Life Eligibility User Manual & Standard Operating Procedure

---

## Document Control & Purpose
* **Module:** Membership Operations / Honors & Governance
* **Primary URL:** `/life-eligibility`
* **Target Audience:** Executive Officers, Board of Directors, Membership Committee
* **Purpose:** This manual provides a non-technical, step-by-step procedure for monitoring and managing **Life Membership** eligibility. It explains how the system tracks member tenure and age, how to review upcoming candidate slates, how to formally promote approved members to Life status, and how the system automatically handles dues exemptions.

---

## 1. How to Navigate to the Life Eligibility Page

Follow these exact steps from anywhere inside the web application:

1. **Locate the Left Navigation Sidebar:**
   Find the dark navigation bar on the left of the screen.
2. **Expand the Operations Workspace:**
   Under **OPERATIONS**, click on **MEMBERSHIP** to expand its menu items.
3. **Click "Life Eligibility":**
   Click on the item labeled **Life Eligibility** (accompanied by the medal/award ribbon icon <i class="bi bi-award"></i>).
4. **Confirmation:**
   The page loads with the header **Life Membership Eligibility** and candidate tracking cards.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the left navigation menu with a red callout indicator around the "Life Eligibility" link.*

---

## 2. Bylaws Rule Summary & Eligibility Formula

Under GFC club bylaws, senior members are honored with permanent **Life Membership** based on age and years of continuous loyalty:

* **General Rule Criteria:**
  * **Continuous Tenure:** Minimum number of consecutive years as an active member in good standing (e.g., 25, 30, or 35 years based on bylaws version).
  * **Minimum Age:** Member must have reached the statutory age requirement (e.g., age 65 or 70).
* **Life Membership Privileges:**
  * Permanent voting privileges for life.
  * Lifetime exemption from annual dues.
  * Exemption from mandatory club work assessments.

---

## 3. Visual Layout & Screen Tour

```
+-----------------------------------------------------------------------------------------------+
| Page Title: Life Eligibility                                                [ Export Report v]|
+-----------------------------------------------------------------------------------------------+
| [ Currently Eligible: 4 ]       [ Approaching in 1 Year: 8 ]       [ Total Active Life: 72 ]  |
+-----------------------------------------------------------------------------------------------+
| [ Action Needed: Eligible Now (4) ]     [ Upcoming Candidates ]     [ Current Life Members ]  |
+-----------------------------------------------------------------------------------------------+
| MEMBER #  | NAME               | JOIN DATE   | YEARS ACTIVE | CURRENT AGE | ACTIONS           |
+-----------+--------------------+-------------+--------------+-------------+-------------------+
| #0614     | Medeiros, Antonio  | Oct 10 1996 | 30 Years     | 68 Years    | [Promote to Life] |
| #0622     | Furtado, Manuel    | Nov 14 1996 | 30 Years     | 71 Years    | [Promote to Life] |
| #0630     | Correia, Jose      | Jan 12 1997 | 30 Years     | 66 Years    | [Promote to Life] |
+-----------------------------------------------------------------------------------------------+
```

### Zone 1: Summary Metric Cards
* **Currently Eligible (Action Needed):** The number of members who meet 100% of the age and service criteria right now and are awaiting formal board approval.
* **Approaching in 1 Year:** Members who will cross the eligibility threshold in the upcoming calendar year (useful for annual budgeting and committee planning).
* **Total Active Life:** Total number of living Life and Honorary Life members currently registered.

### Zone 2: Functional Candidate Tabs
1. **Action Needed: Eligible Now:** Active regular members who qualify today.
2. **Upcoming Candidates:** Roster of members sorted by estimated qualification date.
3. **Current Life Members:** Full directory of members already recognized as Life members, with their promotion dates.

> 📸 **[SCREENSHOT SUGGESTION]:** *Full-screen overview showing the 3 metric cards and the candidate table with "Promote to Life" buttons visible.*

---

## 4. Step-by-Step Operating Procedures

### Procedure A: Reviewing Eligible Candidates for the Annual Meeting
Prior to the club's Annual General Meeting or monthly board session:

1. Open the **Life Eligibility** page.
2. Click on the tab labeled **"Action Needed: Eligible Now"**.
3. Review each candidate displayed in the list:
   * **Join Date & Years Active:** Verifies that their membership has been continuous without lapse.
   * **Current Age:** Calculated automatically from their recorded birth date.
   * **Dues History Link:** Click on their name to inspect their multi-year dues history and confirm they are in good standing.
4. Click **"Export Report"** in the upper right to print or download the candidate slate for review by the Board of Directors.

> 📸 **[SCREENSHOT SUGGESTION]:** *Candidate row close-up showing the Join Date, Years Active, Current Age, and the prominent "Promote to Life" button.*

---

### Procedure B: Promoting an Approved Member to Life Status
Once the Board of Directors or general membership votes to confer Life Membership:

1. On the **Action Needed: Eligible Now** tab, locate the approved member.
2. Click the green button labeled **"Promote to Life"** (<i class="bi bi-award-fill"></i>).
3. A confirmation modal titled **"Promote Member to Life Status"** will appear:
   * **Member Name & Number:** Confirm you are promoting the correct individual.
   * **Promotion Effective Date:** Defaults to today's date. (Can be adjusted to the exact date of the meeting/vote).
   * **Board / Meeting Notes:** Enter an archival note (e.g., *"Approved unanimously by Board of Directors at General Meeting on November 20, 2026"*).
4. Click the green button labeled **"Confirm Promotion"**.
5. **Immediate System Automations:**
   * The member's status is officially changed to **LIFE**.
   * The member moves from the candidate list to the **Current Life Members** roster.
   * A formal commendation note is permanently logged in their club history.
   * The member's dues account is automatically converted to **WAIVED (Life Member)** for all future years.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Promote Member to Life Status" popup window showing the Effective Date input, Meeting Notes box, and "Confirm Promotion" button.*

---

### Procedure C: Handling Honorary Life Membership (Special Board Concurrence)
If the club awards an **Honorary Life Membership** to a member for extraordinary service (even if they have not reached the standard age/years formula):

1. Go to **Members Info** (`/members`) and open the member's profile.
2. Click **"Edit Status"**.
3. Change status to **HONORARY LIFE**.
4. Enter the board resolution reference and effective date.
5. Click **"Save"**.
6. The system grants them all Life privileges, including dues waivers and recognition on the Life roster.

---

### Procedure D: Exporting Candidate Slates for General Meetings
1. In the upper right corner, click **"Export Report"**.
2. Select **"Eligible Candidates Summary (PDF)"**.
3. A clean, printable report will download containing candidate names, induction dates, total years served, and signature lines for the Board President and Recording Secretary.

---

## 5. Frequently Asked Questions

### Q1: Why is a long-time member not appearing on the eligible list?
* **Missing Birth Date:** If the member's birth date is blank in **Members Info**, the system cannot calculate their age. Edit their profile to input their birth date, and they will immediately appear on the list if qualified.
* **Non-Portuguese Status:** Verify whether club bylaws specify different tenure rules or eligibility terms for Regular vs. Regular-NP members.
* **Past Lapses:** If a member resigned and rejoined later, verify whether their join date reflects their original or restarted membership date.
