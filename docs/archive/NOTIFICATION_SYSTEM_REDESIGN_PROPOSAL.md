# Notification System Redesign Proposal

## Current Problems
1. ❌ Reimbursement Settings is a separate page (should be integrated)
2. ❌ Manual member selection (should be automatic based on role)
3. ❌ Only email notifications (no SMS/text option)
4. ❌ No centralized notification preferences
5. ❌ No place to store contact info (email/phone)

---

## Proposed Solution

### **Architecture Overview**

```
User Management (Admin)
├── User List
├── User Details/Edit
└── 🆕 Notification Preferences (NEW PAGE)
    ├── Configure notification methods per user
    ├── Set email addresses
    ├── Set phone numbers
    └── Enable/disable notifications per event type
```

---

## Implementation Plan

### **Phase 1: Centralized Notification Preferences**

#### **New Page: `/admin/notification-preferences`**

**Purpose:** Single place to configure ALL notification settings for ALL users

**Features:**
- List all users (especially Directors/Board Members)
- For each user, configure:
  - ✉️ Email address (primary/alternate)
  - 📱 Phone number (for SMS)
  - Notification preferences per event type

**UI Layout:**
```
┌─────────────────────────────────────────────────────────┐
│  Notification Preferences                               │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  Filter: [All Users ▼] [Directors Only] [Board Only]   │
│                                                         │
│  ┌───────────────────────────────────────────────────┐ │
│  │ User: John Smith (Director)                       │ │
│  │ ┌─────────────────────────────────────────────┐   │ │
│  │ │ Contact Info                                │   │ │
│  │ │ Email: john@example.com                     │   │ │
│  │ │ Phone: (555) 123-4567                       │   │ │
│  │ └─────────────────────────────────────────────┘   │ │
│  │                                                   │ │
│  │ Notification Events:                              │ │
│  │ ┌─────────────────────────────────────────────┐   │ │
│  │ │ Event Type          │ Email │ SMS  │ Both  │   │ │
│  │ ├─────────────────────┼───────┼──────┼───────┤   │ │
│  │ │ Reimbursement       │  ☑    │  ☐   │  ☐    │   │ │
│  │ │ New Member Signup   │  ☐    │  ☑   │  ☐    │   │ │
│  │ │ Dues Payment        │  ☐    │  ☐   │  ☑    │   │ │
│  │ │ System Alerts       │  ☑    │  ☐   │  ☐    │   │ │
│  │ └─────────────────────────────────────────────┘   │ │
│  └───────────────────────────────────────────────────┘ │
│                                                         │
│  [Save All Changes]                                     │
└─────────────────────────────────────────────────────────┘
```

---

### **Phase 2: Update Manage Reimbursements Page**

#### **Integrate Settings into `/reimbursements/manage`**

**Add Settings Section:**
```razor
┌─────────────────────────────────────────────────────────┐
│  Manage Reimbursements                                  │
├─────────────────────────────────────────────────────────┤
│  [Pending] [Approved] [Rejected] [All]                  │
│                                                         │
│  🆕 Settings                                            │
│  ┌─────────────────────────────────────────────────┐   │
│  │ ☑ Require receipts for all items               │   │
│  │                                                 │   │
│  │ Notification Recipients (Auto: Current Directors)│   │
│  │ • John Smith - Email ✓ SMS ✓                   │   │
│  │ • Jane Doe - Email ✓ SMS ✗                     │   │
│  │ • Bob Johnson - Email ✗ SMS ✓                  │   │
│  │                                                 │   │
│  │ [Configure Notifications →]                     │   │
│  └─────────────────────────────────────────────────┘   │
│                                                         │
│  Reimbursement Requests                                 │
│  ┌─────────────────────────────────────────────────┐   │
│  │ ... (existing table)                            │   │
│  └─────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
```

**Key Changes:**
- ✅ Settings embedded at top of page (collapsible section)
- ✅ Auto-populate with current Directors
- ✅ Show notification method per director
- ✅ Link to centralized notification preferences
- ❌ Remove manual member selection
- ❌ Delete `/reimbursements/settings` page

---

### **Phase 3: Database Schema Updates**

#### **New Table: `UserNotificationPreferences`**
```sql
CREATE TABLE UserNotificationPreferences (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT NOT NULL,  -- FK to Users table
    Email NVARCHAR(255),
    Phone NVARCHAR(50),
    
    -- Reimbursement Notifications
    ReimbursementNotifyEmail BIT DEFAULT 0,
    ReimbursementNotifySMS BIT DEFAULT 0,
    
    -- Member Signup Notifications
    MemberSignupNotifyEmail BIT DEFAULT 0,
    MemberSignupNotifySMS BIT DEFAULT 0,
    
    -- Dues Payment Notifications
    DuesPaymentNotifyEmail BIT DEFAULT 0,
    DuesPaymentNotifySMS BIT DEFAULT 0,
    
    -- System Alerts
    SystemAlertNotifyEmail BIT DEFAULT 0,
    SystemAlertNotifySMS BIT DEFAULT 0,
    
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    
    FOREIGN KEY (UserId) REFERENCES Users(Id)
)
```

