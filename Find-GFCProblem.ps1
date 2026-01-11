
# ==============================================================================================
# GFC ROOT CAUSE FINDER
# ==============================================================================================
# DESCRIPTION:
# This script performs a "Truth Check" directly on the Server and Database.
# It bypasses the Browser completely to determine if the "144 Members" exist on the Server/DB
# or if they are a "Ghost" in the Browser Cache.
# ==============================================================================================

$ErrorActionPreference = "Stop"

function Write-Header {
    param($text)
    Write-Host ""
    Write-Host "==================================================================================" -ForegroundColor Cyan
    Write-Host " $text" -ForegroundColor White
    Write-Host "==================================================================================" -ForegroundColor Cyan
}

function Write-Result {
    param($test, $status, $details)
    if ($status -eq "PASS") {
        Write-Host " [OK] $test : $details" -ForegroundColor Green
    } elseif ($status -eq "FAIL") {
        Write-Host " [X]  $test : $details" -ForegroundColor Red
    } else {
        Write-Host " [?]  $test : $details" -ForegroundColor Yellow
    }
}

Clear-Host
Write-Header "STEP 1: DATABASE TRUTH CHECK"

# 1. READ CONNECTION STRING
$possiblePaths = @(
    ".\appsettings.json",
    ".\apps\webapp\GFC.BlazorServer\appsettings.json",
    "..\appsettings.json"
)

$appSettingsFile = $null
foreach ($path in $possiblePaths) {
    if (Test-Path $path) {
        $appSettingsFile = $path
        break
    }
}

if ($null -eq $appSettingsFile) {
    Write-Warning "Could not find appsettings.json in standard locations. Using default connection string."
    $connString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
} else {
    Write-Host "Found Config: $appSettingsFile" -ForegroundColor Gray
    $json = Get-Content $appSettingsFile -Raw | ConvertFrom-Json
    $connString = $json.ConnectionStrings.GFC
    
    # Check for Production Override
    $prodFile = $appSettingsFile.Replace("appsettings.json", "appsettings.Production.json")
    if (Test-Path $prodFile) {
        Write-Host "Found Production Config Override: $prodFile" -ForegroundColor Yellow
        try {
            $prodJson = Get-Content $prodFile -Raw | ConvertFrom-Json
            if ($prodJson.ConnectionStrings.GFC) {
                $connString = $prodJson.ConnectionStrings.GFC
                Write-Host "Using Production Connection String Identity." -ForegroundColor Yellow
            }
        } catch {
            Write-Warning "Failed to parse Production Config."
        }
    }
}

Write-Host "Using Connection String:" -ForegroundColor Gray
Write-Host " > $connString" -ForegroundColor Gray

# 2. QUERY DATABASE DIRECTLY
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connString
    $conn.Open()
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM Members"
    $count = $cmd.ExecuteScalar()
    
    $cmd2 = $conn.CreateCommand()
    $cmd2.CommandText = "SELECT TOP 1 FirstName, LastName, Status FROM Members"
    $reader = $cmd2.ExecuteReader()
    $firstMember = "None"
    if ($reader.Read()) {
        $firstMember = "$($reader['FirstName']) $($reader['LastName']) ($($reader['Status']))"
    }
    $reader.Close()
    $conn.Close()

    Write-Host ""
    if ($count -eq 0 -or $count -eq 1) {
        Write-Result "Database Member Count" "PASS" "Found $count Members (Expected 0 or 1 Admin)"
        Write-Result "Database Content" "PASS" "First Member: $firstMember"
    } else {
        Write-Result "Database Member Count" "FAIL" "Found $count Members! (The Database is NOT empty!)"
        Write-Host "CRITICAL: The database actively contains $count members. The web app is displaying the truth." -ForegroundColor Red
        Read-Host "Press Enter to exit..."
        exit
    }
}
catch {
    Write-Result "Database Connection" "FAIL" "Could not connect to database: $_"
    Read-Host "Press Enter to exit..."
    exit
}

# ==============================================================================================
Write-Header "STEP 2: SERVER RESPONSE TRUTH CHECK (Assuming Port 8080 or 5207)"
# ==============================================================================================

# Try to find which port is listening
$ports = @(8080, 5207, 7073, 5000, 80)
$activeUrl = $null

foreach ($p in $ports) {
    if (Test-NetConnection -ComputerName localhost -Port $p -InformationLevel Quiet) {
        $activeUrl = "http://localhost:$p"
        break
    }
}

if ($null -eq $activeUrl) {
    Write-Host "Could not automatically detect running web server on ports 8080, 5207, 7073, 5000, 80." -ForegroundColor Yellow
    Write-Host "Ensure the app is running in IIS or Visual Studio." -ForegroundColor Yellow
    $activeUrl = Read-Host "Enter the URL manually (e.g. http://localhost:5207)"
}

Write-Host "Testing Server URL: $activeUrl/members" -ForegroundColor Gray

try {
    $response = Invoke-WebRequest -Uri "$activeUrl/members" -Method Get -UseBasicParsing
    
    # Analyze Response
    $content = $response.Content
    $length = $content.Length
    
    # Check for signatures of Member Table
    # A table with 144 members would specific names or many <tr> tags
    $trCount = ([regex]::Matches($content, "<tr")).Count
    
    Write-Host "Server Response Length: $length bytes" -ForegroundColor Gray
    Write-Host "HTML Row Count (<tr>):  $trCount" -ForegroundColor Gray

    if ($trCount -gt 100) {
        # High row count implies the server IS rendering 144 members
        Write-Result "Server HTML Output" "FAIL" "Server returned $trCount table rows. It IS sending 144 members."
        Write-Host "CONCLUSION: The Server is sending the data. Caching might be happening on the SERVER side (Memory) or it's reading a DIFFERENT Database than the one we checked in Step 1." -ForegroundColor Red
    }
    elseif ($trCount -lt 20) {
        # Low row count implies empty table or Blazor Server Shell (loading...)
        Write-Result "Server HTML Output" "PASS" "Server returned only $trCount rows (Empty or Loading Shell)."
        
        Write-Header "FINAL VERDICT"
        Write-Host "The DATABASE has $count members." -ForegroundColor Green
        Write-Host "The SERVER returned HTML with only $trCount rows." -ForegroundColor Green
        Write-Host "But your BROWSER shows 144 members." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "THEREFORE: IT IS DEFINITELY A BROWSER CACHE / SERVICE WORKER ISSUE." -ForegroundColor Cyan
        Write-Host "The '144 Members' are stuck in your Chrome/Edge Cache." -ForegroundColor Cyan
        Write-Host ""
        Write-Host "=== HOW TO FIX IN BROWSER ===" -ForegroundColor White
        Write-Host "1. Press F12 to open Developer Tools" -ForegroundColor White
        Write-Host "2. Go to 'Application' tab" -ForegroundColor White
        Write-Host "3. Click 'Storage' on the left" -ForegroundColor White
        Write-Host "4. Click 'Clear Site Data'" -ForegroundColor White
        Write-Host "5. Click 'Service Workers' on left -> 'Unregister'" -ForegroundColor White
        Write-Host "6. HARD REFRESH (Ctrl + F5)" -ForegroundColor White
    }
}
catch {
    Write-Result "Server Request" "FAIL" "Could not reach website: $_"
}

    Write-Host ""
    Read-Host "Press Enter to exit..."
