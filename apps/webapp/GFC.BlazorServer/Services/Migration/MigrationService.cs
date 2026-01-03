using System.Text.Json;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services.DataProtection;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Migration;

public class MigrationService : IMigrationService
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly IBlazorSystemSettingsService _settingsService;
    private readonly IDataProtectionService _dataProtectionService;
    private readonly IAuditLogger _auditLogger;

    public MigrationService(
        IDbContextFactory<GfcDbContext> dbFactory,
        IBlazorSystemSettingsService settingsService,
        IDataProtectionService dataProtectionService,
        IAuditLogger auditLogger)
    {
        _dbFactory = dbFactory;
        _settingsService = settingsService;
        _dataProtectionService = dataProtectionService;
        _auditLogger = auditLogger;
    }

    public async Task<List<MigrationProfile>> GetAllProfilesAsync()
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        return await db.MigrationProfiles.OrderByDescending(p => p.CreatedAtUtc).ToListAsync();
    }

    public async Task<MigrationProfile> CreateProfileAsync(string name, MigrationMode mode)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var profile = new MigrationProfile
        {
            Name = name,
            Mode = mode,
            CreatedAtUtc = DateTime.UtcNow,
            GatesStatusJson = JsonSerializer.Serialize(new Dictionary<string, GateStatus>
            {
                { "VPN", new GateStatus { IsAutomated = true } },
                { "Router", new GateStatus { IsAutomated = false } }, // Manual
                { "Backup", new GateStatus { IsAutomated = true } },
                { "Restore", new GateStatus { IsAutomated = true } },
                { "SafeMode", new GateStatus { IsAutomated = false } }
            })
        };
        db.MigrationProfiles.Add(profile);
        await db.SaveChangesAsync();
        return profile;
    }

    public async Task<MigrationProfile?> GetProfileAsync(int id)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        return await db.MigrationProfiles.FindAsync(id);
    }

    public async Task<bool> RunGateCheckAsync(int profileId, string gateKey)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var profile = await db.MigrationProfiles.FindAsync(profileId);
        if (profile == null) return false;

        var gates = JsonSerializer.Deserialize<Dictionary<string, GateStatus>>(profile.GatesStatusJson) 
                    ?? new Dictionary<string, GateStatus>();

        if (!gates.ContainsKey(gateKey)) return false;

        bool passed = false;
        string? message = null;

        try {
            switch (gateKey)
            {
                case "VPN":
                    // Check directly against DB for reliability
                    var sys = await db.SystemSettings.FirstOrDefaultAsync();
                    passed = sys?.EnforceVpn == true;
                    message = passed ? "VPN Enforced" : "VPN Enforcement is OFF";
                    break;

                case "Backup":
                    var lastBackup = await _dataProtectionService.GetLastBackupTimeAsync();
                    if (lastBackup.HasValue)
                    {
                        var hours = (DateTime.UtcNow - lastBackup.Value).TotalHours;
                        passed = hours < 24;
                        message = passed ? "Backup within 24h" : $"Last backup was {(int)hours}h ago";
                    }
                    else
                    {
                        passed = false;
                        message = "No backup recorded";
                    }
                    break;

                case "Restore":
                    var lastRestore = await _dataProtectionService.GetLastRestoreTestTimeAsync();
                    if (lastRestore.HasValue)
                    {
                        var days = (DateTime.UtcNow - lastRestore.Value).TotalDays;
                        passed = days < 30;
                        message = passed ? "Restore test within 30d" : $"Last restore test was {(int)days}d ago";
                    }
                    else
                    {
                        passed = false;
                        message = "No restore test recorded";
                    }
                    break;
                
                default:
                    return false; 
            }
        }
        catch(Exception ex)
        {
            passed = false;
            message = $"Error: {ex.Message}";
        }

        var gate = gates[gateKey];
        gate.Passed = passed;
        gate.LastCheckedUtc = DateTime.UtcNow;
        gate.Message = message;
        
        profile.GatesStatusJson = JsonSerializer.Serialize(gates);
        await db.SaveChangesAsync();
        return passed;
    }

    public async Task<bool> AcknowledgeGateAsync(int profileId, string gateKey)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var profile = await db.MigrationProfiles.FindAsync(profileId);
        if (profile == null) return false;

        var gates = JsonSerializer.Deserialize<Dictionary<string, GateStatus>>(profile.GatesStatusJson) ?? new Dictionary<string, GateStatus>();
        if (!gates.ContainsKey(gateKey)) return false;

        var gate = gates[gateKey];
        if (gate.IsAutomated) return false;

        // Toggle logic: if already passed, uncheck it? 
        // User requirements imply "Start a checklist" -> check. 
        // We'll support toggle for UX.
        gate.Passed = !gate.Passed; 
        gate.LastCheckedUtc = DateTime.UtcNow;
        gate.Message = gate.Passed ? "Manually Verified" : "Pending Verification";

        profile.GatesStatusJson = JsonSerializer.Serialize(gates);
        await db.SaveChangesAsync();
        return gate.Passed;
    }

    public async Task<bool> AttemptGoLiveAsync(int profileId)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var profile = await db.MigrationProfiles.FindAsync(profileId);
        if (profile == null) return false;

        var gates = JsonSerializer.Deserialize<Dictionary<string, GateStatus>>(profile.GatesStatusJson) ?? new Dictionary<string, GateStatus>();
        
        bool allPassed = true;
        foreach(var kvp in gates)
        {
            if (!kvp.Value.Passed)
            {
                allPassed = false;
                break;
            }
        }

        if (!allPassed) return false;

        profile.IsCompleted = true;
        
        var sys = await db.SystemSettings.FirstOrDefaultAsync();
        if (sys != null)
        {
            sys.HostingEnvironment = "Production";
        }
        
        await db.SaveChangesAsync();
        
        _auditLogger.Log("SystemDeployment", null, null, $"MigrationWizard GoLive: Profile '{profile.Name}' deployed to Production.");

        return true;
    }

    public async Task DeleteProfileAsync(int id)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var p = await db.MigrationProfiles.FindAsync(id);
        if (p != null)
        {
            db.MigrationProfiles.Remove(p);
            await db.SaveChangesAsync();
        }
    }
}
