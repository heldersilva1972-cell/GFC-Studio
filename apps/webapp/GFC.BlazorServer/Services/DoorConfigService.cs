using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

public class DoorConfigService : IDoorConfigService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<DoorConfigService> _logger;

    public DoorConfigService(GfcDbContext dbContext, ILogger<DoorConfigService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<DoorConfig>> GetConfigsForControllerAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DoorConfigs
            .Include(d => d.Door)
            .Where(d => d.Door != null && d.Door.ControllerId == controllerId)
            .OrderBy(d => d.Door!.DoorIndex)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveConfigsAsync(int controllerId, IEnumerable<DoorConfig> configs, CancellationToken cancellationToken = default)
    {
        var configsList = configs.ToList();
        var now = DateTime.UtcNow;

        foreach (var config in configsList)
        {
            if (config.Id == 0)
            {
                config.CreatedUtc = now;
                config.UpdatedUtc = now;
                _dbContext.DoorConfigs.Add(config);
            }
            else
            {
                config.UpdatedUtc = now;
                _dbContext.DoorConfigs.Update(config);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<DoorConfig?> GetByDoorAsync(int doorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DoorConfigs
            .Include(d => d.Door)
            .FirstOrDefaultAsync(d => d.DoorId == doorId, cancellationToken);
    }

    public DoorConfig CreateDefaultForDoor(Door door)
    {
        return new DoorConfig
        {
            DoorId = door.Id,
            OpenTimeSeconds = 5,
            LockDelaySeconds = 0,
            AlarmEnabled = false,
            CreatedUtc = DateTime.UtcNow
        };
    }

    public ExtendedDoorConfigWriteDto MapToWriteDto(IEnumerable<DoorConfig> configs)
    {
        var doorConfigs = configs
            .Where(c => c.Door != null)
            .Select(c => new ExtendedDoorConfigWriteDto.DoorExtendedConfig
            {
                DoorNumber = c.Door!.DoorIndex,
                LockDelaySeconds = (byte)Math.Clamp(c.LockDelaySeconds, 0, 255),
                NormallyOpenMode = false,
                DoubleLock = false,
                AllowButtonOpen = true
            }).ToList();

        return new ExtendedDoorConfigWriteDto
        {
            Doors = doorConfigs
        };
    }

    public List<DoorConfig> MapFromReadDto(ExtendedConfigDto dto, IEnumerable<Door> doors)
    {
        var doorList = doors.ToList();
        var configs = new List<DoorConfig>();
        var now = DateTime.UtcNow;

        foreach (var door in doorList)
        {
            var dtoConfig = dto.Doors.FirstOrDefault(d => d.DoorNumber == door.DoorIndex);
            if (dtoConfig != null)
            {
                configs.Add(new DoorConfig
                {
                    DoorId = door.Id,
                    LockDelaySeconds = dtoConfig.LockDelaySeconds,
                    OpenTimeSeconds = 5, // Default, not in DTO
                    AlarmEnabled = false, // Not in DTO
                    CreatedUtc = now
                });
            }
        }

        return configs;
    }
}

