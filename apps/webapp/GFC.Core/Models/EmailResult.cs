
namespace GFC.Core.Models
{
    public class EmailResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Message { get => ErrorMessage; set => ErrorMessage = value; }

        public static EmailResult Ok(string? message = null) => new EmailResult { Success = true, ErrorMessage = message };
        public static EmailResult Successful(string? message = null) => new EmailResult { Success = true, ErrorMessage = message };
        public static EmailResult Failure(string message) => new EmailResult { Success = false, ErrorMessage = message };
    }
}
