using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

public class ReimbursementReminderWorker : BackgroundService
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly ILogger<ReimbursementReminderWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ReimbursementReminderWorker(
        IDbContextFactory<GfcDbContext> dbFactory,
        ILogger<ReimbursementReminderWorker> logger,
        IServiceProvider serviceProvider)
    {
        _dbFactory = dbFactory;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Reimbursement Reminder Worker starting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Run every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                
                await ProcessRemindersAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Reimbursement Reminder Worker");
            }
        }
    }

    private async Task ProcessRemindersAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking for aged reimbursements...");

        using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        
        // 1. Find requests that are 'Submitted' and > 14 days old
        // And we haven't sent a reminder in the last 7 days (throttle)
        var twoWeeksAgo = DateTime.UtcNow.AddDays(-14);
        var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

        var agedRequests = await db.ReimbursementRequests
            .Where(r => r.Status == "Submitted")
            .Where(r => r.CreatedUtc <= twoWeeksAgo)
            .Where(r => r.LastReminderSentUtc == null || r.LastReminderSentUtc <= oneWeekAgo)
            .ToListAsync(cancellationToken);

        if (!agedRequests.Any()) return;

        _logger.LogInformation("Found {Count} aged requests needing reminders", agedRequests.Count);

        // 2. Find managers who should receive the push
        // We look for users with permission to the 'Manage Reimbursements' or 'Mobile Reimbursement Manager' page
        // and who have ReceivePush enabled.
        var pushRecipients = await db.UserPagePermissions
            .Where(p => p.ReceivePush && p.CanAccess)
            .Where(p => p.Page.PageRoute.Contains("reimbursement") && p.Page.PageRoute.Contains("manager"))
            .Select(p => p.UserId)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (!pushRecipients.Any())
        {
            _logger.LogWarning("No managers found with ReceivePush enabled for Reimbursement Manager.");
            return;
        }

        // 3. Send notifications
        using var scope = _serviceProvider.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var memberRepository = scope.ServiceProvider.GetRequiredService<IMemberRepository>();

        foreach (var request in agedRequests)
        {
            var staffMember = memberRepository.GetMemberById(request.RequestorMemberId);
            var staffName = staffMember != null ? FormatMemberName(staffMember) : "Staff Member";
            
            var age = (DateTime.UtcNow - request.CreatedUtc).Days;
            var title = "Aged Reimbursement Alert";
            var body = $"Request #{request.Id} from {staffName} has been pending for {age} days. Total: {request.TotalAmount:C2}";
            var url = "/mobile/reimbursements/manager";

            foreach (var userId in pushRecipients)
            {
                try
                {
                    await notificationService.SendPushNotificationAsync(userId, title, body, url);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to send push reminder to user {UserId}: {Message}", userId, ex.Message);
                }
            }

            // Update reminder timestamp to throttle
            request.LastReminderSentUtc = DateTime.UtcNow;
        }

        await db.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Reminder process complete.");
    }
    private string FormatMemberName(Member member)
    {
        var first = member.FirstName?.Trim() ?? "";
        var last = member.LastName?.Trim() ?? "";
        var middle = member.MiddleName?.Trim() ?? "";
        var suffix = member.Suffix?.Trim() ?? "";

        var mFirst = first;
        var mLast = last;
        var mMiddle = string.IsNullOrWhiteSpace(middle) ? "" : " " + middle[0] + ".";
        var mSuffix = string.IsNullOrWhiteSpace(suffix) ? "" : " " + suffix;

        if (string.IsNullOrWhiteSpace(mLast)) return (mFirst + mMiddle + mSuffix).Trim();
        return $"{mLast}, {mFirst}{mMiddle}{mSuffix}".Trim().Replace("  ", " ");
    }
}


