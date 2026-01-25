# N3000 Card Add - BITMASK METHOD
# Based on the specification: Door mask is a single byte bitmask

$TargetIP = "192.168.0.196"
$ControllerSN = 223213880  # 0x0D4DF938
$CardNumber = 5798887

$SrcPort = 60001
$socket = New-Object System.Net.Sockets.UdpClient($SrcPort)
$endpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($TargetIP), 60000)

# Allocate 64 bytes, all zeros
$packet = New-Object byte[] 64

# Header
$packet[0] = 0x17  # PktType
$packet[1] = 0x50  # Func: Add/Update Card

# Serial Number [4..7] = 38 F9 4D 0D (little-endian)
$packet[4] = 0x38
$packet[5] = 0xF9
$packet[6] = 0x4D
$packet[7] = 0x0D

# Payload starts at byte 8
# Card Number [8..11] (little-endian)
[BitConverter]::GetBytes([uint32]$CardNumber).CopyTo($packet, 8)

# Door Mask [12] - BITMASK METHOD
# Door 1 = 0x01, Door 2 = 0x02, Door 3 = 0x04, Door 4 = 0x08
# Both doors = 0x01 | 0x02 = 0x03
$packet[12] = 0x03  # Door 1 + Door 2

# CRC16-IBM
function Get-Crc16-IBM {
    param([byte[]]$buffer)
    $crc = 0xFFFF  # Init to 0xFFFF per spec
    foreach ($b in $buffer) {
        $crc = $crc -bxor $b
        for ($i = 0; $i -lt 8; $i++) {
            if ($crc -band 1) { $crc = ($crc -shr 1) -bxor 0xA001 }
            else { $crc = $crc -shr 1 }
        }
    }
    return $crc
}

# Set CRC bytes to 0 temporarily
$packet[2] = 0
$packet[3] = 0

# Compute CRC over all 64 bytes
$crcValue = Get-Crc16-IBM $packet

# Write CRC to [2..3] little-endian
[BitConverter]::GetBytes([uint16]$crcValue).CopyTo($packet, 2)

# Compute Tail checksum
$sum = 0
for($i=0; $i -lt 63; $i++) { $sum += $packet[$i] }
$packet[63] = [byte]($sum -band 0xFF)

Write-Host "`n=== SENDING CARD WITH BITMASK METHOD ===" -ForegroundColor Cyan
Write-Host "Card Number: $CardNumber" -ForegroundColor Yellow
Write-Host "Door Mask: 0x03 (Door 1 + Door 2)" -ForegroundColor Green
Write-Host "`nPacket Header:" -ForegroundColor White
Write-Host "  [0] Type: 0x$($packet[0].ToString('X2'))"
Write-Host "  [1] Func: 0x$($packet[1].ToString('X2'))"
Write-Host "  [2-3] CRC: 0x$($packet[3].ToString('X2'))$($packet[2].ToString('X2'))"
Write-Host "  [4-7] SN: $($packet[4].ToString('X2')) $($packet[5].ToString('X2')) $($packet[6].ToString('X2')) $($packet[7].ToString('X2'))"
Write-Host "`nPayload:" -ForegroundColor White
Write-Host "  [8-11] Card: $($packet[8].ToString('X2')) $($packet[9].ToString('X2')) $($packet[10].ToString('X2')) $($packet[11].ToString('X2'))"
Write-Host "  [12] Door Mask: 0x$($packet[12].ToString('X2'))" -ForegroundColor Yellow

$socket.Send($packet, 64, $endpoint) | Out-Null

$timer = [System.Diagnostics.Stopwatch]::StartNew()
while($timer.ElapsedMilliseconds -lt 2000) {
    if($socket.Available -gt 0) {
        $recv = $socket.Receive([ref]$endpoint)
        Write-Host "`n[SUCCESS] Controller acknowledged!" -ForegroundColor Green
        Write-Host "Test card $CardNumber on BOTH doors!" -ForegroundColor Yellow
        $socket.Close()
        exit
    }
}

Write-Host "`n[FAILED] No response" -ForegroundColor Red
$socket.Close()
