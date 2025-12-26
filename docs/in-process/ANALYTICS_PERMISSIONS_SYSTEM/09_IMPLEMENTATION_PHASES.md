# Implementation Phases - Analytics & Permissions System

**Document:** Detailed Implementation Plan for Jules  
**Version:** 1.0  
**Last Updated:** December 26, 2025

---

## 🎯 OVERVIEW

This document breaks down the Analytics & Permissions System into 6 manageable phases for Jules to implement. Each phase is designed to be completed independently without breaking existing functionality.

---

## 📋 PHASE 1: DATABASE & FOUNDATION

**Duration:** 2-3 days  
**Priority:** CRITICAL (Must be completed first)

### **Objectives:**
- Create all database tables
- Set up data collection infrastructure
- Implement basic tracking mechanisms
- No UI changes in this phase

### **Tasks:**

#### **1.1 Create Database Migration**
```csharp
// File: Migrations/YYYYMMDD_AddAnalyticsPermissionsSystem.cs

- Create all tables from 01_DATABASE_SCHEMA.md
- Add foreign key constraints
- Create indexes
- Seed default data
```

#### **1.2 Create Entity Models**
```csharp
// Files to create:
- Models/Analytics/AnalyticsPageView.cs
- Models/Analytics/AnalyticsHallRentalVisitor.cs
- Models/Analytics/AnalyticsHallRentalApplication.cs
- Models/Analytics/AnalyticsAggregatedDaily.cs
- Models/Permissions/Role.cs
- Models/Permissions/Permission.cs
- Models/Permissions/RolePermission.cs
- Models/Permissions/UserRole.cs
- Models/Permissions/UserPermission.cs
- Models/Permissions/PermissionAuditLog.cs
- Models/Notifications/NotificationSettings.cs
- Models/Notifications/NotificationTemplate.cs
- Models/Notifications/UserNotificationPreference.cs
- Models/Notifications/NotificationQueue.cs
- Models/Notifications/NotificationHistory.cs
- Models/System/DatabaseStorageMetrics.cs
- Models/System/DataArchivalLog.cs
```

#### **1.3 Update DbContext**
```csharp
// File: Data/GfcDbContext.cs

public DbSet<AnalyticsPageView> AnalyticsPageViews { get; set; }
public DbSet<AnalyticsHallRentalVisitor> AnalyticsHallRentalVisitors { get; set; }
// ... add all other DbSets
```

#### **1.4 Seed Default Data**
```csharp
// File: Data/SeedData/PermissionsSeedData.cs

Default Roles:
- Admin (IsSystemRole = true)
- Operations Director (IsDirectorRole = true)
- Finance Director (IsDirectorRole = true)
- Security Director (IsDirectorRole = true)
- Events Director (IsDirectorRole = true)

Default Permissions:
- See detailed list in 03_PERMISSIONS_SPECIFICATION.md

Default Notification Templates:
- NewHallRentalApplication
- HallRentalApproved
- HallRentalRejected
- PaymentReceived
- PaymentOverdue
- CameraMotionDetected
- CameraOffline
```

#### **1.5 Create Base Services**
```csharp
// Files to create:
- Services/Analytics/AnalyticsTrackingService.cs
- Services/Permissions/PermissionService.cs
- Services/Notifications/NotificationService.cs

// Basic implementations only - full features in later phases
```

#### **1.6 Create Analytics Middleware**
```csharp
// File: Middleware/AnalyticsTrackingMiddleware.cs

- Track page views automatically
- Extract visitor information (city, state, device, browser)
- Generate session IDs
- Store in AnalyticsPageView table
```

#### **1.7 Testing**
- Run migration on development database
- Verify all tables created
- Verify seed data inserted
- Test middleware tracking
- Check no existing functionality broken

### **Deliverables:**
- ✅ All database tables created
- ✅ Entity models defined
- ✅ DbContext updated
- ✅ Default data seeded
- ✅ Basic services created
- ✅ Analytics middleware functional
- ✅ No breaking changes

---

## 🔐 PHASE 2: PERMISSIONS SYSTEM

**Duration:** 3-4 days  
**Priority:** HIGH  
**Depends On:** Phase 1

### **Objectives:**
- Build role management UI
- Implement permission-based menu visibility
- Create audit logging
- Enable permission checks throughout app

