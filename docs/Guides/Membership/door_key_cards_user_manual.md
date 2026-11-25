# GFC Web Application: Door Key Cards User Manual & Standard Operating Procedure

---

## Document Control & Purpose
* **Module:** Physical Security & Access Control
* **Primary URL:** `/keycards`
* **Target Audience:** Executive Officers, Door Access Administrators, Security Personnel
* **Purpose:** This manual provides an exhaustive, non-technical guide on managing physical RFID door key cards. It covers issuing new cards to members, replacing lost or damaged cards, monitoring hardware controller connectivity, and understanding automatic building access policies.

---

## 1. How to Navigate to the Door Key Cards Page

Follow these steps from anywhere inside the web application:

1. **Locate the Left Navigation Sidebar:**
   Find the dark navigation bar on the left of the screen.
2. **Expand the Operations Section:**
   Click on **MEMBERSHIP** under the **OPERATIONS** grouping to reveal its sub-items.
3. **Click "Door Key Cards":**
   Click on the item labeled **Door Key Cards** (accompanied by the key card icon <i class="bi bi-key-fill"></i>).
4. **Confirmation:**
   The page loads with the header **Key Card Management** and shows the live status of the physical door controllers.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the left sidebar showing the "Door Key Cards" link highlighted with a callout indicator.*

---

## 2. Visual Layout & Screen Tour

```
+-----------------------------------------------------------------------------------------------+
| Page Title: Door Key Cards                          [ Controller Status: ONLINE (Synced) ]   |
+-----------------------------------------------------------------------------------------------+
| [ Total Cards: 385 ]     [ Active: 320 ]     [ Deactivated: 45 ]     [ Unassigned Members: 20]|
+-----------------------------------------------------------------------------------------------+
| [ Card Assignments ]  [ Members Needing Cards (20) ]  [ Deactivated / Suspended ]  [ Sync Logs ]
+-----------------------------------------------------------------------------------------------+
| MEMBER #  | MEMBER NAME        | STATUS     | CARD # (HEX/DEC) | CARD STATUS | ACTIONS        |
+-----------+--------------------+------------+------------------+-------------+----------------+
| #1042     | Silva, John M.     | Regular    | 0004281924       | ACTIVE      | [Replace] [Off]|
| #1043     | Costa, Paul A.     | Regular-NP | 0004281925       | DEACTIVATED | [Reactivate]   |
| #0512     | Santos, Manuel     | Life       | 0001092831       | ACTIVE      | [Replace] [Off]|
+-----------------------------------------------------------------------------------------------+
```

### Zone 1: Controller Status Badge (Top Right)
* **Green Badge ("Card Access Controller: Online"):** Indicates the web app has an active connection to the physical building door controller (Mengqi controller box). Any card updates take effect at the doors within seconds.
* **Amber / Red Badge ("Controller: Offline / Disconnected"):** Indicates network communication to the controller box is down. Cards can still be saved in the database and will sync as soon as communication is restored.

### Zone 2: Metric Overview Bar
* **Total Cards:** All physical RFID cards registered in the database.
* **Active:** Cards currently authorized to unlock club entrance doors.
* **Deactivated:** Cards turned off due to unpaid dues, member suspension, or reported lost.
* **Unassigned Members:** Living members in good standing who have not yet been assigned a physical key card.

### Zone 3: Functional Work Tabs
1. **Card Assignments:** Complete roster of members who have an assigned card.
2. **Members Needing Cards:** Actionable list of members awaiting their initial card issuance.
3. **Deactivated / Suspended:** Cards currently locked out.
4. **Sync Logs:** Technical transmission log verifying that door hardware received the latest updates.

> 📸 **[SCREENSHOT SUGGESTION]:** *Wide screenshot of the Door Key Cards dashboard showing the controller status pill, 4 metric cards, and the 4 navigation tabs.*

---

## 3. Step-by-Step Operating Procedures

### Procedure A: Issuing a Key Card to a New Member
When a new member is accepted and arrives to receive their physical door card:

