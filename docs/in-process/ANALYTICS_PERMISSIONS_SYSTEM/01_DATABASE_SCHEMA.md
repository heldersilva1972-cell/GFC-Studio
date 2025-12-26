# Database Schema - Analytics & Permissions System

**Document:** Database Schema Specification  
**Version:** 1.0  
**Last Updated:** December 26, 2025

---

## 📊 OVERVIEW

This document defines all database tables, columns, relationships, and indexes required for the Analytics & Permissions System.

---

## 🗄️ DATABASE TABLES

### **1. ANALYTICS TABLES**

#### **AnalyticsPageView**
Tracks individual page views and visitor behavior.

```sql
CREATE TABLE AnalyticsPageView (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    SessionId UNIQUEIDENTIFIER NOT NULL,
    PageUrl NVARCHAR(500) NOT NULL,
    PageTitle NVARCHAR(200),
    Referrer NVARCHAR(500),
    
    -- Visitor Information
    VisitorCity NVARCHAR(100),
    VisitorState NVARCHAR(50),
    VisitorCountry NVARCHAR(50) DEFAULT 'USA',
    DeviceType NVARCHAR(50), -- Desktop, Mobile, Tablet
    BrowserType NVARCHAR(100),
    OperatingSystem NVARCHAR(100),
    
    -- Behavior Tracking
    TimeOnPage INT, -- Seconds
    ScrollDepth INT, -- Percentage (0-100)
    InteractionCount INT DEFAULT 0,
    
    -- Timestamps
    ViewedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ExitedAt DATETIME2,
    
    -- Indexes
    INDEX IX_SessionId (SessionId),
    INDEX IX_PageUrl (PageUrl),
    INDEX IX_ViewedAt (ViewedAt)
);
```

#### **AnalyticsHallRentalVisitor**
Specific tracking for Hall Rental page visitors.

```sql
CREATE TABLE AnalyticsHallRentalVisitor (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    SessionId UNIQUEIDENTIFIER NOT NULL,
    
    -- Journey Tracking
    ViewedGallery BIT DEFAULT 0,
    ViewedCalendar BIT DEFAULT 0,
    ViewedForm BIT DEFAULT 0,
    SubmittedApplication BIT DEFAULT 0,
    
    -- Calendar Interactions
    MonthsNavigated INT DEFAULT 0,
    DatesClicked NVARCHAR(MAX), -- JSON array of dates
    DatesSelected NVARCHAR(MAX), -- JSON array of dates
    
    -- Form Interactions
    FormStartedAt DATETIME2,
    FormCompletedAt DATETIME2,
    FormAbandonedAt DATETIME2,
    DraftSaved BIT DEFAULT 0,
    
    -- Engagement
    PolicyLinkClicked BIT DEFAULT 0,
    PricingViewed BIT DEFAULT 0,
    TimeOnGallery INT, -- Seconds
    TimeOnCalendar INT,
    TimeOnForm INT,
    
    -- Entry/Exit
    EntryPoint NVARCHAR(200), -- Direct, Google, Social, etc.
    ExitPoint NVARCHAR(200), -- Gallery, Calendar, Form, Success
    
    -- Timestamps
    FirstVisitAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastActivityAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Indexes
    INDEX IX_SessionId (SessionId),
    INDEX IX_FirstVisitAt (FirstVisitAt),
    INDEX IX_SubmittedApplication (SubmittedApplication)
);
```

#### **AnalyticsHallRentalApplication**
Analytics for submitted applications.

