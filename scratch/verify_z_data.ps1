$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== Current Row in PosZReports ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, TotalGrossSales, CashTotal, SalesSummaryJson, ItemTotalsJson, PhysicalTokensJson, ShiftDrinkJson FROM PosZReports WHERE Id = '575c3c9f-c7e6-46ba-8555-a6b29621d361'"
$r = $cmd.ExecuteReader()
if ($r.Read()) {
    Write-Host "Id: $($r['Id'])"
    Write-Host "TotalGrossSales: $($r['TotalGrossSales'])"
    Write-Host "CashTotal: $($r['CashTotal'])"
    Write-Host "Tokens: $($r['PhysicalTokensJson'])"
    Write-Host "ShiftDrink: $($r['ShiftDrinkJson'])"
}
$r.Close()

$conn.Close()
