$controllerIP = "192.168.0.196"
$localIP      = "192.168.0.72"
$port         = 60000
$serialNumber = 223213880 

# Packet Configuration
$type = 0x17
$cmd  = 0x50 # 80 - Add/Update Card (IP64)

$packet = New-Object byte[] 64
$packet[0] = $type
$packet[1] = $cmd

# SN at offset 4
$snBytes = [BitConverter]::GetBytes([uint32]$serialNumber)
[Array]::Copy($snBytes, 0, $packet, 4, 4)

# --- Construct MjRegisterCard 24-byte (approx) structure ---
# We need to simulate the 24 bytes starting from CardID
# MjRegisterCard.ToBytes() layout relative to start of "24 byte" block (Start=Offset 4 of MjRegisterCard):
# 0-7: CardID (8 bytes)
# 8: Option
# 9-11: Password (3 bytes)
# 12-13: Start YMD
# 14-15: End YMD
# 16-19: Door Control (4 bytes)
# 20-21: More Cards
# 22-23: Ext/MaxSwipe

$cardStruct = New-Object byte[] 24

# Card ID (1001)
$cardNo = 1001
$cardBytes = [BitConverter]::GetBytes([uint64]$cardNo)
[Array]::Copy($cardBytes, 0, $cardStruct, 0, 8)

# Option (NotDeleted = 0x80 usually? IsActivated=0 means Active?)
# MjRegisterCard.cs: Activate=64, NotDeleted=128. 
# Default active valid: NotDeleted(128) | Reserved5(32). 
# Unset Activate bit means Activated? IsActivated Property: return (option & Activate) == 0.
# So we want Activate bit OFF.
# Just use 0xA0 (128 + 32)
$cardStruct[8] = 0xA0 

# Password (0, 0, 0)
$cardStruct[9] = 0
$cardStruct[10] = 0
$cardStruct[11] = 0

# Start Date (2024-01-01) - Compressed YMD
# wgTools.MsDateToWgDateYMD: 
# (Y-2000)<<9 | M<<5 | D
$year = 2024
$month = 1
$day = 1
$startVal = (($year - 2000) -shl 9) -bor ($month -shl 5) -bor $day
$startBytes = [BitConverter]::GetBytes([uint16]$startVal)
[Array]::Copy($startBytes, 0, $cardStruct, 12, 2)

# End Date (2029-12-31)
$year = 2029
$month = 12
$day = 31
$endVal = (($year - 2000) -shl 9) -bor ($month -shl 5) -bor $day
$endBytes = [BitConverter]::GetBytes([uint16]$endVal)
[Array]::Copy($endBytes, 0, $cardStruct, 14, 2)

# Door Control (All 4 doors allowed = 1)
$cardStruct[16] = 1
$cardStruct[17] = 1
$cardStruct[18] = 1
$cardStruct[19] = 1

# Reset others
$cardStruct[20] = 0
$cardStruct[21] = 0
$cardStruct[22] = 0 # MaxSwipe L
$cardStruct[23] = 0 # MaxSwipe H

# --- Fill Packet ---

# 1. Copy Full 24 bytes to Packet Offset 8
[Array]::Copy($cardStruct, 0, $packet, 8, 24)

# 2. XID at Packet Offset 40
$xid = 100
$xidBytes = [BitConverter]::GetBytes([uint32]$xid)
[Array]::Copy($xidBytes, 0, $packet, 40, 4)

# 3. Copy Partial 16 bytes (Skipping CardID) to Packet Offset 48
# Source offset 8 (Option) length 16
[Array]::Copy($cardStruct, 8, $packet, 48, 16)

# CRC-16
function Get-CRC16 {
    param([byte[]]$data)
    $crc = 0
    foreach ($b in $data[0,1 + 4..63]) {
        $crc = $crc -bxor $b
        for ($i = 0; $i -lt 8; $i++) {
            if (($crc -band 1) -ne 0) { $crc = ($crc -shr 1) -bxor 0xA001 }
            else { $crc = $crc -shr 1 }
        }
    }
    return $crc
}

$crcValue = Get-CRC16 $packet
$packet[2] = [byte]($crcValue -band 0xFF)
$packet[3] = [byte](($crcValue -shr 8) -band 0xFF)

Write-Host "Sending Add Card 0x50 ($([BitConverter]::ToString($packet)))" -ForegroundColor Yellow

$socket = New-Object System.Net.Sockets.Socket([System.Net.Sockets.AddressFamily]::InterNetwork, [System.Net.Sockets.SocketType]::Dgram, [System.Net.Sockets.ProtocolType]::Udp)
$socket.ReceiveTimeout = 5000

try {
    $socket.Bind((New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($localIP), 0)))
    $socket.SendTo($packet, (New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $port)))
    
    $buffer = New-Object byte[] 1024
    $ep = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
    $len = $socket.ReceiveFrom($buffer, [ref]$ep)
    
    Write-Host "RESPONSE RECEIVED! ($len bytes)" -ForegroundColor Green
    Write-Host "Bytes: $([BitConverter]::ToString($buffer[0..($len-1)]))" -ForegroundColor Cyan
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
finally { $socket.Close() }
