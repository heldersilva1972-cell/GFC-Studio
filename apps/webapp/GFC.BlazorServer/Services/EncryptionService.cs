// [NEW]
using GFC.Core.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IDataProtector _protector;
        private readonly ILogger<EncryptionService> _logger;

        public EncryptionService(IDataProtectionProvider provider, ILogger<EncryptionService> logger)
        {
            _protector = provider.CreateProtector("GFC.Mfa");
            _logger = logger;
        }

        public string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return _protector.Protect(text);
        }

        public string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return string.Empty;

            try
            {
                return _protector.Unprotect(encryptedText);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to decrypt data.");
                // This can happen if the key is wrong or the data is corrupted
                return string.Empty;
            }
        }
    }
}
