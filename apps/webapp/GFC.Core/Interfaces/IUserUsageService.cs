using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IUserUsageService
    {
        Task TrackPageUsageAsync(int userId, string pageIdentifier);
        Task<List<string>> GetTopUsedPagesAsync(int userId, int count = 3);
    }
}
