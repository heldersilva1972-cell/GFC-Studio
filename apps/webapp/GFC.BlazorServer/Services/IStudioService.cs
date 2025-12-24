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
        Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string createdBy);
        Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId);
        Task PublishDraftAsync(int draftId);
    }
}
