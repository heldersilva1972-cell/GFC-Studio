using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services.Migration;

public interface IMigrationService
{
    Task<List<MigrationProfile>> GetAllProfilesAsync();
    Task<MigrationProfile> CreateProfileAsync(string name, MigrationMode mode);
    Task<MigrationProfile?> GetProfileAsync(int id);
    
    /// <summary>
    /// Runs validation logic for a specific gate (e.g., "VPN", "Backup").
    /// Updates the profile's GatesStatusJson state.
    /// </summary>
    Task<bool> RunGateCheckAsync(int profileId, string gateKey);

    /// <summary>
    /// Manually acknowledges a manual gate (e.g., "Router", "SafeMode").
    /// </summary>
    Task<bool> AcknowledgeGateAsync(int profileId, string gateKey);

    /// <summary>
    /// Validates all gates again and marks profile as complete if all pass.
    /// Updates HostingEnvironment to Production.
    /// </summary>
    Task<bool> AttemptGoLiveAsync(int profileId);

    Task DeleteProfileAsync(int id);
}
