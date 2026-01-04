using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository for member key card assignment history.
/// </summary>
public class MemberKeycardRepository : IMemberKeycardRepository
{
    private readonly KeyCardRepository _keyCardRepository = new();

    private static readonly string[] MemberKeycardColumnNames =
    {
        "AssignmentId",
        "MemberId",
        "KeyCardId",
        "FromDate",
        "ToDate",
        "Reason",
        "ChangedBy"
    };

    private static string GetContext(string methodName) => $"{nameof(MemberKeycardRepository)}.{methodName}";

    public MemberKeycardAssignment? GetCurrentAssignmentForMember(int memberId)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT TOP 1 AssignmentId, MemberId, KeyCardId, FromDate, ToDate, Reason, ChangedBy
                FROM dbo.MemberKeycardAssignments
                WHERE MemberId = @MemberId AND ToDate IS NULL
                ORDER BY FromDate DESC";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberId", memberId);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            var assignment = MapReader(reader, nameof(GetCurrentAssignmentForMember));
            assignment.KeyCard = _keyCardRepository.GetById(assignment.KeyCardId);
            return assignment;
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return null;
        }
    }

    public MemberKeycardAssignment? GetCurrentAssignmentForCard(int keyCardId)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT TOP 1 AssignmentId, MemberId, KeyCardId, FromDate, ToDate, Reason, ChangedBy
            FROM dbo.MemberKeycardAssignments
            WHERE KeyCardId = @KeyCardId AND ToDate IS NULL
            ORDER BY FromDate DESC";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@KeyCardId", keyCardId);

        using var reader = command.ExecuteReader();
        if (!reader.Read())
        {
            return null;
        }

        var assignment = MapReader(reader, nameof(GetCurrentAssignmentForCard));
        assignment.KeyCard = _keyCardRepository.GetById(assignment.KeyCardId);
        return assignment;
    }

    public List<MemberKeycardAssignment> GetHistoryForMember(int memberId)
    {
        var history = new List<MemberKeycardAssignment>();

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT AssignmentId, MemberId, KeyCardId, FromDate, ToDate, Reason, ChangedBy
            FROM dbo.MemberKeycardAssignments
            WHERE MemberId = @MemberId
            ORDER BY FromDate DESC";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberId", memberId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var assignment = MapReader(reader, nameof(GetHistoryForMember));
            assignment.KeyCard = _keyCardRepository.GetById(assignment.KeyCardId);
            history.Add(assignment);
        }

        return history;
    }

    public int AddAssignment(MemberKeycardAssignment assignment)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            INSERT INTO dbo.MemberKeycardAssignments (MemberId, KeyCardId, FromDate, ToDate, Reason, ChangedBy)
            VALUES (@MemberId, @KeyCardId, @FromDate, @ToDate, @Reason, @ChangedBy);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberId", assignment.MemberId);
        command.Parameters.AddWithValue("@KeyCardId", assignment.KeyCardId);
        command.Parameters.AddWithValue("@FromDate", assignment.FromDate);
        command.Parameters.AddWithValue("@ToDate", (object?)assignment.ToDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@Reason", assignment.Reason);
        command.Parameters.AddWithValue("@ChangedBy", (object?)assignment.ChangedBy ?? DBNull.Value);

        var newId = (int)command.ExecuteScalar();
        assignment.AssignmentId = newId;
        return newId;
    }

    public void CloseAssignment(int assignmentId, DateTime toDate, string closingReason)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            UPDATE dbo.MemberKeycardAssignments
            SET ToDate = @ToDate,
                Reason = CASE
                    WHEN Reason IS NULL OR LEN(LTRIM(RTRIM(Reason))) = 0 THEN @ClosingReason
                    ELSE Reason + ' | ' + @ClosingReason
                END
            WHERE AssignmentId = @AssignmentId";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ToDate", toDate);
        command.Parameters.AddWithValue("@ClosingReason", closingReason);
        command.Parameters.AddWithValue("@AssignmentId", assignmentId);

        command.ExecuteNonQuery();
    }

    public int GetActiveAssignmentCount()
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.MemberKeycardAssignments
                WHERE ToDate IS NULL";

            using var command = new SqlCommand(sql, connection);
            var result = command.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return 0;
        }
    }

    private static MemberKeycardAssignment MapReader(SqlDataReader reader, string context)
    {
        reader.EnsureColumns(GetContext(context), MemberKeycardColumnNames);

        return new MemberKeycardAssignment
        {
            AssignmentId = (int)reader["AssignmentId"],
            MemberId = (int)reader["MemberId"],
            KeyCardId = (int)reader["KeyCardId"],
            FromDate = (DateTime)reader["FromDate"],
            ToDate = reader["ToDate"] as DateTime?,
            Reason = reader["Reason"].ToString() ?? string.Empty,
            ChangedBy = reader["ChangedBy"] as string
        };
    }
}


