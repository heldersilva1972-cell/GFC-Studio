using Microsoft.Data.SqlClient;
using System;
using System.Data;

public class DbDump
{
    public static void Main()
    {
        string connString = "Server=.\\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;";
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            string sql = "SELECT MemberID, FirstName, LastName, Status, IsNonPortugueseOrigin, AcceptedDate FROM Members WHERE Status = 'GUEST' AND IsNonPortugueseOrigin = 1 ORDER BY AcceptedDate, LastName, FirstName";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("MemberID | Name | AcceptedDate | IsNull");
                    while (reader.Read())
                    {
                        var id = reader["MemberID"];
                        var first = reader["FirstName"];
                        var last = reader["LastName"];
                        var date = reader["AcceptedDate"];
                        bool isNull = date == DBNull.Value;
                        Console.WriteLine($"{id} | {first} {last} | {date ?? "NULL"} | {isNull}");
                    }
                }
            }
        }
    }
}
