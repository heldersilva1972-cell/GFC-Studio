$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

Write-Host "--- POS TOKENS ---"
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, SalePrice FROM PosTokens"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "$($reader['Id']) | $($reader['Name']) | $($reader['SalePrice'])"
}
$reader.Close()

Write-Host "`n--- TOKEN AUDIT LOGS ---"
$cmd.CommandText = "SELECT TOP 10 Id, Timestamp, TokenName, ExpectedStock, ActualCount, Variance, Status, Notes FROM TokenAuditLogEntries ORDER BY Timestamp DESC"
$reader = $cmd.ExecuteReader()
while ($reader.Read()) {
    Write-Host "$($reader['Timestamp']) | $($reader['TokenName']) | Exp:$($reader['ExpectedStock']) | Act:$($reader['ActualCount']) | Var:$($reader['Variance']) | $($reader['Status']) | $($reader['Notes'])"
}
$reader.Close()

Write-Host "`n--- RECENT TOKEN SALES / REDEMPTIONS SINCE 2026-08-11 ---"
$cmd.CommandText = "SELECT Id, Timestamp, TerminalName, BartenderName, ItemsJson FROM PosSales WHERE Timestamp >= '2026-08-11 19:00:00' AND IsVoided = 0 ORDER BY Timestamp ASC"
$reader = $cmd.ExecuteReader()
$salesCount = 0
$redemptionsCount = 0
$tokenItems = @()
while ($reader.Read()) {
    $itemsJson = $reader['ItemsJson'].ToString()
    if ($itemsJson -match "TOKEN|Token|token") {
        $tokenItems += [PSCustomObject]@{
            Id = $reader['Id']
            Timestamp = $reader['Timestamp']
            ItemsJson = $itemsJson
        }
    }
}
$reader.Close()

Write-Host "Total sales with tokens in JSON since Aug 11: $($tokenItems.Count)"
foreach ($t in $tokenItems | Select-Object -First 15) {
    Write-Host "$($t.Timestamp) [Sale $($t.Id)]: $($t.ItemsJson)"
}

$conn.Close()
