# StartSqlServer.ps1
# This script attempts to find and start SQL Server services on the local machine.
# Run this script as Administrator.

Write-Host "Checking for SQL Server services..." -ForegroundColor Cyan

$services = Get-Service | Where-Object { $_.DisplayName -like "*SQL Server*" -and $_.DisplayName -notlike "*Agent*" -and $_.DisplayName -notlike "*Browser*" -and $_.DisplayName -notlike "*Writer*" -and $_.DisplayName -notlike "*VSS*" }

if ($services) {
    foreach ($service in $services) {
        Write-Host "Found service: $($service.Name) ($($service.DisplayName))" -ForegroundColor Gray
        if ($service.Status -ne 'Running') {
            Write-Host "Attempting to start $($service.Name)..." -ForegroundColor Yellow
            try {
                Start-Service -Name $service.Name -ErrorAction Stop
                Write-Host "Successfully started $($service.Name)." -ForegroundColor Green
            }
            catch {
                Write-Host "Failed to start $($service.Name). Ensure you are running as Administrator." -ForegroundColor Red
                Write-Host $_.Exception.Message -ForegroundColor Red
            }
        }
        else {
            Write-Host "$($service.Name) is already running." -ForegroundColor Green
        }
    }
}
else {
    Write-Host "No SQL Server services found." -ForegroundColor Red
    Write-Host "Please ensure Microsoft SQL Server is installed." -ForegroundColor Yellow
}

Write-Host "`nChecking SQL Server Browser service..." -ForegroundColor Cyan
$browserService = Get-Service -Name "SQLBrowser" -ErrorAction SilentlyContinue
if ($browserService) {
     if ($browserService.Status -ne 'Running') {
        Write-Host "Attempting to start SQL Server Browser..." -ForegroundColor Yellow
        try {
            Start-Service -Name "SQLBrowser" -ErrorAction Stop
            Write-Host "Successfully started SQL Server Browser." -ForegroundColor Green
        }
        catch {
             Write-Host "Failed to start SQL Server Browser." -ForegroundColor Red
        }
    }
    else {
        Write-Host "SQL Server Browser is running." -ForegroundColor Green
    }
}
else {
    Write-Host "SQL Server Browser service not found (this is normal for some configurations)." -ForegroundColor Gray
}

Write-Host "`nDone." -ForegroundColor Cyan
