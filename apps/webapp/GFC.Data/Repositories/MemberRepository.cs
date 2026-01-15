using System.Data;
using System.Globalization;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace GFC.Data.Repositories;

/*
 * SQL to add new columns to Members table:
 * 
 * ALTER TABLE Members ADD MiddleName NVARCHAR(50) NULL;
 * ALTER TABLE Members ADD Suffix NVARCHAR(20) NULL;
 * ALTER TABLE Members ADD CellPhone NVARCHAR(20) NULL;
 * ALTER TABLE Members ADD LifeEligibleDate DATE NULL;
 * ALTER TABLE Members ADD DateOfDeath DATE NULL;
 * ALTER TABLE Members ADD InactiveDate DATE NULL;
 */

/// <summary>
/// Repository class for Member data access operations.
/// </summary>
public class MemberRepository : IMemberRepository
{
    private static readonly string[] MemberColumnNames =
    {
        "MemberID",
        "FirstName",
        "MiddleName",
        "LastName",
        "Suffix",
        "Status",
        "IsNonPortugueseOrigin",
        "ApplicationDate",
        "AcceptedDate",
        "StatusChangeDate",
        "DateOfBirth",
        "DateOfDeath",
        "InactiveDate",
        "Notes",
        "Address1",
        "City",
        "State",
        "PostalCode",
        "Phone",
        "CellPhone",
        "Email",
        "LifeEligibleDate"
    };

    private static readonly string MemberSelectColumns = string.Join(", ", MemberColumnNames);

    private static string GetContext(string methodName) => $"{nameof(MemberRepository)}.{methodName}";

    /// <summary>
    /// Gets all members from the database.
    /// </summary>
    public List<Member> GetAllMembers()
    {
        var members = new List<Member>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            var sql = $@"
                SELECT {MemberSelectColumns}
                FROM Members
                ORDER BY LastName, FirstName";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                members.Add(MapReaderToMember(reader, nameof(GetAllMembers)));
            }
        }
        catch (SqlException ex)
        {
            // [DEBUGGING] Log the error instead of swallowing it
            Console.WriteLine($"[MemberRepository Error] Database: {ex.Message}");
            
            if (ex.Number == 208) 
            {
                Console.WriteLine("[MemberRepository] Table 'Members' not found (Error 208). Check connection string and database content.");
            }
            throw; 
        }
        
        return members;
    }

    /// <summary>
    /// Gets a member by ID.
    /// </summary>
    public Member? GetMemberById(int memberId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        var sql = $@"
            SELECT {MemberSelectColumns}
            FROM Members
            WHERE MemberID = @MemberID";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapReaderToMember(reader, nameof(GetMemberById));
        }
        
        return null;
    }

    public (int newMembers, int deactivatedMembers) GetMemberChangeSummaryForYear(int year)
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
SELECT
    SUM(CASE WHEN AcceptedDate >= @StartDate AND AcceptedDate < @EndDate THEN 1 ELSE 0 END) AS NewMembers,
    SUM(CASE WHEN Status = 'INACTIVE' AND InactiveDate >= @StartDate AND InactiveDate < @EndDate THEN 1 ELSE 0 END) AS DeactivatedMembers
