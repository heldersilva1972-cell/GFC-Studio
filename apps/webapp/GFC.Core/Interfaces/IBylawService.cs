using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IBylawService
{
    Task<BylawDocument> GetCurrentBylawsAsync(string category = "Club");
    Task<List<BylawRevision>> GetRevisionHistoryAsync(int documentId);
    Task<BylawDocument> UpdateBylawsAsync(int documentId, string content, string changeReason, string updatedBy);
}
