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

    public int Insert(AuditLogEntry entry)
    {
        try
        {
            return InsertInternal(entry);
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            EnsureTableExists();
            return InsertInternal(entry);
        }
    }

    public async Task<int> InsertAsync(AuditLogEntry entry)
    {
        try
        {
            return await InsertInternalAsync(entry);
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            EnsureTableExists();
            return await InsertInternalAsync(entry);
        }
    }

    private static int InsertInternal(AuditLogEntry entry)
    {
        EnsureInitialized();

        if (entry.PerformedByUserId.HasValue && entry.PerformedByUserId.Value <= 0)
        {
            entry.PerformedByUserId = null;
        }

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
INSERT INTO AuditLogs (TimestampUtc, PerformedByUserId, TargetUserId, TargetMemberId, Action, Details, PageUrl, DurationSeconds, IpAddress, DeviceToken)
OUTPUT INSERTED.AuditLogId
VALUES (@TimestampUtc, @PerformedByUserId, @TargetUserId, @TargetMemberId, @Action, @Details, @PageUrl, @DurationSeconds, @IpAddress, @DeviceToken);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@TimestampUtc", entry.TimestampUtc == default ? DateTime.UtcNow : entry.TimestampUtc);
        command.Parameters.AddWithValue("@PerformedByUserId", entry.PerformedByUserId.HasValue ? entry.PerformedByUserId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@TargetUserId", entry.TargetUserId.HasValue ? entry.TargetUserId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@TargetMemberId", entry.TargetMemberId.HasValue ? entry.TargetMemberId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@Action", entry.Action);
        command.Parameters.AddWithValue("@Details", (object?)entry.Details ?? DBNull.Value);
        command.Parameters.AddWithValue("@PageUrl", (object?)entry.PageUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@DurationSeconds", entry.DurationSeconds);
        command.Parameters.AddWithValue("@IpAddress", (object?)entry.IpAddress ?? DBNull.Value);
        command.Parameters.AddWithValue("@DeviceToken", (object?)entry.DeviceToken ?? DBNull.Value);

        return (int)command.ExecuteScalar();
    }

    private static async Task<int> InsertInternalAsync(AuditLogEntry entry)
    {
        EnsureInitialized();

        if (entry.PerformedByUserId.HasValue && entry.PerformedByUserId.Value <= 0)
        {
            entry.PerformedByUserId = null;
        }

        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
INSERT INTO AuditLogs (TimestampUtc, PerformedByUserId, TargetUserId, TargetMemberId, Action, Details, PageUrl, DurationSeconds, IpAddress, DeviceToken)
OUTPUT INSERTED.AuditLogId
VALUES (@TimestampUtc, @PerformedByUserId, @TargetUserId, @TargetMemberId, @Action, @Details, @PageUrl, @DurationSeconds, @IpAddress, @DeviceToken);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@TimestampUtc", entry.TimestampUtc == default ? DateTime.UtcNow : entry.TimestampUtc);
        command.Parameters.AddWithValue("@PerformedByUserId", entry.PerformedByUserId.HasValue ? entry.PerformedByUserId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@TargetUserId", entry.TargetUserId.HasValue ? entry.TargetUserId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@TargetMemberId", entry.TargetMemberId.HasValue ? entry.TargetMemberId.Value : DBNull.Value);
        command.Parameters.AddWithValue("@Action", entry.Action);
        command.Parameters.AddWithValue("@Details", (object?)entry.Details ?? DBNull.Value);
        command.Parameters.AddWithValue("@PageUrl", (object?)entry.PageUrl ?? DBNull.Value);
        command.Parameters.AddWithValue("@DurationSeconds", entry.DurationSeconds);
        command.Parameters.AddWithValue("@IpAddress", (object?)entry.IpAddress ?? DBNull.Value);
        command.Parameters.AddWithValue("@DeviceToken", (object?)entry.DeviceToken ?? DBNull.Value);

        var result = await command.ExecuteScalarAsync();
        return (int)(result ?? 0);
    }

    public async Task<PagedResult<AuditLogRecord>> GetAuditLogsAsync(
        string? actionFilter,
        string? searchText,
        DateTimeOffset? from,
        DateTimeOffset? to,
        int? targetUserId,
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

        if (targetUserId.HasValue && targetUserId.Value > 0)
        {
            filters.Add("(al.TargetUserId = @TargetUserId OR al.PerformedByUserId = @TargetUserId)");
            parameters.Add(("@TargetUserId", targetUserId.Value));
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

        filters.Add("al.Action <> 'PageView'");
        var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : string.Empty;

        var countSql = $"SELECT COUNT(*) FROM AuditLogs al {whereClause};";

        var pageSql = $@"
SELECT al.AuditLogId, al.TimestampUtc, al.PerformedByUserId, al.TargetUserId, al.TargetMemberId, al.Action, al.Details, al.PageUrl, al.DurationSeconds, al.IpAddress, al.DeviceToken,
       pb.Username AS PerformedByUsername, pb.MemberId AS PerformedByMemberId,
       tb.Username AS TargetUsername, tb.MemberId AS TargetMemberId_AppUser,
       pbm.FirstName AS PerformedByFirstName, pbm.LastName AS PerformedByLastName,
       tbm.FirstName AS TargetFirstName, tbm.LastName AS TargetLastName,
       tm.FirstName AS TargetMemberFirstName, tm.LastName AS TargetMemberLastName
FROM AuditLogs al
LEFT JOIN AppUsers pb ON al.PerformedByUserId = pb.UserId
LEFT JOIN AppUsers tb ON al.TargetUserId = tb.UserId
LEFT JOIN Members pbm ON pb.MemberId = pbm.MemberID
LEFT JOIN Members tbm ON tb.MemberId = tbm.MemberID
LEFT JOIN Members tm ON al.TargetMemberId = tm.MemberID
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

            const string sql = "SELECT DISTINCT Action FROM AuditLogs WHERE Action <> 'PageView' ORDER BY Action ASC;";
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

    public void UpdateDuration(int userId, string pageUrl, int additionalSeconds, string? ipAddress = null, string? deviceToken = null, int? logId = null)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            // [NO GUESSING] If we have a logId, update that exact record.
            // Otherwise, fallback to the latest PageView for this user (compatibility).
            const string sql = @"
UPDATE AuditLogs
SET DurationSeconds = DurationSeconds + @Seconds,
    IpAddress = COALESCE(IpAddress, @IpAddress),
    DeviceToken = COALESCE(DeviceToken, @DeviceToken)
WHERE AuditLogId = COALESCE(@LogId, (
    SELECT TOP 1 AuditLogId 
    FROM AuditLogs
    WHERE PerformedByUserId = @UserId 
      AND Action = 'PageView' 
      AND PageUrl = @PageUrl
      AND TimestampUtc > DATEADD(hour, -2, GETUTCDATE())
    ORDER BY AuditLogId DESC
))";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@PageUrl", pageUrl);
            command.Parameters.AddWithValue("@Seconds", additionalSeconds);
            command.Parameters.AddWithValue("@IpAddress", (object?)ipAddress ?? DBNull.Value);
            command.Parameters.AddWithValue("@DeviceToken", (object?)deviceToken ?? DBNull.Value);
            command.Parameters.AddWithValue("@LogId", (object?)logId ?? DBNull.Value);
            command.ExecuteNonQuery();
        }
        catch { /* Best effort for heartbeat */ }
    }

    public async Task<IReadOnlyList<AuditLogRecord>> GetLiveActivityAsync()
    {
        try
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
SELECT al.AuditLogId, al.TimestampUtc, al.PerformedByUserId, al.TargetUserId, al.TargetMemberId, al.Action, al.Details, al.PageUrl, al.DurationSeconds, al.IpAddress, al.DeviceToken,
       pb.Username AS PerformedByUsername, pb.MemberId AS PerformedByMemberId,
       tb.Username AS TargetUsername, tb.MemberId AS TargetMemberId_AppUser,
       pbm.FirstName AS PerformedByFirstName, pbm.LastName AS PerformedByLastName,
       tbm.FirstName AS TargetFirstName, tbm.LastName AS TargetLastName,
       tm.FirstName AS TargetMemberFirstName, tm.LastName AS TargetMemberLastName
FROM AuditLogs al
INNER JOIN (
    SELECT PerformedByUserId, MAX(AuditLogId) as MaxId
    FROM AuditLogs
    WHERE TimestampUtc > DATEADD(minute, -60, GETUTCDATE())
      AND PerformedByUserId IS NOT NULL
    GROUP BY PerformedByUserId
) latest ON al.AuditLogId = latest.MaxId
LEFT JOIN AppUsers pb ON al.PerformedByUserId = pb.UserId
LEFT JOIN AppUsers tb ON al.TargetUserId = tb.UserId
LEFT JOIN Members pbm ON pb.MemberId = pbm.MemberID
LEFT JOIN Members tbm ON tb.MemberId = tbm.MemberID
LEFT JOIN Members tm ON al.TargetMemberId = tm.MemberID
ORDER BY al.TimestampUtc DESC;";

            var results = new List<AuditLogRecord>();
            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                results.Add(MapReaderToRecord(reader));
            }
            return results;
        }
        catch { return new List<AuditLogRecord>(); }
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
        [PageUrl] NVARCHAR(255) NULL,
        [DurationSeconds] INT NOT NULL DEFAULT 0,
        [IpAddress] NVARCHAR(45) NULL,
        [DeviceToken] NVARCHAR(100) NULL,
        [TargetMemberId] INT NULL,
        CONSTRAINT [FK_AuditLogs_PerformedBy] FOREIGN KEY ([PerformedByUserId]) REFERENCES [dbo].[AppUsers]([UserId]),
        CONSTRAINT [FK_AuditLogs_Target] FOREIGN KEY ([TargetUserId]) REFERENCES [dbo].[AppUsers]([UserId])
    );

    CREATE INDEX [IX_AuditLogs_TimestampUtc] ON [dbo].[AuditLogs]([TimestampUtc] DESC);
    CREATE INDEX [IX_AuditLogs_Action] ON [dbo].[AuditLogs]([Action]);
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND name = N'IpAddress')
    BEGIN
        ALTER TABLE [dbo].[AuditLogs] ADD [IpAddress] NVARCHAR(45) NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND name = N'DeviceToken')
    BEGIN
        ALTER TABLE [dbo].[AuditLogs] ADD [DeviceToken] NVARCHAR(100) NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[AuditLogs]') AND name = N'TargetMemberId')
    BEGIN
        ALTER TABLE [dbo].[AuditLogs] ADD [TargetMemberId] INT NULL;
    END
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
        var timestamp = reader.GetDateTime(reader.GetOrdinal("TimestampUtc"));
        var performedByValue = reader["PerformedByUserId"];
        var targetValue = reader["TargetUserId"];
        var performedByUserId = performedByValue is DBNull ? (int?)null : Convert.ToInt32(performedByValue);
        var targetUserId = targetValue is DBNull ? (int?)null : Convert.ToInt32(targetValue);

        var performedByMemberName = BuildMemberName(reader, "PerformedByFirstName", "PerformedByLastName");
        
        // PRIORITY: 
        // 1. Direct TargetMemberId name (from AuditLogs.TargetMemberId)
        // 2. TargetUserId's linked member name (from tb -> tbm)
        // 3. User's linked member name (legacy check)
        var targetMemberName = BuildMemberName(reader, "TargetMemberFirstName", "TargetMemberLastName") 
                               ?? BuildMemberName(reader, "TargetFirstName", "TargetLastName");
 
        return new AuditLogRecord
        {
            AuditLogId = reader.GetInt32(reader.GetOrdinal("AuditLogId")),
            TimestampUtc = DateTime.SpecifyKind(timestamp, DateTimeKind.Utc),
            PerformedByUserId = performedByUserId,
            TargetUserId = targetUserId,
            TargetMemberId = reader["TargetMemberId"] is DBNull ? (int?)null : Convert.ToInt32(reader["TargetMemberId"]),
            Action = reader.GetString(reader.GetOrdinal("Action")),
            Details = reader["Details"] as string,
            PageUrl = reader["PageUrl"] as string,
            DurationSeconds = reader.IsDBNull(reader.GetOrdinal("DurationSeconds")) ? 0 : reader.GetInt32(reader.GetOrdinal("DurationSeconds")),
            IpAddress = reader["IpAddress"] as string,
            DeviceToken = reader["DeviceToken"] as string,
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
