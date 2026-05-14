
using Microsoft.Data.SqlClient;
using System;
using System.Data;

class Program
{
    static void Main()
    {
        string connectionString = "Server=.\\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("Connection successful.");

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM BingoSessions", conn))
                {
                    int count = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"Total BingoSessions: {count}");
                }

                using (SqlCommand cmd = new SqlCommand("SELECT Id, SessionDate, TotalGrossReceipts, IsDeleted FROM BingoSessions", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Session: Id={reader[0]}, Date={reader[1]}, Gross={reader[2]}, IsDeleted={reader[3]}");
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM BingoLotteryTransactions", conn))
                {
                    int count = (int)cmd.ExecuteScalar();
                    Console.WriteLine($"Total BingoLotteryTransactions: {count}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
