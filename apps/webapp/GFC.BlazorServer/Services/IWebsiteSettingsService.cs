// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IWebsiteSettingsService
    {
        Task<WebsiteSettings> GetWebsiteSettingsAsync();
        Task UpdateWebsiteSettingsAsync(WebsiteSettings settings);
    }
}
