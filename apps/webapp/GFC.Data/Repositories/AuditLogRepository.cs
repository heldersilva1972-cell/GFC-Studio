using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private static bool _isInitialized;
    private static readonly object _initLock = new();

    public void Insert(AuditLogEntry entry)
    {
        try
        {
            InsertInternal(entry);
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            EnsureTableExists();
            InsertInternal(entry);
        }
    }

    private static void InsertInternal(AuditLogEntry entry)
    {
        EnsureInitialized();

        if (entry.PerformedByUserId.HasValue && entry.PerformedByUserId.Value <= 0)
        {
            entry.PerformedByUserId = null;
        }

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
INSERT INTO AuditLogs (TimestampUtc, PerformedByUserId, TargetUserId, Action, Details)
VALUES (@TimestampUtc, @PerformedByUserId, @TargetUserId, @Action, @Details);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@TimestampUtc", entry.TimestampUtc == default ? DateTime.UtcNow : entry.TimestampUtc);
        command.Parameters.AddWithValue("@PerformedByUserId", entry.PerformedByUserId.HasValue ? entry.PerformedByUserId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@TargetUserId", entry.TargetUserId.HasValue ? entry.TargetUserId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@Action", entry.Action);
        command.Parameters.AddWithValue("@Details", (object?)entry.Details ?? DBNull.Value);

        command.ExecuteNonQuery();
    }

    public async Task<PagedResult<AuditLogRecord>> GetAuditLogsAsync(
        string? actionFilter,
        string? searchText,
        DateTimeOffset? from,
        DateTimeOffset? to,
        int pageNumber,
        int pageSize)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }

        if (pageSize <= 0)
        {
            pageSize = 50;
        }

        var filters = new List<string>();
        var parameters = new List<(string Name, object? Value)>();

        if (!string.IsNullOrWhiteSpace(actionFilter))
        {
            filters.Add("al.Action = @ActionFilter");
            parameters.Add(("@ActionFilter", actionFilter));
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            filters.Add("(al.Action LIKE @SearchText OR al.Details LIKE @SearchText)");
            parameters.Add(("@SearchText", $"%{searchText}%"));
        }

        if (from.HasValue)
        {
            filters.Add("al.TimestampUtc >= @FromUtc");
            parameters.Add(("@FromUtc", from.Value.UtcDateTime));
        }

        if (to.HasValue)
        {
            filters.Add("al.TimestampUtc <= @ToUtc");
            parameters.Add(("@ToUtc", to.Value.UtcDateTime));
        }

        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : string.Empty;

        var countSql = $"SELECT COUNT(*) FROM AuditLogs al {whereClause};";

        var pageSql = $@"
SELECT al.AuditLogId, al.TimestampUtc, al.PerformedByUserId, al.TargetUserId, al.Action, al.Details,
       pb.Username AS PerformedByUsername, pb.MemberId AS PerformedByMemberId,
       tb.Username AS TargetUsername, tb.MemberId AS TargetMemberId,
       pbm.FirstName AS PerformedByFirstName, pbm.LastName AS PerformedByLastName,
       tbm.FirstName AS TargetFirstName, tbm.LastName AS TargetLastName
