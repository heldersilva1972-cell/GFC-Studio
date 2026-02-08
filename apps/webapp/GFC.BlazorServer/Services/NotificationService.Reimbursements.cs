using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

public static class ReimbursementNotificationService
{
    public static async Task SendOnSubmittedAsync(
        ReimbursementRequest request,
        IMemberRepository memberRepository,
        GfcDbContext dbContext,
        INotificationService notificationService,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var requestor = memberRepository.GetMemberById(request.RequestorMemberId);
            if (requestor == null) return;

            var template = await LoadTemplateAsync("ReimbursementSubmitted.txt");
            if (string.IsNullOrWhiteSpace(template)) return;

            var body = template
                .Replace("{RequestorName}", $"{requestor.FirstName} {requestor.LastName}")
                .Replace("{RequestId}", request.Id.ToString())
                .Replace("{RequestDate}", request.RequestDate.ToString("MMM d, yyyy"))
                .Replace("{TotalAmount}", request.TotalAmount.ToString("C2"))
                .Replace("{ItemCount}", request.Items.Count.ToString());

            // Get notification recipients from settings
            var settings = await dbContext.ReimbursementSettings.FirstOrDefaultAsync(cancellationToken);
            var recipients = new List<string>();

            if (settings != null && !string.IsNullOrWhiteSpace(settings.NotificationRecipients))
            {
                var recipientIds = settings.NotificationRecipients
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Where(id => int.TryParse(id, out _))
                    .Select(int.Parse)
                    .ToList();

                foreach (var recipientId in recipientIds)
                {
                    var recipient = memberRepository.GetMemberById(recipientId);
                    if (recipient != null && !string.IsNullOrWhiteSpace(recipient.Email))
                    {
                        recipients.Add(recipient.Email);
                    }
                }
            }

            // [PUSH] Notify managers with ReceivePush enabled
            var managerIds = await dbContext.UserPagePermissions
                .Where(p => p.ReceivePush && p.CanAccess)
                .Where(p => p.Page.PageRoute.Contains("reimbursement") && p.Page.PageRoute.Contains("manager"))
                .Select(p => p.UserId)
                .Distinct()
                .ToListAsync(cancellationToken);

            foreach (var managerId in managerIds)
            {
                await notificationService.SendPushNotificationAsync(
                    managerId, 
                    "New Reimbursement", 
                    $"A new request for {request.TotalAmount:C2} has been submitted by {requestor.FirstName}.",
                    "/mobile/reimbursements/manager");
            }

            logger.LogInformation("Sent submission notifications for request {RequestId} to {ManagerCount} managers via Push", request.Id, managerIds.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send submission notification for reimbursement request {RequestId}", request.Id);
        }
    }

    public static async Task SendOnApprovedAsync(
        ReimbursementRequest request,
        IMemberRepository memberRepository,
        INotificationService notificationService,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var requestor = memberRepository.GetMemberById(request.RequestorMemberId);
            if (requestor == null || string.IsNullOrWhiteSpace(requestor.Email)) return;

            var template = await LoadTemplateAsync("ReimbursementApproved.txt");
            if (string.IsNullOrWhiteSpace(template)) return;

            var body = template
                .Replace("{RequestorName}", $"{requestor.FirstName} {requestor.LastName}")
                .Replace("{RequestId}", request.Id.ToString())
                .Replace("{TotalAmount}", request.TotalAmount.ToString("C2"))
                .Replace("{ApprovedDate}", request.ApprovedDateUtc?.ToString("MMM d, yyyy") ?? "N/A");

            await notificationService.SendPushNotificationAsync(
                request.RequestorMemberId, 
                "Reimbursement Approved", 
                $"Your request for {request.TotalAmount:C2} has been approved and moved to payout queue.",
                "/mobile/reimbursements");

            logger.LogInformation("Sent approval push for request {RequestId}", request.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send approval notification for reimbursement request {RequestId}", request.Id);
        }
    }

    public static async Task SendOnRejectedAsync(
        ReimbursementRequest request,
        IMemberRepository memberRepository,
        INotificationService notificationService,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var requestor = memberRepository.GetMemberById(request.RequestorMemberId);
            if (requestor == null || string.IsNullOrWhiteSpace(requestor.Email)) return;

            var template = await LoadTemplateAsync("ReimbursementRejected.txt");
            if (string.IsNullOrWhiteSpace(template)) return;

            var body = template
                .Replace("{RequestorName}", $"{requestor.FirstName} {requestor.LastName}")
                .Replace("{RequestId}", request.Id.ToString())
                .Replace("{TotalAmount}", request.TotalAmount.ToString("C2"))
                .Replace("{RejectReason}", request.RejectReason ?? "No reason provided")
                .Replace("{RejectedDate}", request.RejectedDateUtc?.ToString("MMM d, yyyy") ?? "N/A");

            await notificationService.SendPushNotificationAsync(
                request.RequestorMemberId, 
                "Reimbursement Denied", 
                $"Your request for {request.TotalAmount:C2} was denied: {request.RejectReason}",
                "/mobile/reimbursements");

            logger.LogInformation("Sent rejection push for request {RequestId}", request.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send rejection notification for reimbursement request {RequestId}", request.Id);
        }
    }

    public static async Task SendOnPaidAsync(
        ReimbursementRequest request,
        IMemberRepository memberRepository,
        INotificationService notificationService,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var requestor = memberRepository.GetMemberById(request.RequestorMemberId);
            if (requestor == null || string.IsNullOrWhiteSpace(requestor.Email)) return;

            var template = await LoadTemplateAsync("ReimbursementPaid.txt");
            if (string.IsNullOrWhiteSpace(template)) return;

            var body = template
                .Replace("{RequestorName}", $"{requestor.FirstName} {requestor.LastName}")
                .Replace("{RequestId}", request.Id.ToString())
                .Replace("{TotalAmount}", request.TotalAmount.ToString("C2"))
                .Replace("{PaidDate}", request.PaidDateUtc?.ToString("MMM d, yyyy") ?? "N/A");

            await notificationService.SendPushNotificationAsync(
                request.RequestorMemberId, 
                "Reimbursement Paid!", 
                $"Success! Your reimbursement of {request.TotalAmount:C2} has been paid.",
                "/mobile/reimbursements");

            logger.LogInformation("Sent payment push for request {RequestId}", request.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send paid notification for reimbursement request {RequestId}", request.Id);
        }
    }

    private static async Task<string> LoadTemplateAsync(string fileName)
    {
        try
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NotificationTemplates", fileName);
            if (File.Exists(templatePath))
            {
                return await File.ReadAllTextAsync(templatePath);
            }
        }
        catch
        {
            // Template file not found or error reading
        }
        return string.Empty;
    }

    private static async Task SendEmailAsync(string to, string subject, string body, ILogger logger)
    {
        // Placeholder for email sending - implement based on your email infrastructure
        // This could use SMTP, SendGrid, or another email service
        logger.LogInformation("Email would be sent to {To} with subject: {Subject}", to, subject);
        await Task.CompletedTask;
    }
}

