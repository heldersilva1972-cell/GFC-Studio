using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

    public class KeyHistoryService
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;
        private readonly ILogger<KeyHistoryService> _logger;

        public KeyHistoryService(IDbContextFactory<GfcDbContext> contextFactory, ILogger<KeyHistoryService> logger)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
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

            await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
            dbContext.KeyHistories.Add(history);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task LogReplacementAsync(int memberId, long oldCardNumber, long newCardNumber, string? reason, string? performedBy = null, string? keyType = "Card", CancellationToken cancellationToken = default)
        {
            var history = new KeyHistory
            {
                MemberId = memberId,
                CardNumber = newCardNumber,
                Action = "Replaced",
                Date = DateTime.UtcNow,
                Reason = $"Replaced card {oldCardNumber} with {newCardNumber}. {reason}".Trim(),
                PerformedBy = performedBy,
                KeyType = keyType
            };

            await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
            dbContext.KeyHistories.Add(history);
            await dbContext.SaveChangesAsync(cancellationToken);
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

            await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
            dbContext.KeyHistories.Add(history);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<KeyHistory>> GetHistoryForMemberAsync(int memberId, CancellationToken cancellationToken = default)
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
            return await dbContext.KeyHistories
                .Where(k => k.MemberId == memberId)
                .OrderByDescending(k => k.Date)
                .ToListAsync(cancellationToken);
        }
    }
