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

        public async Task<StudioPage> GetPublishedPageAsync(string slug)
        {
            return await _context.StudioPages
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.IsPublished);
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

        public async Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string username)
        {
            var lastVersion = await _context.StudioDrafts
                .Where(d => d.PageId == pageId)
                .OrderByDescending(d => d.Version)
                .Select(d => d.Version)
                .FirstOrDefaultAsync();

            var newDraft = new StudioDraft
            {
                PageId = pageId,
                ContentJson = contentJson,
                Version = lastVersion + 1,
                CreatedBy = username,
                CreatedAt = DateTime.UtcNow
            };

            _context.StudioDrafts.Add(newDraft);
            await _context.SaveChangesAsync();
            return newDraft;
        }

        public async Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId)
        {
            return await _context.StudioDrafts
                .Where(d => d.PageId == pageId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<StudioDraft> GetDraftAsync(int draftId)
        {
            return await _context.StudioDrafts.FindAsync(draftId);
        }

        public async Task<StudioDraft> GetLatestDraftAsync(int pageId)
        {
            return await _context.StudioDrafts
                .Where(d => d.PageId == pageId)
                .OrderByDescending(d => d.Version)
                .FirstOrDefaultAsync();
        }

        public async Task PublishDraftAsync(int draftId)
        {
            var draft = await _context.StudioDrafts.FindAsync(draftId);
            if (draft == null)
            {
                _logger.LogWarning("PublishDraftAsync: No draft found with Id {DraftId}", draftId);
                throw new ArgumentException($"Draft with Id {draftId} not found.");
            }

            var page = await _context.StudioPages.Include(p => p.Sections).FirstOrDefaultAsync(p => p.Id == draft.PageId);
            if (page == null)
            {
                _logger.LogError("PublishDraftAsync: StudioPage with Id {PageId} not found.", draft.PageId);
                throw new InvalidOperationException($"Could not find the page to publish to.");
            }

            try
            {
                var sectionsFromDraft = JsonSerializer.Deserialize<List<StudioSection>>(draft.ContentJson);

                // Simple merge: Remove old sections, add new ones.
                _context.StudioSections.RemoveRange(page.Sections);
                page.Sections.Clear();

                if (sectionsFromDraft != null)
                {
                    foreach (var section in sectionsFromDraft)
                    {
                        page.Sections.Add(new StudioSection
                        {
                            ClientId = Guid.Empty, // Reset client ID for persistence
                            Title = section.Title,
                            Content = section.Content,
                            PageIndex = section.PageIndex,
                            StudioPageId = page.Id,
                            AnimationSettings = section.AnimationSettings
                        });
                    }
                }

                page.IsPublished = true;
                page.LastPublishedAt = DateTime.UtcNow;
                page.Content = "Snapshot updated from draft v" + draft.Version; // Optional: Or actual summary

                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully published draft {DraftId} for PageId {PageId}", draftId, draft.PageId);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize draft content for DraftId {DraftId}", draftId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while publishing draft {DraftId}", draftId);
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
