<#
.SYNOPSIS
    Verifies Cloudflare Tunnel setup and HTTPS configuration

.DESCRIPTION
    This script performs comprehensive verification of the Cloudflare Tunnel
    installation, including certificate validation, service status, and
    security checks.

.PARAMETER Hostname
    The public hostname to verify (default: gfc.lovanow.com)

.PARAMETER LocalPort
    The local IIS port (default: 8080)

.EXAMPLE
    .\Verify-CloudflareTunnel.ps1 -Hostname gfc.lovanow.com

.NOTES
    Can be run without Administrator privileges
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$false)]
    [string]$Hostname = "gfc.lovanow.com",
    
    [Parameter(Mandatory=$false)]
    [int]$LocalPort = 8080
)

$ErrorActionPreference = "Continue"

# Color output functions
function Write-Pass {
    param([string]$Message)
    Write-Host "  ✓ PASS: $Message" -ForegroundColor Green
}

function Write-Fail {
    param([string]$Message)
    Write-Host "  ✗ FAIL: $Message" -ForegroundColor Red
}

function Write-Warn {
    param([string]$Message)
    Write-Host "  ⚠ WARN: $Message" -ForegroundColor Yellow
}

function Write-TestHeader {
    param([string]$Message)
    Write-Host "`n╔═══════════════════════════════════════════════════════╗" -ForegroundColor Cyan
    Write-Host "  $Message" -ForegroundColor Cyan
    Write-Host "╚═══════════════════════════════════════════════════════╝" -ForegroundColor Cyan
}

# Test results tracking
$script:PassCount = 0
$script:FailCount = 0
$script:WarnCount = 0

function Record-Pass { $script:PassCount++ }
function Record-Fail { $script:FailCount++ }
function Record-Warn { $script:WarnCount++ }

# Test 1: Cloudflared Installation
function Test-CloudflaredInstallation {
    Write-TestHeader "Test 1: Cloudflared Installation"
    
    $cloudflaredPath = "C:\Program Files\cloudflared\cloudflared.exe"
    
    if (Test-Path $cloudflaredPath) {
        Write-Pass "Cloudflared executable found at: $cloudflaredPath"
        Record-Pass
        
        try {
            $version = & $cloudflaredPath --version 2>&1
            Write-Pass "Version: $version"
            Record-Pass
        }
        catch {
            Write-Fail "Could not execute cloudflared: $_"
            Record-Fail
        }
    }
    else {
        Write-Fail "Cloudflared not found at: $cloudflaredPath"
        Record-Fail
    }
    
    # Check PATH
    if ($env:Path -like "*cloudflared*") {
        Write-Pass "Cloudflared is in system PATH"
        Record-Pass
    }
    else {
        Write-Warn "Cloudflared not in PATH (may require restart)"
        Record-Warn
    }
}

# Test 2: Authentication
function Test-CloudflareAuthentication {
    Write-TestHeader "Test 2: Cloudflare Authentication"
    
    $certPath = Join-Path $env:USERPROFILE ".cloudflared\cert.pem"
    
    if (Test-Path $certPath) {
        Write-Pass "Certificate file exists: $certPath"
        Record-Pass
        
        $certInfo = Get-Item $certPath
        Write-Pass "Certificate created: $($certInfo.CreationTime)"
        Record-Pass
        
        # Check file size (should be > 0)
        if ($certInfo.Length -gt 0) {
            Write-Pass "Certificate file size: $($certInfo.Length) bytes"
            Record-Pass
        }
        else {
            Write-Fail "Certificate file is empty"
            Record-Fail
        }
    }
    else {
        Write-Fail "Certificate not found. Run: cloudflared tunnel login"
        Record-Fail
    }
}

