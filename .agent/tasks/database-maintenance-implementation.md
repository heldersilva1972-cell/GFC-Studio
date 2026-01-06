# Database Maintenance Feature - Implementation Plan

## Overview
This document outlines the implementation plan for the Admin-only "Database Maintenance" page that provides safe, sensible workflows for keeping the server database and development copy usable.

## Architecture Overview

### Core Components
1. **Service Layer**: `IDatabaseMaintenanceService` and implementation
2. **UI Layer**: Blazor page with modern card layout
3. **Permission System**: Fine-grained permissions for each operation
4. **Audit System**: Comprehensive logging of all operations
5. **Operation Lock**: Prevent concurrent operations
6. **Configuration**: Settings for backup paths, retention, etc.

## Implementation Phases

### Phase 1: Data Models & Entities
**Files to Create/Modify:**
- `GFC.Core/Models/DatabaseBackup.cs` - Entity for tracking backups
- `GFC.Core/Models/DatabaseOperation.cs` - Entity for operation locks
- `GFC.Core/Models/DatabaseMaintenanceAuditLog.cs` - Specialized audit log
- `GFC.BlazorServer/Data/Entities/SystemSettings.cs` - Add maintenance settings

**New Properties for SystemSettings:**
```csharp
// Database Maintenance Settings
public string BackupStoragePath { get; set; } = "C:\\GFC_Backups\\Sql\\";
public int BackupRetentionCount { get; set; } = 10;
public bool AllowServerRestoreOperations { get; set; } = false;
public bool MaintenanceModeEnabled { get; set; } = false;
```

**DatabaseBackup Entity:**
```csharp
public class DatabaseBackup
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long FileSizeBytes { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public int CreatedByUserId { get; set; }
    public string BackupType { get; set; } // "Manual", "PreRestore", "Scheduled"
    public string FileHash { get; set; } // SHA256 hash
    public bool IsDeleted { get; set; }
}
```

**DatabaseOperation Entity:**
```csharp
public class DatabaseOperation
{
    public int Id { get; set; }
    public string OperationType { get; set; } // "Backup", "Restore", "Migration"
    public string Status { get; set; } // "Running", "Completed", "Failed"
    public DateTime StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public int StartedByUserId { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ProgressLog { get; set; }
}
```

### Phase 2: Service Layer
**Files to Create:**
- `GFC.Core/Interfaces/IDatabaseMaintenanceService.cs`
- `GFC.Core/Services/DatabaseMaintenanceService.cs`
- `GFC.Core/DTOs/DatabaseStatusDto.cs`
- `GFC.Core/DTOs/PendingMigrationDto.cs`
- `GFC.Core/DTOs/BackupInfoDto.cs`

**Service Interface:**
```csharp
public interface IDatabaseMaintenanceService
{
    // Status
    Task<DatabaseStatusDto> GetStatusAsync();
    
    // Backups
    Task<BackupResult> CreateBackupAsync(int userId, string backupType = "Manual");
    Task<List<BackupInfoDto>> ListBackupsAsync();
    Task<Stream> DownloadBackupAsync(int backupId);
    Task DeleteOldBackupsAsync();
    
    // Migrations
    Task<List<PendingMigrationDto>> GetPendingMigrationsAsync();
    Task<MigrationResult> ApplyMigrationsAsync(int userId, IProgress<string> progress);
    
    // Restore
    Task<RestoreResult> RestoreFromBackupAsync(int userId, int backupId, IProgress<string> progress);
    Task<RestoreResult> RestoreFromUploadAsync(int userId, Stream fileStream, string fileName, IProgress<string> progress);
    
    // Operation Lock
    Task<bool> TryAcquireOperationLockAsync(string operationType, int userId);
    Task ReleaseOperationLockAsync(int operationId);
    Task<DatabaseOperation?> GetCurrentOperationAsync();
}
```

### Phase 3: Permission System
**Files to Modify:**
- Add new constants to existing permission system

