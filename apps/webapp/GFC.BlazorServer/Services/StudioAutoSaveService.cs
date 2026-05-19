// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
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
        private ConcurrentDictionary<int, (List<StudioSection> Sections, string UserName)> _dirtyPages = new();
 
         public StudioAutoSaveService(IServiceProvider serviceProvider, ILogger<StudioAutoSaveService> logger)
         {
             _serviceProvider = serviceProvider;
             _logger = logger;
             _timer = new Timer(DoSave, null, Timeout.Infinite, Timeout.Infinite);
         }
 
         public void MarkAsDirty(int pageId, List<StudioSection> sections, string userName)
         {
             _dirtyPages[pageId] = (sections, userName);
             _timer.Change(500, Timeout.Infinite); // Debounce for 500ms as requested
         }
 
         private async void DoSave(object? state)
         {
             try
             {
                 if (_dirtyPages.IsEmpty) return;
 
                 var pagesToSave = new Dictionary<int, (List<StudioSection> Sections, string UserName)>();
                 foreach (var pageId in _dirtyPages.Keys)
                 {
                     if (_dirtyPages.TryRemove(pageId, out var pageData))
                     {
                         pagesToSave[pageId] = pageData;
                     }
                 }
 
                 if (pagesToSave.Count == 0) return;
 
                 // Safely create scope. If host is shutting down and DI is disposed, this throws ObjectDisposedException
                 using var scope = _serviceProvider.CreateScope();
                 var context = scope.ServiceProvider.GetRequiredService<GfcDbContext>();
 
                 foreach (var entry in pagesToSave)
                 {
                     var pageId = entry.Key;
                     var (sections, userName) = entry.Value;
 
                     string contentJson;
                     lock (sections)
                     {
                         // Thread-safe serialization on background thread while sections list is locked
                         contentJson = JsonSerializer.Serialize(sections);
                     }
 
                     try
                     {
                         var lastVersion = await context.StudioDrafts
                             .Where(d => d.StudioPageId == pageId)
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
 
                         context.StudioDrafts.Add(newDraft);
                         var page = await context.StudioPages.FindAsync(pageId);
                         await context.SaveChangesAsync();
                         _logger.LogInformation($"Auto-saved draft for page {pageId}, version {newDraft.Version}");
                     }
                     catch (Exception ex)
                     {
                         _logger.LogError(ex, $"Error auto-saving draft for page {pageId}");
                         // Re-add to the dictionary to try again later
                         _dirtyPages[pageId] = (sections, userName);
                     }
                 }
             }
             catch (ObjectDisposedException)
             {
                 _logger.LogWarning("Auto-save execution skipped: Service provider scope has been disposed during host shutdown.");
             }
             catch (Exception ex)
             {
                 _logger.LogError(ex, "Fatal error caught in auto-save timer background thread.");
             }
         }

        public async Task<int> GetLatestVersionAsync(int pageId)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GfcDbContext>();
            return await context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
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


