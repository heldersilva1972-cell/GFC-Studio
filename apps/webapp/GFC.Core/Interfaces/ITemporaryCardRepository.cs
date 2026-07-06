using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface ITemporaryCardRepository
    {
        Task<TemporaryCard?> GetByIdAsync(int id);
        Task<TemporaryCard?> GetActiveByCardNumberAsync(string cardNumber);
        Task<List<TemporaryCard>> GetAllAsync();
        Task<int> AddAsync(TemporaryCard card);
        Task UpdateAsync(TemporaryCard card);
        Task DeleteAsync(int id);
    }
}
