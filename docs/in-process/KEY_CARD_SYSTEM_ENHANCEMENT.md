# Key Card System Enhancement - Complete Implementation Plan

**Version:** 1.0.0  
**Date:** 2025-12-29  
**Status:** Planning - Ready for Implementation

---

## Executive Summary

This document outlines the complete enhancement of the GFC Key Card Management system, including automatic card lifecycle management, grace period tracking, UI modernization, and integration with the physical access controller.

---

## Table of Contents

1. [Business Rules & Logic](#business-rules--logic)
2. [Database Schema Updates](#database-schema-updates)
3. [Backend Services](#backend-services)
4. [UI Enhancements](#ui-enhancements)
5. [Controller Integration](#controller-integration)
6. [Implementation Phases](#implementation-phases)

---

## Business Rules & Logic

### Card Eligibility Rules

**Eligible Member Statuses:**
- REGULAR
- LIFE
- GUEST (currently allowed)

**Automatic Dues Satisfaction:**
- **Life Members:** Always considered as having satisfied dues
- **Directors (Board Members):** Automatically have dues waived for years they serve
- **Regular Members:** Must have paid or waived dues

### Card Activation/Deactivation Rules

**Card is ACTIVE when:**
- Member has eligible status AND
- Member has satisfied dues (paid, waived, or is Life/Director) AND
- Card has not been reported lost/damaged/replaced

**Card is DEACTIVATED when:**
- Dues become unpaid after grace period expires OR
- Member status changes to INACTIVE, SUSPENDED, or DECEASED OR
- Card is reported lost, damaged, or stolen OR
- Card is replaced with a new one

**Reactivation Rules:**
- **Dues-related deactivation:** Card reactivates INSTANTLY when dues are paid
- **Lost/Damaged cards:** NEVER reactivate (permanently retired)
- **Status-related:** Reactivate when status returns to eligible

### Grace Period Behavior

- Grace period date is set once and stays the same every year unless manually changed
- After grace period ends, unpaid members' cards are automatically deactivated
- Grace period countdown widget shows on dashboard
- Widget hides 7 days after grace period ends
- Widget reappears when new grace period is set or new year begins

### Card vs. Fob Tracking

- System tracks whether assignment is a Card or Key Fob
- Each physical card/fob has a unique number (NEVER reused)
- Track full history: replacements, losses, damages
- Metrics: Total replacements per member, total losses, etc.

---

## Database Schema Updates

### 1. KeyCards Table 🔵 **UPDATED** (Existing - Needs Updates)

```sql
ALTER TABLE dbo.KeyCards
ADD CardType NVARCHAR(10) NULL; -- 'Card' or 'Fob'

-- Add constraint
ALTER TABLE dbo.KeyCards
ADD CONSTRAINT CHK_CardType CHECK (CardType IN ('Card', 'Fob'));

-- Ensure unique card numbers
CREATE UNIQUE INDEX UX_KeyCards_CardNumber 
ON dbo.KeyCards(CardNumber);
```

### 2. MemberKeycardAssignments Table 🔵 **UPDATED** (Existing - Needs Updates)

```sql
ALTER TABLE dbo.MemberKeycardAssignments
ADD ReasonForChange NVARCHAR(50) NULL; -- 'Initial', 'Lost', 'Damaged', 'Replaced', 'Stolen', 'DuesUnpaid', 'StatusChange', 'Reactivated'

ALTER TABLE dbo.MemberKeycardAssignments
ADD DeactivationReason NVARCHAR(50) NULL; -- Reason why ToDate was set

ALTER TABLE dbo.MemberKeycardAssignments
ADD Notes NVARCHAR(500) NULL; -- Additional notes

-- Add constraint for reason codes
ALTER TABLE dbo.MemberKeycardAssignments
ADD CONSTRAINT CHK_ReasonForChange 
CHECK (ReasonForChange IN ('Initial', 'Lost', 'Damaged', 'Replaced', 'Stolen', 'DuesUnpaid', 'StatusChange', 'Reactivated', 'MemberInactive', 'MemberSuspended', 'MemberDeceased'));
```

### 3. DuesYearSettings Table ✅ **EXISTING** (Already Correct)

```sql
-- Already has:
-- Year INT
-- StandardDues DECIMAL
-- GraceEndApplied BIT
-- GraceEndDate DATETIME
```

### 4. CardDeactivationLog Table 🟢 **NEW** (Optional - For Audit Trail)

```sql
CREATE TABLE dbo.CardDeactivationLog (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    KeyCardId INT NOT NULL,
    MemberId INT NOT NULL,
    DeactivatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    Reason NVARCHAR(50) NOT NULL,
    ControllerSynced BIT NOT NULL DEFAULT 0,
    SyncedDate DATETIME NULL,
    Notes NVARCHAR(500) NULL,
    FOREIGN KEY (KeyCardId) REFERENCES dbo.KeyCards(KeyCardId),
    FOREIGN KEY (MemberId) REFERENCES dbo.Members(MemberID)
);
```

---

## Backend Services

### 1. KeyCardLifecycleService 🟢 **NEW**

**Purpose:** Manages automatic activation/deactivation of cards based on eligibility

**Methods:**

```csharp
public class KeyCardLifecycleService
{
    // Check all members and update card statuses
    Task ProcessAllMembersAsync(int year, CancellationToken ct);
    
    // Process a single member (called after dues payment)
    Task ProcessMemberAsync(int memberId, int year, CancellationToken ct);
    
    // Deactivate card with reason
    Task DeactivateCardAsync(int keyCardId, string reason, string notes, CancellationToken ct);
    
    // Reactivate card (dues-related only)
    Task ReactivateCardAsync(int keyCardId, string notes, CancellationToken ct);
    
    // Sync card status to physical controller
    Task SyncCardToControllerAsync(int keyCardId, bool activate, CancellationToken ct);
    
    // Get members at risk (unpaid, within grace period)
    Task<List<MemberAtRiskDto>> GetMembersAtRiskAsync(int year, CancellationToken ct);
}
```

### 2. CardLifecycleBackgroundService 🟢 **NEW**

**Purpose:** Background worker that runs daily to check eligibility and deactivate cards

```csharp
public class CardLifecycleBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Run daily at 2:00 AM
        // Check grace period status
        // Deactivate cards for unpaid members
        // Log all actions
    }
}
```

### 3. Existing Services 🔵 **UPDATED**

**DuesService.cs:**
```csharp
// After recording payment, trigger card reactivation
public async Task RecordPaymentAsync(...)
{
    // ... existing code ...
    
    // NEW: Trigger card reactivation
    await _keyCardLifecycleService.ProcessMemberAsync(memberId, year, ct);
}
```

**KeyCardAdminService.cs:**
```csharp
// Update to track card type and reason
public async Task AssignCardAsync(int memberId, string cardNumber, string cardType, string notes, string source)
{
    // ... validate ...
    
    // Create card with type
    var card = _keyCardRepository.Create(cardNumber, memberId, cardType, notes);
    
    // Create assignment with reason
    var assignment = new MemberKeycardAssignment
    {
        MemberId = memberId,
        KeyCardId = card.KeyCardId,
        FromDate = DateTime.Now,
        ReasonForChange = "Initial",
        Notes = notes
    };
    
    // Sync to controller
    await _lifecycleService.SyncCardToControllerAsync(card.KeyCardId, true, ct);
}
```

---

## UI Enhancements

### 1. Dues Page 🔵 **UPDATED** (`/dues`)

**Current Issues:**
- Basic table layout
- Not visually clear who is at risk
- No card assignment integration

**Enhancements:**

**A. Visual Improvements:**
- Add status badges with icons (✅ Paid, ⏳ Unpaid, 🛡️ Waived, 👔 Director, ⭐ Life)
- Color-coded rows (green = paid, yellow = grace period, red = overdue)
- Progress indicators for grace period countdown
- Improved typography and spacing

**B. Grace Period Indicator:**
```
┌─────────────────────────────────────────────────────┐
│ 🟡 Grace Period: 45 days remaining (Mar 31, 2025)  │
│ ⚠️ 12 members will lose access if dues unpaid      │
│ 🟢 NEW                                              │
└─────────────────────────────────────────────────────┘
```

**C. After Payment Flow:**
```
[Payment Recorded] ✅
    ↓
[Does member have a card?]
    ↓ NO
[Prompt: "Assign key card/fob now?"]
    ↓ YES
[Open Card Reader Modal]
    ↓
[Scan Card → Select Type (Card/Fob) → Confirm]
    ↓
[Card Activated & Synced to Controller] ✅
```

**D. Enhanced Table Columns:**
- Name
- Status (with badge)
- Dues Status (Paid/Unpaid/Waived with icon)
- Amount
- Paid Date
- Days Overdue (if applicable)
- **Card Status** (Has Card / No Card / Deactivated)
- **Controller Sync** (✅ Synced / ⏳ Pending / ⚠️ Failed)
- Actions (Record Payment / Assign Card)

**E. Controller Sync Status Banner:** 🟢 **NEW**
```
┌─────────────────────────────────────────────────────┐
│ ⚠️ Controller Sync Alert                            │
│ 3 cards pending sync to controller                  │
│ Last successful sync: 5 minutes ago                 │
│ [View Sync Queue] [Retry All]                       │
│ 🟢 NEW                                              │
└─────────────────────────────────────────────────────┘
```
*Only shown when there are pending or failed syncs*

### 2. Key Cards Management Page 🔵 **UPDATED** (`/keycards`)

**Current State:**
- Shows existing card assignments
- Basic card management

**Enhancements:**

**A. Page Header with Grace Period & Controller Status:**
```
┌──────────────────────────────────────────────────────────┐
│ Key Card Management                                      │
│                                                          │
│ Grace Period Configuration                    🟢 NEW    │
│ ┌────────────────────────────────────────────────────┐  │
│ │ Grace End Date: [Mar 31, 2025] [Edit] [Save]      │  │
│ │ 🟡 45 days remaining                               │  │
│ │ ⚠️ 12 members at risk                              │  │
│ └────────────────────────────────────────────────────┘  │
│                                                          │
│ Controller Sync Status                        🟢 NEW    │
│ ┌────────────────────────────────────────────────────┐  │
│ │ Controller: ✅ Online                              │  │
│ │ Pending Syncs: 3                                   │  │
│ │ Failed Syncs: 0                                    │  │
│ │ Last Sync: 2 minutes ago                           │  │
│ │ [View Queue] [Retry All] [Force Full Sync]        │  │
│ └────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────┘
```

**B. Dashboard Metrics:**
```
┌──────────────┬──────────────┬──────────────┬──────────────┐
│ Total Cards  │ Active       │ Inactive     │ At Risk      │
│     156      │    144       │     12       │     12       │
└──────────────┴──────────────┴──────────────┴──────────────┘
```

**C. Enhanced Card List:** 🔵 **UPDATED**

Each card shows:
- Card Number
- Type (Card 💳 or Fob 🔑)
- Member Name
- Status (Active/Inactive with color)
- Assigned Date
- Last Used (if available)
- Actions:
  - View Details
  - Replace Card/Fob
  - Deactivate/Reactivate
  - Report Lost/Damaged

**D. Member Search & Filter:**
- Search by name, card number, member ID
- Filter by:
  - Status (Active/Inactive/At Risk)
  - Type (Card/Fob)
  - Eligibility (Eligible/Ineligible)

**E. Assign/Update Card Modal:** 🟢 **NEW**
```
┌─────────────────────────────────────────────┐
│ Assign Key Card/Fob                  🟢 NEW│
│                                             │
│ Step 1: Select Member                       │
│ [Search: Type member name...]               │
│ [Dropdown: Eligible members only]           │
│                                             │
│ Step 2: Scan Card/Fob                       │
│ [Waiting for scan...] 📡                    │
│ Card Number: 123456789                      │
│                                             │
│ Step 3: Select Type                         │
│ ○ Card 💳    ○ Fob 🔑                       │
│                                             │
│ Notes: [Optional notes...]                  │
│                                             │
│ [Cancel] [Assign & Activate]                │
└─────────────────────────────────────────────┘
```

**F. Card Detail View:** 🔵 **UPDATED**
```
┌─────────────────────────────────────────────┐
│ Card #123456789 (Fob 🔑)             🔵 UPD │
│                                             │
│ Status: 🟢 Active                           │
│ Controller Sync: ✅ Synced        🟢 NEW   │
│ Member: John Silva (#1234)                  │
│ Assigned: Jan 15, 2025                      │
│ Last Used: Today at 8:30 AM                 │
│                                             │
│ History:                          🟢 NEW   │
│ • Jan 15, 2025 - Initial Assignment         │
│ • Feb 1, 2025 - Deactivated (Dues Unpaid)  │
│ • Feb 5, 2025 - Reactivated (Payment)      │
│                                             │
│ [Replace] [Report Lost] [Deactivate]       │
└─────────────────────────────────────────────┘
```

### 3. Dashboard Widget: Grace Period Countdown 🟢 **NEW**

**Widget Design:**
```
┌─────────────────────────────────┐
│ 🟡 Grace Period Countdown       │
│                                 │
│        45 DAYS                  │
│     Until March 31, 2025        │
│                                 │
│  ⚠️ 12 members at risk          │
│                                 │
│  [View Details →]               │
└─────────────────────────────────┘
```

**Color Coding:**
- 🟢 Green: > 30 days
- 🟡 Yellow: 15-30 days
- 🟠 Orange: 7-14 days
- 🔴 Red: < 7 days
- ⚫ Gray: Ended (shows for 7 days then hides)

**Widget Behavior:**
- Clickable → navigates to `/keycards`
- Configurable in Quick Action Customization
- Enabled by default
- Auto-hides 7 days after grace period ends
- Reappears when new grace period is set

---

## Controller Integration

### Physical Controller Sync

**When to Sync:**

1. **Card Assigned:** Send `AddOrUpdatePrivilegeAsync()` to controller
2. **Card Activated:** Send `AddOrUpdatePrivilegeAsync()` to controller
3. **Card Deactivated:** Send `DeletePrivilegeAsync()` to controller
4. **Card Replaced:** Delete old, add new

```csharp
public async Task SyncCardToControllerAsync(int keyCardId, bool activate, CancellationToken ct)
{
    var card = await _keyCardRepository.GetByIdAsync(keyCardId);
    if (card == null) return;
    
    var controller = await _controllerRegistry.GetPrimaryControllerAsync();
    if (controller == null) 
    {
        _logger.LogWarning("No controller configured for card sync");
        return;
    }
    
    if (activate)
    {
        // Add card to controller
        var privilege = new CardPrivilegeModel
        {
            CardNumber = long.Parse(card.CardNumber),
            // ... set door permissions based on member eligibility
        };
        
        await _controllerClient.AddOrUpdatePrivilegeAsync(privilege, ct);
        _logger.LogInformation("Card {CardNumber} activated on controller", card.CardNumber);
    }
    else
    {
        // Remove card from controller
        await _controllerClient.DeletePrivilegeAsync(long.Parse(card.CardNumber), ct);
        _logger.LogInformation("Card {CardNumber} deactivated on controller", card.CardNumber);
    }
}
```

### Controller Offline Handling 🟢 **NEW**

**Problem:** What happens if the controller is offline or unreachable when trying to sync a card?

**Solution:** Implement a **Sync Queue with Retry Mechanism**

#### 1. ControllerSyncQueue Table 🟢 **NEW**

```sql
CREATE TABLE dbo.ControllerSyncQueue (
    QueueId INT IDENTITY(1,1) PRIMARY KEY,
    KeyCardId INT NOT NULL,
    CardNumber NVARCHAR(50) NOT NULL,
    Action NVARCHAR(20) NOT NULL, -- 'ACTIVATE' or 'DEACTIVATE'
    QueuedDate DATETIME NOT NULL DEFAULT GETDATE(),
    AttemptCount INT NOT NULL DEFAULT 0,
    LastAttemptDate DATETIME NULL,
    LastError NVARCHAR(500) NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'PENDING', -- 'PENDING', 'PROCESSING', 'COMPLETED'
    CompletedDate DATETIME NULL,
    FOREIGN KEY (KeyCardId) REFERENCES dbo.KeyCards(KeyCardId)
);

CREATE INDEX IX_SyncQueue_Status ON dbo.ControllerSyncQueue(Status);
CREATE INDEX IX_SyncQueue_NextRetry ON dbo.ControllerSyncQueue(LastAttemptDate, AttemptCount) 
    WHERE Status = 'PENDING';
```

**Note:** No 'FAILED' status - syncs remain 'PENDING' until successful

#### 2. Enhanced Sync Method with Queue

```csharp
public async Task SyncCardToControllerAsync(int keyCardId, bool activate, CancellationToken ct)
{
    var card = await _keyCardRepository.GetByIdAsync(keyCardId);
    if (card == null) return;
    
    try
    {
        var controller = await _controllerRegistry.GetPrimaryControllerAsync();
        if (controller == null) 
        {
            _logger.LogWarning("No controller configured - queueing sync for later");
            await QueueSyncAsync(keyCardId, card.CardNumber, activate);
            return;
        }
        
        // Try to sync immediately
        if (activate)
        {
            var privilege = new CardPrivilegeModel
            {
                CardNumber = long.Parse(card.CardNumber),
                // ... set door permissions
            };
            
            await _controllerClient.AddOrUpdatePrivilegeAsync(privilege, ct);
            _logger.LogInformation("Card {CardNumber} activated on controller", card.CardNumber);
        }
        else
        {
            await _controllerClient.DeletePrivilegeAsync(long.Parse(card.CardNumber), ct);
            _logger.LogInformation("Card {CardNumber} deactivated on controller", card.CardNumber);
        }
        
        // Success - mark database as synced
        await MarkCardSyncedAsync(keyCardId, activate);
    }
    catch (Exception ex)
    {
        // Controller is offline or error occurred
        _logger.LogError(ex, "Failed to sync card {CardNumber} to controller - queueing for retry", card.CardNumber);
        await QueueSyncAsync(keyCardId, card.CardNumber, activate, ex.Message);
        
        // Don't throw - card status is updated in database, will sync later
    }
}

private async Task QueueSyncAsync(int keyCardId, string cardNumber, bool activate, string error = null)
{
    await _syncQueueRepository.AddAsync(new ControllerSyncQueueItem
    {
        KeyCardId = keyCardId,
        CardNumber = cardNumber,
        Action = activate ? "ACTIVATE" : "DEACTIVATE",
        QueuedDate = DateTime.Now,
        LastError = error,
        Status = "PENDING"
    });
}
```

#### 3. Background Sync Worker

```csharp
public class ControllerSyncWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ControllerSyncWorker> _logger;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var syncQueue = scope.ServiceProvider.GetRequiredService<IControllerSyncQueueRepository>();
                var controllerClient = scope.ServiceProvider.GetRequiredService<IControllerClient>();
                
                // Get pending items
                var pendingItems = await syncQueue.GetPendingItemsAsync();
                
                foreach (var item in pendingItems)
                {
                    // Calculate next retry time based on attempt count
                    var nextRetryTime = CalculateNextRetryTime(item.AttemptCount, item.LastAttemptDate);
                    
                    // Skip if not time to retry yet
                    if (DateTime.Now < nextRetryTime)
                        continue;
                    
                    try
                    {
                        // Mark as processing
                        await syncQueue.UpdateStatusAsync(item.QueueId, "PROCESSING");
                        
                        // Attempt sync
                        if (item.Action == "ACTIVATE")
                        {
                            var privilege = new CardPrivilegeModel
                            {
                                CardNumber = long.Parse(item.CardNumber),
                                // ... set permissions
                            };
                            await controllerClient.AddOrUpdatePrivilegeAsync(privilege, stoppingToken);
                        }
                        else
                        {
                            await controllerClient.DeletePrivilegeAsync(long.Parse(item.CardNumber), stoppingToken);
                        }
                        
                        // Success - mark as completed
                        await syncQueue.MarkAsCompletedAsync(item.QueueId);
                        _logger.LogInformation("Successfully synced queued item {QueueId} for card {CardNumber} (attempt {Attempt})", 
                            item.QueueId, item.CardNumber, item.AttemptCount + 1);
                    }
                    catch (Exception ex)
                    {
                        // Failed - increment attempt count and log error
                        // NEVER give up - just keep retrying
                        await syncQueue.IncrementAttemptAsync(item.QueueId, ex.Message);
                        _logger.LogWarning(ex, "Failed to sync queued item {QueueId} (attempt {Attempt}) - will retry in 30 minutes", 
                            item.QueueId, item.AttemptCount + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in controller sync worker");
            }
            
            // Run every 5 minutes to check for items ready to retry
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
    
    private DateTime CalculateNextRetryTime(int attemptCount, DateTime? lastAttemptDate)
    {
        if (lastAttemptDate == null)
            return DateTime.Now; // First attempt - immediate
        
        TimeSpan delay = attemptCount switch
        {
            0 => TimeSpan.Zero,              // Immediate
            1 => TimeSpan.FromMinutes(5),    // 5 minutes
            2 => TimeSpan.FromMinutes(15),   // 15 minutes
            _ => TimeSpan.FromMinutes(30)    // 30 minutes forever
        };
        
        return lastAttemptDate.Value.Add(delay);
    }
}
```

#### 4. UI Indicators

**Key Cards Page - Sync Status:**
```
┌─────────────────────────────────────────────┐
│ Card #123456789 (Fob 🔑)                    │
│                                             │
│ Status: 🟢 Active                           │
│ Controller Sync: ⚠️ Pending                 │
│ (Will retry automatically)                  │
│                                             │
│ [Force Sync Now]                            │
└─────────────────────────────────────────────┘
```

**Admin Dashboard - Sync Queue Status:**
```
┌──────────────────────────────────────┐
│ Controller Sync Status               │
│                                      │
│ ✅ Controller Online                 │
│ 📋 Pending Syncs: 3                  │
│ ⚠️ Failed Syncs: 0                   │
│                                      │
│ [View Queue] [Retry All]             │
└──────────────────────────────────────┘
```

#### 5. Retry Strategy

**Exponential Backoff → Steady Retry:**
- Attempt 1: Immediate
- Attempt 2: 5 minutes later
- Attempt 3: 15 minutes later
- Attempt 4+: **Every 30 minutes (FOREVER until successful)**

**Key Points:**
- System **NEVER gives up** - keeps retrying indefinitely
- After initial backoff, settles into 30-minute retry cycle
- Admin can see retry count and next retry time
- Admin can manually force retry at any time
- When controller comes back online, all pending syncs process automatically

**Dashboard Display:**
```
┌──────────────────────────────────────┐
│ Controller Sync Status               │
│                                      │
│ Pending Syncs: 5                     │
│                                      │
│ Card #12345678 (John Silva)          │
│ • Attempts: 47                       │
│ • Next retry: In 12 minutes          │
│ • Queued: 1 day ago                  │
│                                      │
│ Card #87654321 (Jane Doe)            │
│ • Attempts: 3                        │
│ • Next retry: In 2 minutes           │
│ • Queued: 15 minutes ago             │
│                                      │
│ [Retry All Now] [View Full Queue]   │
└──────────────────────────────────────┘
```

**Manual Intervention:**
- Admin can view sync queue
- Admin can manually retry any item at any time
- Admin can force full sync if needed
- System logs all sync attempts for audit

#### 6. Pending Sync Management

**What Happens with Long-Running Pending Syncs:**

1. **System Behavior:**
   - Sync remains in `PENDING` status indefinitely
   - Retries every 30 minutes automatically
   - Logs each attempt with error details
   - **Never marks as permanently failed**
   - Card status in database is always correct (source of truth)
   - Controller may be out of sync until successful

2. **Admin Visibility:**
```
┌──────────────────────────────────────────────────────┐
│ ⚠️ Long-Running Sync Alert                           │
│                                                      │
│ Card #123456789 has been pending for 2 days         │
│ Member: John Silva (#1234)                          │
│ Action: ACTIVATE                                     │
│ Attempts: 96                                         │
│ Last Error: Connection timeout                      │
│ Next Retry: In 18 minutes                           │
│                                                      │
│ Database Status: ACTIVE                              │
│ Controller Status: UNKNOWN (likely inactive)         │
│                                                      │
│ [Retry Now] [Force Full Sync] [View Details]        │
└──────────────────────────────────────────────────────┘
```

3. **Recovery Options:**

**Option A: Wait for Automatic Retry**
```
System continues retrying every 30 minutes
No action needed - will sync when controller is back online
Admin can monitor progress on dashboard
```

**Option B: Manual Retry Now**
```
Admin Actions:
1. Navigate to Key Cards → Controller Sync Status
2. Click "Retry All Now" or select specific items
3. System attempts immediate sync
4. If successful → Marked as completed
5. If fails → Continues 30-minute retry cycle
```

**Option C: Force Full Sync (Nuclear Option)**
```
When to use: Controller was offline for extended period, 
             or database/controller are completely out of sync

Admin Actions:
1. Navigate to Key Cards → Controller Sync Status
2. Click "Force Full Sync"
3. System shows confirmation:
   "This will sync ALL active cards to the controller.
    This may take several minutes. Continue?"
4. Click "Yes, Sync All"
5. System:
   - Gets all active cards from database
   - Clears controller card list
   - Uploads all active cards to controller
   - Marks all pending syncs as completed
   - Logs full sync event
```

**Option D: Individual Card Sync**
```
Admin Actions:
1. Navigate to specific card detail page
2. See sync status: "⏳ Pending (47 attempts)"
3. Click "Force Sync Now"
4. System attempts immediate sync
5. Shows result: Success or continues retrying
```

#### 7. Sync Queue Management Page 🟢 **NEW**

**New Admin Page: `/keycards/sync-queue`**

```
┌──────────────────────────────────────────────────────────┐
│ Controller Sync Queue                                    │
│                                                          │
│ Filters: [All] [Pending] [Completed]                    │
│ Date Range: [Last 7 Days ▼]                             │
│ Sort By: [Oldest First ▼]                               │
│                                                          │
│ ┌────────────────────────────────────────────────────┐  │
│ │ Card #    Member      Action    Status  Attempts  │  │
│ │ ────────────────────────────────────────────────── │  │
│ │ ☑ 12345678  John Silva  ACTIVATE  ⏳ Pending  47  │  │
│ │   Last Error: Connection timeout                   │  │
│ │   Queued: 2 days ago                               │  │
│ │   Next Retry: In 18 minutes                        │  │
│ │   [Retry Now] [View Details]                       │  │
│ │                                                    │  │
│ │ ☑ 87654321  Jane Doe    DEACTIVATE ⏳ Pending  3  │  │
│ │   Last Error: Network unreachable                  │  │
│ │   Queued: 45 minutes ago                           │  │
│ │   Next Retry: In 2 minutes                         │  │
│ │   [Retry Now] [View Details]                       │  │
│ │                                                    │  │
│ │ □ 11223344  Bob Smith   ACTIVATE  ✅ Complete     │  │
│ │   Synced: 5 minutes ago                            │  │
│ └────────────────────────────────────────────────────┘  │
│                                                          │
│ Bulk Actions:                                            │
│ [Retry Selected] [Retry All Pending] [Clear Completed]  │
│ [Force Full Sync] [Export Log]                          │
└──────────────────────────────────────────────────────────┘
```

**Alert Thresholds:**
- 🟡 **Yellow Alert:** Pending > 1 hour (show on dashboard)
- 🟠 **Orange Alert:** Pending > 6 hours (email notification)
- 🔴 **Red Alert:** Pending > 24 hours (urgent email notification)

#### 8. Controller Health Monitoring 🟢 **NEW**

**Real-time Controller Status:**

```csharp
public class ControllerHealthService
{
    // Ping controller every 30 seconds
    public async Task<ControllerHealthStatus> GetHealthAsync()
    {
        try
        {
            var controller = await _registry.GetPrimaryControllerAsync();
            if (controller == null)
                return new ControllerHealthStatus { IsOnline = false, Message = "No controller configured" };
            
            var pingResult = await _client.PingAsync(CancellationToken.None);
            
            return new ControllerHealthStatus
            {
                IsOnline = pingResult,
                LastPing = DateTime.Now,
                Message = pingResult ? "Online" : "Offline",
                PendingSyncs = await _syncQueue.GetPendingCountAsync(),
                FailedSyncs = await _syncQueue.GetFailedCountAsync()
            };
        }
        catch (Exception ex)
        {
            return new ControllerHealthStatus
            {
                IsOnline = false,
                Message = $"Error: {ex.Message}",
                LastPing = DateTime.Now
            };
        }
    }
}
```

**Dashboard Widget:**
```
┌──────────────────────────────────┐
│ 🎛️ Access Controller             │
│                                  │
│ Status: 🔴 OFFLINE               │
│ Last Seen: 15 minutes ago        │
│                                  │
│ ⚠️ 5 cards pending sync          │
│ ⚠️ 2 cards failed sync           │
│                                  │
│ [View Details] [Troubleshoot]   │
└──────────────────────────────────┘
```

#### 9. Automatic Recovery 🟢 **NEW**

**When controller comes back online:**

```csharp
// In ControllerSyncWorker
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        // Check controller health
        var health = await _healthService.GetHealthAsync();
        
        if (health.IsOnline && _wasOffline)
        {
            // Controller just came back online!
            _logger.LogInformation("Controller reconnected - processing sync queue");
            
            // Process ALL pending items (not just new ones)
            await ProcessSyncQueueAsync(includeRecentlyFailed: true);
            
            _wasOffline = false;
        }
        else if (!health.IsOnline)
        {
            _wasOffline = true;
        }
        
        // Normal queue processing...
    }
}
```

#### 10. Notification System 🟢 **NEW**

**Admin Notifications:**

1. **Email Alert (Critical):**
   - Sent when sync fails 10 times
   - Sent when controller goes offline
   - Daily digest of pending/failed syncs

2. **Dashboard Badge:**
   - Red badge on Key Cards menu item
   - Shows count of failed syncs
   - Clickable → goes to sync queue

3. **System Log:**
   - All sync attempts logged
   - Searchable by card number, member, date
   - Export to CSV for analysis

---

## Implementation Phases

### Phase 1: Database & Backend (Week 1)
- [ ] Update database schema (add CardType, ReasonForChange, etc.)
- [ ] Create `KeyCardLifecycleService`
- [ ] Create `CardLifecycleBackgroundService`
- [ ] Update `KeyCardAdminService` for card type tracking
- [ ] Update `DuesService` to trigger card reactivation
- [ ] Add controller sync methods
- [ ] Write unit tests

### Phase 2: Dues Page Modernization (Week 1-2)
- [ ] Redesign Dues page layout
- [ ] Add grace period indicator banner
- [ ] Improve table styling and status badges
- [ ] Add "Assign Card" prompt after payment
- [ ] Integrate card reader modal
- [ ] Add card status column to table
- [ ] Test payment → card assignment flow

### Phase 3: Key Cards Page Enhancement (Week 2)
- [ ] Add grace period configuration section
- [ ] Add dashboard metrics
- [ ] Enhance card list with type indicators
- [ ] Add search and filter functionality
- [ ] Create assign/update card modal
- [ ] Create card detail view
- [ ] Add replace/report lost functionality
- [ ] Test all card management workflows

### Phase 4: Dashboard Widget (Week 2-3)
- [ ] Create Grace Period Countdown widget component
- [ ] Implement color-coded countdown logic
- [ ] Add to Quick Action Customization options
- [ ] Implement auto-hide/show logic
- [ ] Add click navigation to Key Cards page
- [ ] Test widget lifecycle

### Phase 5: Testing & Refinement (Week 3)
- [ ] End-to-end testing of all workflows
- [ ] Test automatic deactivation (simulate grace period end)
- [ ] Test controller sync
- [ ] Performance testing
- [ ] User acceptance testing
- [ ] Bug fixes and refinements

### Phase 6: Documentation & Training (Week 3-4)
- [ ] Update user documentation
- [ ] Create admin guide for grace period management
- [ ] Create training materials
- [ ] Document controller integration
- [ ] Add TODO placeholder for member notifications

---

## Future Enhancements (Not in Scope)

- [ ] Email notifications to members before card deactivation
- [ ] SMS notifications
- [ ] Card inventory tracking
- [ ] Automated reporting (members who lose cards frequently)
- [ ] Mobile app integration for card assignment
- [ ] Biometric integration

---

## Success Criteria

✅ Directors and Life members automatically eligible for cards  
✅ Cards automatically deactivate after grace period for unpaid dues  
✅ Cards instantly reactivate when dues are paid  
✅ Grace period is configurable and visible  
✅ Dashboard widget shows countdown and affected members  
✅ Dues page is modern, clear, and efficient  
✅ Key Cards page supports full card lifecycle management  
✅ Card type (Card vs. Fob) is tracked  
✅ Full history of card assignments is maintained  
✅ Physical controller stays in sync with database  

---

## Notes

- **Member Notifications:** Placeholder added for future implementation
- **Card Reader:** Reuses existing card reader test functionality from Controller Settings
- **Grace Period:** Rolling date that stays the same unless manually changed
- **Unique Card Numbers:** Enforced at database level, never reused
- **Controller Sync:** Real-time sync when cards are activated/deactivated

---

**End of Document**
