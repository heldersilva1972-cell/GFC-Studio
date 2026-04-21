namespace GFC.Core.Models;

/// <summary>
/// Represents an acknowledgment that a board term's assignments are complete.
/// </summary>
public sealed record BoardTermConfirmation(
    int TermYear,
    DateTime ConfirmedOnUtc,
    string ConfirmedBy,
    string? Notes);

