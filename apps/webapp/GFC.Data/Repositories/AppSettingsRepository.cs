using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

public class AppSettingsRepository : IAppSettingsRepository
{
    public AppSettings? GetSetting(string key)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                SELECT SettingKey, SettingValue, Description, LastModified, ModifiedBy
                FROM AppSettings
                WHERE SettingKey = @Key";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Key", key);
            using var reader = command.ExecuteReader();
            return reader.Read() ? MapReaderToSetting(reader) : null;
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            // Table doesn't exist yet - return null
            return null;
        }
    }

    public void SetSetting(string key, string value, string? modifiedBy = null)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            const string sql = @"
                IF EXISTS (SELECT 1 FROM AppSettings WHERE SettingKey = @Key)
                    UPDATE AppSettings
                    SET SettingValue = @Value, LastModified = @LastModified, ModifiedBy = @ModifiedBy
                    WHERE SettingKey = @Key
                ELSE
                    INSERT INTO AppSettings (SettingKey, SettingValue, LastModified, ModifiedBy)
                    VALUES (@Key, @Value, @LastModified, @ModifiedBy)";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Key", key);
            command.Parameters.AddWithValue("@Value", value);
            command.Parameters.AddWithValue("@LastModified", DateTime.UtcNow);
            command.Parameters.AddWithValue("@ModifiedBy", (object?)modifiedBy ?? DBNull.Value);
            command.ExecuteNonQuery();
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            throw new InvalidOperationException(
                "The AppSettings table does not exist. Please run the CreateAuthTables.sql script to create the authentication tables.",
                ex);
        }
    }

    public bool GetBoolSetting(string key, bool defaultValue = false)
    {
        try
        {
            var setting = GetSetting(key);
            if (setting == null) return defaultValue;
            return bool.TryParse(setting.SettingValue, out var result) && result;
        }
        catch
        {
            // Return default value on any error
            return defaultValue;
        }
    }

    public void SetBoolSetting(string key, bool value, string? modifiedBy = null)
    {
        SetSetting(key, value.ToString().ToLowerInvariant(), modifiedBy);
    }

    private static AppSettings MapReaderToSetting(SqlDataReader reader)
    {
        return new AppSettings
        {
            SettingKey = (string)reader["SettingKey"],
            SettingValue = (string)reader["SettingValue"],
            Description = reader["Description"] as string,
            LastModified = reader["LastModified"] as DateTime?,
            ModifiedBy = reader["ModifiedBy"] as string
        };
    }
}

