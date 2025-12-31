# WG3000 Controller Communication Test
# This script sends a properly formatted GetRunStatus command to the controller

$controllerIP = "192.168.1.72"
$controllerPort = 60000
$controllerSN = 223213880  # 0x0D4E8BB8

Write-Host "=== WG3000 Controller Communication Test ===" -ForegroundColor Cyan
Write-Host "Controller: $controllerIP:$controllerPort (SN: $controllerSN)" -ForegroundColor Yellow
Write-Host ""

# Create UDP client
$udpClient = New-Object System.Net.Sockets.UdpClient
$udpClient.Client.ReceiveTimeout = 3000  # 3 second timeout

try {
    # Bind to any local port
    $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $udpClient.Client.Bind($localEndpoint)
    $localPort = $udpClient.Client.LocalEndPoint.Port
    
    Write-Host "Local UDP port: $localPort" -ForegroundColor Green
    Write-Host ""

    # Build WG3000 packet matching vendor structure (WGPacket.cs)
    # Header is 20 bytes total
    $packet = New-Object byte[] 20
    
    # Packet structure (matching WGPacket.cs lines 298-313):
    $packet[0] = 32      # Type 32 (0x20) - GetRunStatus
    $packet[1] = 80      # Command 80 (0x50) - GetRunStatus
    # Bytes 2-3: Will be CRC (set to 0 for now)
    $packet[2] = 0
    $packet[3] = 0
    
    # Bytes 4-7: XID (transaction ID) - 4 bytes
    $xid = 1
    $packet[4] = [byte]($xid -band 0xFF)
    $packet[5] = [byte](($xid -shr 8) -band 0xFF)
    $packet[6] = [byte](($xid -shr 16) -band 0xFF)
    $packet[7] = [byte](($xid -shr 24) -band 0xFF)
    
    # Bytes 8-11: iDevSnFrom (source SN) - 0 for PC
    $packet[8] = 0
    $packet[9] = 0
    $packet[10] = 0
    $packet[11] = 0
    
    # Bytes 12-15: iDevSnTo (target controller SN)
    $packet[12] = [byte]($controllerSN -band 0xFF)
    $packet[13] = [byte](($controllerSN -shr 8) -band 0xFF)
    $packet[14] = [byte](($controllerSN -shr 16) -band 0xFF)
    $packet[15] = [byte](($controllerSN -shr 24) -band 0xFF)
    
    # Byte 16: iCallReturn
    $packet[16] = 0
    
    # Byte 17: driverVer
    $packet[17] = 129  # 0x81
    
    # Byte 18: reserved18 (protocol marker)
    $packet[18] = 0
    
    # Byte 19: reserved19
    $packet[19] = 0
    
    # Calculate CRC-16 IBM over entire packet
    # Simple CRC-16 IBM implementation
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
    
    Write-Host "Packet Structure:" -ForegroundColor Cyan
    Write-Host "  Type: $($packet[0]) (0x$($packet[0].ToString('X2')))" -ForegroundColor Gray
    Write-Host "  Command: $($packet[1]) (0x$($packet[1].ToString('X2')))" -ForegroundColor Gray
    Write-Host "  CRC: 0x$($packet[3].ToString('X2'))$($packet[2].ToString('X2'))" -ForegroundColor Gray
    Write-Host "  XID: $xid" -ForegroundColor Gray
    Write-Host "  Target SN: $controllerSN (0x$($controllerSN.ToString('X8')))" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Raw packet (hex):" -ForegroundColor Gray
    Write-Host "  $([BitConverter]::ToString($packet))" -ForegroundColor DarkGray
    Write-Host ""
    
    # Send packet
    $remoteEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $controllerPort)
    $bytesSent = $udpClient.Send($packet, $packet.Length, $remoteEndpoint)
    Write-Host "✓ Sent $bytesSent bytes to $controllerIP:$controllerPort" -ForegroundColor Green
    Write-Host ""
    
    # Try to receive response
    Write-Host "Waiting for response (3 second timeout)..." -ForegroundColor Cyan
    
    $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $response = $udpClient.Receive([ref]$responseEndpoint)
    
    Write-Host ""
    Write-Host "✓✓✓ SUCCESS! Received response from $($responseEndpoint.Address):$($responseEndpoint.Port)" -ForegroundColor Green
    Write-Host "Response length: $($response.Length) bytes" -ForegroundColor Green
    Write-Host ""
    Write-Host "Response (hex):" -ForegroundColor Cyan
    Write-Host "  $([BitConverter]::ToString($response))" -ForegroundColor Yellow
    Write-Host ""
    
    # Parse basic response info
    if ($response.Length -ge 20) {
        $respType = $response[0]
        $respCmd = $response[1]
        $respCrc = [BitConverter]::ToUInt16($response, 2)
        $respXid = [BitConverter]::ToUInt32($response, 4)
        $respFromSN = [BitConverter]::ToUInt32($response, 8)
        $respToSN = [BitConverter]::ToUInt32($response, 12)
        
        Write-Host "Response Details:" -ForegroundColor Cyan
        Write-Host "  Type: $respType (0x$($respType.ToString('X2')))" -ForegroundColor Gray
        Write-Host "  Command: $respCmd (0x$($respCmd.ToString('X2')))" -ForegroundColor Gray
        Write-Host "  CRC: 0x$($respCrc.ToString('X4'))" -ForegroundColor Gray
        Write-Host "  XID: $respXid" -ForegroundColor Gray
        Write-Host "  From SN: $respFromSN (0x$($respFromSN.ToString('X8')))" -ForegroundColor Gray
        Write-Host "  To SN: $respToSN (0x$($respToSN.ToString('X8')))" -ForegroundColor Gray
        
        if ($response.Length > 20) {
            Write-Host ""
            Write-Host "Payload ($($response.Length - 20) bytes):" -ForegroundColor Cyan
            $payload = $response[20..($response.Length-1)]
            Write-Host "  $([BitConverter]::ToString($payload))" -ForegroundColor Yellow
            
            # Parse door status if this is a run status response
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
                    Write-Host "  Door $($i+1): $status, Relay: $relay" -ForegroundColor $(if ($doorOpen) { "Yellow" } else { "Green" })
                }
            }
        }
    }
    
    Write-Host ""
    Write-Host "=== CONTROLLER IS ONLINE AND RESPONDING ===" -ForegroundColor Green
}
catch [System.Net.Sockets.SocketException] {
    Write-Host ""
    Write-Host "✗ ERROR: No response received (timeout)" -ForegroundColor Red
    Write-Host "Socket Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Possible causes:" -ForegroundColor Yellow
    Write-Host "  1. Controller is offline or unreachable" -ForegroundColor Gray
    Write-Host "  2. Firewall is blocking UDP port $controllerPort" -ForegroundColor Gray
    Write-Host "  3. Wrong serial number: $controllerSN" -ForegroundColor Gray
    Write-Host "  4. Packet format is incorrect" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Try running Wireshark to see if packets are being sent/received" -ForegroundColor Yellow
}
catch {
    Write-Host ""
    Write-Host "✗ ERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host $_.Exception.GetType().FullName -ForegroundColor DarkRed
}
finally {
    $udpClient.Close()
    Write-Host ""
    Write-Host "UDP client closed." -ForegroundColor Gray
}
