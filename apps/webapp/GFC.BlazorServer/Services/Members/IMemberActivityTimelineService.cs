namespace GFC.BlazorServer.Services.Members;

public sealed class MemberActivityEvent
{
    public DateTime TimestampUtc { get; init; }
    public string Source { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public string? Details { get; init; }
}

public interface IMemberActivityTimelineService
{
    Task<IReadOnlyList<MemberActivityEvent>> GetTimelineAsync(int memberId, CancellationToken ct = default);
}

