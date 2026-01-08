using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DbCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=.\\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand("SELECT KeyCardId, MemberID, CardNumber, IsActive FROM dbo.KeyCards WHERE MemberID = 18", connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader[0]}, Member: {reader[1]}, Number: {reader[2]}, Active: {reader[3]}");
                    }
                }
            }
        }
    }
}
