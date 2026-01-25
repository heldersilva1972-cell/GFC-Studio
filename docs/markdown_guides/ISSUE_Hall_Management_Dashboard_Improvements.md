# Hall Management Dashboard - UX/UI Improvements

## Issue Summary
The Hall Management Dashboard (`/admin/hall-management`) requires several UX/UI improvements to enhance usability, especially in dark mode, and to modernize the overall appearance.

## Priority: High
**Affects:** Admin users managing hall rentals and club events
**Impact:** User experience, accessibility, functionality

---

## Issues to Fix

### 1. 🌙 Dark Mode Visibility Issues
**Current Problem:**
- In dark mode, text and UI elements are difficult to see
- Insufficient contrast between background and foreground elements
- Calendar events blend into the background
- Stat cards are barely visible

**Expected Behavior:**
- All text should be clearly readable in dark mode
- Proper contrast ratios for accessibility (WCAG AA minimum)
- Calendar events should stand out
- Stat cards should be clearly visible

**Technical Details:**
- File: `HallManagementDashboard.razor.css`
- Need to add dark mode CSS variables and selectors
- Use `@media (prefers-color-scheme: dark)` or check for dark mode class

**Suggested Solution:**
```css
@media (prefers-color-scheme: dark) {
    .hall-management-dashboard {
        background: linear-gradient(135deg, #1a1a1a 0%, #2d2d2d 100%);
    }
    
    .section-card {
        background: #2d2d2d;
        border: 1px solid #404040;
    }
    
    .calendar-day {
        background: #1f1f1f;
        color: #e0e0e0;
    }
    
    .event-badge {
        /* Increase opacity and brightness for dark mode */
    }
}
```

---

### 2. 🖱️ Hover State Visibility
**Current Problem:**
- When hovering over calendar dates, certain elements are difficult to see
- Hover effects don't provide enough visual feedback
- Hard to distinguish hovered state from normal state

**Expected Behavior:**
- Clear visual feedback when hovering over interactive elements
- Sufficient contrast change to indicate interactivity
- Smooth transitions

**Technical Details:**
- File: `HallManagementDashboard.razor.css`
- Lines: `.calendar-day:hover`, `.event-badge:hover`, etc.

**Suggested Solution:**
```css
.calendar-day:hover:not(.empty-day) {
    background: rgba(59, 130, 246, 0.1);
    box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.3);
    transform: scale(1.05);
    z-index: 10;
}

@media (prefers-color-scheme: dark) {
    .calendar-day:hover:not(.empty-day) {
        background: rgba(59, 130, 246, 0.2);
        box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.5);
    }
}
```

---

### 3. 📜 Scrolling Issues in Sidebar
**Current Problem:**
- When scrolling the "Upcoming Events" list in the sidebar, the entire page scrolls instead of just the list
- Makes it difficult to view all events
- Poor UX for users with many events

**Expected Behavior:**
- Only the events list should scroll
- Page body should remain fixed
- Smooth scrolling within the container

**Technical Details:**
- File: `HallManagementDashboard.razor.css`
- Classes: `.requests-list`, `.events-list`

**Suggested Solution:**
```css
.requests-list,
.events-list {
    max-height: 400px;
    overflow-y: auto;
    overflow-x: hidden;
    overscroll-behavior: contain; /* Prevent scroll chaining */
}

/* Better scrollbar styling */
.requests-list::-webkit-scrollbar,
.events-list::-webkit-scrollbar {
    width: 8px;
}

.requests-list::-webkit-scrollbar-track,
.events-list::-webkit-scrollbar-track {
    background: var(--gray-100);
    border-radius: 4px;
}

.requests-list::-webkit-scrollbar-thumb,
.events-list::-webkit-scrollbar-thumb {
    background: var(--gray-400);
    border-radius: 4px;
}

.requests-list::-webkit-scrollbar-thumb:hover,
.events-list::-webkit-scrollbar-thumb:hover {
    background: var(--gray-500);
}
```

---