### **Tasks:**

#### **2.1 Create Permission Service**
```csharp
// File: Services/Permissions/PermissionService.cs

Methods:
- Task<bool> UserHasPermission(int userId, string module, string feature, string action)
- Task<List<Permission>> GetUserPermissions(int userId)
- Task<List<string>> GetUserModules(int userId)
- Task GrantPermission(int userId, int permissionId, int grantedBy, string reason)
- Task RevokePermission(int userId, int permissionId, int revokedBy, string reason)
- Task<List<Role>> GetUserRoles(int userId)
- Task AssignRole(int userId, int roleId, int assignedBy)
- Task RemoveRole(int userId, int roleId, int removedBy)
```

#### **2.2 Create Role Management Page**
```razor
// File: Pages/Permissions/RoleManagement.razor

Features:
- List all roles
- Create new role
- Edit existing role
- Delete role (with confirmation)
- View users assigned to role
- Permission matrix for each role
```

#### **2.3 Create User Permissions Page**
```razor
// File: Pages/Permissions/UserPermissions.razor

Features:
- Search/filter users
- View user's current roles
- Assign/remove roles
- Custom permissions per user
- Temporary access grants
- Permission history for user
```

#### **2.4 Create Audit Log Page**
```razor
// File: Pages/Permissions/AuditLog.razor

Features:
- Filter by date range
- Filter by user
- Filter by action type
- Export to CSV
- Detailed change view
```

#### **2.5 Implement Menu Visibility**
```razor
// File: Shared/NavMenu.razor

Update navigation to check permissions:

@if (await PermissionService.UserHasPermission(CurrentUserId, "HallRentals", "View", "Any"))
{
    <NavLink href="hall-rentals">
        Hall Rentals
    </NavLink>
}

@if (await PermissionService.UserHasPermission(CurrentUserId, "Analytics", "View", "Any"))
{
    <NavLink href="analytics">
        Analytics
    </NavLink>
}
```

#### **2.6 Create Permission Attribute**
```csharp
// File: Attributes/RequirePermissionAttribute.cs

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePermissionAttribute : Attribute
{
    public string Module { get; set; }
    public string Feature { get; set; }
    public string Action { get; set; }
}

// Usage:
[RequirePermission(Module = "HallRentals", Feature = "Analytics", Action = "View")]
public class HallRentalAnalytics : ComponentBase
{
    // ...
}
```

#### **2.7 Create Permission Check Component**
```razor
// File: Components/PermissionCheck.razor

<PermissionCheck Module="HallRentals" Feature="ManageApplications" Action="Approve">
    <Authorized>
        <button @onclick="ApproveApplication">Approve</button>
    </Authorized>
    <NotAuthorized>
        <span class="text-gray-400">No permission</span>
    </NotAuthorized>
</PermissionCheck>
```

#### **2.8 Update Existing Pages**
```
Add permission checks to:
- Hall Rentals pages
- Camera System pages
- Settings pages
- Any other restricted pages
```

#### **2.9 Testing**
- Test role creation/editing
- Test permission assignment
- Test menu visibility
- Test page access restrictions
- Test audit logging
- Test with different user roles

### **Deliverables:**
- ✅ Role Management page functional
- ✅ User Permissions page functional
- ✅ Audit Log page functional
- ✅ Menu visibility based on permissions
- ✅ Permission checks on all pages
- ✅ Audit trail working
- ✅ No breaking changes

---

## 📊 PHASE 3: ANALYTICS - MAIN DASHBOARD

**Duration:** 2-3 days  
**Priority:** MEDIUM  
**Depends On:** Phase 1, Phase 2

### **Objectives:**
- Build `/analytics` main dashboard
- Display system-wide metrics
- Create module summary cards
- Enable navigation to detailed pages

### **Tasks:**

#### **3.1 Create Analytics Service**
```csharp
// File: Services/Analytics/AnalyticsService.cs

Methods:
- Task<SystemWideMetrics> GetSystemWideMetrics(DateTime startDate, DateTime endDate)
- Task<ModuleSummary> GetModuleSummary(string module, DateTime startDate, DateTime endDate)
- Task<List<ModuleSummary>> GetAllModuleSummaries(DateTime startDate, DateTime endDate)
```

