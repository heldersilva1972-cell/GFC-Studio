using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.BusinessRules;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using MemberDoorAccess = GFC.Core.Models.MemberDoorAccess;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Components.Pages.KeyCardDetails;

public partial class KeyCardDetail : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Inject] private IKeyCardRepository KeyCardRepository { get; set; } = default!;
    [Inject] private IMemberRepository MemberRepository { get; set; } = default!;
    [Inject] private IMemberKeycardRepository MemberKeycardRepository { get; set; } = default!;
    [Inject] private NavigationManager NavManager { get; set; } = default!;
    [Inject] private ILogger<KeyCardDetail> Logger { get; set; } = default!;
    [Inject] private GfcDbContext DbContext { get; set; } = default!;

    private KeyCard? _card;
    private Member? _member;
    private MemberKeycardAssignment? _assignment;
    private List<KeyHistory> _history = new();
    private bool _historyAvailable;
    private bool _loading = true;
    private bool _notFound;
    private string? _error;

    protected override async Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _loading = true;
        _error = null;
        _notFound = false;
        _history.Clear();
        _historyAvailable = false;

        try
        {
            _card = await Task.Run(() => KeyCardRepository.GetById(Id));
            if (_card is null)
            {
                _notFound = true;
                return;
            }

            _member = await Task.Run(() => MemberRepository.GetMemberById(_card.MemberId));
            _assignment = await Task.Run(() => MemberKeycardRepository.GetCurrentAssignmentForCard(_card.KeyCardId));

            if (!string.IsNullOrWhiteSpace(_card.CardNumber) && long.TryParse(_card.CardNumber, out var parsedCardNumber))
            {
                _historyAvailable = true;
                _history = await DbContext.KeyHistories
                    .Where(k => k.CardNumber == parsedCardNumber)
                    .OrderByDescending(k => k.Date)
                    .Take(20)
                    .ToListAsync();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error loading key card {KeyCardId}", Id);
            _error = "Unable to load key card details.";
        }
        finally
        {
            _loading = false;
        }
    }

    private IEnumerable<MemberDoorAccess> AccessEntries =>
        Enumerable.Empty<MemberDoorAccess>();
        // TODO: wire door access collection for this card when available

    private IEnumerable<KeyHistory> HistoryEntries =>
        _historyAvailable ? _history : Enumerable.Empty<KeyHistory>();

    private string GetCardTitle() => string.IsNullOrWhiteSpace(_card?.CardNumber)
        ? $"Key Card #{Id}"
        : $"Key Card #{_card.CardNumber}";

    private string? GetIssuedDateText()
    {
        var issued = _assignment?.FromDate;
        return issued.HasValue ? FormatDate(issued) : null;
    }

    private string FormatDate(DateTime? date)
        => date?.ToString("MMM d, yyyy") ?? string.Empty;

    private string GetMemberName()
    {
        if (_member is null)
        {
            return string.Empty;
        }

        return string.IsNullOrWhiteSpace(_member.MiddleName)
            ? $"{_member.FirstName} {_member.LastName}"
            : $"{_member.FirstName} {_member.MiddleName} {_member.LastName}";
    }

    private string GetMemberStatusLabel()
    {
        if (_member is null)
        {
            return string.Empty;
        }

        var normalized = MemberStatusHelper.NormalizeStatus(_member.Status);
        return normalized switch
        {
            "REGULAR" => "Regular",
            "LIFE" or "LIFE MEMBER" => "Life",
            "GUEST" or "GUEST-NP" => "Guest",
            "INACTIVE" => "Inactive",
            "DECEASED" => "Deceased",
            "REJECTED" => "Rejected",
            _ => _member.Status
        };
    }

    private string GetMemberStatusColor()
    {
        var label = GetMemberStatusLabel();
        return label switch
        {
            "Regular" => "#14b8a6",
            "Life" => "#d6b33d",
            "Guest" => "#6b7280",
            "Inactive" => "#ef4444",
            "Suspended" => "#ef4444",
            "Deceased" => "#ef4444",
            _ => "#6b7280"
        };
    }

    private void NavigateBack() => NavManager.NavigateTo("/keycards");

    private void NavigateToMember()
    {
        if (_member is null)
        {
            return;
        }

        NavManager.NavigateTo($"/members/{_member.MemberID}");
    }

    private void NavigateToEnable()
    {
        if (_card is null)
        {
            return;
        }

        NavManager.NavigateTo($"/keycards/enable/{_card.KeyCardId}");
    }

    private void NavigateToDisable()
    {
        if (_card is null)
        {
            return;
        }

        NavManager.NavigateTo($"/keycards/disable/{_card.KeyCardId}");
    }

    private void NavigateToReplace()
    {
        if (_card is null)
        {
            return;
        }

        NavManager.NavigateTo($"/keycards/replace/{_card.KeyCardId}");
    }
}

