# Test with CORRECTED CRC calculation
$controllerIP = "192.168.1.72"
$controllerPort = 60000
$controllerSN = 223213880

Write-Host "=== Testing with CORRECTED CRC ===" -ForegroundColor Cyan
Write-Host ""

# CORRECTED CRC-16 IBM (matching vendor wgCRC.cs)
function Calculate-CRC16-Corrected {
    param([byte[]]$data)
    
    # CRITICAL: Zero out bytes 2-3 before calculating!
    if ($data.Length > 4) {
        $data[2] = 0
        $data[3] = 0
    }
    
    # Start at 0, not 0xFFFF!
    $crc = 0
    foreach ($b in $data) {
        $crc = $crc -bxor $b
        for ($i = 0; $i -lt 8; $i++) {
            if (($crc -band 1) -ne 0) {
                $crc = ($crc -shr 1) -bxor 0xA001
            } else {
                $crc = $crc -shr 1
            }
        }
    }
    return $crc
}

# Build packet
$packet = New-Object byte[] 20
$packet[0] = 32
$packet[1] = 80
$packet[2] = 0
$packet[3] = 0

$xid = 1
$packet[4] = [byte]($xid -band 0xFF)
$packet[5] = [byte](($xid -shr 8) -band 0xFF)
$packet[6] = [byte](($xid -shr 16) -band 0xFF)
$packet[7] = [byte](($xid -shr 24) -band 0xFF)

$packet[8] = 0
$packet[9] = 0
$packet[10] = 0
$packet[11] = 0

$packet[12] = [byte]($controllerSN -band 0xFF)
$packet[13] = [byte](($controllerSN -shr 8) -band 0xFF)
$packet[14] = [byte](($controllerSN -shr 16) -band 0xFF)
$packet[15] = [byte](($controllerSN -shr 24) -band 0xFF)

$packet[16] = 0
$packet[17] = 129
$packet[18] = 0
$packet[19] = 0

# Calculate CORRECTED CRC
$crc = Calculate-CRC16-Corrected $packet
$packet[2] = [byte]($crc -band 0xFF)
$packet[3] = [byte](($crc -shr 8) -band 0xFF)

Write-Host "Packet with CORRECTED CRC:" -ForegroundColor Green
Write-Host "  $([BitConverter]::ToString($packet))" -ForegroundColor Yellow
Write-Host ""

# Send
$udpClient = New-Object System.Net.Sockets.UdpClient
$udpClient.Client.ReceiveTimeout = 3000

try {
    $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $udpClient.Client.Bind($localEndpoint)
    
    $remoteEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $controllerPort)
    $bytesSent = $udpClient.Send($packet, $packet.Length, $remoteEndpoint)
    
    Write-Host "Sent $bytesSent bytes" -ForegroundColor Green
    Write-Host "Waiting for response..." -ForegroundColor Yellow
    Write-Host ""
    
    $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $response = $udpClient.Receive([ref]$responseEndpoint)
    
    Write-Host "SUCCESS! CONTROLLER RESPONDED!" -ForegroundColor Green
    $respLen = $response.Length
    Write-Host "Response: $respLen bytes" -ForegroundColor Cyan
    Write-Host "  $([BitConverter]::ToString($response))" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "=== THE CRC FIX WORKED! ===" -ForegroundColor Green
    Write-Host ""
    Write-Host "The controller is now communicating!" -ForegroundColor Cyan
    Write-Host "Your GFC application will work now!" -ForegroundColor Green
}
catch {
    Write-Host "✗ Still no response: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "If this still fails, there may be another issue." -ForegroundColor Yellow
}
finally {
    $udpClient.Close()
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
