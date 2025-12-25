# Add Missing SystemSettings Columns Script
# This script adds the missing columns to the SystemSettings table

Write-Host "Adding missing columns to SystemSettings table..." -ForegroundColor Cyan

$scriptPath = $PSScriptRoot
$projectPath = Join-Path $scriptPath "apps\webapp\GFC.BlazorServer"

# SQL script to add missing columns
$sqlScript = @"
USE [GFC_DB]
GO

-- Add missing columns to SystemSettings table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'CloudflareTunnelToken')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [CloudflareTunnelToken] NVARCHAR(500) NULL;
    PRINT 'Added CloudflareTunnelToken column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'MaxSimultaneousViewers')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [MaxSimultaneousViewers] INT NULL DEFAULT 5;
    PRINT 'Added MaxSimultaneousViewers column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'PublicDomain')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [PublicDomain] NVARCHAR(255) NULL;
    PRINT 'Added PublicDomain column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardAllowedIPs')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardAllowedIPs] NVARCHAR(500) NULL;
    PRINT 'Added WireGuardAllowedIPs column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardPort')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardPort] INT NULL DEFAULT 51820;
    PRINT 'Added WireGuardPort column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardServerPublicKey')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardServerPublicKey] NVARCHAR(500) NULL;
    PRINT 'Added WireGuardServerPublicKey column';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemSettings]') AND name = 'WireGuardSubnet')
BEGIN
    ALTER TABLE [dbo].[SystemSettings] ADD [WireGuardSubnet] NVARCHAR(50) NULL DEFAULT '10.8.0.0/24';
    PRINT 'Added WireGuardSubnet column';
END

PRINT 'All missing columns have been added successfully!';
GO
"@

# Save SQL script to file
$sqlFilePath = Join-Path $scriptPath "add-systemsettings-columns.sql"
$sqlScript | Out-File -FilePath $sqlFilePath -Encoding UTF8

Write-Host "SQL script created at: $sqlFilePath" -ForegroundColor Green
Write-Host ""
Write-Host "To apply the changes, run ONE of the following options:" -ForegroundColor Yellow
Write-Host ""
Write-Host "OPTION 1: Using SQL Server Management Studio (SSMS)" -ForegroundColor Cyan
Write-Host "  1. Open SQL Server Management Studio" -ForegroundColor White
Write-Host "  2. Connect to your SQL Server instance" -ForegroundColor White
Write-Host "  3. Open the file: $sqlFilePath" -ForegroundColor White
Write-Host "  4. Execute the script (F5)" -ForegroundColor White
Write-Host ""
Write-Host "OPTION 2: Using sqlcmd command line" -ForegroundColor Cyan
Write-Host "  Run this command:" -ForegroundColor White
Write-Host "  sqlcmd -S (localdb)\MSSQLLocalDB -d GFC_DB -i `"$sqlFilePath`"" -ForegroundColor Gray
Write-Host ""
Write-Host "OPTION 3: Using EF Core Migration (Recommended)" -ForegroundColor Cyan
Write-Host "  cd `"$projectPath`"" -ForegroundColor Gray
Write-Host "  dotnet ef migrations add AddSystemSettingsVpnColumns" -ForegroundColor Gray
Write-Host "  dotnet ef database update" -ForegroundColor Gray
Write-Host ""

# Ask user which option they want
Write-Host "Would you like me to try running the SQL script now using sqlcmd? (Y/N)" -ForegroundColor Yellow
$response = Read-Host

if ($response -eq 'Y' -or $response -eq 'y') {
    Write-Host "Attempting to run SQL script..." -ForegroundColor Cyan
    
    try {
        # Try to run sqlcmd
        $result = sqlcmd -S "(localdb)\MSSQLLocalDB" -d GFC_DB -i "$sqlFilePath" 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "SQL script executed successfully!" -ForegroundColor Green
            Write-Host $result
        } else {
            Write-Host "Error executing SQL script:" -ForegroundColor Red
            Write-Host $result
            Write-Host ""
            Write-Host "Please try one of the manual options above." -ForegroundColor Yellow
        }
    }
    catch {
        Write-Host "Error: sqlcmd not found or failed to execute." -ForegroundColor Red
        Write-Host "Please use SSMS or create an EF Core migration instead." -ForegroundColor Yellow
    }
} else {
    Write-Host "Skipped automatic execution. Please run the script manually using one of the options above." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Script complete!" -ForegroundColor Green
