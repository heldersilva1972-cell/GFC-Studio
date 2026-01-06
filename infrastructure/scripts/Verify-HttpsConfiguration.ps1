# Cloudflare Tunnel HTTPS - Quick Verification Script
# Run this after deploying to production to verify HTTPS is working correctly

Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  Cloudflare Tunnel HTTPS Verification" -ForegroundColor Cyan
Write-Host "  Testing: https://gfc.lovanow.com" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host ""

$url = "https://gfc.lovanow.com"
$passed = 0
$failed = 0
$warnings = 0

# Test 1: DNS Resolution
Write-Host "[Test 1/6] DNS Resolution..." -ForegroundColor Yellow
try {
    $dns = Resolve-DnsName -Name "gfc.lovanow.com" -ErrorAction Stop
    if ($dns) {
        Write-Host "  ✓ PASS: DNS resolves to Cloudflare" -ForegroundColor Green
        $passed++
    }
} catch {
    Write-Host "  ✗ FAIL: DNS resolution failed" -ForegroundColor Red
    $failed++
}

# Test 2: HTTPS Connection
Write-Host "[Test 2/6] HTTPS Connection..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 10 -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host "  ✓ PASS: HTTPS connection successful (Status: $($response.StatusCode))" -ForegroundColor Green
        $passed++
    }
} catch {
    Write-Host "  ✗ FAIL: HTTPS connection failed - $($_.Exception.Message)" -ForegroundColor Red
    $failed++
}

# Test 3: Certificate Validation
Write-Host "[Test 3/6] SSL Certificate..." -ForegroundColor Yellow
try {
    $req = [System.Net.HttpWebRequest]::Create($url)
    $req.Method = "HEAD"
    $req.Timeout = 10000
    $response = $req.GetResponse()
    $cert = $req.ServicePoint.Certificate
    
    if ($cert) {
        $certExpiry = [DateTime]::Parse($cert.GetExpirationDateString())
        $daysUntilExpiry = ($certExpiry - (Get-Date)).Days
        
        if ($daysUntilExpiry -gt 30) {
            Write-Host "  ✓ PASS: Valid certificate (Expires in $daysUntilExpiry days)" -ForegroundColor Green
            $passed++
        } elseif ($daysUntilExpiry -gt 0) {
            Write-Host "  ⚠ WARNING: Certificate expires soon ($daysUntilExpiry days)" -ForegroundColor Yellow
            $warnings++
        } else {
            Write-Host "  ✗ FAIL: Certificate expired" -ForegroundColor Red
            $failed++
        }
    }
    $response.Close()
} catch {
    Write-Host "  ✗ FAIL: Certificate validation failed - $($_.Exception.Message)" -ForegroundColor Red
    $failed++
}

# Test 4: Cloudflare Tunnel Status
Write-Host "[Test 4/6] Cloudflare Tunnel Status..." -ForegroundColor Yellow
try {
    $tunnelStatus = cloudflared tunnel info gfc-webapp 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ PASS: Tunnel 'gfc-webapp' is configured" -ForegroundColor Green
        $passed++
    } else {
        Write-Host "  ⚠ WARNING: Could not verify tunnel status (cloudflared may not be in PATH)" -ForegroundColor Yellow
        $warnings++
    }
} catch {
    Write-Host "  ⚠ WARNING: cloudflared command not found" -ForegroundColor Yellow
    $warnings++
}

# Test 5: Service Worker (PWA)
Write-Host "[Test 5/6] Service Worker Registration..." -ForegroundColor Yellow
try {
    $swUrl = "$url/service-worker.js"
    $swResponse = Invoke-WebRequest -Uri $swUrl -UseBasicParsing -TimeoutSec 10 -ErrorAction Stop
    if ($swResponse.StatusCode -eq 200) {
        Write-Host "  ✓ PASS: Service worker accessible" -ForegroundColor Green
        $passed++
    }
} catch {
    Write-Host "  ⚠ WARNING: Service worker not found (may not be deployed yet)" -ForegroundColor Yellow
    $warnings++
}

# Test 6: Manifest (PWA)
Write-Host "[Test 6/6] PWA Manifest..." -ForegroundColor Yellow
try {
    $manifestUrl = "$url/manifest.json"
    $manifestResponse = Invoke-WebRequest -Uri $manifestUrl -UseBasicParsing -TimeoutSec 10 -ErrorAction Stop
    if ($manifestResponse.StatusCode -eq 200) {
        $manifest = $manifestResponse.Content | ConvertFrom-Json
        if ($manifest.start_url -and $manifest.name) {
            Write-Host "  ✓ PASS: Manifest valid (Name: $($manifest.name))" -ForegroundColor Green
            $passed++
        }
    }
} catch {
    Write-Host "  ⚠ WARNING: Manifest not accessible" -ForegroundColor Yellow
    $warnings++
}

# Summary
Write-Host ""
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  Test Summary" -ForegroundColor Cyan
Write-Host "===========================================================" -ForegroundColor Cyan
Write-Host "  Passed:   $passed" -ForegroundColor Green
Write-Host "  Failed:   $failed" -ForegroundColor $(if ($failed -gt 0) { "Red" } else { "Gray" })
Write-Host "  Warnings: $warnings" -ForegroundColor $(if ($warnings -gt 0) { "Yellow" } else { "Gray" })
Write-Host ""

if ($failed -eq 0 -and $passed -ge 4) {
    Write-Host "✓ OVERALL: HTTPS is working correctly!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Next Steps:" -ForegroundColor Cyan
    Write-Host "  1. Open Chrome/Edge and navigate to: $url" -ForegroundColor White
    Write-Host "  2. Verify lock icon appears (Secure)" -ForegroundColor White
    Write-Host "  3. Press F12 → Console → Check for mixed content warnings" -ForegroundColor White
    Write-Host "  4. Press F12 → Network → Filter by 'http://' → Should be empty" -ForegroundColor White
    Write-Host "  5. Press F12 → Security → Verify 'This page is secure'" -ForegroundColor White
} elseif ($failed -gt 0) {
    Write-Host "✗ OVERALL: Issues detected - review failures above" -ForegroundColor Red
    Write-Host ""
    Write-Host "Troubleshooting:" -ForegroundColor Cyan
    Write-Host "  1. Verify Cloudflare Tunnel is running: cloudflared tunnel info gfc-webapp" -ForegroundColor White
    Write-Host "  2. Check IIS is running on port 8080" -ForegroundColor White
    Write-Host "  3. Review Cloudflare SSL/TLS settings (should be 'Full')" -ForegroundColor White
    Write-Host "  4. Check DNS propagation: nslookup gfc.lovanow.com" -ForegroundColor White
} else {
    Write-Host "⚠ OVERALL: Partial success - review warnings above" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "For detailed testing, see: docs/hosting/CLOUDFLARE_TUNNEL_HTTPS_FIX.md" -ForegroundColor Gray
Write-Host "===========================================================" -ForegroundColor Cyan
