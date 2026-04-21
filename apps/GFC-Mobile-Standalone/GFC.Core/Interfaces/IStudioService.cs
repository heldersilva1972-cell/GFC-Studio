// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IStudioService
    {
        Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string userName);
        Task PublishDraftAsync(int draftId);
        Task<StudioDraft> GetLatestDraftAsync(int pageId);
        Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId);
        Task<DesignTokenModel> GetDesignTokensAsync();
        Task SaveDesignTokensAsync(DesignTokenModel tokens);
    }
}
