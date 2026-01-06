# Database Maintenance Feature - Quick Start Guide

## What We're Building

A comprehensive Admin-only "Database Maintenance" page that provides:

1. **Database Status Dashboard** - View environment, migration status, backup info
2. **Safe Schema Sync** - Apply EF Core migrations with confirmations
3. **Server → Laptop Workflow** - Create and download backups for local dev
4. **Laptop → Server Workflow** - Restore from backup (heavily gated, danger zone)

## Key Safety Features

✅ **Multi-step confirmations** for dangerous operations  
✅ **Typed confirmation phrases** (e.g., "APPLY MIGRATIONS", "RESTORE SERVER DATABASE")  
✅ **Automatic pre-restore backups** before any restore operation  
✅ **Operation locking** - only one operation at a time  
✅ **Comprehensive audit logging** - every action tracked  
✅ **Environment-aware** - extra safeguards in Production  
✅ **Permission-based** - fine-grained access control  

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    DatabaseMaintenance.razor                 │
│                         (UI Layer)                           │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│              IDatabaseMaintenanceService                     │
│                   (Service Layer)                            │
│  • GetStatusAsync()                                          │
│  • CreateBackupAsync()                                       │
│  • ApplyMigrationsAsync()                                    │
│  • RestoreFromBackupAsync()                                  │
│  • Operation Locking                                         │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│                  Database Layer                              │
│  • DatabaseBackups table                                     │
│  • DatabaseOperations table                                  │
│  • SystemSettings (maintenance config)                       │
│  • AuditLogs (all operations tracked)                        │
└─────────────────────────────────────────────────────────────┘
```

## Files Created So Far

### Models & DTOs
- `GFC.Core/Models/DatabaseBackup.cs`
- `GFC.Core/Models/DatabaseOperation.cs`
- `GFC.Core/DTOs/DatabaseStatusDto.cs`
- `GFC.Core/DTOs/PendingMigrationDto.cs`
- `GFC.Core/DTOs/BackupInfoDto.cs`
- `GFC.Core/DTOs/DatabaseOperationResults.cs`

### Interfaces
- `GFC.Core/Interfaces/IDatabaseMaintenanceService.cs`

### Database
- `DatabaseScripts/Manual_DatabaseMaintenance_Schema.sql`

### Updated Files
- `GFC.BlazorServer/Data/Entities/SystemSettings.cs` (added 4 properties)
- `GFC.Core/Services/AuditLogger.cs` (added 8 audit actions)
- `GFC.Data/ApplicationDbContext.cs` (added 2 DbSets + configurations)

## Next: Service Implementation

The service will handle:

1. **SQL Server Backup/Restore** using T-SQL commands
2. **EF Core Migration Detection** using `IMigrator` and `IHistoryRepository`
3. **File Operations** for backup management
4. **SHA256 Hashing** for backup integrity
5. **Operation Locking** to prevent concurrent operations
6. **Progress Reporting** for long-running operations

## User Workflow Examples

### Scenario 1: Apply Migrations After Deployment
```
1. Admin deploys new code with migrations
2. Opens Database Maintenance page
3. Sees "⚠️ DB behind – 2 pending migrations"
4. Clicks "Apply Pending Migrations"
5. Reviews list of migrations
6. Types "APPLY MIGRATIONS" to confirm
7. Migrations applied, audit logged
8. Status shows "✅ Schema up to date"
```

### Scenario 2: Refresh Laptop with Server Data
```
1. Admin opens Database Maintenance page
2. Clicks "Create Backup Now"
3. Backup created: ClubMembership_Prod_20260105_180322_Manual.bak
4. Clicks "Download Backup"
5. Downloads to laptop
6. Uses SSMS to restore to ClubMembership_DevCopy database
7. Works with current production data locally
```

### Scenario 3: Emergency Server Restore
```
1. Admin enables "Allow Server Restore Operations" in settings
2. Opens Database Maintenance page
3. Uploads or selects backup file
4. Reviews impact summary
5. Types "RESTORE SERVER DATABASE"
6. Types "THIS WILL OVERWRITE PRODUCTION DATA"
7. System creates pre-restore safety backup automatically
8. Restore executes with progress indicator
9. Sanity checks run
10. Full audit trail logged
```

## Permission Model

```
DbMaintenance.View                  → View the page
DbMaintenance.Backup.Create         → Create backups
DbMaintenance.Backup.Download       → Download backup files
DbMaintenance.Migrations.Apply      → Apply EF migrations
DbMaintenance.Restore.Execute       → Restore database (highest risk)
```

All permissions require Admin role as baseline.

## Configuration (SystemSettings)

```csharp
BackupStoragePath = "C:\\GFC_Backups\\Sql\\"  // Where backups are stored
BackupRetentionCount = 10                      // Keep last N backups
AllowServerRestoreOperations = false           // Restore toggle (off by default)
MaintenanceModeEnabled = false                 // Maintenance mode flag
```

## Audit Trail Example

Every operation creates detailed audit logs:

```
Action: DbBackupCreated
User: admin@gfc.com
Timestamp: 2026-01-05 18:03:22 UTC
Details: Backup created: ClubMembership_Prod_20260105_180322_Manual.bak (33.3 MB, SHA256: abc123...)
```

```
Action: DbMigrationsApplied
User: admin@gfc.com
Timestamp: 2026-01-05 18:15:44 UTC
Details: Applied 2 migrations: [20260105_AddBackupTables, 20260105_UpdateSystemSettings], Duration: 3s
```

```
Action: DbRestoreCompleted
User: admin@gfc.com
Timestamp: 2026-01-05 19:22:11 UTC
Details: Restored from ClubMembership_Prod_20260105_180322_Manual.bak, Pre-restore backup: ClubMembership_Prod_20260105_192205_PreRestore.bak, Duration: 12s, Sanity checks: PASSED
```

## Testing Strategy

### Unit Tests
- Backup creation logic
- Migration detection logic
- Operation lock mechanism
- File hash calculation

### Integration Tests
- Full backup workflow
- Full migration workflow
- Full restore workflow (dev only)
- Audit logging verification

### Manual Testing Checklist
- [ ] Status card displays correctly
- [ ] Backup creation works
- [ ] Backup download works
- [ ] Migration detection works
- [ ] Migration application works
- [ ] Restore workflow works (dev environment)
- [ ] Confirmations are enforced
- [ ] Progress indicators work
- [ ] Audit logs are created
- [ ] Permissions are enforced
- [ ] Operation locking prevents concurrent ops
- [ ] Maintenance mode toggle works

## Important Reminders

⚠️ **Never use this restore feature casually in production**  
⚠️ **Server DB is the source of truth**  
⚠️ **Schema changes via migrations only**  
⚠️ **No bidirectional sync/merge**  
⚠️ **Always test restore in dev first**  

## Ready to Continue?

Phase 1 (Foundation) is complete. We can now proceed with:

**Phase 2: Service Implementation** - The core business logic  
**Phase 3: UI Development** - The user interface  
**Phase 4: Integration & Testing** - Putting it all together  

Let me know when you're ready to continue!
