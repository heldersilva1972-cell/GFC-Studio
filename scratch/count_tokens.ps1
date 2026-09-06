$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Timestamp, ItemsJson FROM PosSales WHERE Timestamp >= '2026-08-11 19:06:25' AND IsVoided = 0"
$reader = $cmd.ExecuteReader()

$totalSalesLines = @()
$totalRedeemLines = @()

while ($reader.Read()) {
    $ts = $reader['Timestamp']
    $json = $reader['ItemsJson'].ToString()
    
    # Parse JSON
    try {
        $items = $json | ConvertFrom-Json
        foreach ($item in $items) {
            $name = if ($item.Name) { $item.Name } else { "" }
            $cat = if ($item.Category) { $item.Category } else { "" }
            $isApplied = if ($item.PSObject.Properties['IsTokenApplied']) { $item.IsTokenApplied } else { $false }
            $price = if ($item.Price) { [decimal]$item.Price } else { 0 }
            $qty = if ($item.Quantity) { [int]$item.Quantity } else { 1 }
            
            # Check sale
            if (-not $isApplied -and ($cat -eq "TOKENS" -or $name -like "*TOKEN SALE*") -and $name -notlike "*TOKEN REDEEMED*" -and $name -notlike "*(CREDITED*") {
                $totalSalesLines += [PSCustomObject]@{
                    Timestamp = $ts
                    Name = $name
                    Category = $cat
                    Price = $price
                    Qty = $qty
                }
            }
            
            # Check redeem
            if ($isApplied -or $name -like "*TOKEN REDEEMED*" -or $name -like "*(CREDITED*" -or $cat -eq "TOKEN_CREDIT" -or ($price -eq 0 -and $name -like "*TOKEN*" -and $name -notlike "*TOKEN SALE*")) {
                $totalRedeemLines += [PSCustomObject]@{
                    Timestamp = $ts
                    Name = $name
                    Category = $cat
                    Price = $price
                    Qty = $qty
                }
            }
        }
    } catch {}
}
$reader.Close()
$conn.Close()

Write-Host "Total Token Sale Items found since Aug 11 19:06:25: $($totalSalesLines.Count)"
foreach ($s in $totalSalesLines) {
    Write-Host "SALE: $($s.Timestamp) | $($s.Name) | Qty:$($s.Qty) | Price:$($s.Price)"
}

Write-Host "`nTotal Token Redemption Items found since Aug 11 19:06:25: $($totalRedeemLines.Count)"
Write-Host "Sample Redemptions:"
foreach ($r in $totalRedeemLines | Select-Object -First 10) {
    Write-Host "REDEEM: $($r.Timestamp) | $($r.Name) | Qty:$($r.Qty) | Price:$($r.Price)"
}
