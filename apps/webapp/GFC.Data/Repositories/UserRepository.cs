using GFC.Core.Helpers;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

public class UserRepository : IUserRepository
{
    public AppUser? GetByUsername(string username)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes, 
                       ISNULL(PasswordChangeRequired, 0) AS PasswordChangeRequired
                FROM AppUsers
                WHERE Username = @Username";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapReaderToUser(reader) : null;
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            // Table doesn't exist yet - return null
            return null;
        }
        catch (SqlException ex) when (ex.Number == 207) // Invalid column name
        {
            // Column doesn't exist yet - need to run migration script
            // Try query without PasswordChangeRequired column
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes
                FROM AppUsers
                WHERE Username = @Username";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = MapReaderToUserLegacy(reader);
                user.PasswordChangeRequired = false; // Default value
                return user;
            }
            return null;
        }
    }

    public AppUser? GetById(int userId)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes, 
                       ISNULL(PasswordChangeRequired, 0) AS PasswordChangeRequired
                FROM AppUsers
                WHERE UserId = @UserId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapReaderToUser(reader) : null;
        }
        catch (SqlException ex) when (ex.Number == 207) // Invalid column name
        {
            // Column doesn't exist yet - need to run migration script
            // Try query without PasswordChangeRequired column
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes
                FROM AppUsers
                WHERE UserId = @UserId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = MapReaderToUserLegacy(reader);
                user.PasswordChangeRequired = false; // Default value
                return user;
            }
            return null;
        }
    }

    public async Task<AppUser?> GetByIdAsync(int userId)
    {
        try
        {
            using var connection = Db.GetConnection();
            await connection.OpenAsync();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId,
                       CreatedDate, LastLoginDate, CreatedBy, Notes,
                       ISNULL(PasswordChangeRequired, 0) AS PasswordChangeRequired
                FROM AppUsers
                WHERE UserId = @UserId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            using var reader = await command.ExecuteReaderAsync();
            return await reader.ReadAsync() ? MapReaderToUser(reader) : null;
        }
        catch (SqlException ex) when (ex.Number == 207) // Invalid column name
        {
            // Column doesn't exist yet - need to run migration script
            // Try query without PasswordChangeRequired column
            using var connection = Db.GetConnection();
            await connection.OpenAsync();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId,
                       CreatedDate, LastLoginDate, CreatedBy, Notes
                FROM AppUsers
                WHERE UserId = @UserId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var user = MapReaderToUserLegacy(reader);
                user.PasswordChangeRequired = false; // Default value
                return user;
            }
            return null;
        }
    }

    public AppUser? GetByMemberId(int memberId)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes, 
                       ISNULL(PasswordChangeRequired, 0) AS PasswordChangeRequired
                FROM AppUsers
                WHERE MemberId = @MemberId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberId", memberId);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapReaderToUser(reader) : null;
        }
        catch (SqlException ex) when (ex.Number == 207) // Invalid column name
        {
            // Column doesn't exist yet - need to run migration script
            // Try query without PasswordChangeRequired column
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes
                FROM AppUsers
                WHERE MemberId = @MemberId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberId", memberId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = MapReaderToUserLegacy(reader);
                user.PasswordChangeRequired = false; // Default value
                return user;
            }
            return null;
        }
    }

    public List<AppUser> GetAllUsers()
    {
        try
        {
            var users = new List<AppUser>();
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes, 
                       ISNULL(PasswordChangeRequired, 0) AS PasswordChangeRequired
                FROM AppUsers
                ORDER BY Username";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapReaderToUser(reader));
            }
            return users;
        }
        catch (SqlException ex) when (ex.Number == 207) // Invalid column name
        {
            // Column doesn't exist yet - need to run migration script
            // Try query without PasswordChangeRequired column
            var users = new List<AppUser>();
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT UserId, Username, PasswordHash, IsAdmin, IsActive, MemberId, 
                       CreatedDate, LastLoginDate, CreatedBy, Notes
                FROM AppUsers
                ORDER BY Username";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var user = MapReaderToUserLegacy(reader);
                user.PasswordChangeRequired = false; // Default value
                users.Add(user);
            }
            return users;
        }
    }

    public int CreateUser(AppUser user)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                INSERT INTO AppUsers (Username, PasswordHash, IsAdmin, IsActive, MemberId, CreatedDate, CreatedBy, Notes, PasswordChangeRequired)
                VALUES (@Username, @PasswordHash, @IsAdmin, @IsActive, @MemberId, @CreatedDate, @CreatedBy, @Notes, @PasswordChangeRequired);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@MemberId", (object?)user.MemberId ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
            command.Parameters.AddWithValue("@CreatedBy", (object?)user.CreatedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object?)user.Notes ?? DBNull.Value);
            command.Parameters.AddWithValue("@PasswordChangeRequired", user.PasswordChangeRequired);
            return (int)command.ExecuteScalar();
        }
        catch (SqlException ex) when (ex.Number == 207) // Invalid column name
        {
            // Column doesn't exist yet - insert without PasswordChangeRequired
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                INSERT INTO AppUsers (Username, PasswordHash, IsAdmin, IsActive, MemberId, CreatedDate, CreatedBy, Notes)
                VALUES (@Username, @PasswordHash, @IsAdmin, @IsActive, @MemberId, @CreatedDate, @CreatedBy, @Notes);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@MemberId", (object?)user.MemberId ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
            command.Parameters.AddWithValue("@CreatedBy", (object?)user.CreatedBy ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object?)user.Notes ?? DBNull.Value);
            return (int)command.ExecuteScalar();
        }
    }

    public void UpdateUser(AppUser user)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                UPDATE AppUsers
                SET Username = @Username,
                    PasswordHash = @PasswordHash,
                    IsAdmin = @IsAdmin,
                    IsActive = @IsActive,
                    MemberId = @MemberId,
                    LastLoginDate = @LastLoginDate,
                    Notes = @Notes,
                    PasswordChangeRequired = @PasswordChangeRequired
                WHERE UserId = @UserId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@MemberId", (object?)user.MemberId ?? DBNull.Value);
            command.Parameters.AddWithValue("@LastLoginDate", (object?)user.LastLoginDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object?)user.Notes ?? DBNull.Value);
            command.Parameters.AddWithValue("@PasswordChangeRequired", user.PasswordChangeRequired);
            command.ExecuteNonQuery();
        }
        catch (SqlException ex) when (ex.Number == 207) // Invalid column name
        {
            // Column doesn't exist yet - update without PasswordChangeRequired
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                UPDATE AppUsers
                SET Username = @Username,
                    PasswordHash = @PasswordHash,
                    IsAdmin = @IsAdmin,
                    IsActive = @IsActive,
                    MemberId = @MemberId,
                    LastLoginDate = @LastLoginDate,
                    Notes = @Notes
                WHERE UserId = @UserId";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@MemberId", (object?)user.MemberId ?? DBNull.Value);
            command.Parameters.AddWithValue("@LastLoginDate", (object?)user.LastLoginDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Notes", (object?)user.Notes ?? DBNull.Value);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteUser(int userId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        const string sql = @"
            UPDATE AppUsers
            SET IsActive = 0
            WHERE UserId = @UserId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.ExecuteNonQuery();
    }

    public void UpdateLastLogin(int userId, DateTime loginDate)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        const string sql = @"
            UPDATE AppUsers
            SET LastLoginDate = @LastLoginDate
            WHERE UserId = @UserId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@LastLoginDate", loginDate);
        command.ExecuteNonQuery();
    }

    public bool UsernameExists(string username, int? excludeUserId = null)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        var sql = @"
            SELECT COUNT(*)
            FROM AppUsers
            WHERE Username = @Username";
        if (excludeUserId.HasValue)
        {
            sql += " AND UserId != @ExcludeUserId";
        }
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", username);
        if (excludeUserId.HasValue)
        {
            command.Parameters.AddWithValue("@ExcludeUserId", excludeUserId.Value);
        }
        return (int)command.ExecuteScalar() > 0;
    }

    public void ClearPasswordChangeRequired(int userId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        const string sql = @"
            UPDATE AppUsers
            SET PasswordChangeRequired = 0
            WHERE UserId = @UserId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.ExecuteNonQuery();
    }

    private static AppUser MapReaderToUser(SqlDataReader reader)
    {
        return new AppUser
        {
            UserId = reader["UserId"] != DBNull.Value ? (int)reader["UserId"] : 0,
            Username = reader["Username"] != DBNull.Value ? reader["Username"].ToString() ?? string.Empty : string.Empty,
            PasswordHash = reader["PasswordHash"] != DBNull.Value ? reader["PasswordHash"].ToString() ?? string.Empty : string.Empty,
            IsAdmin = reader["IsAdmin"] != DBNull.Value && (bool)reader["IsAdmin"],
            IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"],
            MemberId = reader["MemberId"] as int?,
            CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime)reader["CreatedDate"] : DateTime.UtcNow,
            LastLoginDate = reader["LastLoginDate"] as DateTime?,
            CreatedBy = reader["CreatedBy"] as string,
            Notes = reader["Notes"] as string,
            PasswordChangeRequired = reader["PasswordChangeRequired"] as bool? ?? false
        };
    }

    private static AppUser MapReaderToUserLegacy(SqlDataReader reader)
    {
        return new AppUser
        {
            UserId = reader["UserId"] != DBNull.Value ? (int)reader["UserId"] : 0,
            Username = reader["Username"] != DBNull.Value ? reader["Username"].ToString() ?? string.Empty : string.Empty,
            PasswordHash = reader["PasswordHash"] != DBNull.Value ? reader["PasswordHash"].ToString() ?? string.Empty : string.Empty,
            IsAdmin = reader["IsAdmin"] != DBNull.Value && (bool)reader["IsAdmin"],
            IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"],
            MemberId = reader["MemberId"] as int?,
            CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime)reader["CreatedDate"] : DateTime.UtcNow,
            LastLoginDate = reader["LastLoginDate"] as DateTime?,
            CreatedBy = reader["CreatedBy"] as string,
            Notes = reader["Notes"] as string,
            PasswordChangeRequired = false // Default for legacy records
        };
    }

}

