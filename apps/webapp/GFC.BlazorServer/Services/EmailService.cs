// [NEW]
using System.Threading.Tasks;
using System;

namespace GFC.BlazorServer.Services
{
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Sends an email. This is a placeholder implementation that logs to the console.
        /// </summary>
        public Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            Console.WriteLine("--- SENDING EMAIL (SIMULATED) ---");
            Console.WriteLine($"To: {recipientEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine("Body:");
            Console.WriteLine(body);
            Console.WriteLine("---------------------------------");

            return Task.CompletedTask;
        }
    }
}
