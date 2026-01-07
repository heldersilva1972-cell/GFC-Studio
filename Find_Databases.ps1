$instances = @(".\SQLEXPRESS", "(localdb)\MSSQLLocalDB", ".", "(localdb)\ProjectsV13", "(localdb)\ProjectModels")

foreach ($inst in $instances) {
    Write-Host "Checking Instance: $inst" -ForegroundColor Cyan
    try {
        $query = "SELECT name, create_date FROM sys.databases WHERE name NOT IN ('master','tempdb','model','msdb')"
        $cmd = "sqlcmd -S `"$inst`" -E -W -s `",`" -Q `"$query`" -t 2" 
        # -t 2 sets login timeout to 2 seconds to fail fast
        $res = cmd /c $cmd 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  [SUCCESS] Connected!" -ForegroundColor Green
            if ($res -match "rows affected" -and -not ($res -match "0 rows affected")) {
                Write-Host "  -> FOUND DATABASES:" -ForegroundColor Yellow
                $res | ForEach-Object { Write-Host "     $_" }
            } else {
                Write-Host "  -> Instance is empty (no user databases)." -ForegroundColor Gray
            }
        } else {
            Write-Host "  [FAILED] Could not connect." -ForegroundColor Red
        }
    } catch {
        Write-Host "  [ERROR] Script failure."
    }
    Write-Host ""
}
