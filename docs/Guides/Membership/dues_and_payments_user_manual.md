# GFC Web Application: Dues & Payments User Manual & Standard Operating Procedure

---

## Document Control & Purpose
* **Module:** Membership Operations / Financial Billing
* **Primary URL:** `/dues`
* **Target Audience:** Club Treasurer, Collector of Dues, Bartenders, Administrative Staff
* **Purpose:** This manual provides a non-technical, step-by-step guide on managing member annual dues, collecting and recording payments (cash, check, card), generating receipts, viewing payment histories, and understanding automated bylaws dues waivers.

---

## 1. How to Navigate to the Dues & Payments Page

Follow these steps from anywhere inside the web application:

1. **Locate the Left Navigation Sidebar:**
   Find the dark sidebar navigation on the left of your screen.
2. **Expand Operations:**
   Under **OPERATIONS**, click on **MEMBERSHIP** to view its menu items.
3. **Click "Dues & Payments":**
   Click on the item labeled **Dues & Payments** (accompanied by the wallet icon <i class="bi bi-wallet2"></i>).
4. **Confirmation:**
   The page loads with the header **Dues & Payments** and shows the current billing year summary.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the left navigation menu with "Dues & Payments" highlighted with a red box or arrow.*

---

## 2. Visual Layout & Screen Tour

```
+-----------------------------------------------------------------------------------------------+
| Page Title: Dues & Payments                                       [ Year: 2026 v ] [ Print v ]|
+-----------------------------------------------------------------------------------------------+
| [ Total Billed: $24,500 ]  [ Collected: $18,200 ]  [ Outstanding: 63 ]  [ Waived/Exempt: 78 ] |
+-----------------------------------------------------------------------------------------------+
| [ All Members ] [ Unpaid Only ] [ Paid Only ] [ Waived Only ]        | Search: [ Member / # ] |
+-----------------------------------------------------------------------------------------------+
| MEMBER #  | NAME               | STATUS     | 2026 DUES  | PAID DATE   | METHOD | ACTIONS     |
+-----------+--------------------+------------+------------+-------------+--------+-------------+
| #1042     | Silva, John M.     | Regular    | $100.00    | Nov 02 2026 | Cash   | [Receipt]   |
| #1043     | Costa, Paul A.     | Regular-NP | $100.00    | UNPAID      | --     | [Pay Dues]  |
| #0512     | Santos, Manuel     | Life       | $0.00      | WAIVED      | Life   | [Details]   |
| #0780     | Araujo, Arthur     | Regular    | $0.00      | WAIVED      | Board  | [Details]   |
+-----------------------------------------------------------------------------------------------+
```

### Zone 1: Year Selector & Quick Actions
Located in the upper right corner:
* **Dues Year Dropdown:** Allows you to switch between different billing years (e.g., `2025`, `2026`, `2027`).
* **Print / Export Button:** Generate delinquent lists, payment ledgers, or audit summaries.

### Zone 2: Financial Metric Cards
Four cards summarize the financial health of the current dues cycle:
1. **Total Billed:** The total dollar amount expected based on active billable members.
2. **Collected:** Total dollars received and deposited into club accounts to date.
3. **Outstanding / Unpaid:** Total count of active members who have not yet paid their dues for the selected year.
4. **Waived / Exempt:** Total count of members legally exempt from dues under club bylaws (Life members, honorary members, and active board members).

### Zone 3: Filter Tabs & Member Search
* **Filter Tabs:** Quickly isolate members who need follow-up (*Unpaid Only*), those in good standing (*Paid Only*), or exempt members (*Waived Only*).
* **Live Search:** Search by member name, member number, or receipt number.

> 📸 **[SCREENSHOT SUGGESTION]:** *Overview screenshot of the Dues & Payments page showing the 4 financial cards and the dues roster table.*

---

## 3. Step-by-Step Operating Procedures

### Procedure A: Recording a Dues Payment from a Member
When a member arrives at the club or sends in their annual dues payment:

1. **Verify the Billing Year:**
   Check the **Year Selector** in the top right to make sure it matches the year the member is paying for (e.g., `2026` or advance `2027`).
2. **Locate the Member:**
   Type the member's last name or Member Number into the **Search** box.
3. **Initiate Payment:**
   In the member's row, locate the green button labeled **"Pay Dues"** (or **"Record Payment"**). Click it.
