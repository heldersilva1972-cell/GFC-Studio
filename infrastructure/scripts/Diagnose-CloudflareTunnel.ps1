# Quick Cloudflare Tunnel Diagnostic - Simple Version
# Identifies why the tunnel is not working

Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  Cloudflare Tunnel Quick Diagnostic" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

$issues = @()
$recommendations = @()

# Check 1: Is cloudflared.exe installed?
Write-Host "[1/5] Checking if cloudflared is installed..." -ForegroundColor Yellow
$cloudflaredPaths = @(
    "C:\Program Files\cloudflared\cloudflared.exe",
    "C:\cloudflared\cloudflared.exe",
    "$env:USERPROFILE\cloudflared\cloudflared.exe"
)

$cloudflaredFound = $false
$cloudflaredPath = $null

foreach ($path in $cloudflaredPaths) {
    if (Test-Path $path) {
        $cloudflaredFound = $true
        $cloudflaredPath = $path
        Write-Host "  Found: $path" -ForegroundColor Green
        break
    }
}

if (-not $cloudflaredFound) {
    Write-Host "  cloudflared.exe NOT FOUND" -ForegroundColor Red
    $issues += "Cloudflare Tunnel (cloudflared.exe) is not installed"
    $recommendations += "Run: infrastructure\scripts\Install-CloudflareTunnel.ps1"
}

# Check 2: Is the Windows service installed?
Write-Host "[2/5] Checking Windows service..." -ForegroundColor Yellow
try {
    $service = Get-Service -Name "cloudflared" -ErrorAction Stop
    $statusColor = if ($service.Status -eq 'Running') { 'Green' } else { 'Yellow' }
    Write-Host "  Service exists: $($service.Status)" -ForegroundColor $statusColor
    
    if ($service.Status -ne 'Running') {
        $issues += "Cloudflare Tunnel service is installed but not running (Status: $($service.Status))"
        $recommendations += "Run: Start-Service cloudflared"
    }
    
    if ($service.StartType -ne 'Automatic') {
        $issues += "Service is not set to auto-start (StartType: $($service.StartType))"
        $recommendations += "Run: Set-Service -Name cloudflared -StartupType Automatic"
    }
} catch {
    Write-Host "  Service NOT installed" -ForegroundColor Red
    if ($cloudflaredFound) {
        $issues += "Cloudflare Tunnel service is not installed"
        $recommendations += "Run: cloudflared service install"
    }
}

# Check 3: Is the config file present?
Write-Host "[3/5] Checking configuration..." -ForegroundColor Yellow
$configPath = "$env:USERPROFILE\.cloudflared\config.yml"
if (Test-Path $configPath) {
    Write-Host "  Config file exists: $configPath" -ForegroundColor Green
    
    # Check config content
    $config = Get-Content $configPath -Raw
    if ($config -match "gfc.lovanow.com") {
        Write-Host "  Config contains gfc.lovanow.com" -ForegroundColor Green
    } else {
        Write-Host "  Config does NOT contain gfc.lovanow.com" -ForegroundColor Yellow
        $issues += "Config file exists but may not be configured for gfc.lovanow.com"
        $recommendations += "Review: $configPath"
    }
    
    if ($config -match "localhost:8080") {
        Write-Host "  Config points to localhost:8080" -ForegroundColor Green
    } else {
        Write-Host "  Config does NOT point to localhost:8080" -ForegroundColor Yellow
        $issues += "Config may not be pointing to correct IIS port"
    }
} else {
    Write-Host "  Config file NOT found" -ForegroundColor Red
    $issues += "Cloudflare Tunnel configuration file missing"
    $recommendations += "Run: infrastructure\scripts\Install-CloudflareTunnel.ps1"
}

