using GFC.Core.Interfaces;

namespace GFC.Mobile.Services;

public class VersionService : IVersionService
{
    public string GetDeveloper() => "GFC";
    public string GetYear() => "2026";
    public string GetRevision() => "2.1.36"; 
    public string GetFullVersion() => "GFC 2026 - Revision 2.1.36";
}
