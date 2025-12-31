using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Provides access to the controller command catalogue.
/// </summary>
public class CommandInfoService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;

    public CommandInfoService(IDbContextFactory<GfcDbContext> contextFactory)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
    }

    public async Task<IReadOnlyList<ControllerCommandInfo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.ControllerCommandInfos
            .AsNoTracking()
            .Where(c => c.Enabled)
            .OrderBy(c => c.Category)
            .ThenBy(c => c.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<ControllerCommandInfo?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key is required.", nameof(key));
        }

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.ControllerCommandInfos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Key == key, cancellationToken);
    }
}

