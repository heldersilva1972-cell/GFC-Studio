// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IStudioService
    {
        Task<StudioPage> GetPublishedPageAsync(int id);
        Task<IEnumerable<StudioPage>> GetPublishedPagesAsync();
        Task<IEnumerable<StudioPage>> GetAllPagesAsync();
        Task SaveDraftAsync(StudioDraft draft);
        Task<StudioDraft> GetDraftAsync(int pageId);
        Task PublishDraftAsync(int pageId);
        Task<StudioPage> CreatePageAsync(StudioPage page);
        Task DeletePageAsync(int id);
    }
}
