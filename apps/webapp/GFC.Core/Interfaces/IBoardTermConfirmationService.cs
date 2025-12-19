using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IBoardTermConfirmationService
{
    BoardTermConfirmation? GetConfirmation(int termYear);
    IReadOnlyList<BoardTermConfirmation> GetAllConfirmations();
    void Confirm(int termYear, string confirmedBy, string? notes);
}

