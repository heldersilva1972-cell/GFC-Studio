# Reimbursement Settings Page - How It Works

## Overview
The **Reimbursement Settings** page (`/reimbursements/settings`) is an admin-only configuration page that controls how the reimbursement system behaves and who gets notified when members submit reimbursement requests.

---

## What It Does

### 1. **Receipt Requirement Setting**
- **Toggle:** "Require receipts for all items before submission"
- **Purpose:** When enabled, members MUST upload receipt images for every line item before they can submit a reimbursement request
- **When disabled:** Members can submit requests without receipts (though they may still be required by policy)

### 2. **Notification Recipients**
- **Purpose:** Configures which members receive email notifications when ANY member submits a reimbursement request
- **Use Case:** Typically set to board members, treasurers, or finance committee members who need to review and approve reimbursement requests
- **Storage:** Member IDs are stored as a comma-separated string in the database

---

## How Members Are Selected

### **Current Implementation:**

#### **Member List Source:**
```csharp
// Line 96-99 in Settings.razor
_allMembers = MemberRepository.GetAllMembers()
    .OrderBy(m => m.LastName)
    .ThenBy(m => m.FirstName)
    .ToList();
```

**ALL members** from the database are loaded and displayed in the multi-select dropdown, sorted alphabetically by last name, then first name.

#### **Selection Process:**
1. **Admin opens the page** → All members are loaded into a multi-select dropdown
2. **Admin selects members** → Hold Ctrl/Cmd and click to select multiple members
3. **JavaScript helper** → Captures all selected values from the multi-select element
4. **Save** → Selected member IDs are joined into a comma-separated string and saved to database

#### **Data Flow:**
```
Database (NotificationRecipients)
    ↓
"123,456,789" (comma-separated member IDs)
    ↓
Split into List<string>
    ↓
Display in multi-select (pre-selected)
    ↓
Admin modifies selection
    ↓
JavaScript captures selected values
    ↓
Join back to "123,456,789"
    ↓
Save to database
```

---

## Technical Details

### **Database Schema:**
```sql
CREATE TABLE ReimbursementSettings (
    Id INT PRIMARY KEY,
    ReceiptRequired BIT NOT NULL,
    NotificationRecipients NVARCHAR(MAX) NULL  -- e.g., "123,456,789"
)
```

### **Key Code Sections:**

#### **Loading Settings (OnInitializedAsync):**
```csharp
// Load settings from database
_settings = await ReimbursementService.GetSettingsAsync();

// Parse comma-separated member IDs
_selectedMemberIds = string.IsNullOrWhiteSpace(_settings.NotificationRecipients)
    ? new List<string>()
    : _settings.NotificationRecipients
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim())
        .ToList();

// Load ALL members for dropdown
_allMembers = MemberRepository.GetAllMembers()
    .OrderBy(m => m.LastName)
    .ThenBy(m => m.FirstName)
    .ToList();
```

#### **Multi-Select UI:**
```razor
<select class="form-select" size="10" multiple @ref="_memberSelectElement" @onchange="OnMemberSelectionChanged">
    @foreach (var member in _allMembers)
    {
        <option value="@member.MemberID.ToString()" 
                selected="@_selectedMemberIds.Contains(member.MemberID.ToString())">
            @member.FirstName @member.LastName
        </option>
    }
</select>
```

#### **Capturing Selection (JavaScript):**
```csharp
private async Task OnMemberSelectionChanged(ChangeEventArgs e)
{
    // JavaScript helper gets all selected option values
    var selectedValues = await JSRuntime.InvokeAsync<string[]>(
        "getSelectedValues",
        _memberSelectElement);
    
    _selectedMemberIds = selectedValues?.ToList() ?? new List<string>();
}
```

#### **Saving Settings:**
```csharp
private async Task SaveSettings()
{
    // Convert list back to comma-separated string
    _settings.NotificationRecipients = (_selectedMemberIds == null || _selectedMemberIds.Count == 0)
        ? null
        : string.Join(",", _selectedMemberIds);

    await ReimbursementService.UpdateSettingsAsync(_settings);
}
```

---

## User Experience

### **Admin Workflow:**
1. Navigate to `/reimbursements/settings`
2. See two sections:
   - **General Settings:** Receipt requirement checkbox
   - **Notification Recipients:** Multi-select list of all members
3. Toggle receipt requirement on/off
4. Select members who should receive notifications (Ctrl+Click for multiple)
5. Click "Save Settings"
6. Success message appears

### **Visual Elements:**
- Loading spinner while fetching data
- Error alerts if loading fails
- Success/error messages after save
- Cancel button to return to `/reimbursements`

---

## How Notifications Work (Implied)

When a member submits a reimbursement request:
1. System reads `NotificationRecipients` from settings
2. Splits the comma-separated string to get member IDs
3. Looks up email addresses for those members
4. Sends notification emails to all recipients

---

## Current Limitations & Considerations

### **All Members Are Shown:**
- ✅ **Pro:** Simple, no filtering needed
- ❌ **Con:** List can be very long if you have many members
- ❌ **Con:** Includes inactive members, non-board members, etc.

### **No Role Filtering:**
- Members are not filtered by role (e.g., "Board Member", "Treasurer")
- Admin must manually know which members should receive notifications

### **No Search/Filter:**
- No search box to find specific members quickly
- Must scroll through entire list

### **JavaScript Dependency:**
- Relies on custom JavaScript function `getSelectedValues`
- If JavaScript fails, selection won't work

---

## Potential Improvements

1. **Filter by Role:**
   ```csharp
   _allMembers = MemberRepository.GetAllMembers()
       .Where(m => m.IsActive && (m.IsBoard || m.Role == "Treasurer"))
       .OrderBy(m => m.LastName)
       .ToList();
   ```

2. **Search Box:**
   Add a text input to filter members by name

3. **Visual Grouping:**
   Group by role (Board Members, Treasurers, etc.)

4. **Checkbox List:**
   Replace multi-select with checkboxes for better UX

5. **Email Preview:**
   Show email addresses next to names

---

## Summary

**What:** Admin configuration page for reimbursement system  
**Who Can Access:** Admins only  
**Member Selection:** ALL members from database, manually selected via multi-select dropdown  
**Storage:** Comma-separated member IDs in database  
**Purpose:** Control receipt requirements and notification recipients  
**Current Logic:** No filtering - shows every member in the system
