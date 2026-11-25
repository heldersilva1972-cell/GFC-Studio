// [NEW]
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
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
                        var boardRepository = scope.ServiceProvider.GetRequiredService<IBoardRepository>();
                        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                        var deviceTrustService = scope.ServiceProvider.GetRequiredService<IDeviceTrustService>();
                        var auditLogger = scope.ServiceProvider.GetRequiredService<IAuditLogger>();

                        var settings = await systemSettingsService.GetAsync();
                        if (settings.DirectorAccessExpiryDate.HasValue && settings.DirectorAccessExpiryDate.Value < DateTime.UtcNow)
                        {
                            var expiryDate = settings.DirectorAccessExpiryDate.Value;
                            var currentYear = DateTime.Now.Year;
                            var priorYear = currentYear - 1;

                            _logger.LogInformation("Director access expiry date ({ExpiryDate}) has passed. Evaluating outgoing directors from {PriorYear}...", expiryDate, priorYear);

                            // Outgoing directors: held assignment in priorYear, but NOT in currentYear
                            var priorDirectors = boardRepository.GetAssignmentsByYear(priorYear)
                                .Select(a => a.MemberID)
                                .Distinct()
                                .ToHashSet();

                            var currentDirectors = boardRepository.GetAssignmentsByYear(currentYear)
                                .Select(a => a.MemberID)
                                .Distinct()
                                .ToHashSet();

                            var outgoingMemberIds = priorDirectors.Where(id => !currentDirectors.Contains(id)).ToList();

                            foreach (var memberId in outgoingMemberIds)
                            {
                                try
                                {
                                    var user = userRepository.GetByMemberId(memberId);
                                    if (user != null && user.IsActive && !user.IsAdmin)
                                    {
                                        // Deactivate staff account
                                        user.IsActive = false;
                                        user.Notes = string.IsNullOrWhiteSpace(user.Notes)
                                            ? $"Deactivated on {DateTime.UtcNow:yyyy-MM-dd} (Director term expired)"
                                            : $"{user.Notes} | Deactivated on {DateTime.UtcNow:yyyy-MM-dd} (Director term expired)";

                                        userRepository.UpdateUser(user);

                                        // Revoke trusted devices/sessions
                                        await deviceTrustService.RevokeAllUserDevicesAsync(user.UserId);
                                        deviceTrustService.InvalidateUserSession(user.UserId);

                                        // Audit log
                                        auditLogger.Log(
                                            "DirectorAccessExpired",
                                            null,
                                            null,
                                            $"Account automatically deactivated due to end of director board term. MemberID: {memberId}, Term: {priorYear}",
                                            targetMemberId: memberId
                                        );

                                        _logger.LogInformation("Deactivated outgoing director user {Username} (MemberID: {MemberId})", user.Username, memberId);
                                    }
                                }
                                catch (Exception userEx)
                                {
                                    _logger.LogError(userEx, "Error deactivating outgoing director for MemberID {MemberId}", memberId);
                                }
                            }

                            // Clear expiry date once processed
                            settings.DirectorAccessExpiryDate = null;
                            await systemSettingsService.UpdateAsync(settings);
                            _logger.LogInformation("Completed director access expiry cleanup.");
                        }
                    }
                }
                catch (Exception ex) when (!(ex is OperationCanceledException))
                {
                    _logger.LogError(ex, "An error occurred while checking for director access expiry.");
                }

                // Check once every 15 minutes
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
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
