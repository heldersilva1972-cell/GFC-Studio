$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

$query = "SELECT Id, TerminalName, BartenderName, Timestamp, TotalGrossSales, CashTotal, SalesSummaryJson FROM PosZReports ORDER BY Timestamp DESC"
$cmd = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $cmd.ExecuteReader()

while ($reader.Read()) {
    $id = $reader["Id"]
    $ts = $reader["Timestamp"]
    $bartender = $reader["BartenderName"]
    $gross = [decimal]$reader["TotalGrossSales"]
    $cash = [decimal]$reader["CashTotal"]
    $json = $reader["SalesSummaryJson"].ToString()
    
    # Calculate sum from items json
    $items = ConvertFrom-Json $json
    $itemSum = 0.0
    foreach ($item in $items) {
        $p = if ($item.Price) { [double]$item.Price } else { 0.0 }
        $q = if ($item.Quantity) { [double]$item.Quantity } else { 1.0 }
        $itemSum += ($p * $q)
    }
    
    if ([Math]::Abs($gross - $itemSum) -gt 1.0) {
        Write-Host "ZReport [$id] ($ts - $bartender): Stored Gross=$gross, ItemSum=$itemSum (DIFF=$($gross - $itemSum))"
    }
}

$reader.Close()
$connection.Close()
