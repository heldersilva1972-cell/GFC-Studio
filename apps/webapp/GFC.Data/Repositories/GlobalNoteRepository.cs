using System.Linq;
using GFC.Core.Models;
using GFC.Core.Interfaces;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository class for GlobalNote data access operations.
/// </summary>
public class GlobalNoteRepository : IGlobalNoteRepository
{
    /// <summary>
    /// Inserts a new global note into the database.
    /// </summary>
    public int InsertNote(GlobalNote note)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            INSERT INTO GlobalNotes (NoteDate, Category, Text)
            VALUES (@NoteDate, @Category, @Text);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@NoteDate", note.NoteDate);
        command.Parameters.AddWithValue("@Category", (object?)note.Category ?? DBNull.Value);
        command.Parameters.AddWithValue("@Text", note.Text);
        
        var newId = (int)command.ExecuteScalar();
        return newId;
    }

    /// <summary>
    /// Creates a status change note and inserts it.
    /// </summary>
    public void LogStatusChange(int memberId, string oldStatus, string newStatus, string? userName = null)
    {
        var note = new GlobalNote
        {
            NoteDate = DateTime.Now,
            Category = "STATUS CHANGE",
            Text = $"Member ID {memberId}: Status changed from {oldStatus} to {newStatus}" +
                   (string.IsNullOrWhiteSpace(userName) ? "" : $" by {userName}") +
                   $" on {DateTime.Now:yyyy-MM-dd}"
        };
        
        InsertNote(note);
    }

    public IReadOnlyList<GlobalNote> GetByCategory(string category)
    {
        var notes = new List<GlobalNote>();

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT GlobalNoteID, NoteDate, Category, Text
            FROM GlobalNotes
            WHERE Category = @Category
            ORDER BY NoteDate DESC, GlobalNoteID DESC";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Category", category);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            notes.Add(new GlobalNote
            {
                GlobalNoteID = (int)reader["GlobalNoteID"],
                NoteDate = (DateTime)reader["NoteDate"],
                Category = reader["Category"] as string,
                Text = reader["Text"].ToString() ?? string.Empty
            });
        }

        return notes;
    }

    public GlobalNote? GetLatestByCategory(string category)
        => GetByCategory(category).FirstOrDefault();
}


