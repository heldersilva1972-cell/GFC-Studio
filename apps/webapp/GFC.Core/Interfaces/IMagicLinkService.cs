// [NEW]
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.Core.Interfaces
{
    public interface IMagicLinkService
    {
        Task<RequestMagicLinkResult> RequestMagicLinkAsync(string email, string ipAddress);
        Task<MagicLinkValidationResult> ValidateMagicLinkTokenAsync(string token);
    }

    public class RequestMagicLinkResult
    {
        public bool Success { get; set; }
        public string UserMessage { get; set; }
    }

    public class MagicLinkValidationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public AppUser User { get; set; }
    }
}
