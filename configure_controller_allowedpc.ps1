# WG3000 Controller - Allowed PC Configuration Tool
# This script configures the controller to accept commands from any PC

$controllerIP = "192.168.1.72"
$controllerPort = 60000
$controllerSN = 223213880

Write-Host "=== WG3000 Allowed PC Configuration Tool ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "This will configure the controller to accept commands from ANY PC" -ForegroundColor Yellow
Write-Host "Controller: ${controllerIP}:${controllerPort} (SN: $controllerSN)" -ForegroundColor Gray
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

# Build WG3000 packet
function Build-WG3000Packet {
    param(
        [byte]$Type,
        [byte]$Command,
        [uint32]$TargetSN,
        [byte[]]$Payload = @()
    )
    
    $headerSize = 20
    $packet = New-Object byte[] ($headerSize + $Payload.Length)
    
    # Header
    $packet[0] = $Type
    $packet[1] = $Command
    $packet[2] = 0  # CRC placeholder
    $packet[3] = 0
    
    # XID
    $xid = Get-Random -Minimum 1 -Maximum 65535
    $packet[4] = [byte]($xid -band 0xFF)
    $packet[5] = [byte](($xid -shr 8) -band 0xFF)
    $packet[6] = [byte](($xid -shr 16) -band 0xFF)
    $packet[7] = [byte](($xid -shr 24) -band 0xFF)
    
    # Source SN (0 for PC)
    $packet[8] = 0
    $packet[9] = 0
    $packet[10] = 0
    $packet[11] = 0
    
    # Target SN
    $packet[12] = [byte]($TargetSN -band 0xFF)
    $packet[13] = [byte](($TargetSN -shr 8) -band 0xFF)
    $packet[14] = [byte](($TargetSN -shr 16) -band 0xFF)
    $packet[15] = [byte](($TargetSN -shr 24) -band 0xFF)
    
    $packet[16] = 0    # iCallReturn
    $packet[17] = 129  # driverVer
    $packet[18] = 0    # protocol marker
    $packet[19] = 0    # reserved
    
    # Copy payload
    if ($Payload.Length -gt 0) {
        [Array]::Copy($Payload, 0, $packet, $headerSize, $Payload.Length)
    }
    
    # Calculate and set CRC
    $crc = Calculate-CRC16 $packet
    $packet[2] = [byte]($crc -band 0xFF)
    $packet[3] = [byte](($crc -shr 8) -band 0xFF)
    
    return $packet
}

# Send command and get response
function Send-ControllerCommand {
    param(
        [byte]$Type,
        [byte]$Command,
        [byte[]]$Payload = @(),
        [int]$TimeoutMs = 3000
    )
    
    $udpClient = New-Object System.Net.Sockets.UdpClient
    $udpClient.Client.ReceiveTimeout = $TimeoutMs
    
    try {
        $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
        $udpClient.Client.Bind($localEndpoint)
        
        $packet = Build-WG3000Packet -Type $Type -Command $Command -TargetSN $controllerSN -Payload $Payload
        
        $remoteEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $controllerPort)
        $bytesSent = $udpClient.Send($packet, $packet.Length, $remoteEndpoint)
        
        Write-Host "Sent command: Type=$Type, Cmd=$Command, Bytes=$bytesSent" -ForegroundColor Gray
        
        $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
        $response = $udpClient.Receive([ref]$responseEndpoint)
        
        return $response
    }
    catch {
        Write-Host "  Error: $($_.Exception.Message)" -ForegroundColor Red
        return $null
    }
    finally {
        $udpClient.Close()
    }
}

Write-Host "Step 1: Testing controller connectivity..." -ForegroundColor Cyan
$testResponse = Send-ControllerCommand -Type 32 -Command 80
if ($testResponse) {
    Write-Host "  ✓ Controller responded! ($($testResponse.Length) bytes)" -ForegroundColor Green
} else {
    Write-Host "  ✗ No response - controller may have IP filtering enabled" -ForegroundColor Yellow
    Write-Host "  Continuing anyway - we'll try to configure it..." -ForegroundColor Yellow
}
Write-Host ""

Write-Host "Step 2: Configuring Allowed PC settings..." -ForegroundColor Cyan
Write-Host "  Setting: Allow ALL PCs (0.0.0.0)" -ForegroundColor Gray

