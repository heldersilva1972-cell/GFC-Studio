using GFC.Core.Interfaces;

namespace GFC.Mobile.Services;

public class VersionService : IVersionService
{
    public string GetDeveloper() => "GFC Tech";
    public string GetYear() => DateTime.Now.Year.ToString();
    public string GetRevision() => "1.5.2 (Dynamic Sync)"; 
    public string GetFullVersion() => "GFC Mobile Revision 1.5.2 (Dynamic Sync)";
}
