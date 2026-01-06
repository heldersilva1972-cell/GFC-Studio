# Apply Migration Wizard Schema
# This script runs the migration using the correct SQL Server instance

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Migration Wizard Schema Installation" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Try different SQL Server instance names
$instances = @(
    "(localdb)\MSSQLLocalDB",
    ".\ClubMembership",
    "localhost",
    "(localdb)\ClubMembership",
    "."
)

$scriptPath = "docs\DatabaseScripts\Manual_MigrationWizard_Schema.sql"
$database = "ClubMembership"

$success = $false

foreach ($instance in $instances) {
    Write-Host "Trying SQL Server instance: $instance" -ForegroundColor Yellow
    
    try {
        $result = sqlcmd -S $instance -d $database -i $scriptPath -b 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ SUCCESS! Connected to: $instance" -ForegroundColor Green
            Write-Host ""
            Write-Host "Migration script executed successfully!" -ForegroundColor Green
            $success = $true
            break
        }
    }
    catch {
        Write-Host "  ✗ Failed to connect" -ForegroundColor Red
    }
}

if (-not $success) {
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Red
    Write-Host "ALTERNATIVE: Run via Application Startup" -ForegroundColor Red
    Write-Host "========================================" -ForegroundColor Red
    Write-Host ""
    Write-Host "The migration will run automatically when you start the application." -ForegroundColor Yellow
    Write-Host "The app's Program.cs already includes auto-migration logic." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Just restart your app and the table will be created automatically!" -ForegroundColor Green
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Complete" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
