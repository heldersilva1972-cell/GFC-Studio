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
        Task SaveDraftAsync(StudioDraft draft);
        Task<StudioDraft> GetDraftAsync(int pageId);
        Task PublishDraftAsync(int pageId);
    }
}
