using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resend;
using System.Text;

namespace GFC.BlazorServer.Services;

public class ResendApiService(
    IOptionsMonitor<EmailSettings> emailSettings,
    IAuditLogger auditLogger,
    ILogger<ResendApiService> logger,
    IResend resend) : IEmailService
{
    private EmailSettings _settings => emailSettings.CurrentValue;

    public async Task SendEmailAsync(string recipientEmail, string subject, string body, Dictionary<string, byte[]>? attachments = null)
    {
        try
        {
            var message = new EmailMessage();
            message.From = $"{_settings.FromName} <{_settings.FromAddress}>";
            message.To.Add(recipientEmail);
            message.Subject = subject;
            message.HtmlBody = body;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(new EmailAttachment
                    {
                        Filename = attachment.Key,
                        Content = attachment.Value
                    });
                }
            }

            await resend.EmailSendAsync(message);
            logger.LogInformation("Email sent successfully via Resend to {Recipient}", recipientEmail);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email via Resend to {Recipient}", recipientEmail);
            auditLogger.Log("Resend Email Failure", null, null, $"Recipient: {recipientEmail} | Error: {ex.Message}");
            // We do not rethrow to ensure the app doesn't crash as per requirements
        }
    }

    public async Task SendOrderEmailAsync(string toEmail, string subject, LiquorOrder order)
    {
        try
        {
            var body = GenerateOrderHtmlTable(order);
            await SendEmailAsync(toEmail, subject, body);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send order email for Order #{OrderId}", order.Id);
            auditLogger.Log("Order Email Failure", null, null, $"Order #{order.Id} | Error: {ex.Message}");
        }
    }

    private string GenerateOrderHtmlTable(LiquorOrder order)
    {
        var sb = new StringBuilder();
        sb.Append("<div style='font-family: sans-serif; max-width: 600px; margin: auto; border: 1px solid #eee; padding: 20px; border-radius: 10px;'>");
        sb.Append("<h2 style='color: #2c3e50; border-bottom: 2px solid #3498db; padding-bottom: 10px;'>Liquor Purchase Order</h2>");
        sb.Append("<table style='width: 100%; margin-bottom: 20px;'>");
        sb.Append($"<tr><td><strong>Order ID:</strong></td><td align='right'>#{order.Id}</td></tr>");
        sb.Append($"<tr><td><strong>Date:</strong></td><td align='right'>{order.OrderDate:f}</td></tr>");
        if (order.Vendor != null)
        {
            sb.Append($"<tr><td><strong>Vendor:</strong></td><td align='right'>{order.Vendor.Name}</td></tr>");
        }
        sb.Append("</table>");
        
        if (!string.IsNullOrEmpty(order.SpecialInstructions))
        {
            sb.Append("<div style='background: #f9f9f9; padding: 10px; border-left: 4px solid #3498db; margin-bottom: 20px;'>");
            sb.Append("<strong>Special Instructions:</strong><br/>");
            sb.Append(order.SpecialInstructions);
            sb.Append("</div>");
        }

        sb.Append("<table border='0' cellpadding='10' cellspacing='0' style='width: 100%; border-collapse: collapse;'>");
        sb.Append("<tr style='background-color: #3498db; color: white;'>");
        sb.Append("<th align='left'>Product</th><th align='center'>Qty</th><th align='right'>Price</th><th align='right'>Total</th>");
        sb.Append("</tr>");

        foreach (var item in order.OrderItems)
        {
            var subtotal = item.UnitPriceAtTimeOfOrder * item.Quantity;
            sb.Append("<tr style='border-bottom: 1px solid #eee;'>");
            sb.Append($"<td>{item.LiquorItem?.Name}<br/><small style='color: #7f8c8d;'>{item.LiquorItem?.BottleSize}</small></td>");
            sb.Append($"<td align='center'>{item.Quantity}</td>");
            sb.Append($"<td align='right'>{item.UnitPriceAtTimeOfOrder:C}</td>");
            sb.Append($"<td align='right'><strong>{subtotal:C}</strong></td>");
            sb.Append("</tr>");
        }

        sb.Append("<tr>");
        sb.Append("<td colspan='3' align='right' style='padding-top: 20px;'><strong>Items Total:</strong></td>");
        sb.Append($"<td align='right' style='padding-top: 20px; font-size: 1.2em; color: #2c3e50;'><strong>{order.ItemsTotal:C}</strong></td>");
        sb.Append("</tr>");
        sb.Append("</table>");
        
        sb.Append("<p style='margin-top: 30px; font-size: 0.8em; color: #bdc3c7; text-align: center; border-top: 1px solid #eee; padding-top: 10px;'>");
        sb.Append("This is an automated order sent from the LiquorHub POS system.");
        sb.Append("</p>");
        sb.Append("</div>");

        return sb.ToString();
    }
}
