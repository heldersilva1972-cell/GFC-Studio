using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace GFC.Mobile.Services;

public class MobileSalibrationService : ISalibrationService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private const string LocalParticipantsKey = "gfc_salibration_participants";
    private const string LocalSettingsKey = "gfc_salibration_settings";

    public MobileSalibrationService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task<SalibrationSettings> GetSettingsAsync()
    {
        try
        {
            var cachedJson = await _js.InvokeAsync<string>("localStorage.getItem", LocalSettingsKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                var settings = JsonSerializer.Deserialize<SalibrationSettings>(cachedJson);
                if (settings != null) return settings;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SalibrationService] Error reading settings from local storage: {ex.Message}");
        }

        // Try API fallback
        try
        {
            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(3));
            var apiSettings = await _http.GetFromJsonAsync<SalibrationSettings>("api/salibration/settings", cts.Token);
            if (apiSettings != null)
            {
                await SaveSettingsAsync(apiSettings);
                return apiSettings;
            }
        }
        catch { }

        return new SalibrationSettings();
    }

    public async Task<bool> SaveSettingsAsync(SalibrationSettings settings)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings);
            await _js.InvokeVoidAsync("localStorage.setItem", LocalSettingsKey, json);
            
            // Background sync to server if available
            _ = Task.Run(async () =>
            {
                try
                {
                    using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(5));
                    await _http.PostAsJsonAsync("api/salibration/settings", settings, cts.Token);
                }
                catch { }
            });

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SalibrationService] Error saving settings: {ex.Message}");
            return false;
        }
    }

    public async Task<List<SalibrationParticipant>> GetAllParticipantsAsync()
    {
        try
        {
            var cachedJson = await _js.InvokeAsync<string>("localStorage.getItem", LocalParticipantsKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                var list = JsonSerializer.Deserialize<List<SalibrationParticipant>>(cachedJson);
                if (list != null) return list;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SalibrationService] Error reading participants: {ex.Message}");
        }

        return new List<SalibrationParticipant>();
    }

    public async Task<SalibrationParticipant> SaveParticipantWithTicketsAsync(SalibrationParticipant participant, List<string> ticketNumbers)
    {
        var participants = await GetAllParticipantsAsync();

        if (participant.Id <= 0)
        {
            participant.Id = participants.Any() ? participants.Max(p => p.Id) + 1 : 1;
            participant.CreatedAt = DateTime.Now;
        }
        else
        {
            participants.RemoveAll(p => p.Id == participant.Id);
        }

        var tickets = ticketNumbers
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select((t, index) => new SalibrationTicket
            {
                Id = index + 1,
                ParticipantId = participant.Id,
                TicketNumber = t.Trim(),
                CreatedAt = DateTime.Now
            }).ToList();

        participant.Tickets = tickets;
        participants.Insert(0, participant);

        try
        {
            var json = JsonSerializer.Serialize(participants);
            await _js.InvokeVoidAsync("localStorage.setItem", LocalParticipantsKey, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SalibrationService] Error saving participants to storage: {ex.Message}");
        }

        // Background server sync
        _ = Task.Run(async () =>
        {
            try
            {
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(5));
                await _http.PostAsJsonAsync("api/salibration/participant", participant, cts.Token);
            }
            catch { }
        });

        return participant;
    }

    public async Task<SalibrationWinnerResult> LookupWinnerByTicketAsync(string ticketNumber)
    {
        if (string.IsNullOrWhiteSpace(ticketNumber))
        {
            return new SalibrationWinnerResult { Found = false, TicketNumber = ticketNumber };
        }

        var cleanSearch = ticketNumber.Trim().ToLowerInvariant();
        var participants = await GetAllParticipantsAsync();

        foreach (var p in participants)
        {
            if (p.Tickets != null && p.Tickets.Any(t => (t.TicketNumber ?? "").Trim().ToLowerInvariant() == cleanSearch))
            {
                var winningTicket = p.Tickets.First(t => (t.TicketNumber ?? "").Trim().ToLowerInvariant() == cleanSearch);
                return new SalibrationWinnerResult
                {
                    Found = true,
                    TicketNumber = winningTicket.TicketNumber,
                    Participant = p,
                    RegisteredAt = winningTicket.CreatedAt,
                    TotalTicketsHeld = p.Tickets.Count
                };
            }
        }

        return new SalibrationWinnerResult
        {
            Found = false,
            TicketNumber = ticketNumber
        };
    }

    public async Task<bool> DeleteParticipantAsync(int participantId)
    {
        var participants = await GetAllParticipantsAsync();
        var removed = participants.RemoveAll(p => p.Id == participantId);
        if (removed > 0)
        {
            try
            {
                var json = JsonSerializer.Serialize(participants);
                await _js.InvokeVoidAsync("localStorage.setItem", LocalParticipantsKey, json);
                return true;
            }
            catch { }
        }
        return false;
    }

    public async Task<int> GetTotalTicketCountAsync()
    {
        var participants = await GetAllParticipantsAsync();
        return participants.Sum(p => p.Tickets?.Count ?? 0);
    }

    public async Task<bool> ClearAllDataAsync()
    {
        try
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", LocalParticipantsKey);
            
            // Sync reset to server in background
            _ = Task.Run(async () =>
            {
                try
                {
                    using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(5));
                    await _http.PostAsync("api/salibration/clear-all", null, cts.Token);
                }
                catch { }
            });

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SalibrationService] Error clearing all data: {ex.Message}");
            return false;
        }
    }
}
