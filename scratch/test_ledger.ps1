$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connString)
$conn.Open()

# PosTokens
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Id, Name, SalePrice FROM PosTokens"
$r = $cmd.ExecuteReader()
$tokens = @()
while ($r.Read()) { $tokens += [PSCustomObject]@{ Id = [int]$r['Id']; Name = $r['Name'].ToString(); SalePrice = [decimal]$r['SalePrice'] } }
$r.Close()

# Logs
$cmd.CommandText = "SELECT Id, Timestamp, TokenName, ExpectedStock, ActualCount, Variance, Status, Notes, PerformedBy FROM TokenAuditLogEntries ORDER BY Timestamp ASC"
$r = $cmd.ExecuteReader()
$logs = @()
while ($r.Read()) {
    $logs += [PSCustomObject]@{
        Timestamp = [datetime]$r['Timestamp']
        TokenName = $r['TokenName'].ToString()
        ExpectedStock = [int]$r['ExpectedStock']
        ActualCount = [int]$r['ActualCount']
        Variance = [int]$r['Variance']
        Status = $r['Status'].ToString()
        Notes = $r['Notes'].ToString()
        PerformedBy = $r['PerformedBy'].ToString()
    }
}
$r.Close()

# Sales
$cmd.CommandText = "SELECT Id, Timestamp, TerminalName, BartenderName, ItemsJson FROM PosSales WHERE Timestamp >= '2026-08-11 19:00:00' AND IsVoided = 0 ORDER BY Timestamp ASC"
$r = $cmd.ExecuteReader()
$sales = @()
while ($r.Read()) {
    $sales += [PSCustomObject]@{
        Id = $r['Id']
        Timestamp = [datetime]$r['Timestamp']
        TerminalName = $r['TerminalName'].ToString()
        BartenderName = $r['BartenderName'].ToString()
        ItemsJson = $r['ItemsJson'].ToString()
    }
}
$r.Close()
$conn.Close()

