// [NEW]
using GFC.Core.Interfaces;

namespace GFC.BlazorServer.Services
{
    public class UserConnectionService : IUserConnectionService
    {
        public string? IpAddress { get; set; }
        public GFC.Core.Interfaces.LocationType LocationType { get; set; } = GFC.Core.Interfaces.LocationType.Unknown;
    }
}
