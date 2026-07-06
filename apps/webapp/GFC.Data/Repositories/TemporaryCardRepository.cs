using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GFC.Data.Repositories
{
    public class TemporaryCardRepository : ITemporaryCardRepository
    {
        public async Task<TemporaryCard?> GetByIdAsync(int id)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT TemporaryCardId, CardNumber, HolderName, Purpose, IsActive, ActiveFrom, ActiveTo, CreatedAt
                FROM dbo.TemporaryCards
                WHERE TemporaryCardId = @Id";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<TemporaryCard?> GetActiveByCardNumberAsync(string cardNumber)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT TemporaryCardId, CardNumber, HolderName, Purpose, IsActive, ActiveFrom, ActiveTo, CreatedAt
                FROM dbo.TemporaryCards
                WHERE CardNumber = @CardNumber AND IsActive = 1";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CardNumber", cardNumber);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReader(reader);
            }
            return null;
        }

        public async Task<List<TemporaryCard>> GetAllAsync()
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT TemporaryCardId, CardNumber, HolderName, Purpose, IsActive, ActiveFrom, ActiveTo, CreatedAt
                FROM dbo.TemporaryCards
                ORDER BY CreatedAt DESC";

            using var command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();
            var list = new List<TemporaryCard>();
            while (await reader.ReadAsync())
            {
                list.Add(MapReader(reader));
            }
            return list;
        }

        public async Task<int> AddAsync(TemporaryCard card)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                INSERT INTO dbo.TemporaryCards 
                (CardNumber, HolderName, Purpose, IsActive, ActiveFrom, ActiveTo, CreatedAt)
                VALUES 
                (@CardNumber, @HolderName, @Purpose, @IsActive, @ActiveFrom, @ActiveTo, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CardNumber", (object?)card.CardNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@HolderName", card.HolderName);
            command.Parameters.AddWithValue("@Purpose", card.Purpose);
            command.Parameters.AddWithValue("@IsActive", card.IsActive);
            command.Parameters.AddWithValue("@ActiveFrom", card.ActiveFrom);
            command.Parameters.AddWithValue("@ActiveTo", card.ActiveTo);
            command.Parameters.AddWithValue("@CreatedAt", card.CreatedAt == default ? DateTime.Now : card.CreatedAt);

            var result = await command.ExecuteScalarAsync();
            return (int)result;
        }

        public async Task UpdateAsync(TemporaryCard card)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = @"
                UPDATE dbo.TemporaryCards
                SET CardNumber = @CardNumber,
                    HolderName = @HolderName,
                    Purpose = @Purpose,
                    IsActive = @IsActive,
                    ActiveFrom = @ActiveFrom,
                    ActiveTo = @ActiveTo
                WHERE TemporaryCardId = @TemporaryCardId";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CardNumber", (object?)card.CardNumber ?? DBNull.Value);
            command.Parameters.AddWithValue("@HolderName", card.HolderName);
            command.Parameters.AddWithValue("@Purpose", card.Purpose);
            command.Parameters.AddWithValue("@IsActive", card.IsActive);
            command.Parameters.AddWithValue("@ActiveFrom", card.ActiveFrom);
            command.Parameters.AddWithValue("@ActiveTo", card.ActiveTo);
            command.Parameters.AddWithValue("@TemporaryCardId", card.TemporaryCardId);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();

            const string sql = "DELETE FROM dbo.TemporaryCards WHERE TemporaryCardId = @Id";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }

        private static TemporaryCard MapReader(SqlDataReader reader)
        {
            return new TemporaryCard
            {
                TemporaryCardId = (int)reader["TemporaryCardId"],
                CardNumber = reader["CardNumber"] is DBNull ? null : reader["CardNumber"].ToString(),
                HolderName = reader["HolderName"].ToString() ?? string.Empty,
                Purpose = reader["Purpose"].ToString() ?? string.Empty,
                IsActive = (bool)reader["IsActive"],
                ActiveFrom = (DateTime)reader["ActiveFrom"],
                ActiveTo = (DateTime)reader["ActiveTo"],
                CreatedAt = (DateTime)reader["CreatedAt"]
            };
        }
    }
}
