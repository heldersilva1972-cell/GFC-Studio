$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

# 1. Master Tokens
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, SalePrice FROM PosTokens"
$r = $cmd.ExecuteReader()
$tokens = @()
while ($r.Read()) {
    $tokens += [PSCustomObject]@{ Id = [int]$r['Id']; Name = $r['Name'].ToString(); SalePrice = [decimal]$r['SalePrice'] }
}
$r.Close()

# 2. Token Audit Logs
$cmd.CommandText = "SELECT Id, Timestamp, TokenName, ExpectedStock, ActualCount, Variance, Status FROM TokenAuditLogEntries ORDER BY Timestamp ASC"
$r = $cmd.ExecuteReader()
$logs = @()
while ($r.Read()) {
    $logs += [PSCustomObject]@{
        Id = [int]$r['Id']
        Timestamp = [datetime]$r['Timestamp']
        TokenName = $r['TokenName'].ToString()
        ExpectedStock = [int]$r['ExpectedStock']
        ActualCount = [int]$r['ActualCount']
        Variance = [int]$r['Variance']
        Status = $r['Status'].ToString()
    }
}
$r.Close()

# 3. All Sales since 2026-08-11
$cmd.CommandText = "SELECT Id, Timestamp, ItemsJson FROM PosSales WHERE Timestamp >= '2026-08-11 19:00:00' AND IsVoided = 0"
$r = $cmd.ExecuteReader()
$sales = @()
while ($r.Read()) {
    $sales += [PSCustomObject]@{
        Id = $r['Id']
        Timestamp = [datetime]$r['Timestamp']
        ItemsJson = $r['ItemsJson'].ToString()
    }
}
$r.Close()
$conn.Close()

Write-Host "Simulating C# calculation for each Token..."
foreach ($tok in $tokens) {
    $tokName = $tok.Name
    $tokenLogs = $logs | Where-Object { $_.TokenName.Trim() -eq $tokName.Trim() -or $_.TokenName -like "*$tokName*" }
    $lastAudit = $tokenLogs | Where-Object { $_.Status -in @("AUDIT_BASELINE_SET", "BALANCED", "LEGACY_INFLOW", "SHRINKAGE") } | Select-Object -Last 1
    
    if (-not $lastAudit) {
        Write-Host "Token: $tokName -> No Audit Baseline Found."
        continue
    }
    
    $baseline = $lastAudit.ActualCount
    $cutoff = $lastAudit.Timestamp
    
    $soldSince = 0
    $redeemedSince = 0
    
    foreach ($s in $sales | Where-Object { $_.Timestamp -ge $cutoff }) {
        try {
            $items = $s.ItemsJson | ConvertFrom-Json
            foreach ($item in $items) {
                $name = if ($item.Name) { $item.Name.ToUpper() } else { "" }
                $cat = if ($item.Category) { $item.Category.ToUpper() } else { "" }
                $isApplied = if ($item.PSObject.Properties['IsTokenApplied']) { $item.IsTokenApplied } else { $false }
                $price = if ($item.Price) { [decimal]$item.Price } else { 0 }
                $qty = if ($item.Quantity) { [int]$item.Quantity } else { 1 }
                $appliedId = if ($item.PSObject.Properties['AppliedTokenId'] -and $item.AppliedTokenId) { [int]$item.AppliedTokenId } else { $null }
                
                # Check Sale
                $isSale = (-not $isApplied) -and ($cat -eq "TOKENS" -or $cat -eq "TOKEN" -or $name -like "*TOKEN SALE*") -and ($name -notlike "*TOKEN REDEEMED*") -and ($name -notlike "*(CREDITED*")
                if ($isSale) {
                    $match = $tokens | Sort-Object { $_.Name.Length } -Descending | Where-Object { $name -like "*$($_.Name.ToUpper())*" } | Select-Object -First 1
                    if ($match -and $match.Name -eq $tokName) {
                        $soldSince += $qty
                    }
                }
                
                # Check Redeem
                $isRedeem = $isApplied -or ($name -like "*TOKEN REDEEMED*") -or ($name -like "*(CREDITED*") -or ($cat -eq "TOKEN_CREDIT") -or ($price -eq 0 -and $name -like "*TOKEN*" -and $name -notlike "*TOKEN SALE*")
                if ($isRedeem) {
                    $match = $null
                    if ($appliedId -and $appliedId -gt 0) {
                        $match = $tokens | Where-Object { $_.Id -eq $appliedId } | Select-Object -First 1
                    }
                    if (-not $match) {
                        if ($name -like "*BEER*" -or $name -like "*SELTZER*" -or $name -like "*CIDER*") {
                            $match = $tokens | Sort-Object { $_.Name.Length } -Descending | Where-Object { $_.Name.ToUpper() -like "*BEER*" -or $_.Name.ToUpper() -like "*SELTZER*" } | Select-Object -First 1
                        } else {
                            $match = $tokens | Sort-Object { $_.Name.Length } -Descending | Where-Object { $_.Name.ToUpper() -notlike "*BEER*" -and $_.Name.ToUpper() -notlike "*SELTZER*" } | Select-Object -First 1
                        }
                    }
                    if ($match -and $match.Name -eq $tokName) {
                        $redeemedSince += $qty
                    }
                }
            }
        } catch {}
    }
    
    $expected = [Math]::Max(0, $baseline - $soldSince + $redeemedSince)
    Write-Host "Token: $tokName | Baseline (at $cutoff): $baseline | SoldSince: $soldSince | RedeemedSince: $redeemedSince | ExpectedStock: $expected"
}
