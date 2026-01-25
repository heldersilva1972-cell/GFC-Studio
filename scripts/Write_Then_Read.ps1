# Write then Read Card - Immediate verification
$TargetIP = "192.168.0.196"
$ControllerSN = 223213880
$CardNumber = 5798887
$SrcPort = 60001

Write-Host "=== STEP 1: WRITING CARD WITH ALL DOORS ENABLED ===" -ForegroundColor Cyan

$socket = New-Object System.Net.Sockets.UdpClient($SrcPort)
$endpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($TargetIP), 60000)

$packet = New-Object byte[] 64
$packet[0] = 0x17
$packet[1] = 0x50  # Add/Update

[BitConverter]::GetBytes([uint32]$ControllerSN).CopyTo($packet, 4)
[BitConverter]::GetBytes([uint32]$CardNumber).CopyTo($packet, 8)

# Dates
$packet[12] = 0x20; $packet[13] = 0x25; $packet[14] = 0x01; $packet[15] = 0x01
$packet[16] = 0x20; $packet[17] = 0x99; $packet[18] = 0x12; $packet[19] = 0x31

# ALL DOORS
$packet[20] = 0x01; $packet[21] = 0x01; $packet[22] = 0x01; $packet[23] = 0x01
$packet[24] = 0x01; $packet[25] = 0x01; $packet[26] = 0x01; $packet[27] = 0x01

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

Write-Host "Sending packet with bytes 20-23 = 01 01 01 01..." -ForegroundColor Yellow
$socket.Send($packet, 64, $endpoint) | Out-Null

Start-Sleep -Milliseconds 500
$socket.Close()

Write-Host "`n=== STEP 2: READING BACK WHAT WAS STORED ===" -ForegroundColor Cyan
Start-Sleep -Milliseconds 500

$socket = New-Object System.Net.Sockets.UdpClient($SrcPort)
$packet2 = New-Object byte[] 64
$packet2[0] = 0x17
$packet2[1] = 0x5C  # Read Card

[BitConverter]::GetBytes([uint32]$ControllerSN).CopyTo($packet2, 4)
[BitConverter]::GetBytes([uint32]$CardNumber).CopyTo($packet2, 8)

[BitConverter]::GetBytes([uint16]$SrcPort).CopyTo($packet2, 2)
$crcValue2 = Get-Crc16 $packet2
[BitConverter]::GetBytes([uint16]$crcValue2).CopyTo($packet2, 2)

$sum2 = 0
for($i=0; $i -lt 63; $i++) { $sum2 += $packet2[$i] }
$packet2[63] = [byte]($sum2 -band 0xFF)

$socket.Send($packet2, 64, $endpoint) | Out-Null

$timer = [System.Diagnostics.Stopwatch]::StartNew()
while($timer.ElapsedMilliseconds -lt 2000) {
    if($socket.Available -gt 0) {
        $recv = $socket.Receive([ref]$endpoint)
        Write-Host "Controller response:" -ForegroundColor Green
        Write-Host "Full packet: $([BitConverter]::ToString($recv))" -ForegroundColor White
        
        Write-Host "`nChecking ALL byte positions for 0x01:" -ForegroundColor Yellow
        for($i=0; $i -lt 64; $i++) {
            if($recv[$i] -eq 0x01) {
                Write-Host "  Byte $i = 0x01 *** FOUND" -ForegroundColor Green
            }
        }
        
        $socket.Close()
        exit
    }
}

Write-Host "No response" -ForegroundColor Red
$socket.Close()
