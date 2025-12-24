// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IVpnProfileRepository
    {
        Task<VpnProfile> GetByIdAsync(int id);
        Task<VpnProfile> GetByUserIdAsync(int userId);
        Task<List<VpnProfile>> GetAllAsync();
        Task AddAsync(VpnProfile profile);
        Task UpdateAsync(VpnProfile profile);
        Task<string> GetNextAvailableIpAddressAsync();
    }
}
