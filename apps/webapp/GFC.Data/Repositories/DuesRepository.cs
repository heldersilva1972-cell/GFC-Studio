using System.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Repository for reading and writing member dues payments.
/// </summary>
public class DuesRepository : IDuesRepository
{
    private static readonly string[] DuesPaymentColumnNames =
    {
        "DuesPaymentID",
        "MemberID",
        "Year",
        "Amount",
        "PaidDate",
        "PaymentType",
        "Notes"
    };

    private static string GetContext(string methodName) => $"{nameof(DuesRepository)}.{methodName}";

    /// <summary>
    /// Gets all dues payment records.
    /// </summary>
    public List<DuesPayment> GetAllDues()
    {
        var dues = new List<DuesPayment>();

        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT DuesPaymentID, MemberID, Year, Amount, PaidDate, PaymentType, Notes
                FROM DuesPayments";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                dues.Add(MapReaderToDuesPayment(reader, nameof(GetAllDues)));
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return new List<DuesPayment>();
        }

        return dues;
    }

    /// <summary>
    /// Gets all dues payment records for a specific year.
    /// </summary>
    public List<DuesPayment> GetDuesForYear(int year)
    {
        var dues = new List<DuesPayment>();

        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
            SELECT DuesPaymentID, MemberID, Year, Amount, PaidDate, PaymentType, Notes
            FROM DuesPayments
            WHERE Year = @Year";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Year", year);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                dues.Add(MapReaderToDuesPayment(reader, nameof(GetDuesForYear)));
            }
        }
        catch (SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            // Table doesn't exist yet, return empty list safely
            return new List<DuesPayment>();
        }

        return dues;
    }

    /// <summary>
    /// Gets all dues payment records for a specific member across all years.
    /// </summary>
    public List<DuesPayment> GetDuesForMember(int memberId)
    {
        var dues = new List<DuesPayment>();

        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT DuesPaymentID, MemberID, Year, Amount, PaidDate, PaymentType, Notes
                FROM DuesPayments
                WHERE MemberID = @MemberID
                ORDER BY Year, PaidDate";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                dues.Add(MapReaderToDuesPayment(reader, nameof(GetDuesForMember)));
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return new List<DuesPayment>();
        }

        return dues;
    }

    /// <summary>
    /// Gets an existing dues record for a member/year, or null if none exists.
    /// </summary>
    public DuesPayment? GetDuesForMemberYear(int memberId, int year)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT TOP 1 DuesPaymentID, MemberID, Year, Amount, PaidDate, PaymentType, Notes
                FROM DuesPayments
                WHERE MemberID = @MemberID AND Year = @Year";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);
            command.Parameters.AddWithValue("@Year", year);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToDuesPayment(reader, nameof(GetDuesForMemberYear));
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Gets distinct dues years that exist in the DuesPayments table.
    /// </summary>
    public List<int> GetDistinctYears()
    {
        var years = new List<int>();

        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT DISTINCT Year
                FROM DuesPayments
                ORDER BY Year";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader["Year"] is int year)
                {
                    years.Add(year);
                }
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            var currentYear = DateTime.Now.Year;
            return new List<int> { currentYear, currentYear - 1 };
        }

        return years;
    }

    /// <summary>
    /// Inserts or updates a dues payment record for a member/year.
    /// </summary>
    public void UpsertDues(DuesPayment payment)
    {
        using var connection = Db.GetConnection();
        connection.Open();

        // Check if a record already exists for this member/year
        const string selectSql = @"
            SELECT TOP 1 DuesPaymentID
            FROM DuesPayments
            WHERE MemberID = @MemberID AND Year = @Year";

        using var selectCommand = new SqlCommand(selectSql, connection);
        selectCommand.Parameters.AddWithValue("@MemberID", payment.MemberID);
        selectCommand.Parameters.AddWithValue("@Year", payment.Year);

        var existingIdObj = selectCommand.ExecuteScalar();

        if (existingIdObj is int existingId)
        {
            // Update existing record
            const string updateSql = @"
                UPDATE DuesPayments
                SET Amount = @Amount,
                    PaidDate = @PaidDate,
                    PaymentType = @PaymentType,
                    Notes = @Notes
                WHERE DuesPaymentID = @DuesPaymentID";

            using var updateCommand = new SqlCommand(updateSql, connection);
            updateCommand.Parameters.AddWithValue("@DuesPaymentID", existingId);
            AddCommonParameters(updateCommand, payment);

            updateCommand.ExecuteNonQuery();
            payment.DuesPaymentID = existingId;
        }
        else
        {
            // Insert new record
            const string insertSql = @"
                INSERT INTO DuesPayments (MemberID, Year, Amount, PaidDate, PaymentType, Notes)
                VALUES (@MemberID, @Year, @Amount, @PaidDate, @PaymentType, @Notes);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var insertCommand = new SqlCommand(insertSql, connection);
            AddCommonParameters(insertCommand, payment);

            var newId = (int)insertCommand.ExecuteScalar();
            payment.DuesPaymentID = newId;
        }
    }

    /// <summary>
    /// Returns true if the member has a paid (non-waived) dues record for the specified year.
    /// </summary>
    public bool HasPaidForYear(int memberId, int year)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT COUNT(*)
                FROM DuesPayments
                WHERE MemberID = @MemberID
                  AND Year = @Year
                  AND PaidDate IS NOT NULL
                  AND (PaymentType IS NULL OR UPPER(PaymentType) <> 'WAIVED')";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);
            command.Parameters.AddWithValue("@Year", year);

            var count = (int)command.ExecuteScalar();
            return count > 0;
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return false;
        }
    }

    /// <summary>
    /// Ensures a dues record exists for the supplied member/year that is marked as WAIVED with the supplied reason.
    /// Converts existing unpaid records into waived entries when necessary.
    /// </summary>
    public DuesPayment EnsureWaivedDuesRecord(int memberId, int year, string reasonNote)
    {
        var existing = GetDuesForMemberYear(memberId, year);
        if (existing != null &&
            existing.PaidDate.HasValue &&
            !IsWaivedPayment(existing))
        {
            // Already paid via a normal payment – leave it alone.
            return existing;
        }

        var record = existing ?? new DuesPayment
        {
            MemberID = memberId,
            Year = year
        };

        record.PaymentType = "WAIVED";
        record.PaidDate ??= DateTime.Today;
        record.Amount ??= 0m;

        var reason = string.IsNullOrWhiteSpace(reasonNote)
            ? "Waived"
            : reasonNote.Trim();

        if (string.IsNullOrWhiteSpace(record.Notes))
        {
            record.Notes = reason;
        }
        else if (!record.Notes.Contains(reason, StringComparison.OrdinalIgnoreCase))
        {
            record.Notes = $"{record.Notes} | {reason}";
        }

        UpsertDues(record);
        return record;
    }

    /// <summary>
    /// Returns true if the member has any dues records at all.
    /// </summary>
    public bool MemberHasAnyDues(int memberId)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT COUNT(*)
                FROM DuesPayments
                WHERE MemberID = @MemberID";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);

            var count = (int)command.ExecuteScalar();
            return count > 0;
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return false;
        }
    }

    /// <summary>
    /// Returns true if the member has any dues-related history (payments or waivers).
    /// </summary>
    public bool MemberHasDuesHistory(int memberId)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT CASE
                         WHEN EXISTS (SELECT 1 FROM DuesPayments WHERE MemberID = @MemberID)
                              OR EXISTS (SELECT 1 FROM DuesWaiverPeriods WHERE MemberId = @MemberID)
                         THEN 1
                         ELSE 0
                       END";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);

            var result = (int)command.ExecuteScalar();
            return result > 0;
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return false;
        }
    }

    /// <summary>
    /// Returns true if the member has paid or been waived for the specified year.
    /// </summary>
    public bool MemberHasPaidOrWaivedDuesForYear(int memberId, int year)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT CASE
                         WHEN EXISTS (
                             SELECT 1
                             FROM DuesPayments
                             WHERE MemberID = @MemberID
                               AND Year = @Year
                               AND (
                                   PaidDate IS NOT NULL
                                   OR (PaymentType IS NOT NULL AND UPPER(PaymentType) = 'WAIVED')
                               )
                         )
                         OR EXISTS (
                             SELECT 1
                             FROM DuesWaiverPeriods
                             WHERE MemberId = @MemberID
                               AND @Year BETWEEN StartYear AND EndYear
                         )
                         OR EXISTS (
                             SELECT 1
                             FROM Waivers
                             WHERE MemberId = @MemberID
                               AND Year = @Year
                         )
                         THEN 1
                         ELSE 0
                       END";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);
            command.Parameters.AddWithValue("@Year", year);

            var result = (int)command.ExecuteScalar();
            return result > 0;
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return false;
        }
    }

    private static DuesPayment MapReaderToDuesPayment(SqlDataReader reader, string context)
    {
        reader.EnsureColumns(GetContext(context), DuesPaymentColumnNames);

        return new DuesPayment
        {
            DuesPaymentID = (int)reader["DuesPaymentID"],
            MemberID = (int)reader["MemberID"],
            Year = (int)reader["Year"],
            Amount = reader["Amount"] is DBNull ? null : (decimal?)reader["Amount"],
            PaidDate = reader["PaidDate"] is DBNull ? null : (DateTime?)reader["PaidDate"],
            PaymentType = reader["PaymentType"] as string,
            Notes = reader["Notes"] as string
        };
    }

    private static void AddCommonParameters(SqlCommand command, DuesPayment payment)
    {
        command.Parameters.AddWithValue("@MemberID", payment.MemberID);
        command.Parameters.AddWithValue("@Year", payment.Year);
        command.Parameters.AddWithValue("@Amount", (object?)payment.Amount ?? DBNull.Value);
        command.Parameters.AddWithValue("@PaidDate", (object?)payment.PaidDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@PaymentType", (object?)payment.PaymentType ?? DBNull.Value);
        command.Parameters.AddWithValue("@Notes", (object?)payment.Notes ?? DBNull.Value);
    }

    private static bool IsWaivedPayment(DuesPayment payment)
    {
        return string.Equals(payment.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsPaidPayment(DuesPayment payment)
    {
        return payment.PaidDate.HasValue && !IsWaivedPayment(payment);
    }

    /// <summary>
    /// Updates an existing dues record based on its primary key.
    /// </summary>
    public void UpdateDuesRecord(DuesPayment payment)
    {
        if (payment.DuesPaymentID <= 0)
            throw new ArgumentException("Payment must have a valid ID to update.", nameof(payment));

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            UPDATE DuesPayments
            SET Amount = @Amount,
                PaidDate = @PaidDate,
                PaymentType = @PaymentType,
                Notes = @Notes
            WHERE DuesPaymentID = @DuesPaymentID";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@DuesPaymentID", payment.DuesPaymentID);
        command.Parameters.AddWithValue("@Amount", (object?)payment.Amount ?? DBNull.Value);
        command.Parameters.AddWithValue("@PaidDate", (object?)payment.PaidDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@PaymentType", (object?)payment.PaymentType ?? DBNull.Value);
        command.Parameters.AddWithValue("@Notes", (object?)payment.Notes ?? DBNull.Value);

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Retrieves dues records for a member whose Year falls within the supplied inclusive range.
    /// </summary>
    public List<DuesPayment> GetDuesForMemberBetweenYears(int memberId, int startYear, int endYear)
    {
        var dues = new List<DuesPayment>();

        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT DuesPaymentID, MemberID, Year, Amount, PaidDate, PaymentType, Notes
                FROM DuesPayments
                WHERE MemberID = @MemberID
                  AND Year BETWEEN @StartYear AND @EndYear";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MemberID", memberId);
            command.Parameters.AddWithValue("@StartYear", startYear);
            command.Parameters.AddWithValue("@EndYear", endYear);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                dues.Add(MapReaderToDuesPayment(reader, nameof(GetDuesForMemberBetweenYears)));
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return new List<DuesPayment>();
        }

        return dues;
    }

    /// <summary>
    /// Returns dues records that conflict with a waiver (already Paid or Waived) within the range.
    /// </summary>
    public List<DuesPayment> GetConflictingWaiverDues(int memberId, int startYear, int endYear)
    {
        return GetDuesForMemberBetweenYears(memberId, startYear, endYear)
            .Where(r => IsWaivedPayment(r) || IsPaidPayment(r))
            .ToList();
    }

    public static string GetPaymentStatus(DuesPayment payment)
    {
        if (IsWaivedPayment(payment))
        {
            return "Waived";
        }

        if (IsPaidPayment(payment))
        {
            return "Paid";
        }

        return "Unpaid";
    }

    public void AppendNoteToUnpaidDues(int memberId, string note)
    {
        if (memberId <= 0)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(note))
        {
            return;
        }

        using var connection = Db.GetConnection();
        connection.Open();

        const string sql = @"
            UPDATE DuesPayments
            SET Notes = CASE
                WHEN Notes IS NULL OR LTRIM(RTRIM(Notes)) = '' THEN @Note
                ELSE Notes + CHAR(13) + CHAR(10) + @Note
            END
            WHERE MemberID = @MemberID
              AND PaidDate IS NULL";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        command.Parameters.AddWithValue("@Note", note);

        command.ExecuteNonQuery();
    }

}



