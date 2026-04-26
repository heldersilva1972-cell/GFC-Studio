using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services;

public class PosVersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => "2.6.2"; // Hardened UI for Notice Modal (Inlined Styles)
    public string GetFullVersion() => $"GFC POS Standalone v{GetRevision()}";
}
