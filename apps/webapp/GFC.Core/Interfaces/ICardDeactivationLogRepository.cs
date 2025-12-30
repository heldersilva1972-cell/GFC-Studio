using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface ICardDeactivationLogRepository
    {
        Task<int> AddAsync(CardDeactivationLog log);
        Task<List<CardDeactivationLog>> GetByMemberIdAsync(int memberId);
        Task<List<CardDeactivationLog>> GetByCardIdAsync(int cardId);
        Task UpdateSyncStatusAsync(int logId, bool synced, DateTime? syncedDate);
    }
}
