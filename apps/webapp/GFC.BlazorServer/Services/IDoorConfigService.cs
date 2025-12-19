using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services.Models;

namespace GFC.BlazorServer.Services;

public interface IDoorConfigService
{
    Task<List<DoorConfig>> GetConfigsForControllerAsync(int controllerId, CancellationToken cancellationToken = default);
    Task<DoorConfig?> GetByDoorAsync(int doorId, CancellationToken cancellationToken = default);
    Task SaveConfigsAsync(int controllerId, IEnumerable<DoorConfig> configs, CancellationToken cancellationToken = default);
    DoorConfig CreateDefaultForDoor(Door door);
    ExtendedDoorConfigWriteDto MapToWriteDto(IEnumerable<DoorConfig> configs);
    List<DoorConfig> MapFromReadDto(ExtendedConfigDto dto, IEnumerable<Door> doors);
}

