using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing system-wide settings.
/// </summary>
public class SystemSettingsService : ISystemSettingsService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<SystemSettingsService> _logger;

    public SystemSettingsService(GfcDbContext dbContext, ILogger<SystemSettingsService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<SystemSettings> GetAsync()
    {
        var settings = await _dbContext.SystemSettings.FindAsync(1);
        
        if (settings == null)
        {
            // Create default settings if none exist
            settings = new SystemSettings
            {
                Id = 1,

                LastUpdatedUtc = null
            };
            
            _dbContext.SystemSettings.Add(settings);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Created default SystemSettings");
        }
        
        return settings;
    }


}

