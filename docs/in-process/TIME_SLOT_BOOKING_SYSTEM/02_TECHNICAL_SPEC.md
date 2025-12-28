# Technical Specification - Time-Slot Booking System

## Database Changes

### AvailabilityCalendar Table
**New Fields:**
```sql
ALTER TABLE AvailabilityCalendar
ADD AllowSecondBooking BIT DEFAULT 0,
ADD BufferTimeMinutes INT NULL,
ADD IsSecondEvent BIT DEFAULT 0;

-- AllowSecondBooking: Flag to enable partial day availability
-- BufferTimeMinutes: Gap between events (required when AllowSecondBooking = true)
-- IsSecondEvent: Marks this as the second event on a multi-event day
```

**Existing Fields (Ensure Populated):**
- `StartTime` (varchar) - Required when AllowSecondBooking = true
- `EndTime` (varchar) - Required when AllowSecondBooking = true
- `Date` (datetime)
- `Description` (varchar)
- `EventType` (int) - 0=ClubEvent, 1=Blackout

### HallRentalRequest Table
**No changes needed** - Already has:
- `RequestedDate`
- `StartTime`
- `EndTime`
- `Status`

## C# Model Updates

### AvailabilityCalendar.cs
```csharp
public class AvailabilityCalendar
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public int EventType { get; set; } // 0=ClubEvent, 1=Blackout
    public bool Booked { get; set; }
    
    // NEW PROPERTIES
    public bool AllowSecondBooking { get; set; } = false;
    public int? BufferTimeMinutes { get; set; }
    public bool IsSecondEvent { get; set; } = false;
}
```

## Service Layer Changes

### IRentalService.cs - New Methods
```csharp
// Check if date allows second booking
Task<bool> CanAddSecondEventAsync(DateTime date);

// Get first event for a date
Task<AvailabilityCalendar?> GetFirstEventForDateAsync(DateTime date);

// Calculate available start time for second event
Task<TimeSpan?> GetAvailableStartTimeAsync(DateTime date);

// Add second event (club or enable public booking)
Task<bool> AddSecondEventAsync(DateTime date, int bufferMinutes, bool openToPublic, 
    string? eventName = null, string? startTime = null, string? endTime = null);

// Validate time slot availability
Task<bool> ValidateTimeSlotAsync(DateTime date, string startTime, string endTime);
```

### RentalService.cs - Implementation Logic

#### Time Overlap Detection
```csharp
private bool TimesOverlap(string start1, string end1, string start2, string end2)
{
    var s1 = TimeSpan.Parse(start1);
    var e1 = TimeSpan.Parse(end1);
    var s2 = TimeSpan.Parse(start2);
    var e2 = TimeSpan.Parse(end2);
    
    return s1 < e2 && s2 < e1; // Standard interval overlap check
}
```

#### Buffer Time Calculation
```csharp
private TimeSpan CalculateAvailableStartTime(string firstEventEndTime, int bufferMinutes)
{
    var endTime = TimeSpan.Parse(firstEventEndTime);
    return endTime.Add(TimeSpan.FromMinutes(bufferMinutes));
}
```

#### Event Count Check
```csharp
private async Task<int> GetEventCountForDateAsync(DateTime date)
{
    var clubEvents = await _context.AvailabilityCalendars
        .Where(e => e.Date.Date == date.Date)
        .CountAsync();
        
    var rentals = await _context.HallRentalRequests
        .Where(r => r.RequestedDate.Date == date.Date && 
                    r.Status == RentalStatus.Approved)
        .CountAsync();
        
    return clubEvents + rentals;
}
```

## Admin Dashboard Changes

### HallManagementDashboard.razor - New State Variables
```csharp
// Second event modal state
private bool _showSecondEventModal = false;
private AvailabilityCalendar? _firstEvent = null;
private int _bufferTimeMinutes = 60; // User input
private bool _openToPublic = true; // Toggle
private string _secondEventName = "";
private string _secondEventStartTime = "";
private string _secondEventEndTime = "";
```

