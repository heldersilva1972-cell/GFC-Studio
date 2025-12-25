// [NEW]
namespace GFC.Core.Models;

public class MfaSetupInfo
{
    public string SecretKey { get; set; } = string.Empty;
    public string EncryptedSecretKey { get; set; } = string.Empty;
    public string QrCodeImageUrl { get; set; } = string.Empty;
}
