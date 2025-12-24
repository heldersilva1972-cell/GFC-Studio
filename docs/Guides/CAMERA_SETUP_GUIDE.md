# GFC Camera System Setup Guide

## Overview
The GFC Camera System uses a two-tier architecture to stream IP cameras to your web browser.

## Architecture
```
[IP Cameras/NVR] → [Video Agent Service] → [GFC Web App] → [Browser]
```

## Prerequisites

### 1. FFmpeg Installation
The Video Agent requires FFmpeg to convert RTSP streams to HLS format.

**Download FFmpeg:**
- Windows: https://www.gyan.dev/ffmpeg/builds/
- Download the "ffmpeg-release-essentials.zip"
- Extract to `C:\ffmpeg`
- Add `C:\ffmpeg\bin` to your Windows PATH

**Verify Installation:**
```cmd
ffmpeg -version
```

### 2. Camera/NVR Requirements
- IP cameras or NVR accessible on your network
- RTSP stream support
- Username and password for authentication
- Network connectivity between server and cameras

## Configuration Steps

### Step 1: Configure Video Agent

Edit `apps\services\GFC.VideoAgent\appsettings.json`:

```json
{
  "VideoAgent": {
    "OutputDirectory": "C:\\temp\\hls-streams",
    "ListenPort": 5101,
    "FFmpegPath": "ffmpeg",
    "AllowedOrigins": [
      "https://localhost:5000",
      "https://localhost:5001"
    ]
  },
  "NvrSettings": {
    "IpAddress": "192.168.1.64",      // Your NVR or camera IP
    "RtspPort": 554,                   // Standard RTSP port
    "Username": "admin",               // Camera/NVR username
    "Password": "your_password",       // Camera/NVR password
    "Cameras": [
      {
        "Id": 1,
        "Name": "Main Entrance",
        "RtspPath": "/cam/realmonitor?channel=1&subtype=0",
        "Enabled": true
      }
    ]
  }
}
```

### Step 2: Find Your Camera Information

#### Option A: Using an NVR (Recommended)

1. **Find NVR IP Address:**
   - Check your router's connected devices list
   - Look for device named after your NVR brand (Dahua, Hikvision, etc.)

2. **Access NVR Web Interface:**
   - Open browser: `http://[NVR_IP_ADDRESS]`
   - Login with admin credentials

3. **Common NVR RTSP Formats:**

**Dahua NVR:**
```
rtsp://admin:password@192.168.1.64:554/cam/realmonitor?channel=1&subtype=0
```
- Channel 1 = `/cam/realmonitor?channel=1&subtype=0`
- Channel 2 = `/cam/realmonitor?channel=2&subtype=0`
- `subtype=0` = Main stream (high quality)
- `subtype=1` = Sub stream (lower quality)

**Hikvision NVR:**
```
rtsp://admin:password@192.168.1.64:554/Streaming/Channels/101
```
- Channel 1 Main = `/Streaming/Channels/101`
- Channel 1 Sub = `/Streaming/Channels/102`
- Channel 2 Main = `/Streaming/Channels/201`

**Amcrest NVR:**
```
rtsp://admin:password@192.168.1.64:554/cam/realmonitor?channel=1&subtype=1
```

#### Option B: Individual IP Cameras

1. **Find Camera IP:**
   - Use network scanner: [Angry IP Scanner](https://angryip.org/)
   - Check router DHCP client list
   - Use manufacturer's discovery tool

2. **Test RTSP URL with VLC:**
   - Open VLC Media Player
   - Media → Open Network Stream
   - Enter: `rtsp://username:password@camera_ip:554/stream_path`
   - Click Play

3. **Common Camera RTSP Formats:**

**Hikvision:**
```
rtsp://admin:password@192.168.1.100:554/Streaming/Channels/101
```

**Dahua:**
```
rtsp://admin:password@192.168.1.100:554/cam/realmonitor?channel=1&subtype=0
```

**Reolink:**
```
rtsp://admin:password@192.168.1.100:554/h264Preview_01_main
```

**Amcrest:**
```
rtsp://admin:password@192.168.1.100:554/cam/realmonitor?channel=1&subtype=1
```

**Axis:**
```
rtsp://root:password@192.168.1.100/axis-media/media.amp
```

### Step 3: Test RTSP Connection

Before adding to GFC, test your RTSP URL:

1. **Install VLC Media Player** (if not installed)
2. Open VLC → Media → Open Network Stream
3. Paste your full RTSP URL
4. If video plays, your URL is correct! ✅

### Step 4: Add Camera to GFC System

1. **Start Video Agent Service:**
   ```cmd
   cd apps\services\GFC.VideoAgent
   dotnet run
   ```

2. **Start GFC Web App:**
   ```cmd
   cd apps\webapp\GFC.BlazorServer
   dotnet run
   ```

3. **Add Camera via Web Interface:**
   - Navigate to: `https://localhost:5001/cameras`
   - Click "Add Camera"
   - Fill in:
     - **Name**: Descriptive name (e.g., "Main Entrance")
     - **RTSP URL**: Full RTSP URL including credentials
     - **Enabled**: Check the box
   - Click "Save"

4. **View Camera Grid:**
   - Navigate to: `https://localhost:5001/cameras/grid`
   - Your camera should appear in the grid

## Testing Without Real Cameras

If you don't have cameras yet, you can test with public streams:

### Big Buck Bunny Test Stream
```
rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4
```

**Add to appsettings.json:**
```json
"Cameras": [
  {
    "Id": 1,
    "Name": "Test Stream - Big Buck Bunny",
    "RtspPath": "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4",
    "Enabled": true
  }
]
```

**Or add via web interface:**
- Name: `Test Stream`
- RTSP URL: `rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4`

## Troubleshooting

### Camera Not Loading
1. **Check FFmpeg:**
   ```cmd
   ffmpeg -version
   ```
   If error, FFmpeg is not installed or not in PATH

2. **Check Video Agent is Running:**
   - Should be listening on port 5101
   - Check console for errors

3. **Test RTSP URL in VLC:**
   - If VLC can't play it, GFC won't either
   - Verify credentials, IP address, and path

4. **Check Network Connectivity:**
   ```cmd
   ping [camera_ip]
   ```

5. **Check Firewall:**
   - Ensure port 554 (RTSP) is not blocked
   - Ensure port 5101 (Video Agent) is not blocked

### "Loading cameras..." Forever
- This means no cameras in database
- Add cameras via Camera Management page
- Or check database connection

### Stream Starts Then Stops
- Check FFmpeg logs in Video Agent console
- May be authentication issue
- May be network bandwidth issue

## Security Notes

⚠️ **Important Security Considerations:**

1. **Never commit passwords to Git:**
   - Use environment variables for production
   - Keep `appsettings.json` in `.gitignore`

2. **Change default camera passwords:**
   - Default passwords are security risks
   - Use strong, unique passwords

3. **Network Security:**
   - Keep cameras on isolated VLAN if possible
   - Don't expose RTSP ports to internet
   - Use VPN for remote access

4. **HTTPS Only:**
   - Always use HTTPS in production
   - Never send credentials over HTTP

## Next Steps

1. ✅ Install FFmpeg
2. ✅ Find your camera/NVR IP address
3. ✅ Test RTSP URL in VLC
4. ✅ Update Video Agent configuration
5. ✅ Start Video Agent service
6. ✅ Add cameras via web interface
7. ✅ View camera grid

## Support

For issues:
1. Check Video Agent console logs
2. Check browser console (F12)
3. Verify RTSP URL works in VLC
4. Check network connectivity
