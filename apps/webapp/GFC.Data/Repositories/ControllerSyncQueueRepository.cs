using System.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository for managing controller sync queue operations
/// </summary>
public class ControllerSyncQueueRepository : IControllerSyncQueueRepository
{
    public async Task<int> AddAsync(ControllerSyncQueueItem item)
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            INSERT INTO dbo.ControllerSyncQueue 
                (KeyCardId, CardNumber, Action, QueuedDate, AttemptCount, LastError, Status)
            VALUES 
                (@KeyCardId, @CardNumber, @Action, @QueuedDate, @AttemptCount, @LastError, @Status);
            SELECT CAST(SCOPE_IDENTITY() as int);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@KeyCardId", item.KeyCardId);
        command.Parameters.AddWithValue("@CardNumber", item.CardNumber);
        command.Parameters.AddWithValue("@Action", item.Action);
        command.Parameters.AddWithValue("@QueuedDate", item.QueuedDate);
        command.Parameters.AddWithValue("@AttemptCount", item.AttemptCount);
        command.Parameters.AddWithValue("@LastError", (object?)item.LastError ?? DBNull.Value);
        command.Parameters.AddWithValue("@Status", item.Status);

        var queueId = (int)await command.ExecuteScalarAsync();
        return queueId;
    }

    public async Task<List<ControllerSyncQueueItem>> GetPendingItemsAsync()
    {
        return await GetByStatusAsync("PENDING");
    }

    public async Task<int> GetPendingCountAsync()
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = "SELECT COUNT(*) FROM dbo.ControllerSyncQueue WHERE Status = 'PENDING'";

        using var command = new SqlCommand(sql, connection);
        return (int)await command.ExecuteScalarAsync();
    }

    public async Task UpdateStatusAsync(int queueId, string status)
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            UPDATE dbo.ControllerSyncQueue 
            SET Status = @Status
            WHERE QueueId = @QueueId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@QueueId", queueId);
        command.Parameters.AddWithValue("@Status", status);

        await command.ExecuteNonQueryAsync();
    }

    public async Task MarkAsCompletedAsync(int queueId)
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            UPDATE dbo.ControllerSyncQueue 
            SET Status = 'COMPLETED',
                CompletedDate = GETDATE()
            WHERE QueueId = @QueueId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@QueueId", queueId);

        await command.ExecuteNonQueryAsync();
    }

    public async Task IncrementAttemptAsync(int queueId, string error)
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            UPDATE dbo.ControllerSyncQueue 
            SET AttemptCount = AttemptCount + 1,
                LastAttemptDate = GETDATE(),
                LastError = @LastError,
                Status = 'PENDING'
            WHERE QueueId = @QueueId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@QueueId", queueId);
        command.Parameters.AddWithValue("@LastError", error ?? string.Empty);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<ControllerSyncQueueItem>> GetByStatusAsync(string status)
    {
        var items = new List<ControllerSyncQueueItem>();

        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            SELECT QueueId, KeyCardId, CardNumber, Action, QueuedDate, 
                   AttemptCount, LastAttemptDate, LastError, Status, CompletedDate
            FROM dbo.ControllerSyncQueue
            WHERE Status = @Status
            ORDER BY QueuedDate ASC";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Status", status);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            items.Add(MapFromReader(reader));
        }

        return items;
    }

    public async Task<ControllerSyncQueueItem?> GetByIdAsync(int queueId)
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            SELECT QueueId, KeyCardId, CardNumber, Action, QueuedDate, 
                   AttemptCount, LastAttemptDate, LastError, Status, CompletedDate
            FROM dbo.ControllerSyncQueue
            WHERE QueueId = @QueueId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@QueueId", queueId);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task DeleteCompletedOlderThanAsync(int days)
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            DELETE FROM dbo.ControllerSyncQueue
            WHERE Status = 'COMPLETED' 
            AND CompletedDate < DATEADD(day, -@Days, GETDATE())";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Days", days);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<DateTime?> GetLastCompletedTimeAsync()
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = "SELECT MAX(CompletedDate) FROM dbo.ControllerSyncQueue WHERE Status = 'COMPLETED'";

        using var command = new SqlCommand(sql, connection);
        var result = await command.ExecuteScalarAsync();
        return result == DBNull.Value ? null : (DateTime?)result;
    }

    public async Task ResetPendingAsync()
    {
        using var connection = Db.GetConnection();
        await connection.OpenAsync();

        const string sql = @"
            UPDATE dbo.ControllerSyncQueue 
            SET Status = 'PENDING',
                AttemptCount = 0,
                LastAttemptDate = NULL
            WHERE Status IN ('PENDING', 'PROCESSING')";

        using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }

    private static ControllerSyncQueueItem MapFromReader(SqlDataReader reader)
    {
        return new ControllerSyncQueueItem
        {
            QueueId = reader.GetInt32(reader.GetOrdinal("QueueId")),
            KeyCardId = reader.GetInt32(reader.GetOrdinal("KeyCardId")),
            CardNumber = reader.GetString(reader.GetOrdinal("CardNumber")),
            Action = reader.GetString(reader.GetOrdinal("Action")),
            QueuedDate = reader.GetDateTime(reader.GetOrdinal("QueuedDate")),
            AttemptCount = reader.GetInt32(reader.GetOrdinal("AttemptCount")),
            LastAttemptDate = reader.IsDBNull(reader.GetOrdinal("LastAttemptDate")) 
                ? null 
                : reader.GetDateTime(reader.GetOrdinal("LastAttemptDate")),
            LastError = reader.IsDBNull(reader.GetOrdinal("LastError")) 
                ? null 
                : reader.GetString(reader.GetOrdinal("LastError")),
            Status = reader.GetString(reader.GetOrdinal("Status")),
            CompletedDate = reader.IsDBNull(reader.GetOrdinal("CompletedDate")) 
                ? null 
                : reader.GetDateTime(reader.GetOrdinal("CompletedDate"))
        };
    }
}
