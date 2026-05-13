# GFC Lottery Reconciliation & Data Integrity Logic

**Last Updated**: May 13, 2026  
**Status**: ACTIVE / MANDATORY

| Version | Date | Author | Description |
| :--- | :--- | :--- | :--- |
| v1.0 | May 13, 2026 | Antigravity | Initial Specification: Fresh Start Day Shifts, Ghost Variance Fix, and Data Hardening. |

---

This document defines the official business rules and mathematical formulas for the GFC Lottery Sales Management system. All future updates to the lottery module MUST adhere to these rules.

## 1. Shift Sequencing & Baselines
The lottery machine resets its cumulative totals every night. Therefore, the system handles shift activity as follows:

### Day Shift (The Fresh Start)
*   **Baseline**: ALWAYS starts at **$0.00**.
*   **Activity Calculation**: We treat the numbers entered from the machine report as the final total for that shift.
    *   `Shift Net Sales = (Report Sales) - (Report Payouts) - (Report Tickets)`
*   **Rule**: The system must NOT pull any data from the previous day to calculate a Day shift.

### Night Shift (The Daily Audit)
*   **Baseline**: Pulls the totals from the **Day Shift** of the same day.
*   **Activity Calculation**: We subtract the Day shift readings to isolate only the activity that happened in the evening.
    *   `Evening Sales = (Report Total Sales) - (Day Shift Total Sales)`
    *   `Evening Payouts = (Report Total Payouts) - (Day Shift Total Payouts)`
    *   `Shift Net Sales = (Evening Sales) - (Evening Payouts) - (Evening Tickets)`

---

## 2. Variance & Expected Cash
The system distinguishes between "Expected Cash" (what should be in the bag) and "Variance" (whether the bartender was short/over).

### The Variance Formula (The "Pre-Drop" Audit)
Variance measures the accuracy of the cash count **before** any money is moved into envelopes or refills.
*   **Formula**: `Actual Count - (Beginning Cash + Shift Net Sales + Backup Bag Inbound)`
*   **Rule**: Envelope Drops and Bag Refills (Outbound) are **IGNORED** for variance. If a bartender counts $1,600 and the machine says they should have $1,600, the Variance is **$0.00**, even if they are about to drop $400 into an envelope.

### Expected Drawer (The "Target Bag")
This column shows what should be left in the bag for the next person after all distributions are finished.
*   **Target**: Typically **$1,200.00**.
*   **Formula**: `(Beginning Cash + Shift Net Sales + Bag In) - (Envelope Drop + Bag Out)`

---

## 3. Distribution Rules
*   **Envelope Drops**:
    *   Only calculated for **Night** shifts.
    *   Threshold: If Ending Cash > $1,200, the extra is moved to an envelope.
    *   If Ending Cash < $1,200, the drop is **$0.00**. (Being below $1,200 is NOT a shortage).
*   **Backup Bag**:
    *   **Out From Bag**: Money added to the drawer (Adds to expectation).
    *   **Back To Bag**: Money returned to the backup bag (Subtracts from expectation).

---

## 4. Data Integrity & Hardening
To prevent data corruption and duplicates, the following technical constraints are enforced:
1.  **Unique Constraint**: The database enforces a unique index on `(ShiftDate, ShiftType, MachineId)`.
2.  **Duplicate Merging**: The sync service (MobileReportingService) must identify and clean up duplicate records during save, keeping only the most recent entry.
3.  **State Reset**: The Mobile UI must wipe all money fields whenever the Date or ShiftType is changed to prevent cross-contamination from previous reports.
