# Notification System - Final Implementation Plan

## ✅ APPROVED DESIGN

### **Key Decisions:**
1. **Manual Selection** - Admin manually selects which users get notifications (not automatic)
2. **Location** - Integrated into User Management page (`/users`)
3. **Default** - All notifications OFF by default
4. **Directors Filter** - Filter to show only current/active Directors
5. **Reminders** - Alert when new Directors need notification setup
6. **Cleanup** - Auto-disable notifications when Director leaves board
7. **Visual Tracking** - Any page modified by this plan must include a visible **[MODIFIED]** tag. New components (like the notification modal) must be tagged/wrapped with a visible **[NEW]** tag.

---

## 📋 IMPLEMENTATION STEPS

### **Phase 1: Database & Entities**

#### 1.1 Update UserNotificationPreferences Entity
**File:** `Data/Entities/UserNotificationPreferences.cs`

Add reminder tracking fields:
```csharp
public bool NotificationReminderDismissed { get; set; } = false;
public DateTime? NotificationReminderDismissedAt { get; set; }
```

#### 1.2 Create Database Migration
```sql
-- Add UserNotificationPreferences table
CREATE TABLE UserNotificationPreferences (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Email NVARCHAR(255) NULL,
    Phone NVARCHAR(50) NULL,
    
    ReimbursementNotifyEmail BIT NOT NULL DEFAULT 0,
    ReimbursementNotifySMS BIT NOT NULL DEFAULT 0,
    MemberSignupNotifyEmail BIT NOT NULL DEFAULT 0,
    MemberSignupNotifySMS BIT NOT NULL DEFAULT 0,
    DuesPaymentNotifyEmail BIT NOT NULL DEFAULT 0,
    DuesPaymentNotifySMS BIT NOT NULL DEFAULT 0,
    SystemAlertNotifyEmail BIT NOT NULL DEFAULT 0,
    SystemAlertNotifySMS BIT NOT NULL DEFAULT 0,
    LotterySalesNotifyEmail BIT NOT NULL DEFAULT 0,
    LotterySalesNotifySMS BIT NOT NULL DEFAULT 0,
    ControllerEventNotifyEmail BIT NOT NULL DEFAULT 0,
    ControllerEventNotifySMS BIT NOT NULL DEFAULT 0,
    
    NotificationReminderDismissed BIT NOT NULL DEFAULT 0,
    NotificationReminderDismissedAt DATETIME2 NULL,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE INDEX IX_UserNotificationPreferences_UserId 
ON UserNotificationPreferences(UserId);
```

---

### **Phase 2: User Management Page Updates**

#### 2.1 Add Filter Tabs
**File:** `Components/Pages/UserManagement.razor`

Add above user table:
```razor
<!-- Filter Tabs -->
<div class="mb-3">
    <ul class="nav nav-tabs">
        <li class="nav-item">
            <button class="nav-link @(_userFilter == "all" ? "active" : "")" 
                    @onclick='() => SetFilter("all")'>
                All Users (@_users.Count)
            </button>
        </li>
        <li class="nav-item">
            <button class="nav-link @(_userFilter == "directors" ? "active" : "")" 
                    @onclick='() => SetFilter("directors")'>
                Current Directors (@FilteredUsers.Count(u => IsCurrentDirector(u)))
            </button>
        </li>
        <li class="nav-item">
            <button class="nav-link @(_userFilter == "admins" ? "active" : "")" 
                    @onclick='() => SetFilter("admins")'>
                Admins (@_users.Count(u => u.IsAdmin))
            </button>
        </li>
        <li class="nav-item">
            <button class="nav-link @(_userFilter == "active" ? "active" : "")" 
                    @onclick='() => SetFilter("active")'>
                Active (@_users.Count(u => u.IsActive))
            </button>
        </li>
    </ul>
</div>
```

