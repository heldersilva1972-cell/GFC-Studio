using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Services.Core;

public interface ICardReaderProfileService
{
    Task<CardReaderProfile> GetOrCreateAsync(CancellationToken ct = default);

    Task<CardReaderProfile> UpdateProfileAsync(CardReaderProfile updated, CancellationToken ct = default);

    string? ParseCardFromRaw(string rawInput, CardReaderProfile profile, out string debugInfo);
}
