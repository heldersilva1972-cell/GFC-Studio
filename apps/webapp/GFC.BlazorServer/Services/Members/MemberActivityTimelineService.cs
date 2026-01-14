using System.Globalization;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Members;

public sealed class MemberActivityTimelineService : IMemberActivityTimelineService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<MemberActivityTimelineService> _logger;
    private readonly IMemberHistoryService _memberHistoryService;
    private readonly IBlazorSystemSettingsService _settingsService;
    private readonly TimeZoneInfo _systemTimeZone;
    private static readonly IReadOnlyDictionary<string, string> _sourceLabels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Member"] = "Member",
        ["Dues"] = "Dues",
        ["Key"] = "Key",
        ["NP Queue"] = "NP Queue",
        ["Controller"] = "Controller"
    };

    public MemberActivityTimelineService(
        GfcDbContext dbContext,
        ILogger<MemberActivityTimelineService> logger,
        IMemberHistoryService memberHistoryService,
        IBlazorSystemSettingsService settingsService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _memberHistoryService = memberHistoryService ?? throw new ArgumentNullException(nameof(memberHistoryService));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        
        try
        {
            var settings = _settingsService.GetSettings();
            _systemTimeZone = TimeZoneInfo.FindSystemTimeZoneById(settings.SystemTimeZoneId ?? "Eastern Standard Time");
        }
        catch
        {
            _systemTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        }
    }

    public async Task<IReadOnlyList<MemberActivityEvent>> GetTimelineAsync(int memberId, CancellationToken ct = default)
    {
        var events = new List<MemberActivityEvent>();

        await AddMemberChangesAsync(memberId, events, ct);
        await AddDuesPaymentsAsync(memberId, events, ct);
        await AddNpQueueEntriesAsync(memberId, events, ct);
        await AddKeyHistoryAsync(memberId, events, ct);
        AddControllerEventsIfSupported(events);

        return events
            .OrderByDescending(e => e.TimestampUtc)
            .Take(100)
            .ToList();
    }

    private async Task AddMemberChangesAsync(int memberId, List<MemberActivityEvent> events, CancellationToken ct)
    {
        try
        {
            var changes = await Task.Run(() => _memberHistoryService.GetHistory(memberId), ct);
            foreach (var change in changes)
            {
                var timestamp = EnsureUtc(change.ChangeDate);
                var details = $"From '{change.OldValue ?? "—"}' to '{change.NewValue ?? "—"}'";
                if (!string.IsNullOrWhiteSpace(change.ChangedBy))
                {
                    details += $" (by {change.ChangedBy})";
                }

                events.Add(new MemberActivityEvent
                {
                    TimestampUtc = EnsureUtc(timestamp),
                    Source = GetSourceLabel("Member"),
                    Summary = $"Field '{change.FieldName}' changed",
                    Details = details
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load member change history for member {MemberId}", memberId);
        }
    }

    private async Task AddDuesPaymentsAsync(int memberId, List<MemberActivityEvent> events, CancellationToken ct)
    {
        try
        {
            var dues = await _dbContext.DuesPayments
                .AsNoTracking()
                .Where(d => d.MemberId == memberId)
                .OrderByDescending(d => d.PaidDate ?? DateTime.MinValue)
                .ThenByDescending(d => d.Year)
                .ToListAsync(ct);

            foreach (var payment in dues)
            {
                var amountText = payment.Amount.HasValue
                    ? payment.Amount.Value.ToString("C", new CultureInfo("en-US"))
                    : "Payment";
                var paidDate = payment.PaidDate ?? new DateTime(payment.Year, 1, 1);
                var details = $"Year {payment.Year}";
                if (!string.IsNullOrWhiteSpace(payment.Notes))
                {
                    details += $"; Notes: {payment.Notes}";
                }

                events.Add(new MemberActivityEvent
                {
                    TimestampUtc = EnsureUtc(paidDate),
                    Source = GetSourceLabel("Dues"),
                    Summary = $"Dues payment {amountText}",
                    Details = details
                });
            }

            // Also add waiver events
            var waivers = await _dbContext.Waivers
                .AsNoTracking()
                .Where(w => w.MemberId == memberId)
                .OrderByDescending(w => w.Year)
                .ToListAsync(ct);

            foreach (var waiver in waivers)
            {
                var waiverDate = new DateTime(waiver.Year, 1, 1);
                var details = $"Year {waiver.Year}; Reason: {waiver.Reason}";
                if (!string.IsNullOrWhiteSpace(waiver.Notes))
                {
                    details += $"; Notes: {waiver.Notes}";
                }

                events.Add(new MemberActivityEvent
                {
                    TimestampUtc = EnsureUtc(waiverDate),
                    Source = GetSourceLabel("Dues"),
                    Summary = $"Dues waived",
                    Details = details
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load dues payments for member {MemberId}", memberId);
        }
    }

    private async Task AddNpQueueEntriesAsync(int memberId, List<MemberActivityEvent> events, CancellationToken ct)
    {
        try
        {
            var entries = await _dbContext.NPQueueEntries
                .AsNoTracking()
                .Where(n => n.MemberId == memberId)
                .OrderByDescending(n => n.AddedDate)
                .ToListAsync(ct);

            foreach (var entry in entries)
            {
                var summary = "NP queue action";
                var details = $"Status {entry.Status}, position {entry.QueuePosition}";

                events.Add(new MemberActivityEvent
                {
                    TimestampUtc = EnsureUtc(entry.AddedDate),
                    Source = GetSourceLabel("NP Queue"),
                    Summary = summary,
                    Details = details
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load NP queue entries for member {MemberId}", memberId);
        }
    }

    private async Task AddKeyHistoryAsync(int memberId, List<MemberActivityEvent> events, CancellationToken ct)
    {
        try
        {
            var history = await _dbContext.KeyHistories
                .AsNoTracking()
                .Where(k => k.MemberId == memberId)
                .OrderByDescending(k => k.Date)
                .ToListAsync(ct);

            foreach (var entry in history)
            {
                var details = $"Card {entry.CardNumber}";
                if (!string.IsNullOrWhiteSpace(entry.Reason))
                {
                    details += $"; {entry.Reason}";
                }

                events.Add(new MemberActivityEvent
                {
                    TimestampUtc = EnsureUtc(entry.Date),
                    Source = GetSourceLabel("Key"),
                    Summary = $"Key card {entry.Action}",
                    Details = details
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load key history for member {MemberId}", memberId);
        }
    }

    private void AddControllerEventsIfSupported(List<MemberActivityEvent> events)
    {
        // ControllerEvent does not have a MemberId link; skip to avoid incorrect associations.
        _ = events;
    }

    private DateTime EnsureUtc(DateTime value)
    {
        if (value.Kind == DateTimeKind.Utc)
        {
            return value;
        }

        // Treat both Local and Unspecified as Club Local time
        return TimeZoneInfo.ConvertTimeToUtc(value, _systemTimeZone);
    }

    private static string GetSourceLabel(string source)
        => _sourceLabels.TryGetValue(source, out var label) ? label : source;
}

