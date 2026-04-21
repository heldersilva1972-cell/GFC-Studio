// [NEW]
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface INavMenuService
    {
        Task<NavMenuEntry> GetNavMenuEntryAsync(int id);
        Task<IEnumerable<NavMenuEntry>> GetNavMenuEntriesAsync();
        Task CreateNavMenuEntryAsync(NavMenuEntry entry);
        Task UpdateNavMenuEntryAsync(NavMenuEntry entry);
        Task DeleteNavMenuEntryAsync(int id);
    }
}


