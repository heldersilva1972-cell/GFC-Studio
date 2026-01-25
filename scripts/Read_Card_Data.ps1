# Read Card Data from Controller
# Command 0x5C - Read card privileges

$TargetIP = "192.168.0.196"
$ControllerSN = 223213880
$CardNumber = 5798887

$SrcPort = 60001
$socket = New-Object System.Net.Sockets.UdpClient($SrcPort)
$endpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($TargetIP), 60000)

$packet = New-Object byte[] 64
$packet[0] = 0x17  # Type
$packet[1] = 0x5C  # CMD: Read Card (92 decimal)

[BitConverter]::GetBytes([uint32]$ControllerSN).CopyTo($packet, 4)
[BitConverter]::GetBytes([uint32]$CardNumber).CopyTo($packet, 8)

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

Write-Host "`nREADING CARD $CardNumber FROM CONTROLLER..." -ForegroundColor Cyan
$socket.Send($packet, 64, $endpoint) | Out-Null

$timer = [System.Diagnostics.Stopwatch]::StartNew()
while($timer.ElapsedMilliseconds -lt 2000) {
    if($socket.Available -gt 0) {
        $recv = $socket.Receive([ref]$endpoint)
        Write-Host "[SUCCESS] Got response!" -ForegroundColor Green
        
        Write-Host "`nResponse bytes 20-23 (Door Access):" -ForegroundColor Yellow
        Write-Host "  Byte 20 (Door 1): 0x$($recv[20].ToString('X2')) $(if($recv[20] -eq 1){'✓ ENABLED'}else{'✗ DISABLED'})" -ForegroundColor $(if($recv[20] -eq 1){'Green'}else{'Red'})
        Write-Host "  Byte 21 (Door 2): 0x$($recv[21].ToString('X2')) $(if($recv[21] -eq 1){'✓ ENABLED'}else{'✗ DISABLED'})" -ForegroundColor $(if($recv[21] -eq 1){'Green'}else{'Red'})
        Write-Host "  Byte 22 (Door 3): 0x$($recv[22].ToString('X2')) $(if($recv[22] -eq 1){'✓ ENABLED'}else{'✗ DISABLED'})" -ForegroundColor $(if($recv[22] -eq 1){'Green'}else{'Red'})
        Write-Host "  Byte 23 (Door 4): 0x$($recv[23].ToString('X2')) $(if($recv[23] -eq 1){'✓ ENABLED'}else{'✗ DISABLED'})" -ForegroundColor $(if($recv[23] -eq 1){'Green'}else{'Red'})
        
        Write-Host "`nFull response packet:" -ForegroundColor White
        Write-Host ([BitConverter]::ToString($recv))
        
        $socket.Close()
        exit
    }
}

Write-Host "[FAILED] No response" -ForegroundColor Red
$socket.Close()
