$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, ShiftType, RecordSalesToBar, HoursWorked FROM PosZReports WHERE Id = '48268130-9915-432c-97f0-ad23ed77611b'"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$r = $cmd.ExecuteReader()

if ($r.Read()) {
    Write-Host "PosZReport ID: $($r['Id'])"
    Write-Host "TerminalName: $($r['TerminalName'])"
    Write-Host "BartenderName: $($r['BartenderName'])"
    Write-Host "Timestamp: $($r['Timestamp'])"
    Write-Host "TotalGrossSales: $($r['TotalGrossSales'])"
    Write-Host "CashTotal: $($r['CashTotal'])"
    Write-Host "ShiftType: $($r['ShiftType'])"
    Write-Host "RecordSalesToBar: $($r['RecordSalesToBar'])"
    Write-Host "HoursWorked: $($r['HoursWorked'])"
}

$r.Close()
$connection.Close()
