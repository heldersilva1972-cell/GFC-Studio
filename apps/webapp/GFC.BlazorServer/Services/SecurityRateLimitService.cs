using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for tracking and rate-limiting suspicious access attempts
/// </summary>
public interface ISecurityRateLimitService
{
    bool IsRateLimited(string ipAddress);
    void RecordBlockedAttempt(string ipAddress);
    void ClearAttempts(string ipAddress);
}

public class SecurityRateLimitService : ISecurityRateLimitService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<SecurityRateLimitService> _logger;
    private const int MaxAttemptsPerHour = 10;
    private const int BlockDurationMinutes = 60;

    public SecurityRateLimitService(IMemoryCache cache, ILogger<SecurityRateLimitService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public bool IsRateLimited(string ipAddress)
    {
        var key = $"blocked_attempts_{ipAddress}";
        
        if (_cache.TryGetValue(key, out int attempts))
        {
            if (attempts >= MaxAttemptsPerHour)
            {
                _logger.LogWarning("IP {IpAddress} is rate-limited ({Attempts} attempts)", ipAddress, attempts);
                return true;
            }
        }

        return false;
    }

    public void RecordBlockedAttempt(string ipAddress)
    {
        var key = $"blocked_attempts_{ipAddress}";
        
        if (_cache.TryGetValue(key, out int attempts))
        {
            attempts++;
        }
        else
        {
            attempts = 1;
        }

        _cache.Set(key, attempts, TimeSpan.FromMinutes(BlockDurationMinutes));
        
        if (attempts >= MaxAttemptsPerHour)
        {
            _logger.LogWarning("IP {IpAddress} has been rate-limited after {Attempts} blocked attempts", 
                ipAddress, attempts);
        }
    }

    public void ClearAttempts(string ipAddress)
    {
        var key = $"blocked_attempts_{ipAddress}";
        _cache.Remove(key);
    }
}
