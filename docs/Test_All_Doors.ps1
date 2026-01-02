# Test All Doors - N3000 Controller
# This sends a test card with ALL doors enabled to see which physical door responds

$TargetIP = "192.168.0.196"
$ControllerSN = 223213880
$TestCardNumber = 5798887  # Actual card number to test

$SrcPort = 60001  # Use different port to avoid conflict
$socket = New-Object System.Net.Sockets.UdpClient($SrcPort)
$endpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($TargetIP), 60000)

# Build 64-Byte Frame
$packet = New-Object byte[] 64
$packet[0] = 0x17  # Type
$packet[1] = 0x50  # CMD: Add/Update Card (80 decimal)

# Controller SN at offset 4-7
[BitConverter]::GetBytes([uint32]$ControllerSN).CopyTo($packet, 4)

# Payload starts at offset 8
# Card Number (4 bytes) at offset 8-11
[BitConverter]::GetBytes([uint32]$TestCardNumber).CopyTo($packet, 8)

# Start Date (4 bytes BCD) at offset 12-15: 2025-01-01
$packet[12] = 0x20  # Century
$packet[13] = 0x25  # Year
$packet[14] = 0x01  # Month
$packet[15] = 0x01  # Day

# End Date (4 bytes BCD) at offset 16-19: 2099-12-31
$packet[16] = 0x20  # Century
$packet[17] = 0x99  # Year
$packet[18] = 0x12  # Month
$packet[19] = 0x31  # Day

# Door Access (Bytes 20-23) - ENABLE ALL DOORS
$packet[20] = 0x01  # Door 1 - ENABLED
$packet[21] = 0x01  # Door 2 - ENABLED
$packet[22] = 0x01  # Door 3 - ENABLED
$packet[23] = 0x01  # Door 4 - ENABLED

# Timezone Indices (Bytes 24-27) - Always allowed
$packet[24] = 0x01  # Door 1 timezone
$packet[25] = 0x01  # Door 2 timezone
$packet[26] = 0x01  # Door 3 timezone
$packet[27] = 0x01  # Door 4 timezone

# Calculate CRC16 (Salted with source port)
function Get-Crc16 {
    param([byte[]]$buffer)
    $crc = 0x0000
    foreach ($b in $buffer) {
        $crc = $crc -bxor $b
        for ($i = 0; $i -lt 8; $i++) {
            if ($crc -band 1) { $crc = ($crc -shr 1) -bxor 0xA001 }
            else { $crc = $crc -shr 1 }
        }
    }
    return $crc
}

# Salt CRC position with source port
[BitConverter]::GetBytes([uint16]$SrcPort).CopyTo($packet, 2)
$crcValue = Get-Crc16 $packet
[BitConverter]::GetBytes([uint16]$crcValue).CopyTo($packet, 2)

# Calculate Tail Sum
$sum = 0
for($i=0; $i -lt 63; $i++) { $sum += $packet[$i] }
$packet[63] = [byte]($sum -band 0xFF)

# Display packet for verification
Write-Host "`nSENDING TEST PACKET - ALL DOORS ENABLED" -ForegroundColor Cyan
Write-Host "Card Number: $TestCardNumber" -ForegroundColor Yellow
Write-Host "Doors Enabled: 1, 2, 3, 4 (ALL)" -ForegroundColor Green
Write-Host "`nPacket Bytes 20-23 (Door Access):" -ForegroundColor White
Write-Host "  Byte 20 (Door 1): 0x$($packet[20].ToString('X2'))" -ForegroundColor $(if($packet[20] -eq 1){'Green'}else{'Red'})
Write-Host "  Byte 21 (Door 2): 0x$($packet[21].ToString('X2'))" -ForegroundColor $(if($packet[21] -eq 1){'Green'}else{'Red'})
Write-Host "  Byte 22 (Door 3): 0x$($packet[22].ToString('X2'))" -ForegroundColor $(if($packet[22] -eq 1){'Green'}else{'Red'})
Write-Host "  Byte 23 (Door 4): 0x$($packet[23].ToString('X2'))" -ForegroundColor $(if($packet[23] -eq 1){'Green'}else{'Red'})

# SEND
$socket.Send($packet, 64, $endpoint) | Out-Null

# LISTEN for response
$timer = [System.Diagnostics.Stopwatch]::StartNew()
$responded = $false
while($timer.ElapsedMilliseconds -lt 2000) {
    if($socket.Available -gt 0) {
        $recv = $socket.Receive([ref]$endpoint)
        $responded = $true
        Write-Host "`n[SUCCESS] Controller Acknowledged!" -ForegroundColor Green
        break
    }
}

if (-not $responded) {
    Write-Host "`n[FAILED] No response from controller." -ForegroundColor Red
}

$socket.Close()

Write-Host "`n=== TEST INSTRUCTIONS ===" -ForegroundColor Cyan
Write-Host "1. Use test card #$TestCardNumber on each physical door reader"
Write-Host "2. Note which doors actually grant access"
Write-Host "3. This will tell us if there's a physical wiring mismatch"
Write-Host ""
