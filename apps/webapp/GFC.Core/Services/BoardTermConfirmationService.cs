using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Persists and retrieves board term confirmation acknowledgements using global notes.
/// </summary>
public class BoardTermConfirmationService : IBoardTermConfirmationService
{
    private const string Category = "BOARD_TERM_CONFIRM";
    private readonly IGlobalNoteRepository _globalNoteRepository;

    public BoardTermConfirmationService(IGlobalNoteRepository globalNoteRepository)
    {
        _globalNoteRepository = globalNoteRepository ?? throw new ArgumentNullException(nameof(globalNoteRepository));
    }

    public BoardTermConfirmation? GetConfirmation(int termYear)
    {
        return GetAllConfirmations()
            .Where(c => c.TermYear == termYear)
            .OrderByDescending(c => c.ConfirmedOnUtc)
            .FirstOrDefault();
    }

    public IReadOnlyList<BoardTermConfirmation> GetAllConfirmations()
    {
        var notes = _globalNoteRepository.GetByCategory(Category);
        var list = new List<BoardTermConfirmation>();

        foreach (var note in notes)
        {
            if (TryDeserialize(note.Text, out var confirmation))
            {
                list.Add(confirmation);
            }
        }

        return list;
    }

    public void Confirm(int termYear, string confirmedBy, string? notes)
    {
        var payload = new BoardTermConfirmation(
            termYear,
            DateTime.UtcNow,
            confirmedBy,
            notes);

        var serialized = JsonSerializer.Serialize(payload);
        var globalNote = new GlobalNote
        {
            NoteDate = DateTime.UtcNow,
            Category = Category,
            Text = serialized
        };

        _globalNoteRepository.InsertNote(globalNote);
    }

    private static bool TryDeserialize(string text, out BoardTermConfirmation confirmation)
    {
        try
        {
            var parsed = JsonSerializer.Deserialize<BoardTermConfirmation>(text);
            if (parsed is null)
            {
                confirmation = default!;
                return false;
            }

            confirmation = parsed;
            return true;
        }
        catch
        {
            confirmation = default!;
            return false;
        }
    }
}

