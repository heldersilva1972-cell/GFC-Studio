using GFC.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GFC.Client.Services;

/// <summary>
/// Lightweight WASM-side stub for IVersionService.
/// Updated to return real revision numbers to prevent update loops.
/// </summary>
public class VersionService : IVersionService
{
    private readonly IConfiguration _configuration;

    public VersionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetDeveloper() => _configuration["ApplicationVersion:Developer"] ?? "GFC";
    public string GetYear() => _configuration["ApplicationVersion:Year"] ?? DateTime.Now.Year.ToString();
    public string GetRevision() => _configuration["ApplicationVersion:MobileRevision"] ?? _configuration["ApplicationVersion:Revision"] ?? "2.1.0";
    
    public string GetFullVersion()
    {
        return $"{GetDeveloper()} {GetYear()} - Revision {GetRevision()}";
    }

    public string GetMobileVersion() => GetFullVersion();
    public string GetPosVersion() => GetFullVersion();
}
