$Report = New-Object Global:System.Collections.Generic.List[string]
$Report.Add("--- GFC Background Diagnostics ---")

# 1. SQL Services
$sqlServices = Get-Service -Name "MSSQL$SQLEXPRESS", "SQLBrowser" -ErrorAction SilentlyContinue
foreach ($s in $sqlServices) {
    $Report.Add("Service: $($s.Name) Status: $($s.Status) StartType: $($s.StartType)")
}

# 2. App Pool State
Import-Module WebAdministration
$pool = Get-WebAppPoolState -Name "GFCWebApp" -ErrorAction SilentlyContinue
$Report.Add("App Pool GFCWebApp State: $($pool.Value)")

# 3. Connection String Test
$configPath = "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\appsettings.json"
if (Test-Path $configPath) {
    $config = Get-Content $configPath | ConvertFrom-Json
    $conn = $config.ConnectionStrings.GFC
    $Report.Add("Configured ConnString: $conn")
}

# 4. Try SQL Connection using DotNet (more precise than sqlcmd)
$connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;Connect Timeout=5"
$connObject = New-Object System.Data.SqlClient.SqlConnection($connString)
try {
    $connObject.Open()
    $Report.Add("DotNet SQL Connection: SUCCESS")
    $connObject.Close()
} catch {
    $Report.Add("DotNet SQL Connection: FAILED - $($_.Exception.Message)")
}

$Report | Out-File "c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\diag_results.txt"