foreach ($tok in $tokens | Where-Object { $_.Name -in @("Beer", "Mixed Drink") }) {
    $tokName = $tok.Name
    Write-Host "`n========================================================"
    Write-Host "LEDGER FOR $tokName"
    Write-Host "========================================================"
    
    $tokenLogs = $logs | Where-Object { $_.TokenName.Trim() -eq $tokName.Trim() -or $_.TokenName -like "*$tokName*" }
    $lastAudit = $tokenLogs | Where-Object { $_.Status -in @("AUDIT_BASELINE_SET", "BALANCED", "LEGACY_INFLOW", "SHRINKAGE") } | Select-Object -Last 1
    if (-not $lastAudit) { continue }
    
    $cutoff = $lastAudit.Timestamp
    
    $events = @()
    # 1. Baseline audit event
    $events += [PSCustomObject]@{
        Timestamp = $lastAudit.Timestamp
        EventType = $lastAudit.Status
        ShiftLabel = "Physical Audit Baseline"
        Bartender = $lastAudit.PerformedBy
        Sold = 0
        Redeemed = 0
        Replenished = 0
        AuditCount = $lastAudit.ActualCount
        Notes = $lastAudit.Notes
    }
    
    # 2. Subsequent logs
    foreach ($l in $tokenLogs | Where-Object { $_.Timestamp -gt $cutoff }) {
        $events += [PSCustomObject]@{
            Timestamp = $l.Timestamp
            EventType = $l.Status
            ShiftLabel = if ($l.Status -eq "STOCK_REPLENISHED") { "Stock Replenishment" } else { "Audit Adjustment" }
            Bartender = $l.PerformedBy
            Sold = 0
            Redeemed = 0
            Replenished = if ($l.Status -eq "STOCK_REPLENISHED") { $l.Variance } else { 0 }
            AuditCount = $l.ActualCount
            Notes = $l.Notes
        }
    }
    
    # 3. Shift activities
    $salesSince = $sales | Where-Object { $_.Timestamp -ge $cutoff }
    
    # Parse sales into line items
    $lineItems = @()
    foreach ($s in $salesSince) {
        $bName = if ($s.BartenderName) { $s.BartenderName.Trim() } else { "Unassigned" }
        $termName = if ($s.TerminalName) { $s.TerminalName.Trim() } else { "Station" }
        try {
            $items = $s.ItemsJson | ConvertFrom-Json
            foreach ($item in $items) {
                $name = if ($item.Name) { $item.Name.ToUpper() } else { "" }
                $cat = if ($item.Category) { $item.Category.ToUpper() } else { "" }
                $isApplied = if ($item.PSObject.Properties['IsTokenApplied']) { $item.IsTokenApplied } else { $false }
                $price = if ($item.Price) { [decimal]$item.Price } else { 0 }
                $qty = if ($item.Quantity) { [int]$item.Quantity } else { 1 }
                $appliedId = if ($item.PSObject.Properties['AppliedTokenId'] -and $item.AppliedTokenId) { [int]$item.AppliedTokenId } else { $null }
                
                # Check sale
                $isSale = (-not $isApplied) -and ($cat -eq "TOKENS" -or $cat -eq "TOKEN" -or $name -like "*TOKEN SALE*") -and ($name -notlike "*TOKEN REDEEMED*") -and ($name -notlike "*(CREDITED*")
                if ($isSale) {
                    $match = $tokens | Sort-Object { $_.Name.Length } -Descending | Where-Object { $name -like "*$($_.Name.ToUpper())*" } | Select-Object -First 1
                    if ($match -and $match.Name -eq $tokName) {
                        $lineItems += [PSCustomObject]@{
                            Timestamp = $s.Timestamp
                            Date = $s.Timestamp.Date
                            Bartender = $bName
                            Terminal = $termName
                            Type = "SALE"
                            Qty = $qty
                        }
                    }
                }
                
                # Check redeem
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
                        $lineItems += [PSCustomObject]@{
                            Timestamp = $s.Timestamp
                            Date = $s.Timestamp.Date
                            Bartender = $bName
                            Terminal = $termName
                            Type = "REDEEM"
                            Qty = $qty
                        }
                    }
                }
            }
        } catch {}
    }
    
    # Group line items by Date + Bartender
    $groupedShifts = $lineItems | Group-Object { "$($_.Date.ToString('yyyy-MM-dd'))|$($_.Bartender)" }
    foreach ($grp in $groupedShifts) {
        $firstItem = $grp.Group[0]
        $lastItem = $grp.Group | Sort-Object { $_.Timestamp } | Select-Object -Last 1
        $sold = ($grp.Group | Where-Object { $_.Type -eq "SALE" } | Measure-Object -Property Qty -Sum).Sum
        $redeemed = ($grp.Group | Where-Object { $_.Type -eq "REDEEM" } | Measure-Object -Property Qty -Sum).Sum
        if (-not $sold) { $sold = 0 }
        if (-not $redeemed) { $redeemed = 0 }
        
        $events += [PSCustomObject]@{
            Timestamp = $lastItem.Timestamp
            EventType = "SHIFT_ACTIVITY"
            ShiftLabel = "$($firstItem.Date.ToString('ddd, MMM dd')) Shift"
            Bartender = $firstItem.Bartender
            Sold = $sold
            Redeemed = $redeemed
            Replenished = 0
            AuditCount = 0
            Notes = ""
        }
    }
    
    # Sort chronological
    $sortedEvents = $events | Sort-Object { $_.Timestamp }
    $runningStock = 0
    
    $ledgerRows = @()
    foreach ($ev in $sortedEvents) {
        $start = $runningStock
        if ($ev.EventType -in @("AUDIT_BASELINE_SET", "BALANCED", "LEGACY_INFLOW", "SHRINKAGE")) {
            $runningStock = $ev.AuditCount
        } elseif ($ev.EventType -eq "STOCK_REPLENISHED") {
            $runningStock += $ev.Replenished
        } else {
            $runningStock = [Math]::Max(0, $runningStock - $ev.Sold + $ev.Redeemed)
        }
        
        $ledgerRows += [PSCustomObject]@{
            Timestamp = $ev.Timestamp
            EventType = $ev.EventType
            ShiftLabel = $ev.ShiftLabel
            Bartender = $ev.Bartender
            Starting = $start
            Sold = $ev.Sold
            Redeemed = $ev.Redeemed
            Replenished = $ev.Replenished
            Ending = $runningStock
            Notes = $ev.Notes
        }
    }
    
    # Display latest on top
    Write-Host "DISPLAYING LATEST ON TOP (Total rows: $($ledgerRows.Count)):"
    $reversed = [System.Collections.ArrayList]@($ledgerRows)
    $reversed.Reverse()
    foreach ($row in $reversed | Select-Object -First 10) {
        Write-Host "$($row.Timestamp.ToString('yyyy-MM-dd HH:mm')) | $($row.ShiftLabel) ($($row.Bartender)) | $($row.EventType) | Start:$($row.Starting) | Sold:-$($row.Sold) | Redeemed:+$($row.Redeemed) | ENDING:$($row.Ending)"
    }
}
