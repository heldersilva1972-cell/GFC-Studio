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

namespace GFC.BlazorServer.Services
{
    using Microsoft.AspNetCore.Http;

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

        public async Task<StudioPage> CreatePageAsync(string title, string folder = "/", int? cloneFromPageId = null)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();

            var baseSlug = title.ToLower().Replace(" ", "-").Replace("&", "and").Replace(".", "-");
            var slug = baseSlug;
            int counter = 1;
            
            while (await context.StudioPages.AnyAsync(p => p.Slug == slug))
            {
                slug = $"{baseSlug}-{counter++}";
            }

            var newPage = new StudioPage
            {
                Title = title,
                Folder = folder,
                IsPublished = false,
                Status = "Draft",
                Slug = slug,
                CreatedBy = GetCurrentUserName(),
                UpdatedBy = GetCurrentUserName(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
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
                        ComponentType = s.ComponentType,
                        Data = s.Data,
                        OrderIndex = s.OrderIndex,
                        AnimationSettingsJson = s.AnimationSettingsJson,
                        StylesJson = s.StylesJson,
                        InteractionJson = s.InteractionJson,
                        DataBindingJson = s.DataBindingJson,
                        IsVisible = s.IsVisible,
                        VisibleOnDesktop = s.VisibleOnDesktop,
                        VisibleOnTablet = s.VisibleOnTablet,
                        VisibleOnMobile = s.VisibleOnMobile,
                        CreatedBy = GetCurrentUserName(),
                        UpdatedBy = GetCurrentUserName(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        StudioPageId = newPage.Id
                    }).ToList();

                    context.StudioSections.AddRange(newSections);
                    await context.SaveChangesAsync();
                }

                // Also clone the latest draft if one exists
                var latestDraft = await context.StudioDrafts
                    .Where(d => d.StudioPageId == cloneFromPageId.Value)
                    .OrderByDescending(d => d.CreatedAt)
                    .FirstOrDefaultAsync();

                if (latestDraft != null)
                {
                    var newDraft = new StudioDraft
                    {
                        StudioPageId = newPage.Id,
                        ContentSnapshotJson = latestDraft.ContentSnapshotJson,
                        ChangeDescription = $"Cloned from {cloneFromPageId}",
                        Version = 1,
                        IsPublished = false,
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
