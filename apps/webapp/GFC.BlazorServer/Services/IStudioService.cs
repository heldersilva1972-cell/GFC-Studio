// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IStudioService
    {
        Task<StudioPage> GetPublishedPageAsync(int id);
        Task<StudioPage> GetPublishedPageAsync(string slug);
        Task<IEnumerable<StudioPage>> GetPublishedPagesAsync();
        Task<IEnumerable<StudioPage>> GetAllPagesAsync();
        
        // Draft/Versioning
        Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string username, string? changeDescription = null);
        Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId);
        Task<StudioDraft> GetDraftAsync(int draftId);
        Task<StudioDraft> GetLatestDraftAsync(int pageId);
        Task PublishDraftAsync(int draftId);
        
        Task<StudioPage> CreatePageAsync(StudioPage page);
        Task DeletePageAsync(int id);
    }
}
