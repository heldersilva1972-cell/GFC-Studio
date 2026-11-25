# GFC Web Application: Non-Portuguese Queue User Manual & Standard Operating Procedure

---

## Document Control & Purpose
* **Module:** Membership Operations / Quota Governance
* **Primary URL:** `/np-queue`
* **Target Audience:** Membership Committee, Executive Officers, Board of Directors
* **Purpose:** This manual provides an exhaustive, non-technical guide on managing the **Non-Portuguese (NP) Applicant Waiting List**. It explains how the bylaws quota cap is monitored, how applicants are added and ranked chronologically, how open quota vacancies are tracked, and the exact procedure for formally admitting an applicant into active **Regular-NP** membership.

---

## 1. How to Navigate to the Non-Portuguese Queue Page

Follow these exact steps from anywhere inside the web application:

1. **Locate the Left Navigation Sidebar:**
   Find the dark sidebar menu on the left side of your browser.
2. **Expand the Operations Workspace:**
   Under the primary **OPERATIONS** grouping, click on **MEMBERSHIP** to expand its menu items.
3. **Click "Non-Portuguese Queue":**
   Click on the item labeled **Non-Portuguese Queue** (accompanied by the queue icon <i class="bi bi-arrow-up-right-circle"></i>).
4. **Confirmation:**
   The page loads with the header **Non-Portuguese Membership Queue** and displays the quota gauge and ordered applicant table.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the left navigation sidebar with "MEMBERSHIP" expanded and a red callout indicator around "Non-Portuguese Queue".*

---

## 2. Bylaws Quota Cap Overview

Under Article II of the club bylaws:
* A strict percentage or numerical ceiling is established for members admitted under the Non-Portuguese qualification (for example, a cap of 50 total active Regular-NP members).
* When the quota is full, prospective applicants must be placed on a waiting list in strict chronological order of application.
* When an active Regular-NP member resigns, passes away, or is promoted to Life status, a quota slot opens up and the next applicant in line is eligible for admission.

---

## 3. Visual Layout & Screen Tour

```
+-----------------------------------------------------------------------------------------------+
| Page Title: Non-Portuguese Membership Queue                     [ + Add Applicant btn ] [ v ] |
+-----------------------------------------------------------------------------------------------+
| [ Quota Cap: 50 ]        [ Current Active: 48 ]        [ Open Slots: 2 ]       [ Waiting: 14 ]|
+-----------------------------------------------------------------------------------------------+
| [ Active Waiting Queue (14) ]          [ Admitted History ]          [ Cancelled / Withdrawn ]|
+-----------------------------------------------------------------------------------------------+
| QUEUE #   | APPLICANT NAME     | DATE APPLIED | SPONSORS             | CONTACT PHONE | ACTIONS|
+-----------+--------------------+--------------+----------------------+---------------+--------+
| #1        | Miller, Robert     | Jan 14 2024  | Silva, J. / Costa, P.| (508) 555-8811| [Admit]|
| #2        | O'Connor, Thomas   | Mar 02 2024  | Santos, M.           | (508) 555-9922| [Admit]|
| #3        | Williams, David    | Jul 19 2024  | Araujo, A. / Diaz, F.| (508) 555-3344| [Edit] |
+-----------------------------------------------------------------------------------------------+
```

### Zone 1: The Quota & Capacity Status Bar
Four high-visibility cards at the top of the screen:
1. **Quota Cap:** Maximum number of active Non-Portuguese voting members allowed under bylaws.
2. **Current Active:** Total active Regular-NP members currently on the club roster.
3. **Open Slots (Green Badge):** Available seats that can be filled immediately (`Cap - Active`).
4. **Waiting:** Total applicants currently on the queue.

### Zone 2: Navigation Tabs
* **Active Waiting Queue:** The live, ranked list of applicants awaiting admission.
* **Admitted History:** Archival record of all past applicants and the dates they were admitted.
* **Cancelled / Withdrawn:** Applicants who withdrew their applications or relocated.

### Zone 3: The Ranked Applicant Table
* **Queue #:** Official ranking number (#1, #2, #3, etc.) determined automatically by application timestamp.
* **Applicant Name & Contact:** Full name, mobile phone, and email.
* **Date Applied:** Official date application was verified and fee paid.
* **Sponsors:** Names of the current club members in good standing who endorsed the application.
* **Actions Column:** Action buttons to **Admit**, **Edit**, or **Remove**.

