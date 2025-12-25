// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

{
    public class PageService : IPageService
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PageService(IDbContextFactory<GfcDbContext> contextFactory, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUserName() => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        public async Task<List<StudioPage>> GetAllPagesAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.StudioPages.OrderBy(p => p.Title).ToListAsync();
        }

        public async Task<StudioPage> GetPageAsync(int pageId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.StudioPages.FindAsync(pageId);
        }

        public async Task<StudioPage> CreatePageAsync(string title, int? cloneFromPageId = null)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var newPage = new StudioPage
            {
                Title = title,
                IsPublished = false,
                // Slug will be generated on first publish or based on title
                Slug = title.ToLower().Replace(" ", "-").Replace("&", "and")
            };

            context.StudioPages.Add(newPage);
            await context.SaveChangesAsync();

            if (cloneFromPageId.HasValue)
            {
                var originalPageSections = await context.StudioSections
                    .Where(s => s.StudioPageId == cloneFromPageId.Value)
                    .ToListAsync();

                if (originalPageSections.Any())
                {
                    var newSections = originalPageSections.Select(s => new StudioSection
                    {
                        ClientId = Guid.NewGuid(),
                        Title = s.Title,
                        Content = s.Content,
                        PageIndex = s.PageIndex,
                        AnimationSettings = s.AnimationSettings,
                        StudioPageId = newPage.Id
                    }).ToList();

                    context.StudioSections.AddRange(newSections);
                    await context.SaveChangesAsync();
                }

                // Also clone the latest draft if one exists
                var latestDraft = await context.StudioDrafts
                    .Where(d => d.PageId == cloneFromPageId.Value)
                    .OrderByDescending(d => d.CreatedAt)
                    .FirstOrDefaultAsync();

                if (latestDraft != null)
                {
                    var newDraft = new StudioDraft
                    {
                        PageId = newPage.Id,
                        ContentSnapshotJson = latestDraft.ContentSnapshotJson,
                        CreatedBy = GetCurrentUserName(),
                        CreatedAt = DateTime.UtcNow
                    };
                    context.StudioDrafts.Add(newDraft);
                    await context.SaveChangesAsync();
                }
            }

            return newPage;
        }

        public async Task UpdatePageAsync(StudioPage page)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Entry(page).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeletePageAsync(int pageId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var page = await context.StudioPages.FindAsync(pageId);
            if (page != null)
            {
                context.StudioPages.Remove(page);
                await context.SaveChangesAsync(); // Cascade delete will handle sections and drafts
            }
        }
    }
}
