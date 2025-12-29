using System;
using System.Collections.Generic;
using System.IO;
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
                        var oldSections = context.StudioSections.Where(s => s.StudioPageId == page.Id);
                        context.StudioSections.RemoveRange(oldSections);
                        
                        // Add new sections
                        foreach(var ds in draftSections)
                        {
                            var newSection = new StudioSection
                            {
                                StudioPageId = page.Id,
                                ComponentType = ds.ComponentType,
                                OrderIndex = ds.OrderIndex,
                                ClientId = ds.ClientId,
                                
                                // Content & Properties
                                PropertiesJson = ds.PropertiesJson,
                                Content = ds.Content,
                                
                                // Styles & Design
                                StylesJson = ds.StylesJson,
                                
                                // Animations
                                AnimationSettingsJson = ds.AnimationSettingsJson,
                                
                                // Interactions
                                InteractionJson = ds.InteractionJson,
                                
                                // Data Binding
                                DataBindingJson = ds.DataBindingJson,

                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = "System (Publish)"
                            };
                            
                            // Ensure data is synced (legacy support)
                            newSection.SyncPropertiesToData();
                            
                            context.StudioSections.Add(newSection);
                        }
                    }
                }
                catch (Exception ex) 
                { 
                     // Log but allow publish state to update? 
                     // For now, rethrow to alert user
                     throw new InvalidOperationException("Failed to process draft content: " + ex.Message);
                }
            }

            // Update Page Metadata
            page.UpdatedAt = DateTime.UtcNow;
            page.Status = "Published"; // Ensure status is updated

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
