# TASK FOR JULES: GFC Studio - Phase 6A-4 (Enhanced Staff Shifts UI/UX)

## 🎯 OBJECTIVE
Enhance the Staff Shifts page with a professional, interactive weekly grid scheduler, complete with modal-based assignment workflows, visual status indicators, and mobile responsiveness.

**Status**: READY TO START  
**Previous Phase**: Phase 6A-3 (Mission Control Backend) - IN PROGRESS  
**Dependencies**: Phase 6A-3 must complete first (basic StaffShifts page and services)  
**Reference**: `docs/in-process/PHASE_6A_STUDIO/MASTER_PLAN.md`

---

## 🏛️ ARCHITECTURAL CONTEXT
- **Builds Upon**: Phase 6A-3's `StaffShifts.razor` and `IShiftService`
- **Enhancement Focus**: Transform basic shift management into an intuitive, visual scheduling experience
- **Infrastructure**: Blazor components, Bootstrap 5, Entity Framework Core
- **Location**: 
  - Main Page: `apps/webapp/GFC.BlazorServer/Components/Pages/Admin/StaffShifts.razor`
  - Components: `apps/webapp/GFC.BlazorServer/Components/Shared/`
  - Services: `apps/webapp/GFC.BlazorServer/Services/` (extend existing)

---

## 📋 TASKS TO COMPLETE

### **TASK 1: Weekly Grid Scheduler** 📅

#### 1.1 Transform StaffShifts Page to Grid Layout
**File**: `apps/webapp/GFC.BlazorServer/Components/Pages/Admin/StaffShifts.razor`

**Visual Layout**:
```
┌─────────────────────────────────────────────────────────────┐
│  Staff Shifts Manager                          [Week: ◄ ►]  │
│  December 23-29, 2025                                        │
├─────────────────────────────────────────────────────────────┤
│  ┌───────┬────────┬────────┬────────┬────────┬────────┬────┐│
│  │       │  Mon   │  Tue   │  Wed   │  Thu   │  Fri   │ Sat││
│  │       │ 12/23  │ 12/24  │ 12/25  │ 12/26  │ 12/27  │12/28││
│  ├───────┼────────┼────────┼────────┼────────┼────────┼────┤│
│  │ Day   │ [+]    │ John D │ [+]    │ Mike S │ [+]    │ [+]││
│  │ Shift │        │ ✓      │        │ ⏱️     │        │    ││
│  ├───────┼────────┼────────┼────────┼────────┼────────┼────┤│
│  │ Night │ Sarah M│ [+]    │ CLOSED │ [+]    │ Tom R  │ [+]││
│  │ Shift │ ✓      │        │        │        │ ⏱️     │    ││
│  └───────┴────────┴────────┴────────┴────────┴────────┴────┘│
│  Legend: [+] = Empty  ✓ = Completed  ⏱️ = Scheduled          │
└─────────────────────────────────────────────────────────────┘
```

**Requirements**:
- **Grid Structure**: 
  - 2 rows (Day Shift, Night Shift)
  - 7 columns (Monday-Sunday)
  - Total: 14 interactive cells
- **Week Navigation**:
  - Header shows date range (e.g., "December 23-29, 2025")
  - **◄ Previous Week** button: Loads previous 7 days
  - **► Next Week** button: Loads next 7 days
  - Default: Current week on page load
- **Cell States & Styling**:
  1. **Empty**: 
     - Display: `[+]` button
     - Background: `#f8f9fa` (light gray)
     - Cursor: pointer
  2. **Scheduled**: 
     - Display: Staff name + ⏱️ badge
     - Background: `#ffffff` (white)
     - Badge color: `#0d6efd` (blue)
  3. **Completed**: 
     - Display: Staff name + ✓ badge
     - Background: `#d4edda` (light green)
     - Badge color: `#198754` (green)
  4. **No-Show**: 
     - Display: Staff name + ⚠️ badge
     - Background: `#f8d7da` (light red)
     - Badge color: `#dc3545` (red)
  5. **Closed**: 
     - Display: "CLOSED" text
     - Background: `#6c757d` (gray)
     - Text color: `#ffffff` (white)

#### 1.2 Cell Interaction Logic

**A. Adding a Shift (Click [+] button)**:
1. User clicks `[+]` in empty cell
2. `ShiftAssignmentModal` opens in **create mode**
3. Date and shift type auto-populated from clicked cell
4. User selects staff member from dropdown
5. User selects status (default: "Scheduled")
6. Click "Assign Shift" button
7. Call `ShiftService.AssignShiftAsync()`
8. On success: Close modal, refresh grid, show success toast
9. On error: Display error message in modal