4. **Complete the Payment Window:**
   A modal dialog titled **"Record Dues Payment"** will appear:
   * **Member Name & Number:** Verify the displayed name matches the person paying.
   * **Amount:** The standard annual dues fee (e.g., `$100.00`) is pre-filled. You can adjust this if paying a custom or partial fee.
   * **Payment Method:** Click the dropdown and select:
     * *Cash*
     * *Check* (If selected, enter the check number in the *Check #* box).
     * *Credit / Debit Card*
     * *Online Transfer*
   * **Payment Date:** Automatically set to today's date (can be adjusted if recording a mailed check from an earlier date).
   * **Notes:** *(Optional)* Enter any reference details (e.g., *"Paid at bar to bartender John"*).
5. **Submit the Payment:**
   Click the green **"Save Payment"** button.
6. **Result:**
   * The status immediately turns to **PAID**.
   * The row displays the payment date, dollar amount, and method.
   * If the member's door key card was deactivated due to non-payment, **the system reactivates their key card immediately**.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the "Record Dues Payment" modal dialog showing Amount, Payment Method dropdown, Check # field, and the "Save Payment" button.*

---

### Procedure B: Printing or Emailing a Dues Receipt
1. Find the member's row in the dues table.
2. In the Actions column on the right, click **"Receipt"** (<i class="bi bi-receipt"></i>).
3. A printable receipt will open in a preview window showing:
   * Club Header & Seal.
   * Member Name & Badge Number.
   * Term Year Paid.
   * Payment Method, Date, and Amount Received.
   * Cashier / Officer username who logged the transaction.
4. Click **"Print"** to send to any physical receipt printer or desktop printer, or click **"Download PDF"**.

> 📸 **[SCREENSHOT SUGGESTION]:** *Screenshot of the official GFC Member Dues Receipt preview modal.*

---

### Procedure C: Handling Automatic Bylaws Waivers
The system manages bylaws exemptions automatically without requiring manual payment entries:

* **Life Members & Honorary Life:**
  * When a member reaches Life status, their dues for all future years are automatically marked as **WAIVED (Life Member)** with a `$0.00` balance.
* **Active Board Officers & Directors:**
  * Active board members serving during that term year automatically receive a **WAIVED (Board Service)** designation in accordance with club bylaws.
* **Special Board Hardship Waivers:**
  * If the Executive Board votes to waive dues for a member facing illness or active military service, click **"Grant Waiver"** on their row, select the approved category, enter the board meeting date, and click **Save**.

---

### Procedure D: Reversing or Correcting an Accidental Payment Entry
If a payment was entered under the wrong member by mistake:

1. Locate the member's record.
2. Click the small **Edit / Actions** button beside the paid status.
3. Click **"Void Payment"** or **"Revert to Unpaid"**.
4. The system will ask for a reason for the void (e.g., *"Bounced check"* or *"Entered under wrong member by mistake"*).
5. Click **"Confirm Void"**.
6. The record is restored to **UNPAID**, and an audit log entry is recorded for security.

---

### Procedure E: Generating the Unpaid / Delinquent Member List
When preparing for board review or sending reminder notices:

1. Click the **"Unpaid Only"** tab at the top of the table.
2. The table will instantly filter to show only members with outstanding dues.
3. Click the **"Export"** dropdown in the top right.
4. Select **"Print Delinquent Notice Report (PDF)"** or **"Export to Excel"**.
5. The generated report includes member names, phone numbers, mailing addresses, and balance owed.

> 📸 **[SCREENSHOT SUGGESTION]:** *Table view with the "Unpaid Only" tab selected, showing the yellow unpaid badges and the "Export" button open.*

---

## 4. Key Financial Rules & Best Practices

1. **Grace Period & Access Lockouts:**
   * The club bylaws define a grace period (e.g., through January 31st or March 31st).
   * Once the grace period expires, any member who remains **UNPAID** will have their door access card deactivated automatically by the background system.
2. **Immediate Lock Reactivation:**
   * Do not worry about manually turning door cards back on. As soon as you click **"Save Payment"**, the software immediately commands the door controllers to re-enable building access for that member.
3. **Audit Trail:**
   * Every dollar recorded is tied to the logged-in user account that processed it, ensuring transparent and accurate financial accounting.