#### **3.2 Create Main Analytics Page**
```razor
// File: Pages/Analytics/Index.razor

Layout:
┌─────────────────────────────────────┐
│  System-Wide Metrics                │
│  [Total Users] [Active Today] etc.  │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│  Hall Rentals Summary               │
│  [View Details →]                   │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│  Camera System Summary              │
│  [View Details →]                   │
└─────────────────────────────────────┘
```

#### **3.3 Create Module Summary Component**
```razor
// File: Components/Analytics/ModuleSummaryCard.razor

Props:
- ModuleName
- Metrics (dictionary)
- DetailUrl
- Icon
- Color
```

#### **3.4 Create Date Range Selector**
```razor
// File: Components/Analytics/DateRangeSelector.razor

Options:
- Today
- Last 7 Days
- Last 30 Days
- Last 90 Days
- Custom Range
```

#### **3.5 Create Chart Components**
```razor
// Files:
- Components/Analytics/LineChart.razor
- Components/Analytics/PieChart.razor
- Components/Analytics/BarChart.razor
- Components/Analytics/HeatMap.razor

// Use Chart.js or similar library
```

#### **3.6 Add Analytics Menu Item**
```razor
// File: Shared/NavMenu.razor

<NavLink href="analytics">
    <span class="icon">📊</span>
    Analytics
</NavLink>
```

#### **3.7 Testing**
- Test dashboard loads quickly
- Test date range filtering
- Test module summary cards
- Test navigation to detail pages
- Test with different user permissions

### **Deliverables:**
- ✅ Main analytics dashboard functional
- ✅ System-wide metrics displayed
- ✅ Module summaries working
- ✅ Charts rendering correctly
- ✅ Date filtering working
- ✅ Navigation to detail pages
- ✅ Performance acceptable (< 2 seconds)

---

## 🏛️ PHASE 4: ANALYTICS - HALL RENTALS DETAIL

**Duration:** 3-4 days  
**Priority:** HIGH  
**Depends On:** Phase 1, Phase 3

### **Objectives:**
- Build `/analytics/hall-rentals` detailed page
- Implement visitor tracking
- Display application statistics
- Show revenue analytics
- Create booking pattern visualizations

### **Tasks:**

#### **4.1 Create Hall Rental Analytics Service**
```csharp
// File: Services/Analytics/HallRentalAnalyticsService.cs

Methods:
- Task<VisitorMetrics> GetVisitorMetrics(DateTime start, DateTime end)
- Task<ApplicationMetrics> GetApplicationMetrics(DateTime start, DateTime end)
- Task<RevenueMetrics> GetRevenueMetrics(DateTime start, DateTime end)
- Task<List<PopularDate>> GetPopularDates(DateTime start, DateTime end)
- Task<Dictionary<string, int>> GetEventTypeDistribution(DateTime start, DateTime end)
- Task<ConversionFunnel> GetConversionFunnel(DateTime start, DateTime end)
```

#### **4.2 Create Hall Rentals Analytics Page**
```razor
// File: Pages/Analytics/HallRentals.razor

Tier 1: Hero Metrics
- Total Applications
- Pending Review
- Revenue This Month
- Conversion Rate

Tier 2: Quick Insights
- Today's Activity
- Popular Dates
- Event Type Distribution
- Calendar Heatmap

Tier 3: Deep Dive Tabs
- Traffic Analytics
- Application Details
- Revenue Reports
- Operational Metrics
```

#### **4.3 Implement Visitor Tracking**
```csharp
// File: Services/Analytics/HallRentalTrackingService.cs

Track:
- Gallery views
- Calendar interactions
- Form starts/completions
- Draft saves
- Policy link clicks
- Date selections
- Entry/exit points
```

#### **4.4 Create Conversion Funnel Component**
```razor
// File: Components/Analytics/ConversionFunnel.razor

Display:
Gallery → Calendar → Form → Submission
  100%      75%      40%      18%
```

#### **4.5 Create Calendar Heatmap Component**
```razor
// File: Components/Analytics/CalendarHeatmap.razor

Visual representation of:
- Most requested dates
- Booking density
- Available vs booked
```

