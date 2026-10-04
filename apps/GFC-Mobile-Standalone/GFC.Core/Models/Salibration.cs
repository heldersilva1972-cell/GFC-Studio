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

public class SalibrationSettings
{
    public string PageTitle { get; set; } = "Sal-ibration";
    public string TicketPrefix { get; set; } = "1987";
    public int VariableDigitCount { get; set; } = 3;
    public string ThankYouMessage { get; set; } = "Thank you so much for your generosity and incredible support! Your contribution makes a real difference in our community fundraiser. We’ve saved your numbers — good luck in the drawing! 🌟";
    public int ThankYouDisplaySeconds { get; set; } = 5;
}

public class SalibrationWinnerResult
{
    public bool Found { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public SalibrationParticipant? Participant { get; set; }
    public DateTime? RegisteredAt { get; set; }
    public int TotalTicketsHeld { get; set; }
}