### 4. 🎨 Modernize Rental Request Details Modal
**Current Problem:**
- Modal looks plain and outdated
- Close button is just text, not visually appealing
- Layout is basic with no visual hierarchy
- Lacks modern design elements

**Expected Behavior:**
- Modern, visually appealing modal design
- Pill-style close button or icon button
- Better use of gradients, shadows, and spacing
- Clear visual hierarchy

**Technical Details:**
- File: `HallManagementDashboard.razor`
- Section: Request Details Modal (around line 388-500)
- File: `HallManagementDashboard.razor.css`
- Classes: `.modal-container`, `.modal-header`, `.modal-footer`

**Suggested Changes:**

**Razor Markup:**
```razor
<div class="modal-header">
    <div class="modal-title-section">
        <i class="bi bi-file-text-fill modal-icon"></i>
        <h3>Rental Request Details</h3>
    </div>
    <button class="btn-close-pill" @onclick="() => CloseRequestModal()">
        <i class="bi bi-x-lg"></i>
    </button>
</div>
```

**CSS:**
```css
.modal-header {
    background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-light) 100%);
    color: white;
    padding: 1.5rem;
    border-radius: 1rem 1rem 0 0;
}

.modal-title-section {
    display: flex;
    align-items: center;
    gap: 0.75rem;
}

.modal-icon {
    font-size: 1.5rem;
    opacity: 0.9;
}

.btn-close-pill {
    width: 2.5rem;
    height: 2.5rem;
    border-radius: 9999px;
    background: rgba(255, 255, 255, 0.2);
    border: none;
    color: white;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    transition: all 0.2s ease;
}

.btn-close-pill:hover {
    background: rgba(255, 255, 255, 0.3);
    transform: scale(1.1);
}

.detail-card {
    background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
    padding: 1.5rem;
    border-radius: 0.75rem;
    border-left: 4px solid var(--primary-color);
}

.modal-footer {
    background: var(--gray-50);
    padding: 1.5rem;
    border-radius: 0 0 1rem 1rem;
    display: flex;
    justify-content: flex-end;
    gap: 0.75rem;
}
```

---

### 5. 🎨 Modernize Add Club Event Modal
**Current Problem:**
- Form looks plain and basic
- No visual interest or modern design elements
- Labels are plain text
- No icons or visual cues

**Expected Behavior:**
- Modern form design with icons
- Better input styling
- Visual feedback for required fields
- Gradient header like request details

**Technical Details:**
- File: `HallManagementDashboard.razor`
- Section: Event Details Modal (around line 347-386)

**Suggested Changes:**

**Razor Markup:**
```razor
<div class="modal-header">
    <div class="modal-title-section">
        <i class="bi bi-calendar-plus-fill modal-icon"></i>
        <h3>@(_editingEvent != null ? "Edit Club Event" : "Add Club Event")</h3>
    </div>
    <button class="btn-close-pill" @onclick="() => CloseEventModal()">
        <i class="bi bi-x-lg"></i>
    </button>
</div>
<div class="modal-body">
    <div class="form-group-modern">
        <label>
            <i class="bi bi-tag-fill"></i>
            Event Name
        </label>
        <input type="text" class="form-input-modern" @bind="_eventForm.Description" placeholder="e.g., Christmas Party" />
    </div>
    <div class="form-group-modern">
        <label>
            <i class="bi bi-calendar3"></i>
            Date
        </label>
        <input type="date" class="form-input-modern" @bind="_eventForm.Date" />
    </div>
    <div class="form-row">
        <div class="form-group-modern">
            <label>
                <i class="bi bi-clock"></i>
                Start Time
            </label>
            <input type="text" class="form-input-modern" @bind="_eventForm.StartTime" placeholder="e.g., 3:00 PM" />
        </div>
        <div class="form-group-modern">
            <label>
                <i class="bi bi-clock-fill"></i>
                End Time
            </label>
            <input type="text" class="form-input-modern" @bind="_eventForm.EndTime" placeholder="e.g., 11:00 PM" />
        </div>
    </div>
</div>
```

