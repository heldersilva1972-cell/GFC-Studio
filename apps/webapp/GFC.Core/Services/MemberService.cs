using GFC.Core.BusinessRules;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Business logic service for member operations.
/// Handles status transitions, validation, and logging.
/// </summary>
public class MemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IGlobalNoteRepository _noteRepository;
    private readonly IDuesRepository _duesRepository;
    private readonly IHistoryRepository _historyRepository;
    private readonly IAuditLogger _auditLogger;
    private readonly KeyCardLifecycleService _keyCardLifecycleService;

    public MemberService(
        IMemberRepository memberRepository,
        IGlobalNoteRepository noteRepository,
        IDuesRepository duesRepository,
        IHistoryRepository historyRepository,
        IAuditLogger auditLogger,
        KeyCardLifecycleService keyCardLifecycleService)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        _keyCardLifecycleService = keyCardLifecycleService ?? throw new ArgumentNullException(nameof(keyCardLifecycleService));
    }

    /// <summary>
    /// Updates a member's status and logs the change.
    /// Wrapper that preserves previous call sites.
    /// </summary>
    public async Task UpdateMemberStatusAsync(Member member, string newStatus, string? userName = null)
    {
        await UpdateMemberStatusAsync(member, newStatus, userName, bypassAutomations: false);
    }

    /// <summary>
    /// Updates a member's status and logs the change.
    /// Added overload to retain compatibility with callers that pass a bypass flag.
    /// </summary>
    public async Task UpdateMemberStatusAsync(Member member, string newStatus, string? userName, bool bypassAutomations)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));

        if (string.IsNullOrWhiteSpace(newStatus))
            throw new ArgumentException("Status cannot be empty.", nameof(newStatus));

        // Validate status value
        var validStatuses = new[] { "REGULAR", "GUEST", "LIFE", "LIFE MEMBER", "INACTIVE", "DECEASED", "REJECTED" };
        if (!validStatuses.Contains(newStatus.ToUpper()))
            throw new ArgumentException($"Invalid status: {newStatus}. Must be one of: {string.Join(", ", validStatuses)}", nameof(newStatus));

        var oldStatus = member.Status;
        newStatus = newStatus.ToUpper();

        // Only update if status actually changed
        if (oldStatus.Equals(newStatus, StringComparison.OrdinalIgnoreCase))
            return;

        // Update member status and set change date (or specific effective date)
        member.Status = newStatus;
        member.StatusChangeDate = DateTime.Today;
        NormalizeStatusDates(member);

        // Save the member
        if (member.MemberID > 0)
        {
            _memberRepository.UpdateMember(member);
        }
        else
        {
            var newId = _memberRepository.InsertMember(member);
            member.MemberID = newId;
        }

        // Log the status change
        _noteRepository.LogStatusChange(member.MemberID, oldStatus, newStatus, userName);
        _historyRepository.LogMemberChange(member.MemberID, "Status", oldStatus, newStatus, userName);

        AppendStatusNotesIfApplicable(member, oldStatus);

        // Keep audit behavior consistent; bypass flag reserved for future automation toggles.
        TryAuditStatusChange(member.MemberID, oldStatus, newStatus);
        
        // Evaluate key card eligibility immediately
        if (member.MemberID > 0)
        {
            await _keyCardLifecycleService.ProcessMemberAsync(member.MemberID, DateTime.Today.Year);
        }
    }

    /// <summary>
    /// Saves a member (insert or update) and handles status changes.
    /// </summary>
    public async Task SaveMemberAsync(Member member, string? userName = null)
    {
        if (member == null)
            throw new ArgumentNullException(nameof(member));

        // Validate required fields
        if (string.IsNullOrWhiteSpace(member.FirstName))
            throw new ArgumentException("First name is required.", nameof(member));

        if (string.IsNullOrWhiteSpace(member.LastName))
            throw new ArgumentException("Last name is required.", nameof(member));

        if (string.IsNullOrWhiteSpace(member.Status))
            throw new ArgumentException("Status is required.", nameof(member));

        // Check if this is an update and status changed
        bool statusChanged = false;
        string? previousStatus = null;

        if (member.MemberID > 0)
        {
            var existingMember = _memberRepository.GetMemberById(member.MemberID);
            if (existingMember != null && !existingMember.Status.Equals(member.Status, StringComparison.OrdinalIgnoreCase))
            {
                statusChanged = true;
                previousStatus = existingMember.Status;
                // Status changed - update status change date and log
                member.StatusChangeDate = DateTime.Today;
                _noteRepository.LogStatusChange(member.MemberID, existingMember.Status, member.Status, userName);
            }
        }
        else
        {
            // New member - set application date if not set
            if (member.ApplicationDate == null)
                member.ApplicationDate = DateTime.Today;
            
            // Set status change date for new members
            if (member.StatusChangeDate == null)
                member.StatusChangeDate = DateTime.Today;
        }

        NormalizeStatusDates(member);

        // Save the member
        if (member.MemberID > 0)
        {
            _memberRepository.UpdateMember(member);
        }
        else
        {
            var newId = _memberRepository.InsertMember(member);
            member.MemberID = newId;
        }
        if (statusChanged)
        {
            AppendStatusNotesIfApplicable(member, previousStatus);
            if (!string.IsNullOrWhiteSpace(previousStatus))
            {
                _historyRepository.LogMemberChange(member.MemberID, "Status", previousStatus, member.Status, userName);
            }
        }

        // Evaluate key card eligibility immediately
        if (member.MemberID > 0)
        {
            await _keyCardLifecycleService.ProcessMemberAsync(member.MemberID, DateTime.Today.Year);
        }
    }

    /// <summary>
    /// Validates member data before saving.
    /// </summary>
    public List<string> ValidateMember(Member member)
    {
        var errors = new List<string>();

        if (member == null)
        {
            errors.Add("Member cannot be null.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(member.FirstName))
            errors.Add("First name is required.");

        if (string.IsNullOrWhiteSpace(member.LastName))
            errors.Add("Last name is required.");

        if (string.IsNullOrWhiteSpace(member.Status))
            errors.Add("Status is required.");

        var validStatuses = new[] { "REGULAR", "GUEST", "LIFE", "LIFE MEMBER", "INACTIVE", "DECEASED", "REJECTED" };
        if (!string.IsNullOrWhiteSpace(member.Status) && 
            !validStatuses.Contains(member.Status.ToUpper()))
        {
            errors.Add($"Invalid status: {member.Status}. Must be one of: {string.Join(", ", validStatuses)}");
        }

        // Basic email validation if provided
        if (!string.IsNullOrWhiteSpace(member.Email) && 
            !member.Email.Contains("@", StringComparison.Ordinal))
        {
            errors.Add("Email address appears to be invalid.");
        }

        return errors;
    }

    private static void NormalizeStatusDates(Member member)
    {
        if (member == null)
            return;

        if (string.Equals(member.Status, "INACTIVE", StringComparison.OrdinalIgnoreCase))
        {
            if (!member.InactiveDate.HasValue)
            {
                member.InactiveDate = member.StatusChangeDate ?? DateTime.Today;
            }

            member.StatusChangeDate = member.InactiveDate;
        }
        else if (string.Equals(member.Status, "DECEASED", StringComparison.OrdinalIgnoreCase))
        {
            if (!member.DateOfDeath.HasValue)
            {
                member.DateOfDeath = member.StatusChangeDate ?? DateTime.Today;
            }

            member.StatusChangeDate = member.DateOfDeath;
        }
    }

    private void AppendStatusNotesIfApplicable(Member member, string? previousStatus)
    {
        if (member == null || member.MemberID <= 0)
            return;

        bool becameInactive = string.Equals(member.Status, "INACTIVE", StringComparison.OrdinalIgnoreCase) &&
                              !string.Equals(previousStatus, member.Status, StringComparison.OrdinalIgnoreCase);

        bool becameDeceased = string.Equals(member.Status, "DECEASED", StringComparison.OrdinalIgnoreCase) &&
                              !string.Equals(previousStatus, member.Status, StringComparison.OrdinalIgnoreCase);

        if (becameInactive && member.InactiveDate.HasValue)
        {
            var note = $"Member inactive as of {member.InactiveDate.Value:MM/dd/yyyy}";
            _duesRepository.AppendNoteToUnpaidDues(member.MemberID, note);
        }

        if (becameDeceased && member.DateOfDeath.HasValue)
        {
            var note = $"Member deceased on {member.DateOfDeath.Value:MM/dd/yyyy}";
            _duesRepository.AppendNoteToUnpaidDues(member.MemberID, note);
        }
    }

    private void TryAuditStatusChange(int memberId, string? oldStatus, string? newStatus)
    {
        if (string.Equals(oldStatus, newStatus, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var oldNormalized = MemberStatusHelper.NormalizeStatus(oldStatus);
        var newNormalized = MemberStatusHelper.NormalizeStatus(newStatus);

        if (oldNormalized.Equals("LIFE", StringComparison.OrdinalIgnoreCase) ||
            newNormalized.Equals("LIFE", StringComparison.OrdinalIgnoreCase))
        {
            var details = $"Status change: {oldStatus ?? "unknown"} -> {newStatus ?? "unknown"}";
            _auditLogger.Log(
                AuditLogActions.LifeStatusChanged,
                null,
                null,
                details);
        }

        if (string.Equals(newNormalized, "INACTIVE", StringComparison.OrdinalIgnoreCase))
        {
            var details = $"Dues status change: {oldStatus ?? "unknown"} -> {newStatus ?? "unknown"} (set inactive for dues)";
            _auditLogger.Log(
                AuditLogActions.DuesChanged,
                null,
                null,
                details);
        }
    }
}



