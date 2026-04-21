// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface ITemplateService
    {
        Task<StudioTemplate> GetTemplateAsync(int id);
        Task<List<StudioTemplate>> GetAllTemplatesAsync();
        Task<List<StudioTemplate>> GetTemplatesByCategoryAsync(string category);
        Task CreateTemplateAsync(StudioTemplate template);
        Task UpdateTemplateAsync(StudioTemplate template);
        Task DeleteTemplateAsync(int id);
    }
}
