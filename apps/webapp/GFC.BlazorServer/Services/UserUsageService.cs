using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services
{
    public class UserUsageService : IUserUsageService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;

        public UserUsageService(IDbContextFactory<GfcDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
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

            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();

                return await db.UserPageUsage
                    .Where(u => u.UserId == userId)
                    .OrderByDescending(u => u.UsageCount)
                    .ThenByDescending(u => u.LastUsedUtc)
                    .Take(count)
                    .Select(u => u.PageIdentifier)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting top pages: {ex.Message}");
                return new List<string>();
            }
        }
    }
}
