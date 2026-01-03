// [NEW]
using GFC.Core.Models.Security;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface ITrustedDeviceRepository
    {
        Task<TrustedDevice?> GetByTokenAsync(string token);
        Task CreateAsync(TrustedDevice device);
        Task UpdateAsync(TrustedDevice device);
        Task DeleteAsync(int id);
    }
}
