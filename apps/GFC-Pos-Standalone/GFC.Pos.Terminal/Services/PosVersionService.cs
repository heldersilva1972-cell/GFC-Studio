using GFC.Core.Interfaces;

namespace GFC.Pos.Terminal.Services;

public class PosVersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => "2.1.2"; // Hardened Persistence Update
    public string GetFullVersion() => $"GFC POS Standalone v{GetRevision()}";
}
