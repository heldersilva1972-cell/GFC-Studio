using GFC.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GFC.BlazorServer.Services;

public class VersionService : IVersionService
{
    private readonly IConfiguration _configuration;

    public VersionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetDeveloper()
    {
        return _configuration["ApplicationVersion:Developer"] ?? "Unknown";
    }

    public string GetYear()
    {
        return _configuration["ApplicationVersion:Year"] ?? DateTime.Now.Year.ToString();
    }

    public string GetRevision()
    {
        return _configuration["ApplicationVersion:Revision"] ?? "1.0";
    }

    public string GetFullVersion()
    {
        var developer = GetDeveloper();
        var year = GetYear();
        var revision = GetRevision();
        return $"{developer} {year} - Revision {revision}";
    }
}

