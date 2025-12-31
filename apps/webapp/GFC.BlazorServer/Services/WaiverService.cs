using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using GFC.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

public class WaiverService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<WaiverService> _logger;
    private readonly IAuditLogger _auditLogger;

    public WaiverService(IDbContextFactory<GfcDbContext> contextFactory, ILogger<WaiverService> logger, IAuditLogger auditLogger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
    }

    public async Task<bool> IsWaivedAsync(int memberId, int year, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Waivers
            .AnyAsync(w => w.MemberId == memberId && w.Year == year, cancellationToken);
    }

    public async Task AddWaiverAsync(int memberId, int year, string reason, string? notes, int? performedByUserId = null, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Reason is required.", nameof(reason));
        }

        var existing = await dbContext.Waivers
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.MemberId == memberId && w.Year == year, cancellationToken);

        var previous = existing is null
            ? null
            : new
            {
                existing.Reason,
                existing.Notes
            };

        var tracked = existing == null
            ? null
            : await dbContext.Waivers.FirstOrDefaultAsync(w => w.Id == existing.Id, cancellationToken);

        if (existing != null)
        {
            tracked!.Reason = reason.Trim();
            tracked.Notes = notes;
            dbContext.Waivers.Update(tracked);
        }
        else
        {
            var waiver = new Waiver
            {
                MemberId = memberId,
                Year = year,
                Reason = reason.Trim(),
                Notes = notes
            };
            dbContext.Waivers.Add(waiver);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var previousSummary = previous == null
            ? "no previous waiver"
            : $"previous reason {previous.Reason}, notes: {previous.Notes ?? "none"}";
        var details = $"Waiver set for {year}: reason {reason.Trim()}, notes: {notes ?? "none"}; {previousSummary}";
        _auditLogger.Log(
            AuditLogActions.DuesWaiverChanged,
            performedByUserId,
            null,
            details);
    }

    public async Task<List<Waiver>> GetWaiversForMemberAsync(int memberId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Waivers
            .Where(w => w.MemberId == memberId)
            .OrderByDescending(w => w.Year)
            .ToListAsync(cancellationToken);
    }
}