#### **4.6 Create Revenue Chart Component**
```razor
// File: Components/Analytics/RevenueChart.razor

Display:
- Revenue over time
- Revenue by membership type
- Revenue by event type
- Average booking value
```

#### **4.7 Create Popular Dates Table**
```razor
// File: Components/Analytics/PopularDatesTable.razor

Columns:
- Date
- Requests
- Status (Available/Booked)
- Event Type (if booked)
```

#### **4.8 Implement Export Functionality**
```csharp
// File: Services/Analytics/AnalyticsExportService.cs

Export formats:
- PDF report
- Excel spreadsheet
- CSV data
```

#### **4.9 Testing**
- Test all metrics calculations
- Test visitor tracking accuracy
- Test charts rendering
- Test export functionality
- Test performance with large datasets

### **Deliverables:**
- ✅ Hall Rentals analytics page functional
- ✅ Visitor tracking working
- ✅ All metrics accurate
- ✅ Charts and visualizations working
- ✅ Export functionality working
- ✅ Performance acceptable

---

## 🔔 PHASE 5: NOTIFICATION SYSTEM

**Duration:** 2-3 days  
**Priority:** MEDIUM  
**Depends On:** Phase 1, Phase 2

### **Objectives:**
- Build notification settings page
- Implement email notifications
- Implement SMS notifications
- Implement in-app notifications
- Create user preference system

### **Tasks:**

#### **5.1 Create Notification Service**
```csharp
// File: Services/Notifications/NotificationService.cs

Methods:
- Task SendNotification(int userId, string eventType, Dictionary<string, string> data)
- Task SendEmail(string to, string subject, string body)
- Task SendSMS(string to, string body)
- Task SendInApp(int userId, string title, string body, string icon, string color)
- Task ProcessNotificationQueue()
- Task GroupNotifications()
```

#### **5.2 Create Email Service**
```csharp
// File: Services/Notifications/EmailService.cs

- Use SMTP settings from NotificationSettings
- Support HTML templates
- Handle attachments
- Track delivery status
```

#### **5.3 Create SMS Service**
```csharp
// File: Services/Notifications/SmsService.cs

- Integrate with Twilio (or similar)
- Use settings from NotificationSettings
- Track delivery status
```

#### **5.4 Create In-App Notification Service**
```csharp
// File: Services/Notifications/InAppNotificationService.cs

- Use SignalR for real-time delivery
- Store in database
- Mark as read/unread
- Auto-delete after retention period
```

#### **5.5 Create Notification Settings Page**
```razor
// File: Pages/Settings/Notifications.razor

Sections:
- Email Settings (SMTP configuration)
- SMS Settings (Provider configuration)
- In-App Settings
- Grouping Settings
- Test Send Buttons
```

#### **5.6 Create Notification Templates Page**
```razor
// File: Pages/Settings/NotificationTemplates.razor

Features:
- List all templates
- Edit template content
- Preview template
- Test send
- Placeholder documentation
```

#### **5.7 Create User Notification Preferences**
```razor
// File: Pages/Profile/NotificationPreferences.razor

Per event type:
- Enable/disable email
- Enable/disable SMS
- Enable/disable in-app
- Quiet hours settings
- Grouping preferences
```

#### **5.8 Create Notification History Page**
```razor
// File: Pages/Settings/NotificationHistory.razor

Features:
- Filter by date, user, channel
- View delivery status
- Resend failed notifications
- Export log
```

#### **5.9 Create Notification Bell Component**
```razor
// File: Components/NotificationBell.razor

Features:
- Badge with unread count
- Dropdown with recent notifications
- Mark as read
- Link to full history
```

#### **5.10 Implement Background Worker**
```csharp
// File: Services/BackgroundWorkers/NotificationWorker.cs

- Process notification queue every minute
- Group notifications based on settings
- Send via appropriate channels
- Update delivery status
- Retry failed notifications
```

#### **5.11 Testing**
- Test email sending
- Test SMS sending
- Test in-app notifications
- Test grouping logic
- Test quiet hours
- Test user preferences
- Test notification history

### **Deliverables:**
- ✅ Notification settings page functional
- ✅ Email notifications working
- ✅ SMS notifications working
- ✅ In-app notifications working
- ✅ User preferences working
- ✅ Grouping working
- ✅ Notification history working
- ✅ Background worker running

---

