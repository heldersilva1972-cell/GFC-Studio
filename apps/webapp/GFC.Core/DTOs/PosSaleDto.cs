using System;

namespace GFC.Core.DTOs;

public class PosSaleDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string TerminalName { get; set; } = "";
    public string BartenderName { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public string PaymentType { get; set; } = "";
    public string ItemsJson { get; set; } = "";
}
