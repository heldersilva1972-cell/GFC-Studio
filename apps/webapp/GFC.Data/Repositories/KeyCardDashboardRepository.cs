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
        
        try
        {
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT
                    m.MemberID,
                    m.FirstName,
                    m.LastName,
                    m.Status AS MemberStatus,
                    COALESCE(dp.PaymentType, 
                        (SELECT TOP 1 'WAIVED' FROM dbo.Waivers WHERE MemberId = m.MemberId AND Year = @Year),
                        (SELECT TOP 1 'WAIVED' FROM dbo.DuesWaiverPeriods WHERE MemberId = m.MemberId AND @Year BETWEEN StartYear AND EndYear)
                    ) AS PaymentType,
                    dp.PaidDate,
                    COALESCE(dpPrev.PaymentType, 
                        (SELECT TOP 1 'WAIVED' FROM dbo.Waivers WHERE MemberId = m.MemberId AND Year = @PreviousYear),
                        (SELECT TOP 1 'WAIVED' FROM dbo.DuesWaiverPeriods WHERE MemberId = m.MemberId AND @PreviousYear BETWEEN StartYear AND EndYear)
                    ) AS PreviousPaymentType,
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
                rows.Add(new KeyCardMemberRow
                {
                    MemberId = (int)reader["MemberID"],
                    FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
                    LastName = reader["LastName"]?.ToString() ?? string.Empty,
                    MemberStatus = reader["MemberStatus"]?.ToString() ?? string.Empty,
                    DuesPaymentType = reader["PaymentType"] as string,
                    DuesPaidDate = reader["PaidDate"] is DBNull ? null : (DateTime?)reader["PaidDate"],
                    PreviousYearPaymentType = reader["PreviousPaymentType"] as string,
                    PreviousYearPaidDate = reader["PreviousPaidDate"] is DBNull ? null : (DateTime?)reader["PreviousPaidDate"],
                    AssignmentId = reader["AssignmentId"] is DBNull ? null : (int?)reader["AssignmentId"],
                    KeyCardId = reader["KeyCardId"] is DBNull ? null : (int?)reader["KeyCardId"],
                    KeyCardNumber = reader["CardNumber"] as string
                });
            }
        }
        catch (SqlException ex) when (ex.Number == 208)
        {
            // Ignore missing tables error and return empty list
        }

        return rows;
    }
}


