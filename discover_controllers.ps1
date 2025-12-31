# WG3000 Controller Discovery
# Finds all controllers on the network without needing serial number

Write-Host "=== WG3000 Controller Discovery ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Searching for controllers on the network..." -ForegroundColor Yellow
Write-Host ""

# CRC-16 IBM function
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

# Build discovery packet (broadcast)
$packet = New-Object byte[] 20

# Discovery/Search command: Type 23, Command 148
$packet[0] = 23      # Type
$packet[1] = 148     # Command (Search/Discovery)
$packet[2] = 0       # CRC placeholder
$packet[3] = 0

# XID
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

# Target SN (0 for broadcast)
$packet[12] = 0
$packet[13] = 0
$packet[14] = 0
$packet[15] = 0

$packet[16] = 0    # iCallReturn
$packet[17] = 129  # driverVer
$packet[18] = 0    # protocol marker
$packet[19] = 0    # reserved

# Calculate CRC
$crc = Calculate-CRC16 $packet
$packet[2] = [byte]($crc -band 0xFF)
$packet[3] = [byte](($crc -shr 8) -band 0xFF)

Write-Host "Discovery packet: $([BitConverter]::ToString($packet))" -ForegroundColor DarkGray
Write-Host ""

# Create UDP client
$udpClient = New-Object System.Net.Sockets.UdpClient
$udpClient.Client.ReceiveTimeout = 5000  # 5 second timeout
$udpClient.EnableBroadcast = $true

try {
    # Bind to any local port
    $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $udpClient.Client.Bind($localEndpoint)
    
    Write-Host "Sending broadcast discovery..." -ForegroundColor Cyan
    
    # Send to broadcast address
    $broadcastEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Broadcast, 60000)
    $bytesSent = $udpClient.Send($packet, $packet.Length, $broadcastEndpoint)
    
    Write-Host "Sent $bytesSent bytes to broadcast (255.255.255.255:60000)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Waiting for responses..." -ForegroundColor Yellow
    Write-Host ""
    
    $foundControllers = @()
    $startTime = Get-Date
    
    # Listen for responses for 5 seconds
    while (((Get-Date) - $startTime).TotalSeconds -lt 5) {
        try {
            $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
            $response = $udpClient.Receive([ref]$responseEndpoint)
            
            if ($response.Length -ge 20) {
                # Parse response
                $respType = $response[0]
                $respCmd = $response[1]
                $respSN = [BitConverter]::ToUInt32($response, 8)
                $respIP = $responseEndpoint.Address.ToString()
                
                Write-Host "✓ Found controller!" -ForegroundColor Green
                Write-Host "  IP Address: $respIP" -ForegroundColor Cyan
                Write-Host "  Serial Number: $respSN (0x$($respSN.ToString('X8')))" -ForegroundColor Cyan
                Write-Host "  Response Type: $respType, Command: $respCmd" -ForegroundColor Gray
                Write-Host "  Raw: $([BitConverter]::ToString($response[0..19]))" -ForegroundColor DarkGray
                Write-Host ""
                
                $foundControllers += @{
                    IP = $respIP
                    SerialNumber = $respSN
                    Response = $response
                }
            }
        }
        catch [System.Net.Sockets.SocketException] {
            # Timeout - no more responses
            break
        }
    }
    
    if ($foundControllers.Count -eq 0) {
        Write-Host "✗ No controllers found" -ForegroundColor Red
        Write-Host ""
        Write-Host "Possible reasons:" -ForegroundColor Yellow
        Write-Host "  1. Controller is not powered on" -ForegroundColor Gray
        Write-Host "  2. Controller is on a different network/subnet" -ForegroundColor Gray
        Write-Host "  3. Firewall is blocking UDP port 60000" -ForegroundColor Gray
        Write-Host "  4. Network cable is not connected" -ForegroundColor Gray
    } else {
        Write-Host "=== Discovery Complete ===" -ForegroundColor Green
        Write-Host ""
        Write-Host "Found $($foundControllers.Count) controller(s)" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "Update your configuration:" -ForegroundColor Yellow
        foreach ($ctrl in $foundControllers) {
            Write-Host "  Serial Number: $($ctrl.SerialNumber)" -ForegroundColor White
            Write-Host "  IP Address: $($ctrl.IP)" -ForegroundColor White
            Write-Host ""
        }
        
        # Save to file
        $configFile = "discovered_controllers.txt"
        $foundControllers | ForEach-Object {
            "SerialNumber: $($_.SerialNumber)`r`nIPAddress: $($_.IP)`r`n---" | Out-File -FilePath $configFile -Append
        }
        Write-Host "Configuration saved to: $configFile" -ForegroundColor Gray
    }
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
finally {
    $udpClient.Close()
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
