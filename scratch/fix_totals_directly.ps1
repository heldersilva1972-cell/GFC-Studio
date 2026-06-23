$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
$connection.Open()

# Fetch received orders with negative total cost
$query = "SELECT Id, ItemsTotal, TaxAmount, AdditionalCosts, TotalCost FROM LiquorOrders WHERE Status = 'Received' AND TotalCost < 0"
$command = New-Object System.Data.SqlClient.SqlCommand($query, $connection)
$reader = $command.ExecuteReader()
$orders = New-Object System.Collections.Generic.List[PSObject]
while ($reader.Read()) {
    $orders.Add([PSCustomObject]@{
        Id = $reader["Id"]
        ItemsTotal = $reader["ItemsTotal"]
        TaxAmount = $reader["TaxAmount"]
        AdditionalCosts = $reader["AdditionalCosts"]
        TotalCost = $reader["TotalCost"]
    })
}
$reader.Close()

Write-Host "Repairing negative orders..."

foreach ($order in $orders) {
    # Fetch items to calculate the incorrect received total that was used during check-in
    $itemQuery = "SELECT Quantity, UnitPriceAtTimeOfOrder FROM LiquorOrderItems WHERE OrderId = $($order.Id) AND IsBackordered = 0"
    $itemCommand = New-Object System.Data.SqlClient.SqlCommand($itemQuery, $connection)
    $itemReader = $itemCommand.ExecuteReader()
    
    $incorrectReceivedTotal = [decimal]0
    while ($itemReader.Read()) {
        $qty = [int]$itemReader["Quantity"]
        $unitPrice = [decimal]$itemReader["UnitPriceAtTimeOfOrder"]
        $incorrectReceivedTotal += ($unitPrice * $qty)
    }
    $itemReader.Close()
    
    # The actual amount the user typed in as Total Due is: AdditionalCosts + incorrectReceivedTotal
    $enteredTotalDue = $order.AdditionalCosts + $incorrectReceivedTotal
    
    # Corrected AdditionalCosts & TotalCost
    $correctedAdditionalCosts = $enteredTotalDue - $order.ItemsTotal - $order.TaxAmount
    $correctedTotalCost = $enteredTotalDue
    
    Write-Host "Order ID: $($order.Id)"
    Write-Host "  Old Total: $($order.TotalCost) | Old Add: $($order.AdditionalCosts)"
    Write-Host "  New Total: $correctedTotalCost | New Add: $correctedAdditionalCosts"
    
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

$connection.Close()
Write-Host "Finished repairing negative orders!"