FROM AuditLogs al
LEFT JOIN AppUsers pb ON al.PerformedByUserId = pb.UserId
LEFT JOIN AppUsers tb ON al.TargetUserId = tb.UserId
LEFT JOIN Members pbm ON pb.MemberId = pbm.MemberID
LEFT JOIN Members tbm ON tb.MemberId = tbm.MemberID
{whereClause}
ORDER BY al.TimestampUtc DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        try
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            using var countCommand = new SqlCommand(countSql, connection);
            ApplyParameters(countCommand, parameters);
            var total = Convert.ToInt32(await countCommand.ExecuteScalarAsync());

            using var pageCommand = new SqlCommand(pageSql, connection);
            ApplyParameters(pageCommand, parameters);
            pageCommand.Parameters.AddWithValue("@Offset", (pageNumber - 1) * pageSize);
            pageCommand.Parameters.AddWithValue("@PageSize", pageSize);

            var results = new List<AuditLogRecord>();
            using var reader = await pageCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(MapReaderToRecord(reader));
            }

            return new PagedResult<AuditLogRecord>(results, total, pageNumber, pageSize);
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            // Table has not been created yet; return empty results without altering schema.
            return new PagedResult<AuditLogRecord>(new List<AuditLogRecord>(), 0, pageNumber, pageSize);
        }
    }

    public async Task<IReadOnlyList<string>> GetDistinctActionsAsync()
    {
        try
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = "SELECT DISTINCT Action FROM AuditLogs ORDER BY Action ASC;";
            using var command = new SqlCommand(sql, connection);
            var actions = new List<string>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (reader["Action"] is string action && !string.IsNullOrWhiteSpace(action))
                {
                    actions.Add(action);
                }
            }

            return actions;
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            return Array.Empty<string>();
        }
    }

    private static void EnsureInitialized()
    {
        if (_isInitialized)
        {
            return;
        }

        lock (_initLock)
        {
            if (_isInitialized)
            {
                return;
            }

            EnsureTableExists();
            _isInitialized = true;
        }
    }

    private static void EnsureTableExists()
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[AuditLogs](
        [AuditLogId] INT IDENTITY(1,1) PRIMARY KEY,
        [TimestampUtc] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        [PerformedByUserId] INT NULL,
        [TargetUserId] INT NULL,
        [Action] NVARCHAR(100) NOT NULL,
        [Details] NVARCHAR(MAX) NULL,
        CONSTRAINT [FK_AuditLogs_PerformedBy] FOREIGN KEY ([PerformedByUserId]) REFERENCES [dbo].[AppUsers]([UserId]),
        CONSTRAINT [FK_AuditLogs_Target] FOREIGN KEY ([TargetUserId]) REFERENCES [dbo].[AppUsers]([UserId])
    );

    CREATE INDEX [IX_AuditLogs_TimestampUtc] ON [dbo].[AuditLogs]([TimestampUtc] DESC);
    CREATE INDEX [IX_AuditLogs_Action] ON [dbo].[AuditLogs]([Action]);
END";

        using var command = new SqlCommand(sql, connection);
        command.ExecuteNonQuery();
    }

    private static void ApplyParameters(SqlCommand command, List<(string Name, object? Value)> parameters)
    {
        foreach (var (name, value) in parameters)
        {
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }
    }

    private static AuditLogRecord MapReaderToRecord(SqlDataReader reader)
    {
        var timestampOrdinal = reader.GetOrdinal("TimestampUtc");
        var timestamp = reader.IsDBNull(timestampOrdinal) ? DateTime.UtcNow : reader.GetDateTime(timestampOrdinal);
        
        var performedByValue = reader["PerformedByUserId"];
        var targetValue = reader["TargetUserId"];
        var performedByUserId = performedByValue is DBNull ? (int?)null : Convert.ToInt32(performedByValue);
        var targetUserId = targetValue is DBNull ? (int?)null : Convert.ToInt32(targetValue);

        var performedByMemberName = BuildMemberName(reader, "PerformedByFirstName", "PerformedByLastName");
        var targetMemberName = BuildMemberName(reader, "TargetFirstName", "TargetLastName");

        var actionOrdinal = reader.GetOrdinal("Action");
        var actionValue = reader.IsDBNull(actionOrdinal) ? "Unknown" : reader.GetString(actionOrdinal);

        var auditLogIdOrdinal = reader.GetOrdinal("AuditLogId");
        var auditLogId = reader.IsDBNull(auditLogIdOrdinal) ? 0 : reader.GetInt32(auditLogIdOrdinal);

        return new AuditLogRecord
        {
            AuditLogId = auditLogId,
            TimestampUtc = DateTime.SpecifyKind(timestamp, DateTimeKind.Utc),
            PerformedByUserId = performedByUserId,
            TargetUserId = targetUserId,
            Action = actionValue,
            Details = reader["Details"] as string,
            PerformedByDisplayName = BuildDisplayName(performedByUserId, reader["PerformedByUsername"] as string, performedByMemberName),
            TargetDisplayName = BuildDisplayName(targetUserId, reader["TargetUsername"] as string, targetMemberName)
        };
    }

    private static string BuildDisplayName(int? userId, string? username, string? memberName)
    {
        if (!string.IsNullOrWhiteSpace(memberName))
        {
            return memberName;
        }

        if (!string.IsNullOrWhiteSpace(username))
        {
            return username;
        }

        return userId.HasValue ? $"User #{userId.Value}" : "System";
    }

    private static string? BuildMemberName(SqlDataReader reader, string firstNameColumn, string lastNameColumn)
    {
        var first = reader[firstNameColumn] as string;
        var last = reader[lastNameColumn] as string;

        if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
        {
            return null;
        }

        return $"{last}, {first}";
    }
}
