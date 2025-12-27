using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides read-only access to the controller + door registry.
/// </summary>
public class ControllerRegistryService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;

    public ControllerRegistryService(IDbContextFactory<GfcDbContext> contextFactory)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
    }

    public async Task<IReadOnlyList<ControllerDevice>> GetControllersAsync(bool includeDoors = true, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var query = dbContext.Controllers.AsNoTracking().AsQueryable();
        if (includeDoors)
        {
            query = query.Include(c => c.Doors.OrderBy(d => d.DoorIndex));
        }

        return await query.OrderBy(c => c.Name).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Door>> GetDoorsAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Doors
            .AsNoTracking()
            .Where(d => d.ControllerId == controllerId)
            .OrderBy(d => d.DoorIndex)
            .ToListAsync(cancellationToken);
    }

    public async Task<ControllerDevice?> GetControllerBySerialNumberAsync(uint serialNumber, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Controllers
            .AsNoTracking()
            .Include(c => c.Doors)
            .FirstOrDefaultAsync(c => c.SerialNumber == serialNumber, cancellationToken);
    }

    public async Task<ControllerDevice?> GetControllerByIdAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Controllers
            .AsNoTracking()
            .Include(c => c.Doors)
            .FirstOrDefaultAsync(c => c.Id == controllerId, cancellationToken);
    }
}

