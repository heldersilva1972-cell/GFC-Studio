using System;
using GFC.Core.Helpers;
using GFC.Core.Services;

namespace GFC.PasswordTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // CHANGE THIS PASSWORD IF NEEDED
            const string newPassword = "GfcAdmin!2024";
            const string username = "admin";

            var passwordPolicy = new PasswordPolicy();
            var result = passwordPolicy.Validate(username, newPassword);
            if (!result.IsValid)
            {
                Console.WriteLine("Password does not meet the policy: " + (string.IsNullOrWhiteSpace(result.ErrorMessage) ? passwordPolicy.RequirementSummary : result.ErrorMessage));
                return;
            }

            var hash = PasswordHelper.HashPassword(newPassword);
            Console.WriteLine("New admin password: " + newPassword);
            Console.WriteLine("Password hash: " + hash);
        }
    }
}
