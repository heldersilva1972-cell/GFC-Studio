# TASK FOR JULES: GFC Studio - Phase 6A-3 (Mission Control Backend)

## 🎯 OBJECTIVE
Implement the operational backend dashboards for Hall Rentals and Staff Shift management, completing the GFC Studio ecosystem's business logic layer.

**Status**: READY TO START  
**Previous Phase**: Phase 6A-2 (Advanced Tools) - COMPLETED  
**Reference**: `docs/in-process/PHASE_6A_STUDIO/PHASE_6A_3_MISSION_CONTROL.md`

---

## 🏛️ ARCHITECTURAL CONTEXT
- **Shared Data**: All modules use the existing `GfcDbContext`.
- **Infrastructure**: Entity Framework Core with SQL Server.
- **Location**: 
  - Models: `apps/webapp/GFC.Core/Models/`
  - Services: `apps/webapp/GFC.BlazorServer/Services/`
  - Pages: `apps/webapp/GFC.BlazorServer/Components/Pages/Admin/`
- **Database Tables**: `HallRentals`, `HallRentalRequests`, `StaffShifts`, `ShiftReports` (already exist from Phase 6A-1)

---

## 📋 TASKS TO COMPLETE

### **TASK 1: Hall Rentals Manager UI** 🏛️

#### 1.1 Create Admin Dashboard Page
**File**: `apps/webapp/GFC.BlazorServer/Components/Pages/Admin/RentalManagement.razor`

**Requirements**:
- Route: `/admin/rental-management`
- Display all rental requests in a data grid with columns:
  - Applicant Name
  - Event Date
  - Guest Count
  - Status (Pending/Approved/Denied)
  - Total Price
  - Kitchen Used (Yes/No)
- Filter options:
  - Status dropdown (All, Pending, Approved, Denied)
  - Date range picker
- Action buttons per row:
  - **Approve** (green) - Changes status to Approved, sends confirmation email
  - **Deny** (red) - Changes status to Denied, sends rejection email
  - **View Details** - Opens modal with full request info and internal notes

#### 1.2 Calendar View Component
**Component**: `CalendarView.razor` (embedded in RentalManagement page)

**Requirements**:
- Monthly calendar display showing all approved rentals
- Color-coded events:
  - Green: Approved with kitchen
  - Blue: Approved without kitchen
  - Yellow: Pending requests (semi-transparent)
- Click event to view rental details
- Visual indicator for double-bookings (red border/warning icon)

#### 1.3 Rental Request Detail Modal
**Component**: `RentalDetailModal.razor`

**Requirements**:
- Display all fields from `HallRentalRequest` model
- Editable "Internal Notes" field (admin only)
- Show pricing calculation breakdown
- Action buttons: Approve, Deny, Save Notes, Close

---

### **TASK 2: Staff Shift Manager** 📅

#### 2.1 Create Shift Scheduler Page
**File**: `apps/webapp/GFC.BlazorServer/Components/Pages/Admin/StaffShifts.razor`

**Requirements**:
- Route: `/admin/staff-shifts`
- Weekly grid view (7 days × 2 shifts = 14 slots)
- Columns: Day of Week
- Rows: Day Shift, Night Shift
- Each cell shows:
  - Assigned staff member name (if any)
  - Shift status badge
  - Quick action buttons (Edit, Clear)

#### 2.2 Shift Assignment Interface
**Component**: `ShiftAssignmentModal.razor`

**Requirements**:
- Dropdown to select staff member from Members table
- Date picker for shift date
- Radio buttons for shift type (Day/Night)
- Status dropdown (Scheduled, Completed, No-Show)
- Save and Cancel buttons

#### 2.3 Shift Report Viewer
**Component**: `ShiftReportViewer.razor` (embedded in StaffShifts page)

**Requirements**:
- Read-only display of submitted shift reports
- Filter by date range and staff member
- Display fields:
  - Bartender name
  - Date and shift type
  - Bar sales, Lotto sales, Total deposit
  - Submission timestamp
  - Late submission flag (if submitted >24hrs after shift)
- Export to Excel button

---

### **TASK 3: Service Layer Enhancements** 🛠️

#### 3.1 Enhance IRentalService
**File**: `apps/webapp/GFC.BlazorServer/Services/IRentalService.cs`

Add methods:
```csharp
Task<IEnumerable<HallRentalRequest>> GetRequestsByStatusAsync(string status);
Task<IEnumerable<HallRentalRequest>> GetRequestsByDateRangeAsync(DateTime start, DateTime end);
Task ApproveRequestAsync(int requestId, string approvedBy);
Task DenyRequestAsync(int requestId, string deniedBy, string reason);
Task UpdateInternalNotesAsync(int requestId, string notes);
Task<bool> CheckDoubleBookingAsync(DateTime eventDate);
```

