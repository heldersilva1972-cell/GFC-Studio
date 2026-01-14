using System;
using System.Data;
using Microsoft.Data.SqlClient;

string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;MultipleActiveResultSets=true;";
using var conn = new SqlConnection(connStr);
conn.Open();

Console.WriteLine("--- ANALYZING DAN FIALHO (ID 3) ---");

// 1. Check Waivers Table
using var cmd1 = new SqlCommand("SELECT * FROM Waivers WHERE MemberId = 3", conn);
using var r1 = cmd1.ExecuteReader();
bool foundInWaivers = false;
while (r1.Read()) {
    foundInWaivers = true;
    Console.WriteLine($"[Waivers] ID: {r1["Id"]}, Year: {r1["Year"]}, Reason: {r1["Reason"]}");
}
r1.Close();
if (!foundInWaivers) Console.WriteLine("[Waivers] No records found for ID 3.");

// 2. Check DuesWaiverPeriods Table
using var cmd2 = new SqlCommand("SELECT * FROM DuesWaiverPeriods WHERE MemberId = 3", conn);
using var r2 = cmd2.ExecuteReader();
bool foundInPeriods = false;
while (r2.Read()) {
    foundInPeriods = true;
    Console.WriteLine($"[Periods] ID: {r2["WaiverId"]}, Range: {r2["StartYear"]}-{r2["EndYear"]}, Reason: {r2["Reason"]}");
}
r2.Close();
if (!foundInPeriods) Console.WriteLine("[Periods] No records found for ID 3.");

// 3. Check DuesPayments Table (Is there a record preventing waiver display?)
using var cmd3 = new SqlCommand("SELECT * FROM DuesPayments WHERE MemberID = 3 AND Year = 2025", conn);
using var r3 = cmd3.ExecuteReader();
while (r3.Read()) {
    Console.WriteLine($"[Payments] 2025 - Amount: {r3["Amount"]}, PaidDate: {r3["PaidDate"]}, Type: {r3["PaymentType"]}");
}
r3.Close();
