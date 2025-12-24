// [NEW]
using GFC.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GFC.Core.Services
{
    public class StreamSecurityService : IStreamSecurityService
    {
        private readonly byte[] _secretKey;
        private readonly int _tokenValiditySeconds;
        private readonly bool _ipLockingEnabled;

        public StreamSecurityService(IConfiguration configuration)
        {
            var secret = configuration["StreamSecurity:SecretKey"];
            if (string.IsNullOrEmpty(secret) || secret.Length < 32)
            {
                throw new ArgumentException("StreamSecurity:SecretKey must be configured and be at least 32 characters long.");
            }
            _secretKey = Encoding.UTF8.GetBytes(secret);
            _tokenValiditySeconds = configuration.GetValue<int?>("StreamSecurity:TokenValiditySeconds") ?? 60;
            _ipLockingEnabled = configuration.GetValue<bool?>("StreamSecurity:IPLockingEnabled") ?? false;
        }

        public string GenerateStreamToken(int cameraId, string ipAddress)
        {
            var expiry = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _tokenValiditySeconds;
            var payload = _ipLockingEnabled && !string.IsNullOrEmpty(ipAddress)
                ? $"{cameraId}:{expiry}:{ipAddress}"
                : $"{cameraId}:{expiry}";

            using (var hmac = new HMACSHA256(_secretKey))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                var signature = Convert.ToBase64String(hash);
                return $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(payload))}.{signature}";
            }
        }

        public bool ValidateStreamToken(string token, int cameraId, string ipAddress)
        {
            if (string.IsNullOrEmpty(token)) return false;

            var parts = token.Split('.');
            if (parts.Length != 2) return false;

            try
            {
                var payloadBase64 = parts[0];
                var signature = parts[1];

                var payloadBytes = Convert.FromBase64String(payloadBase64);
                var payloadString = Encoding.UTF8.GetString(payloadBytes);

                using (var hmac = new HMACSHA256(_secretKey))
                {
                    var expectedHash = hmac.ComputeHash(payloadBytes);
                    var expectedSignature = Convert.ToBase64String(expectedHash);

                    if (signature != expectedSignature)
                    {
                        return false;
                    }
                }

                var payloadParts = payloadString.Split(':');
                var tokenCameraId = int.Parse(payloadParts[0]);
                var expiry = long.Parse(payloadParts[1]);

                if (tokenCameraId != cameraId) return false;

                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expiry) return false;

                if (_ipLockingEnabled)
                {
                    if (payloadParts.Length != 3 || payloadParts[2] != ipAddress)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                // Catch any parsing/decoding errors
                return false;
            }
        }
    }
}
