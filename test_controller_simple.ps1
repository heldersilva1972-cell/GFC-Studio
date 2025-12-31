# WG3000 Controller Communication Test - Simple Version
# Tests UDP communication with the controller

$controllerIP = "192.168.1.72"
$controllerPort = 60000
$controllerSN = 223213880  # 0x0D4E8BB8

Write-Host "=== WG3000 Controller Test ===" -ForegroundColor Cyan
Write-Host "Controller: ${controllerIP}:${controllerPort}" -ForegroundColor Yellow
Write-Host "Serial Number: $controllerSN" -ForegroundColor Yellow
Write-Host ""

# Create UDP client
$udpClient = New-Object System.Net.Sockets.UdpClient
$udpClient.Client.ReceiveTimeout = 3000

try {
    # Bind to any local port
    $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $udpClient.Client.Bind($localEndpoint)
    $localPort = $udpClient.Client.LocalEndPoint.Port
    
    Write-Host "Local port: $localPort" -ForegroundColor Green
    Write-Host ""

    # Build WG3000 packet (20 byte header)
    $packet = New-Object byte[] 20
    
    # Packet structure:
    $packet[0] = 32      # Type
    $packet[1] = 80      # Command
    $packet[2] = 0       # CRC (calculated later)
    $packet[3] = 0
    
    # XID (4 bytes)
    $xid = 1
    $packet[4] = [byte]($xid -band 0xFF)
    $packet[5] = [byte](($xid -shr 8) -band 0xFF)
    $packet[6] = [byte](($xid -shr 16) -band 0xFF)
    $packet[7] = [byte](($xid -shr 24) -band 0xFF)
    
    # Source SN (0 for PC)
    $packet[8] = 0
    $packet[9] = 0
    $packet[10] = 0
    $packet[11] = 0
    
    # Target controller SN
    $packet[12] = [byte]($controllerSN -band 0xFF)
    $packet[13] = [byte](($controllerSN -shr 8) -band 0xFF)
    $packet[14] = [byte](($controllerSN -shr 16) -band 0xFF)
    $packet[15] = [byte](($controllerSN -shr 24) -band 0xFF)
    
    $packet[16] = 0      # iCallReturn
    $packet[17] = 129    # driverVer
    $packet[18] = 0      # protocol marker
    $packet[19] = 0      # reserved
    
    # Calculate CRC-16 IBM
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
    
    $crc = Calculate-CRC16 $packet
    $packet[2] = [byte]($crc -band 0xFF)
    $packet[3] = [byte](($crc -shr 8) -band 0xFF)
    
    Write-Host "Packet (hex): $([BitConverter]::ToString($packet))" -ForegroundColor Gray
    Write-Host ""
    
    # Send packet
    $remoteEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $controllerPort)
    $bytesSent = $udpClient.Send($packet, $packet.Length, $remoteEndpoint)
    Write-Host "Sent $bytesSent bytes" -ForegroundColor Green
    Write-Host ""
    
    # Receive response
    Write-Host "Waiting for response..." -ForegroundColor Cyan
    
    $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $response = $udpClient.Receive([ref]$responseEndpoint)
    
    Write-Host ""
    Write-Host "SUCCESS! Received $($response.Length) bytes from $($responseEndpoint.Address)" -ForegroundColor Green
    Write-Host "Response (hex): $([BitConverter]::ToString($response))" -ForegroundColor Yellow
    Write-Host ""
    
    # Parse response
    if ($response.Length -ge 20) {
        $respType = $response[0]
        $respCmd = $response[1]
        $respFromSN = [BitConverter]::ToUInt32($response, 8)
        
        Write-Host "Response Type: $respType, Command: $respCmd" -ForegroundColor Cyan
        Write-Host "From SN: $respFromSN (0x$($respFromSN.ToString('X8')))" -ForegroundColor Cyan
        
        if ($response.Length > 20) {
            $payload = $response[20..($response.Length-1)]
            Write-Host "Payload: $([BitConverter]::ToString($payload))" -ForegroundColor Yellow
            
            if ($payload.Length -ge 2) {
                Write-Host ""
                Write-Host "Door Status:" -ForegroundColor Cyan
                $doorByte = $payload[0]
                $relayByte = $payload[1]
                for ($i = 0; $i -lt 4; $i++) {
                    $doorOpen = (($doorByte -band (1 -shl $i)) -ne 0)
                    $relayOn = (($relayByte -band (1 -shl $i)) -ne 0)
                    $status = if ($doorOpen) { "OPEN" } else { "CLOSED" }
                    $relay = if ($relayOn) { "ON" } else { "OFF" }
                    $color = if ($doorOpen) { "Yellow" } else { "Green" }
                    Write-Host "  Door $($i+1): $status, Relay: $relay" -ForegroundColor $color
                }
            }
        }
    }
    
    Write-Host ""
    Write-Host "=== CONTROLLER IS ONLINE ===" -ForegroundColor Green
}
catch [System.Net.Sockets.SocketException] {
    Write-Host ""
    Write-Host "ERROR: No response (timeout)" -ForegroundColor Red
    Write-Host "Message: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Possible causes:" -ForegroundColor Yellow
    Write-Host "  1. Controller is offline" -ForegroundColor Gray
    Write-Host "  2. Firewall blocking port $controllerPort" -ForegroundColor Gray
    Write-Host "  3. Wrong serial number" -ForegroundColor Gray
    Write-Host "  4. Network issue" -ForegroundColor Gray
}
catch {
    Write-Host ""
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
}
finally {
    $udpClient.Close()
    Write-Host ""
    Write-Host "Test complete." -ForegroundColor Gray
}
