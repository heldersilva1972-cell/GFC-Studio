using GFC.Core.Interfaces;
using System.Reflection;
using System.Text.Json;

namespace GFC.Mobile.Services;

public class VersionService : IVersionService
{
    private readonly string _revision;
    private readonly string _fullVersion;

    public VersionService()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("version.json");
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                var doc = JsonDocument.Parse(json);
                _revision = doc.RootElement.GetProperty("version").GetString() ?? "Unknown";
            }
            else
            {
                _revision = "2.1.100-dev";
            }
        }
        catch
        {
            _revision = "2.1.100-err";
        }
        
        _fullVersion = $"GFC 2026 - Revision {_revision}";
    }

    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => _revision;
    public string GetFullVersion() => _fullVersion;
}