FROM Members;";

            var startDate = new DateTime(year, 1, 1);
            var endDate = startDate.AddYears(1);

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@StartDate", startDate);
            command.Parameters.AddWithValue("@EndDate", endDate);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var newMembers = reader["NewMembers"] is DBNull ? 0 : (int)reader["NewMembers"];
                var deactivated = reader["DeactivatedMembers"] is DBNull ? 0 : (int)reader["DeactivatedMembers"];
                return (newMembers, deactivated);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetMemberChangeSummaryForYear: {ex.Message}");
            throw;
        }

        return (0, 0);
    }

    /// <summary>
    /// Searches members by last name (case-insensitive partial match).
    /// </summary>
    public List<Member> SearchByLastName(string lastName)
    {
        var members = new List<Member>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        var sql = $@"
            SELECT {MemberSelectColumns}
            FROM Members
            WHERE LastName LIKE @LastName
            ORDER BY LastName, FirstName";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@LastName", $"%{lastName}%");
        
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            members.Add(MapReaderToMember(reader, nameof(SearchByLastName)));
        }
        
        return members;
    }

    /// <summary>
    /// Inserts a new member into the database.
    /// </summary>
    public int InsertMember(Member member)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            INSERT INTO Members (Status, FirstName, MiddleName, LastName, Suffix, Address1, 
                                City, State, PostalCode, Phone, CellPhone, Email, ApplicationDate, 
                                AcceptedDate, StatusChangeDate, DateOfBirth, Notes, IsNonPortugueseOrigin, LifeEligibleDate, DateOfDeath, InactiveDate)
            VALUES (@Status, @FirstName, @MiddleName, @LastName, @Suffix, @Address1, 
                    @City, @State, @PostalCode, @Phone, @CellPhone, @Email, @ApplicationDate, 
                    @AcceptedDate, @StatusChangeDate, @DateOfBirth, @Notes, @IsNonPortugueseOrigin, @LifeEligibleDate, @DateOfDeath, @InactiveDate);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
        
        using var command = new SqlCommand(sql, connection);
        AddMemberParameters(command, member);
        
        var newId = (int)command.ExecuteScalar();
        return newId;
    }

    /// <summary>
    /// Updates an existing member in the database.
    /// </summary>
    public void UpdateMember(Member member)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"
            UPDATE Members
            SET Status = @Status,
                FirstName = @FirstName,
                MiddleName = @MiddleName,
                LastName = @LastName,
                Suffix = @Suffix,
                Address1 = @Address1,
                City = @City,
                State = @State,
                PostalCode = @PostalCode,
                Phone = @Phone,
                CellPhone = @CellPhone,
                Email = @Email,
                ApplicationDate = @ApplicationDate,
                AcceptedDate = @AcceptedDate,
                StatusChangeDate = @StatusChangeDate,
                DateOfBirth = @DateOfBirth,
                Notes = @Notes,
                IsNonPortugueseOrigin = @IsNonPortugueseOrigin,
                LifeEligibleDate = @LifeEligibleDate,
                DateOfDeath = @DateOfDeath,
                InactiveDate = @InactiveDate
            WHERE MemberID = @MemberID";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", member.MemberID);
        AddMemberParameters(command, member);
        
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Deletes a member from the database (cascade deletes related records).
    /// </summary>
    public void DeleteMember(int memberId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "DELETE FROM Members WHERE MemberID = @MemberID";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MemberID", memberId);
        
        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Gets count of members by status.
    /// </summary>
    public Dictionary<string, int> GetMemberCountsByStatus()
    {
        var counts = new Dictionary<string, int>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            const string sql = @"
                SELECT Status, COUNT(*) AS Count
                FROM Members
                GROUP BY Status";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var status = reader["Status"].ToString() ?? "UNKNOWN";
                var count = Convert.ToInt32(reader["Count"]);
                counts[status] = count;
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetMemberCountsByStatus: {ex.Message}");
            throw;
        }
        
        return counts;
    }

    /// <summary>
    /// Gets the total count of all members.
    /// </summary>
    public int GetTotalMemberCount()
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            const string sql = "SELECT COUNT(*) FROM Members";
            
            using var command = new SqlCommand(sql, connection);
            var count = (int)command.ExecuteScalar();
            
            return count;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetTotalMemberCount: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Maps a SqlDataReader row to a Member object.
    /// </summary>
    private static Member MapReaderToMember(SqlDataReader reader, string context)
    {
        reader.EnsureColumns(GetContext(context), MemberColumnNames);

        DateTime? ReadNullableDateTime(string columnName)
        {
            var value = reader[columnName];
            return value == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(value);
        }

        string? ReadNullableString(string columnName)
        {
            var value = reader[columnName];
            return value == DBNull.Value ? null : value.ToString();
        }

        bool ReadBool(string columnName)
        {
            var value = reader[columnName];
            return value != DBNull.Value && Convert.ToBoolean(value);
        }

        var rawNotes = ReadNullableString("Notes");
        var addressStatus = AddressInvalidNoteHelper.Decode(rawNotes);

        return new Member
        {
            MemberID = (int)reader["MemberID"],
            FirstName = reader["FirstName"].ToString() ?? string.Empty,
            MiddleName = ReadNullableString("MiddleName"),
            LastName = reader["LastName"].ToString() ?? string.Empty,
            Suffix = ReadNullableString("Suffix"),
            Status = reader["Status"].ToString() ?? string.Empty,
            IsNonPortugueseOrigin = ReadBool("IsNonPortugueseOrigin"),
            ApplicationDate = ReadNullableDateTime("ApplicationDate"),
            AcceptedDate = ReadNullableDateTime("AcceptedDate"),
            StatusChangeDate = ReadNullableDateTime("StatusChangeDate"),
            DateOfBirth = ReadNullableDateTime("DateOfBirth"),
            DateOfDeath = ReadNullableDateTime("DateOfDeath"),
            InactiveDate = ReadNullableDateTime("InactiveDate"),
            Notes = addressStatus.CleanNotes,
            Address1 = ReadNullableString("Address1"),
            City = ReadNullableString("City"),
            State = ReadNullableString("State"),
            PostalCode = ReadNullableString("PostalCode"),
            Phone = ReadNullableString("Phone"),
            CellPhone = ReadNullableString("CellPhone"),
            Email = ReadNullableString("Email"),
            LifeEligibleDate = ReadNullableDateTime("LifeEligibleDate"),
            AddressInvalid = addressStatus.IsInvalid,
            AddressInvalidDate = addressStatus.InvalidDate
        };
    }

    /// <summary>
    /// Adds member parameters to a SqlCommand.
    /// </summary>
    private static void AddMemberParameters(SqlCommand command, Member member)
    {
        command.Parameters.AddWithValue("@Status", member.Status);
        command.Parameters.AddWithValue("@FirstName", member.FirstName);
        command.Parameters.AddWithValue("@MiddleName", (object?)member.MiddleName ?? DBNull.Value);
        command.Parameters.AddWithValue("@LastName", member.LastName);
        command.Parameters.AddWithValue("@Suffix", (object?)member.Suffix ?? DBNull.Value);
        command.Parameters.AddWithValue("@Address1", (object?)member.Address1 ?? DBNull.Value);
        command.Parameters.AddWithValue("@City", (object?)member.City ?? DBNull.Value);
        command.Parameters.AddWithValue("@State", (object?)member.State ?? DBNull.Value);
        command.Parameters.AddWithValue("@PostalCode", (object?)member.PostalCode ?? DBNull.Value);
        command.Parameters.AddWithValue("@Phone", (object?)member.Phone ?? DBNull.Value);
        command.Parameters.AddWithValue("@CellPhone", (object?)member.CellPhone ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)member.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@ApplicationDate", (object?)member.ApplicationDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@AcceptedDate", (object?)member.AcceptedDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@StatusChangeDate", (object?)member.StatusChangeDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@DateOfBirth", (object?)member.DateOfBirth ?? DBNull.Value);
        var encodedNotes = AddressInvalidNoteHelper.Encode(member.Notes, member.AddressInvalid, member.AddressInvalidDate);
        command.Parameters.AddWithValue("@Notes", (object?)encodedNotes ?? DBNull.Value);
        command.Parameters.AddWithValue("@IsNonPortugueseOrigin", member.IsNonPortugueseOrigin);
        command.Parameters.AddWithValue("@LifeEligibleDate", (object?)member.LifeEligibleDate ?? DBNull.Value);
        command.Parameters.AddWithValue("@DateOfDeath", (object?)member.DateOfDeath ?? DBNull.Value);
        command.Parameters.AddWithValue("@InactiveDate", (object?)member.InactiveDate ?? DBNull.Value);
    }

    private static class AddressInvalidNoteHelper
    {
        private static readonly Regex TokenRegex = new(@"\[ADDRESS_INVALID:(\d{4}-\d{2}-\d{2})\]", RegexOptions.Compiled);

        public static (bool IsInvalid, DateTime? InvalidDate, string? CleanNotes) Decode(string? notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
            {
                return (false, null, notes);
            }

            var match = TokenRegex.Match(notes);
            if (!match.Success)
            {
                return (false, null, notes);
            }

            DateTime? parsedDate = null;
            if (DateTime.TryParseExact(match.Groups[1].Value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                parsedDate = dt;
            }

            var cleaned = TokenRegex.Replace(notes, string.Empty).Trim();
            return (true, parsedDate, string.IsNullOrWhiteSpace(cleaned) ? null : cleaned);
        }

        public static string? Encode(string? notes, bool isInvalid, DateTime? invalidDate)
        {
            var baseNotes = notes ?? string.Empty;
            var cleaned = TokenRegex.Replace(baseNotes, string.Empty).Trim();

            if (!isInvalid)
            {
                return string.IsNullOrWhiteSpace(cleaned) ? null : cleaned;
            }

            var date = invalidDate ?? DateTime.Today;
            var token = $"[ADDRESS_INVALID:{date:yyyy-MM-dd}]";

            return string.IsNullOrWhiteSpace(cleaned)
                ? token
                : $"{token}\n{cleaned}";
        }
    }

    /// <summary>
    /// Gets distinct City values from the Members table.
    /// </summary>
    public List<string> GetDistinctCities()
    {
        var cities = new List<string>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            const string sql = @"
                SELECT DISTINCT City 
                FROM Members 
                WHERE City IS NOT NULL AND City <> '' 
                ORDER BY City";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var city = reader["City"].ToString();
                if (!string.IsNullOrWhiteSpace(city))
                {
                    cities.Add(city);
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetDistinctCities: {ex.Message}");
            throw;
        }
        
        return cities;
    }

    /// <summary>
    /// Gets distinct State values from the Members table.
    /// </summary>
    public List<string> GetDistinctStates()
    {
        var states = new List<string>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            const string sql = @"
                SELECT DISTINCT State 
                FROM Members 
                WHERE State IS NOT NULL AND State <> '' 
                ORDER BY State";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var state = reader["State"].ToString();
                if (!string.IsNullOrWhiteSpace(state))
                {
                    states.Add(state);
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetDistinctStates: {ex.Message}");
            throw;
        }
        
        return states;
    }

    /// <summary>
    /// Gets distinct PostalCode values from the Members table.
    /// </summary>
    public List<string> GetDistinctPostalCodes()
    {
        var postalCodes = new List<string>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            const string sql = @"
                SELECT DISTINCT PostalCode 
                FROM Members 
                WHERE PostalCode IS NOT NULL AND PostalCode <> '' 
                ORDER BY PostalCode";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var postalCode = reader["PostalCode"].ToString();
                if (!string.IsNullOrWhiteSpace(postalCode))
                {
                    postalCodes.Add(postalCode);
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetDistinctPostalCodes: {ex.Message}");
            throw;
        }
        
        return postalCodes;
    }

    /// <summary>
    /// Gets the count of Non-Portuguese Regular members.
    /// </summary>
    public int GetNonPortugueseRegularCount()
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            const string sql = @"
                SELECT COUNT(*) 
                FROM Members 
                WHERE Status = 'REGULAR' AND IsNonPortugueseOrigin = 1";
            
            using var command = new SqlCommand(sql, connection);
            var count = (int)command.ExecuteScalar();
            
            return count;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetNonPortugueseRegularCount: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gets the queue of Non-Portuguese Guests ordered by AcceptedDate.
    /// 
    /// Non-Portuguese Queue includes:
    ///   - IsNonPortugueseOrigin = 1
    ///   - Status = 'GUEST' (members waiting in the queue)
    /// 
    /// Non-Portuguese Queue excludes:
    ///   - Status = 'REGULAR' (promoted to Regular-NP)
    ///   - Status = 'REGULAR-NP' (if this status exists)
    ///   - Status = 'LIFE' or 'LIFE MEMBER'
    ///   - Status = 'INACTIVE'
    ///   - Status = 'DECEASED'
    ///   - Status = 'REJECTED'
    /// </summary>
    public List<MemberQueueItem> GetNonPortugueseGuestQueue()
    {
        var queue = new List<MemberQueueItem>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            const string sql = @"
                SELECT MemberID, LastName, FirstName, MiddleName, AcceptedDate
                FROM Members
                WHERE Status = 'GUEST' AND IsNonPortugueseOrigin = 1
                ORDER BY AcceptedDate, LastName, FirstName";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            int position = 1;
            while (reader.Read())
            {
                queue.Add(new MemberQueueItem
                {
                    MemberID = (int)reader["MemberID"],
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    MiddleName = reader["MiddleName"] as string,
                    AcceptedDate = reader["AcceptedDate"] as DateTime?,
                    Position = position++
                });
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetNonPortugueseGuestQueue: {ex.Message}");
            throw;
        }
        
        return queue;
    }

    /// <summary>
    /// Gets the number of Non-Portuguese guests currently waiting in the queue.
    /// 
    /// Counts only members with:
    ///   - IsNonPortugueseOrigin = 1
    ///   - Status = 'GUEST' (excludes REGULAR, REGULAR-NP, LIFE, INACTIVE, DECEASED, REJECTED)
    /// </summary>
    public int GetNonPortugueseQueueCount()
    {
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT COUNT(*)
                FROM Members
                WHERE Status = 'GUEST'
                  AND IsNonPortugueseOrigin = 1";

            using var command = new SqlCommand(sql, connection);
            var count = (int)command.ExecuteScalar();
            return count;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetNonPortugueseQueueCount: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gets all members who currently have Life status.
    /// </summary>
    public List<Member> GetLifeMembers()
    {
        var members = new List<Member>();

        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            var sql = $@"
                SELECT {MemberSelectColumns}
                FROM Members
                WHERE UPPER(Status) IN ('LIFE', 'LIFE MEMBER')
                ORDER BY LastName, FirstName";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                members.Add(MapReaderToMember(reader, nameof(GetLifeMembers)));
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetLifeMembers: {ex.Message}");
            throw;
        }

        return members;
    }

    /// <summary>
    /// Gets the count of members who are currently eligible for Life Membership.
    /// Uses the same eligibility logic as IsLifeEligible (15+ REGULAR years and age >= 65).
    /// Note: This method uses in-memory calculation with history repository for accuracy,
    /// as SQL cannot easily determine when a member became REGULAR from change history.
    /// </summary>
    /// <param name="asOfDate">The date to check eligibility as of</param>
    /// <param name="historyRepository">History repository to determine when members became REGULAR</param>
    /// <returns>Count of eligible members</returns>
    public int GetLifeEligibleCount(DateTime asOfDate, IHistoryRepository? historyRepository = null)
    {
        try
        {
            var eligibleMembers = GetLifeEligibleMembers(asOfDate, historyRepository);
            return eligibleMembers.Count;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"[MemberRepository Error] GetLifeEligibleCount: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gets the date when a member became REGULAR status.
    /// Priority order:
    /// 1. HistoryRepository.GetEarliestRegularDate (from change history)
    /// 2. StatusChangeDate if current status is REGULAR
    /// 3. AcceptedDate as fallback (data quality fallback)
    /// </summary>
    /// <param name="member">The member to check</param>
    /// <param name="historyRepository">Optional history repository to check change history</param>
    /// <returns>The date when the member became REGULAR, or null if not available</returns>
    public static DateTime? GetRegularSinceDate(Member member, IHistoryRepository? historyRepository = null)
    {
        // Priority 1: Check change history for when status became REGULAR
        if (historyRepository != null && member.MemberID > 0)
        {
            try
            {
                var historyDate = historyRepository.GetEarliestRegularDate(member.MemberID);
                if (historyDate.HasValue)
                {
                    return historyDate.Value;
                }
            }
            catch
            {
                // If history lookup fails, continue to next priority
            }
        }
        
        // Priority 2: Use StatusChangeDate if current status is REGULAR
        if (string.Equals(member.Status, "REGULAR", StringComparison.OrdinalIgnoreCase) && 
            member.StatusChangeDate.HasValue)
        {
            return member.StatusChangeDate.Value;
        }
        
        // Priority 3: Fallback to AcceptedDate (data quality fallback - not ideal but better than nothing)
        return member.AcceptedDate;
    }
    
    /// <summary>
    /// Calculates the number of whole years between two dates.
    /// </summary>
    private static int GetWholeYears(DateTime startDate, DateTime endDate)
    {
        var years = endDate.Year - startDate.Year;
        if (endDate.Month < startDate.Month || 
            (endDate.Month == startDate.Month && endDate.Day < startDate.Day))
        {
            years--;
        }
        return years;
    }
    
    /// <summary>
    /// Determines if a member is eligible for Life Membership and calculates the eligibility date.
    /// 
    /// By-law rule: Any REGULAR member who has served at least fifteen (15) consecutive years 
    /// of membership in good standing, and has at least attained age 65 at the completion of 
    /// said service, will be granted Life Membership.
    /// 
    /// Eligibility requirements:
    /// - Status must be REGULAR
    /// - Age >= 65
    /// - RegularYears >= 15 (calculated from date became REGULAR, not total membership)
    /// 
    /// LifeEligibilityDate = max(RegularSince + 15 years, DateOfBirth + 65 years)
    /// </summary>
    /// <param name="member">The member to check</param>
    /// <param name="asOfDate">The date to check eligibility as of</param>
    /// <param name="eligibilityDate">Output parameter: the date when the member becomes/will become eligible</param>
    /// <param name="historyRepository">Optional history repository to determine when member became REGULAR</param>
    /// <returns>True if the member is currently eligible, false otherwise</returns>
    public static bool IsLifeEligible(Member member, DateTime asOfDate, out DateTime? eligibilityDate, IHistoryRepository? historyRepository = null)
    {
        eligibilityDate = null;
        
        // Must be REGULAR status
        if (!string.Equals(member.Status, "REGULAR", StringComparison.OrdinalIgnoreCase))
            return false;
        
        // Must have DateOfBirth
        if (!member.DateOfBirth.HasValue)
            return false;
        
        // Get the date when member became REGULAR (priority: history > StatusChangeDate > AcceptedDate)
        var regularSinceDate = GetRegularSinceDate(member, historyRepository);
        if (!regularSinceDate.HasValue)
        {
            // Cannot calculate eligibility without knowing when they became REGULAR
            return false;
        }
        
        var regularSince = regularSinceDate.Value.Date;
        var dateOfBirth = member.DateOfBirth.Value.Date;
        
        // Calculate the two eligibility thresholds
        var minServiceDate = regularSince.AddYears(15);  // 15 years as REGULAR
        var minAgeDate = dateOfBirth.AddYears(65);       // 65 years old
        
        // Eligibility date is the LATER of the two thresholds
        eligibilityDate = minServiceDate > minAgeDate ? minServiceDate : minAgeDate;
        
        // Check if both conditions are met as of the specified date
        var regularYears = GetWholeYears(regularSince, asOfDate);
        var age = GetWholeYears(dateOfBirth, asOfDate);
        
        return regularYears >= 15 && age >= 65 && eligibilityDate <= asOfDate.Date;
    }

    /// <summary>
    /// Gets all members who are currently eligible for Life Membership.
    /// Excludes INACTIVE and DECEASED members.
    /// Uses change history to determine when members became REGULAR for accurate eligibility calculation.
    /// </summary>
    /// <param name="asOfDate">The date to check eligibility as of</param>
    /// <param name="historyRepository">History repository to determine when members became REGULAR</param>
    /// <returns>List of eligible members, sorted by LifeEligibleDate (or calculated eligibility date), then LastName, FirstName</returns>
    public List<Member> GetLifeEligibleMembers(DateTime asOfDate, IHistoryRepository? historyRepository = null)
    {
        var allMembers = GetAllMembers();
        var eligibleMembers = new List<(Member member, DateTime eligibilityDate)>();
        
        foreach (var member in allMembers)
        {
            // Exclude INACTIVE and DECEASED members
            if (member.Status.Equals("INACTIVE", StringComparison.OrdinalIgnoreCase) ||
                member.Status.Equals("DECEASED", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            
            if (IsLifeEligible(member, asOfDate, out var eligibilityDate, historyRepository) && eligibilityDate.HasValue)
            {
                // Use stored LifeEligibleDate if available, otherwise use calculated date
                var finalEligibilityDate = member.LifeEligibleDate ?? eligibilityDate.Value;
                eligibleMembers.Add((member, finalEligibilityDate));
            }
        }
        
        // Sort by eligibility date, then LastName, FirstName
        return eligibleMembers
            .OrderBy(x => x.eligibilityDate)
            .ThenBy(x => x.member.LastName)
            .ThenBy(x => x.member.FirstName)
            .Select(x => x.member)
            .ToList();
    }

    /// <summary>
    /// Gets all active members (REGULAR, GUEST, LIFE MEMBER) excluding INACTIVE and DECEASED.
    /// </summary>
    public List<Member> GetActiveMembers()
    {
        var members = new List<Member>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            var sql = $@"
                SELECT {MemberSelectColumns}
                FROM Members
                WHERE Status IN ('REGULAR', 'GUEST', 'LIFE MEMBER', 'LIFE')
                ORDER BY LastName, FirstName";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                members.Add(MapReaderToMember(reader, nameof(GetActiveMembers)));
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return new List<Member>();
        }
        
        return members;
    }

    /// <summary>
    /// Gets all inactive members.
    /// </summary>
    public List<Member> GetInactiveMembers()
    {
        var members = new List<Member>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            var sql = $@"
                SELECT {MemberSelectColumns}
                FROM Members
                WHERE Status = 'INACTIVE'
                ORDER BY LastName, FirstName";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                members.Add(MapReaderToMember(reader, nameof(GetInactiveMembers)));
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return new List<Member>();
        }
        
        return members;
    }

    /// <summary>
    /// Gets all deceased members.
    /// </summary>
    public List<Member> GetDeceasedMembers()
    {
        var members = new List<Member>();
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            var sql = $@"
                SELECT {MemberSelectColumns}
                FROM Members
                WHERE Status = 'DECEASED'
                ORDER BY LastName, FirstName";
            
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                members.Add(MapReaderToMember(reader, nameof(GetDeceasedMembers)));
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            return new List<Member>();
        }
        
        return members;
    }

    /// <summary>
    /// Finds members by first name, last name, and date of birth (case-insensitive name comparison).
    /// Used for duplicate detection when adding new members.
    /// </summary>
    public List<Member> FindByNameAndDob(string firstName, string lastName, DateTime dateOfBirth)
    {
        var members = new List<Member>();
        
        using var connection = Db.GetConnection();
        connection.Open();
        
        var sql = $@"
            SELECT {MemberSelectColumns}
            FROM Members
            WHERE LOWER(FirstName) = LOWER(@FirstName)
              AND LOWER(LastName) = LOWER(@LastName)
              AND DateOfBirth = @DateOfBirth";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@FirstName", firstName);
        command.Parameters.AddWithValue("@LastName", lastName);
        command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth.Date);
        
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            members.Add(MapReaderToMember(reader, nameof(FindByNameAndDob)));
        }
        
        return members;
    }
}

/// <summary>
/// Represents a member in the Non-Portuguese Guest queue.
/// </summary>