```sql
CREATE TABLE AnalyticsHallRentalApplication (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    ApplicationId INT NOT NULL, -- FK to HallRentalApplication
    SessionId UNIQUEIDENTIFIER,
    
    -- Application Details
    MembershipType NVARCHAR(50), -- Member, Non-Member, Non-Profit
    EventType NVARCHAR(100),
    GuestCount INT,
    EventDate DATE,
    
    -- Services Requested
    BarServiceRequested BIT DEFAULT 0,
    KitchenRequested BIT DEFAULT 0,
    AVEquipmentRequested BIT DEFAULT 0,
    SetupTimeRequested BIT DEFAULT 0,
    
    -- Financial
    EstimatedRevenue DECIMAL(10,2),
    ActualRevenue DECIMAL(10,2),
    SecurityDeposit DECIMAL(10,2),
    
    -- Processing
    Status NVARCHAR(50), -- Pending, Approved, Rejected
    SubmittedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ApprovedAt DATETIME2,
    RejectedAt DATETIME2,
    PaymentReceivedAt DATETIME2,
    
    -- Lead Time
    DaysInAdvance INT, -- Days between submission and event date
    
    -- Geographic
    ApplicantCity NVARCHAR(100),
    ApplicantState NVARCHAR(50),
    
    -- Indexes
    INDEX IX_ApplicationId (ApplicationId),
    INDEX IX_EventDate (EventDate),
    INDEX IX_SubmittedAt (SubmittedAt),
    INDEX IX_Status (Status)
);
```

#### **AnalyticsAggregatedDaily**
Pre-aggregated daily statistics for performance.

```sql
CREATE TABLE AnalyticsAggregatedDaily (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Date DATE NOT NULL,
    Module NVARCHAR(50) NOT NULL, -- HallRentals, CameraSystem, etc.
    
    -- Visitor Metrics
    TotalVisitors INT DEFAULT 0,
    UniqueVisitors INT DEFAULT 0,
    BounceRate DECIMAL(5,2), -- Percentage
    AvgSessionDuration INT, -- Seconds
    
    -- Hall Rental Specific
    ApplicationsSubmitted INT DEFAULT 0,
    ApplicationsApproved INT DEFAULT 0,
    ApplicationsRejected INT DEFAULT 0,
    RevenueGenerated DECIMAL(10,2) DEFAULT 0,
    
    -- Conversion Metrics
    GalleryToCalendar DECIMAL(5,2), -- Percentage
    CalendarToForm DECIMAL(5,2),
    FormToSubmission DECIMAL(5,2),
    OverallConversion DECIMAL(5,2),
    
    -- Timestamps
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Indexes
    UNIQUE INDEX UX_Date_Module (Date, Module),
    INDEX IX_Date (Date)
);
```

---

### **2. PERMISSIONS TABLES**

#### **Role**
Defines user roles with customizable permissions.

```sql
CREATE TABLE Role (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    
    -- Role Type
    IsSystemRole BIT DEFAULT 0, -- Admin, Director roles
    IsDirectorRole BIT DEFAULT 0,
    IsCustomRole BIT DEFAULT 1,
    
    -- Metadata
    CreatedBy INT, -- FK to User
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy INT,
    UpdatedAt DATETIME2,
    IsActive BIT DEFAULT 1,
    
    -- Indexes
    INDEX IX_Name (Name),
    INDEX IX_IsActive (IsActive)
);
```

#### **Permission**
Defines all available permissions in the system.

```sql
CREATE TABLE Permission (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Module NVARCHAR(100) NOT NULL, -- HallRentals, CameraSystem, etc.
    Feature NVARCHAR(100) NOT NULL, -- Analytics, ManageApplications, etc.
    Action NVARCHAR(50) NOT NULL, -- View, Create, Edit, Delete, Approve, Export
    
    -- Display
    DisplayName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    
    -- Categorization
    Category NVARCHAR(100), -- Operations, Finance, Security, Events
    SortOrder INT DEFAULT 0,
    
    -- Migration Tracking
    IsNew BIT DEFAULT 0,
    MigratedFrom NVARCHAR(200), -- Previous location if migrated
    
    -- Metadata
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1,
    
    -- Indexes
    UNIQUE INDEX UX_Module_Feature_Action (Module, Feature, Action),
    INDEX IX_Module (Module),
    INDEX IX_Category (Category)
);
```

#### **RolePermission**
Maps permissions to roles.

```sql
CREATE TABLE RolePermission (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleId INT NOT NULL,
    PermissionId INT NOT NULL,
    
    -- Metadata
    GrantedBy INT, -- FK to User
    GrantedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Foreign Keys
    FOREIGN KEY (RoleId) REFERENCES Role(Id) ON DELETE CASCADE,
    FOREIGN KEY (PermissionId) REFERENCES Permission(Id) ON DELETE CASCADE,
    
    -- Indexes
    UNIQUE INDEX UX_Role_Permission (RoleId, PermissionId),
    INDEX IX_RoleId (RoleId),
    INDEX IX_PermissionId (PermissionId)
);
```

