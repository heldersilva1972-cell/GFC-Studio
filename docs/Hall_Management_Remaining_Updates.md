# Hall Management Dashboard - Remaining Razor Component Updates

## STATUS: CSS Complete ✅ | Razor Component: In Progress

The CSS file has been fully updated with:
- ✅ Dark mode support
- ✅ Better hover states  
- ✅ Fixed scrolling
- ✅ Modern modal styles
- ✅ Modern form styles
- ✅ Clickable stat card styles

## Remaining Razor Component Updates Needed:

### 1. Make Stat Cards Clickable (Lines 36-73)

**FIND:**
```razor
<div class="stat-card stat-primary">
```

**REPLACE WITH:**
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

**Repeat for all 4 stat cards with appropriate FilterType:**
- Pending Requests: `FilterType.Pending`
- Approved Rentals: `FilterType.Approved`
- Club Events: `FilterType.ClubEvents`
- Total Bookings: `FilterType.All`

### 2. Modernize Request Details Modal (Lines ~388-500)

**FIND the modal header:**
```razor
<div class="modal-header">
    <h3>Rental Request Details</h3>
    <button class="btn-close" @onclick="() => CloseRequestModal()">
        <i class="bi bi-x-lg"></i>
    </button>
</div>
```

**REPLACE WITH:**
```razor
<div class="modal-container modal-large modal-modern" @onclick:stopPropagation="true">
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

### 3. Modernize Add Club Event Modal (Lines ~347-386)

**FIND:**
```razor
<div class="modal-header">
    <h3>@(_editingEvent != null ? "Edit Club Event" : "Add Club Event")</h3>
    <button class="btn-close" @onclick="() => CloseEventModal()">
```

**REPLACE WITH:**
```razor
<div class="modal-container modal-medium modal-modern" @onclick:stopPropagation="true">
    <div class="modal-header">
        <div class="modal-title-section">
            <i class="bi bi-calendar-plus-fill modal-icon"></i>
            <h3>@(_editingEvent != null ? "Edit Club Event" : "Add Club Event")</h3>
        </div>
        <button class="btn-close-pill" @onclick="() => CloseEventModal()">
            <i class="bi bi-x-lg"></i>
        </button>
    </div>
```

**ALSO UPDATE FORM FIELDS:**
```razor
<div class="form-group-modern">
    <label>
        <i class="bi bi-tag-fill"></i>
        Event Name
    </label>
    <input type="text" class="form-input-modern" @bind="_eventForm.Description" placeholder="e.g., Christmas Party" />
