using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services
{
    public class StudioService : IStudioService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;

        public StudioService(IDbContextFactory<GfcDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<StudioPage> GetPageAsync(int pageId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.StudioPages
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.Id == pageId);
        }

        public async Task<StudioDraft> GetLatestDraftAsync(int pageId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string createdBy)
        {
            using var context = _dbContextFactory.CreateDbContext();
            
            // Get current version number
            var latestDraft = await context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.Version)
                .FirstOrDefaultAsync();

            var newVersion = (latestDraft?.Version ?? 0) + 1;

            var draft = new StudioDraft
            {
                StudioPageId = pageId,
                ContentJson = contentJson,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                Version = newVersion,
                IsPublished = false
            };

            context.StudioDrafts.Add(draft);
            await context.SaveChangesAsync();
            return draft;
        }

        public async Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.CreatedAt)
                .Take(20)
                .ToListAsync();
        }

        public async Task PublishDraftAsync(int draftId)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var draft = await context.StudioDrafts.FindAsync(draftId);
            if (draft == null) throw new InvalidOperationException("Draft not found");

            var page = await context.StudioPages.FindAsync(draft.StudioPageId);
            if (page == null) throw new InvalidOperationException("Page not found");

            // Mark this draft as published
            draft.IsPublished = true;
            draft.PublishedAt = DateTime.UtcNow;

            // TODO: In a real implementation, this would trigger a rebuild or cache invalidation
            // For now, we just update the database state
            
            await context.SaveChangesAsync();
        }
    }
}
