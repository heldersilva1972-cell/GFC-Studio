using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connStr = "Server=(localdb)\\mssqllocaldb;Database=GFC_System_DB;Trusted_Connection=True;MultipleActiveResultSets=true";
        using var conn = new SqlConnection(connStr);
        conn.Open();

        using var cmd = new SqlCommand(@"
            SELECT ap.PageId, ap.PageName, ap.PageRoute, ap.Category, upp.CanAccess, u.Username
            FROM AppPages ap
            LEFT JOIN UserPagePermissions upp ON ap.PageId = upp.PageId
            LEFT JOIN AppUsers u ON upp.UserId = u.UserId
            WHERE ap.PageRoute LIKE '%mobile%' OR ap.PageRoute LIKE '%analytics%' OR ap.PageRoute LIKE '%dues%' OR ap.PageRoute LIKE '%users%'
        ", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($"{reader["PageId"]} | {reader["PageName"]} | {reader["PageRoute"]} | {reader["Category"]} | {reader["CanAccess"]} | {reader["Username"]}");
        }
    }
}
