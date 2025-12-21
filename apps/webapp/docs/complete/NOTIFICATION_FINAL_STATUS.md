# Notification System - Final Status & Remaining Tasks

## ✅ COMPLETED (Phases 1-3)

### Phase 1: Database & Entities ✅
- [x] UserNotificationPreferences entity with reminder fields
- [x] Database migration SQL script
- [x] Added to GfcDbContext

### Phase 2: User Management UI ✅
- [x] Reminder alert for Directors
- [x] Filter tabs (All/Directors/Admins/Active/Inactive)
- [x] Notifications button in Actions column
- [x] Full notification configuration modal
- [x] All code-behind logic (fields, properties, methods)

### Phase 3: Repository & Services ✅
- [x] IUserNotificationPreferencesRepository interface
- [x] UserNotificationPreferencesRepository implementation
- [x] Registered in DI container (Program.cs)
- [x] Added DbSet to GfcDbContext

---

## 🔄 REMAINING TASKS

### Task 1: Wire Up Repository to User Management Page

**File:** `Components/Pages/UserManagement.razor`

**Inject the repository:**
```razor
@inject IUserNotificationPreferencesRepository NotificationPrefsRepo
```

**Update methods to use repository:**

1. **ConfigureNotifications** - Load existing preferences:
```csharp
private async Task ConfigureNotifications(int userId)
{
    _selectedNotificationUserId = userId;
    var user = _users.FirstOrDefault(u => u.UserId == userId);
    _selectedUserName = user?.Username ?? "";
    _selectedUserIsDirector = user != null && IsCurrentDirector(user);
    _notificationError = "";
    
    // Load existing preferences from database
    var prefs = await NotificationPrefsRepo.GetByUserIdAsync(userId);
    _notificationForm = prefs != null ? MapToForm(prefs) : new NotificationPreferencesForm();
    
    _showNotificationModal = true;
}

private NotificationPreferencesForm MapToForm(UserNotificationPreferences prefs)
{
    return new NotificationPreferencesForm
    {
        Email = prefs.Email,
        Phone = prefs.Phone,
        ReimbursementNotifyEmail = prefs.ReimbursementNotifyEmail,
        ReimbursementNotifySMS = prefs.ReimbursementNotifySMS,
        MemberSignupNotifyEmail = prefs.MemberSignupNotifyEmail,
        MemberSignupNotifySMS = prefs.MemberSignupNotifySMS,
        DuesPaymentNotifyEmail = prefs.DuesPaymentNotifyEmail,
        DuesPaymentNotifySMS = prefs.DuesPaymentNotifySMS,
        SystemAlertNotifyEmail = prefs.SystemAlertNotifyEmail,
        SystemAlertNotifySMS = prefs.SystemAlertNotifySMS,
        LotterySalesNotifyEmail = prefs.LotterySalesNotifyEmail,
        LotterySalesNotifySMS = prefs.LotterySalesNotifySMS,
        ControllerEventNotifyEmail = prefs.ControllerEventNotifyEmail,
        ControllerEventNotifySMS = prefs.ControllerEventNotifySMS
    };
}
```

2. **SaveNotificationPreferences** - Save to database:
```csharp
private async Task SaveNotificationPreferences()
{
    _savingNotifications = true;
    _notificationError = "";
    
    try
    {
        if (!_selectedNotificationUserId.HasValue) return;
        
        // Get existing or create new
        var prefs = await NotificationPrefsRepo.GetByUserIdAsync(_selectedNotificationUserId.Value);
        
        if (prefs == null)
        {
            // Create new
            prefs = new UserNotificationPreferences
            {
                UserId = _selectedNotificationUserId.Value
            };
            MapFormToEntity(_notificationForm, prefs);
            await NotificationPrefsRepo.CreateAsync(prefs);
        }
        else
        {
            // Update existing
            MapFormToEntity(_notificationForm, prefs);
            await NotificationPrefsRepo.UpdateAsync(prefs);
        }
        
        // Close modal on success
        _showNotificationModal = false;
        
        // Refresh to update reminder if needed
        await LoadData();
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "Failed to save notification preferences");
        _notificationError = "Failed to save preferences: " + ex.Message;
    }
    finally
    {
        _savingNotifications = false;
    }
}

private void MapFormToEntity(NotificationPreferencesForm form, UserNotificationPreferences entity)
{
    entity.Email = form.Email;
    entity.Phone = form.Phone;
    entity.ReimbursementNotifyEmail = form.ReimbursementNotifyEmail;
    entity.ReimbursementNotifySMS = form.ReimbursementNotifySMS;
    entity.MemberSignupNotifyEmail = form.MemberSignupNotifyEmail;
    entity.MemberSignupNotifySMS = form.MemberSignupNotifySMS;
    entity.DuesPaymentNotifyEmail = form.DuesPaymentNotifyEmail;
    entity.DuesPaymentNotifySMS = form.DuesPaymentNotifySMS;
    entity.SystemAlertNotifyEmail = form.SystemAlertNotifyEmail;
    entity.SystemAlertNotifySMS = form.SystemAlertNotifySMS;
    entity.LotterySalesNotifyEmail = form.LotterySalesNotifyEmail;
    entity.LotterySalesNotifySMS = form.LotterySalesNotifySMS;
    entity.ControllerEventNotifyEmail = form.ControllerEventNotifyEmail;
    entity.ControllerEventNotifySMS = form.ControllerEventNotifySMS;
    
    // Auto-dismiss reminder if any notification is enabled
    if (form.ReimbursementNotifyEmail || form.ReimbursementNotifySMS ||
        form.MemberSignupNotifyEmail || form.MemberSignupNotifySMS ||
        form.DuesPaymentNotifyEmail || form.DuesPaymentNotifySMS ||
        form.SystemAlertNotifyEmail || form.SystemAlertNotifySMS ||
        form.LotterySalesNotifyEmail || form.LotterySalesNotifySMS ||
        form.ControllerEventNotifyEmail || form.ControllerEventNotifySMS)
    {
        entity.NotificationReminderDismissed = true;
        entity.NotificationReminderDismissedAt = DateTime.UtcNow;
    }
}
```

