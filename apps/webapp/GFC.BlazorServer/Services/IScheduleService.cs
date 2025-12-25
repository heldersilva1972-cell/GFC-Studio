using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;

namespace GFC.BlazorServer.Services;

public interface IScheduleService
{
    Task<List<Holiday>> GetHolidaysAsync(CancellationToken cancellationToken = default);
    Task<Holiday?> GetHolidayAsync(int id, CancellationToken cancellationToken = default);
    Task<int> AddHolidayAsync(Holiday holiday, CancellationToken cancellationToken = default);
    Task UpdateHolidayAsync(Holiday holiday, CancellationToken cancellationToken = default);
    Task DeleteHolidayAsync(int id, CancellationToken cancellationToken = default);

    Task<List<SpecialEvent>> GetSpecialEventsAsync(CancellationToken cancellationToken = default);
    Task<SpecialEvent?> GetSpecialEventAsync(int id, CancellationToken cancellationToken = default);
    Task<int> AddSpecialEventAsync(SpecialEvent specialEvent, CancellationToken cancellationToken = default);
    Task UpdateSpecialEventAsync(SpecialEvent specialEvent, CancellationToken cancellationToken = default);
    Task DeleteSpecialEventAsync(int id, CancellationToken cancellationToken = default);

    Task<List<TimeProfile>> GetProfilesAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
    Task<TimeProfile?> GetProfileAsync(int id, CancellationToken cancellationToken = default);
    Task<List<TimeProfileInterval>> GetIntervalsAsync(int profileId, CancellationToken cancellationToken = default);
    Task<int> SaveProfileAsync(TimeProfile profile, CancellationToken cancellationToken = default);
    Task DeleteProfileAsync(int id, CancellationToken cancellationToken = default);

    Task<List<ControllerTimeProfileLink>> GetControllerLinksAsync(int controllerId, CancellationToken cancellationToken = default);
    Task SaveControllerLinkAsync(int controllerId, int profileId, int controllerProfileIndex, CancellationToken cancellationToken = default);
    Task SaveLinksAsync(int controllerId, List<ControllerTimeProfileLink> links, CancellationToken cancellationToken = default);

    Task<TimeScheduleCompiledDto> CompileForControllerAsync(int controllerId, CancellationToken cancellationToken = default);
    Task<bool> SyncToControllerAsync(int controllerId, uint controllerSerialNumber, CancellationToken cancellationToken = default);
}

