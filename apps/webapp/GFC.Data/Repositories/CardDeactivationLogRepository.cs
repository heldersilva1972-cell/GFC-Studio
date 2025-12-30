namespace GFC.Data.Repositories
{
    using GFC.Core.Interfaces;
    using GFC.Core.Models;
    using Microsoft.Data.SqlClient;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CardDeactivationLogRepository : GFC.Core.Interfaces.ICardDeactivationLogRepository
    {
        private static string GetContext(string methodName) => $"{nameof(CardDeactivationLogRepository)}.{methodName}";
        
        private static readonly string[] ColumnNames = 
        { 
            "LogId", "KeyCardId", "MemberId", "DeactivatedDate", "Reason", "ControllerSynced", "SyncedDate", "Notes" 
        };

        public async Task<int> AddAsync(GFC.Core.Models.CardDeactivationLog log)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                INSERT INTO dbo.CardDeactivationLog 
                (KeyCardId, MemberId, DeactivatedDate, Reason, ControllerSynced, SyncedDate, Notes)
                VALUES 
                (@KeyCardId, @MemberId, @DeactivatedDate, @Reason, @ControllerSynced, @SyncedDate, @Notes);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@KeyCardId", log.KeyCardId);
            command.Parameters.AddWithValue("@MemberId", log.MemberId);
            command.Parameters.AddWithValue("@DeactivatedDate", log.DeactivatedDate == default ? DateTime.Now : log.DeactivatedDate);
            command.Parameters.AddWithValue("@Reason", log.Reason);
            command.Parameters.AddWithValue("@ControllerSynced", log.ControllerSynced);
            command.Parameters.AddWithValue("@SyncedDate", (object?)log.SyncedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object?)log.Notes ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return (int)result;
        }

        public async Task<List<GFC.Core.Models.CardDeactivationLog>> GetByMemberIdAsync(int memberId)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT LogId, KeyCardId, MemberId, DeactivatedDate, Reason, ControllerSynced, SyncedDate, Notes
                FROM dbo.CardDeactivationLog
                WHERE MemberId = @MemberId
                ORDER BY DeactivatedDate DESC";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberId", memberId);

            using var reader = await command.ExecuteReaderAsync();
            var logs = new List<CardDeactivationLog>();
            while (await reader.ReadAsync())
            {
                logs.Add(MapReader(reader));
            }
            return logs;
        }

        public async Task<List<GFC.Core.Models.CardDeactivationLog>> GetByCardIdAsync(int cardId)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT LogId, KeyCardId, MemberId, DeactivatedDate, Reason, ControllerSynced, SyncedDate, Notes
                FROM dbo.CardDeactivationLog
                WHERE KeyCardId = @KeyCardId
                ORDER BY DeactivatedDate DESC";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@KeyCardId", cardId);

            using var reader = await command.ExecuteReaderAsync();
            var logs = new List<CardDeactivationLog>();
            while (await reader.ReadAsync())
            {
                logs.Add(MapReader(reader));
            }
            return logs;
        }

        public async Task UpdateSyncStatusAsync(int logId, bool synced, DateTime? syncedDate)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                UPDATE dbo.CardDeactivationLog
                SET ControllerSynced = @Synced,
                    SyncedDate = @SyncedDate
                WHERE LogId = @LogId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Synced", synced);
            command.Parameters.AddWithValue("@SyncedDate", (object?)syncedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@LogId", logId);

            await command.ExecuteNonQueryAsync();
        }

        private static GFC.Core.Models.CardDeactivationLog MapReader(SqlDataReader reader)
        {
            return new GFC.Core.Models.CardDeactivationLog
            {
                LogId = (int)reader["LogId"],
                KeyCardId = (int)reader["KeyCardId"],
                MemberId = (int)reader["MemberId"],
                DeactivatedDate = (DateTime)reader["DeactivatedDate"],
                Reason = reader["Reason"].ToString() ?? "",
                ControllerSynced = (bool)reader["ControllerSynced"],
                SyncedDate = reader["SyncedDate"] as DateTime?,
                Notes = reader["Notes"] as string
            };
        }
    }
}
