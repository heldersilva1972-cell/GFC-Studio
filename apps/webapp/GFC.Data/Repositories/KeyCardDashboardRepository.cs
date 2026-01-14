using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

/// <summary>
/// Provides the aggregated data needed to render the Key Cards dashboard quickly.
/// </summary>
public sealed class KeyCardDashboardRepository : IKeyCardDashboardRepository
{
    public List<KeyCardMemberRow> GetMembersForYear(int year)
    {
        var rows = new List<KeyCardMemberRow>();
        var waiverStatusCurrent = new Dictionary<int, string>();
        var waiverStatusPrevious = new Dictionary<int, string>();

        using var connection = Db.GetConnection();
        connection.Open();

        // 1. Fetch Waivers for CURRENT YEAR from NEW table (DuesWaiverPeriods)
        try
        {
            const string sqlWaiversNew = @"
                SELECT MemberId, 'WAIVED' AS Status 
                FROM dbo.DuesWaiverPeriods 
                WHERE @Year BETWEEN StartYear AND EndYear";
            
            using var cmdNew = new SqlCommand(sqlWaiversNew, connection);
            cmdNew.Parameters.AddWithValue("@Year", year);
            using var readerNew = cmdNew.ExecuteReader();
            while (readerNew.Read())
            {
                var memberId = (int)readerNew["MemberId"];
                waiverStatusCurrent[memberId] = "WAIVED";
            }
        }
        catch (SqlException ex) when (ex.Number == 208) { } // Ignore if table missing

        // 2. Fetch Waivers for CURRENT YEAR from LEGACY table (Waivers)
        try
        {
            const string sqlWaiversOld = @"
                SELECT MemberId, 'WAIVED' AS Status 
                FROM dbo.Waivers 
                WHERE Year = @Year";

            using var cmdOld = new SqlCommand(sqlWaiversOld, connection);
            cmdOld.Parameters.AddWithValue("@Year", year);
            using var readerOld = cmdOld.ExecuteReader();
            while (readerOld.Read())
            {
                var memberId = (int)readerOld["MemberId"];
                if (!waiverStatusCurrent.ContainsKey(memberId))
                {
                    waiverStatusCurrent[memberId] = "WAIVED";
                }
            }
        }
        catch (SqlException ex) when (ex.Number == 208) { } // Ignore if table missing

        // 3. Fetch Waivers for PREVIOUS YEAR from NEW table (DuesWaiverPeriods)
        try
        {
            const string sqlWaiversNewPrev = @"
                SELECT MemberId, 'WAIVED' AS Status 
                FROM dbo.DuesWaiverPeriods 
                WHERE @PreviousYear BETWEEN StartYear AND EndYear";
            
            using var cmdNewPrev = new SqlCommand(sqlWaiversNewPrev, connection);
            cmdNewPrev.Parameters.AddWithValue("@PreviousYear", year - 1);
            using var readerNewPrev = cmdNewPrev.ExecuteReader();
            while (readerNewPrev.Read())
            {
                var memberId = (int)readerNewPrev["MemberId"];
                waiverStatusPrevious[memberId] = "WAIVED";
            }
        }
        catch (SqlException ex) when (ex.Number == 208) { } // Ignore if table missing

        // 4. Fetch Waivers for PREVIOUS YEAR from LEGACY table (Waivers)
        try
        {
            const string sqlWaiversOldPrev = @"
                SELECT MemberId, 'WAIVED' AS Status 
                FROM dbo.Waivers 
                WHERE Year = @PreviousYear";

            using var cmdOldPrev = new SqlCommand(sqlWaiversOldPrev, connection);
            cmdOldPrev.Parameters.AddWithValue("@PreviousYear", year - 1);
            using var readerOldPrev = cmdOldPrev.ExecuteReader();
            while (readerOldPrev.Read())
            {
                var memberId = (int)readerOldPrev["MemberId"];
                if (!waiverStatusPrevious.ContainsKey(memberId))
                {
                    waiverStatusPrevious[memberId] = "WAIVED";
                }
            }
        }
        catch (SqlException ex) when (ex.Number == 208) { } // Ignore if table missing

        // 5. Fetch Members and Payments (Standard)
        const string sql = @"
            SELECT
                m.MemberID,
                m.FirstName,
                m.LastName,
                m.Status AS MemberStatus,
                dp.PaymentType,
                dp.PaidDate,
                dpPrev.PaymentType AS PreviousPaymentType,
                dpPrev.PaidDate AS PreviousPaidDate,
                currentAssignment.AssignmentId,
                currentAssignment.KeyCardId,
                kc.CardNumber
            FROM dbo.Members AS m
            LEFT JOIN dbo.DuesPayments AS dp
                ON dp.MemberID = m.MemberID
               AND dp.Year = @Year
            LEFT JOIN dbo.DuesPayments AS dpPrev
                ON dpPrev.MemberID = m.MemberID
               AND dpPrev.Year = @PreviousYear
            OUTER APPLY (
                SELECT TOP 1 AssignmentId, KeyCardId
                FROM dbo.MemberKeycardAssignments
                WHERE MemberId = m.MemberID AND ToDate IS NULL
                ORDER BY FromDate DESC
            ) AS currentAssignment
            LEFT JOIN dbo.KeyCards AS kc
                ON kc.KeyCardId = currentAssignment.KeyCardId
            WHERE m.Status <> 'DECEASED'
            ORDER BY m.LastName, m.FirstName;";

        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Year", year);
        command.Parameters.AddWithValue("@PreviousYear", year - 1);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var memberId = (int)reader["MemberID"];
            var dbPaymentType = reader["PaymentType"] as string;
            var dbPreviousPaymentType = reader["PreviousPaymentType"] as string;
            
            // Merge waiver status for CURRENT year
            var finalPaymentType = dbPaymentType;
            if (string.IsNullOrEmpty(finalPaymentType) && waiverStatusCurrent.ContainsKey(memberId))
            {
                finalPaymentType = "WAIVED";
            }

            // Merge waiver status for PREVIOUS year
            var finalPreviousPaymentType = dbPreviousPaymentType;
            if (string.IsNullOrEmpty(finalPreviousPaymentType) && waiverStatusPrevious.ContainsKey(memberId))
            {
                finalPreviousPaymentType = "WAIVED";
            }

            rows.Add(new KeyCardMemberRow
            {
                MemberId = memberId,
                FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
                LastName = reader["LastName"]?.ToString() ?? string.Empty,
                MemberStatus = reader["MemberStatus"]?.ToString() ?? string.Empty,
                DuesPaymentType = finalPaymentType,
                DuesPaidDate = reader["PaidDate"] is DBNull ? null : (DateTime?)reader["PaidDate"],
                PreviousYearPaymentType = finalPreviousPaymentType,
                PreviousYearPaidDate = reader["PreviousPaidDate"] is DBNull ? null : (DateTime?)reader["PreviousPaidDate"],
                AssignmentId = reader["AssignmentId"] is DBNull ? null : (int?)reader["AssignmentId"],
                KeyCardId = reader["KeyCardId"] is DBNull ? null : (int?)reader["KeyCardId"],
                KeyCardNumber = reader["CardNumber"] as string
            });
        }

        return rows;
    }
}


