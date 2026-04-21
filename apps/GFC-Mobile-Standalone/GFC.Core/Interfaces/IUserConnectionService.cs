using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IUserConnectionService
    {
        string? IpAddress { get; set; }
        LocationType LocationType { get; set; }
        bool IsMobile { get; set; }
        void SetConnectionInfo(string ipAddress, LocationType locationType);
        void DetectConnectionIfNeeded();
        Task DetectConnectionIfNeededAsync();
    }
}
