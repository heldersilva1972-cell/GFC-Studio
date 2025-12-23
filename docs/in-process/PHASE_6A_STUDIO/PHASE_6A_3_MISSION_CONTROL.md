# PHASE 6A-3: MISSION CONTROL (Operations Backend)

**Objective:** Build the administrative dashboards for the non-visual business logic (Rentals & Shifts).

## 1. Hall Rentals Manager
- [ ] **Data Model**: `HallRentals` (Applicant, Date, Status, Price).
- [ ] **Admin Dashboard (`/admin/rentals`)**:
    - Data Grid showing all pending requests.
    - "Approve/Deny" buttons that trigger email notifications.
    - Calendar View to spot double-bookings.

## 2. Staff Shift Manager
- [ ] **Data Model**: `StaffShifts` (StaffId, Date, Role, ShiftType).
- [ ] **Scheduler UI (`/admin/schedule`)**:
    - Weekly grid view.
    - Drag-and-drop interface to assign Staff Members to slots.
- [ ] **Report Viewer**: Read-only view of the "Z-Tape" digital reports submitted by bartenders.