# Test 3: Tunnel Configuration
function Test-TunnelConfiguration {
    Write-TestHeader "Test 3: Tunnel Configuration"
    
    $configPath = Join-Path $env:USERPROFILE ".cloudflared\config.yml"
    
    if (Test-Path $configPath) {
        Write-Pass "Config file exists: $configPath"
        Record-Pass
        
        $config = Get-Content $configPath -Raw
        
        # Check for required fields
        if ($config -match "tunnel:\s*([a-f0-9-]{36})") {
            $tunnelId = $matches[1]
            Write-Pass "Tunnel ID found: $tunnelId"
            Record-Pass
            
            # Check credentials file
            $credPath = Join-Path $env:USERPROFILE ".cloudflared\$tunnelId.json"
            if (Test-Path $credPath) {
                Write-Pass "Credentials file exists: $credPath"
                Record-Pass
            }
            else {
                Write-Fail "Credentials file not found: $credPath"
                Record-Fail
            }
        }
        else {
            Write-Fail "Tunnel ID not found in config"
            Record-Fail
        }
        
        if ($config -match "hostname:\s*$Hostname") {
            Write-Pass "Hostname configured: $Hostname"
            Record-Pass
        }
        else {
            Write-Warn "Hostname '$Hostname' not found in config"
            Record-Warn
        }
        
        if ($config -match "service:\s*http://localhost:(\d+)") {
            $port = $matches[1]
            Write-Pass "Local service: http://localhost:$port"
            if ($port -eq $LocalPort) {
                Record-Pass
            }
            else {
                Write-Warn "Port mismatch: expected $LocalPort, found $port"
                Record-Warn
            }
        }
        else {
            Write-Fail "Service URL not found in config"
            Record-Fail
        }
    }
    else {
        Write-Fail "Config file not found: $configPath"
        Record-Fail
    }
}

# Test 4: Windows Service
function Test-WindowsService {
    Write-TestHeader "Test 4: Windows Service Status"
    
    try {
        $service = Get-Service -Name "cloudflared" -ErrorAction Stop
        
        Write-Pass "Service 'cloudflared' exists"
        Record-Pass
        
        if ($service.Status -eq "Running") {
            Write-Pass "Service is Running"
            Record-Pass
        }
        else {
            Write-Fail "Service is $($service.Status) (should be Running)"
            Record-Fail
        }
        
        if ($service.StartType -eq "Automatic") {
            Write-Pass "Service startup: Automatic"
            Record-Pass
        }
        else {
            Write-Warn "Service startup: $($service.StartType) (should be Automatic)"
            Record-Warn
        }
    }
    catch {
        Write-Fail "Service 'cloudflared' not found or not accessible"
        Record-Fail
    }
}

# Test 5: Local IIS Endpoint
function Test-LocalEndpoint {
    Write-TestHeader "Test 5: Local IIS Endpoint"
    
    $localUrl = "http://localhost:$LocalPort"
    
    try {
        $response = Invoke-WebRequest -Uri $localUrl -TimeoutSec 10 -UseBasicParsing
        
        if ($response.StatusCode -eq 200) {
            Write-Pass "Local endpoint accessible: $localUrl"
            Record-Pass
        }
        else {
            Write-Warn "Local endpoint returned status: $($response.StatusCode)"
            Record-Warn
        }
    }
    catch {
        Write-Fail "Cannot reach local endpoint: $localUrl"
        Write-Fail "Error: $_"
        Record-Fail
    }
}

# Test 6: HTTPS Certificate
function Test-HTTPSCertificate {
    Write-TestHeader "Test 6: HTTPS Certificate Validation"
    
    $publicUrl = "https://$Hostname"
    
    try {
        # Test HTTPS connectivity
        $response = Invoke-WebRequest -Uri $publicUrl -TimeoutSec 15 -UseBasicParsing
        
        if ($response.StatusCode -eq 200) {
            Write-Pass "HTTPS endpoint accessible: $publicUrl"
            Record-Pass
        }
        else {
            Write-Warn "HTTPS endpoint returned status: $($response.StatusCode)"
            Record-Warn
        }
        
        # Check certificate (requires .NET)
        $request = [System.Net.HttpWebRequest]::Create($publicUrl)
        $request.Timeout = 15000
        $request.AllowAutoRedirect = $true
        
        try {
            $response = $request.GetResponse()
            $cert = $request.ServicePoint.Certificate
            
            if ($cert) {
                $cert2 = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2 $cert
                
                Write-Pass "Certificate Subject: $($cert2.Subject)"
                Write-Pass "Certificate Issuer: $($cert2.Issuer)"
                Write-Pass "Valid From: $($cert2.NotBefore)"
                Write-Pass "Valid Until: $($cert2.NotAfter)"
                Record-Pass
                
                # Check if certificate is valid
                $now = Get-Date
                if ($now -ge $cert2.NotBefore -and $now -le $cert2.NotAfter) {
                    Write-Pass "Certificate is currently valid"
                    Record-Pass
                }
                else {
                    Write-Fail "Certificate is expired or not yet valid"
                    Record-Fail
                }
                
                # Check issuer (should be Cloudflare or Let's Encrypt)
                if ($cert2.Issuer -match "Cloudflare|Let's Encrypt") {
                    Write-Pass "Certificate issued by trusted CA"
                    Record-Pass
                }
                else {
                    Write-Warn "Certificate issuer may not be Cloudflare/Let's Encrypt"
                    Record-Warn
                }
            }
            
            $response.Close()
        }
        catch {
            Write-Fail "Certificate validation failed: $_"
            Record-Fail
        }
    }
    catch {
        Write-Fail "Cannot reach HTTPS endpoint: $publicUrl"
        Write-Fail "Error: $_"
        Record-Fail
    }
}

