
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace CheckDb {
    class Program {
        static async Task Main(string[] args) {
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=GFC;Trusted_Connection=True;TrustServerCertificate=True;";
            
            try {
                using var conn = new SqlConnection(connectionString);
                await conn.OpenAsync();
                Console.WriteLine("Connected to Database.");

                Console.WriteLine("\nChecking LiquorOrderItems table schema...");
                using var cmd = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'LiquorOrderItems'", conn);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync()) {
                    Console.WriteLine($"- {reader.GetString(0)}");
                }
                await reader.CloseAsync();

            } catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
