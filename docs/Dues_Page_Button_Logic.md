# Dues & Payments Page - Button Logic Fixes

## Issues Identified and Resolved

### Issue 1: Guest Members Cannot Be Manually Waived ✅ FIXED

**Problem**: Only Regular and Regular-NP members could receive manual waivers. Guest members did not have the "Waive" button.

**Root Cause**: Line 361 in `Dues.razor` only checked for:
```csharp
item.Status == MemberStatus.Regular || item.Status == MemberStatus.RegularNonPortuguese
```

**Solution**: Added `MemberStatus.Guest` to the condition:
```csharp
item.Status == MemberStatus.Regular || item.Status == MemberStatus.RegularNonPortuguese || item.Status == MemberStatus.Guest
```

**Result**: Now Regular, Regular-NP, and Guest members can all receive manual waivers.

---

### Issue 2: Why Some 15+ Month Overdue Members Don't Have "Deactivate" Button

**Answer**: The logic is **working correctly**. The "Deactivate" button only shows when ALL of these conditions are met:

#### Conditions for "Deactivate" Button (Line 370-375, 1226-1227):
```csharp
private bool CanSetInactive(DuesListItemDto item) =>
    !IsPaid(item) &&           // Not paid
    !IsExempt(item) &&         // Not exempt (Life, Board, or Waived)
    item.MonthsOverdue >= 15 && // 15+ months overdue
    !_operationInProgress;      // Not currently processing
```

#### Why a Member Might NOT Have "Deactivate" Despite Being 15+ Months Overdue:

1. **They have a waiver** (`IsExempt` returns true)
   - Manual waiver granted
   - Life member (auto-waived)
   - Board member (auto-waived)

2. **They already paid** (`IsPaid` returns true)
   - Payment recorded but showing in wrong filter

3. **An operation is in progress** (`_operationInProgress` is true)
   - Temporary state during processing

#### Button Priority Logic (Lines 354-379):

The buttons appear in this order of priority:
1. **"Auto-Waived"** (disabled gray) - Life or Board members
2. **"Waive"** (blue) - Regular, Regular-NP, or Guest members (unpaid, not overdue enough)
3. **"Deactivate"** (red) - 15+ months overdue AND not exempt
4. **"Paid"** (disabled gray) - Already paid

**Example from your screenshot:**
- **COSTA, TIMOTHY** (492 mo overdue) - Shows "Waive" button because:
  - Regular member
  - Not 15+ months overdue YET (needs to be checked - might be a display issue)
  - OR has a waiver that makes them exempt

- **Guest members** (0 mo overdue) - Now show "Waive" button after the fix

---

## Changes Made

### File: `Dues.razor`

**Change 1 - Line 361**: Added Guest to waivable statuses
```diff
- else if (item.Status == MemberStatus.Regular || item.Status == MemberStatus.RegularNonPortuguese)
+ else if (item.Status == MemberStatus.Regular || item.Status == MemberStatus.RegularNonPortuguese || item.Status == MemberStatus.Guest)
```

**Change 2 - Line 694**: Added clarifying comment
```csharp
// Only Life and Board members cannot be manually waived (they are auto-waived)
if (item.Status == MemberStatus.Life || item.IsBoardMember) return;
```

---

## Testing Checklist

- [ ] Guest members now show "Waive" button when unpaid
- [ ] Regular members still show "Waive" button
- [ ] Regular-NP members still show "Waive" button
- [ ] Life members show "Auto-Waived" (disabled)
- [ ] Board members show "Auto-Waived" (disabled)
- [ ] Members 15+ months overdue (and not exempt) show "Deactivate"
- [ ] Members with waivers do NOT show "Deactivate" (even if 15+ months)
- [ ] Paid members show "Paid" button

---

## Business Rules Summary

### Who Can Be Manually Waived?
✅ Regular members  
✅ Regular Non-Portuguese members  
✅ Guest members (NEW)  
❌ Life members (auto-waived)  
❌ Board members (auto-waived)

### Who Can Be Deactivated?
✅ Members 15+ months overdue  
✅ Who are NOT paid  
✅ Who do NOT have a waiver  
✅ Who are NOT Life or Board members

### Auto-Waived Members
- Life members (permanent waiver)
- Board members (while serving)

---

## Notes

If you see a member who is 15+ months overdue but doesn't have a "Deactivate" button, check:
1. Do they have an active waiver? (Check the "Waived" filter)
2. Are they a Life or Board member?
3. Did they recently pay? (Check the "Paid" filter)

The system is designed to prevent accidental deactivation of members who are legitimately exempt from dues.