# Test 7: DNS Resolution
function Test-DNSResolution {
    Write-TestHeader "Test 7: DNS Resolution"
    
    try {
        $dnsResult = Resolve-DnsName -Name $Hostname -ErrorAction Stop
        
        if ($dnsResult) {
            Write-Pass "DNS resolves for: $Hostname"
            Record-Pass
            
            foreach ($record in $dnsResult) {
                if ($record.Type -eq "A") {
                    Write-Pass "A Record: $($record.IPAddress)"
                }
                elseif ($record.Type -eq "CNAME") {
                    Write-Pass "CNAME: $($record.NameHost)"
                    
                    # Check if it's a Cloudflare tunnel
                    if ($record.NameHost -match "cfargotunnel.com") {
                        Write-Pass "DNS points to Cloudflare Tunnel"
                        Record-Pass
                    }
                }
            }
        }
    }
    catch {
        Write-Fail "DNS resolution failed for: $Hostname"
        Write-Fail "Error: $_"
        Record-Fail
    }
}

# Test 8: Firewall / Port Security
function Test-PortSecurity {
    Write-TestHeader "Test 8: Port Security (Localhost Only)"
    
    try {
        $listening = Get-NetTCPConnection -State Listen -ErrorAction SilentlyContinue | 
                     Where-Object { $_.LocalPort -eq $LocalPort }
        
        if ($listening) {
            $localOnly = $listening | Where-Object { $_.LocalAddress -eq "127.0.0.1" -or $_.LocalAddress -eq "::1" }
            $publicBind = $listening | Where-Object { $_.LocalAddress -eq "0.0.0.0" -or $_.LocalAddress -eq "::" }
            
            if ($localOnly -and -not $publicBind) {
                Write-Pass "Port $LocalPort is bound to localhost only (secure)"
                Record-Pass
            }
            elseif ($publicBind) {
                Write-Warn "Port $LocalPort is publicly accessible (0.0.0.0)"
                Write-Warn "Consider binding IIS to localhost only for security"
                Record-Warn
            }
            else {
                Write-Pass "Port $LocalPort is listening"
                Record-Pass
            }
        }
        else {
            Write-Fail "Port $LocalPort is not listening"
            Record-Fail
        }
    }
    catch {
        Write-Warn "Could not check port bindings: $_"
        Record-Warn
    }
}

# Test 9: Tunnel Connectivity
function Test-TunnelConnectivity {
    Write-TestHeader "Test 9: Tunnel Connectivity"
    
    $cloudflaredPath = "C:\Program Files\cloudflared\cloudflared.exe"
    
    if (Test-Path $cloudflaredPath) {
        try {
            $tunnelInfo = & $cloudflaredPath tunnel info gfc-webapp 2>&1 | Out-String
            
            if ($tunnelInfo -match "error") {
                Write-Fail "Tunnel info returned error"
                Record-Fail
            }
            else {
                Write-Pass "Tunnel information retrieved successfully"
                Record-Pass
                
                # Check for active connections
                if ($tunnelInfo -match "(\d+)\s+connection") {
                    $connections = $matches[1]
                    if ([int]$connections -ge 4) {
                        Write-Pass "Tunnel has $connections active connections (healthy)"
                        Record-Pass
                    }
                    else {
                        Write-Warn "Tunnel has only $connections connections (expected 4)"
                        Record-Warn
                    }
                }
            }
        }
        catch {
            Write-Warn "Could not retrieve tunnel info: $_"
            Record-Warn
        }
    }
}

