using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Provides CRUD operations for physical key cards.
/// </summary>
public class KeyCardRepository : IKeyCardRepository
{
    private static readonly string[] KeyCardColumnNames =
    {
        "KeyCardId",
        "MemberID",
        "CardNumber",
        "Notes"
    };

    private static string GetContext(string methodName) => $"{nameof(KeyCardRepository)}.{methodName}";

    public KeyCard? GetById(int keyCardId)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT KeyCardId, MemberID, CardNumber, Notes
            FROM dbo.KeyCards
            WHERE KeyCardId = @KeyCardId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@KeyCardId", keyCardId);

        using var reader = command.ExecuteReader();
        return reader.Read() ? MapReader(reader, nameof(GetById)) : null;
    }

    public KeyCard? GetByCardNumber(string cardNumber)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT KeyCardId, MemberID, CardNumber, Notes
            FROM dbo.KeyCards
            WHERE CardNumber = @CardNumber";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CardNumber", cardNumber);

        using var reader = command.ExecuteReader();
        return reader.Read() ? MapReader(reader, nameof(GetByCardNumber)) : null;
    }

    public List<KeyCard> GetAll()
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT KeyCardId, MemberID, CardNumber, Notes
            FROM dbo.KeyCards
            ORDER BY KeyCardId DESC";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        var cards = new List<KeyCard>();
        while (reader.Read())
        {
            cards.Add(MapReader(reader, nameof(GetAll)));
        }

        return cards;
    }

    public KeyCard Create(string cardNumber, int memberId, string? notes)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            INSERT INTO dbo.KeyCards (MemberID, CardNumber, Notes)
            VALUES (@MemberID, @CardNumber, @Notes);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        command.Parameters.AddWithValue("@CardNumber", cardNumber);
        command.Parameters.AddWithValue("@Notes", (object?)notes ?? DBNull.Value);

        var newId = (int)command.ExecuteScalar();
        return GetById(newId)!;
    }

    public void Update(KeyCard card)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            UPDATE dbo.KeyCards
            SET MemberID = @MemberID,
                CardNumber = @CardNumber,
                Notes = @Notes
            WHERE KeyCardId = @KeyCardId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", card.MemberId);
        command.Parameters.AddWithValue("@CardNumber", card.CardNumber);
        command.Parameters.AddWithValue("@Notes", (object?)card.Notes ?? DBNull.Value);
        command.Parameters.AddWithValue("@KeyCardId", card.KeyCardId);

        command.ExecuteNonQuery();
    }

    private static KeyCard MapReader(SqlDataReader reader, string context)
    {
        reader.EnsureColumns(GetContext(context), KeyCardColumnNames);

        return new KeyCard
        {
            KeyCardId = (int)reader["KeyCardId"],
            MemberId = (int)reader["MemberID"],
            CardNumber = reader["CardNumber"].ToString() ?? string.Empty,
            IsActive = true,
            Notes = reader["Notes"] as string,
            CreatedDate = DateTime.MinValue
        };
    }
}


