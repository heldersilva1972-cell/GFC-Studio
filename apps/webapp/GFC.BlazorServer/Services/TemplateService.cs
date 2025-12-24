using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;

        public TemplateService(IDbContextFactory<GfcDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<StudioTemplate>> GetAllTemplatesAsync()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.StudioTemplates.ToListAsync();
        }

        public async Task<StudioTemplate> CreateTemplateAsync(StudioTemplate template)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.StudioTemplates.Add(template);
            await context.SaveChangesAsync();
            return template;
        }

        public async Task DeleteTemplateAsync(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var template = await context.StudioTemplates.FindAsync(id);
            if (template != null)
            {
                context.StudioTemplates.Remove(template);
                await context.SaveChangesAsync();
            }
        }
    }
}
