$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Fetch orders 1, 2, 5, 8
$query = "SELECT Id, ItemsTotal, AdditionalCosts, TotalCost FROM LiquorOrders WHERE Id IN (1, 2, 5, 8)"
$command = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $command.ExecuteReader()
$orders = New-Object System.Collections.Generic.List[PSObject]
while ($reader.Read()) {
    $orders.Add([PSCustomObject]@{
        Id = $reader["Id"]
        ItemsTotal = $reader["ItemsTotal"]
        AdditionalCosts = $reader["AdditionalCosts"]
        TotalCost = $reader["TotalCost"]
    })
}
$reader.Close()

foreach ($order in $orders) {
    # Fetch items
    $itemQuery = "SELECT oi.Quantity, oi.UnitPriceAtTimeOfOrder, oi.CasePriceAtTimeOfOrder, oi.BottleFeeAtTimeOfOrder, i.PackSize, i.IsBeer FROM LiquorOrderItems oi JOIN LiquorItems i ON oi.LiquorItemId = i.Id WHERE oi.OrderId = $($order.Id)"
    $itemCommand = New-Object System.Data.SqlClient.SqlCommand($itemQuery, $connection)
    $itemReader = $itemCommand.ExecuteReader()
    
    $incorrectReceivedTotal = [decimal]0
    $trueReceivedTotal = [decimal]0
    
    while ($itemReader.Read()) {
        $qty = [int]$itemReader["Quantity"]
        $unitPrice = [decimal]$itemReader["UnitPriceAtTimeOfOrder"]
        $casePrice = if ($itemReader["CasePriceAtTimeOfOrder"] -eq [DBNull]::Value) { $unitPrice * [int]$itemReader["PackSize"] } else { [decimal]$itemReader["CasePriceAtTimeOfOrder"] }
        $fee = [decimal]$itemReader["BottleFeeAtTimeOfOrder"]
        $packSize = [int]$itemReader["PackSize"]
        
        $incorrectReceivedTotal += ($unitPrice * $qty)
        
        $lineTotal = [decimal]0
        if ($packSize -gt 1) {
            $cases = [int]($qty / $packSize)
            $bottles = $qty % $packSize
            $bottlePrice = if ($itemReader["CasePriceAtTimeOfOrder"] -ne [DBNull]::Value) { $unitPrice } else { $unitPrice / $packSize }
            $lineTotal = ($cases * $casePrice) + ($bottles * $bottlePrice)
        } else {
            $lineTotal = $unitPrice * $qty
        }
        $lineTotal += ($fee * $qty)
        $trueReceivedTotal += $lineTotal
    }
    $itemReader.Close()
    
    Write-Host "Order $($order.Id) | ItemsTotal: $($order.ItemsTotal) | Add: $($order.AdditionalCosts) | Total: $($order.TotalCost)"
    Write-Host "  incorrectReceivedTotal: $incorrectReceivedTotal"
    Write-Host "  trueReceivedTotal: $trueReceivedTotal"
}
$connection.Close()
