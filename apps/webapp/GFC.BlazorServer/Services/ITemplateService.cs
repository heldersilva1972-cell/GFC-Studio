using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services
{
    public interface ITemplateService
    {
        Task<List<StudioTemplate>> GetAllTemplatesAsync();
        Task<StudioTemplate> CreateTemplateAsync(StudioTemplate template);
        Task DeleteTemplateAsync(int id);
    }
}
