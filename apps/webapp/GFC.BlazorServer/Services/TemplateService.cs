using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;

        public TemplateService(IDbContextFactory<GfcDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<StudioTemplate> GetTemplateAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.StudioTemplates.FindAsync(id);
        }

        public async Task<List<StudioTemplate>> GetAllTemplatesAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.StudioTemplates.OrderBy(t => t.Category).ThenBy(t => t.Name).ToListAsync();
        }

        public async Task<List<StudioTemplate>> GetTemplatesByCategoryAsync(string category)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            return await context.StudioTemplates
                .Where(t => t.Category == category)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task CreateTemplateAsync(StudioTemplate template)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.StudioTemplates.Add(template);
            await context.SaveChangesAsync();
        }

        public async Task UpdateTemplateAsync(StudioTemplate template)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            context.StudioTemplates.Update(template);
            await context.SaveChangesAsync();
        }

        public async Task DeleteTemplateAsync(int id)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var template = await context.StudioTemplates.FindAsync(id);
            if (template != null)
            {
                context.StudioTemplates.Remove(template);
                await context.SaveChangesAsync();
            }
        }
    }
}
