
namespace GFC.Core.Models
{
    public class EmailResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public static EmailResult Ok() => new EmailResult { Success = true };
        public static EmailResult Failure(string message) => new EmailResult { Success = false, ErrorMessage = message };
    }
}