### New Modal Component
```razor
<!-- Add Second Event Modal -->
@if (_showSecondEventModal && _firstEvent != null)
{
    <div class="modal-overlay">
        <div class="modal-container modal-medium">
            <div class="modal-header modal-header-gold">
                <h3>Add Second Event - @_firstEvent.Date.ToString("MMMM d, yyyy")</h3>
            </div>
            <div class="modal-body">
                <!-- First Event Summary -->
                <div class="alert alert-info">
                    <strong>First Event:</strong> @_firstEvent.Description<br/>
                    <strong>Time:</strong> @_firstEvent.StartTime - @_firstEvent.EndTime
                </div>
                
                <!-- Buffer Time Input -->
                <div class="form-group-modern">
                    <label>Buffer Time (minutes)</label>
                    <input type="number" @bind="_bufferTimeMinutes" min="0" max="480" />
                    <small>Time between events for cleanup/setup</small>
                </div>
                
                <!-- Available Time Display -->
                <div class="alert alert-success">
                    <strong>Available From:</strong> @CalculateAvailableTime()
                </div>
                
                <!-- Choice: Public or Club Event -->
                <div class="form-group-modern">
                    <label>
                        <input type="radio" @bind="_openToPublic" value="true" />
                        Open this time slot to public rentals
                    </label>
                    <label>
                        <input type="radio" @bind="_openToPublic" value="false" />
                        Add another club event now
                    </label>
                </div>
                
                <!-- If adding club event -->
                @if (!_openToPublic)
                {
                    <div class="form-group-modern">
                        <label>Event Name</label>
                        <input type="text" @bind="_secondEventName" />
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <label>Start Time</label>
                            <input type="text" @bind="_secondEventStartTime" />
                        </div>
                        <div class="col-6">
                            <label>End Time</label>
                            <input type="text" @bind="_secondEventEndTime" />
                        </div>
                    </div>
                }
            </div>
            <div class="modal-footer">
                <button class="btn-modern btn-outline" @onclick="CloseSecondEventModal">Cancel</button>
                <button class="btn-modern btn-gold" @onclick="SaveSecondEvent">Save</button>
            </div>
        </div>
    </div>
}
```

## Public Website Changes (Phase 2)

### Calendar Component (Next.js)
**File:** `apps/website/components/HallRentalCalendar.tsx`

#### New Day Status Enum
```typescript
enum DayStatus {
  Available = 'available',      // Green - fully open
  PartiallyAvailable = 'partial', // Yellow - second slot open
  FullyBooked = 'booked'         // Red - no slots
}
```

#### API Response Enhancement
```typescript
interface CalendarDay {
  date: string;
  status: DayStatus;
  availableFrom?: string; // e.g., "1:00 PM" if partial
  events: {
    startTime: string;
    endTime: string;
    isPublic: boolean; // false for club events
  }[];
}
```

#### Visual Indicator CSS
```css
.calendar-day.available {
  background: #dcfce7; /* Green-100 */
  border: 2px solid #22c55e; /* Green-500 */
}

.calendar-day.partial {
  background: #fef3c7; /* Yellow-100 */
  border: 2px solid #f59e0b; /* Yellow-500 */
}

.calendar-day.booked {
  background: #fee2e2; /* Red-100 */
  border: 2px solid #ef4444; /* Red-500 */
}
```

### Rental Form Validation
**File:** `apps/website/app/hall-rental/page.tsx`

#### Time Slot Enforcement
```typescript
const validateTimeSlot = (selectedDate: Date, startTime: string) => {
  const dayInfo = calendarData.find(d => d.date === selectedDate);
  
  if (dayInfo?.status === 'partial' && dayInfo.availableFrom) {
    const availableTime = parseTime(dayInfo.availableFrom);
    const requestedTime = parseTime(startTime);
    
    if (requestedTime < availableTime) {
      return {
        valid: false,
        message: `This date is only available from ${dayInfo.availableFrom} onwards`
      };
    }
  }
  
  return { valid: true };
};
```

## Validation Rules Summary

### When Adding Second Event
1. ✅ Check event count < 2
2. ✅ Verify first event has start/end times
3. ✅ Validate buffer time > 0
4. ✅ If adding club event, check time overlap
5. ✅ If opening to public, set AllowSecondBooking flag

### When Public User Books Partial Day
1. ✅ Verify date has AllowSecondBooking = true
2. ✅ Calculate minimum start time (first event end + buffer)
3. ✅ Validate requested start time >= minimum
4. ✅ Check no overlap with first event
5. ✅ Verify total event count < 2

### When Editing First Event
1. ✅ Check if second booking exists
2. ✅ If yes, show warning modal
3. ✅ Suggest recalculating available time
4. ✅ Require admin confirmation

## Error Messages

```csharp
public static class BookingErrors
{
    public const string MaxEventsReached = "This date already has the maximum of 2 events.";
    public const string TimeOverlap = "The selected time overlaps with an existing event.";
    public const string BufferRequired = "Buffer time must be specified for multi-event days.";
    public const string InvalidTimeSlot = "The requested time slot is not available.";
    public const string FirstEventMissingTimes = "First event must have start and end times to enable second booking.";
}
```

## Migration Script

```sql
-- Add new columns
ALTER TABLE AvailabilityCalendar
ADD AllowSecondBooking BIT DEFAULT 0;

ALTER TABLE AvailabilityCalendar
ADD BufferTimeMinutes INT NULL;

ALTER TABLE AvailabilityCalendar
ADD IsSecondEvent BIT DEFAULT 0;

-- Update existing records to ensure consistency
UPDATE AvailabilityCalendar
SET AllowSecondBooking = 0,
    IsSecondEvent = 0
WHERE AllowSecondBooking IS NULL;
```
