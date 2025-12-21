# Card Reader Emulator Component

## Overview
A dedicated tile component for simulating card swipes in the Simulation Control Panel.

## Features
✅ **Scan-Once-Then-Clear** - Card locks after scanning, requires manual clear
✅ **Controller & Door Selection** - Choose target controller and door
✅ **Physical Card Reader Support** - Works with USB keyboard-wedge readers
✅ **Manual Entry** - Can also type card numbers manually
✅ **Visual Feedback** - Shows locked state, success messages, and status
✅ **Auto-Clear** - Automatically clears card after successful swipe
✅ **NEW Badge** - Highlights this is a new feature

## Component: CardReaderEmulator.razor

### Parameters:
```csharp
[Parameter] public List<ControllerDevice> Controllers { get; set; }
[Parameter] public bool IsProcessing { get; set; }
[Parameter] public EventCallback<CardSwipeRequest> OnCardSwipe { get; set; }
```

### CardSwipeRequest DTO:
```csharp
public class CardSwipeRequest
{
    public int ControllerId { get; set; }
    public int DoorIndex { get; set; }
    public string CardNumber { get; set; }
}
```

## How It Works

### 1. **Card Scanning Flow**
```
User scans card (or types + presses Enter)
    ↓
Card number locks in field
    ↓
"LOCKED" badge appears
    ↓
"Swipe Card" button becomes enabled
    ↓
User clicks "Swipe Card"
    ↓
Event fires to parent component
    ↓
Card auto-clears after 1.5 seconds
```

### 2. **Clear Card Flow**
```
Card is locked
    ↓
User clicks "Clear" button
    ↓
Card number clears
    ↓
Field becomes editable again
    ↓
Ready for next scan
```

## Integration Example

### Add to Dashboard.razor:
```razor
<div class="row g-3">
    <!-- Left Column -->
    <div class="col-lg-5">
        <SimulationStatusTile ... />
        <SimulationControlBench ... />
        
        <!-- NEW: Card Reader Emulator -->
        <CardReaderEmulator 
            Controllers="@_controllers"
            IsProcessing="@_isProcessing"
            OnCardSwipe="HandleCardSwipe" />
    </div>
    
    <!-- Right Column -->
    <div class="col-lg-7">
        <ControllerVisualizer ... />
        <SimulationTraceLog ... />
    </div>
</div>
```

### Add Event Handler:
```csharp
private async Task HandleCardSwipe(CardReaderEmulator.CardSwipeRequest request)
{
    _isProcessing = true;
    
    try
    {
        // Simulate card swipe event
        await SimulationService.InjectCardSwipeAsync(
            request.ControllerId, 
            request.DoorIndex, 
            request.CardNumber
        );
        
        _successMessage = $"Card {request.CardNumber} swiped successfully";
        
        // Refresh controller state
        await LoadControllerStateAsync(request.ControllerId);
    }
    catch (Exception ex)
    {
        _errorMessage = $"Failed to swipe card: {ex.Message}";
    }
    finally
    {
        _isProcessing = false;
    }
}
```

## Visual Design

### States:
1. **Ready State** (Default)
   - Input enabled
   - Placeholder: "Scan card or type number..."
   - No badges

2. **Locked State** (After scan)
   - Input disabled
   - Green "LOCKED" badge
   - "Clear" button visible
   - Success message: "Card scanned! Click 'Swipe Card' to simulate access."

3. **Processing State**
   - All buttons disabled
   - Shows processing status

### Colors:
- **Success**: Green badges and messages
- **Warning**: Yellow "Clear" button
- **Info**: Blue status messages
- **Danger**: Red error messages

## Benefits

### For Testing:
- Quickly test card access scenarios
- No need to navigate to separate test page
- Works with real USB card readers
- Can also type card numbers manually

### For Simulation:
- Integrated into simulation workflow
- Automatically triggers simulation events
- Visual feedback for all actions
- Auto-clears for rapid testing

### For UX:
- Clear visual states
- Intuitive workflow
- Prevents accidental re-scans
- Helpful status messages

## Usage Tips

### With Physical Card Reader:
1. Select controller and door
2. Focus on card number field
3. Tap card on USB reader
4. Card number auto-fills and locks
5. Click "Swipe Card"
6. Card auto-clears after success

### Manual Entry:
1. Select controller and door
2. Type card number
3. Press Enter to lock
4. Click "Swipe Card"
5. Card auto-clears after success

### Quick Testing:
1. Leave controller/door selected
2. Scan multiple cards in sequence
3. Each swipe auto-clears
4. No need to manually clear between scans

## Future Enhancements

- [ ] Card number validation
- [ ] Recent cards dropdown
- [ ] Batch swipe mode (multiple cards)
- [ ] Swipe history
- [ ] Card number lookup from database
- [ ] QR code support
- [ ] NFC emulation

---

**File Location:** `Components/Pages/Simulation/CardReaderEmulator.razor`  
**Status:** ✅ Complete and ready to integrate
