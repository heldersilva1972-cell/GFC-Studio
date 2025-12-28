# Implementation Plan - Time-Slot Booking System

## Git Workflow

### Branch Strategy
```bash
# Create feature branch from master
git checkout master
git pull origin master
git checkout -b feature/time-slot-booking

# Work on feature
# ... make changes ...
git add .
git commit -m "feat: add time-slot booking system"

# When ready to merge
git checkout master
git merge feature/time-slot-booking
git push origin master

# If issues arise, revert to master
git checkout master
# feature branch remains for future fixes
```

## Phase 1: Backend & Database (4-5 hours)

### Step 1.1: Database Migration (30 min)
**Files:**
- Create: `migrations/AddTimeSlotBookingFields.sql`

**Tasks:**
1. ✅ Add `AllowSecondBooking` column to AvailabilityCalendar
2. ✅ Add `BufferTimeMinutes` column to AvailabilityCalendar
3. ✅ Add `IsSecondEvent` column to AvailabilityCalendar
4. ✅ Update existing records with default values
5. ✅ Test migration on dev database

**SQL Script:**
```sql
-- See 02_TECHNICAL_SPEC.md for full migration script
```

### Step 1.2: Update C# Models (15 min)
**Files:**
- `GFC.Core/Models/AvailabilityCalendar.cs`

**Tasks:**
1. ✅ Add new properties to AvailabilityCalendar class
2. ✅ Add data annotations if needed
3. ✅ Update DbContext if using EF Core migrations

### Step 1.3: Service Layer - Interface (30 min)
**Files:**
- `GFC.BlazorServer/Services/IRentalService.cs`

**Tasks:**
1. ✅ Add `CanAddSecondEventAsync` method signature
2. ✅ Add `GetFirstEventForDateAsync` method signature
3. ✅ Add `GetAvailableStartTimeAsync` method signature
4. ✅ Add `AddSecondEventAsync` method signature
5. ✅ Add `ValidateTimeSlotAsync` method signature

### Step 1.4: Service Layer - Implementation (2 hours)
**Files:**
- `GFC.BlazorServer/Services/RentalService.cs`

**Tasks:**
1. ✅ Implement time overlap detection helper
2. ✅ Implement buffer time calculation helper
3. ✅ Implement event count check helper
4. ✅ Implement `CanAddSecondEventAsync`
5. ✅ Implement `GetFirstEventForDateAsync`
6. ✅ Implement `GetAvailableStartTimeAsync`
7. ✅ Implement `AddSecondEventAsync`
8. ✅ Implement `ValidateTimeSlotAsync`
9. ✅ Update `IsDateAlreadyBookedAsync` to respect time slots
10. ✅ Add error handling and logging

**Key Logic:**
```csharp
public async Task<bool> CanAddSecondEventAsync(DateTime date)
{
    var eventCount = await GetEventCountForDateAsync(date);
    return eventCount == 1; // Can add second if exactly 1 exists
}

public async Task<AvailabilityCalendar?> GetFirstEventForDateAsync(DateTime date)
{
    return await _context.AvailabilityCalendars
        .Where(e => e.Date.Date == date.Date && !e.IsSecondEvent)
        .FirstOrDefaultAsync();
}

public async Task<TimeSpan?> GetAvailableStartTimeAsync(DateTime date)
{
    var firstEvent = await GetFirstEventForDateAsync(date);
    if (firstEvent == null || !firstEvent.AllowSecondBooking) return null;
    
    var endTime = TimeSpan.Parse(firstEvent.EndTime);
    var buffer = TimeSpan.FromMinutes(firstEvent.BufferTimeMinutes ?? 0);
    return endTime.Add(buffer);
}
```

### Step 1.5: Unit Tests (1 hour)
**Files:**
- Create: `GFC.Tests/Services/RentalServiceTimeSlotTests.cs`

**Test Cases:**
1. ✅ Time overlap detection works correctly
2. ✅ Buffer time calculation is accurate
3. ✅ Event count check handles all scenarios
4. ✅ Cannot add third event
5. ✅ Can add second event when first exists
6. ✅ Validation rejects overlapping times
7. ✅ Validation accepts non-overlapping times

