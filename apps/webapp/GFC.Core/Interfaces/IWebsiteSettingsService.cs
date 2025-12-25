// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IWebsiteSettingsService
    {
        Task<WebsiteSettings> GetWebsiteSettingsAsync();
        Task UpdateWebsiteSettingsAsync(WebsiteSettings settings);
    }
}
