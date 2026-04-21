using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GFC.BlazorServer.Services
{
    public class UserUsageService : IUserUsageService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;
        private readonly IMemoryCache _cache;
        private const string CacheKeyPrefix = "UserUsage_";

        public UserUsageService(IDbContextFactory<GfcDbContext> dbFactory, IMemoryCache cache)
        {
            _dbFactory = dbFactory;
            _cache = cache;
        }

        public void ClearCache()
        {
            // Note: In MemoryCache we'd normally clear specific keys if we had the list,
            // but for simplicity we'll just let them expire or rely on Track updates.
        }

        public async Task TrackPageUsageAsync(int userId, string pageIdentifier)
        {
            if (userId <= 0 || string.IsNullOrEmpty(pageIdentifier)) return;

            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                
                var usage = await db.UserPageUsage
                    .FirstOrDefaultAsync(u => u.UserId == userId && u.PageIdentifier == pageIdentifier);

                if (usage != null)
                {
                    usage.UsageCount++;
                    usage.LastUsedUtc = DateTime.UtcNow;
                }
                else
                {
                    usage = new UserPageUsage
                    {
                        UserId = userId,
                        PageIdentifier = pageIdentifier,
                        UsageCount = 1,
                        LastUsedUtc = DateTime.UtcNow
                    };
                    db.UserPageUsage.Add(usage);
                }

                await db.SaveChangesAsync();
                
                // Invalidate cache for this user since we just updated their usage
                _cache.Remove($"{CacheKeyPrefix}{userId}");
            }
            catch (Exception ex)
            {
                // Silently fail for tracking to not interrupt user flow
                Console.WriteLine($"Error tracking page usage: {ex.Message}");
            }
        }

        public async Task<List<string>> GetTopUsedPagesAsync(int userId, int count = 3)
        {
            if (userId <= 0) return new List<string>();

            string key = $"{CacheKeyPrefix}{userId}";

            // 1. Check Global Memory Cache
            if (_cache.TryGetValue(key, out List<string>? cached) && cached != null)
            {
                return cached.Take(count).ToList();
            }

            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();

                var results = await db.UserPageUsage
                    .Where(u => u.UserId == userId)
                    .OrderByDescending(u => u.UsageCount)
                    .ThenByDescending(u => u.LastUsedUtc)
                    .Take(20) // Get more than requested for cache
                    .Select(u => u.PageIdentifier)
                    .ToListAsync();

                // 2. Save to Global Cache (30 min duration)
                _cache.Set(key, results, TimeSpan.FromMinutes(30));

                return results.Take(count).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting top pages: {ex.Message}");
                return new List<string>();
            }
        }
    }
}


