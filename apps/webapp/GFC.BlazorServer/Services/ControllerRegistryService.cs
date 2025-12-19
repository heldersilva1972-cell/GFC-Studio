using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides read-only access to the controller + door registry.
/// </summary>
public class ControllerRegistryService
{
    private readonly GfcDbContext _dbContext;

    public ControllerRegistryService(GfcDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IReadOnlyList<ControllerDevice>> GetControllersAsync(bool includeDoors = true, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Controllers.AsNoTracking().AsQueryable();
        if (includeDoors)
        {
            query = query.Include(c => c.Doors.OrderBy(d => d.DoorIndex));
        }

        return await query.OrderBy(c => c.Name).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Door>> GetDoorsAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Doors
            .AsNoTracking()
            .Where(d => d.ControllerId == controllerId)
            .OrderBy(d => d.DoorIndex)
            .ToListAsync(cancellationToken);
    }

    public Task<ControllerDevice?> GetControllerBySerialNumberAsync(uint serialNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.Controllers
            .AsNoTracking()
            .Include(c => c.Doors)
            .FirstOrDefaultAsync(c => c.SerialNumber == serialNumber, cancellationToken);
    }

    public Task<ControllerDevice?> GetControllerByIdAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Controllers
            .AsNoTracking()
            .Include(c => c.Doors)
            .FirstOrDefaultAsync(c => c.Id == controllerId, cancellationToken);
    }
}

