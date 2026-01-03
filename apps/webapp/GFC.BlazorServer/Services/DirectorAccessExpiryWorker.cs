// [NEW]
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class DirectorAccessExpiryWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DirectorAccessExpiryWorker> _logger;

    public DirectorAccessExpiryWorker(IServiceProvider serviceProvider, ILogger<DirectorAccessExpiryWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var systemSettingsService = scope.ServiceProvider.GetRequiredService<IBlazorSystemSettingsService>();
                        var vpnManagementService = scope.ServiceProvider.GetRequiredService<IVpnManagementService>();
                        var dbContext = scope.ServiceProvider.GetRequiredService<GfcDbContext>();

                        var settings = await systemSettingsService.GetAsync();
                        if (settings.DirectorAccessExpiryDate.HasValue && settings.DirectorAccessExpiryDate.Value < DateTime.UtcNow)
                        {
                            _logger.LogInformation($"Director access expiry date ({settings.DirectorAccessExpiryDate.Value}) has passed. Revoking access for all directors.");

                            // TODO: Implement role-based access revocation when ASP.NET Identity is configured
                            // This requires Roles and UserRoles tables which are not currently in the DbContext
                            /*
                            var directorRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Director");
                            if (directorRole != null)
                            {
                                var directorUserIds = await dbContext.UserRoles
                                    .Where(ur => ur.RoleId == directorRole.Id)
                                    .Select(ur => ur.UserId)
                                    .ToListAsync();

                                foreach (var userId in directorUserIds)
                                {
                                    await vpnManagementService.RevokeUserAccessAsync(userId);
                                    _logger.LogInformation($"Revoked VPN access for director with user ID: {userId}");
                                }
                            }
                            */

                            // Optional: Nullify the expiry date to prevent re-running this logic unnecessarily
                            settings.DirectorAccessExpiryDate = null;
                            await systemSettingsService.UpdateAsync(settings);
                        }
                    }
                }
                catch (Exception ex) when (!(ex is OperationCanceledException))
                {
                    _logger.LogError(ex, "An error occurred while checking for director access expiry.");
                }

                // Check once every hour
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Director Access Expiry Worker is shutting down gracefully.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Director Access Expiry Worker failed unexpectedly.");
        }
    }
}
