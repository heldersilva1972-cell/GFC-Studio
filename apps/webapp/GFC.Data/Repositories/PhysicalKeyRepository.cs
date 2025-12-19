using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories
{
    /// <summary>
    /// Repository for physical door key tracking operations.
    /// </summary>
    public class PhysicalKeyRepository : IPhysicalKeyRepository
    {
        private static readonly string[] PhysicalKeyColumnNames =
        {
            "PhysicalKeyID",
            "MemberID",
            "IssuedDate",
            "ReturnedDate",
            "IssuedBy",
            "ReturnedBy",
            "Notes"
        };

        private static string GetContext(string methodName) => $"{nameof(PhysicalKeyRepository)}.{methodName}";

        public List<PhysicalKey> GetAllKeys()
        {
            var keys = new List<PhysicalKey>();
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT pk.PhysicalKeyID, pk.MemberID, pk.IssuedDate, pk.ReturnedDate, 
                       pk.IssuedBy, pk.ReturnedBy, pk.Notes,
                       m.FirstName + ' ' + ISNULL(m.MiddleName + ' ', '') + m.LastName + ISNULL(' ' + m.Suffix, '') AS MemberName
                FROM dbo.PhysicalKeys pk
                INNER JOIN dbo.Members m ON pk.MemberID = m.MemberID
                ORDER BY pk.IssuedDate DESC";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                keys.Add(MapReader(reader, nameof(GetAllKeys)));
            }

            return keys;
        }

        public List<PhysicalKey> GetActiveKeys()
        {
            var keys = new List<PhysicalKey>();
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT pk.PhysicalKeyID, pk.MemberID, pk.IssuedDate, pk.ReturnedDate, 
                       pk.IssuedBy, pk.ReturnedBy, pk.Notes,
                       m.FirstName + ' ' + ISNULL(m.MiddleName + ' ', '') + m.LastName + ISNULL(' ' + m.Suffix, '') AS MemberName
                FROM dbo.PhysicalKeys pk
                INNER JOIN dbo.Members m ON pk.MemberID = m.MemberID
                WHERE pk.ReturnedDate IS NULL
                ORDER BY pk.IssuedDate DESC";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                keys.Add(MapReader(reader, nameof(GetActiveKeys)));
            }

            return keys;
        }

        public List<PhysicalKey> GetKeysForMember(int memberId)
        {
            var keys = new List<PhysicalKey>();
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT pk.PhysicalKeyID, pk.MemberID, pk.IssuedDate, pk.ReturnedDate, 
                       pk.IssuedBy, pk.ReturnedBy, pk.Notes,
                       m.FirstName + ' ' + ISNULL(m.MiddleName + ' ', '') + m.LastName + ISNULL(' ' + m.Suffix, '') AS MemberName
                FROM dbo.PhysicalKeys pk
                INNER JOIN dbo.Members m ON pk.MemberID = m.MemberID
                WHERE pk.MemberID = @MemberID
                ORDER BY pk.IssuedDate DESC";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                keys.Add(MapReader(reader, nameof(GetKeysForMember)));
            }

            return keys;
        }

        public PhysicalKey? GetKeyById(int keyId)
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT pk.PhysicalKeyID, pk.MemberID, pk.IssuedDate, pk.ReturnedDate, 
                       pk.IssuedBy, pk.ReturnedBy, pk.Notes,
                       m.FirstName + ' ' + ISNULL(m.MiddleName + ' ', '') + m.LastName + ISNULL(' ' + m.Suffix, '') AS MemberName
                FROM dbo.PhysicalKeys pk
                INNER JOIN dbo.Members m ON pk.MemberID = m.MemberID
                WHERE pk.PhysicalKeyID = @PhysicalKeyID";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PhysicalKeyID", keyId);
            using var reader = command.ExecuteReader();

            return reader.Read() ? MapReader(reader, nameof(GetKeyById)) : null;
        }

        public int IssueKey(PhysicalKey key)
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                INSERT INTO dbo.PhysicalKeys (MemberID, IssuedDate, IssuedBy, Notes)
                VALUES (@MemberID, @IssuedDate, @IssuedBy, @Notes);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", key.MemberID);
            command.Parameters.AddWithValue("@IssuedDate", key.IssuedDate);
            command.Parameters.AddWithValue("@IssuedBy", (object?)key.IssuedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object?)key.Notes ?? DBNull.Value);

            return (int)command.ExecuteScalar();
        }

        public void ReturnKey(int keyId, DateTime returnedDate, string? returnedBy)
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                UPDATE dbo.PhysicalKeys
                SET ReturnedDate = @ReturnedDate,
                    ReturnedBy = @ReturnedBy
                WHERE PhysicalKeyID = @PhysicalKeyID";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PhysicalKeyID", keyId);
            command.Parameters.AddWithValue("@ReturnedDate", returnedDate);
            command.Parameters.AddWithValue("@ReturnedBy", (object?)returnedBy ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public void UpdateKey(PhysicalKey key)
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                UPDATE dbo.PhysicalKeys
                SET MemberID = @MemberID,
                    IssuedDate = @IssuedDate,
                    ReturnedDate = @ReturnedDate,
                    IssuedBy = @IssuedBy,
                    ReturnedBy = @ReturnedBy,
                    Notes = @Notes
                WHERE PhysicalKeyID = @PhysicalKeyID";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PhysicalKeyID", key.PhysicalKeyID);
            command.Parameters.AddWithValue("@MemberID", key.MemberID);
            command.Parameters.AddWithValue("@IssuedDate", key.IssuedDate);
            command.Parameters.AddWithValue("@ReturnedDate", (object?)key.ReturnedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@IssuedBy", (object?)key.IssuedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReturnedBy", (object?)key.ReturnedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object?)key.Notes ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public void DeleteKey(int keyId)
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"DELETE FROM dbo.PhysicalKeys WHERE PhysicalKeyID = @PhysicalKeyID";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PhysicalKeyID", keyId);

            command.ExecuteNonQuery();
        }

        private static PhysicalKey MapReader(SqlDataReader reader, string context)
        {
            reader.EnsureColumns(GetContext(context), PhysicalKeyColumnNames);

            return new PhysicalKey
            {
                PhysicalKeyID = (int)reader["PhysicalKeyID"],
                MemberID = (int)reader["MemberID"],
                IssuedDate = (DateTime)reader["IssuedDate"],
                ReturnedDate = reader["ReturnedDate"] as DateTime?,
                IssuedBy = reader["IssuedBy"] as string,
                ReturnedBy = reader["ReturnedBy"] as string,
                Notes = reader["Notes"] as string,
                MemberName = reader["MemberName"] as string
            };
        }
    }
}

