# Database Maintenance Feature - Progress Report

## ✅ Completed (Phase 1 & 2 Foundation)

### Data Models & Entities
- [x] Created `DatabaseBackup.cs` entity with all required properties
- [x] Created `DatabaseOperation.cs` entity for operation locking
- [x] Updated `SystemSettings.cs` with maintenance configuration properties:
  - `BackupStoragePath` (default: C:\GFC_Backups\Sql\)
  - `BackupRetentionCount` (default: 10, range 1-100)
  - `AllowServerRestoreOperations` (default: false)
  - `MaintenanceModeEnabled` (default: false)

### DTOs
- [x] Created `DatabaseStatusDto.cs` for status display
- [x] Created `PendingMigrationDto.cs` for migration information
- [x] Created `BackupInfoDto.cs` for backup listing
- [x] Created `DatabaseOperationResults.cs` with:
  - `BackupResult`
  - `MigrationResult`
  - `RestoreResult`
  - `SanityCheckResult`

### Service Interface
- [x] Created `IDatabaseMaintenanceService.cs` with complete method signatures:
  - Status methods
  - Backup methods (create, list, download, cleanup)
  - Migration methods (list pending, apply)
  - Restore methods (from backup ID or upload)
  - Operation lock methods
  - Maintenance mode methods

### Audit System
- [x] Added new audit action constants to `AuditLogger.cs`:
  - `DbBackupCreated`
  - `DbBackupDownloaded`
  - `DbMigrationsApplied`
  - `DbRestoreStarted`
  - `DbRestoreCompleted`
  - `DbRestoreFailed`
  - `DbMaintenanceModeEnabled`
  - `DbMaintenanceModeDisabled`

### Database Schema
- [x] Created `Manual_DatabaseMaintenance_Schema.sql` script with:
  - `DatabaseBackups` table with indexes
  - `DatabaseOperations` table with indexes
  - SystemSettings column additions
  - Proper foreign key constraints

### DbContext Updates
- [x] Added `DbSet<DatabaseBackup>` to ApplicationDbContext
- [x] Added `DbSet<DatabaseOperation>` to ApplicationDbContext
- [x] Configured entity relationships and constraints in `OnModelCreating`

## 🚧 Next Steps (Phase 3-5)

### Phase 3: Service Implementation
**Priority: HIGH**
- [ ] Create `DatabaseMaintenanceService.cs` implementing `IDatabaseMaintenanceService`
- [ ] Implement `GetStatusAsync()` - query DB for current state
- [ ] Implement `CreateBackupAsync()` - SQL Server BACKUP DATABASE command
- [ ] Implement `ListBackupsAsync()` - query DatabaseBackups table
- [ ] Implement `DownloadBackupAsync()` - file stream with audit logging
- [ ] Implement `GetPendingMigrationsAsync()` - use EF Core migration APIs
- [ ] Implement `ApplyMigrationsAsync()` - use EF Core Migrator
- [ ] Implement `RestoreFromBackupAsync()` - SQL Server RESTORE DATABASE command
- [ ] Implement operation lock mechanism
- [ ] Implement maintenance mode toggle
- [ ] Add comprehensive error handling and logging

### Phase 4: UI Components
**Priority: HIGH**
- [ ] Create `DatabaseMaintenance.razor` page
- [ ] Create `DatabaseMaintenance.razor.css` for styling
- [ ] Implement Database Status card (read-only display)
- [ ] Implement Schema Sync card (migrations)
- [ ] Implement Backup card (create & download)
- [ ] Implement Restore card (danger zone)
- [ ] Create confirmation dialogs with typed phrases
- [ ] Create progress indicator component
- [ ] Add user guidance section

### Phase 5: Integration & Testing
**Priority: MEDIUM**
- [ ] Update `NavMenu.razor` to add navigation link
- [ ] Register service in dependency injection
- [ ] Run database schema script on development database
- [ ] Create backup storage directory
- [ ] Test backup creation workflow
- [ ] Test migration detection and application
- [ ] Test restore workflow (dev environment only)
- [ ] Verify audit logging
- [ ] Test permission enforcement
- [ ] Test operation locking

## 📋 Before You Can Use This Feature

1. **Run the SQL Schema Script:**
   ```bash
   # Execute this file against your ClubMembership database:
   apps/webapp/DatabaseScripts/Manual_DatabaseMaintenance_Schema.sql
   ```

2. **Create Backup Directory:**
   ```powershell
   # Create the default backup directory:
   New-Item -Path "C:\GFC_Backups\Sql" -ItemType Directory -Force
   
   # Grant SQL Server service account permissions:
   # (Adjust based on your SQL Server service account)
   icacls "C:\GFC_Backups\Sql" /grant "NT SERVICE\MSSQLSERVER:(OI)(CI)F"
   ```

3. **Register Service:**
   - Add to `Program.cs` or `Startup.cs`:
   ```csharp
   builder.Services.AddScoped<IDatabaseMaintenanceService, DatabaseMaintenanceService>();
   ```

## 🔒 Security Considerations

- All operations require Admin role
- Fine-grained permissions will be enforced
- Sensitive connection strings are never displayed in UI
- All operations are audit logged
- Multi-step confirmations for dangerous operations
- Production environment has extra safeguards
- Operation locking prevents concurrent execution

## 📝 Implementation Notes

### Backup Naming Convention
Format: `ClubMembership_{Environment}_{yyyyMMdd_HHmmss}_{Type}.bak`
Example: `ClubMembership_Prod_20260105_180000_Manual.bak`

### Migration Detection
Will use EF Core's `IMigrator` and `IHistoryRepository` interfaces to:
- Get applied migrations from `__EFMigrationsHistory` table
- Compare with migrations in the application assembly
- Detect pending migrations

### Restore Safety Process
1. Validate backup file exists and is readable
2. Create pre-restore safety backup automatically
3. Optionally enable maintenance mode
4. Kill existing database connections
5. Execute RESTORE DATABASE command
6. Run sanity checks (table existence, row counts)
7. Disable maintenance mode
8. Log completion with full audit trail

### Sanity Checks
After restore, verify:
- Critical tables exist (AppUsers, Members, SystemSettings)
- SystemSettings has Id=1 record
- At least one admin user exists
- Row counts are reasonable

## 🎯 Estimated Completion Time

- **Phase 3 (Service):** 6-8 hours
- **Phase 4 (UI):** 6-8 hours
- **Phase 5 (Integration):** 2-3 hours
- **Total:** ~14-19 hours of development time

## 📞 Ready for Next Phase

The foundation is complete and solid. We're ready to proceed with:
1. **Service implementation** (most complex part)
2. **UI development** (user-facing)
3. **Testing and refinement**

Would you like me to continue with Phase 3 (Service Implementation)?
