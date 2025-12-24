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
        Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string username);
        Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId);
        Task<StudioDraft> GetDraftAsync(int draftId);
        Task<StudioDraft> GetLatestDraftAsync(int pageId);
        Task<StudioDraft> NameDraftAsync(int draftId, string name);
        Task PublishDraftAsync(int draftId);
        Task UnpublishPageAsync(int pageId);
        
        Task<StudioPage> CreatePageAsync(StudioPage page);
        Task<StudioPage> ClonePageAsync(int pageId, string newTitle);
        Task DeletePageAsync(int id);

        // Locking
        Task<bool> AcquireLockAsync(int pageId, string username);
        Task ReleaseLockAsync(int pageId, string username);
        Task ForceReleaseLockAsync(int pageId);
        Task<StudioLock> GetLockAsync(int pageId);
    }
}