> 📸 **[SCREENSHOT SUGGESTION]:** *Wide-angle screenshot of the Quota Status Bar showing "Open Slots: 2" and the ranked waiting list below.*

---

## 4. Step-by-Step Operating Procedures

### Procedure A: Adding a New Applicant to the Waiting List
When a prospective member submits a formal application with required sponsorship:

1. Click the blue **"+ Add Applicant"** button in the upper right corner.
2. A modal dialog titled **"Add Applicant to NP Queue"** will appear:
   * **Full Name:** First Name, Middle Initial, and Last Name.
   * **Application Date:** Enter the exact date the application was received by the club.
   * **Primary Sponsor:** Select an active club member from the sponsor dropdown.
   * **Secondary Sponsor:** *(Optional, if required by club rules).*
   * **Contact Information:** Address, Phone Number, and Email Address.
   * **Notes:** *(Optional)* Application fee receipt number or committee remarks.
3. Click **"Save to Queue"**.
4. **Automatic System Ranking:**
   * The system immediately places the applicant into their exact chronological position in line based on their application date.
   * The **Waiting** counter increments by 1.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Add Applicant to NP Queue" modal window showing the applicant contact fields, sponsor dropdown, and "Save to Queue" button.*

---

### Procedure B: Admitting an Applicant when a Slot Opens
When an open slot becomes available (verified by the green **"Open Slots"** indicator showing 1 or more):

1. Confirm that **Open Slots** is `1` or greater.
2. Identify the **#1 Ranked Applicant** at the top of the table.
3. Click the green button labeled **"Admit to Membership"** (<i class="bi bi-person-check-fill"></i>).
4. An admission confirmation wizard titled **"Admit Applicant to Regular-NP Membership"** will open:
   * **Candidate Summary:** Displays the applicant's name, application date, and sponsors.
   * **Member Number:** The system automatically suggests the next available member badge number (or you can override it with a physical badge number).
   * **Admission Effective Date:** Defaults to today's date.
   * **Dues Assessment:** Shows the pro-rated or initial annual dues amount.
5. Click **"Confirm Admission"**.
6. **Immediate System Automations:**
   * The applicant is removed from the waiting list and moved to **Admitted History**.
   * A full active member profile is created in **Members Info** with the status set to **Regular-NP**.
   * An initial dues billing record is created under **Dues & Payments**.
   * The **Current Active** counter increments, and **Open Slots** decreases.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Admit Applicant to Regular-NP Membership" confirmation window showing Member Number assignment and the green "Confirm Admission" button.*

---

### Procedure C: Handling Applicant Withdrawals or Disqualifications
If an applicant informs the club they have moved or wish to withdraw:

1. Locate the applicant in the table.
2. Click the **Edit / Actions** dropdown on their row and select **"Withdraw / Remove"**.
3. Select the reason:
   * *Applicant Request / Withdrew*
   * *Relocated*
   * *Failed Background Check / Ineligible*
4. Click **"Confirm Removal"**.
5. The applicant is archived into the **Cancelled / Withdrawn** tab, and **all applicants behind them automatically move up one position in line**.

---

### Procedure D: Printing the Official Queue for Club Bulletin Board
Club rules often require posting the current queue on the member bulletin board:

1. Click the **"Export"** button in the upper right corner.
2. Select **"Printable Waiting List (PDF)"**.
3. A clean document will generate showing applicant names, sponsor names, date applied, and queue rank, with sensitive private contact details omitted for public posting.

---

## 5. Frequently Asked Questions

### Q1: Can an administrator manually skip someone in line?
* **No.** To prevent disputes and ensure fairness under club bylaws, the system strictly orders applicants by their recorded **Application Date**. An applicant cannot jump ahead of someone who applied earlier unless the earlier applicant withdraws or is formally deferred by the board.

### Q2: What happens when an active Regular-NP member is promoted to Life status?
* Under club rules, when a Regular-NP member reaches Life membership, they become a Life member and no longer occupy a slot against the Regular-NP quota cap.
* As soon as they are promoted in the **Life Eligibility** page, **Open Slots** on this page immediately increases by 1, allowing the next applicant to be admitted!