#### **UserRole**
Assigns roles to users.

```sql
CREATE TABLE UserRole (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL, -- FK to existing User table
    RoleId INT NOT NULL,
    
    -- Metadata
    AssignedBy INT, -- FK to User
    AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1,
    
    -- Foreign Keys
    FOREIGN KEY (RoleId) REFERENCES Role(Id) ON DELETE CASCADE,
    
    -- Indexes
    UNIQUE INDEX UX_User_Role (UserId, RoleId),
    INDEX IX_UserId (UserId),
    INDEX IX_RoleId (RoleId)
);
```

#### **UserPermission**
Custom permissions for individual users (overrides role permissions).

```sql
CREATE TABLE UserPermission (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    PermissionId INT NOT NULL,
    
    -- Grant or Deny
    IsGranted BIT NOT NULL, -- True = Grant, False = Deny
    
    -- Temporary Access
    ExpiresAt DATETIME2,
    
    -- Metadata
    GrantedBy INT, -- FK to User
    GrantedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Reason NVARCHAR(500),
    
    -- Foreign Keys
    FOREIGN KEY (PermissionId) REFERENCES Permission(Id) ON DELETE CASCADE,
    
    -- Indexes
    UNIQUE INDEX UX_User_Permission (UserId, PermissionId),
    INDEX IX_UserId (UserId),
    INDEX IX_ExpiresAt (ExpiresAt)
);
```

#### **PermissionAuditLog**
Tracks all permission changes.

```sql
CREATE TABLE PermissionAuditLog (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    
    -- What Changed
    EntityType NVARCHAR(50) NOT NULL, -- Role, UserRole, UserPermission
    EntityId INT NOT NULL,
    Action NVARCHAR(50) NOT NULL, -- Created, Updated, Deleted, Granted, Revoked
    
    -- Who
    ChangedBy INT NOT NULL, -- FK to User
    AffectedUserId INT, -- If user-specific change
    
    -- Details
    OldValue NVARCHAR(MAX), -- JSON
    NewValue NVARCHAR(MAX), -- JSON
    Reason NVARCHAR(500),
    
    -- When
    ChangedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Indexes
    INDEX IX_EntityType_EntityId (EntityType, EntityId),
    INDEX IX_ChangedBy (ChangedBy),
    INDEX IX_AffectedUserId (AffectedUserId),
    INDEX IX_ChangedAt (ChangedAt)
);
```

---

### **3. NOTIFICATION TABLES**

#### **NotificationSettings**
Global notification configuration.

```sql
CREATE TABLE NotificationSettings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    
    -- Email Configuration
    SmtpServer NVARCHAR(200),
    SmtpPort INT,
    SmtpUsername NVARCHAR(200),
    SmtpPassword NVARCHAR(500), -- Encrypted
    SmtpFromEmail NVARCHAR(200),
    SmtpFromName NVARCHAR(200),
    SmtpUseSsl BIT DEFAULT 1,
    
    -- SMS Configuration
    SmsProvider NVARCHAR(50), -- Twilio, etc.
    SmsAccountSid NVARCHAR(200),
    SmsAuthToken NVARCHAR(500), -- Encrypted
    SmsFromNumber NVARCHAR(20),
    
    -- In-App Configuration
    EnablePushNotifications BIT DEFAULT 1,
    ShowNotificationBadge BIT DEFAULT 1,
    PlayNotificationSound BIT DEFAULT 1,
    NotificationRetentionDays INT DEFAULT 30,
    
    -- Grouping Settings
    EnableGrouping BIT DEFAULT 1,
    DigestWindowMinutes INT DEFAULT 15,
    ThresholdModeEnabled BIT DEFAULT 1,
    ThresholdIndividualMax INT DEFAULT 2,
    ThresholdGroupMin INT DEFAULT 3,
    
    -- Metadata
    UpdatedBy INT,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Only one row allowed
    CONSTRAINT CK_SingleRow CHECK (Id = 1)
);
```