1. Click on the tab labeled **"Members Needing Cards"** (or click the blue **"+ Assign Card"** button).
2. Locate the member from the list.
3. Click the green button labeled **"Assign Card"** on their row.
4. A window titled **"Assign Door Key Card"** will appear:
   * **Member Name & ID:** Confirm the member's identity.
   * **Card Number Field:**
     * If using a USB card reader plugged into your computer: click into the box and scan/tap the card against the reader. The card number will automatically type itself in.
     * If entering manually: type the 8 to 10 digit number printed on the back of the physical card.
   * **Facility Code:** *(Optional, if your cards utilize a separate 3-digit site code).*
5. Click **"Save & Activate Card"**.
6. **Result:**
   * The system links the physical card to the member's profile.
   * It transmits a sync packet to the physical door controller box.
   * A green message confirms: *"Card successfully assigned and activated on entrance doors."*

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Assign Door Key Card" modal window showing the Card Number input field and the "Save & Activate" button.*

---

### Procedure B: Replacing a Lost, Stolen, or Damaged Card
If a member loses their card or it stops reading at the door reader:

1. Click the **"Card Assignments"** tab.
2. In the search box, type the member's name or Member Number.
3. On the member's row, click the amber button labeled **"Replace Card"**.
4. A replacement wizard will open:
   * **Step 1: Old Card Action:** Check the box: *"Immediately revoke and disable old card (Card #[OldNumber])"*. (This guarantees that if someone finds the lost card, it will not unlock the club).
   * **Step 2: Reason for Replacement:** Choose from the dropdown:
     * *Lost Card*
     * *Damaged / Broken Card*
     * *Stolen Card*
   * **Step 3: New Card Number:** Tap the new card on your USB desktop scanner, or manually type the new card number.
5. Click **"Confirm Replacement"**.
6. The system immediately revokes the old card and activates the new one at the door hardware.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Replace Card" popup modal showing the old card revocation checkmark and new card number input.*

---

### Procedure C: Manually Suspending or Deactivating a Card
If a member is placed under temporary disciplinary suspension by the board:

1. Search for the member in the **Card Assignments** tab.
2. Click the red button labeled **"Deactivate"** (<i class="bi bi-shield-x"></i>).
3. Select the reason:
   * *Board Disciplinary Suspension*
   * *Investigation Pending*
   * *Member Request*
4. Click **"Confirm Deactivation"**.
5. The card status changes to **DEACTIVATED**, and the door controller deletes their access privilege immediately.

---

### Procedure D: Reactivating a Suspended Card
1. Go to the **"Deactivated / Suspended"** tab.
2. Find the member.
3. Click the green button labeled **"Reactivate"**.
4. Confirm the reactivation. The card is restored to the door controllers within seconds.

---

## 4. How the Automated Card Lifecycle Works Behind the Scenes

The GFC web application manages door security autonomously based on membership rules:

```
[ Dues Payment Status ] ------------> [ Key Card Lifecycle Service ]
         │                                       │
         ├─ Unpaid past Grace Period?            ├─ Commands Door Box: DISABLE CARD
         │                                       │
         └─ Dues Paid in Full?                   └─ Commands Door Box: ACTIVATE CARD (Instant)
```

1. **Annual Dues Expiration Lockout:**
   * Under club bylaws, members have until the end of the annual grace period to pay their annual dues.
   * If a member remains unpaid once the grace period expires, **the system deactivates their door key card automatically**.
2. **Instant Reactivation upon Payment:**
   * When an unpaid member pays their dues at the bar or office, **you do not need to touch the Key Cards page**.
   * The moment the payment is saved in the **Dues & Payments** module, the system triggers the background door sync and re-enables their card in real time.
3. **Resignations & Deceased Status:**
   * Changing a member's status to Inactive or Deceased in **Members Info** instantly revokes their key card across all building entrances.

---

## 5. Troubleshooting & FAQs

### Q1: The card was assigned, but the member says it doesn't open the door. What should I check?
1. Check the **Controller Status** badge at the top right of the Key Cards screen. Ensure it reads **Online**. If it is offline, the door controller may need a quick power cycle or network check.
2. Verify the member's dues status under **Dues & Payments**. If dues are unpaid and past grace, the card will be disabled.
3. Double-check that the printed number was typed correctly without missing leading zeros.

### Q2: What happens if the internet goes down?
* The physical door controller box stores all active card numbers in its own internal memory. Even if the internet or club network goes down, **members whose cards are active will still be able to scan and enter the club normally**.