## Phase 2: Admin Dashboard UI (3-4 hours)

### Step 2.1: Update SelectDay Logic (30 min)
**Files:**
- `GFC.BlazorServer/Components/Pages/Admin/HallManagementDashboard.razor`

**Tasks:**
1. ✅ Modify `SelectDay` to check event count
2. ✅ If 1 event exists, show "Add Second Event" option
3. ✅ If 2 events exist, show both events (read-only)

### Step 2.2: Create Second Event Modal (2 hours)
**Files:**
- `GFC.BlazorServer/Components/Pages/Admin/HallManagementDashboard.razor`
- `GFC.BlazorServer/Components/Pages/Admin/HallManagementDashboard.razor.css`

**Tasks:**
1. ✅ Add modal state variables
2. ✅ Create modal HTML structure
3. ✅ Add buffer time input field
4. ✅ Add available time display (calculated)
5. ✅ Add radio buttons: "Open to public" vs "Add club event"
6. ✅ Conditionally show club event form
7. ✅ Add validation for buffer time (required, > 0)
8. ✅ Add validation for club event times (no overlap)
9. ✅ Style modal with gold theme

### Step 2.3: Implement Save Logic (1 hour)
**Files:**
- `GFC.BlazorServer/Components/Pages/Admin/HallManagementDashboard.razor`

**Tasks:**
1. ✅ Create `SaveSecondEvent` method
2. ✅ Validate buffer time
3. ✅ If "Open to public": Update first event with AllowSecondBooking flag
4. ✅ If "Add club event": Create new AvailabilityCalendar record
5. ✅ Show success/error toast
6. ✅ Reload calendar data
7. ✅ Close modal

### Step 2.4: Edit First Event Warning (30 min)
**Files:**
- `GFC.BlazorServer/Components/Pages/Admin/HallManagementDashboard.razor`

**Tasks:**
1. ✅ Check if second event exists before allowing edit
2. ✅ Show warning modal with details
3. ✅ Suggest recalculating available time
4. ✅ Require confirmation to proceed

### Step 2.5: Update Calendar Display (30 min)
**Files:**
- `GFC.BlazorServer/Components/Pages/Admin/HallManagementDashboard.razor`
- `GFC.BlazorServer/Components/Pages/Admin/HallManagementDashboard.razor.css`

**Tasks:**
1. ✅ Show indicator for partial days (maybe small icon)
2. ✅ Update `GetEventsForDay` to handle multiple events
3. ✅ Show both events in calendar cell (if space allows)

## Phase 3: Public Website Integration (4-5 hours)

### Step 3.1: API Endpoint (1 hour)
**Files:**
- Create: `GFC.BlazorServer/Controllers/Api/CalendarApiController.cs`

**Tasks:**
1. ✅ Create GET endpoint: `/api/calendar/availability/{year}/{month}`
2. ✅ Return day status (available/partial/booked)
3. ✅ Include available start time for partial days
4. ✅ Add CORS configuration if needed

**Response Format:**
```json
{
  "month": 12,
  "year": 2025,
  "days": [
    {
      "date": "2025-12-15",
      "status": "partial",
      "availableFrom": "1:00 PM"
    },
    {
      "date": "2025-12-24",
      "status": "booked"
    }
  ]
}
```

### Step 3.2: Next.js Calendar Component (2 hours)
**Files:**
- `apps/website/components/HallRentalCalendar.tsx`
- `apps/website/components/HallRentalCalendar.module.css`

**Tasks:**
1. ✅ Fetch calendar data from new API
2. ✅ Update day rendering to show 3 states (green/yellow/red)
3. ✅ Add click handler for partial days
4. ✅ Show popup with available time
5. ✅ Style partial days with yellow background

### Step 3.3: Rental Form Updates (1.5 hours)
**Files:**
- `apps/website/app/hall-rental/page.tsx`
- `apps/website/components/RentalForm.tsx`