## 🗄️ PHASE 6: ADDITIONAL FEATURES

**Duration:** 2-3 days  
**Priority:** LOW  
**Depends On:** All previous phases

### **Objectives:**
- Add database storage monitor
- Implement data archival
- Add export capabilities
- Create analytics for other modules
- Polish and optimize

### **Tasks:**

#### **6.1 Create Database Monitor Service**
```csharp
// File: Services/System/DatabaseMonitorService.cs

Methods:
- Task<DatabaseStorageMetrics> GetCurrentMetrics()
- Task<List<DatabaseStorageMetrics>> GetHistoricalMetrics(int days)
- Task<bool> ShouldArchive()
- Task<List<string>> GetArchivalRecommendations()
```

#### **6.2 Create Database Monitor Page**
```razor
// File: Pages/Settings/DatabaseMonitor.razor

Display:
- Total database size
- Size by module
- Growth rate
- Projected size
- Archival recommendations
- Manual archive button
```

#### **6.3 Create Data Archival Service**
```csharp
// File: Services/System/DataArchivalService.cs

Methods:
- Task ArchiveOldAnalytics(DateTime cutoffDate)
- Task ArchiveOldNotifications(DateTime cutoffDate)
- Task<DataArchivalLog> GetArchivalHistory()
```

#### **6.4 Implement Auto-Archival**
```csharp
// File: Services/BackgroundWorkers/ArchivalWorker.cs

- Run daily
- Archive analytics data > 1 year old
- Archive notification history > 90 days old
- Log all archival operations
```

#### **6.5 Create Camera System Analytics**
```razor
// File: Pages/Analytics/CameraSystem.razor

Metrics:
- Camera uptime
- Motion events
- Storage usage
- Recording statistics
```

#### **6.6 Create Events Analytics**
```razor
// File: Pages/Analytics/Events.razor

Metrics:
- Event attendance
- Event types
- Venue utilization
```

#### **6.7 Add Visitor Tracking Toggle**
```razor
// File: Pages/Settings/Analytics.razor

Settings:
- Tracking mode (Anonymous / Returning Visitor)
- Cookie consent banner toggle
- Cookie duration
- Data retention period
```

#### **6.8 Performance Optimization**
```
- Add caching for frequently accessed data
- Optimize database queries
- Add pagination for large datasets
- Implement lazy loading
```

#### **6.9 Final Testing**
- Full system integration test
- Performance testing
- Security audit
- User acceptance testing

### **Deliverables:**
- ✅ Database monitor functional
- ✅ Auto-archival working
- ✅ Additional module analytics
- ✅ Visitor tracking configurable
- ✅ Performance optimized
- ✅ All features tested
- ✅ Documentation complete

---

## 📝 GENERAL GUIDELINES FOR JULES

### **Code Standards:**
1. Follow existing code patterns in the project
2. Use meaningful variable/method names
3. Add XML comments to public methods
4. Handle errors gracefully
5. Log important operations

### **Testing Requirements:**
1. Test each feature before moving to next
2. Test with different user roles
3. Test edge cases
4. Test performance with large datasets
5. Test on development environment first

### **Database Changes:**
1. Always create migration scripts
2. Test migrations on dev database first
3. Backup production before applying
4. Document any manual steps required

### **UI/UX Guidelines:**
1. Follow existing design patterns
2. Use Tailwind CSS classes
3. Ensure responsive design
4. Add loading states
5. Show user-friendly error messages

### **Security:**
1. Validate all user inputs
2. Check permissions before operations
3. Encrypt sensitive data
4. Log security-related events
5. Follow OWASP guidelines

---

## ✅ PHASE COMPLETION CHECKLIST

**Before marking a phase complete:**
- [ ] All tasks completed
- [ ] Code reviewed
- [ ] Tests passing
- [ ] Documentation updated
- [ ] No breaking changes
- [ ] Performance acceptable
- [ ] Security verified
- [ ] Deployed to development
- [ ] User acceptance obtained

---

## 📞 SUPPORT & QUESTIONS

If Jules encounters any issues or has questions:
1. Document the issue clearly
2. Provide error messages/logs
3. Suggest possible solutions
4. Ask for clarification if needed

---

**Last Updated:** December 26, 2025  
**Version:** 1.0
