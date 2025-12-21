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
CREATE INDEX IX_UserNotificationPreferences_UserId ON UserNotificationPreferences(UserId);
```

### **Phase 2: User Management Page Updates**
- [ ] Add Filter Tabs (All, Directors, Admins, Active)
- [ ] Add Reminder Alert for new Directors
- [ ] Add "Notifications" button to Actions column
- [ ] Implement Notification Configuration Modal
- [ ] Add Code-Behind Logic for filters and modal state

### **Phase 3: Repository & Services**
- [ ] Create `IUserNotificationPreferencesRepository`
- [ ] Create `UserNotificationPreferencesRepository` implementation
- [ ] Create `NotificationService.cs` with Director verification logic

### **Phase 4: Director Management Integration**
- [ ] Auto-Disable notifications on Director Removal

---

## 📋 IMPLEMENTATION STATUS

### **Phase 1: Database & Entities** ✅ COMPLETE
- [x] UserNotificationPreferences entity with reminder fields
- [x] Database migration SQL script
- [x] Added to GfcDbContext

### **Phase 2: User Management UI** ✅ COMPLETE
- [x] Reminder alert for Directors
- [x] Filter tabs (All/Directors/Admins/Active/Inactive)
- [x] Notifications button in Actions column
- [x] Full notification configuration modal
- [x] All code-behind logic

### **Phase 3: Repository & Services** ✅ COMPLETE
- [x] IUserNotificationPreferencesRepository interface
- [x] UserNotificationPreferencesRepository implementation
- [x] Registered in DI container (Program.cs)
- [x] Added DbSet to GfcDbContext

---

## 🔄 REMAINING TASKS
- [ ] **Task 1: Wire Up Repository to User Management Page** (IN PROGRESS)
- [ ] **Task 2: Implement Director Checking** (PENDING)
- [ ] **Task 3: Run Database Migration** (PENDING)
- [ ] **Task 4: Update Reimbursement Notification Sending** (PENDING)

---

## 📊 TESTING CHECKLIST
- [ ] Rebuild project
- [ ] Run database migration
- [ ] Navigate to `/users` and verify filters
- [ ] Verify "Notifications" modal saves to DB
- [ ] Verify notifications only fire for current Directors
- [ ] Verify auto-disable when Director is removed
