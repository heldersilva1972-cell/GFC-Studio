// [NEW]
namespace GFC.Core.Models
{
    public class VerificationResult
    {
        public string TestName { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
