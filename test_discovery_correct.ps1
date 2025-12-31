# CORRECT Discovery/Search Command for WG3000
$controllerPort = 60000

Write-Host "=== WG3000 Controller Discovery (CORRECT METHOD) ===" -ForegroundColor Cyan
Write-Host ""

# CORRECTED CRC-16 IBM
function Calculate-CRC16-Corrected {
    param([byte[]]$data)
    
    if ($data.Length > 4) {
        $data[2] = 0
        $data[3] = 0
    }
    
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

# Build SEARCH packet (Type 36, Command 16)
$packet = New-Object byte[] 20
$packet[0] = 36      # Type = 36 (Search/Discovery)
$packet[1] = 16      # Command = 16 (Search)
$packet[2] = 0
$packet[3] = 0

$xid = 1
$packet[4] = [byte]($xid -band 0xFF)
$packet[5] = [byte](($xid -shr 8) -band 0xFF)
$packet[6] = [byte](($xid -shr 16) -band 0xFF)
$packet[7] = [byte](($xid -shr 24) -band 0xFF)

# Source SN = 0
$packet[8] = 0
$packet[9] = 0
$packet[10] = 0
$packet[11] = 0

# Target SN = 0xFFFFFFFF (BROADCAST!)
$packet[12] = 0xFF
$packet[13] = 0xFF
$packet[14] = 0xFF
$packet[15] = 0xFF

$packet[16] = 0
$packet[17] = 129
$packet[18] = 0
$packet[19] = 0

# Calculate CRC
$crc = Calculate-CRC16-Corrected $packet
$packet[2] = [byte]($crc -band 0xFF)
$packet[3] = [byte](($crc -shr 8) -band 0xFF)

Write-Host "SEARCH Packet (Type 36, Cmd 16, Broadcast):" -ForegroundColor Green
Write-Host "  $([BitConverter]::ToString($packet))" -ForegroundColor Yellow
Write-Host ""

# Send
$udpClient = New-Object System.Net.Sockets.UdpClient
$udpClient.Client.ReceiveTimeout = 5000
$udpClient.EnableBroadcast = $true

try {
    $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $udpClient.Client.Bind($localEndpoint)
    
    # Send to broadcast
    $broadcastEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Broadcast, $controllerPort)
    $bytesSent = $udpClient.Send($packet, $packet.Length, $broadcastEndpoint)
    
    Write-Host "Sent $bytesSent bytes to BROADCAST (255.255.255.255:60000)" -ForegroundColor Green
    Write-Host "Waiting for controller responses..." -ForegroundColor Yellow
    Write-Host ""
    
    $foundControllers = @()
    $startTime = Get-Date
    
    while (((Get-Date) - $startTime).TotalSeconds -lt 5) {
        try {
            $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
            $response = $udpClient.Receive([ref]$responseEndpoint)
            
            if ($response.Length -ge 20) {
                $respType = $response[0]
                $respCmd = $response[1]
                $respSN = [BitConverter]::ToUInt32($response, 8)
                $respIP = $responseEndpoint.Address.ToString()
                
                Write-Host "SUCCESS! Controller found!" -ForegroundColor Green
                Write-Host "  IP: $respIP" -ForegroundColor Cyan
                Write-Host "  Serial Number: $respSN (0x$($respSN.ToString('X8')))" -ForegroundColor Cyan
                Write-Host "  Response: $([BitConverter]::ToString($response[0..19]))" -ForegroundColor Yellow
                Write-Host ""
                
                $foundControllers += @{
                    IP = $respIP
                    SN = $respSN
                }
            }
        }
        catch {
            break
        }
    }
    
    if ($foundControllers.Count -eq 0) {
        Write-Host "No controllers responded" -ForegroundColor Red
    } else {
        Write-Host "=== DISCOVERY SUCCESSFUL ===" -ForegroundColor Green
        Write-Host ""
        Write-Host "Found $($foundControllers.Count) controller(s):" -ForegroundColor Cyan
        foreach ($ctrl in $foundControllers) {
            Write-Host "  IP: $($ctrl.IP), SN: $($ctrl.SN)" -ForegroundColor White
        }
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