**B. Editing a Shift (Click assigned cell)**:
1. User clicks cell with staff name
2. `ShiftAssignmentModal` opens in **edit mode**
3. All fields pre-populated with existing data
4. User can:
   - Change staff member
   - Update status
   - Click "Remove Assignment" to delete
5. Click "Save Changes" button
6. Call `ShiftService.UpdateShiftAsync()`
7. On success: Close modal, refresh grid, show success toast
8. On error: Display error message in modal

**C. Quick Actions (Hover over assigned cell)**:
- On hover, show overlay with action buttons:
  ```
  ┌──────────────────────┐
  │  John Doe            │
  │  ✓ Completed         │
  │  [✏️ Edit] [🗑️ Clear] │
  └──────────────────────┘
  ```
- **✏️ Edit**: Opens edit modal
- **🗑️ Clear**: Shows confirmation dialog → Deletes assignment

**D. View Shift Report (Click completed shift)**:
- Only enabled if status = "Completed"
- Opens `ShiftReportViewer` modal
- Displays sales data, submission time, late flag

---

### **TASK 2: Shift Assignment Modal** 📝

#### 2.1 Create ShiftAssignmentModal Component
**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/ShiftAssignmentModal.razor`

**Create Mode Layout**:
```
┌─────────────────────────────────────────┐
│  Assign Shift                     [×]   │
├─────────────────────────────────────────┤
│  Date: [Monday, Dec 23, 2025] 🔒       │
│  Shift Type: ● Day Shift  ○ Night Shift│
│                                         │
│  Staff Member:                          │
│  ┌─────────────────────────────────┐   │
│  │ [Select Staff Member ▼]         │   │
│  │  - John Doe                     │   │
│  │  - Sarah Miller                 │   │
│  │  - Mike Smith                   │   │
│  └─────────────────────────────────┘   │
│                                         │
│  Status: [Scheduled ▼]                  │
│  (Options: Scheduled, Completed, NoShow)│
│                                         │
│  [Cancel]              [Assign Shift]   │
└─────────────────────────────────────────┘
```

**Edit Mode Layout**:
```
┌─────────────────────────────────────────┐
│  Edit Shift Assignment            [×]   │
├─────────────────────────────────────────┤
│  Date: [Monday, Dec 23, 2025] 🔒       │
│  Shift Type: ● Day Shift  ○ Night Shift│
│                                         │
│  Staff Member:                          │
│  ┌─────────────────────────────────┐   │
│  │ John Doe                    ▼   │   │
│  └─────────────────────────────────┘   │
│                                         │
│  Status: [Completed ▼]                  │
│                                         │
│  🗑️ [Remove Assignment]                │
│                                         │
│  [Cancel]              [Save Changes]   │
└─────────────────────────────────────────┘
```

**Component Parameters**:
```csharp
[Parameter] public bool IsOpen { get; set; }
[Parameter] public bool IsEditMode { get; set; }
[Parameter] public DateTime ShiftDate { get; set; }
[Parameter] public int ShiftType { get; set; } // 1=Day, 2=Night
[Parameter] public StaffShift? ExistingShift { get; set; }
[Parameter] public EventCallback OnSave { get; set; }
[Parameter] public EventCallback OnCancel { get; set; }
```

**Field Requirements**:

1. **Date Field**:
   - Type: Read-only text display
   - Format: "Monday, December 23, 2025"
   - Locked (🔒 icon) - cannot be changed

2. **Shift Type**:
   - Type: Radio buttons
   - Options: "Day Shift" (1), "Night Shift" (2)
   - Create mode: Pre-selected from clicked cell
   - Edit mode: Locked (cannot change)

3. **Staff Member Dropdown**:
   - Data source: `Members` table (via service)
   - Filter: Only active members
   - Sort: Alphabetically by last name
   - Display format: "FirstName LastName"
   - Required field validation

4. **Status Dropdown**:
   - Options: 
     - "Scheduled" (default for new)
     - "Completed"
     - "NoShow"
   - Default: "Scheduled" in create mode
   - Editable in both modes

**Validation Rules**:
- Staff member must be selected
- Check for duplicate assignment:
  - Query: Same staff + same date + same shift type
  - If duplicate found: Show error toast "This staff member is already assigned to this shift"
  - Prevent save until resolved
- Status must be selected

**Actions**:

**Create Mode**:
- **Cancel**: Close modal without saving
- **Assign Shift**: 
  - Validate form
  - Call `ShiftService.AssignShiftAsync(staffMemberId, date, shiftType, status)`
  - On success: Trigger `OnSave` callback
  - On error: Display error message

**Edit Mode**:
- **Cancel**: Close modal without saving
- **Save Changes**: 
  - Validate form
  - Call `ShiftService.UpdateShiftAsync(shiftId, staffMemberId, status)`
  - On success: Trigger `OnSave` callback
  - On error: Display error message
- **Remove Assignment**: 
  - Show confirmation dialog: "Are you sure you want to remove this shift assignment?"
  - If confirmed: Call `ShiftService.DeleteShiftAsync(shiftId)`
  - On success: Trigger `OnSave` callback

---

### **TASK 3: Shift Report Viewer Modal** 📊

#### 3.1 Create ShiftReportViewer Component
**File**: `apps/webapp/GFC.BlazorServer/Components/Shared/ShiftReportViewer.razor`

**Modal Layout**:
```
┌─────────────────────────────────────────┐
│  Shift Report - John Doe          [×]   │
│  Monday, Dec 23, 2025 - Day Shift       │
├─────────────────────────────────────────┤
│  Bartender: John Doe                    │
│  Submitted: Dec 24, 2025 8:15 AM        │
│  Status: ✓ On Time                      │
│  (or: ⚠️ Late - Submitted 36 hrs late)  │
│                                         │
│  ─────────────────────────────────────  │
│  Bar Sales:        $1,245.00            │
│  Lotto Sales:        $387.50            │
│  Total Deposit:    $1,632.50            │
│  ─────────────────────────────────────  │
│                                         │
│  [Close]              [Export PDF]      │
└─────────────────────────────────────────┘
```

**Component Parameters**:
```csharp
[Parameter] public bool IsOpen { get; set; }
[Parameter] public int ShiftId { get; set; }
[Parameter] public EventCallback OnClose { get; set; }
```

**Requirements**:
- **Data Loading**: 
  - On open, call `ShiftService.GetReportByShiftIdAsync(shiftId)`
  - Show loading spinner while fetching
  - If no report exists: Display "No report submitted yet" message
- **Display Fields**:
  - Bartender name (from Members table via FK)
  - Shift date and type (formatted)
  - Submission timestamp (formatted: "MMM dd, yyyy h:mm tt")
  - Late flag calculation:
    - If `SubmittedAt > ShiftDate + 24 hours`: Show ⚠️ "Late - Submitted X hrs late"
    - Else: Show ✓ "On Time"
  - Bar sales (currency format: `$1,245.00`)
  - Lotto sales (currency format)
  - Total deposit (currency format)
- **Actions**:
  - **Close**: Dismiss modal
  - **Export PDF**: Generate PDF with report data (optional for Phase 6A-4, can defer)

---

### **TASK 4: Mobile Responsiveness** 📱

#### 4.1 Implement Responsive Grid
**Breakpoint**: `< 768px` (Bootstrap `md` breakpoint)

**Desktop View** (≥ 768px):
- Full 7-column grid as shown above

**Mobile View** (< 768px):
- Collapse to day-by-day list view
- Swipeable navigation between days

**Mobile Layout**:
```
┌─────────────────────────────────┐
│  [◄] Monday, Dec 23 [►]         │
│  ─────────────────────────────  │
│  Day Shift:                     │
│  [+] Add Assignment             │
│                                 │
│  Night Shift:                   │
│  Sarah Miller ✓                 │
│  [Edit] [Clear]                 │
├─────────────────────────────────┤
│  [◄] Tuesday, Dec 24 [►]        │
│  ─────────────────────────────  │
│  Day Shift:                     │
│  John Doe ⏱️                    │
│  [Edit] [Clear]                 │
│                                 │
│  Night Shift:                   │
│  [+] Add Assignment             │
└─────────────────────────────────┘
```

**Implementation**:
- Use Bootstrap responsive utilities (`d-none d-md-block`, etc.)
- Day navigation: ◄ Previous Day / Next Day ►
- Each day shows 2 shift slots vertically
- Same interaction patterns as desktop (click to edit/add)

---

### **TASK 5: Service Layer Enhancements** 🛠️

#### 5.1 Extend IShiftService
**File**: `apps/webapp/GFC.BlazorServer/Services/IShiftService.cs`

Add methods (if not already present from Phase 6A-3):
```csharp
Task<IEnumerable<StaffShift>> GetShiftsByWeekAsync(DateTime weekStart);
Task<StaffShift> AssignShiftAsync(int staffMemberId, DateTime date, int shiftType, string status = "Scheduled");
Task UpdateShiftAsync(int shiftId, int staffMemberId, string status);
Task DeleteShiftAsync(int shiftId);
Task<bool> CheckDuplicateAssignmentAsync(int staffMemberId, DateTime date, int shiftType, int? excludeShiftId = null);
Task<ShiftReport?> GetReportByShiftIdAsync(int shiftId);
```

#### 5.2 Implement Enhanced Methods
**File**: `apps/webapp/GFC.BlazorServer/Services/ShiftService.cs`

**GetShiftsByWeekAsync**:
```csharp
public async Task<IEnumerable<StaffShift>> GetShiftsByWeekAsync(DateTime weekStart)
{
    var weekEnd = weekStart.AddDays(7);
    return await _context.StaffShifts
        .Include(s => s.StaffMember) // Assuming navigation property
        .Where(s => s.Date >= weekStart && s.Date < weekEnd)
        .OrderBy(s => s.Date)
        .ThenBy(s => s.ShiftType)
        .ToListAsync();
}
```

**CheckDuplicateAssignmentAsync**:
```csharp
public async Task<bool> CheckDuplicateAssignmentAsync(int staffMemberId, DateTime date, int shiftType, int? excludeShiftId = null)
{
    var query = _context.StaffShifts
        .Where(s => s.StaffMemberId == staffMemberId 
                 && s.Date.Date == date.Date 
                 && s.ShiftType == shiftType);
    
    if (excludeShiftId.HasValue)
        query = query.Where(s => s.Id != excludeShiftId.Value);
    
    return await query.AnyAsync();
}
```

---

## ⚠️ CRITICAL RULES

1. **Data Integrity**:
   - Prevent duplicate assignments (same staff, same date/shift)
   - Validate all user inputs before database operations
   - Use transactions for multi-step operations

2. **UI/UX Standards**:
   - All modals use Bootstrap 5 modal component
   - Loading states for all async operations
   - Success/error toast notifications for all actions
   - Smooth transitions and animations
   - Accessible keyboard navigation (Tab, Enter, Esc)

3. **Error Handling**:
   - Wrap all service calls in try-catch blocks
   - Log errors with `ILogger<T>`
   - Display user-friendly error messages
   - Never expose stack traces to users

4. **Performance**:
   - Load only current week's data by default
   - Implement efficient database queries (use `.Include()` wisely)
   - Debounce rapid clicks on action buttons

5. **Security**:
   - Admin-only access (verify in `@code` block)
   - Validate all inputs server-side
   - Log all shift assignments/modifications with username

---

## ✅ SUCCESS CRITERIA

- [ ] Weekly grid displays correctly with 7 days × 2 shifts
- [ ] Week navigation (Previous/Next) loads correct data
- [ ] Empty cells show [+] button and open create modal on click
- [ ] Assigned cells show staff name and status badge
- [ ] Hover over assigned cells shows Edit/Clear quick actions
- [ ] Create modal successfully assigns shifts
- [ ] Edit modal successfully updates shifts
- [ ] Remove assignment works with confirmation
- [ ] Duplicate assignment validation prevents conflicts
- [ ] Shift Report Viewer displays report data correctly
- [ ] Mobile view collapses to day-by-day list
- [ ] All modals are keyboard accessible (Esc to close)
- [ ] Toast notifications appear for all actions
- [ ] Loading spinners show during async operations
- [ ] Project builds without errors
- [ ] All components properly scoped and reusable

---

## 📝 IMPLEMENTATION NOTES

### Component Structure
```
StaffShifts.razor (Main Page)
├── WeekNavigator (inline component)
├── ShiftGrid (inline or separate component)
│   └── ShiftCell (inline component)
├── ShiftAssignmentModal.razor
└── ShiftReportViewer.razor
```

### State Management
- Use component-level state for modal visibility
- Refresh grid after any data modification
- Maintain current week selection across operations

### Styling
- Use Bootstrap 5 utilities for layout
- Custom CSS for cell states (create `StaffShifts.razor.css`)
- Ensure consistent spacing and alignment

### Testing Checklist
1. Create shift for each day/shift combination
2. Edit existing shifts
3. Delete shifts
4. Try to create duplicate assignment (should fail)
5. Navigate between weeks
6. Test on mobile viewport
7. Test keyboard navigation
8. Verify all toasts appear correctly

---

**Task Created**: 2025-12-23  
**Assigned To**: JULES 🚀  
**Estimated Complexity**: Medium-High  
**Estimated Time**: 4-6 hours  
**Depends On**: Phase 6A-3 completion
