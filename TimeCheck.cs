using System;
using System.IO;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace TimeFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Console.WriteLine("Connected to Database.");
                    
                    using (var cmd = new SqlCommand("SELECT SystemTimeZoneId FROM SystemSettings WHERE Id = 1", conn))
                    {
                        var result = cmd.ExecuteScalar();
                        Console.WriteLine($"Current SystemTimeZoneId in DB: {result ?? "NULL"}");
                    }
                    
                    Console.WriteLine($"Machine Local Time: {DateTime.Now}");
                    Console.WriteLine($"Machine UTC Time: {DateTime.UtcNow}");
                    Console.WriteLine($"Machine TimeZone Id: {TimeZoneInfo.Local.Id}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
