# Network Migration Wizard - Implementation Plan

## Status: Foundation Created ✅

## What's Been Created:

### 1. Database Model ✅
**File**: `Data/Entities/NetworkMigration.cs`
- Tracks all network migrations
- Full audit trail (who, when, what changed)
- Rollback capability with 24-hour expiry
- Status tracking (Pending → InProgress → Completed/Failed/RolledBack)

### 2. Data Models ✅
**File**: `Services/NetworkMigration/NetworkMigrationModels.cs`
- `NetworkMigrationRequest` - User's migration request
- `NetworkConfiguration` - Current/target config
- `ConnectionTestResult` - VPN/network test results
- `MigrationResult` - Migration outcome
- `ValidationResult` - Pre-flight validation
- `WireGuardConfig` - VPN config export

### 3. Service Interface ✅
**File**: `Services/NetworkMigration/INetworkMigrationService.cs`
- `DetectCurrentConfigAsync()` - Auto-detect current setup
- `ValidateNewConfigAsync()` - Validate before applying
- `TestConnectionAsync()` - Test VPN connectivity
- `ExecuteMigrationAsync()` - Perform migration
- `RollbackMigrationAsync()` - Undo migration
- `GenerateWireGuardConfigAsync()` - Export VPN config
- `GetMigrationHistoryAsync()` - View past migrations
- `CleanupExpiredBackupsAsync()` - Maintenance task

## Next Steps:

### Phase 2: Service Implementation
**File to create**: `Services/NetworkMigration/NetworkMigrationService.cs`

**Key Methods**:
```csharp
public async Task<MigrationResult> ExecuteMigrationAsync(NetworkMigrationRequest request)
{
    // 1. Validate request
    var validation = await ValidateNewConfigAsync(request);
    if (!validation.IsValid) return error;
    
    // 2. Get current config
    var currentConfig = await DetectCurrentConfigAsync(request.ControllerId);
    
    // 3. Create migration record
    var migration = new NetworkMigration { ... };
    await _db.NetworkMigrations.AddAsync(migration);
    
    // 4. Save backup configuration
    var controller = await _db.Controllers.FindAsync(request.ControllerId);
    controller.BackupIpAddress = controller.IpAddress;
    controller.BackupExpiresUtc = DateTime.UtcNow.AddHours(24);
    
    // 5. Apply new configuration
    controller.IpAddress = request.NewIpAddress;
    controller.Port = request.NewPort;
    controller.NetworkType = request.TargetNetworkType;
    controller.VpnProfileId = request.VpnProfileId;
    
    // 6. Test new connection
    var testResult = await TestConnectionAsync(request.NewIpAddress, request.NewPort, request.VpnProfileId);
    
    // 7. If test fails, rollback
    if (!testResult.Success && !request.SkipConnectionTest)
    {
        // Rollback
        controller.IpAddress = controller.BackupIpAddress;
        migration.Status = "Failed";
        return failure;
    }
    
    // 8. Mark migration complete
    migration.Status = "Completed";
    migration.CompletedUtc = DateTime.UtcNow;
    await _db.SaveChangesAsync();
    
    return success;
}
```

### Phase 3: Database Migration
**File to create**: `docs/DatabaseScripts/Add_NetworkMigration_Table.sql`

```sql
-- Add NetworkMigrations table
CREATE TABLE NetworkMigrations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ControllerId INT NOT NULL,
    MigrationType NVARCHAR(50) NOT NULL,
    PreviousNetworkType NVARCHAR(50),
    PreviousIpAddress NVARCHAR(50),
    PreviousPort INT,
    PreviousVpnProfileId INT NULL,
    NewNetworkType NVARCHAR(50),
    NewIpAddress NVARCHAR(50),
    NewPort INT,
    NewVpnProfileId INT NULL,
    InitiatedUtc DATETIME NOT NULL,
    CompletedUtc DATETIME NULL,
    Status NVARCHAR(50) NOT NULL,
    InitiatedByUser NVARCHAR(100) NOT NULL,
    TestResultsJson NVARCHAR(MAX),
    ErrorMessage NVARCHAR(MAX),
    BackupExpiresUtc DATETIME NULL,
    CanRollback BIT NOT NULL DEFAULT 1,
    Notes NVARCHAR(MAX),
    FOREIGN KEY (ControllerId) REFERENCES Controllers(Id)
);

-- Add columns to Controllers table
ALTER TABLE Controllers ADD
    NetworkType NVARCHAR(50) DEFAULT 'LAN',
    VpnProfileId INT NULL,
    BackupIpAddress NVARCHAR(50) NULL,
    BackupPort INT NULL,
    BackupExpiresUtc DATETIME NULL,
    LastMigrationUtc DATETIME NULL;

-- Add index
CREATE INDEX IX_NetworkMigrations_ControllerId ON NetworkMigrations(ControllerId);
CREATE INDEX IX_NetworkMigrations_Status ON NetworkMigrations(Status);
```

