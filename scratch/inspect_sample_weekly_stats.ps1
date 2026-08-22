$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = @"
SELECT TOP 5 
    Id,
    WeekEndingDate,
    OnlineNetSales,
    OnlineCommission,
    OnlineCashes,
    OnlineCashBonus,
    OnlineClaimsBonus,
    OnlineDue,
    InstantGrossSales,
    InstantCommission,
    InstantCashes,
    InstantCashBonus,
    InstantClaimsBonus,
    InstantDue,
    TotalDue,
    CreatedBy,
    CreatedDate
FROM LotteryWeeklyStats 
ORDER BY WeekEndingDate DESC
"@

$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

while ($r.Read()) {
    Write-Host ("ID={0} | Week={1:yyyy-MM-dd} | OL_Sales={2:C2} | OL_Comm={3:C2} | Inst_Sales={4:C2} | Inst_Comm={5:C2} | TotalDue={6:C2} | ImportedBy={7} | ImportDate={8:yyyy-MM-dd}" -f `
        $r["Id"], $r["WeekEndingDate"], $r["OnlineNetSales"], $r["OnlineCommission"], $r["InstantGrossSales"], $r["InstantCommission"], $r["TotalDue"], $r["CreatedBy"], $r["CreatedDate"])
}
$r.Close()
$connection.Close()
