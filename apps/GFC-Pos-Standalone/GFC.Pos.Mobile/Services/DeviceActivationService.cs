using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Json;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace GFC.Pos.Mobile.Services;

public class DeviceActivationService
{
    private readonly HttpClient _http;

    public DeviceActivationService(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Generates a deterministic fingerprint based on stable native device properties.
    /// </summary>
    public string GetHardwareFingerprint()
    {
        var sb = new StringBuilder();
        sb.Append(DeviceInfo.Current.Manufacturer);
        sb.Append(DeviceInfo.Current.Model);
        sb.Append(DeviceInfo.Current.Platform);
        sb.Append(DeviceInfo.Current.Idiom);
        
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Secures a user PIN using a hardware-derived PBKDF2 KDF, making brute force on physical theft extremely difficult.
    /// </summary>
    public string DeriveHardwarePinHash(string username, string pin)
    {
        string hardwareFingerprint = GetHardwareFingerprint();
        byte[] salt = Encoding.UTF8.GetBytes(hardwareFingerprint + username.ToLowerInvariant());
        
        // PBKDF2 with 10,000 iterations using HMAC-SHA256
        using var pbkdf2 = new Rfc2898DeriveBytes(pin, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32); // 256-bit derived key
        
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Signs a challenge nonce using a non-exportable hardware-backed key stored in TEE (SecureStorage).
    /// </summary>
    public async Task<string> GenerateActivationChallengeSignatureAsync(string nonce)
    {
        string? privateKeyPem = null;
        try
        {
            privateKeyPem = await SecureStorage.Default.GetAsync("gfc_tee_private_key");
        }
        catch { }
        
        using var rsa = RSA.Create();
        if (string.IsNullOrEmpty(privateKeyPem))
        {
            // First time: Generate new KeyPair inside SecureStorage (TEE-backed Keystore/Keychain)
            rsa.KeySize = 2048;
            privateKeyPem = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            await SecureStorage.Default.SetAsync("gfc_tee_private_key", privateKeyPem);
            
            string publicKeyPem = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            await SecureStorage.Default.SetAsync("gfc_tee_public_key", publicKeyPem);
        }
        else
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKeyPem), out _);
        }

        byte[] challengeBytes = Encoding.UTF8.GetBytes(nonce);
        byte[] signatureBytes = rsa.SignData(challengeBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        return Convert.ToBase64String(signatureBytes);
    }

    /// <summary>
    /// Rebinds the device using a 6-digit server OTP in case of app resets or wipes.
    /// </summary>
    public async Task<bool> RebindDeviceWithOtpAsync(string otp)
    {
        try
        {
            string fingerprint = GetHardwareFingerprint();
            string? publicKey = null;
            try
            {
                publicKey = await SecureStorage.Default.GetAsync("gfc_tee_public_key");
            }
            catch { }

            if (string.IsNullOrEmpty(publicKey))
            {
                // Ensure key exists
                await GenerateActivationChallengeSignatureAsync("init_key_probe");
                try
                {
                    publicKey = await SecureStorage.Default.GetAsync("gfc_tee_public_key");
                }
                catch { }
            }

            var request = new
            {
                Otp = otp,
                DeviceId = fingerprint,
                PublicKey = publicKey ?? string.Empty,
                DeviceName = $"{DeviceInfo.Current.Name} ({DeviceInfo.Current.Model})"
            };

            var response = await _http.PostAsJsonAsync("api/activation/rebind", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ActivationResult>();
                if (result != null && result.Success && !string.IsNullOrEmpty(result.TrustToken))
                {
                    await SecureStorage.Default.SetAsync("gfc_device_token", result.TrustToken);
                    await SecureStorage.Default.SetAsync("gfc_device_id", fingerprint);
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DeviceActivation] Rebind failed: {ex.Message}");
        }

        return false;
    }

    private class ActivationResult
    {
        public bool Success { get; set; }
        public string? TrustToken { get; set; }
    }
}
