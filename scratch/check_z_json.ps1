$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT SalesSummaryJson, ItemTotalsJson, CashTotal, TotalGrossSales FROM PosZReports WHERE Id = '575c3c9f-c7e6-46ba-8555-a6b29621d361'"
$r = $cmd.ExecuteReader()
if ($r.Read()) {
    Write-Host "SalesSummaryJson:" $r['SalesSummaryJson']
    Write-Host "`nItemTotalsJson:" $r['ItemTotalsJson']
    Write-Host "`nCashTotal:" $r['CashTotal']
    Write-Host "TotalGrossSales:" $r['TotalGrossSales']
}
$r.Close()
$conn.Close()
