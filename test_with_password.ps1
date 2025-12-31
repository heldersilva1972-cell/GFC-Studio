# Controller Test with Password
# Tests both "abc123" and "123" passwords

$controllerIP = "192.168.1.72"
$controllerPort = 60000
$controllerSN = 223213880

Write-Host "=== Controller Test with Password ===" -ForegroundColor Cyan
Write-Host ""

# Load n3k_comm.dll for encryption
$dllPath = "C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\ACTUAL CONTROLLER SOFTWARE\AccessControl.en\n3k_comm.dll"

if (Test-Path $dllPath) {
    Write-Host "Loading encryption library..." -ForegroundColor Yellow
    
    # Define P/Invoke signatures
    $signature = @"
    [DllImport("$dllPath", CallingConvention = CallingConvention.Cdecl)]
    public static extern int enc(byte[] pwgPkt, int len, byte[] k);
"@
    
    try {
        Add-Type -MemberDefinition $signature -Name "N3kComm" -Namespace "WG3000"
        Write-Host "✓ Encryption library loaded" -ForegroundColor Green
    } catch {
        Write-Host "⚠ Could not load encryption: $($_.Exception.Message)" -ForegroundColor Yellow
    }
} else {
    Write-Host "⚠ n3k_comm.dll not found at: $dllPath" -ForegroundColor Yellow
}

Write-Host ""

# CRC-16 IBM
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

# Test with password
function Test-WithPassword {
    param([string]$password)
    
    Write-Host "Testing with password: '$password'" -ForegroundColor Cyan
    
    # Build packet
    $packet = New-Object byte[] 20
    $packet[0] = 32
    $packet[1] = 80
    $packet[2] = 0
    $packet[3] = 0
    
    $xid = 1
    $packet[4] = [byte]($xid -band 0xFF)
    $packet[5] = [byte](($xid -shr 8) -band 0xFF)
    $packet[6] = [byte](($xid -shr 16) -band 0xFF)
    $packet[7] = [byte](($xid -shr 24) -band 0xFF)
    
    $packet[8] = 0
    $packet[9] = 0
    $packet[10] = 0
    $packet[11] = 0
    
    $packet[12] = [byte]($controllerSN -band 0xFF)
    $packet[13] = [byte](($controllerSN -shr 8) -band 0xFF)
    $packet[14] = [byte](($controllerSN -shr 16) -band 0xFF)
    $packet[15] = [byte](($controllerSN -shr 24) -band 0xFF)
    
    $packet[16] = 0
    $packet[17] = 129
    $packet[18] = 0
    $packet[19] = 0
    
    # CRC
    $crc = Calculate-CRC16 $packet
    $packet[2] = [byte]($crc -band 0xFF)
    $packet[3] = [byte](($crc -shr 8) -band 0xFF)
    
    # Encrypt if we have the library
    if ([WG3000.N3kComm] -and $password) {
        $passBytes = New-Object byte[] 16
        $passChars = $password.PadRight(16, "`0").ToCharArray()
        for ($i = 0; $i -lt 16; $i++) {
            $passBytes[$i] = [byte]$passChars[$i]
        }
        
        [WG3000.N3kComm]::enc($packet, $packet.Length, $passBytes) | Out-Null
        Write-Host "  Packet encrypted" -ForegroundColor Gray
    }
    
    Write-Host "  Packet: $([BitConverter]::ToString($packet[0..19]))" -ForegroundColor DarkGray
    
    # Send
    $udpClient = New-Object System.Net.Sockets.UdpClient
    $udpClient.Client.ReceiveTimeout = 3000
    
    try {
        $localEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
        $udpClient.Client.Bind($localEndpoint)
        
        $remoteEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($controllerIP), $controllerPort)
        $bytesSent = $udpClient.Send($packet, $packet.Length, $remoteEndpoint)
        
        Write-Host "  Sent $bytesSent bytes" -ForegroundColor Gray
        
        $responseEndpoint = New-Object System.Net.IPEndPoint([System.Net.IPAddress]::Any, 0)
        $response = $udpClient.Receive([ref]$responseEndpoint)
        
        Write-Host "  ✓✓✓ SUCCESS! Response received!" -ForegroundColor Green
        Write-Host "  Response: $([BitConverter]::ToString($response[0..19]))" -ForegroundColor Cyan
        Write-Host ""
        return $true
    }
    catch {
        Write-Host "  ✗ No response" -ForegroundColor Red
        Write-Host ""
        return $false
    }
    finally {
        $udpClient.Close()
    }
}

# Try different passwords
$passwords = @("abc123", "123", "abc", "", "admin", "password")

foreach ($pwd in $passwords) {
    $result = Test-WithPassword -password $pwd
    if ($result) {
        Write-Host "=== FOUND WORKING PASSWORD: '$pwd' ===" -ForegroundColor Green
        Write-Host ""
        Write-Host "Update your application configuration:" -ForegroundColor Cyan
        Write-Host "  Communication Password: $pwd" -ForegroundColor Yellow
        break
    }
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
