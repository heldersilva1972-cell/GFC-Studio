using System;
using Microsoft.Data.SqlClient;
using GFC.Data;

namespace Diagnostics;

public class DbCheck
{
    public static void Main()
    {
        try
        {
            // Use same logic as repository
            using var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;MultipleActiveResultSets=true;Connect Timeout=5;");
            connection.Open();
            
            Console.WriteLine("--- Waivers Table ---");
            using var cmd1 = new SqlCommand("SELECT * FROM Waivers", connection);
            using var r1 = cmd1.ExecuteReader();
            while (r1.Read())
            {
                Console.WriteLine($"ID: {r1["Id"]}, MemberID: {r1["MemberId"]}, Year: {r1["Year"]}, Reason: {r1["Reason"]}");
            }
            r1.Close();

            Console.WriteLine("\n--- DuesWaiverPeriods Table ---");
            using var cmd2 = new SqlCommand("SELECT * FROM DuesWaiverPeriods", connection);
            using var r2 = cmd2.ExecuteReader();
            while (r2.Read())
            {
                Console.WriteLine($"ID: {r2["WaiverId"]}, MemberID: {r2["MemberId"]}, Range: {r2["StartYear"]}-{r2["EndYear"]}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
