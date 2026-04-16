using GFC.Core.Interfaces;

namespace GFC.Client.Services;

/// <summary>
/// Lightweight WASM-side stub for IVersionService.
/// The mobile shift report uses this only for a version display badge.
/// </summary>
public class VersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => DateTime.Now.Year.ToString();
    public string GetRevision() => "WASM";
    public string GetFullVersion() => $"GFC {DateTime.Now.Year} (Mobile)";
}
