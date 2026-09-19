$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$conn.Open()

Write-Host "=== 1. Check LiquorTransactions for 2026-09-17 night ==="
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM LiquorTransactions WHERE Timestamp >= '2026-09-17 17:00:00' AND Timestamp <= '2026-09-18 05:00:00'"
$r = $cmd.ExecuteReader()
while ($r.Read()) {
    Write-Host "Id: $($r['Id']) | ItemId: $($r['LiquorItemId']) | Qty: $($r['Quantity']) | Type: $($r['TransactionType']) | Notes: $($r['Notes']) | Time: $($r['Timestamp'])"
}
$r.Close()

Write-Host "`n=== 2. Check TokenAuditLogs or Token logs ==="
$cmd2 = $conn.CreateCommand()
$cmd2.CommandText = "SELECT * FROM sys.tables WHERE name LIKE '%Token%'"
$r2 = $cmd2.ExecuteReader()
$tokenTables = @()
while ($r2.Read()) {
    $tokenTables += $r2['name']
}
$r2.Close()
Write-Host "Token tables: $($tokenTables -join ', ')"

foreach ($t in $tokenTables) {
    Write-Host "--- Table: $t ---"
    $cmdT = $conn.CreateCommand()
    $cmdT.CommandText = "SELECT TOP 5 * FROM $t ORDER BY 1 DESC"
    $rT = $cmdT.ExecuteReader()
    while ($rT.Read()) {
        $row = @()
        for ($i=0; $i -lt $rT.FieldCount; $i++) {
            $row += "$($rT.GetName($i))=$($rT.GetValue($i))"
        }
        Write-Host ($row -join ' | ')
    }
    $rT.Close()
}

Write-Host "`n=== 3. Check Shift Drink logs / tables ==="
$cmd3 = $conn.CreateCommand()
$cmd3.CommandText = "SELECT * FROM sys.tables WHERE name LIKE '%Shift%' OR name LIKE '%Drink%'"
$r3 = $cmd3.ExecuteReader()
while ($r3.Read()) {
    Write-Host "Found table: $($r3['name'])"
}
$r3.Close()

Write-Host "`n=== 4. Check All tables related to Payroll / WorkHours ==="
$cmd4 = $conn.CreateCommand()
$cmd4.CommandText = "SELECT * FROM sys.tables WHERE name LIKE '%Hour%' OR name LIKE '%Work%' OR name LIKE '%Time%'"
$r4 = $cmd4.ExecuteReader()
while ($r4.Read()) {
    Write-Host "Found table: $($r4['name'])"
}
$r4.Close()

$conn.Close()