# Check 4: Is IIS running on port 8080?
Write-Host "[4/5] Checking IIS..." -ForegroundColor Yellow
try {
    $iisService = Get-Service W3SVC -ErrorAction Stop
    if ($iisService.Status -eq 'Running') {
        Write-Host "  IIS service is running" -ForegroundColor Green
        
        # Check if port 8080 is listening
        $port8080 = netstat -ano | Select-String ":8080.*LISTENING"
        if ($port8080) {
            Write-Host "  Port 8080 is listening" -ForegroundColor Green
            
            # Try to connect locally
            try {
                $localTest = Invoke-WebRequest http://localhost:8080 -UseBasicParsing -TimeoutSec 5 -ErrorAction Stop
                Write-Host "  Local site responds (Status: $($localTest.StatusCode))" -ForegroundColor Green
            } catch {
                Write-Host "  Local site does NOT respond" -ForegroundColor Red
                $issues += "IIS is running but site on port 8080 is not responding"
                $recommendations += "Check IIS site configuration and ensure app is running"
            }
        } else {
            Write-Host "  Port 8080 is NOT listening" -ForegroundColor Red
            $issues += "IIS is running but not listening on port 8080"
            $recommendations += "Check IIS site bindings - should include http://localhost:8080"
        }
    } else {
        Write-Host "  IIS service is NOT running (Status: $($iisService.Status))" -ForegroundColor Red
        $issues += "IIS service is not running"
        $recommendations += "Run: Start-Service W3SVC"
    }
} catch {
    Write-Host "  IIS service NOT found" -ForegroundColor Red
    $issues += "IIS is not installed or not configured"
    $recommendations += "Install IIS and configure the GFC application"
}

# Check 5: Can we reach Cloudflare?
Write-Host "[5/5] Checking Cloudflare connectivity..." -ForegroundColor Yellow
try {
    $dns = Resolve-DnsName -Name "gfc.lovanow.com" -ErrorAction Stop
    $cloudflareIPs = $dns | Where-Object { $_.Type -eq 'A' } | Select-Object -ExpandProperty IPAddress
    
    if ($cloudflareIPs) {
        Write-Host "  DNS resolves to Cloudflare: $($cloudflareIPs -join ', ')" -ForegroundColor Green
    } else {
        Write-Host "  DNS resolves but no A records found" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  DNS resolution failed" -ForegroundColor Red
    $issues += "Cannot resolve gfc.lovanow.com"
    $recommendations += "Check Cloudflare DNS configuration"
}

# Summary
Write-Host ""
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  Diagnostic Summary" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

if ($issues.Count -eq 0) {
    Write-Host "No issues detected!" -ForegroundColor Green
    Write-Host ""
    Write-Host "If the site still doesn't work, try:" -ForegroundColor Cyan
    Write-Host "  1. Restart the cloudflared service: Restart-Service cloudflared" -ForegroundColor White
    Write-Host "  2. Wait 30 seconds for tunnel to stabilize" -ForegroundColor White
    Write-Host "  3. Test: https://gfc.lovanow.com" -ForegroundColor White
} else {
    Write-Host "Found $($issues.Count) issue(s):" -ForegroundColor Red
    Write-Host ""
    
    for ($i = 0; $i -lt $issues.Count; $i++) {
        Write-Host "  $($i + 1). $($issues[$i])" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "Recommended Actions:" -ForegroundColor Cyan
    Write-Host ""
    
    for ($i = 0; $i -lt $recommendations.Count; $i++) {
        Write-Host "  $($i + 1). $($recommendations[$i])" -ForegroundColor White
    }
}

Write-Host ""
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

# Provide next steps based on findings
if (-not $cloudflaredFound) {
    Write-Host "NEXT STEP: Install Cloudflare Tunnel" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Run this command:" -ForegroundColor Cyan
    Write-Host '  .\Install-CloudflareTunnel.ps1 -IISPort 8080 -Hostname gfc.lovanow.com' -ForegroundColor White
} elseif ($service -and $service.Status -ne 'Running') {
    Write-Host "NEXT STEP: Start Cloudflare Tunnel Service" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Run this command:" -ForegroundColor Cyan
    Write-Host '  Start-Service cloudflared' -ForegroundColor White
    Write-Host '  Get-Service cloudflared  # Verify it started' -ForegroundColor White
} else {
    Write-Host "NEXT STEP: Review issues above and apply recommended actions" -ForegroundColor Yellow
}

Write-Host ""