#### **Update `ReimbursementSettings`**
```sql
-- Remove NotificationRecipients column (no longer needed)
ALTER TABLE ReimbursementSettings
DROP COLUMN NotificationRecipients;

-- Keep only:
-- - Id
-- - ReceiptRequired
```

---

### **Phase 4: Notification Service**

#### **New Service: `NotificationService.cs`**

```csharp
public class NotificationService
{
    public async Task SendReimbursementNotificationsAsync(ReimbursementRequest request)
    {
        // 1. Get all current Directors
        var directors = await _userRepository.GetDirectorsAsync();
        
        // 2. Get notification preferences for each director
        var preferences = await _notificationPrefsRepository
            .GetByUserIdsAsync(directors.Select(d => d.Id));
        
        // 3. Send notifications based on preferences
        foreach (var director in directors)
        {
            var pref = preferences.FirstOrDefault(p => p.UserId == director.Id);
            if (pref == null) continue;
            
            // Send email if enabled
            if (pref.ReimbursementNotifyEmail && !string.IsNullOrEmpty(pref.Email))
            {
                await _emailService.SendReimbursementNotificationAsync(
                    pref.Email, request);
            }
            
            // Send SMS if enabled
            if (pref.ReimbursementNotifySMS && !string.IsNullOrEmpty(pref.Phone))
            {
                await _smsService.SendReimbursementNotificationAsync(
                    pref.Phone, request);
            }
        }
    }
}
```

---

## Benefits of This Approach

### **1. Centralized Management**
- ✅ One place to manage ALL notifications
- ✅ Consistent UI/UX across all notification types
- ✅ Easy to add new notification types

### **2. Role-Based Automation**
- ✅ Auto-populate Directors (no manual selection)
- ✅ When someone becomes/stops being a Director, notifications auto-update
- ✅ No stale notification lists

### **3. Multi-Channel Support**
- ✅ Email notifications
- ✅ SMS/text notifications
- ✅ Per-user, per-event preferences

### **4. Scalability**
- ✅ Easy to add new notification types:
  - New member signups
  - Dues payments
  - Lottery sales
  - System alerts
  - Controller events
- ✅ Easy to add new notification channels (push, Slack, etc.)

### **5. Better UX**
- ✅ Settings integrated into relevant pages
- ✅ No separate settings pages to hunt for
- ✅ Clear visibility of who gets notified

---

## Migration Path

### **Step 1: Create New Infrastructure**
1. Create `UserNotificationPreferences` table
2. Create `NotificationPreferencesPage.razor`
3. Create `NotificationService.cs`
4. Add email/phone fields to User entity (if not exists)

### **Step 2: Migrate Existing Data**
1. Read current `ReimbursementSettings.NotificationRecipients`
2. For each member ID:
   - Create `UserNotificationPreferences` record
   - Set `ReimbursementNotifyEmail = true`
   - Set email from User table

### **Step 3: Update Reimbursement Pages**
1. Add settings section to `ManageReimbursements.razor`
2. Update to use `NotificationService`
3. Remove `/reimbursements/settings` page
4. Remove from NavMenu

### **Step 4: Clean Up**
1. Drop `NotificationRecipients` column from `ReimbursementSettings`
2. Update all notification-sending code to use `NotificationService`

---

## Other Pages That May Need Cleanup

### **Potential Notification Points:**
1. **Member Signup** - Notify Directors when new member signs up
2. **Dues Payment** - Notify Treasurer when dues are paid
3. **Lottery Sales** - Notify Lottery Manager
4. **Controller Events** - Notify Maintenance team for door issues
5. **System Diagnostics** - Notify IT admin for system issues
6. **Audit Logs** - Notify admins for security events

### **Pages to Review:**
- `/members` - New member notifications?
- `/dues` - Payment notifications?
- `/lottery` - Sales notifications?
- `/admin/system-diagnostics` - Alert notifications?
- `/admin/audit-logs` - Security notifications?

---

## Recommendation

**YES, create the centralized Notification Preferences page!**

### **Immediate Actions:**
1. ✅ Create `/admin/notification-preferences` page
2. ✅ Add `UserNotificationPreferences` table
3. ✅ Move reimbursement settings into `/reimbursements/manage`
4. ✅ Delete `/reimbursements/settings` page
5. ✅ Auto-populate with Directors only
6. ✅ Add Email/SMS toggle per director

### **Future Enhancements:**
- Add more notification types as needed
- Add push notifications
- Add notification history/logs
- Add notification templates
- Add quiet hours (don't send SMS at night)

---

## Summary

**Current:** Manual member selection, email-only, separate settings page  
**Proposed:** Auto Directors, Email+SMS, centralized preferences, integrated settings  
**Result:** Better UX, more flexible, easier to maintain, scalable

**Next Step:** Shall I start implementing this redesign?
