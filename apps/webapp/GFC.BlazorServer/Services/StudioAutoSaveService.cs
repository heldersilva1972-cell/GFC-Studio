// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IStudioAutoSaveService : IDisposable
    {
        void MarkAsDirty(int pageId, List<StudioSection> sections, string userName);
        Task<int> GetLatestVersionAsync(int pageId);
    }

    public class StudioAutoSaveService : IStudioAutoSaveService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StudioAutoSaveService> _logger;
        private Timer _timer;
        private ConcurrentDictionary<int, (string Content, string UserName)> _dirtyPages = new();

        public StudioAutoSaveService(IServiceProvider serviceProvider, ILogger<StudioAutoSaveService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _timer = new Timer(DoSave, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void MarkAsDirty(int pageId, List<StudioSection> sections, string userName)
        {
            var contentJson = JsonSerializer.Serialize(sections);
            _dirtyPages[pageId] = (contentJson, userName);
            _timer.Change(2000, Timeout.Infinite); // Debounce for 2 seconds
        }

        private async void DoSave(object? state)
        {
            if (_dirtyPages.IsEmpty) return;

            var pagesToSave = new Dictionary<int, (string Content, string UserName)>();
            foreach (var pageId in _dirtyPages.Keys)
            {
                if (_dirtyPages.TryRemove(pageId, out var pageData))
                {
                    pagesToSave[pageId] = pageData;
                }
            }

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GfcDbContext>();

            foreach (var entry in pagesToSave)
            {
                var pageId = entry.Key;
                var (contentJson, userName) = entry.Value;

                try
                {
                    var lastVersion = await context.Drafts
                        .Where(d => d.PageId == pageId)
                        .OrderByDescending(d => d.Version)
                        .Select(d => d.Version)
                        .FirstOrDefaultAsync();

                    var newDraft = new StudioDraft
                    {
                        PageId = pageId,
                        ContentJson = contentJson,
                        Version = lastVersion + 1,
                        CreatedBy = userName,
                        CreatedAt = DateTime.UtcNow
                    };

                    context.Drafts.Add(newDraft);
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"Auto-saved draft for page {pageId}, version {newDraft.Version}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error auto-saving draft for page {pageId}");
                    // Re-add to the dictionary to try again later
                    _dirtyPages[pageId] = contentJson;
                }
            }
        }

        public async Task<int> GetLatestVersionAsync(int pageId)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Drafts
                .Where(d => d.PageId == pageId)
                .OrderByDescending(d => d.Version)
                .Select(d => d.Version)
                .FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