### Phase 4: UI Wizard Component
**File to create**: `Components/Pages/Admin/NetworkMigrationWizard.razor`

**Structure**:
```razor
@page "/admin/controllers/{ControllerId:int}/migrate-network"
@inject INetworkMigrationService MigrationService

<div class="wizard-container">
    @if (_currentStep == 1)
    {
        <Step1_DetectCurrent />
    }
    else if (_currentStep == 2)
    {
        <Step2_ConfigureVpn />
    }
    else if (_currentStep == 3)
    {
        <Step3_TestConnection />
    }
    else if (_currentStep == 4)
    {
        <Step4_ReviewPlan />
    }
    else if (_currentStep == 5)
    {
        <Step5_ExecuteMigration />
    }
    else if (_currentStep == 6)
    {
        <Step6_Success />
    }
</div>

@code {
    [Parameter] public int ControllerId { get; set; }
    private int _currentStep = 1;
    private NetworkConfiguration _currentConfig;
    private NetworkMigrationRequest _request;
    private ConnectionTestResult _testResult;
    private MigrationResult _migrationResult;
}
```

### Phase 5: Integration Points

**Add to Controllers page**:
```razor
<!-- In Controllers list/detail page -->
<button class="btn btn-primary" @onclick="() => NavigateToMigration(controller.Id)">
    <i class="bi bi-arrow-left-right"></i> Change Network
</button>
```

**Add to navigation** (if needed):
- Admin → Controllers → [Select Controller] → Network Settings

### Phase 6: Testing Checklist

- [ ] Create migration record in database
- [ ] Detect current LAN configuration
- [ ] Validate VPN profile selection
- [ ] Test VPN connection (ping, UDP port)
- [ ] Apply migration with backup
- [ ] Verify controller responds on new IP
- [ ] Test rollback functionality
- [ ] Verify backup expires after 24 hours
- [ ] Generate WireGuard config
- [ ] Export QR code
- [ ] View migration history

## Database Schema Changes Required:

1. Add `NetworkMigrations` table
2. Add columns to `Controllers` table:
   - `NetworkType`
   - `VpnProfileId`
   - `BackupIpAddress`
   - `BackupPort`
   - `BackupExpiresUtc`
   - `LastMigrationUtc`

3. Ensure `VpnProfiles` table has:
   - `WireGuardPublicKey`
   - `WireGuardPrivateKey`
   - `WireGuardPeerPublicKey`

## Dependencies:

- ✅ `ControllerDevice` entity (exists)
- ✅ `VpnProfile` entity (exists)
- ⬜ WireGuard key generation library (need to add)
- ⬜ QR code generation library (need to add)
- ⬜ UDP connection testing utility (may exist)

## Recommended Libraries to Add:

```xml
<!-- Add to GFC.BlazorServer.csproj -->
<PackageReference Include="WireGuard.NET" Version="1.0.0" />
<PackageReference Include="QRCoder" Version="1.4.3" />
```

## Timeline Estimate:

- Phase 2 (Service Implementation): 4-6 hours
- Phase 3 (Database Migration): 1 hour
- Phase 4 (UI Wizard): 6-8 hours
- Phase 5 (Integration): 2 hours
- Phase 6 (Testing): 3-4 hours

**Total**: ~16-21 hours of development

## Priority:

**HIGH** - This is a critical feature for operational flexibility

## Notes:

- Consider adding email notifications when migration completes
- Add Slack/Teams webhook for migration events
- Log all migrations for compliance/audit
- Consider adding "dry run" mode to preview changes
- Add automatic cleanup job for expired backups

## Current Status:

✅ **Foundation Complete** - Models, interfaces, and database schema designed
⬜ **Service Implementation** - Next step
⬜ **UI Wizard** - After service
⬜ **Testing** - Final step

**Ready for Phase 2 implementation!**