#### 2.2 Add Reminder Alert
Add at top of page (after page title):
```razor
@if (HasNewDirectorsWithoutNotifications())
{
    <div class="alert alert-warning alert-dismissible fade show mb-3" role="alert">
        <i class="bi bi-bell"></i>
        <strong>Action Required:</strong> 
        @_newDirectorsCount Director(s) need notification preferences configured.
        <button class="btn btn-sm btn-primary ms-2" @onclick='() => SetFilter("directors")'>
            View Directors
        </button>
        <button type="button" class="btn-close" @onclick="DismissReminder"></button>
    </div>
}
```

#### 2.3 Add Notifications Button to Actions Column
Update table header:
```razor
<th>Actions</th>
```

Update table row:
```razor
<td>
    <div class="table-actions">
        <button class="btn btn-outline-primary btn-sm" @onclick="() => ViewHistory(user.UserId)">
            History
        </button>
        <button class="btn btn-outline btn-sm" @onclick="() => EditUser(user.UserId)">
            Edit
        </button>
        <button class="btn btn-outline-info btn-sm" @onclick="() => ConfigureNotifications(user.UserId)">
            <i class="bi bi-bell"></i> Notifications
        </button>
        @if (!user.IsAdmin)
        {
            <!-- existing deactivate buttons -->
        }
    </div>
</td>
```

#### 2.4 Add Notification Configuration Modal
```razor
@* Notification Configuration Modal *@
@if (_showNotificationModal && _selectedUserId.HasValue)
{
    <div class="modal-backdrop"></div>
    <div class="modal-card" style="max-width: 700px;">
        <div class="modal-card__header d-flex justify-content-between align-items-center mb-3">
            <h4 class="mb-0">
                Notification Preferences - @_selectedUserName
                @if (_selectedUserIsDirector)
                {
                    <span class="badge bg-primary ms-2">Director</span>
                }
            </h4>
            <button class="btn btn-link text-muted p-0" type="button" @onclick="CloseNotificationModal">
                <i class="bi bi-x-lg"></i>
            </button>
        </div>

        <!-- Contact Information -->
        <div class="row g-3 mb-4">
            <div class="col-md-6">
                <label class="form-label small">Email Address</label>
                <input type="email" class="form-control form-control-sm" 
                       @bind="_notificationForm.Email" placeholder="user@example.com" />
            </div>
            <div class="col-md-6">
                <label class="form-label small">Phone Number</label>
                <input type="tel" class="form-control form-control-sm" 
                       @bind="_notificationForm.Phone" placeholder="(555) 123-4567" />
            </div>
        </div>

        <!-- Notification Preferences Table -->
        <h6 class="mb-3">Notification Events</h6>
        <div class="table-responsive">
            <table class="table table-sm table-bordered">
                <thead class="table-light">
                    <tr>
                        <th>Event Type</th>
                        <th class="text-center" style="width: 80px;">None</th>
                        <th class="text-center" style="width: 80px;">Email</th>
                        <th class="text-center" style="width: 80px;">SMS</th>
                        <th class="text-center" style="width: 80px;">Both</th>
                    </tr>
                </thead>
                <tbody>
                    <!-- Reimbursement -->
                    <tr>
                        <td><i class="bi bi-receipt text-primary"></i> Reimbursement Requests</td>
                        <td class="text-center">
                            <input type="radio" name="reimb" 
                                   checked="@(!_notificationForm.ReimbursementNotifyEmail && !_notificationForm.ReimbursementNotifySMS)"
                                   @onchange="() => SetNotification(NotificationType.Reimbursement, NotificationMethod.None)" />
                        </td>
                        <td class="text-center">
                            <input type="radio" name="reimb" 
                                   checked="@(_notificationForm.ReimbursementNotifyEmail && !_notificationForm.ReimbursementNotifySMS)"
                                   @onchange="() => SetNotification(NotificationType.Reimbursement, NotificationMethod.Email)" />
                        </td>
                        <td class="text-center">
                            <input type="radio" name="reimb" 
                                   checked="@(!_notificationForm.ReimbursementNotifyEmail && _notificationForm.ReimbursementNotifySMS)"
                                   @onchange="() => SetNotification(NotificationType.Reimbursement, NotificationMethod.SMS)" />
                        </td>
                        <td class="text-center">
                            <input type="radio" name="reimb" 
                                   checked="@(_notificationForm.ReimbursementNotifyEmail && _notificationForm.ReimbursementNotifySMS)"
                                   @onchange="() => SetNotification(NotificationType.Reimbursement, NotificationMethod.Both)" />
                        </td>
                    </tr>
                    <!-- Repeat for other notification types -->
                </tbody>
            </table>
        </div>

        <div class="modal-card__actions">
            <button type="button" class="btn btn-primary" @onclick="SaveNotificationPreferences" disabled="@_savingNotifications">
                <span class="spinner-border spinner-border-sm me-1" hidden="@(!_savingNotifications)"></span>
                Save Preferences
            </button>
            <button type="button" class="btn btn-outline" @onclick="CloseNotificationModal">Cancel</button>
        </div>
    </div>
}
```

