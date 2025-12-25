# PowerShell script to run the SQL fix for SystemSettings, WebsiteSettings, and ProtectedDocuments
# Run this if you see "Invalid column name" errors on those pages

$projectPath = "apps\webapp\GFC.BlazorServer"
$sqlFilePath = "add-systemsettings-columns.sql"

# SQL script logic is now fully contained in add-systemsettings-columns.sql
Write-Host "--- GFC Database Repair Tool ---" -ForegroundColor Green
Write-Host "This script will ensure all required columns and tables exist for:"
Write-Host "1. Security & Remote Access"
Write-Host "2. Website Settings & Branding"
Write-Host "3. Protected Document Management"
Write-Host ""

$confirm = Read-Host "Proceed with database update? (Y/N)"
if ($confirm -ne "Y") {
    Write-Host "Update cancelled." -ForegroundColor Yellow
    exit
}

if (Test-Path $sqlFilePath) {
    try {
        Write-Host "Executing SQL script on ClubMembership database..." -ForegroundColor Cyan
        $result = sqlcmd -S "localhost" -d ClubMembership -i "$sqlFilePath" 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Database repair completed successfully!" -ForegroundColor Green
            Write-Host $result
        } else {
            Write-Host "X Error executing SQL script:" -ForegroundColor Red
            Write-Host $result
            Write-Host ""
            Write-Host "Manual fix: Open '$sqlFilePath' in SSMS and run it against the 'ClubMembership' database." -ForegroundColor Gray
        }
    } catch {
        Write-Host "X An error occurred: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "Make sure 'sqlcmd' is installed and your SQL Server is running on localhost." -ForegroundColor Gray
    }
} else {
    Write-Host "X Error: '$sqlFilePath' not found!" -ForegroundColor Red
}

Write-Host ""
Write-Host "Press any key to exit..."
$Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
