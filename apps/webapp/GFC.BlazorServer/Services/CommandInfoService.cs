using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides access to the controller command catalogue.
/// </summary>
public class CommandInfoService
{
    private readonly GfcDbContext _dbContext;

    public CommandInfoService(GfcDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IReadOnlyList<ControllerCommandInfo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.ControllerCommandInfos
            .AsNoTracking()
            .Where(c => c.Enabled)
            .OrderBy(c => c.Category)
            .ThenBy(c => c.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public Task<ControllerCommandInfo?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key is required.", nameof(key));
        }

        return _dbContext.ControllerCommandInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Key == key, cancellationToken);
    }
}