**New Permissions:**
```csharp
public static class DbMaintenancePermissions
{
    public const string View = "DbMaintenance.View";
    public const string BackupCreate = "DbMaintenance.Backup.Create";
    public const string BackupDownload = "DbMaintenance.Backup.Download";
    public const string MigrationsApply = "DbMaintenance.Migrations.Apply";
    public const string RestoreExecute = "DbMaintenance.Restore.Execute";
}
```

### Phase 4: Audit Logging
**Files to Modify:**
- `GFC.Core/Services/AuditLogger.cs` - Add new audit actions

**New Audit Actions:**
```csharp
public const string DbBackupCreated = "DbBackupCreated";
public const string DbBackupDownloaded = "DbBackupDownloaded";
public const string DbMigrationsApplied = "DbMigrationsApplied";
public const string DbRestoreStarted = "DbRestoreStarted";
public const string DbRestoreCompleted = "DbRestoreCompleted";
public const string DbRestoreFailed = "DbRestoreFailed";
public const string DbMaintenanceModeEnabled = "DbMaintenanceModeEnabled";
public const string DbMaintenanceModeDisabled = "DbMaintenanceModeDisabled";
```

### Phase 5: UI Components
**Files to Create:**
- `GFC.BlazorServer/Components/Pages/Admin/DatabaseMaintenance.razor`
- `GFC.BlazorServer/Components/Pages/Admin/DatabaseMaintenance.razor.css`
- `GFC.BlazorServer/Components/Shared/ConfirmationDialog.razor` (if not exists)
- `GFC.BlazorServer/Components/Shared/ProgressIndicator.razor`

**Page Structure:**
```
DatabaseMaintenance.razor
├── Header (Environment Badge, Maintenance Mode Toggle)
├── Card 1: Database Status (Read-only)
│   ├── Environment Indicator
│   ├── Connection Info (masked)
│   ├── Migration Status
│   └── Last Backup Info
├── Card 2: Schema Sync (Apply Migrations)
│   ├── Pending Migrations List
│   ├── Pre-check Summary
│   ├── Confirmation Input
│   └── Apply Button
├── Card 3: Server → Laptop (Create & Download Backup)
│   ├── Create Backup Button
│   ├── Backup List
│   └── Download Buttons
├── Card 4: Laptop → Server (Restore - Danger Zone)
│   ├── Enable/Disable Toggle (based on setting)
│   ├── Upload Backup File
│   ├── Select Existing Backup
│   ├── Impact Summary
│   ├── Multi-step Confirmation
│   └── Restore Button
└── Card 5: User Guidance
    └── How to use safely instructions
```

### Phase 6: Database Schema Updates
**Files to Create:**
- `DatabaseScripts/Manual_DatabaseMaintenance_Schema.sql`

**SQL Script:**
```sql
-- DatabaseBackups table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DatabaseBackups')
BEGIN
    CREATE TABLE DatabaseBackups (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FileName NVARCHAR(255) NOT NULL,
        FilePath NVARCHAR(500) NOT NULL,
        FileSizeBytes BIGINT NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL,
        CreatedByUserId INT NOT NULL,
        BackupType NVARCHAR(50) NOT NULL,
        FileHash NVARCHAR(64) NOT NULL,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_DatabaseBackups_AppUsers FOREIGN KEY (CreatedByUserId) REFERENCES AppUsers(UserId)
    );
END

-- DatabaseOperations table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DatabaseOperations')
BEGIN
    CREATE TABLE DatabaseOperations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OperationType NVARCHAR(50) NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        StartedAtUtc DATETIME2 NOT NULL,
        CompletedAtUtc DATETIME2 NULL,
        StartedByUserId INT NOT NULL,
        ErrorMessage NVARCHAR(MAX) NULL,
        ProgressLog NVARCHAR(MAX) NULL,
        CONSTRAINT FK_DatabaseOperations_AppUsers FOREIGN KEY (StartedByUserId) REFERENCES AppUsers(UserId)
    );
END

-- Add new columns to SystemSettings
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'BackupStoragePath')
BEGIN
    ALTER TABLE SystemSettings ADD BackupStoragePath NVARCHAR(500) NOT NULL DEFAULT 'C:\GFC_Backups\Sql\';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'BackupRetentionCount')
BEGIN
    ALTER TABLE SystemSettings ADD BackupRetentionCount INT NOT NULL DEFAULT 10;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'AllowServerRestoreOperations')
BEGIN
    ALTER TABLE SystemSettings ADD AllowServerRestoreOperations BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SystemSettings') AND name = 'MaintenanceModeEnabled')
BEGIN
    ALTER TABLE SystemSettings ADD MaintenanceModeEnabled BIT NOT NULL DEFAULT 0;
END
```

