using Microsoft.JSInterop;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services;

public class TutorialService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IMemberRepository _memberRepository;
    private readonly IKeyCardRepository _keyCardRepository;
    private readonly IDuesRepository _duesRepository;
    private readonly ILogger<TutorialService> _logger;

    private const string STORAGE_KEY = "gfc_tutorial_state";
    private const int SAMPLE_MEMBER_ID = -999;

    public event Action? OnTutorialStateChanged;

    public TutorialState CurrentState { get; private set; } = new();

    public TutorialService(
        IJSRuntime jsRuntime,
        IMemberRepository memberRepository,
        IKeyCardRepository keyCardRepository,
        IDuesRepository duesRepository,
        ILogger<TutorialService> logger)
    {
        _jsRuntime = jsRuntime;
        _memberRepository = memberRepository;
        _keyCardRepository = keyCardRepository;
        _duesRepository = duesRepository;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var stateJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", STORAGE_KEY);
            if (!string.IsNullOrEmpty(stateJson))
            {
                CurrentState = System.Text.Json.JsonSerializer.Deserialize<TutorialState>(stateJson) ?? new();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load tutorial state from localStorage");
            CurrentState = new TutorialState();
        }
    }

    public async Task StartTutorialAsync(bool useSampleMode)
    {
        try
        {
            CurrentState = new TutorialState
            {
                IsActive = true,
                CurrentStep = 1,
                UseSampleMode = useSampleMode,
                StartedAtUtc = DateTime.UtcNow
            };

            if (useSampleMode)
            {
                await CreateSampleMemberAsync();
            }

            await SaveStateAsync();
            NotifyStateChanged();
            
            _logger.LogInformation("Tutorial started successfully. Step: {Step}, SampleMode: {SampleMode}", 
                CurrentState.CurrentStep, useSampleMode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CRITICAL: Failed to start tutorial");
            // Reset state on error
            CurrentState = new TutorialState { IsActive = false };
            // Don't throw - let the caller handle it gracefully
        }
    }

    public async Task NextStepAsync()
    {
        if (!CurrentState.IsActive) return;

        CurrentState.CurrentStep++;
        CurrentState.LastUpdatedUtc = DateTime.UtcNow;

        await SaveStateAsync();
        NotifyStateChanged();
    }

    public async Task GoToStepAsync(int stepNumber)
    {
        if (!CurrentState.IsActive) return;

        CurrentState.CurrentStep = stepNumber;
        CurrentState.LastUpdatedUtc = DateTime.UtcNow;

        await SaveStateAsync();
        NotifyStateChanged();
    }

    public async Task CompleteTutorialAsync()
    {
        if (CurrentState.UseSampleMode && CurrentState.SampleMemberId.HasValue)
        {
            await CleanupSampleDataAsync();
        }

        CurrentState = new TutorialState
        {
            IsActive = false,
            CompletedAtUtc = DateTime.UtcNow
        };

        await SaveStateAsync();
        NotifyStateChanged();
    }

    public async Task CancelTutorialAsync()
    {
        if (CurrentState.UseSampleMode && CurrentState.SampleMemberId.HasValue)
        {
            await CleanupSampleDataAsync();
        }

        CurrentState = new TutorialState { IsActive = false };
        await SaveStateAsync();
        NotifyStateChanged();
    }

    private async Task CreateSampleMemberAsync()
    {
        try
        {
            // SIMPLIFIED: Don't actually create in database - just track the ID
            // The database insert is causing circuit breaks
            CurrentState.SampleMemberId = SAMPLE_MEMBER_ID;
            _logger.LogInformation("Tutorial will use sample member ID: {MemberId}", SAMPLE_MEMBER_ID);
            
            /* DISABLED: Database operations cause circuit breaks
            // Check if sample member already exists
            var existing = _memberRepository.GetMemberById(SAMPLE_MEMBER_ID);
            if (existing != null)
            {
                CurrentState.SampleMemberId = SAMPLE_MEMBER_ID;
                return;
            }

            var sampleMember = new Member
            {
                MemberID = SAMPLE_MEMBER_ID,
                FirstName = "Tutorial",
                LastName = "Demo",
                Status = "REGULAR",
                ApplicationDate = DateTime.Today,
                AcceptedDate = DateTime.Today,
                DateOfBirth = new DateTime(1990, 1, 1),
                Address1 = "123 Sample Street",
                City = "Demo City",
                State = "MA",
                PostalCode = "02101",
                Email = "tutorial@demo.local",
                Phone = "555-0100",
                Notes = "AUTO-GENERATED TUTORIAL SAMPLE - WILL BE DELETED"
            };

            var memberId = _memberRepository.InsertMember(sampleMember);
            CurrentState.SampleMemberId = memberId;

            _logger.LogInformation("Created sample member for tutorial: ID {MemberId}", memberId);
            */
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create sample member for tutorial");
        }
        
        await Task.CompletedTask;
    }

    private async Task CleanupSampleDataAsync()
    {
        try
        {
            if (!CurrentState.SampleMemberId.HasValue) return;

            var memberId = CurrentState.SampleMemberId.Value;

            // Delete any key cards
            var cards = _keyCardRepository.GetAll().Where(c => c.MemberId == memberId).ToList();
            foreach (var card in cards)
            {
                _keyCardRepository.Delete(card.KeyCardId);
            }

            // Note: IDuesRepository doesn't have a Delete method, so we'll leave dues records
            // They will be orphaned when the member is deleted, which is acceptable for tutorial cleanup
            // Alternatively, we could update them to mark as deleted or use UpsertDues to clear them

            // Delete the member
            _memberRepository.DeleteMember(memberId);

            _logger.LogInformation("Cleaned up sample tutorial data for member ID {MemberId}", memberId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cleanup sample tutorial data");
        }

        await Task.CompletedTask;
    }

    private async Task SaveStateAsync()
    {
        // DISABLED: JSInterop causes circuit breaks
        // TODO: Re-enable once JSInterop is stable
        /*
        try
        {
            var stateJson = System.Text.Json.JsonSerializer.Serialize(CurrentState);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", STORAGE_KEY, stateJson);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to save tutorial state to localStorage");
        }
        */
        await Task.CompletedTask;
    }

    private void NotifyStateChanged() => OnTutorialStateChanged?.Invoke();

    public bool ShouldShowStep(int stepNumber) => CurrentState.IsActive && CurrentState.CurrentStep == stepNumber;

    public int GetSampleMemberId() => CurrentState.SampleMemberId ?? SAMPLE_MEMBER_ID;
}

public class TutorialState
{
    public bool IsActive { get; set; }
    public int CurrentStep { get; set; }
    public bool UseSampleMode { get; set; }
    public int? SampleMemberId { get; set; }
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? LastUpdatedUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
}
