using System;
using Microsoft.Data.SqlClient;

namespace CheckDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = @"Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;";
            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT MemberID, FirstName, LastName, AcceptedDate, Status FROM Members WHERE MemberID IN (69, 114)", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader[0]}, Name: {reader[1]} {reader[2]}, Accepted: {(reader[3] == DBNull.Value ? "NULL" : reader[3])}, Status: {reader[4]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
