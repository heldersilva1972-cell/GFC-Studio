using System;
using Microsoft.Data.SqlClient;

string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;MultipleActiveResultSets=true;";
using var conn = new SqlConnection(connStr);
conn.Open();

Console.WriteLine("--- MIGRATING WAIVERS TO PERIODS ---");

// 1. Get all from Waivers
using var cmdSelect = new SqlCommand("SELECT MemberId, [Year], Reason FROM Waivers", conn);
using var reader = cmdSelect.ExecuteReader();

var insertCmds = new System.Collections.Generic.List<SqlCommand>();
while (reader.Read()) {
    int memberId = (int)reader["MemberId"];
    int year = (int)reader["Year"];
    string reason = reader["Reason"].ToString();
    
    // Check if it already exists in Periods
    string checkSql = "SELECT COUNT(*) FROM DuesWaiverPeriods WHERE MemberId = @m AND @y BETWEEN StartYear AND EndYear";
    using var cmdCheck = new SqlCommand(checkSql, conn);
    cmdCheck.Parameters.AddWithValue("@m", memberId);
    cmdCheck.Parameters.AddWithValue("@y", year);
    
    // We can't use reader and command on same connection unless MARS is on, which it is.
    int count = (int)cmdCheck.ExecuteScalar();
    
    if (count == 0) {
        Console.WriteLine($"Migrating: Member {memberId}, Year {year}, Reason: {reason}");
        string insertSql = "INSERT INTO DuesWaiverPeriods (MemberId, StartYear, EndYear, Reason, CreatedDate, CreatedBy) " +
                          "VALUES (@m, @y, @y, @r, @d, 'Migration')";
        var cmdInsert = new SqlCommand(insertSql, conn);
        cmdInsert.Parameters.AddWithValue("@m", memberId);
        cmdInsert.Parameters.AddWithValue("@y", year);
        cmdInsert.Parameters.AddWithValue("@r", reason);
        cmdInsert.Parameters.AddWithValue("@d", DateTime.Now);
        insertCmds.Add(cmdInsert);
    } else {
        Console.WriteLine($"Skipping: Member {memberId}, Year {year} already covered.");
    }
}
reader.Close();

foreach (var cmd in insertCmds) {
    cmd.Connection = conn;
    cmd.ExecuteNonQuery();
}

Console.WriteLine("Migration complete.");
