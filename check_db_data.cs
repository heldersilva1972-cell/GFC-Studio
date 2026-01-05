using System;
using Microsoft.Data.SqlClient;

class Program {
    static void Main() {
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";
        try {
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                Console.WriteLine("Connection successful.");
                
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Members", connection)) {
                    int count = (int)command.ExecuteScalar();
                    Console.WriteLine($"Members count: {count}");
                }

                using (SqlCommand command = new SqlCommand("SELECT TOP 5 Name FROM Members", connection)) {
                    using (SqlDataReader reader = command.ExecuteReader()) {
                        Console.WriteLine("Top 5 Members:");
                        while (reader.Read()) {
                            Console.WriteLine($"- {reader[0]}");
                        }
                    }
                }
            }
        } catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