# Test 10: Service Worker / PWA Readiness
function Test-PWAReadiness {
    Write-TestHeader "Test 10: PWA / Service Worker Readiness"
    
    $publicUrl = "https://$Hostname"
    
    try {
        $response = Invoke-WebRequest -Uri $publicUrl -TimeoutSec 15 -UseBasicParsing
        
        # Check for Service Worker registration
        if ($response.Content -match "serviceWorker|service-worker") {
            Write-Pass "Service Worker code detected in page"
            Record-Pass
        }
        else {
            Write-Warn "Service Worker code not detected (may be in separate file)"
            Record-Warn
        }
        
        # Check for manifest
        if ($response.Content -match "manifest\.json|manifest\.webmanifest") {
            Write-Pass "PWA manifest detected"
            Record-Pass
        }
        else {
            Write-Warn "PWA manifest not detected in HTML"
            Record-Warn
        }
        
        # Check security headers
        $headers = $response.Headers
        
        if ($headers["Strict-Transport-Security"]) {
            Write-Pass "HSTS header present: $($headers['Strict-Transport-Security'])"
            Record-Pass
        }
        else {
            Write-Warn "HSTS header not present (recommended for PWA)"
            Record-Warn
        }
    }
    catch {
        Write-Warn "Could not check PWA readiness: $_"
        Record-Warn
    }
}

# Generate Summary Report
function Show-Summary {
    Write-Host "`n`n"
    Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Magenta
    Write-Host "║                    VERIFICATION SUMMARY                        ║" -ForegroundColor Magenta
    Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Magenta
    Write-Host ""
    
    $total = $script:PassCount + $script:FailCount + $script:WarnCount
    $passPercent = if ($total -gt 0) { [math]::Round(($script:PassCount / $total) * 100, 1) } else { 0 }
    
    Write-Host "  Total Tests:    $total" -ForegroundColor Cyan
    Write-Host "  ✓ Passed:       $script:PassCount ($passPercent%)" -ForegroundColor Green
    Write-Host "  ✗ Failed:       $script:FailCount" -ForegroundColor Red
    Write-Host "  ⚠ Warnings:     $script:WarnCount" -ForegroundColor Yellow
    Write-Host ""
    
    if ($script:FailCount -eq 0 -and $script:WarnCount -eq 0) {
        Write-Host "  🎉 ALL CHECKS PASSED! Cloudflare Tunnel is properly configured." -ForegroundColor Green
        Write-Host ""
        Write-Host "  Next Steps:" -ForegroundColor Cyan
        Write-Host "    1. Test login from external network" -ForegroundColor Gray
        Write-Host "    2. Verify Service Worker in browser DevTools" -ForegroundColor Gray
        Write-Host "    3. Test PWA installation on mobile device" -ForegroundColor Gray
        Write-Host "    4. Monitor tunnel logs for any issues" -ForegroundColor Gray
    }
    elseif ($script:FailCount -eq 0) {
        Write-Host "  ✓ No critical failures, but some warnings to review." -ForegroundColor Yellow
    }
    else {
        Write-Host "  ⚠ Some checks failed. Review the output above for details." -ForegroundColor Red
        Write-Host ""
        Write-Host "  Common fixes:" -ForegroundColor Cyan
        Write-Host "    - Ensure cloudflared service is running: Start-Service cloudflared" -ForegroundColor Gray
        Write-Host "    - Verify IIS is running on port $LocalPort" -ForegroundColor Gray
        Write-Host "    - Check config file: $env:USERPROFILE\.cloudflared\config.yml" -ForegroundColor Gray
        Write-Host "    - Review setup guide: infrastructure\CLOUDFLARE_TUNNEL_SETUP.md" -ForegroundColor Gray
    }
    
    Write-Host ""
    Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Magenta
    Write-Host ""
}

# Main execution
try {
    Write-Host @"

╔════════════════════════════════════════════════════════════════╗
║         CLOUDFLARE TUNNEL VERIFICATION SCRIPT                  ║
║                  GFC Web Application                           ║
╚════════════════════════════════════════════════════════════════╝

Testing configuration for: https://$Hostname
Local service port: $LocalPort

"@ -ForegroundColor Magenta

    # Run all tests
    Test-CloudflaredInstallation
    Test-CloudflareAuthentication
    Test-TunnelConfiguration
    Test-WindowsService
    Test-LocalEndpoint
    Test-DNSResolution
    Test-HTTPSCertificate
    Test-PortSecurity
    Test-TunnelConnectivity
    Test-PWAReadiness
    
    # Show summary
    Show-Summary
    
    # Exit code based on failures
    if ($script:FailCount -gt 0) {
        exit 1
    }
    else {
        exit 0
    }
}
catch {
    Write-Host "`nUnexpected error during verification: $_" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor Red
    exit 1
}
