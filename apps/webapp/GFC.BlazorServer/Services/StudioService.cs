// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class StudioService : IStudioService
    {
        private readonly GfcDbContext _context;
        private readonly ILogger<StudioService> _logger;

        public StudioService(GfcDbContext context, ILogger<StudioService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<StudioPage> GetPublishedPageAsync(int id)
        {
            return await _context.StudioPages
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsPublished);
        }

        public async Task<IEnumerable<StudioPage>> GetPublishedPagesAsync()
        {
            return await _context.StudioPages
                .Where(p => p.IsPublished)
                .Include(p => p.Sections)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudioPage>> GetAllPagesAsync()
        {
            return await _context.StudioPages
                .Include(p => p.Sections)
                .ToListAsync();
        }

        public async Task SaveDraftAsync(StudioDraft draft)
        {
            if (draft.Id > 0)
            {
                _context.StudioDrafts.Update(draft);
            }
            else
            {
                _context.StudioDrafts.Add(draft);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<StudioDraft> GetDraftAsync(int pageId)
        {
            return await _context.StudioDrafts.FirstOrDefaultAsync(d => d.PageId == pageId);
        }

        public async Task PublishDraftAsync(int pageId)
        {
            var draft = await _context.StudioDrafts.FirstOrDefaultAsync(d => d.PageId == pageId);
            if (draft == null)
            {
                _logger.LogWarning("PublishDraftAsync: No draft found for PageId {PageId}", pageId);
                return; // Or throw an exception
            }

            var page = await _context.StudioPages.Include(p => p.Sections).FirstOrDefaultAsync(p => p.Id == pageId);
            if (page == null)
            {
                _logger.LogError("PublishDraftAsync: StudioPage with Id {PageId} not found.", pageId);
                throw new InvalidOperationException($"Could not find the page to publish to.");
            }

            try
            {
                var newSections = JsonSerializer.Deserialize<List<StudioSection>>(draft.ContentSnapshotJson);

                // Simple merge: Remove old sections, add new ones.
                _context.StudioSections.RemoveRange(page.Sections);
                page.Sections.Clear();

                foreach (var section in newSections)
                {
                    page.Sections.Add(new StudioSection
                    {
                        // Ensure client-side IDs are not persisted
                        ClientId = Guid.Empty,
                        Title = section.Title,
                        Content = section.Content,
                        PageIndex = section.PageIndex,
                        StudioPageId = page.Id,
                        AnimationSettings = section.AnimationSettings
                    });
                }

                // Delete the draft after publishing
                _context.StudioDrafts.Remove(draft);

                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully published draft for PageId {PageId}", pageId);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize draft content for PageId {PageId}", pageId);
                // Handle the error, maybe by not proceeding with the publish
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while publishing draft for PageId {PageId}", pageId);
                throw;
            }
        }

        public async Task<StudioPage> CreatePageAsync(StudioPage page)
        {
            _context.StudioPages.Add(page);
            await _context.SaveChangesAsync();
            return page;
        }

        public async Task DeletePageAsync(int id)
        {
            var page = await _context.StudioPages.FindAsync(id);
            if (page != null)
            {
                // Sections should be deleted automatically if CASCADE is on, 
                // but let's be safe.
                var sections = await _context.StudioSections.Where(s => s.StudioPageId == id).ToListAsync();
                _context.StudioSections.RemoveRange(sections);
                
                var drafts = await _context.StudioDrafts.Where(d => d.PageId == id).ToListAsync();
                _context.StudioDrafts.RemoveRange(drafts);

                _context.StudioPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }
    }
}
