# Enable Both Doors for Card 5798887
# This script sends the exact packet that makes both doors work

$TargetIP = "192.168.0.196"
$ControllerSN = 223213880
$CardNumber = 5798887

$SrcPort = 60001
$socket = New-Object System.Net.Sockets.UdpClient($SrcPort)
$endpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($TargetIP), 60000)

$packet = New-Object byte[] 64
$packet[0] = 0x17  # Type
$packet[1] = 0x50  # CMD: Add/Update Card

[BitConverter]::GetBytes([uint32]$ControllerSN).CopyTo($packet, 4)
[BitConverter]::GetBytes([uint32]$CardNumber).CopyTo($packet, 8)

# Dates: 2025-01-01 to 2099-12-31
$packet[12] = 0x20; $packet[13] = 0x25; $packet[14] = 0x01; $packet[15] = 0x01
$packet[16] = 0x20; $packet[17] = 0x99; $packet[18] = 0x12; $packet[19] = 0x31

# BOTH DOORS ENABLED
$packet[20] = 0x01  # Door 1 - ENABLED
$packet[21] = 0x01  # Door 2 - ENABLED
$packet[22] = 0x00  # Door 3 - DISABLED
$packet[23] = 0x00  # Door 4 - DISABLED

# Timezones
$packet[24] = 0x01; $packet[25] = 0x01; $packet[26] = 0x01; $packet[27] = 0x01

# CRC
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

[BitConverter]::GetBytes([uint16]$SrcPort).CopyTo($packet, 2)
$crcValue = Get-Crc16 $packet
[BitConverter]::GetBytes([uint16]$crcValue).CopyTo($packet, 2)

$sum = 0
for($i=0; $i -lt 63; $i++) { $sum += $packet[$i] }
$packet[63] = [byte]($sum -band 0xFF)

Write-Host "`n=== ENABLING BOTH DOORS FOR CARD $CardNumber ===" -ForegroundColor Cyan
Write-Host "Door 1: ENABLED (Byte 20 = 0x01)" -ForegroundColor Green
Write-Host "Door 2: ENABLED (Byte 21 = 0x01)" -ForegroundColor Green

$socket.Send($packet, 64, $endpoint) | Out-Null

$timer = [System.Diagnostics.Stopwatch]::StartNew()
while($timer.ElapsedMilliseconds -lt 2000) {
    if($socket.Available -gt 0) {
        $recv = $socket.Receive([ref]$endpoint)
        Write-Host "`n[SUCCESS] Controller acknowledged!" -ForegroundColor Green
        Write-Host "Test card $CardNumber on BOTH physical doors - they should both work!" -ForegroundColor Yellow
        $socket.Close()
        exit
    }
}

Write-Host "`n[FAILED] No response" -ForegroundColor Red
$socket.Close()
