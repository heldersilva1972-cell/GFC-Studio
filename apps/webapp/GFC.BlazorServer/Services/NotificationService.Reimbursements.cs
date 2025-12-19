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

            // Send email to all recipients
            foreach (var recipientEmail in recipients)
            {
                await SendEmailAsync(recipientEmail, $"New Reimbursement Request #{request.Id}", body, logger);
            }

            logger.LogInformation("Sent submission notification for reimbursement request {RequestId} to {RecipientCount} recipients", request.Id, recipients.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send submission notification for reimbursement request {RequestId}", request.Id);
        }
    }

    public static async Task SendOnApprovedAsync(
        ReimbursementRequest request,
        IMemberRepository memberRepository,
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

            await SendEmailAsync(requestor.Email, "Reimbursement Request Approved", body, logger);
            logger.LogInformation("Sent approval notification for reimbursement request {RequestId}", request.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send approval notification for reimbursement request {RequestId}", request.Id);
        }
    }

    public static async Task SendOnRejectedAsync(
        ReimbursementRequest request,
        IMemberRepository memberRepository,
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

            await SendEmailAsync(requestor.Email, "Reimbursement Request Rejected", body, logger);
            logger.LogInformation("Sent rejection notification for reimbursement request {RequestId}", request.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send rejection notification for reimbursement request {RequestId}", request.Id);
        }
    }

    public static async Task SendOnPaidAsync(
        ReimbursementRequest request,
        IMemberRepository memberRepository,
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

            await SendEmailAsync(requestor.Email, "Reimbursement Request Paid", body, logger);
            logger.LogInformation("Sent paid notification for reimbursement request {RequestId}", request.Id);
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

