using GFC.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for providing application versioning information.
/// Prioritizes the HIGHEST version between Database and Configuration.
/// This ensures updates propagate even if DB sync fails or Config is behind.
/// </summary>
public class VersionService : IVersionService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _scopeFactory;

    public VersionService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
    }

    public string GetDeveloper() => _configuration["ApplicationVersion:Developer"] ?? "GFC";
    public string GetYear() => _configuration["ApplicationVersion:Year"] ?? DateTime.Now.Year.ToString();
    
    public string GetRevision()
    {
        var configRev = _configuration["ApplicationVersion:Revision"] ?? "2.1.0";
        try {
            using var scope = _scopeFactory.CreateScope();
            var settingsService = scope.ServiceProvider.GetRequiredService<IBlazorSystemSettingsService>();
            var settings = settingsService.GetSettings();
            var dbRev = settings?.WebappRevision;
            
            return GetMaxVersion(configRev, dbRev);
        } catch {
            return configRev;
        }
    }

    public string GetFullVersion()
    {
        return $"{GetDeveloper()} {GetYear()} - Revision {GetRevision()}";
    }

    public string GetMobileVersion()
    {
        var configRev = _configuration["ApplicationVersion:MobileRevision"] ?? "2.1.0";
        try {
            using var scope = _scopeFactory.CreateScope();
            var settingsService = scope.ServiceProvider.GetRequiredService<IBlazorSystemSettingsService>();
            var settings = settingsService.GetSettings();
            var dbRev = settings?.MobileRevision;
            
            var revision = GetMaxVersion(configRev, dbRev);
            return $"{GetDeveloper()} {GetYear()} - Revision {revision}";
        } catch {
            return $"{GetDeveloper()} {GetYear()} - Revision {configRev}";
        }
    }

    public string GetPosVersion()
    {
        var configRev = _configuration["ApplicationVersion:PosRevision"] ?? "2.1.0";
        try {
            using var scope = _scopeFactory.CreateScope();
            var settingsService = scope.ServiceProvider.GetRequiredService<IBlazorSystemSettingsService>();
            var settings = settingsService.GetSettings();
            var dbRev = settings?.PosRevision;
            
            var revision = GetMaxVersion(configRev, dbRev);
            return $"{GetDeveloper()} {GetYear()} - Revision {revision}";
        } catch {
            return $"{GetDeveloper()} {GetYear()} - Revision {configRev}";
        }
    }

    private string GetMaxVersion(string v1, string? v2)
    {
        if (string.IsNullOrEmpty(v2)) return v1;
        
        try {
            var parts1 = v1.Split('.').Select(p => int.TryParse(p, out int v) ? v : 0).ToArray();
            var parts2 = v2.Split('.').Select(p => int.TryParse(p, out int v) ? v : 0).ToArray();
            
            for (int i = 0; i < Math.Max(parts1.Length, parts2.Length); i++) {
                int p1 = i < parts1.Length ? parts1[i] : 0;
                int p2 = i < parts2.Length ? parts2[i] : 0;
                if (p1 > p2) return v1;
                if (p2 > p1) return v2;
            }
        } catch { }
        
        return v1;
    }
}