#### **NotificationTemplate**
Customizable notification templates.

```sql
CREATE TABLE NotificationTemplate (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EventType NVARCHAR(100) NOT NULL UNIQUE, -- NewApplication, PaymentReceived, etc.
    Module NVARCHAR(50) NOT NULL, -- HallRentals, CameraSystem, etc.
    
    -- Email Template
    EmailSubject NVARCHAR(200),
    EmailBody NVARCHAR(MAX), -- Supports placeholders like {applicant_name}
    
    -- SMS Template
    SmsBody NVARCHAR(500),
    
    -- In-App Template
    InAppTitle NVARCHAR(200),
    InAppBody NVARCHAR(1000),
    InAppIcon NVARCHAR(50), -- Icon name
    InAppColor NVARCHAR(20), -- Hex color
    
    -- Settings
    IsActive BIT DEFAULT 1,
    Priority NVARCHAR(20) DEFAULT 'Normal', -- Low, Normal, High, Urgent
    
    -- Metadata
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    
    -- Indexes
    INDEX IX_EventType (EventType),
    INDEX IX_Module (Module)
);
```

#### **UserNotificationPreference**
Per-user notification preferences.

```sql
CREATE TABLE UserNotificationPreference (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    EventType NVARCHAR(100) NOT NULL, -- FK to NotificationTemplate
    
    -- Channel Preferences
    EnableEmail BIT DEFAULT 1,
    EnableSms BIT DEFAULT 0,
    EnableInApp BIT DEFAULT 1,
    
    -- Scheduling
    QuietHoursEnabled BIT DEFAULT 0,
    QuietHoursStart TIME, -- e.g., 22:00
    QuietHoursEnd TIME, -- e.g., 07:00
    
    -- Grouping Override
    UseGlobalGrouping BIT DEFAULT 1,
    CustomGroupingEnabled BIT DEFAULT 0,
    
    -- Metadata
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Indexes
    UNIQUE INDEX UX_User_EventType (UserId, EventType),
    INDEX IX_UserId (UserId)
);
```

#### **NotificationQueue**
Pending notifications to be sent.

```sql
CREATE TABLE NotificationQueue (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    
    -- Target
    UserId INT NOT NULL,
    EventType NVARCHAR(100) NOT NULL,
    
    -- Channels
    SendEmail BIT DEFAULT 0,
    SendSms BIT DEFAULT 0,
    SendInApp BIT DEFAULT 0,
    
    -- Content
    Subject NVARCHAR(200),
    Body NVARCHAR(MAX),
    Data NVARCHAR(MAX), -- JSON with template variables
    
    -- Grouping
    GroupId UNIQUEIDENTIFIER, -- For grouped notifications
    IsGrouped BIT DEFAULT 0,
    
    -- Status
    Status NVARCHAR(50) DEFAULT 'Pending', -- Pending, Sent, Failed, Grouped
    AttemptCount INT DEFAULT 0,
    LastAttemptAt DATETIME2,
    ErrorMessage NVARCHAR(MAX),
    
    -- Scheduling
    ScheduledFor DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SentAt DATETIME2,
    
    -- Metadata
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Indexes
    INDEX IX_UserId (UserId),
    INDEX IX_Status (Status),
    INDEX IX_ScheduledFor (ScheduledFor),
    INDEX IX_GroupId (GroupId)
);
```

#### **NotificationHistory**
Log of all sent notifications.

```sql
CREATE TABLE NotificationHistory (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    
    -- Reference
    QueueId BIGINT, -- FK to NotificationQueue
    UserId INT NOT NULL,
    EventType NVARCHAR(100) NOT NULL,
    
    -- Delivery
    Channel NVARCHAR(20) NOT NULL, -- Email, SMS, InApp
    Status NVARCHAR(50) NOT NULL, -- Sent, Failed, Bounced
    
    -- Content
    Subject NVARCHAR(200),
    Body NVARCHAR(MAX),
    
    -- Delivery Details
    SentAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    DeliveredAt DATETIME2,
    ReadAt DATETIME2, -- For in-app notifications
    ErrorMessage NVARCHAR(MAX),
    
    -- Metadata
    ExternalId NVARCHAR(200), -- Provider message ID
    
    -- Indexes
    INDEX IX_QueueId (QueueId),
    INDEX IX_UserId (UserId),
    INDEX IX_SentAt (SentAt),
    INDEX IX_Channel (Channel)
);
```