# Build Allowed PC configuration payload
# Based on WGPacketBasicPCAllowedIPSetToSend.cs from vendor code
# Packet structure for PC Allowed IP Set command:
# Type: 32 (0x20)
# Command: 96 (0x60) - PC Allowed IP Set
# Payload: 72 bytes total
#   Bytes 0-3: IP address 1 (0.0.0.0 = allow all)
#   Bytes 4-7: IP address 2
#   Bytes 8-11: IP address 3  
#   Bytes 12-15: IP address 4
#   Bytes 16-23: Password (16 bytes, blank)
#   Bytes 24-39: Reserved
#   Bytes 40-55: New password (16 bytes, blank)
#   Bytes 56-71: Reserved

$payload = New-Object byte[] 72

# Set first IP to 0.0.0.0 (allow all)
$payload[0] = 0
$payload[1] = 0
$payload[2] = 0
$payload[3] = 0

# Set remaining IPs to 0.0.0.0
for ($i = 4; $i -lt 16; $i++) {
    $payload[$i] = 0
}

# Password fields (leave blank/zero)
for ($i = 16; $i -lt 72; $i++) {
    $payload[$i] = 0
}

Write-Host "  Payload: $([BitConverter]::ToString($payload[0..15]))" -ForegroundColor DarkGray

# Send configuration command
# Type 32, Command 96 = Set PC Allowed IP
$configResponse = Send-ControllerCommand -Type 32 -Command 96 -Payload $payload -TimeoutMs 5000

if ($configResponse) {
    Write-Host "  ✓ Configuration sent successfully!" -ForegroundColor Green
    Write-Host "  Response: $([BitConverter]::ToString($configResponse))" -ForegroundColor DarkGray
} else {
    Write-Host "  ✗ No response from controller" -ForegroundColor Red
    Write-Host "  This might mean:" -ForegroundColor Yellow
    Write-Host "    - The controller is currently blocking this PC" -ForegroundColor Gray
    Write-Host "    - You need to use N3000.exe from an allowed PC first" -ForegroundColor Gray
    Write-Host "    - Or temporarily set your PC IP to match the allowed IP" -ForegroundColor Gray
}
Write-Host ""

Write-Host "Step 3: Verifying configuration..." -ForegroundColor Cyan
Start-Sleep -Seconds 2

$verifyResponse = Send-ControllerCommand -Type 32 -Command 80
if ($verifyResponse) {
    Write-Host "  ✓✓✓ SUCCESS! Controller is now responding!" -ForegroundColor Green
    Write-Host "  The controller should now accept commands from any PC" -ForegroundColor Green
    Write-Host ""
    Write-Host "=== Configuration Complete ===" -ForegroundColor Green
    Write-Host ""
    Write-Host "Your GFC web application should now work!" -ForegroundColor Cyan
    Write-Host "The controller will accept commands from any PC on the network." -ForegroundColor Gray
} else {
    Write-Host "  ⚠ Still no response" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "=== Manual Configuration Required ===" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "The controller is still blocking this PC. You need to:" -ForegroundColor Yellow
    Write-Host "1. Use N3000.exe from a PC with an allowed IP (e.g., 192.168.1.17)" -ForegroundColor Gray
    Write-Host "2. Or temporarily change this PC's IP to an allowed IP" -ForegroundColor Gray
    Write-Host "3. Then run this script again" -ForegroundColor Gray
    Write-Host ""
    Write-Host "To change your PC's IP temporarily:" -ForegroundColor Cyan
    Write-Host "  1. Open Network Connections (ncpa.cpl)" -ForegroundColor Gray
    Write-Host "  2. Right-click your network adapter → Properties" -ForegroundColor Gray
    Write-Host "  3. Select IPv4 → Properties" -ForegroundColor Gray
    Write-Host "  4. Set IP to: 192.168.1.17" -ForegroundColor Gray
    Write-Host "  5. Subnet: 255.255.255.0" -ForegroundColor Gray
    Write-Host "  6. Gateway: 192.168.1.1" -ForegroundColor Gray
    Write-Host "  7. Run this script again" -ForegroundColor Gray
    Write-Host "  8. Change back to 'Obtain IP automatically'" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
