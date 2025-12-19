using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

public class LoginHistoryRepository : ILoginHistoryRepository
{
    public void LogLogin(LoginHistory history)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                INSERT INTO LoginHistory (UserId, Username, LoginDate, IpAddress, LoginSuccessful, FailureReason)
                VALUES (@UserId, @Username, @LoginDate, @IpAddress, @LoginSuccessful, @FailureReason)";
            using var command = new SqlCommand(sql, connection);

            // If we don't have a valid user id (failed login, unknown user, etc.), store NULL
            if (history.UserId.HasValue && history.UserId.Value > 0)
                command.Parameters.AddWithValue("@UserId", history.UserId.Value);
            else
                command.Parameters.AddWithValue("@UserId", DBNull.Value);

            command.Parameters.AddWithValue("@Username", (object?)history.Username ?? DBNull.Value);
            command.Parameters.AddWithValue("@LoginDate", history.LoginDate);
            command.Parameters.AddWithValue("@IpAddress", (object?)history.IpAddress ?? DBNull.Value);
            command.Parameters.AddWithValue("@LoginSuccessful", history.LoginSuccessful);
            command.Parameters.AddWithValue("@FailureReason", (object?)history.FailureReason ?? DBNull.Value);
            command.ExecuteNonQuery();
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name: table not yet created
        {
            // Table doesn't exist yet – silently skip logging (startup / first-run scenario)
        }
        catch (SqlException ex) when (ex.Number == 547) // FK violation
        {
            // Foreign key conflict (e.g., user deleted after login but before log write).
            // Do NOT crash login – just skip this history row.
            // Optionally log via ILogger if you have it available.
        }
    }

    public List<LoginHistory> GetUserLoginHistory(int userId, int limit = 50)
    {
        var history = new List<LoginHistory>();
        using var connection = Db.GetConnection();
        connection.Open();
        const string sql = @"
            SELECT TOP (@Limit) LoginHistoryId, UserId, Username, LoginDate, IpAddress, LoginSuccessful, FailureReason
            FROM LoginHistory
            WHERE UserId = @UserId
            ORDER BY LoginDate DESC";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Limit", limit);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            history.Add(MapReaderToHistory(reader));
        }
        return history;
    }

    public List<LoginHistory> GetAllLoginHistory(int limit = 100)
    {
        var history = new List<LoginHistory>();
        using var connection = Db.GetConnection();
        connection.Open();
        const string sql = @"
            SELECT TOP (@Limit) LoginHistoryId, UserId, Username, LoginDate, IpAddress, LoginSuccessful, FailureReason
            FROM LoginHistory
            ORDER BY LoginDate DESC";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Limit", limit);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            history.Add(MapReaderToHistory(reader));
        }
        return history;
    }

    private static LoginHistory MapReaderToHistory(SqlDataReader reader)
    {
        return new LoginHistory
        {
            LoginHistoryId = (int)reader["LoginHistoryId"],
            UserId = reader["UserId"] as int?,
            Username = (string)reader["Username"],
            LoginDate = (DateTime)reader["LoginDate"],
            IpAddress = reader["IpAddress"] as string,
            LoginSuccessful = (bool)reader["LoginSuccessful"],
            FailureReason = reader["FailureReason"] as string
        };
    }
}