#### 2.5 Add Code-Behind Logic
```csharp
// Filter state
private string _userFilter = "all";
private List<UserListItemDto> FilteredUsers => _userFilter switch
{
    "directors" => _users.Where(u => IsCurrentDirector(u)).ToList(),
    "admins" => _users.Where(u => u.IsAdmin).ToList(),
    "active" => _users.Where(u => u.IsActive).ToList(),
    "inactive" => _users.Where(u => !u.IsActive).ToList(),
    _ => _users
};

private void SetFilter(string filter)
{
    _userFilter = filter;
    StateHasChanged();
}

// Director checking
private List<DirectorDto> _directors = new();

private bool IsCurrentDirector(UserListItemDto user)
{
    if (!user.MemberId.HasValue) return false;
    var director = _directors.FirstOrDefault(d => d.MemberId == user.MemberId.Value);
    return director != null && director.IsActive;
}

// Notification modal state
private bool _showNotificationModal = false;
private int? _selectedUserId = null;
private string _selectedUserName = "";
private bool _selectedUserIsDirector = false;
private bool _savingNotifications = false;
private NotificationPreferencesForm _notificationForm = new();

// Reminder state
private int _newDirectorsCount = 0;

private bool HasNewDirectorsWithoutNotifications()
{
    var currentDirectors = _users.Where(u => IsCurrentDirector(u)).ToList();
    _newDirectorsCount = currentDirectors.Count(d => 
    {
        var prefs = GetNotificationPreferences(d.UserId);
        return prefs == null || 
               (!prefs.NotificationReminderDismissed && AllNotificationsAreOff(prefs));
    });
    return _newDirectorsCount > 0;
}

private bool AllNotificationsAreOff(UserNotificationPreferences prefs)
{
    return !prefs.ReimbursementNotifyEmail && !prefs.ReimbursementNotifySMS &&
           !prefs.MemberSignupNotifyEmail && !prefs.MemberSignupNotifySMS &&
           !prefs.DuesPaymentNotifyEmail && !prefs.DuesPaymentNotifySMS &&
           !prefs.SystemAlertNotifyEmail && !prefs.SystemAlertNotifySMS &&
           !prefs.LotterySalesNotifyEmail && !prefs.LotterySalesNotifySMS &&
           !prefs.ControllerEventNotifyEmail && !prefs.ControllerEventNotifySMS;
}

private async Task DismissReminder()
{
    // Mark all new directors' reminders as dismissed
    // Implementation in Phase 3
}

private async Task ConfigureNotifications(int userId)
{
    _selectedUserId = userId;
    var user = _users.FirstOrDefault(u => u.UserId == userId);
    _selectedUserName = user?.Username ?? "";
    _selectedUserIsDirector = user != null && IsCurrentDirector(user);
    
    // Load existing preferences
    var prefs = await GetNotificationPreferences(userId);
    _notificationForm = prefs != null ? MapToForm(prefs) : new NotificationPreferencesForm();
    
    _showNotificationModal = true;
}

private void CloseNotificationModal()
{
    _showNotificationModal = false;
    _selectedUserId = null;
    _notificationForm = new();
}

private void SetNotification(NotificationType type, NotificationMethod method)
{
    var (email, sms) = method switch
    {
        NotificationMethod.Email => (true, false),
        NotificationMethod.SMS => (false, true),
        NotificationMethod.Both => (true, true),
        _ => (false, false)
    };

    switch (type)
    {
        case NotificationType.Reimbursement:
            _notificationForm.ReimbursementNotifyEmail = email;
            _notificationForm.ReimbursementNotifySMS = sms;
            break;
        // ... other types
    }
}

private async Task SaveNotificationPreferences()
{
    _savingNotifications = true;
    try
    {
        // Save to database
        // Auto-dismiss reminder if any notification is enabled
        _showNotificationModal = false;
        await LoadData(); // Refresh to update reminder
    }
    finally
    {
        _savingNotifications = false;
    }
}

private enum NotificationType
{
    Reimbursement, MemberSignup, DuesPayment, 
    SystemAlert, LotterySales, ControllerEvent
}

private enum NotificationMethod
{
    None, Email, SMS, Both
}

private class NotificationPreferencesForm
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool ReimbursementNotifyEmail { get; set; }
    public bool ReimbursementNotifySMS { get; set; }
    // ... all other notification flags
}
```

