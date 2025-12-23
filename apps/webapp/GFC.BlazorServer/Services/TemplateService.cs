// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly GfcDbContext _context;

        public TemplateService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<StudioTemplate> GetTemplateAsync(int id)
        {
            return await _context.StudioTemplates.FindAsync(id);
        }

        public async Task<List<StudioTemplate>> GetAllTemplatesAsync()
        {
            return await _context.StudioTemplates.ToListAsync();
        }

        public async Task<List<StudioTemplate>> GetTemplatesByCategoryAsync(string category)
        {
            return await _context.StudioTemplates
                .Where(t => t.Category == category)
                .ToListAsync();
        }

        public async Task CreateTemplateAsync(StudioTemplate template)
        {
            _context.StudioTemplates.Add(template);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTemplateAsync(StudioTemplate template)
        {
            _context.Entry(template).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTemplateAsync(int id)
        {
            var template = await _context.StudioTemplates.FindAsync(id);
            if (template != null)
            {
                _context.StudioTemplates.Remove(template);
                await _context.SaveChangesAsync();
            }
        }
    }
}