**CSS:**
```css
.form-group-modern {
    margin-bottom: 1.5rem;
}

.form-group-modern label {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    font-weight: 600;
    font-size: 0.875rem;
    color: var(--gray-700);
    margin-bottom: 0.5rem;
}

.form-group-modern label i {
    color: var(--primary-color);
}

.form-input-modern {
    width: 100%;
    padding: 0.75rem 1rem;
    border: 2px solid var(--gray-200);
    border-radius: 0.5rem;
    font-size: 0.95rem;
    transition: all 0.2s ease;
}

.form-input-modern:focus {
    outline: none;
    border-color: var(--primary-color);
    box-shadow: 0 0 0 3px rgba(26, 42, 68, 0.1);
}
```

---

### 6. 🚫 Prevent Multiple Events Per Day
**Current Problem:**
- System allows adding multiple club events on the same day
- Can cause conflicts and confusion
- No validation before saving

**Expected Behavior:**
- Check if an event already exists for the selected date
- Show error message if date is already taken
- Prevent form submission

**Technical Details:**
- File: `HallManagementDashboard.razor`
- Method: `SaveClubEvent()`

**Suggested Implementation:**
```csharp
private async Task SaveClubEvent()
{
    try
    {
        // Check if event already exists for this date
        var existingEvent = _clubEvents.FirstOrDefault(e => e.Date.Date == _eventForm.Date.Date);
        
        if (existingEvent != null && (_editingEvent == null || existingEvent.Date != _editingEvent.Date))
        {
            await ToastService.ShowToastAsync(
                $"A club event already exists on {_eventForm.Date.ToShortDateString()}. Please choose a different date or edit the existing event.",
                ToastLevel.Warning
            );
            return;
        }
        
        await RentalService.AddBlackoutDateAsync(_eventForm.Date, _eventForm.Description, _eventForm.StartTime, _eventForm.EndTime);
        await ToastService.ShowToastAsync("Club event saved successfully", ToastLevel.Success);
        CloseEventModal();
        await LoadData();
    }
    catch (Exception ex)
    {
        await ToastService.ShowToastAsync($"Error saving event: {ex.Message}", ToastLevel.Error);
    }
}
```

---

### 7. 🖱️ Click on Calendar Day to Show Details
**Current Problem:**
- Currently, you must click on the specific event badge within a day
- Clicking on the day itself doesn't show the event details
- Makes it harder to view events, especially on mobile

**Expected Behavior:**
- Clicking anywhere on a calendar day should show all events for that day
- If only one event, show its details directly
- If multiple events, show a list to choose from

**Technical Details:**
- File: `HallManagementDashboard.razor`
- Section: Calendar Days rendering (around line 115-153)

**Current Code:**
```razor
<div class="calendar-day" @onclick="() => SelectDay(currentDate)">
```

**Issue:** `SelectDay` opens day modal, but doesn't show event details

**Suggested Fix:**
```csharp
private void SelectDay(DateTime date)
{
    var dayEvents = GetEventsForDay(date);
    
    if (dayEvents.Count == 0)
    {
        // Show empty day modal with option to add event
        _selectedDay = date;
    }
    else if (dayEvents.Count == 1)
    {
        // Show details for the single event
        var evt = dayEvents.First();
        if (evt.Request != null)
        {
            ViewRequestDetails(evt.Request);
        }
        else
        {
            _selectedDay = date; // Show day modal for club events
        }
    }
    else
    {
        // Show day modal with all events
        _selectedDay = date;
    }
}
```

---

### 8. 📊 Make Stat Cards Clickable
**Current Problem:**
- Stat cards (Pending Requests, Approved Rentals, Club Events, Total Bookings) are not interactive
- Users cannot click them to see filtered lists
- Missed opportunity for quick navigation

**Expected Behavior:**
- Clicking on "Pending Requests" shows all pending requests
- Clicking on "Approved Rentals" shows all approved rentals
- Clicking on "Club Events" shows all club events
- Clicking on "Total Bookings" shows all bookings
- Opens a modal or filters the main view

