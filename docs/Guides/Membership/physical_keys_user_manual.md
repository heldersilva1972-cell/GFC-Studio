# GFC Web Application: Physical Keys User Manual & Standard Operating Procedure

---

## Document Control & Purpose
* **Module:** Physical Security / Facilities & Asset Management
* **Primary URL:** `/physicalkeys`
* **Target Audience:** Executive Officers, House & Property Committee, Bar Manager, Security
* **Purpose:** This manual provides an exhaustive, non-technical guide on managing physical brass keys for club facilities. While RFID cards handle the exterior doors, physical keys secure sensitive internal areas (Executive Office, Liquor Storage Rooms, Bar Register Cabinets, Kitchen Pantry, Tool Sheds, and Main Deadbolts). This guide details inventory tracking, issuing keys to personnel, processing returns, and conducting security audits.

---

## 1. How to Navigate to the Physical Keys Page

Follow these exact steps from anywhere inside the web application:

1. **Locate the Left Navigation Sidebar:**
   Look at the navigation menu on the left side of your browser.
2. **Expand Operations:**
   Under **OPERATIONS**, click on **MEMBERSHIP** to expand its sub-items.
3. **Click "Physical Keys":**
   Click on the item labeled **Physical Keys** (accompanied by the key icon <i class="bi bi-key"></i>).
4. **Confirmation:**
   The page loads with the header **Physical Key Inventory & Issuance** and shows the live key tracking register.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the left navigation sidebar with "MEMBERSHIP" expanded and a red callout indicator around "Physical Keys".*

---

## 2. Visual Layout & Screen Tour

```
+-----------------------------------------------------------------------------------------------+
| Page Title: Physical Key Inventory & Tracking              [ + Issue Key btn ] [ Audit Report]|
+-----------------------------------------------------------------------------------------------+
| [ Total Keys: 45 ]        [ In Circulation: 28 ]        [ In Club Safe: 17 ]       [ Lost: 2 ]|
+-----------------------------------------------------------------------------------------------+
| [ Keys in Circulation (28) ]     [ Available in Safe (17) ]     [ Complete Audit History ]    |
+-----------------------------------------------------------------------------------------------+
| KEY TAG / STAMP | LOCK / AREA ASSIGNED | HOLDER NAME        | ROLE       | DATE ISSUED| ACTIONS|
+-----------------+----------------------+--------------------+------------+------------+--------+
| TAG #OM-01      | Office Master Deadbolt| Silva, John M.    | President  | Jan 05 2026| [Return|
| TAG #LQ-04      | Liquor Storage Room  | Araujo, Arthur D.  | Bartender  | Mar 12 2026| [Return|
| TAG #SH-02      | Maintenance Shed     | Costa, Paul A.     | Director   | Feb 10 2026| [Return|
+-----------------------------------------------------------------------------------------------+
```

### Zone 1: Inventory Metric Bar
Four cards summarizing the club's physical key accountability:
1. **Total Keys:** All physical keys stamped and cataloged in the club inventory.
2. **In Circulation:** Keys currently in the custody of officers, bartenders, or maintenance staff.
3. **In Club Safe:** Available keys securely locked in the key cabinet inside the executive office.
4. **Lost / Missing:** Keys reported lost or not returned, flagging potential rekeying requirements.

### Zone 2: Register Work Tabs
* **Keys in Circulation:** Shows every person who currently possesses a club key, what lock it opens, and when it was given to them.
* **Available in Safe:** Master inventory of unassigned keys currently hanging in the key cabinet.
* **Complete Audit History:** Permanent log of every issuance and return over the club's history.

> 📸 **[SCREENSHOT SUGGESTION]:** *Overview screenshot showing the 4 inventory cards and the active "Keys in Circulation" table.*

---

## 3. Step-by-Step Operating Procedures

### Procedure A: Issuing a Physical Key to an Officer or Staff Member
When a newly elected officer, bartender, or committee chair is given a physical key:

1. Click the blue **"+ Issue Key"** button located in the top right corner.
2. A modal dialog titled **"Issue Physical Key"** will open:
   * **Select Member / Recipient:** Click the dropdown and search for the member or staff member receiving the key.
   * **Key Tag / Stamped ID:** Choose the key from the list of available keys in the safe (e.g., `TAG #OM-02 - Office Master`, `TAG #LQ-03 - Liquor Storage`).
   * **Issue Date:** Defaults to today's date (can be adjusted if recording a handover from earlier).
   * **Expected Return Date:** *(Optional)* For board members, select December 31st of their term year; for contractors, select the project completion date.
   * **Notes / Authorization:** Enter authorization details (e.g., *"Handed over by House Committee Chair after board swearing-in"*).
3. Click the green button labeled **"Confirm Key Issuance"**.
4. **System Result:**
   * The key moves from *In Safe* to *In Circulation*.
   * It appears under the member's profile in **Members Info**.
   * An entry is logged in the permanent key audit history.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Issue Physical Key" popup window showing the Recipient dropdown, Available Key selector, Expected Return Date, and "Confirm Key Issuance" button.*

---

### Procedure B: Processing a Key Return (End of Term or Departure)
When an officer completes their term or a bartender leaves employment:

1. Ensure the physical key is in hand.
2. Open the **"Keys in Circulation"** tab.
3. Search for the member's name or the stamped Key Tag number.
4. On that key's row, click the button labeled **"Return Key"** (<i class="bi bi-box-arrow-in-down-left"></i>).
5. A confirmation dialog titled **"Record Key Return"** will appear:
   * **Return Date:** Defaults to today's date.
   * **Physical Condition:** Select:
     * *Returned in Good Condition*
     * *Returned Damaged / Bent*
   * **Witness / Received By:** Shows your username as the officer receiving the key into the safe.
6. Click **"Confirm Key Return"**.
7. **System Result:**
   * The key is removed from the member's profile.
   * The key is automatically returned to the **"Available in Safe"** tab.
   * The **In Circulation** counter decreases by 1.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Record Key Return" confirmation modal showing the Key Tag summary, Condition dropdown, and "Confirm Key Return" button.*

---

### Procedure C: Reporting a Key as Lost or Stolen
If a key is misplaced, lost, or stolen:

1. Locate the key in the **Keys in Circulation** tab.
2. Click the small **Options** dropdown on the row and select **"Report Lost / Stolen"**.
3. A security alert dialog will appear:
   * Enter the date the loss occurred or was discovered.
   * Enter detailed circumstances (e.g., *"Reported lost by holder while traveling"*).
   * Note whether the lock cylinder requires rekeying (e.g., *"Recommended: Rekey Liquor Storage padlock"*).
4. Click **"Mark as Lost"**.
5. The key is flagged as **LOST** (highlighted in red) to alert the board during security reviews.

---

### Procedure D: Conducting an Annual Physical Key Audit
At the close of each calendar year:

1. Click the **"Audit Report"** button in the upper right corner.
2. Select **"Print Master Key Register (PDF)"**.
3. Use the generated document to physically verify each key hanging in the club safe against the master record.
4. Contact any key holders whose expected return date has passed to collect and log their keys.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the generated printable Key Audit Register PDF report.*

---

## 4. Key Security Rules & Best Practices

1. **Verify Physical Stampings:**
   * Always verify the stamped number on the brass key matches the system Tag ID before handing it over or accepting a return.
2. **Never Hand Over Keys Informally:**
   * Officers should never pass a physical key directly to their successor in the parking lot or lounge without an administrator recording the return and re-issuance in the software.
3. **Safe Storage:**
   * All keys not currently listed as *In Circulation* must be stored in the locked key safe in the executive office.
