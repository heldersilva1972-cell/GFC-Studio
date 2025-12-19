using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Core;

public class CardReaderProfileService : ICardReaderProfileService
{
    private readonly GfcDbContext _dbContext;

    public CardReaderProfileService(GfcDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<CardReaderProfile> GetOrCreateAsync(CancellationToken ct = default)
    {
        var profile = await _dbContext.CardReaderProfiles.FindAsync(new object[] { 1 }, ct);

        if (profile != null)
        {
            return profile;
        }

        profile = new CardReaderProfile
        {
            Id = 1,
            DigitsOnly = true,
            MinLength = null,
            MaxLength = null
        };

        _dbContext.CardReaderProfiles.Add(profile);
        await _dbContext.SaveChangesAsync(ct);

        return profile;
    }

    public async Task<CardReaderProfile> UpdateProfileAsync(CardReaderProfile updated, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(updated);

        var profile = await _dbContext.CardReaderProfiles.FindAsync(new object[] { 1 }, ct);

        if (profile == null)
        {
            profile = new CardReaderProfile { Id = 1 };
            _dbContext.CardReaderProfiles.Add(profile);
        }

        profile.DigitsOnly = updated.DigitsOnly;
        profile.MinLength = updated.MinLength;
        profile.MaxLength = updated.MaxLength;
        profile.PrefixToTrim = updated.PrefixToTrim;
        profile.SuffixToTrim = updated.SuffixToTrim;
        profile.LastSampleRaw = updated.LastSampleRaw;
        profile.LastSampleParsed = updated.LastSampleParsed;
        profile.LastUpdatedUtc = updated.LastUpdatedUtc;

        await _dbContext.SaveChangesAsync(ct);

        return profile;
    }

    public string? ParseCardFromRaw(string rawInput, CardReaderProfile profile, out string debugInfo)
    {
        if (profile == null)
        {
            debugInfo = "Profile not provided.";
            return null;
        }

        if (string.IsNullOrWhiteSpace(rawInput))
        {
            debugInfo = "No raw input provided.";
            return null;
        }

        var working = rawInput;
        var debug = new StringBuilder();
        var prefixApplied = false;
        var suffixApplied = false;

        if (!string.IsNullOrWhiteSpace(profile.PrefixToTrim) &&
            working.StartsWith(profile.PrefixToTrim, StringComparison.Ordinal))
        {
            working = working[profile.PrefixToTrim.Length..];
            prefixApplied = true;
        }

        if (!string.IsNullOrWhiteSpace(profile.SuffixToTrim) &&
            working.EndsWith(profile.SuffixToTrim, StringComparison.Ordinal))
        {
            working = working[..^profile.SuffixToTrim.Length];
            suffixApplied = true;
        }

        if (profile.DigitsOnly)
        {
            working = string.Concat(working.Where(char.IsDigit));
        }

        if (profile.MinLength.HasValue && working.Length < profile.MinLength.Value)
        {
            debugInfo = $"Parsed length {working.Length} below minimum {profile.MinLength.Value}.";
            return null;
        }

        if (profile.MaxLength.HasValue && working.Length > profile.MaxLength.Value)
        {
            debugInfo = $"Parsed length {working.Length} exceeds maximum {profile.MaxLength.Value}.";
            return null;
        }

        debug.Append($"Prefix trimmed: {prefixApplied}; ");
        debug.Append($"Suffix trimmed: {suffixApplied}; ");
        debug.Append($"Digits only: {profile.DigitsOnly}; ");
        debug.Append($"Final length: {working.Length}");

        debugInfo = debug.ToString();
        return working;
    }
}
