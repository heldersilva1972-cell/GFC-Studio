using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Singleton service to manage temporary MFA codes in memory.
/// </summary>
public interface IMfaChallengeService
{
    Task<string> CreateChallengeAsync(int userId);
    Task<bool> ValidateChallengeAsync(int userId, string code);
}

public class MfaChallengeService : IMfaChallengeService
{
    private readonly ConcurrentDictionary<int, MfaSession> _sessions = new();
    private static readonly TimeSpan CodeExpiry = TimeSpan.FromMinutes(10);

    public Task<string> CreateChallengeAsync(int userId)
    {
        var code = GenerateRandomCode();
        var session = new MfaSession
        {
            Code = code,
            ExpiresAt = DateTime.UtcNow.Add(CodeExpiry)
        };
        _sessions[userId] = session;
        return Task.FromResult(code);
    }

    public Task<bool> ValidateChallengeAsync(int userId, string code)
    {
        if (!_sessions.TryGetValue(userId, out var session))
        {
            return Task.FromResult(false);
        }

        if (session.ExpiresAt < DateTime.UtcNow)
        {
            _sessions.TryRemove(userId, out _);
            return Task.FromResult(false);
        }

        var isValid = session.Code == code;
        if (isValid)
        {
            _sessions.TryRemove(userId, out _);
        }
        return Task.FromResult(isValid);
    }

    private static string GenerateRandomCode()
    {
        return RandomNumberGenerator.GetInt32(100000, 999999).ToString();
    }

    private class MfaSession
    {
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
