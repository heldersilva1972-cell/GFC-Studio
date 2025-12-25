// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IStudioPageRepository
    {
        Task<StudioPage> GetByIdAsync(int id);
        Task<StudioPage> GetBySlugAsync(string slug);
        Task<IEnumerable<StudioPage>> GetAllAsync();
        Task AddAsync(StudioPage page);
        Task UpdateAsync(StudioPage page);
        Task DeleteAsync(int id);
    }
}
