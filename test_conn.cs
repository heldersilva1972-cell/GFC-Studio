using System;
using Microsoft.Data.SqlClient;

class Program {
    static void Main() {
        string connString = "Server=localhost;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";
        try {
            using var conn = new SqlConnection(connString);
            Console.WriteLine("Connecting to: " + connString);
            conn.Open();
            Console.WriteLine("Success!");
            using var cmd = new SqlCommand("SELECT TOP 1 name FROM sys.databases", conn);
            var result = cmd.ExecuteScalar();
            Console.WriteLine("Database check: " + result);
        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
            if (ex.InnerException != null) {
                Console.WriteLine("Inner Error: " + ex.InnerException.Message);
            }
        }
    }
}