---

### **Phase 3: Repository & Services**

#### 3.1 Create Repository Interface
**File:** `Core/Interfaces/IUserNotificationPreferencesRepository.cs`

#### 3.2 Create Repository Implementation
**File:** `Data/Repositories/UserNotificationPreferencesRepository.cs`

#### 3.3 Create Notification Service
**File:** `Services/NotificationService.cs`

With Director verification:
```csharp
public async Task SendReimbursementNotificationsAsync(ReimbursementRequest request)
{
    var usersWithNotifications = await _prefsRepo.GetUsersWithReimbursementNotificationsAsync();
    
    foreach (var userPref in usersWithNotifications)
    {
        // Verify user is still a current Director
        if (!await _directorService.IsCurrentDirectorAsync(userPref.UserId))
        {
            Logger.LogInformation("Skipping notification for user {UserId} - no longer a Director", userPref.UserId);
            continue;
        }
        
        // Send notifications
        if (userPref.ReimbursementNotifyEmail && !string.IsNullOrEmpty(userPref.Email))
            await _emailService.SendAsync(...);
        
        if (userPref.ReimbursementNotifySMS && !string.IsNullOrEmpty(userPref.Phone))
            await _smsService.SendAsync(...);
    }
}
```

---

### **Phase 4: Director Management Integration**

#### 4.1 Auto-Disable on Director Removal
Update Director removal logic:
```csharp
public async Task RemoveDirectorAsync(int directorId)
{
    var director = await _directorRepo.GetByIdAsync(directorId);
    
    // Remove from Directors
    await _directorRepo.RemoveAsync(directorId);
    
    // Disable all notifications
    if (director?.UserId != null)
    {
        await _notificationService.DisableAllNotificationsAsync(director.UserId);
    }
}
```

---

### **Phase 5: Cleanup**

#### 5.1 Delete Old Files
- ❌ Delete `Components/Pages/Reimbursements/Settings.razor`
- ❌ Delete `Components/Pages/Admin/NotificationPreferences.razor` (standalone page)

#### 5.2 Update ReimbursementSettings
Remove `NotificationRecipients` column

#### 5.3 Update Manage Reimbursements Page
Add read-only display of who gets notifications

---

## 📊 TESTING CHECKLIST

- [ ] Add new Director → Reminder appears
- [ ] Configure notifications → Reminder disappears
- [ ] Dismiss reminder → Stays dismissed
- [ ] Filter to Directors → Shows only current Directors
- [ ] Remove Director → Notifications auto-disabled
- [ ] Submit reimbursement → Only current Directors notified
- [ ] All notifications default to OFF

---

## 🚀 DEPLOYMENT ORDER

1. Database migration
2. Repository & services
3. User Management page updates
4. Director management integration
5. Cleanup old files
6. Testing

---

**Status:** Ready to implement
**Estimated Time:** 4-6 hours
