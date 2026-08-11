$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Z-Report for Aug 7 night shift
$zId = "7601b530-e787-4e58-9369-d567925f4a7d"
$zQuery = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports WHERE Id = '$zId'"
$cmd = New-Object System.Data.SqlClient.SqlCommand($zQuery, $connection)
$r = $cmd.ExecuteReader()
$zTime = $null
$term = ""
if ($r.Read()) {
    $zTime = [DateTime]$r["Timestamp"]
    $term = $r["TerminalName"]
}
$r.Close()

# Previous Z-Report time
$prevQuery = "SELECT TOP 1 Timestamp FROM PosZReports WHERE TerminalName = '$term' AND Timestamp < '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' ORDER BY Timestamp DESC"
$pcmd = New-Object System.Data.SqlClient.SqlCommand($prevQuery, $connection)
$prevTime = [DateTime]$pcmd.ExecuteScalar()

Write-Host "Auditing shift from $prevTime to $zTime"

# Query PosSales in this window
$sQuery = "SELECT Id, Timestamp, BartenderName, PaymentType, TotalAmount, ItemsJson FROM PosSales WHERE TerminalName = '$term' AND Timestamp <= '$($zTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' AND Timestamp > '$($prevTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))' ORDER BY Timestamp"
$scmd = New-Object System.Data.SqlClient.SqlCommand($sQuery, $connection)
$sr = $scmd.ExecuteReader()

$sales = @()
while ($sr.Read()) {
    $sales += [PSCustomObject]@{
        Id = $sr["Id"]
        Timestamp = $sr["Timestamp"]
        TotalAmount = $sr["TotalAmount"]
        PaymentType = $sr["PaymentType"]
        ItemsJson = $sr["ItemsJson"]
    }
}
$sr.Close()

Write-Host "Found $($sales.Count) sales transactions."

foreach ($s in $sales) {
    Write-Host "--- Sale $($s.Id) at $($s.Timestamp) Total: $($s.TotalAmount) Pay: $($s.PaymentType) ---"
    Write-Host "Items: $($s.ItemsJson)"
}

$connection.Close()
