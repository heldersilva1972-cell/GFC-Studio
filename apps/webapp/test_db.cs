using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TestUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = @"Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;";
            int memberId = 69;
            DateTime acceptedDate = new DateTime(2024, 3, 5);

            try
            {
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();
                    Console.WriteLine("Connected to DB.");

                    using (var cmd = new SqlCommand("UPDATE Members SET AcceptedDate = @AccDate WHERE MemberID = @ID", conn))
                    {
                        cmd.Parameters.Add("@AccDate", SqlDbType.DateTime2).Value = acceptedDate;
                        cmd.Parameters.AddWithValue("@ID", memberId);

                        int rows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Updated {rows} row(s).");
                    }

                    using (var cmd = new SqlCommand("SELECT AcceptedDate FROM Members WHERE MemberID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", memberId);
                        var result = cmd.ExecuteScalar();
                        Console.WriteLine($"Final Value in DB: {result}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
