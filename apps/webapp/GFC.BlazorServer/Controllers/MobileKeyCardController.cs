using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.Core.Services;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-keycards")]
[Authorize]
public class MobileKeyCardController : ControllerBase
{
    private readonly IKeyCardRepository _keyCardRepo;
    private readonly IKeyCardDashboardRepository _dashboardRepo;
    private readonly KeyCardService _cardService;
    private readonly IDuesYearSettingsRepository _duesSettingsRepo;
    private readonly KeyCardLifecycleService _lifecycleService;
    private readonly KeyHistoryService _keyHistoryService;
    private readonly IAuditLogger _auditLogger;
    private readonly IMemberRepository _memberRepo;
    private readonly MemberService _memberService;

    public MobileKeyCardController(
        IKeyCardRepository keyCardRepo,
        IKeyCardDashboardRepository dashboardRepo,
        KeyCardService cardService,
        IDuesYearSettingsRepository duesSettingsRepo,
        KeyCardLifecycleService lifecycleService,
        KeyHistoryService keyHistoryService,
        IAuditLogger auditLogger,
        IMemberRepository memberRepo,
        MemberService memberService)
    {
        _keyCardRepo = keyCardRepo;
        _dashboardRepo = dashboardRepo;
        _cardService = cardService;
        _duesSettingsRepo = duesSettingsRepo;
        _lifecycleService = lifecycleService;
        _keyHistoryService = keyHistoryService;
        _auditLogger = auditLogger;
        _memberRepo = memberRepo;
        _memberService = memberService;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<List<MobileKeyCardDashboardRow>>> GetDashboardData(int year)
    {
        try
        {
            // 1. Get the unified dashboard rows (contains all members + their current card info)
            var dashboardRows = _dashboardRepo.GetMembersForYear(year);
            
            // 2. Get year settings for grace period logic
            var settings = _duesSettingsRepo.GetSettingsForYear(year);
            var graceDate = settings?.GraceEndDate?.Date;

            var result = new List<MobileKeyCardDashboardRow>();

            foreach (var row in dashboardRows)
            {
                // Evaluate eligibility using business logic
                var eligibility = _cardService.EvaluateEligibilityForRow(row, year, graceDate);

                var isDirector = row.IsDirectorCurrent;
                var isLifeMember = string.Equals(row.MemberStatus, "LIFE", StringComparison.OrdinalIgnoreCase);
                var isPaid = isDirector || isLifeMember || KeyCardService.IsDuesSatisfied(row.DuesPaymentType, row.DuesPaidDate);

                result.Add(new MobileKeyCardDashboardRow
                {
                    CardId = row.KeyCardId ?? 0,
                    CardNumber = row.KeyCardNumber ?? "NO CARD",
                    MemberId = row.MemberId,
                    MemberName = row.DisplayName,
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    IsActive = row.HasActiveAssignment,
                    MemberStatus = row.MemberStatus,
                    IsEligible = eligibility.Eligible,
                    IsPaid = isPaid,
                    DeactivationDate = eligibility.GraceEndDate,
                    IsDelinquent = !eligibility.Eligible && !eligibility.GracePeriodActive,
                    InGracePeriod = eligibility.GracePeriodActive,
                    DeactivationReason = eligibility.Reason,
                    Year = year,
                    IsSyncPending = false,
                    IsDirector = isDirector,
                    IsLifeMember = isLifeMember
                });
            }

            return Ok(result.OrderBy(r => r.LastName).ToList());
        }
        catch (Exception ex)
        {
            // [DEBUG] Return the full error message and stack trace to identify the cause of the 500
            return StatusCode(500, new { 
                Message = ex.Message, 
                StackTrace = ex.StackTrace,
                InnerMessage = ex.InnerException?.Message 
            });
        }
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> ToggleCardStatus([FromBody] ToggleCardRequest request)
    {
        try
        {
            var card = _keyCardRepo.GetById(request.CardId);
            if (card == null) return NotFound();

            var user = User.Identity?.Name ?? "Mobile App";

            if (card.IsActive)
            {
                await _lifecycleService.DeactivateCardAsync(card.KeyCardId, "Manual", "Deactivated via Mobile", user);
                _auditLogger.Log("KeyCardDeactivated", null, null, $"Card #{card.CardNumber} deactivated via Mobile by {user}", targetMemberId: card.MemberId);
            }
            else
            {
                await _lifecycleService.ReactivateCardAsync(card.KeyCardId, "Reactivated via Mobile", user);
                _auditLogger.Log("KeyCardReactivated", null, null, $"Card #{card.CardNumber} reactivated via Mobile by {user}", targetMemberId: card.MemberId);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignCard([FromBody] AssignCardRequest request)
    {
        try
        {
            var existingCard = _keyCardRepo.GetByCardNumber(request.CardNumber);
            if (existingCard != null) return BadRequest($"Card #{request.CardNumber} is already assigned.");

            var user = User.Identity?.Name ?? "Mobile App";
            var activeCard = _keyCardRepo.GetActiveMemberCard(request.MemberId);
            
            if (activeCard != null)
            {
                await _lifecycleService.DeactivateCardAsync(activeCard.KeyCardId, "Card Replaced", $"Replaced by new card #{request.CardNumber}", user);
                _keyCardRepo.ReplaceCard(request.MemberId, request.CardNumber, request.Notes ?? "Replaced via Mobile", request.CardType ?? "Card");
                
                if (long.TryParse(request.CardNumber, out long newNum) && long.TryParse(activeCard.CardNumber, out long oldNum))
                {
                    await _keyHistoryService.LogReplacementAsync(request.MemberId, oldNum, newNum, request.Notes ?? "Replaced via Mobile", user, request.CardType ?? "Card");
                    _auditLogger.Log("KeyCardReplaced", null, null, $"Card #{oldNum} replaced with #{request.CardNumber} for member {request.MemberId} by {user} via Mobile.", targetMemberId: request.MemberId);
                }
            }
            else
            {
                _keyCardRepo.Create(request.CardNumber, request.MemberId, request.Notes ?? "Assigned via Mobile", request.CardType ?? "Card");
                if (long.TryParse(request.CardNumber, out long cardNumberLong))
                {
                    await _keyHistoryService.LogAssignmentAsync(request.MemberId, cardNumberLong, request.Notes ?? "Assigned via Mobile", user, request.CardType ?? "Card");
                    _auditLogger.Log("KeyCardAssigned", null, null, $"Card #{request.CardNumber} assigned to member {request.MemberId} by {user} via Mobile.", targetMemberId: request.MemberId);
                }
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("reactivate-member")]
    public async Task<IActionResult> ReactivateMember([FromBody] int memberId)
    {
        try
        {
            var member = _memberRepo.GetMemberById(memberId);
            if (member == null) return NotFound();

            var user = User.Identity?.Name ?? "Mobile App";
            member.Status = "REGULAR";
            member.StatusChangeDate = DateTime.Now;
            member.InactiveDate = null;
            _memberRepo.UpdateMember(member);
            
            _auditLogger.Log("MemberReactivated", null, null, $"Member {member.FirstName} {member.LastName} reactivated via Mobile by {user}", targetMemberId: memberId);
            
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
