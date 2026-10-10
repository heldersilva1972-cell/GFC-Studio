using System;
using System.Collections.Generic;

namespace GFC.Core.Models;

public class SalibrationParticipant
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public List<SalibrationTicket> Tickets { get; set; } = new();
}

public class SalibrationTicket
{
    public int Id { get; set; }
    public int ParticipantId { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsWinner { get; set; }
    public DateTime? WonAt { get; set; }
}

public class SalibrationRollConfig
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Label { get; set; } = "Main Roll";
    public string Prefix { get; set; } = "1987";
    public int DigitCount { get; set; } = 3;
    public string ColorBadge { get; set; } = "primary"; // primary (blue), danger (red), success (green), warning (amber), purple
}

public class SalibrationSettings
{
    public string PageTitle { get; set; } = "Sal-ibration";
    public string TicketPrefix { get; set; } = "1987";
    public int VariableDigitCount { get; set; } = 3;
    public string ThankYouMessage { get; set; } = "Thank you so much for your generosity and incredible support! Your contribution makes a real difference in our community fundraiser. We’ve saved your numbers — good luck in the drawing! 🌟";
    public int ThankYouDisplaySeconds { get; set; } = 5;
    public List<SalibrationRollConfig> RollConfigs { get; set; } = new();

    public List<SalibrationRollConfig> GetActiveRollConfigs()
    {
        if (RollConfigs != null && RollConfigs.Count > 0)
        {
            return RollConfigs;
        }

        return new List<SalibrationRollConfig>
        {
            new SalibrationRollConfig
            {
                Id = "default-1",
                Label = "Main Roll",
                Prefix = string.IsNullOrEmpty(TicketPrefix) ? "1987" : TicketPrefix,
                DigitCount = VariableDigitCount > 0 ? VariableDigitCount : 3,
                ColorBadge = "primary"
            }
        };
    }
}

public class SalibrationWinnerResult
{
    public bool Found { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public SalibrationParticipant? Participant { get; set; }
    public DateTime? RegisteredAt { get; set; }
    public int TotalTicketsHeld { get; set; }
}
