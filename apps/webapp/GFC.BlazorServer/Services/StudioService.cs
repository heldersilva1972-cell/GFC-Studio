// [MODIFIED]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class StudioService : IStudioService
    {
        private const string DesignTokensFilePath = "designtokens.json";
        private readonly GfcDbContext _context;
        private readonly IJSRuntime _jsRuntime;

        public StudioService(GfcDbContext context, IJSRuntime jsRuntime)
        {
            _context = context;
            _jsRuntime = jsRuntime;
        }

        public async Task<StudioDraft> SaveDraftAsync(int pageId, string contentJson, string userName)
        {
            var draft = new StudioDraft
            {
                StudioPageId = pageId,
                ContentJson = contentJson,
                CreatedBy = userName,
                CreatedAt = System.DateTime.UtcNow,
                Version = (await _context.StudioDrafts.Where(d => d.StudioPageId == pageId).CountAsync()) + 1
            };
            _context.StudioDrafts.Add(draft);
            await _context.SaveChangesAsync();
            return draft;
        }

        public async Task PublishDraftAsync(int draftId)
        {
            var draft = await _context.StudioDrafts.FindAsync(draftId);
            if (draft != null)
            {
                var page = await _context.StudioPages.FindAsync(draft.StudioPageId);
                if (page != null)
                {
                    page.ContentJson = draft.ContentJson;
                    _context.StudioPages.Update(page);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<StudioDraft> GetLatestDraftAsync(int pageId)
        {
            return await _context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.Version)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StudioDraft>> GetDraftHistoryAsync(int pageId)
        {
            return await _context.StudioDrafts
                .Where(d => d.StudioPageId == pageId)
                .OrderByDescending(d => d.Version)
                .ToListAsync();
        }

        public async Task<DesignTokenModel> GetDesignTokensAsync()
        {
            if (!File.Exists(DesignTokensFilePath))
            {
                return new DesignTokenModel();
            }

            var json = await File.ReadAllTextAsync(DesignTokensFilePath);
            return JsonSerializer.Deserialize<DesignTokenModel>(json);
        }

        public async Task SaveDesignTokensAsync(DesignTokenModel tokens)
        {
            var json = JsonSerializer.Serialize(tokens, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(DesignTokensFilePath, json);
            await _jsRuntime.InvokeVoidAsync("postThemeUpdate", tokens);
        }
    }
}
