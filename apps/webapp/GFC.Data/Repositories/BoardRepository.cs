using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository class for Board of Directors data access operations.
/// </summary>
public class BoardRepository : IBoardRepository
{
    private static readonly string[] BoardAssignmentColumnNames =
    {
        "AssignmentID",
        "MemberID",
        "PositionID",
        "TermYear",
        "StartDate",
        "EndDate",
        "Notes",
        "MemberName",
        "PositionName"
    };

    private static string GetContext(string methodName) => $"{nameof(BoardRepository)}.{methodName}";

    /// <summary>
    /// Gets all board positions.
    /// </summary>
    public List<BoardPosition> GetAllPositions()
    {
        var positions = new List<BoardPosition>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT PositionID, PositionName, MaxSeats
            FROM BoardPositions
            ORDER BY PositionName";
        
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            positions.Add(new BoardPosition
            {
                PositionID = (int)reader["PositionID"],
                PositionName = reader["PositionName"].ToString() ?? string.Empty,
                MaxSeats = (int)reader["MaxSeats"]
            });
        }
        
        return positions;
    }

    /// <summary>
    /// Gets a board position by ID.
    /// </summary>
    public BoardPosition? GetPositionById(int positionId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT PositionID, PositionName, MaxSeats
            FROM BoardPositions
            WHERE PositionID = @PositionID";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PositionID", positionId);
        
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new BoardPosition
            {
                PositionID = (int)reader["PositionID"],
                PositionName = reader["PositionName"].ToString() ?? string.Empty,
                MaxSeats = (int)reader["MaxSeats"]
            };
        }
        
        return null;
    }

    /// <summary>
    /// Gets all board assignments with member and position information.
    /// </summary>
    public List<BoardAssignment> GetAllAssignments()
    {
        var assignments = new List<BoardAssignment>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT ba.AssignmentID, ba.MemberID, ba.PositionID, ba.TermYear,
                   ba.StartDate, ba.EndDate, ba.Notes,
                   m.FirstName + ' ' + ISNULL(m.MiddleName + ' ', '') + m.LastName + ISNULL(' ' + m.Suffix, '') AS MemberName,
                   bp.PositionName
            FROM BoardAssignments ba
            INNER JOIN Members m ON ba.MemberID = m.MemberID
            INNER JOIN BoardPositions bp ON ba.PositionID = bp.PositionID
            ORDER BY ba.TermYear DESC, bp.PositionName, m.LastName, m.FirstName";
        
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            assignments.Add(MapReaderToBoardAssignment(reader, nameof(GetAllAssignments)));
        }
        
        return assignments;
    }

    /// <summary>
    /// Gets all board assignments for a specific term year.
    /// </summary>
    public List<BoardAssignment> GetAssignmentsByYear(int termYear)
    {
        var assignments = new List<BoardAssignment>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT ba.AssignmentID, ba.MemberID, ba.PositionID, ba.TermYear,
                   ba.StartDate, ba.EndDate, ba.Notes,
                   m.FirstName + ' ' + ISNULL(m.MiddleName + ' ', '') + m.LastName + ISNULL(' ' + m.Suffix, '') AS MemberName,
                   bp.PositionName
            FROM BoardAssignments ba
            INNER JOIN Members m ON ba.MemberID = m.MemberID
            INNER JOIN BoardPositions bp ON ba.PositionID = bp.PositionID
            WHERE ba.TermYear = @TermYear
            ORDER BY bp.PositionName, m.LastName, m.FirstName";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@TermYear", termYear);
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            assignments.Add(MapReaderToBoardAssignment(reader, nameof(GetAssignmentsByYear)));
        }
        
        return assignments;
    }

    /// <summary>
    /// Gets the most recent term year that has board assignments.
    /// </summary>
    public int? GetMostRecentTermYear()
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT TOP 1 TermYear
            FROM BoardAssignments
            ORDER BY TermYear DESC";
        
        using var command = new SqlCommand(sql, connection);
        var result = command.ExecuteScalar();
        
        return result as int?;
    }

    /// <summary>
    /// Gets all unique term years that have board assignments, ordered by year descending.
    /// </summary>
    public List<int> GetAllTermYears()
    {
        var years = new List<int>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT DISTINCT TermYear
            FROM BoardAssignments
            ORDER BY TermYear DESC";
        
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            years.Add((int)reader["TermYear"]);
        }
        
        return years;
    }

    /// <summary>
    /// Gets all board assignments for a specific member.
    /// </summary>
    public List<BoardAssignment> GetAssignmentsByMember(int memberId)
    {
        var assignments = new List<BoardAssignment>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT ba.AssignmentID, ba.MemberID, ba.PositionID, ba.TermYear,
                   ba.StartDate, ba.EndDate, ba.Notes,
                   m.FirstName + ' ' + ISNULL(m.MiddleName + ' ', '') + m.LastName + ISNULL(' ' + m.Suffix, '') AS MemberName,
                   bp.PositionName
            FROM BoardAssignments ba
            INNER JOIN Members m ON ba.MemberID = m.MemberID
            INNER JOIN BoardPositions bp ON ba.PositionID = bp.PositionID
            WHERE ba.MemberID = @MemberID
            ORDER BY ba.TermYear DESC, bp.PositionName";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            assignments.Add(MapReaderToBoardAssignment(reader, nameof(GetAssignmentsByMember)));
        }
        
        return assignments;
    }

    /// <summary>
    /// Returns true if the specified member holds any board position for the supplied term year.
    /// </summary>
    public bool IsBoardMemberForYear(int memberId, int termYear)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT COUNT(*)
            FROM BoardAssignments
            WHERE MemberID = @MemberID
              AND TermYear = @TermYear";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        command.Parameters.AddWithValue("@TermYear", termYear);

        var count = (int)command.ExecuteScalar();
        return count > 0;
    }

    /// <summary>
    /// Determines whether a member has at least one board assignment.
    /// Uses a fast SELECT TOP 1 query.
    /// </summary>
    public bool HasAssignmentsForMember(int memberId)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            SELECT TOP 1 1
            FROM BoardAssignments
            WHERE MemberID = @MemberID";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);

        var result = command.ExecuteScalar();
        return result != null;
    }

    /// <summary>
    /// Gets the count of assignments for a specific position and term year.
    /// </summary>
    public int GetAssignmentCount(int positionId, int termYear)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            SELECT COUNT(*)
            FROM BoardAssignments
            WHERE PositionID = @PositionID AND TermYear = @TermYear";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PositionID", positionId);
        command.Parameters.AddWithValue("@TermYear", termYear);
        
        return Convert.ToInt32(command.ExecuteScalar());
    }

    /// <summary>
    /// Inserts a new board assignment.
    /// </summary>
    public int InsertAssignment(BoardAssignment assignment)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            INSERT INTO BoardAssignments (MemberID, PositionID, TermYear, StartDate, EndDate, Notes)
            VALUES (@MemberID, @PositionID, @TermYear, @StartDate, @EndDate, @Notes);
            SELECT CAST(SCOPE_IDENTITY() AS INT)";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", assignment.MemberID);
        command.Parameters.AddWithValue("@PositionID", assignment.PositionID);
        command.Parameters.AddWithValue("@TermYear", assignment.TermYear);
        command.Parameters.AddWithValue("@StartDate", (object?)assignment.StartDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@EndDate", (object?)assignment.EndDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@Notes", (object?)assignment.Notes ?? DBNull.Value);
        
        return (int)command.ExecuteScalar();
    }

    /// <summary>
    /// Updates an existing board assignment.
    /// </summary>
    public void UpdateAssignment(BoardAssignment assignment)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            UPDATE BoardAssignments
            SET MemberID = @MemberID,
                PositionID = @PositionID,
                TermYear = @TermYear,
                StartDate = @StartDate,
                EndDate = @EndDate,
                Notes = @Notes
            WHERE AssignmentID = @AssignmentID";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", assignment.MemberID);
        command.Parameters.AddWithValue("@PositionID", assignment.PositionID);
        command.Parameters.AddWithValue("@TermYear", assignment.TermYear);
        command.Parameters.AddWithValue("@StartDate", (object?)assignment.StartDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@EndDate", (object?)assignment.EndDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@Notes", (object?)assignment.Notes ?? DBNull.Value);
        command.Parameters.AddWithValue("@AssignmentID", assignment.AssignmentID);

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Removes an existing board assignment.
    /// </summary>
    public void RemoveAssignment(int assignmentId)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            DELETE FROM BoardAssignments
            WHERE AssignmentID = @AssignmentID";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@AssignmentID", assignmentId);

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Maps a SqlDataReader row to a BoardAssignment object.
    /// </summary>
    private BoardAssignment MapReaderToBoardAssignment(SqlDataReader reader, string context)
    {
        reader.EnsureColumns(GetContext(context), BoardAssignmentColumnNames);

        return new BoardAssignment
        {
            AssignmentID = (int)reader["AssignmentID"],
            MemberID = (int)reader["MemberID"],
            PositionID = (int)reader["PositionID"],
            TermYear = (int)reader["TermYear"],
            StartDate = reader["StartDate"] == DBNull.Value ? null : (DateTime?)reader["StartDate"],
            EndDate = reader["EndDate"] == DBNull.Value ? null : (DateTime?)reader["EndDate"],
            Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),
            MemberName = reader["MemberName"]?.ToString(),
            PositionName = reader["PositionName"]?.ToString()
        };
    }
}


