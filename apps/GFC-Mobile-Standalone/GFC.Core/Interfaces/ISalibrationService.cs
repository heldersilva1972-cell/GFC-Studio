using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface ISalibrationService
{
    Task<SalibrationParticipant> SaveParticipantWithTicketsAsync(SalibrationParticipant participant, List<string> ticketNumbers);
    Task<SalibrationWinnerResult> LookupWinnerByTicketAsync(string ticketNumber);
    Task<List<SalibrationParticipant>> GetAllParticipantsAsync();
    Task<SalibrationSettings> GetSettingsAsync();
    Task<bool> SaveSettingsAsync(SalibrationSettings settings);
    Task<bool> DeleteParticipantAsync(int participantId);
    Task<int> GetTotalTicketCountAsync();
    Task<bool> ClearAllDataAsync();
}
