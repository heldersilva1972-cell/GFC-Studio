$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

# 1. Tokens
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, StartingLiabilityBalance FROM PosTokens"
$r = $cmd.ExecuteReader()
$tokens = @()
while ($r.Read()) { $tokens += [PSCustomObject]@{ Id = [int]$r['Id']; Name = $r['Name'].ToString(); StartingBalance = [int]$r['StartingLiabilityBalance'] } }
$r.Close()

# 2. Legacy Reset logs
$cmd.CommandText = "SELECT Id, Timestamp, TokenName, ExpectedStock, ActualCount, Variance, Status FROM TokenAuditLogEntries WHERE Status = 'LEGACY_RESET' ORDER BY Timestamp ASC"
$r = $cmd.ExecuteReader()
$legacyLogs = @()
while ($r.Read()) {
    $legacyLogs += [PSCustomObject]@{
        Timestamp = [datetime]$r['Timestamp']
        TokenName = $r['TokenName'].ToString()
        ActualCount = [int]$r['ActualCount']
    }
}
$r.Close()

# 3. All sales since 2026-08-01
$cmd.CommandText = "SELECT Id, Timestamp, ItemsJson FROM PosSales WHERE Timestamp >= '2026-08-01' AND IsVoided = 0 ORDER BY Timestamp ASC"
$r = $cmd.ExecuteReader()
$sales = @()
while ($r.Read()) { $sales += [PSCustomObject]@{ Timestamp = [datetime]$r['Timestamp']; ItemsJson = $r['ItemsJson'].ToString() } }
$r.Close()
$conn.Close()

Write-Host "=== LEGACY COUNT AUDIT CHECK ==="
foreach ($tok in $tokens | Where-Object { $_.Name -in @("Beer", "Mixed Drink") }) {
    $tokName = $tok.Name
    $lastReset = $legacyLogs | Where-Object { $_.TokenName -eq $tokName } | Select-Object -Last 1
    
    Write-Host "`nToken: $tokName (Starting Liability Balance in PosTokens table: $($tok.StartingBalance))"
    if ($lastReset) {
        $resetTime = $lastReset.Timestamp
        $baseline = $lastReset.ActualCount
        Write-Host "Last LEGACY_RESET on: $resetTime -> Reset to $baseline"
        
        $soldSinceReset = 0
        $redeemedSinceReset = 0
        
        foreach ($s in $sales | Where-Object { $_.Timestamp -ge $resetTime }) {
            try {
                $items = $s.ItemsJson | ConvertFrom-Json
                foreach ($item in $items) {
                    $name = if ($item.Name) { $item.Name.ToUpper() } else { "" }
                    $cat = if ($item.Category) { $item.Category.ToUpper() } else { "" }
                    $isApplied = if ($item.PSObject.Properties['IsTokenApplied']) { $item.IsTokenApplied } else { $false }
                    $price = if ($item.Price) { [decimal]$item.Price } else { 0 }
                    $qty = if ($item.Quantity) { [int]$item.Quantity } else { 1 }
                    $appliedId = if ($item.PSObject.Properties['AppliedTokenId'] -and $item.AppliedTokenId) { [int]$item.AppliedTokenId } else { $null }
                    
                    # Sale
                    if (-not $isApplied -and ($cat -eq "TOKENS" -or $name -like "*TOKEN SALE*") -and $name -notlike "*TOKEN REDEEMED*") {
                        if ($name -like "*$($tokName.ToUpper())*") { $soldSinceReset += $qty }
                    }
                    
                    # Redeem
                    if ($isApplied -or $name -like "*TOKEN REDEEMED*" -or $name -like "*(CREDITED*" -or $cat -eq "TOKEN_CREDIT" -or ($price -eq 0 -and $name -like "*TOKEN*" -and $name -notlike "*TOKEN SALE*")) {
                        $match = $false
                        if ($appliedId -and $appliedId -eq $tok.Id) { $match = $true }
                        if (-not $match) {
                            if ($tokName -eq "Beer" -and ($name -like "*BEER*" -or $name -like "*SELTZER*" -or $name -like "*CIDER*")) { $match = $true }
                            if ($tokName -eq "Mixed Drink" -and $name -notlike "*BEER*" -and $name -notlike "*SELTZER*" -and $name -notlike "*CIDER*") { $match = $true }
                        }
                        if ($match) { $redeemedSinceReset += $qty }
                    }
                }
            } catch {}
        }
        
        $legacyCount = $baseline + [Math]::Max(0, $redeemedSinceReset - $soldSinceReset)
        Write-Host "Sold Since Reset: $soldSinceReset | Redeemed Since Reset: $redeemedSinceReset"
        Write-Host "Excess Redemptions: $([Math]::Max(0, $redeemedSinceReset - $soldSinceReset))"
        Write-Host "Computed Legacy Count: $legacyCount"
    } else {
        Write-Host "No LEGACY_RESET log found for $tokName."
    }
}
