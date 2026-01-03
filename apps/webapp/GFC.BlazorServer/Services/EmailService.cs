// [MODIFIED]
using System.Threading.Tasks;
using System;
using GFC.Core.Interfaces;

namespace GFC.BlazorServer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IUrlHelperService _urlHelperService;

        public EmailService(IUrlHelperService urlHelperService)
        {
            _urlHelperService = urlHelperService;
        }

        /// <summary>
        /// Sends an email. This is a placeholder implementation that logs to the console.
        /// </summary>
        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var baseUrl = await _urlHelperService.GetBaseUrlAsync();
            Console.WriteLine("--- SENDING EMAIL (SIMULATED) ---");
            Console.WriteLine($"To: {recipientEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body (using base URL: {baseUrl}):");
            Console.WriteLine(body);
            Console.WriteLine("---------------------------------");
        }
    }
}
