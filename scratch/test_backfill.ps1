$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# 1. Fetch all Z-Reports ordered by Terminal and Timestamp
$zCmd = New-Object System.Data.SqlClient.SqlCommand("SELECT Id, TerminalName, Timestamp, TotalGrossSales, SalesSummaryJson, ItemTotalsJson FROM PosZReports ORDER BY TerminalName, Timestamp", $connection)
$zReader = $zCmd.ExecuteReader()

$zList = @()
while ($zReader.Read()) {
    $zList += [PSCustomObject]@{
        Id = $zReader["Id"]
        TerminalName = $zReader["TerminalName"]
        Timestamp = [DateTime]$zReader["Timestamp"]
        Gross = [decimal]$zReader["TotalGrossSales"]
        SalesJson = $zReader["SalesSummaryJson"]
        ItemTotalsJson = if ($zReader["ItemTotalsJson"] -ne [DBNull]::Value) { $zReader["ItemTotalsJson"] } else { "{}" }
    }
}
$zReader.Close()

Write-Host "Found $($zList.Count) total Z-reports."

# Group Z-reports by TerminalName
$termGroups = $zList | Group-Object TerminalName

$updatedCount = 0

foreach ($group in $termGroups) {
    $reports = $group.Group | Sort-Object Timestamp
    for ($i = 0; $i -lt $reports.Count; $i++) {
        $r = $reports[$i]
        
        # Calculate time window for this shift
        $endTime = $r.Timestamp
        $startTime = if ($i -gt 0) { $reports[$i-1].Timestamp } else { $endTime.AddDays(-1) }
        
        # Query sales between startTime and endTime for this terminal
        $tCmd = New-Object System.Data.SqlClient.SqlCommand("SELECT TotalAmount, ItemsJson, IsVoided, PaymentType FROM PosSales WHERE Timestamp > @start AND Timestamp <= @end AND (IsVoided = 0 OR IsVoided IS NULL)", $connection)
        $tCmd.Parameters.AddWithValue("@start", $startTime) | Out-Null
        $tCmd.Parameters.AddWithValue("@end", $endTime) | Out-Null
        
        $tReader = $tCmd.ExecuteReader()
        
        $totals = @{}
        while ($tReader.Read()) {
            $paymentType = if ($tReader["PaymentType"] -ne [DBNull]::Value) { [string]$tReader["PaymentType"] } else { "" }
            if ($paymentType -eq "PAYOUT") { continue }
            
            $itemsJson = if ($tReader["ItemsJson"] -ne [DBNull]::Value) { [string]$tReader["ItemsJson"] } else { "[]" }
            if ([string]::IsNullOrWhiteSpace($itemsJson)) { continue }
            
            try {
                $items = ConvertFrom-Json $itemsJson
                foreach ($item in $items) {
                    $name = $item.Name
                    $qty = if ($item.Quantity) { [int]$item.Quantity } else { 1 }
                    $price = if ($item.Price) { [decimal]$item.Price } else { 0 }
                    
                    if ($name -and -not $name.StartsWith("TAB DEPOSIT:") -and -not $name.StartsWith("INITIAL DEPOSIT:") -and -not $name.StartsWith("DEPOSIT CORRECTION:") -and -not $name.StartsWith("RETURNED FUNDS:")) {
                        if (-not $totals.ContainsKey($name)) { $totals[$name] = 0 }
                        $totals[$name] += ($price * $qty)
                    }
                }
            } catch {}
        }
        $tReader.Close()
        
        if ($totals.Count -gt 0) {
            $json = ConvertTo-Json $totals -Compress
            $uCmd = New-Object System.Data.SqlClient.SqlCommand("UPDATE PosZReports SET ItemTotalsJson = @json WHERE Id = @id", $connection)
            $uCmd.Parameters.AddWithValue("@json", $json) | Out-Null
            $uCmd.Parameters.AddWithValue("@id", $r.Id) | Out-Null
            $uCmd.ExecuteNonQuery() | Out-Null
            $updatedCount++
        }
    }
}

Write-Host "Successfully backfilled ItemTotalsJson for $updatedCount historical Z-reports."

$connection.Close()
