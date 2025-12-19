# Phase 1 Complete: Simulation Dashboard Modularization

## ✅ Components Created

### 1. **SimulationStatusTile.razor**
**Purpose:** Display system mode, simulation clock, and controller connection status

**Features:**
- Pulsing LED indicator (green for sim, red for live)
- Real-time simulation clock display with offset
- Controller online status
- Automatic mode detection

**Parameters:**
```csharp
[Parameter] public DateTime? SimulationTime { get; set; }
[Parameter] public TimeSpan? TimeOffset { get; set; }
[Parameter] public bool ControllerOnline { get; set; }
[Parameter] public int ControllerCount { get; set; }
```

---

### 2. **SimulationControlBench.razor**
**Purpose:** Unified interface for time control and event injection

**Features:**
- Time advancement buttons (+10m, +1h, +6h, +1d)
- Sync to real-time button
- Event injection form (controller, door, event type, card ID)
- Disabled state when processing

**Parameters:**
```csharp
[Parameter] public DateTime CurrentTime { get; set; }
[Parameter] public List<ControllerDevice> Controllers { get; set; }
[Parameter] public bool IsProcessing { get; set; }
[Parameter] public EventCallback<TimeSpan> OnAdvanceTime { get; set; }
[Parameter] public EventCallback OnSyncToNow { get; set; }
[Parameter] public EventCallback<EventInjectionRequest> OnInjectEvent { get; set; }
```

---

### 3. **ControllerVisualizer.razor**
**Purpose:** Visual "LED" style display of door states

**Features:**
- Grid layout of door tiles
- Color-coded LED indicators (green=unlocked, red=locked, yellow=open, gray=closed)
- Hover effects and visual feedback
- Reset controller button

**Parameters:**
```csharp
[Parameter] public List<ControllerDevice> Controllers { get; set; }
[Parameter] public List<DoorStateDto> DoorStates { get; set; }
[Parameter] public int SelectedControllerId { get; set; }
[Parameter] public EventCallback<int> SelectedControllerIdChanged { get; set; }
[Parameter] public EventCallback OnResetController { get; set; }
[Parameter] public bool IsProcessing { get; set; }
```

---

### 4. **SimulationTraceLog.razor**
**Purpose:** Display recent simulation traces with auto-refresh

**Features:**
- Auto-refresh toggle
- Color-coded status badges
- Scrollable table (max 400px height)
- "View Details" button for each trace
- Last updated timestamp

**Parameters:**
```csharp
[Parameter] public List<TraceDto> Traces { get; set; }
[Parameter] public bool AutoRefresh { get; set; }
[Parameter] public EventCallback<bool> AutoRefreshChanged { get; set; }
[Parameter] public EventCallback OnRefresh { get; set; }
[Parameter] public EventCallback<TraceDto> OnViewDetails { get; set; }
[Parameter] public bool IsLoading { get; set; }
[Parameter] public bool IsRefreshing { get; set; }
```

---

## 🔧 Integration Steps

### Step 1: Update Dashboard.razor
Replace the existing inline sections with the new components:

```razor
<!-- Replace the Time Control card with: -->
<SimulationControlBench 
    CurrentTime="@_currentSimTime"
    Controllers="@_controllers"
    IsProcessing="@_isProcessing"
    OnAdvanceTime="AdvanceTime"
    OnSyncToNow="AdvanceTimeToNow"
    OnInjectEvent="HandleEventInjection" />

<!-- Replace the Controller State card with: -->
<ControllerVisualizer 
    Controllers="@_controllers"
    DoorStates="@_doorStates"
    SelectedControllerId="@_selectedControllerId"
    SelectedControllerIdChanged="OnControllerSelectionChanged"
    OnResetController="ResetControllerState"
    IsProcessing="@_isProcessing" />

<!-- Replace the Recent Traces section with: -->
<SimulationTraceLog 
    Traces="@_traces"
    AutoRefresh="@_autoRefresh"
    AutoRefreshChanged="OnAutoRefreshChanged"
    OnRefresh="LoadTracesAsync"
    OnViewDetails="ShowTraceDetails"
    IsLoading="@_isLoadingTraces"
    IsRefreshing="@_isRefreshingTraces" />
```

### Step 2: Add DTO Mapping
You'll need to map your existing data to the component DTOs:

```csharp
// For ControllerVisualizer
private List<ControllerVisualizer.DoorStateDto> _doorStates => 
    _selectedController?.Doors.Select(d => new ControllerVisualizer.DoorStateDto
    {
        DoorIndex = d.DoorIndex,
        DoorName = d.Name,
        IsLocked = /* get from StateStore */,
        IsDoorOpen = /* get from StateStore */
    }).ToList() ?? new();

// For SimulationTraceLog
private List<SimulationTraceLog.TraceDto> _traces => 
    _recentTraces.Select(t => new SimulationTraceLog.TraceDto
    {
        Id = t.Id,
        Timestamp = t.Timestamp,
        EventType = t.EventType,
        ControllerName = t.ControllerName,
        DoorIndex = t.DoorIndex,
        Status = t.Status,
        Details = t.Details
    }).ToList();
```

### Step 3: Add Event Handlers
Implement the new event callback handlers:

```csharp
private async Task HandleEventInjection(SimulationControlBench.EventInjectionRequest request)
{
    // Your existing event injection logic
    await InjectEventAsync(request.ControllerId, request.DoorIndex, request.EventType, request.CardId);
}

private async Task OnControllerSelectionChanged(int controllerId)
{
    _selectedControllerId = controllerId;
    await LoadControllerStateAsync(controllerId);
}

private async Task OnAutoRefreshChanged(bool enabled)
{
    _autoRefresh = enabled;
    if (enabled)
    {
        StartAutoRefreshTimer();
    }
    else
    {
        StopAutoRefreshTimer();
    }
}
```

---

## 📊 Benefits of Modularization

✅ **Maintainability:** Each component has a single, clear responsibility  
✅ **Reusability:** Components can be used in other simulation-related pages  
✅ **Testability:** Easier to unit test individual components  
✅ **Performance:** Smaller components = more efficient re-rendering  
✅ **Readability:** Dashboard.razor is now much cleaner and easier to understand  

---

## 🚀 Next Steps (Phase 2)

1. **Replay Timeline UI** - Build the scrub bar for watching past sessions
2. **Preset Logic** - Wire up "Busy Friday Night" to generate traffic
3. **Polish** - Standardize colors, improve JSON viewer, responsive fixes

---

## 📝 Notes

- All components use Bootstrap 5 classes for consistency
- LED indicators use CSS animations for the pulsing effect
- Components are designed to be self-contained with minimal dependencies
- Error boundaries are handled at the parent (Dashboard) level
