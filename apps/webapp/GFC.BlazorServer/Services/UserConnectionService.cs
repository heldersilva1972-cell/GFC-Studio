// [NEW]
using GFC.Core.Interfaces;

namespace GFC.BlazorServer.Services
{
    public class UserConnectionService : IUserConnectionService
    {
        public string? IpAddress { get; set; }
        public LocationType LocationType { get; set; } = LocationType.Unknown;
    }
}
