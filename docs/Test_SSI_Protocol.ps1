# SSI Protocol Verification Test
# ---------------------------------------------------------
# UPDATE THESE TWO VALUES FIRST:
$TargetIP = "192.168.0.196" # <--- YOUR PROVIDED IP
$ControllerSN = 223213880  # <--- From appsettings.json, updating if needed

$SrcPort = 60000
$socket = New-Object System.Net.Sockets.UdpClient($SrcPort)
$endpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($TargetIP), 60000)

# 1. Build 64-Byte Frame
$packet = New-Object byte[] 64
$packet[0] = 0x17 # Type
$packet[1] = 0x5A # CMD: Get Door Params

# 2. SALT CRC with SrcPort (Correction 2)
[BitConverter]::GetBytes([uint16]$SrcPort).CopyTo($packet, 2)

# 3. Headers (Correction 1: Shifted Offsets)
[BitConverter]::GetBytes([uint32]1).CopyTo($packet, 4)  # Transaction ID
[BitConverter]::GetBytes([uint32]0).CopyTo($packet, 8)  # Source SN
[BitConverter]::GetBytes([uint32]$ControllerSN).CopyTo($packet, 12) # Dest SN

# 4. Payload (Correction 3: Safety Bytes at Offset 16)
$packet[16] = 0x01 # Door Index 1
$packet[17] = 0x55 
$packet[18] = 0xAA
$packet[19] = 0x55
$packet[20] = 0xAA

# 5. Calculate CRC16 (Salted)
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
$crcValue = Get-Crc16 $packet
[BitConverter]::GetBytes([uint16]$crcValue).CopyTo($packet, 2)

# 6. Calculate Tail Sum (Mandatory final byte)
$sum = 0
for($i=0; $i -lt 63; $i++) { $sum += $packet[$i] }
$packet[63] = [byte]($sum -band 0xFF)

# SEND
Write-Host "SENDING SSI PACKET TO $TargetIP (SN: $ControllerSN)..." -ForegroundColor Cyan
$socket.Send($packet, 64, $endpoint)

# LISTEN
$timer = [System.Diagnostics.Stopwatch]::StartNew()
$responded = $false
while($timer.ElapsedMilliseconds -lt 2500) {
    if($socket.Available -gt 0) {
        $recv = $socket.Receive([ref]$endpoint)
        $responded = $true
        Write-Host "`n[!!!] RESPONSE RECEIVED [!!!]" -ForegroundColor Green
        Write-Host "RAW DATA: " -NoNewline
        Write-Host ([System.BitConverter]::ToString($recv)) -ForegroundColor Gray
        
        # Check the NEW offset 17 for the Timer
        $hwTimer = $recv[17]
        Write-Host "Extracted Hardware Timer: $hwTimer seconds" -ForegroundColor Yellow
        break
    }
}

if (-not $responded) {
    Write-Host "`n[X] NO RESPONSE FROM CONTROLLER." -ForegroundColor Red
    Write-Host "Check your IP ($TargetIP) and Serial Number ($ControllerSN)."
}

$socket.Close()
