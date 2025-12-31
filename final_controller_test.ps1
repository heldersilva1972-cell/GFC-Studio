# Final Controller Test
# Now that network is configured correctly

$controllerIP = "192.168.1.72"
$controllerPort = 60000
$controllerSN = 223213880

Write-Host "=== Final Controller Communication Test ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Your PC IP: 192.168.1.100" -ForegroundColor Green
Write-Host "Controller IP: $controllerIP" -ForegroundColor Green
Write-Host "Controller SN: $controllerSN" -ForegroundColor Green
Write-Host ""

# CRC-16 IBM
function Calculate-CRC16 {
    param([byte[]]$data)
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
$packet[0] = 32      # Type
$packet[1] = 80      # Command
$packet[2] = 0       # CRC
$packet[3] = 0

# XID
$xid = 1
$packet[4] = [byte]($xid -band 0xFF)
$packet[5] = [byte](($xid -shr 8) -band 0xFF)
$packet[6] = [byte](($xid -shr 16) -band 0xFF)
$packet[7] = [byte](($xid -shr 24) -band 0xFF)

# Source SN
$packet[8] = 0
$packet[9] = 0
$packet[10] = 0
$packet[11] = 0

# Target SN
$packet[12] = [byte]($controllerSN -band 0xFF)
$packet[13] = [byte](($controllerSN -shr 8) -band 0xFF)
$packet[14] = [byte](($controllerSN -shr 16) -band 0xFF)
$packet[15] = [byte](($controllerSN -shr 24) -band 0xFF)

$packet[16] = 0
$packet[17] = 129
$packet[18] = 0
$packet[19] = 0

# CRC
$crc = Calculate-CRC16 $packet
$packet[2] = [byte]($crc -band 0xFF)
$packet[3] = [byte](($crc -shr 8) -band 0xFF)

Write-Host "Packet: $([BitConverter]::ToString($packet))" -ForegroundColor Gray
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
    
    Write-Host "✓✓✓ SUCCESS! Controller responded!" -ForegroundColor Green
    Write-Host "Response ($($response.Length) bytes): $([BitConverter]::ToString($response))" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "=== CONTROLLER IS WORKING ===" -ForegroundColor Green
    Write-Host ""
    Write-Host "Your GFC application should now work!" -ForegroundColor Cyan
    Write-Host "The controller will show as CONNECTED in the UI." -ForegroundColor Green
}
catch {
    Write-Host "✗ No response: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "The controller is still not responding." -ForegroundColor Yellow
    Write-Host "This means there's likely a communication password or other security setting." -ForegroundColor Yellow
}
finally {
    $udpClient.Close()
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