#### 3.2 Enhance IShiftService
**File**: `apps/webapp/GFC.BlazorServer/Services/IShiftService.cs`

Add methods:
```csharp
Task<IEnumerable<StaffShift>> GetShiftsByWeekAsync(DateTime weekStart);
Task<StaffShift> AssignShiftAsync(int staffMemberId, DateTime date, int shiftType);
Task UpdateShiftStatusAsync(int shiftId, string status);
Task<IEnumerable<ShiftReport>> GetReportsByDateRangeAsync(DateTime start, DateTime end);
Task<ShiftReport> GetReportByShiftIdAsync(int shiftId);
```

#### 3.3 Implement Enhanced Services
**Files**: 
- `apps/webapp/GFC.BlazorServer/Services/RentalService.cs`
- `apps/webapp/GFC.BlazorServer/Services/ShiftService.cs`

Implement all new methods with proper error handling and logging.

---

### **TASK 4: Email Notification Integration** 📧

#### 4.1 Rental Approval/Denial Emails
**Service**: `INotificationService`

Add methods:
```csharp
Task SendRentalApprovalEmailAsync(HallRentalRequest request);
Task SendRentalDenialEmailAsync(HallRentalRequest request, string reason);
```

**Email Templates**:
- **Approval**: Include event date, guest count, total price, payment instructions
- **Denial**: Include reason, contact information for questions

#### 4.2 Shift Reminder Emails
Add method:
```csharp
Task SendShiftReminderEmailAsync(StaffShift shift);
```

**Email Template**: Remind staff member of upcoming shift 24 hours in advance.

---

### **TASK 5: Navigation Integration** 🧭

Update `NavMenu.razor` to ensure the following links are active:
- **Rental Management** (`/admin/rental-management`) - Already exists, verify functionality
- **Staff Shifts** (`/admin/staff-shifts`) - Already exists, verify functionality

---

## ⚠️ CRITICAL RULES

1. **Data Validation**: 
   - Prevent double-bookings for the same date
   - Validate guest count ≤ 180 (hall capacity)
   - Ensure shift assignments don't overlap for the same staff member

2. **Security**:
   - All admin pages require `IsAdmin` authorization
   - Log all approval/denial actions with username and timestamp

3. **UI/UX Standards**:
   - Use Bootstrap 5 components for consistency
   - Add loading spinners for async operations
   - Display success/error toast notifications for all actions
   - Ensure mobile responsiveness

4. **Error Handling**:
   - Wrap all database operations in try-catch blocks
   - Log errors with `ILogger`
   - Display user-friendly error messages

5. **Testing**:
   - Verify calendar view correctly identifies double-bookings
   - Test email sending (use test mode if available)
   - Ensure shift grid updates in real-time after assignments

---

## ✅ SUCCESS CRITERIA

- [ ] Hall Rentals Manager displays all requests with correct filtering
- [ ] Calendar view shows approved rentals and warns of double-bookings
- [ ] Approve/Deny actions update status and send emails
- [ ] Staff Shifts page displays weekly grid with assigned shifts
- [ ] Shift assignment modal successfully creates/updates shifts
- [ ] Shift Report Viewer displays submitted reports with filtering
- [ ] All services registered in `Program.cs`
- [ ] Navigation menu links work correctly
- [ ] Project builds successfully without errors
- [ ] All admin pages require proper authorization

---

## 📝 IMPLEMENTATION NOTES

### Database Schema (Already Exists)
The following tables were created in Phase 6A-1:

**HallRentalRequest**:
- Id (int, PK)
- ApplicantName (string)
- ContactInfo (string)
- EventDate (DateTime)
- GuestCount (int)
- KitchenUsed (bool)
- TotalPrice (decimal)
- Status (string: Pending/Approved/Denied)
- InternalNotes (string, nullable)
- CreatedAt (DateTime)
- ApprovedBy (string, nullable)
- ApprovedAt (DateTime?, nullable)

**StaffShift**:
- Id (int, PK)
- StaffMemberId (int, FK to Members)
- Date (DateTime)
- ShiftType (int: 1=Day, 2=Night)
- Status (string: Scheduled/Completed/NoShow)

**ShiftReport**:
- Id (int, PK)
- ShiftId (int, FK to StaffShift)
- BartenderId (int, FK to Members)
- BarSales (decimal)
- LottoSales (decimal)
- TotalDeposit (decimal)
- SubmittedAt (DateTime)
- IsLate (bool)

### Pricing Rules (from GFC Master Spec)
- Member Rate: $250
- Non-Member Rate: $500
- Kitchen Usage: +$50
- Capacity: 180 guests max

---

**Task Created**: 2025-12-23  
**Assigned To**: JULES 🚀  
**Estimated Complexity**: Medium-High  
**Estimated Time**: 6-8 hours