---

### **4. SYSTEM MONITORING TABLES**

#### **DatabaseStorageMetrics**
Tracks database storage usage.

```sql
CREATE TABLE DatabaseStorageMetrics (
    Id INT PRIMARY KEY IDENTITY(1,1),
    
    -- Overall Database
    TotalSizeMB DECIMAL(10,2),
    DataSizeMB DECIMAL(10,2),
    IndexSizeMB DECIMAL(10,2),
    UnusedSpaceMB DECIMAL(10,2),
    
    -- Module-Specific
    HallRentalDataMB DECIMAL(10,2),
    AnalyticsDataMB DECIMAL(10,2),
    CameraSystemDataMB DECIMAL(10,2),
    
    -- Growth Metrics
    GrowthRateMBPerDay DECIMAL(10,2),
    ProjectedSizeIn30DaysMB DECIMAL(10,2),
    ProjectedSizeIn90DaysMB DECIMAL(10,2),
    
    -- Recommendations
    CleanupRecommended BIT DEFAULT 0,
    ArchivalRecommended BIT DEFAULT 0,
    
    -- Timestamp
    MeasuredAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Indexes
    INDEX IX_MeasuredAt (MeasuredAt)
);
```

#### **DataArchivalLog**
Tracks data archival operations.

```sql
CREATE TABLE DataArchivalLog (
    Id INT PRIMARY KEY IDENTITY(1,1),
    
    -- What Was Archived
    TableName NVARCHAR(100) NOT NULL,
    RecordsArchived INT NOT NULL,
    DateRangeStart DATE,
    DateRangeEnd DATE,
    
    -- Archive Location
    ArchiveTableName NVARCHAR(100),
    ArchiveFilePath NVARCHAR(500),
    
    -- Performance
    DurationSeconds INT,
    SpaceFreedMB DECIMAL(10,2),
    
    -- Metadata
    ArchivedBy INT, -- FK to User
    ArchivedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Indexes
    INDEX IX_TableName (TableName),
    INDEX IX_ArchivedAt (ArchivedAt)
);
```

---

## 🔗 RELATIONSHIPS DIAGRAM

```
User (existing)
  ├─── UserRole ──→ Role
  │                  └─── RolePermission ──→ Permission
  └─── UserPermission ──→ Permission

HallRentalApplication (existing)
  └─── AnalyticsHallRentalApplication

AnalyticsPageView
  └─── SessionId links to AnalyticsHallRentalVisitor

NotificationTemplate
  └─── UserNotificationPreference
       └─── NotificationQueue ──→ NotificationHistory
```

---

## 📈 INDEXES SUMMARY

**Performance-Critical Indexes:**
- All foreign keys
- Date/timestamp columns (for range queries)
- Status columns (for filtering)
- SessionId (for visitor tracking)
- UserId (for user-specific queries)

---

## 🔒 SECURITY CONSIDERATIONS

1. **Encrypted Fields:**
   - SmtpPassword
   - SmsAuthToken
   - Any API keys

2. **Soft Deletes:**
   - Use `IsActive` flags instead of hard deletes
   - Maintain audit trail

3. **Data Retention:**
   - Auto-archive analytics data > 1 year
   - Keep audit logs indefinitely
   - Notification history: 90 days

---

## 📝 MIGRATION NOTES

**For Jules:**
1. Create migration scripts in order
2. Seed default data (roles, permissions, templates)
3. Test on development database first
4. Backup production before applying

**Default Roles to Seed:**
- Admin (all permissions)
- Operations Director
- Finance Director
- Security Director
- Events Director

**Default Permissions to Seed:**
- See `03_PERMISSIONS_SPECIFICATION.md` for complete list

---

**Last Updated:** December 26, 2025  
**Version:** 1.0
