$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# 1. Fetch all received orders
$query = @"
SELECT o.Id, o.ItemsTotal, o.TaxAmount, o.AdditionalCosts, o.TotalCost
FROM LiquorOrders o
WHERE o.Status = 'Received'
"@

$command = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $command.ExecuteReader()

$ordersToFix = New-Object System.Collections.Generic.List[PSObject]

while ($reader.Read()) {
    $ordersToFix.Add([PSCustomObject]@{
        Id = $reader["Id"]
        ItemsTotal = $reader["ItemsTotal"]
        TaxAmount = $reader["TaxAmount"]
        AdditionalCosts = $reader["AdditionalCosts"]
        TotalCost = $reader["TotalCost"]
    })
}
$reader.Close()

Write-Host "Analyzing received orders..."

foreach ($order in $ordersToFix) {
    # Fetch items for this order
    $itemQuery = @"
SELECT oi.Quantity, oi.UnitPriceAtTimeOfOrder, oi.CasePriceAtTimeOfOrder, oi.BottleFeeAtTimeOfOrder, i.PackSize, i.IsBeer
FROM LiquorOrderItems oi
JOIN LiquorItems i ON oi.LiquorItemId = i.Id
WHERE oi.OrderId = $($order.Id) AND oi.IsBackordered = 0
"@
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
        
        # Incorrect calculation: simple multiplication
        $incorrectReceivedTotal += ($unitPrice * $qty)
        
        # True calculation: case / bottle logic
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
    
    # Calculate what the user entered as TotalDue
    $enteredTotalDue = $order.AdditionalCosts + $incorrectReceivedTotal
    
    # Corrected AdditionalCosts & TotalCost
    $correctedAdditionalCosts = $enteredTotalDue - $trueReceivedTotal
    $correctedTotalCost = $order.ItemsTotal + $order.TaxAmount + $correctedAdditionalCosts
    
    # If the corrected values are different from current ones, write them
    if ([Math]::Abs($correctedTotalCost - $order.TotalCost) -gt 0.01) {
        Write-Host "Order ID: $($order.Id) (FIXED)"
        Write-Host "  Current -> Total: $($order.TotalCost) | Add: $($order.AdditionalCosts)"
        Write-Host "  Corrected -> Total: $correctedTotalCost | Add: $correctedAdditionalCosts"
        
        # Update order in DB
        $updateQuery = @"
UPDATE LiquorOrders
SET AdditionalCosts = @add, TotalCost = @total
WHERE Id = @id
"@
        $updateCommand = New-Object System.Data.SqlClient.SqlCommand($updateQuery, $connection)
        $updateCommand.Parameters.AddWithValue("@add", $correctedAdditionalCosts) | Out-Null
        $updateCommand.Parameters.AddWithValue("@total", $correctedTotalCost) | Out-Null
        $updateCommand.Parameters.AddWithValue("@id", $order.Id) | Out-Null
        $updateCommand.ExecuteNonQuery() | Out-Null
    }
}

$connection.Close()
Write-Host "Finished database correction!"
