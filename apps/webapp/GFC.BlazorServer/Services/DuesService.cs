using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using GFC.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

public class DuesService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<DuesService> _logger;
    private readonly IAuditLogger _auditLogger;
    private readonly KeyCardLifecycleService? _keyCardLifecycleService;

    public DuesService(
        GfcDbContext dbContext, 
        ILogger<DuesService> logger, 
        IAuditLogger auditLogger,
        KeyCardLifecycleService? keyCardLifecycleService = null)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        _keyCardLifecycleService = keyCardLifecycleService; // Optional - may not be registered yet
    }

    public async Task<bool> IsDuesPaidAsync(int memberId, int year, CancellationToken cancellationToken = default)
    {
        var payment = await _dbContext.DuesPayments
            .FirstOrDefaultAsync(d => d.MemberId == memberId && d.Year == year, cancellationToken);
        
        return payment != null && payment.PaidDate.HasValue && payment.Amount.HasValue && payment.Amount.Value > 0;
    }

    public async Task<DuesState> GetDuesStateAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var currentYear = DateTime.Now.Year;
        var payments = await _dbContext.DuesPayments
            .Where(d => d.MemberId == memberId)
            .OrderByDescending(d => d.Year)
            .ToListAsync(cancellationToken);

        var currentYearPayment = payments.FirstOrDefault(p => p.Year == currentYear);
        var previousYearPayment = payments.FirstOrDefault(p => p.Year == currentYear - 1);

        return new DuesState
        {
            MemberId = memberId,
            CurrentYear = currentYear,
            CurrentYearPaid = currentYearPayment != null && currentYearPayment.PaidDate.HasValue && currentYearPayment.Amount.HasValue && currentYearPayment.Amount.Value > 0,
            PreviousYearPaid = previousYearPayment != null && previousYearPayment.PaidDate.HasValue && previousYearPayment.Amount.HasValue && previousYearPayment.Amount.Value > 0,
            AllPayments = payments
        };
    }

    public async Task RecordPaymentAsync(int memberId, int year, decimal amount, DateTime paidDate, string? notes, string? paymentType = null, int? performedByUserId = null, CancellationToken cancellationToken = default)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0.", nameof(amount));
        }

        if (year > DateTime.Now.Year + 10)
        {
            throw new ArgumentException("Year cannot be more than 10 years in the future.", nameof(year));
        }

        var existing = await _dbContext.DuesPayments
            .FirstOrDefaultAsync(d => d.MemberId == memberId && d.Year == year, cancellationToken);

        var previousDetails = existing is null
            ? null
            : new
            {
                existing.Amount,
                existing.PaidDate,
                existing.PaymentType,
                existing.Notes
            };

        string? recorderName = null;
        if (performedByUserId.HasValue)
        {
            var user = await _dbContext.AppUsers.FindAsync(new object[] { performedByUserId.Value }, cancellationToken);
            recorderName = user?.Username ?? "Unknown";
        }

        if (existing != null)
        {
            existing.Amount = amount;
            existing.PaidDate = paidDate;
            existing.PaymentType = paymentType;
            existing.Notes = notes;
            existing.RecordedByUserId = performedByUserId;
            existing.RecordedBy = recorderName;
            _dbContext.DuesPayments.Update(existing);
        }
        else
        {
            var payment = new DuesPayment
            {
                MemberId = memberId,
                Year = year,
                Amount = amount,
                PaidDate = paidDate,
                PaymentType = paymentType,
                Notes = notes,
                RecordedByUserId = performedByUserId,
                RecordedBy = recorderName
            };
            _dbContext.DuesPayments.Add(payment);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var member = await _dbContext.Members.FindAsync(new object[] { memberId }, cancellationToken);
        var memberName = member != null ? $"{member.LastName}, {member.FirstName}" : $"Member #{memberId}";

        var previousSummary = previousDetails == null
            ? "no previous payment"
            : $"previous amount {previousDetails.Amount?.ToString() ?? "n/a"}, paid {previousDetails.PaidDate?.ToShortDateString() ?? "n/a"}, notes: {previousDetails.Notes ?? "none"}";
        
        var details = $"[{memberName}] Recorded dues for {year}: amount {amount}, paid {paidDate:d}, notes: {notes ?? "none"}; {previousSummary}";
        
        var actionType = AuditLogActions.DuesPaymentAdded;
        if (existing != null)
        {
            actionType = AuditLogActions.DuesPaymentUpdated;
        }
        else if (year > DateTime.Today.Year)
        {
            actionType = AuditLogActions.DuesAdvancedAdded;
        }

        _auditLogger.Log(
            actionType,
            performedByUserId,
            null,
            details,
            targetMemberId: memberId);

        // Trigger card reactivation if lifecycle service is available
        if (_keyCardLifecycleService != null)
        {
            try
            {
                await _keyCardLifecycleService.ProcessMemberAsync(memberId, year, cancellationToken);
                _logger.LogInformation("Triggered card eligibility check for member {MemberId} after dues payment", memberId);
            }
            catch (Exception ex)
            {
                // Log but don't fail the dues payment if card processing fails
                _logger.LogError(ex, "Error processing card eligibility for member {MemberId} after dues payment", memberId);
            }
        }
    }

    public async Task<List<DuesPayment>> GetDuesForMemberAsync(int memberId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DuesPayments
            .Where(d => d.MemberId == memberId)
            .OrderByDescending(d => d.Year)
            .ToListAsync(cancellationToken);
    }
}

public class DuesState
{
    public int MemberId { get; set; }
    public int CurrentYear { get; set; }
    public bool CurrentYearPaid { get; set; }
    public bool PreviousYearPaid { get; set; }
    public List<DuesPayment> AllPayments { get; set; } = new();
}
