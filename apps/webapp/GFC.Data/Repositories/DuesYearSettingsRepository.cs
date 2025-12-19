using System.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository for reading and writing yearly dues settings.
/// </summary>
public class DuesYearSettingsRepository : IDuesYearSettingsRepository
{
    /// <summary>
    /// Gets the dues settings for a specific year, or null if none exist.
    /// </summary>
    public DuesYearSettings? GetSettingsForYear(int year)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT [Year],
                   StandardDues,
                   GraceEndApplied,
                   GraceEndDate
            FROM dbo.DuesYearSettings
            WHERE [Year] = @Year";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Year", year);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new DuesYearSettings
            {
                Year = (int)reader["Year"],
                StandardDues = (decimal)reader["StandardDues"],
                GraceEndApplied = reader["GraceEndApplied"] is DBNull ? false : (bool)reader["GraceEndApplied"],
                GraceEndDate = reader["GraceEndDate"] is DBNull ? null : (DateTime?)reader["GraceEndDate"]
            };
        }

        return null;
    }

    /// <summary>
    /// Returns all configured dues years.
    /// </summary>
    public List<int> GetAllYears()
    {
        var years = new List<int>();

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT [Year]
            FROM dbo.DuesYearSettings
            ORDER BY [Year]";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            years.Add((int)reader["Year"]);
        }

        return years;
    }

    /// <summary>
    /// Inserts or updates the dues settings record for a year.
    /// </summary>
    public void UpsertSettings(DuesYearSettings settings)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
IF EXISTS (SELECT 1 FROM dbo.DuesYearSettings WHERE [Year] = @Year)
BEGIN
    UPDATE dbo.DuesYearSettings
    SET StandardDues = @StandardDues,
        GraceEndApplied = @GraceEndApplied,
        GraceEndDate = @GraceEndDate
    WHERE [Year] = @Year;
END
ELSE
BEGIN
    INSERT INTO dbo.DuesYearSettings ([Year], StandardDues, GraceEndApplied, GraceEndDate)
    VALUES (@Year, @StandardDues, @GraceEndApplied, @GraceEndDate);
END";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Year", settings.Year);
        command.Parameters.AddWithValue("@StandardDues", settings.StandardDues);
        command.Parameters.AddWithValue("@GraceEndApplied", settings.GraceEndApplied);
        command.Parameters.AddWithValue("@GraceEndDate", (object?)settings.GraceEndDate ?? DBNull.Value);

        command.ExecuteNonQuery();
    }

}



