// [NEW]
namespace GFC.Core.Interfaces
{
    public interface IUserConnectionService
    {
        string? IpAddress { get; set; }
        LocationType LocationType { get; set; }
        void SetConnectionInfo(string ipAddress, LocationType locationType);
        void DetectConnectionIfNeeded();
    }
}
