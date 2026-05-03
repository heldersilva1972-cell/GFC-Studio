using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string connectionString = "Server=localhost;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("SELECT DISTINCT Category FROM AppPages", connection))
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("Categories in AppPages:");
                while (reader.Read())
                {
                    Console.WriteLine("- " + reader["Category"]);
                }
            }

            using (var command = new SqlCommand("SELECT PageName, PageRoute, Category FROM AppPages WHERE PageRoute LIKE '%mobile%'", connection))
            using (var reader = command.ExecuteReader())
            {
                Console.WriteLine("\nMobile Pages in AppPages:");
                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["PageName"]} ({reader["PageRoute"]}) in {reader["Category"]}");
                }
            }
        }
    }
}