---

### Task 2: Implement Director Checking

**Need to determine:** How are Directors tracked in your system?

**Option A:** If there's a Directors table/service:
```csharp
@inject IDirectorService DirectorService

private bool IsCurrentDirector(UserListItemDto user)
{
    if (!user.MemberId.HasValue) return false;
    return DirectorService.IsCurrentDirector(user.MemberId.Value);
}
```

**Option B:** If Directors are tracked by a flag on Members:
```csharp
@inject IMemberRepository MemberRepo

private bool IsCurrentDirector(UserListItemDto user)
{
    if (!user.MemberId.HasValue) return false;
    var member = MemberRepo.GetMember(user.MemberId.Value);
    return member?.IsDirector == true && member.IsActive;
}
```

---

### Task 3: Run Database Migration

**Execute the SQL script:**
```bash
# Run the migration script
sqlcmd -S your_server -d your_database -i "Database/Migrations/AddUserNotificationPreferences.sql"
```

Or use Entity Framework migrations if preferred.

---

### Task 4: Delete Old Files (Optional)

Once everything is working:
- Delete `Components/Pages/Reimbursements/Settings.razor`
- Delete `Components/Pages/Admin/NotificationPreferences.razor` (standalone page)

---

### Task 5: Update Reimbursement Notification Sending

**When a reimbursement is submitted**, use the new system:

```csharp
public async Task SendReimbursementNotificationsAsync(ReimbursementRequest request)
{
    // Get all users with reimbursement notifications enabled
    var usersWithNotifications = await _notificationPrefsRepo
        .GetUsersWithReimbursementNotificationsAsync();
    
    foreach (var userPref in usersWithNotifications)
    {
        // Verify user is still a current Director (if required)
        // Skip this check if you want ANY user to be able to receive notifications
        
        // Send email if enabled
        if (userPref.ReimbursementNotifyEmail && !string.IsNullOrEmpty(userPref.Email))
        {
            await _emailService.SendReimbursementNotificationAsync(userPref.Email, request);
        }
        
        // Send SMS if enabled
        if (userPref.ReimbursementNotifySMS && !string.IsNullOrEmpty(userPref.Phone))
        {
            await _smsService.SendReimbursementSMSAsync(userPref.Phone, request);
        }
    }
}
```

---

## 🎯 TESTING CHECKLIST

- [ ] Rebuild project (`Ctrl+Shift+B`)
- [ ] Run database migration
- [ ] Navigate to `/users`
- [ ] Click filter tabs - verify filtering works
- [ ] Click "Notifications" button - verify modal opens
- [ ] Configure notifications - verify save works
- [ ] Check database - verify record created/updated
- [ ] Submit test reimbursement - verify notifications sent

---

## 📝 KNOWN LIMITATIONS

1. **Director checking** - Currently returns false (needs implementation)
2. **Reminder logic** - Doesn't check actual Director status yet
3. **SMS service** - Not implemented (needs Twilio/AWS SNS integration)
4. **Email templates** - Using basic emails (could be enhanced)

---

## 🚀 DEPLOYMENT NOTES

1. Run database migration FIRST
2. Deploy code changes
3. Test notification configuration
4. Monitor logs for errors
5. Verify emails/SMS are sent correctly

---

**Status:** Ready for final integration and testing!
**Estimated Time to Complete:** 1-2 hours
