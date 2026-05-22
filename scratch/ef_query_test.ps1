$ProjectDir = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer"
$BinDir = "$ProjectDir\bin\Debug\net10.0"

if (-not (Test-Path "$BinDir\GFC.BlazorServer.dll")) {
    Write-Host "Please build the project first so GFC.BlazorServer.dll exists."
    exit
}

# Add types
Add-Type -Path "$BinDir\Microsoft.EntityFrameworkCore.dll"
Add-Type -Path "$BinDir\GFC.BlazorServer.dll"
Add-Type -Path "$BinDir\GFC.Core.dll"

# Setup DbContext options
$optionsBuilder = New-Object Microsoft.EntityFrameworkCore.DbContextOptionsBuilder[GFC.BlazorServer.Data.GfcDbContext]
$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
[Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions]::UseSqlServer($optionsBuilder, $connString)

$db = New-Object GFC.BlazorServer.Data.GfcDbContext($optionsBuilder.Options)

try {
    Write-Host "Attempting to query Z-Reports via EF Core..."
    $zReports = $db.PosZReports.ToList()
    Write-Host "Success! Loaded $($zReports.Count) Z-reports."
} catch {
    Write-Host "`nEF Core Query Failed!"
    $ex = $_.Exception
    Write-Host "Exception: $($ex.GetType().FullName) - $($ex.Message)"
    if ($ex.InnerException) {
        Write-Host "Inner: $($ex.InnerException.GetType().FullName) - $($ex.InnerException.Message)"
        if ($ex.InnerException.InnerException) {
            Write-Host "Inner Inner: $($ex.InnerException.InnerException.GetType().FullName) - $($ex.InnerException.InnerException.Message)"
        }
    }
    Write-Host "`nStack Trace:"
    Write-Host $ex.StackTrace
} finally {
    $db.Dispose()
}
