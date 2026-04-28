using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository for logging and retrieving member change history.
/// </summary>
public class HistoryRepository : IHistoryRepository
{
    /// <summary>
    /// Logs a single field change for a member.
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="fieldName">The name of the field that changed</param>
    /// <param name="oldValue">The old value (can be null or empty)</param>
    /// <param name="newValue">The new value (can be null or empty)</param>
    public void LogMemberChange(int memberId, string fieldName, string? oldValue, string? newValue)
    {
        LogMemberChange(memberId, fieldName, oldValue, newValue, null);
    }

    public void LogMemberChange(int memberId, string fieldName, string? oldValue, string? newValue, string? changedBy)
    {
        // Skip if values are the same (including both null/empty)
        var oldVal = oldValue ?? string.Empty;
        var newVal = newValue ?? string.Empty;
        
        if (oldVal.Equals(newVal, StringComparison.Ordinal))
        {
            return; // No change, don't log
        }
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            INSERT INTO MemberChangeHistory (MemberID, FieldName, OldValue, NewValue, ChangedBy)
            VALUES (@MemberID, @FieldName, @OldValue, @NewValue, @ChangedBy)";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        command.Parameters.AddWithValue("@FieldName", fieldName);
        command.Parameters.AddWithValue("@OldValue", (object?)oldValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@NewValue", (object?)newValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@ChangedBy", (object?)changedBy ?? Environment.UserName);
        
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Returns true if at least one history record exists for the specified member.
    /// </summary>
    public bool MemberHasHistory(int memberId)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT COUNT(1)
            FROM MemberChangeHistory
            WHERE MemberID = @MemberID";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);

        var count = (int)command.ExecuteScalar();
        return count > 0;
    }

