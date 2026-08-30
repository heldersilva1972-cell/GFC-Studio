$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$tables = @("ActiveEvents", "EventTemplates", "PosSales", "PosZReports", "ProductItems", "LiquorTransactions")
foreach ($t in $tables) {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM $t WHERE 1=1"
    try {
        $cnt = $cmd.ExecuteScalar()
        Write-Output "Table $t exists. Total rows: $cnt"
    } catch {
        Write-Output "Table $t error: $_"
    }
}

# Search for Horseshoes in ActiveEvents
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT * FROM ActiveEvents WHERE Name LIKE '%Horseshoe%'"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
Write-Output "ActiveEvents matching 'Horseshoe': $($dt.Rows.Count)"
$dt | Format-Table -AutoSize | Out-String | Write-Output

# Search for Horseshoes in EventTemplates
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, IsActive, IsDeleted, RecipientEventName FROM EventTemplates WHERE Name LIKE '%Horseshoe%'"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
Write-Output "EventTemplates matching 'Horseshoe': $($dt.Rows.Count)"
$dt | Format-Table -AutoSize | Out-String | Write-Output

# Search for Horseshoes in PosSales
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Timestamp, ActiveEventId, PaymentType, TotalAmount, ItemsJson FROM PosSales WHERE ItemsJson LIKE '%Horseshoe%' OR PaymentType LIKE '%Horseshoe%'"
$dt = New-Object System.Data.DataTable
(New-Object System.Data.SqlClient.SqlDataAdapter($cmd)).Fill($dt) | Out-Null
Write-Output "PosSales matching 'Horseshoe': $($dt.Rows.Count)"
$dt | Format-Table -AutoSize | Out-String | Write-Output

$conn.Close()