</div>
```

### 4. Add Validation to Prevent Multiple Events Per Day

**FIND the SaveClubEvent method in @code section:**
```csharp
private async Task SaveClubEvent()
{
    try
    {
        await RentalService.AddBlackoutDateAsync(...);
```

**REPLACE WITH:**
```csharp
private async Task SaveClubEvent()
{
    try
    {
        // Validate event name
        if (string.IsNullOrWhiteSpace(_eventForm.Description))
        {
            await ToastService.ShowToastAsync("Please enter an event name", ToastLevel.Warning);
            return;
        }
        
        // Check for existing event on same date
        var existingEvent = _clubEvents.FirstOrDefault(e => e.Date.Date == _eventForm.Date.Date);
        
        if (existingEvent != null && (_editingEvent == null || existingEvent.Date != _editingEvent.Date))
        {
            await ToastService.ShowToastAsync(
                $"A club event '{existingEvent.Description}' already exists on {_eventForm.Date.ToString("MMMM d, yyyy")}. Please choose a different date or edit the existing event.",
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

### 5. Improve SelectDay to Show Details Directly

**FIND:**
```csharp
private void SelectDay(DateTime date)
{
    _selectedDay = date;
}
```

**REPLACE WITH:**
```csharp
private void SelectDay(DateTime date)
{
    var dayEvents = GetEventsForDay(date);
    
    if (dayEvents.Count == 0)
    {
        _selectedDay = date;
    }
    else if (dayEvents.Count == 1 && (dayEvents[0].Type == EventType.RentalApproved || dayEvents[0].Type == EventType.RentalPending))
    {
        ViewRequestDetails(dayEvents[0].Request);
    }
    else
    {
        _selectedDay = date;
    }
}
```

### 6. Add Clickable Stats Functionality

**ADD TO @code SECTION (after existing variables):**
```csharp
// Filtered list modal
private enum FilterType { Pending, Approved, ClubEvents, All }
private FilterType? _activeFilter = null;
private bool _showFilteredModal = false;

private void ShowFilteredList(FilterType filter)
{
    _activeFilter = filter;
    _showFilteredModal = true;
}

private string GetFilterTitle()
{
    return _activeFilter switch
    {
        FilterType.Pending => "Pending Requests",
        FilterType.Approved => "Approved Rentals",
        FilterType.ClubEvents => "Club Events",
        FilterType.All => "All Bookings",
        _ => "Events"
    };
}

private List<object> GetFilteredItems()
{
    return _activeFilter switch
    {
        FilterType.Pending => _pendingRequests.Cast<object>().ToList(),
        FilterType.Approved => _allRequests.Where(r => r.Status == RentalStatus.Approved).Cast<object>().ToList(),
        FilterType.ClubEvents => _clubEvents.Cast<object>().ToList(),
        FilterType.All => _upcomingEvents.Cast<object>().ToList(),
        _ => new List<object>()
    };
}

private void SelectFilteredItem(object item)
{
    if (item is HallRentalRequest request)
    {
        ViewRequestDetails(request);
        _showFilteredModal = false;
    }
    else if (item is AvailabilityCalendar clubEvent)
    {
        _selectedDay = clubEvent.Date;
        _showFilteredModal = false;
    }
}
```

**ADD FILTERED LIST MODAL (after existing modals, before @code):**
```razor
<!-- Filtered List Modal -->
@if (_showFilteredModal)
{
    <div class="modal-overlay" @onclick="() => _showFilteredModal = false">
        <div class="modal-container modal-medium modal-modern" @onclick:stopPropagation="true">
            <div class="modal-header">
                <div class="modal-title-section">
                    <i class="bi bi-funnel-fill modal-icon"></i>
                    <h3>@GetFilterTitle()</h3>
                </div>
                <button class="btn-close-pill" @onclick="() => _showFilteredModal = false">
                    <i class="bi bi-x-lg"></i>
                </button>
            </div>
            <div class="modal-body">
                <div class="filtered-list">
                    @foreach (var item in GetFilteredItems())
                    {
                        @if (item is HallRentalRequest request)
                        {
                            <div class="filtered-item" @onclick="() => SelectFilteredItem(item)">
                                <div class="request-header">
                                    <span class="request-name">@request.ApplicantName</span>
                                    <span class="request-date">@request.RequestedDate.ToString("MMM dd")</span>
                                </div>
                                <div class="request-details">
                                    <span class="request-type">@(request.EventType ?? "General Rental")</span>
                                    @if (!string.IsNullOrEmpty(request.StartTime))
                                    {
                                        <span class="request-time">@request.StartTime - @request.EndTime</span>
                                    }
                                </div>
                            </div>
                        }
                        else if (item is AvailabilityCalendar clubEvent)
                        {
                            <div class="filtered-item" @onclick="() => SelectFilteredItem(item)">
                                <div class="request-header">
                                    <span class="request-name">@clubEvent.Description</span>
                                    <span class="request-date">@clubEvent.Date.ToString("MMM dd")</span>
                                </div>
                                @if (!string.IsNullOrEmpty(clubEvent.StartTime))
                                {
                                    <div class="request-details">
                                        <span class="request-time">@clubEvent.StartTime - @clubEvent.EndTime</span>
                                    </div>
                                }
                            </div>
                        }
                    }
                </div>
            </div>
        </div>
    </div>
}
```

## Summary of Changes:

1. ✅ CSS: Dark mode, hover states, scrolling - COMPLETE
2. ⏳ Razor: Stat cards clickable - CODE PROVIDED ABOVE
3. ⏳ Razor: Modern modals - CODE PROVIDED ABOVE
4. ⏳ Razor: Validation - CODE PROVIDED ABOVE
5. ⏳ Razor: Improved day click - CODE PROVIDED ABOVE
6. ⏳ Razor: Filtered list modal - CODE PROVIDED ABOVE

## Next Steps:

The CSS changes are complete and will take effect immediately after browser refresh.

For the Razor component changes, I can either:
A) Apply them all now (will take 5-10 minutes)
B) Provide you with a complete updated file
C) Apply them one at a time so you can test each

Which would you prefer?
