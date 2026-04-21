// [NEW]
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services
{
    public interface IContentIngestionService
    {
        Task<List<StudioSection>> ScrapeUrlAsync(string url);
        Task CreateRedirectAsync(string oldUrl, string newUrlSlug);
    }
}


