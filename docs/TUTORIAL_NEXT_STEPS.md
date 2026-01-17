# Tutorial - Remaining Steps to Implement

## ✅ What's Working Now
- Tutorial start card appears on Dashboard
- Step 1 bubble shows welcome message
- Step 2 bubble shows "Navigate to Membership" message
- Clicking "Next" advances steps
- Clicking "Cancel" cancels tutorial
- State management works

## ❌ What Needs to be Added

### Step 3-4: Members Page (AddMember.razor)
**Location**: `/members` page

Add to `AddMember.razor`:
```razor
@inject TutorialService TutorialService

@* Step 3: Click Add Member button *@
@if (TutorialService.ShouldShowStep(3))
{
    <TutorialBubble 
        Show="true"
        Title="Add a New Member"
        Message="Click the 'Add Member' button to open the form where you'll enter the new member's information."
        CurrentStep="3"
        TotalSteps="10"
        ShowNext="false"
        ShowWait="true"
        WaitingText="Waiting for you to click Add Member..."
        AllowCancel="true"
        OnCancel="@(async () => await TutorialService.CancelTutorialAsync())" />
}

@* Step 4: Fill out form *@
@if (TutorialService.ShouldShowStep(4))
{
    <TutorialBubble 
        Show="true"
        Title="Fill Out Member Information"
        Message="Enter the member's details in the form. Fill in at least the required fields (First Name, Last Name, Status). When done, click Save."
        Tip="In sample mode, we've pre-filled some data for you!"
        CurrentStep="4"
        TotalSteps="10"
        ShowNext="false"
        ShowWait="true"
        WaitingText="Waiting for you to save the member..."
        AllowCancel="true"
        OnCancel="@(async () => await TutorialService.CancelTutorialAsync())" />
}
```

**In the code section**, add:
```csharp
protected override void OnInitialized()
{
    TutorialService.OnTutorialStateChanged += HandleTutorialStateChanged;
    
    // Auto-advance from Step 2 to Step 3 when page loads
    if (TutorialService.ShouldShowStep(2))
    {
        _ = TutorialService.NextStepAsync();
    }
}

private void HandleTutorialStateChanged()
{
    InvokeAsync(StateHasChanged);
}

// In the Save button click handler, add:
private async Task SaveMember()
{
    // ... existing save logic ...
    
    // Auto-advance to Step 5 after saving
    if (TutorialService.ShouldShowStep(4))
    {
        await TutorialService.GoToStepAsync(5);
        NavManager.NavigateTo("/dues");
    }
}
```

### Step 5-7: Dues Page (Dues.razor)
**Location**: `/dues` page

Add similar bubbles for:
- Step 5: Navigate to Dues (auto-advance from AddMember)
- Step 6: Find the member you just added
- Step 7: Record a payment

### Step 8-10: Key Cards Page (KeyCards.razor)
**Location**: `/keycards` page

Add bubbles for:
- Step 8: Navigate to Key Cards
- Step 9: Click "Needing Cards" filter
- Step 10: Assign and activate card, then show success screen

## Quick Implementation Guide

1. **Inject TutorialService** in each page
2. **Subscribe to state changes** in OnInitialized
3. **Add TutorialBubble components** for each step
4. **Auto-advance** when user completes actions
5. **Navigate** to next page when appropriate

## Files to Edit
- ✅ `Dashboard.razor` - Steps 1-2 (DONE)
- ❌ `AddMember.razor` - Steps 3-4
- ❌ `Dues.razor` - Steps 5-7
- ❌ `KeyCards.razor` - Steps 8-10
- ❌ `NavMenu.razor` - Add pulsing highlight (optional)

## Current Status
**Phase 1 Complete**: Tutorial infrastructure works!
**Phase 2 Needed**: Wire up remaining steps 3-10

The foundation is solid. Just need to copy the pattern from Steps 1-2 to the other pages.
