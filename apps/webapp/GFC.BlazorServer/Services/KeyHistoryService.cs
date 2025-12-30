using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

public class KeyHistoryService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<KeyHistoryService> _logger;

    public KeyHistoryService(GfcDbContext dbContext, ILogger<KeyHistoryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task LogAssignmentAsync(int memberId, long cardNumber, string? reason, string? performedBy = null, string? keyType = "Card", CancellationToken cancellationToken = default)
    {
        var history = new KeyHistory
        {
            MemberId = memberId,
            CardNumber = cardNumber,
            Action = "Assigned",
            Date = DateTime.UtcNow,
            Reason = reason,
            PerformedBy = performedBy,
            KeyType = keyType
        };

        _dbContext.KeyHistories.Add(history);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task LogRevocationAsync(int memberId, long cardNumber, string? reason, string? performedBy = null, string? keyType = null, CancellationToken cancellationToken = default)
    {
        var history = new KeyHistory
        {
            MemberId = memberId,
            CardNumber = cardNumber,
            Action = "Revoked",
            Date = DateTime.UtcNow,
            Reason = reason,
            PerformedBy = performedBy,
            KeyType = keyType
        };

        _dbContext.KeyHistories.Add(history);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<KeyHistory>> GetHistoryForMemberAsync(int memberId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.KeyHistories
            .Where(k => k.MemberId == memberId)
            .OrderByDescending(k => k.Date)
            .ToListAsync(cancellationToken);
    }
}