    /// <summary>
    /// Gets all change history records for a specific member.
    /// </summary>
    public List<MemberChangeHistory> GetMemberHistory(int memberId)
    {
        var history = new List<MemberChangeHistory>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT ChangeDate, FieldName, OldValue, NewValue, ChangedBy
            FROM MemberChangeHistory
            WHERE MemberID = @MemberID
            ORDER BY ChangeDate DESC, ChangeID DESC";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            history.Add(new MemberChangeHistory
            {
                ChangeDate = (DateTime)reader["ChangeDate"],
                FieldName = reader["FieldName"].ToString() ?? string.Empty,
                OldValue = reader["OldValue"] as string,
                NewValue = reader["NewValue"] as string,
                ChangedBy = reader["ChangedBy"] as string
            });
        }
        return history;
    }

    public List<MemberChangeHistory> GetGlobalHistory(int count = 100)
    {
        var history = new List<MemberChangeHistory>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT TOP (@Count) h.ChangeDate, h.FieldName, h.OldValue, h.NewValue, h.ChangedBy, h.MemberID,
                                m.FirstName, m.LastName, m.MiddleName, m.Suffix
            FROM MemberChangeHistory h
            LEFT JOIN Members m ON h.MemberID = m.MemberID
            ORDER BY h.ChangeDate DESC, h.ChangeID DESC";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Count", count);
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var first = reader["FirstName"]?.ToString() ?? "";
            var last = reader["LastName"]?.ToString() ?? "";
            var middle = reader["MiddleName"]?.ToString() ?? "";
            var suffix = reader["Suffix"]?.ToString() ?? "";

            var mFirst = first.Trim();
            var mLast = last.Trim();
            var mMiddle = string.IsNullOrWhiteSpace(middle) ? "" : " " + middle.Trim()[0] + ".";
            var mSuffix = string.IsNullOrWhiteSpace(suffix) ? "" : " " + suffix.Trim();

            var memberName = string.IsNullOrWhiteSpace(mLast) 
                ? (string.IsNullOrWhiteSpace(mFirst) ? $"ID #{reader["MemberID"]}" : $"{mFirst}{mMiddle}{mSuffix}".Trim())
                : $"{mLast}, {mFirst}{mMiddle}{mSuffix}".Trim().Replace("  ", " ");

            history.Add(new MemberChangeHistory
            {
                ChangeDate = (DateTime)reader["ChangeDate"],
                FieldName = reader["FieldName"].ToString() ?? string.Empty,
                OldValue = reader["OldValue"] as string,
                NewValue = reader["NewValue"] as string,
                ChangedBy = reader["ChangedBy"] as string,
                MemberID = (int)reader["MemberID"],
                MemberName = memberName // Note: Added a MemberName property to the model if it exists, otherwise we'll handle it in the UI
            });
        }
        
        return history;
    }

    public async Task<List<MemberChangeHistory>> GetGlobalHistoryAsync(int count = 100)
    {
        return await Task.Run(() => GetGlobalHistory(count));
    }

    /// <summary>
    /// Gets the date when a member was changed from GUEST to REGULAR status.
    /// Returns the earliest change date where OldValue was GUEST and NewValue was REGULAR.
    /// Returns null if no such change is found.
    /// </summary>
    public DateTime? GetGuestToRegularDate(int memberId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT TOP 1 ChangeDate
            FROM MemberChangeHistory
            WHERE MemberID = @MemberID
              AND FieldName = 'Status'
              AND OldValue LIKE 'GUEST'
              AND NewValue LIKE 'REGULAR'
            ORDER BY ChangeDate ASC";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        
        var result = command.ExecuteScalar();
        return result as DateTime?;
    }

    /// <summary>
    /// Gets the earliest date when a member's status was changed to REGULAR.
    /// Returns the earliest change date where NewValue is REGULAR, regardless of OldValue.
    /// Returns null if no such change is found.
    /// </summary>
    public DateTime? GetEarliestRegularDate(int memberId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT TOP 1 ChangeDate
            FROM MemberChangeHistory
            WHERE MemberID = @MemberID
              AND FieldName = 'Status'
              AND NewValue = 'REGULAR'
            ORDER BY ChangeDate ASC";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        
        var result = command.ExecuteScalar();
        return result as DateTime?;
    }

    public Dictionary<int, DateTime> GetEarliestRegularDates(IEnumerable<int> memberIds)
    {
        var results = new Dictionary<int, DateTime>();
        var idList = memberIds.ToList();
        if (!idList.Any()) return results;

        using var connection = Db.GetConnection();
        connection.Open();

        var chunks = idList.Select((id, index) => new { id, index })
                           .GroupBy(x => x.index / 1000)
                           .Select(g => g.Select(x => x.id).ToList())
                           .ToList();

        foreach (var chunk in chunks)
        {
            var paramNames = chunk.Select((id, i) => $"@id{i}").ToList();
            var sql = $@"
                SELECT MemberID, MIN(ChangeDate) as EarliestDate
                FROM MemberChangeHistory
                WHERE FieldName = 'Status'
                  AND NewValue = 'REGULAR'
                  AND MemberID IN ({string.Join(",", paramNames)})
                GROUP BY MemberID";

            using var command = new SqlCommand(sql, connection);
            for (int i = 0; i < chunk.Count; i++)
            {
                command.Parameters.AddWithValue($"@id{i}", chunk[i]);
            }

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                results[(int)reader["MemberID"]] = (DateTime)reader["EarliestDate"];
            }
        }

        return results;
    }

    /// <summary>
    /// Logs a specific historical event with a manual timestamp.
    /// </summary>
    public void LogHistoricalEvent(int memberId, string fieldName, string? oldValue, string newValue, DateTime eventDate, string? changedBy = null)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        // Ensure we explicitly set the ChangeDate
        const string sql = @"
            INSERT INTO MemberChangeHistory (MemberID, FieldName, OldValue, NewValue, ChangedBy, ChangeDate)
            VALUES (@MemberID, @FieldName, @OldValue, @NewValue, @ChangedBy, @ChangeDate)";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        command.Parameters.AddWithValue("@FieldName", fieldName);
        command.Parameters.AddWithValue("@OldValue", (object?)oldValue ?? DBNull.Value);
        command.Parameters.AddWithValue("@NewValue", newValue); // NewValue is required for historical logs
        command.Parameters.AddWithValue("@ChangedBy", (object?)changedBy ?? Environment.UserName);
        command.Parameters.AddWithValue("@ChangeDate", eventDate);
        
        command.ExecuteNonQuery();
    }
}

/// <summary>
/// Represents a single change history record for display purposes.
/// </summary>
