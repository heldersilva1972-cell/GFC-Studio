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
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var page = await context.StudioPages
                .Include(p => p.Sections)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == pageId);
                
            if (page != null && page.Sections != null)
            {
                foreach(var section in page.Sections)
                {
                    section.SyncDataToProperties();
                }
            }
            return page;
        }

        public async Task<StudioDraft> GetLatestDraftAsync(int pageId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.CreatedAt)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string createdBy, string changeDescription = null)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            
            // Get current version number
            var latestDraft = await context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.Version)
                .FirstOrDefaultAsync();

            var newVersion = (latestDraft?.Version ?? 0) + 1;

            var draft = new StudioDraft
            {
                StudioPageId = pageId,
                ContentSnapshotJson = contentJson,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow,
                Version = newVersion,
                IsPublished = false,
                ChangeDescription = changeDescription
            };

            context.StudioDrafts.Add(draft);
            await context.SaveChangesAsync();
            return draft;
        }

        public async Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.CreatedAt)
                .Take(20)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task PublishDraftAsync(int draftId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var draft = await context.StudioDrafts.FindAsync(draftId);
            if (draft == null) throw new InvalidOperationException("Draft not found");

            var page = await context.StudioPages
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.Id == draft.StudioPageId);
                
            if (page == null) throw new InvalidOperationException("Page not found");

            // Mark this draft as published
            draft.IsPublished = true;
            draft.PublishedAt = DateTime.UtcNow;
            
            // Sync sections from draft content to actual page sections
            if (!string.IsNullOrEmpty(draft.ContentSnapshotJson))
            {
                try 
                {
                    var draftSections = System.Text.Json.JsonSerializer.Deserialize<List<StudioSection>>(draft.ContentSnapshotJson);
                    if (draftSections != null)
                    {
                        // Remove existing sections
                        context.StudioSections.RemoveRange(page.Sections);
                        
                        // Add new sections
                        foreach(var ds in draftSections)
                        {
                            var newSection = new StudioSection
                            {
                                StudioPageId = page.Id,
                                Type = ds.sectionType ?? "Unknown",
                                OrderIndex = ds.PageIndex,
                                AnimationSettingsJson = ds.AnimationSettingsJson,
                                // Important: Sync properties to Data string for storage
                                properties = ds.properties ?? new Dictionary<string, object>()
                            };
                            
                            // Re-apply content/properties
                            if (!string.IsNullOrEmpty(ds.Content)) newSection.properties["content"] = ds.Content;
                            if (!string.IsNullOrEmpty(ds.Title)) newSection.properties["headline"] = ds.Title;
                            
                            newSection.SyncPropertiesToData();
                            context.StudioSections.Add(newSection);
                        }
                    }
                }
                catch { /* ignore bad json */ }
            }

            await context.SaveChangesAsync();
        }

        public async Task<bool> AcquireLockAsync(int pageId, string username)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var existingLock = await context.StudioLocks.FirstOrDefaultAsync(l => l.PageId == pageId);
            if (existingLock != null)
            {
                // Lock exists. Check if it's expired.
                if (DateTime.UtcNow - existingLock.LockedAt > TimeSpan.FromMinutes(5))
                {
                    // Lock is expired. Take it over.
                    existingLock.LockedBy = username;
                    existingLock.LockedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();
                    return true;
                }
                return false; // Lock is held by someone else.
            }

            var newLock = new StudioLock
            {
                PageId = pageId,
                LockedBy = username,
                LockedAt = DateTime.UtcNow
            };
            context.StudioLocks.Add(newLock);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task ReleaseLockAsync(int pageId, string username)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var existingLock = await context.StudioLocks.FirstOrDefaultAsync(l => l.PageId == pageId && l.LockedBy == username);
            if (existingLock != null)
            {
                context.StudioLocks.Remove(existingLock);
                await context.SaveChangesAsync();
            }
        }

        public async Task ForceReleaseLockAsync(int pageId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var existingLock = await context.StudioLocks.FirstOrDefaultAsync(l => l.PageId == pageId);
            if (existingLock != null)
            {
                context.StudioLocks.Remove(existingLock);
                await context.SaveChangesAsync();
            }
        }

        public async Task<StudioLock> GetLockAsync(int pageId)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.StudioLocks.FirstOrDefaultAsync(l => l.PageId == pageId);
        }
    }
}