### Phase 7: Navigation & Access Control
**Files to Modify:**
- `GFC.BlazorServer/Components/Layout/NavMenu.razor` - Add navigation link

**Navigation Entry:**
```razor
<NavLink class="nav-link" href="admin/maintenance/database" Match="NavLinkMatch.All">
    <span class="bi bi-database-gear" aria-hidden="true"></span> Database Maintenance
</NavLink>
```

## Security Checklist

- [ ] All operations require Admin role
- [ ] Fine-grained permissions implemented
- [ ] CSRF protection enabled
- [ ] Rate limiting on operations
- [ ] Operation lock prevents concurrent execution
- [ ] Sensitive connection strings never displayed
- [ ] All operations logged to audit system
- [ ] Multi-step confirmations for dangerous operations
- [ ] Production environment has extra safeguards
- [ ] Backup file integrity verification (SHA256)

## Testing Checklist

### Unit Tests
- [ ] Service methods for backup creation
- [ ] Service methods for migration detection
- [ ] Service methods for restore operations
- [ ] Permission validation
- [ ] Operation lock mechanism

### Integration Tests
- [ ] Full backup workflow
- [ ] Full migration workflow
- [ ] Full restore workflow
- [ ] Audit logging verification

### Manual Testing
- [ ] UI displays correct environment
- [ ] Backup creation and download
- [ ] Migration application
- [ ] Restore workflow (in dev environment)
- [ ] Permission enforcement
- [ ] Confirmation dialogs
- [ ] Progress indicators
- [ ] Error handling

## Implementation Order

1. **Day 1: Foundation**
   - Create data models
   - Update SystemSettings
   - Create database schema script
   - Run schema script

2. **Day 2: Service Layer**
   - Create service interface
   - Implement backup methods
   - Implement migration methods
   - Add audit logging

3. **Day 3: UI - Part 1**
   - Create main page structure
   - Implement Database Status card
   - Implement Schema Sync card
   - Add styling

4. **Day 4: UI - Part 2**
   - Implement Backup card
   - Implement Restore card (Danger Zone)
   - Add confirmation dialogs
   - Add progress indicators

5. **Day 5: Testing & Polish**
   - Test all workflows
   - Add error handling
   - Refine UI/UX
   - Update documentation

## Notes

### Backup Naming Convention
`ClubMembership_{Environment}_{yyyyMMdd_HHmmss}_{Type}.bak`
Example: `ClubMembership_Prod_20260105_143022_Manual.bak`

### Migration Detection
Use EF Core's `IMigrator` and `IHistoryRepository` to detect pending migrations.

### Restore Process
1. Validate backup file
2. Create pre-restore safety backup
3. Put app in maintenance mode (optional)
4. Kill existing connections
5. Restore database
6. Run sanity checks
7. Exit maintenance mode
8. Log completion

### Sanity Checks After Restore
- Verify critical tables exist
- Check row counts for key tables
- Verify SystemSettings record exists
- Verify at least one admin user exists

## User Guidance Text

**Normal Development Workflow:**
1. Work locally on your laptop
2. Publish code changes to server
3. Run migrations on server (schema only)

**Refreshing Laptop Data:**
1. Create server backup
2. Download backup file
3. Restore locally to a DEV COPY database (recommended name: `ClubMembership_DevCopy`)

**Server Restore (Emergency Only):**
1. Only use in disaster recovery or controlled rollback scenarios
2. Requires explicit enable in settings
3. Automatically creates pre-restore safety backup
4. Requires multiple typed confirmations

**Important:**
- Never use LocalDB for IIS production
- Server DB is the source of truth
- Schema changes via migrations only
- No bidirectional sync/merge