**Technical Details:**
- File: `HallManagementDashboard.razor`
- Section: Stats Grid (around line 36-73)

**Suggested Implementation:**

**Razor Markup:**
```razor
<div class="stat-card stat-primary clickable" @onclick="() => ShowFilteredList(FilterType.Pending)">
    <div class="stat-icon">
        <i class="bi bi-calendar-check"></i>
    </div>
    <div class="stat-content">
        <div class="stat-label">Pending Requests</div>
        <div class="stat-value">@_pendingCount</div>
    </div>
    <i class="bi bi-chevron-right stat-arrow"></i>
</div>
```

**CSS:**
```css
.stat-card.clickable {
    cursor: pointer;
    position: relative;
}

.stat-card.clickable:hover {
    transform: translateY(-4px);
    box-shadow: var(--shadow-lg);
}

.stat-arrow {
    position: absolute;
    right: 1rem;
    top: 50%;
    transform: translateY(-50%);
    opacity: 0;
    transition: all 0.2s ease;
}

.stat-card.clickable:hover .stat-arrow {
    opacity: 0.5;
    transform: translateY(-50%) translateX(4px);
}
```

**Code:**
```csharp
private enum FilterType { Pending, Approved, ClubEvents, All }
private FilterType? _activeFilter = null;
private bool _showFilteredModal = false;

private void ShowFilteredList(FilterType filter)
{
    _activeFilter = filter;
    _showFilteredModal = true;
}

// Add modal to display filtered list
```

---

## Additional Notes

### About the CD Command Issue
The `cd apps/website` command going to the studio directory is a **Windows terminal path resolution issue**, not related to the dashboard code. This happens because:
- Windows PowerShell/CMD may have cached paths
- There might be a symlink or junction point
- The current directory context is different

**Solution:** This is outside the scope of this dashboard issue. User should:
1. Use absolute paths: `cd "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\website"`
2. Or navigate from root: `cd \` then `cd` to the correct path
3. Check for symlinks: `dir /AL` in the apps directory

---

## Testing Checklist

After implementing fixes, verify:
- [ ] Dashboard displays correctly in light mode
- [ ] Dashboard displays correctly in dark mode
- [ ] All text is readable in both modes
- [ ] Hover states are clearly visible
- [ ] Sidebar lists scroll independently
- [ ] Request details modal looks modern
- [ ] Add club event modal looks modern
- [ ] Cannot add multiple events on same day
- [ ] Clicking calendar day shows event details
- [ ] Stat cards are clickable and show filtered lists
- [ ] All modals have pill-style close buttons
- [ ] Forms have icons and modern styling
- [ ] Responsive design works on mobile

---

## Files to Modify

1. **HallManagementDashboard.razor** - Component logic and markup
2. **HallManagementDashboard.razor.css** - Styling and dark mode

---

## Acceptance Criteria

- [ ] All issues listed above are resolved
- [ ] Code follows existing project conventions
- [ ] No breaking changes to existing functionality
- [ ] Tested in both light and dark modes
- [ ] Tested on desktop and mobile viewports
- [ ] No console errors
- [ ] Smooth animations and transitions
- [ ] Accessible (keyboard navigation, screen readers)

---

## Screenshots

### Current Issues
See uploaded screenshots showing:
1. Dark mode visibility problems
2. Plain modal design
3. Hover state issues

### Expected Result
- Modern, visually appealing dashboard
- Clear visibility in all modes
- Smooth, intuitive interactions
- Professional appearance

---

## Priority & Effort Estimate

**Priority:** High
**Effort:** Medium (4-6 hours)
**Complexity:** Medium

**Breakdown:**
- Dark mode CSS: 1 hour
- Modal redesign: 1.5 hours
- Scrolling fix: 0.5 hours
- Validation logic: 1 hour
- Clickable stats: 1.5 hours
- Testing: 1 hour

---

## Related Issues
- None currently

## Labels
- `enhancement`
- `ui/ux`
- `accessibility`
- `admin-dashboard`
- `high-priority`
