// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface ISeoService
    {
        Task<SeoSettings> GetSeoSettingsForPageAsync(int studioPageId);
        Task<SeoSettings> SaveSeoSettingsAsync(SeoSettings settings);
    }
}
