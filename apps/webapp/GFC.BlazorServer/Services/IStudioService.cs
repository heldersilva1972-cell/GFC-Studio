using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services
{
    public interface IStudioService
    {
        Task<StudioPage> GetPageAsync(int pageId);
        Task<StudioDraft> GetLatestDraftAsync(int pageId);
        Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string createdBy, string changeDescription = null);
        Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId);
        Task PublishDraftAsync(int draftId);
        
        Task<bool> AcquireLockAsync(int pageId, string username);
        Task ReleaseLockAsync(int pageId, string username);
        Task ForceReleaseLockAsync(int pageId);
        Task<StudioLock> GetLockAsync(int pageId);
    }
}