**Tasks:**
1. ✅ Pass selected date info to form
2. ✅ If partial day, show available time notice
3. ✅ Disable start time options before available time
4. ✅ Add client-side validation for time slot
5. ✅ Add server-side validation in API

### Step 3.4: Validation API Endpoint (30 min)
**Files:**
- `GFC.BlazorServer/Controllers/Api/RentalRequestApiController.cs`

**Tasks:**
1. ✅ Update POST endpoint to validate time slot
2. ✅ Call `ValidateTimeSlotAsync` from service
3. ✅ Return clear error message if invalid
4. ✅ Prevent booking if validation fails

## Phase 4: Testing & QA (2-3 hours)

### Step 4.1: Manual Testing Checklist
**Admin Dashboard:**
- [ ] Add second event (open to public) - verify flag set
- [ ] Add second event (club event) - verify both events show
- [ ] Try to add third event - verify blocked
- [ ] Edit first event with second event existing - verify warning
- [ ] Delete first event with second event - verify handling
- [ ] Buffer time validation (0, negative, very large)
- [ ] Time overlap validation

**Public Website:**
- [ ] View calendar with partial days - verify yellow color
- [ ] Click partial day - verify available time shown
- [ ] Book partial day with valid time - verify success
- [ ] Book partial day with invalid time - verify error
- [ ] Book partial day after second event added - verify blocked

### Step 4.2: Edge Case Testing
- [ ] Midnight crossing events (11 PM - 1 AM)
- [ ] Same start and end time
- [ ] Very short buffer times (1 minute)
- [ ] Very long buffer times (8 hours)
- [ ] Concurrent booking attempts
- [ ] Database transaction rollback scenarios

### Step 4.3: Performance Testing
- [ ] Calendar load time with 100+ events
- [ ] Time slot validation response time
- [ ] Concurrent admin edits

## Phase 5: Documentation & Deployment (1 hour)

### Step 5.1: User Documentation
**Files:**
- Create: `docs/user-guides/time-slot-booking-admin.md`
- Create: `docs/user-guides/time-slot-booking-public.md`

### Step 5.2: Deployment Checklist
- [ ] Run database migration on production
- [ ] Deploy backend changes
- [ ] Deploy frontend changes
- [ ] Verify API endpoints accessible
- [ ] Test on production with real data
- [ ] Monitor error logs for 24 hours

### Step 5.3: Rollback Plan
```bash
# If issues arise in production
git revert <commit-hash>
git push origin master

# Rollback database migration
-- Run reverse migration script
```

## Timeline Summary

| Phase | Duration | Dependencies |
|-------|----------|--------------|
| Phase 1: Backend | 4-5 hours | None |
| Phase 2: Admin UI | 3-4 hours | Phase 1 complete |
| Phase 3: Public Website | 4-5 hours | Phase 1 complete |
| Phase 4: Testing | 2-3 hours | Phases 1-3 complete |
| Phase 5: Deployment | 1 hour | Phase 4 complete |
| **Total** | **14-18 hours** | |

## Risk Mitigation

### High Risk Items
1. **Database migration failure**
   - Mitigation: Test on dev/staging first
   - Backup production database before migration

2. **Time zone handling**
   - Mitigation: Store times as strings (e.g., "2:00 PM")
   - Document timezone assumptions

3. **Concurrent booking conflicts**
   - Mitigation: Use database transactions
   - Add optimistic locking if needed

### Medium Risk Items
1. **User confusion about partial days**
   - Mitigation: Clear UI messaging
   - Add help tooltips

2. **Performance with many events**
   - Mitigation: Add database indexes
   - Implement caching if needed

## Success Criteria

✅ Admin can enable second booking on any date
✅ Admin can set custom buffer times
✅ Admin can add club events to partial days
✅ Public sees partial days as yellow
✅ Public can only book available time slots
✅ System prevents overlapping events
✅ Maximum 2 events per day enforced
✅ All validation rules working correctly
✅ No breaking changes to existing functionality
