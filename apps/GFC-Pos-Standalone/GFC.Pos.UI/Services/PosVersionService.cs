using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services;

public class PosVersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => "2.5.4"; // Offline Sync Visibility
    public string GetFullVersion() => $"GFC POS Standalone v{GetRevision()}";
}
