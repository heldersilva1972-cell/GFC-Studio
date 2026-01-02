using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository for managing member dues waiver periods.
/// </summary>
public class DuesWaiverRepository : IDuesWaiverRepository
{
    private static readonly string[] WaiverColumnNames =
    {
        "WaiverId",
        "MemberId",
        "StartYear",
        "EndYear",
        "Reason",
        "CreatedDate",
        "CreatedBy"
    };

    private static string GetContext(string methodName) => $"{nameof(DuesWaiverRepository)}.{methodName}";

    public List<DuesWaiverPeriod> GetWaiversForMember(int memberId)
    {
        var waivers = new List<DuesWaiverPeriod>();

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT WaiverId, MemberId, StartYear, EndYear, Reason, CreatedDate, CreatedBy
            FROM DuesWaiverPeriods
            WHERE MemberId = @MemberId
            ORDER BY StartYear";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberId", memberId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            waivers.Add(MapReader(reader, nameof(GetWaiversForMember)));
        }

        return waivers;
    }

    public List<DuesWaiverPeriod> GetWaiversForYear(int year)
    {
        var waivers = new List<DuesWaiverPeriod>();

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT WaiverId, MemberId, StartYear, EndYear, Reason, CreatedDate, CreatedBy
            FROM DuesWaiverPeriods
            WHERE @Year BETWEEN StartYear AND EndYear";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Year", year);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            waivers.Add(MapReader(reader, nameof(GetWaiversForYear)));
        }

        return waivers;
    }

    public List<DuesWaiverPeriod> GetAllWaivers()
    {
        var waivers = new List<DuesWaiverPeriod>();

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT WaiverId, MemberId, StartYear, EndYear, Reason, CreatedDate, CreatedBy
            FROM DuesWaiverPeriods
            ORDER BY MemberId, StartYear";

        using var command = new SqlCommand(sql, connection);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            waivers.Add(MapReader(reader, nameof(GetAllWaivers)));
        }

        return waivers;
    }

    public bool HasWaiverForYear(int memberId, int year)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT COUNT(*)
            FROM DuesWaiverPeriods
            WHERE MemberId = @MemberId
              AND @Year BETWEEN StartYear AND EndYear";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberId", memberId);
        command.Parameters.AddWithValue("@Year", year);

        var count = (int)command.ExecuteScalar();
        return count > 0;
    }

    public void AddWaiver(DuesWaiverPeriod waiver)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            INSERT INTO DuesWaiverPeriods (MemberId, StartYear, EndYear, Reason, CreatedDate, CreatedBy)
            VALUES (@MemberId, @StartYear, @EndYear, @Reason, @CreatedDate, @CreatedBy);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberId", waiver.MemberId);
        command.Parameters.AddWithValue("@StartYear", waiver.StartYear);
        command.Parameters.AddWithValue("@EndYear", waiver.EndYear);
        command.Parameters.AddWithValue("@Reason", waiver.Reason);
        command.Parameters.AddWithValue("@CreatedDate", waiver.CreatedDate == default ? DateTime.Now : waiver.CreatedDate);
        command.Parameters.AddWithValue("@CreatedBy", (object?)waiver.CreatedBy ?? DBNull.Value);

        waiver.WaiverId = (int)command.ExecuteScalar();
    }

    public void UpdateWaiver(DuesWaiverPeriod waiver)
    {
        if (waiver.WaiverId <= 0)
            throw new ArgumentException("Waiver must have a valid ID to update.", nameof(waiver));

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            UPDATE DuesWaiverPeriods
            SET StartYear = @StartYear,
                EndYear = @EndYear,
                Reason = @Reason
            WHERE WaiverId = @WaiverId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@WaiverId", waiver.WaiverId);
        command.Parameters.AddWithValue("@StartYear", waiver.StartYear);
        command.Parameters.AddWithValue("@EndYear", waiver.EndYear);
        command.Parameters.AddWithValue("@Reason", waiver.Reason);

        command.ExecuteNonQuery();
    }

    public void DeleteWaiver(int waiverId)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = "DELETE FROM DuesWaiverPeriods WHERE WaiverId = @WaiverId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@WaiverId", waiverId);

        command.ExecuteNonQuery();
    }

    public DuesWaiverPeriod? GetWaiverById(int waiverId)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT WaiverId, MemberId, StartYear, EndYear, Reason, CreatedDate, CreatedBy
            FROM DuesWaiverPeriods
            WHERE WaiverId = @WaiverId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@WaiverId", waiverId);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapReader(reader, nameof(GetWaiverById));
        }

        return null;
    }

    private static DuesWaiverPeriod MapReader(SqlDataReader reader, string context)
    {
        reader.EnsureColumns(GetContext(context), WaiverColumnNames);

        return new DuesWaiverPeriod
        {
            WaiverId = (int)reader["WaiverId"],
            MemberId = (int)reader["MemberId"],
            StartYear = (int)reader["StartYear"],
            EndYear = (int)reader["EndYear"],
            Reason = reader["Reason"].ToString() ?? string.Empty,
            CreatedDate = (DateTime)reader["CreatedDate"],
            CreatedBy = reader["CreatedBy"] as string
        };
    }
}


