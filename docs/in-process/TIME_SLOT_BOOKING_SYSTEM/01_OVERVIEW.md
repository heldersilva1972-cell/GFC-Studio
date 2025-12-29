# Time-Slot Based Booking System - Overview

## Project Goal
Enable multiple events on the same day with time-slot management, allowing admins to maximize hall usage while maintaining control over public bookings.

## Current System Limitation
- One event per day (hard block)
- If morning meeting exists, entire day is blocked
- Lost revenue opportunities for evening rentals

## New System Capabilities
- **Admin Control**: Enable "second booking" on specific dates
- **Time Management**: Set buffer times between events
- **Public Access**: Show "Partially Available" with time constraints
- **Maximum**: 2 events per day
- **Safety**: Public website still defaults to one booking unless admin enables partial day

## Key Business Rules

### Default Behavior
- ✅ One event per day (both admin and public)
- ✅ Existing behavior unchanged unless admin opts in

### Admin Override
- ✅ Admin can enable "Allow Second Booking" for specific dates
- ✅ Admin sets buffer time in minutes (required, no default)
- ✅ Admin can either:
  - Open slot to public (goes through normal approval)
  - Add second club event immediately (blocks public)

### Public Website
- ✅ Shows "Partially Available" (no event details shown)
- ✅ Calculates available time: First Event End + Buffer
- ✅ User can only book remaining time slot
- ✅ Normal approval process applies

### Validation Rules
- ✅ Maximum 2 events per day
- ✅ Time ranges cannot overlap
- ✅ Buffer time must be respected
- ✅ If admin adds second club event → Day shows "Fully Booked" to public

## Example Scenario

**Admin adds first event:**
- Board Meeting: 9:00 AM - 12:00 PM
- Day shows as "Booked" (red) to public

**Admin enables second booking:**
- Sets buffer time: 60 minutes
- Chooses: "Open to public rentals"
- Day shows as "Partially Available" (yellow) to public

**Public user clicks the date:**
- Sees: "Available from 1:00 PM onwards"
- Books: 6:00 PM - 11:00 PM (Birthday Party)
- Submits request → Admin approves
- Day now shows as "Fully Booked" (red)

## Implementation Status
- 📋 **Status**: Planning Phase
- 🎯 **Priority**: Medium (Revenue Enhancement)
- ⏱️ **Estimated Effort**: 6-8 hours
- 🔀 **Git Strategy**: Feature branch (`feature/time-slot-booking`)

## Next Steps
1. Review and approve this plan
2. Create feature branch
3. Implement Phase 1 (Backend + Admin Dashboard)
4. Test thoroughly
5. Implement Phase 2 (Public Website Integration)
6. Merge to master

## Documentation Files
- `01_OVERVIEW.md` - This file
- `02_TECHNICAL_SPEC.md` - Database schema and code changes
- `03_UI_MOCKUPS.md` - Admin dashboard flow
- `04_IMPLEMENTATION_PLAN.md` - Step-by-step coding tasks
- `05_TESTING_CHECKLIST.md` - QA scenarios
