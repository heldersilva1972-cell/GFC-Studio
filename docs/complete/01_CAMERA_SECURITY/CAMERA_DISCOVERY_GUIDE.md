# Camera Auto-Discovery Feature

## Overview
The Camera Auto-Discovery feature automatically scans your network to find IP cameras and allows you to add them to the system with just a few clicks - no manual RTSP URL configuration needed!

## How It Works

### Discovery Methods
The system uses multiple methods to find cameras on your network:

1. **ONVIF WS-Discovery** (Recommended)
   - Sends multicast probe messages to discover ONVIF-compatible cameras
   - Automatically retrieves camera name, manufacturer, model, and capabilities
   - Works with most modern IP cameras

2. **Port Scanning**
   - Scans common RTSP ports (554, 8554, 10554)
   - Identifies devices with open RTSP services
   - Fallback method for non-ONVIF cameras

3. **Manufacturer Detection**
   - Attempts to identify camera brand (Hikvision, Dahua, Amcrest, etc.)
   - Provides default credentials for common manufacturers
   - Suggests appropriate stream paths

## Using the Discovery Feature

### Step 1: Navigate to Discovery Page
- Go to **Controllers** → **Discover Cameras** in the navigation menu
- Or navigate directly to `/cameras/discover`

### Step 2: Configure Scan Settings
- **Network Range**: Enter CIDR notation (e.g., `192.168.1.0/24`)
  - Leave empty to auto-detect your local subnet
  - `/24` scans 254 addresses (192.168.1.1 to 192.168.1.254)
- **Timeout**: Set scan duration (10-120 seconds)
  - Longer timeouts find more cameras but take more time
  - Recommended: 30 seconds

### Step 3: Start Discovery
- Click **"Start Discovery"** button
- Wait for scan to complete (progress indicator shown)
- System will display all found cameras

### Step 4: Review Discovered Cameras
Each discovered camera shows:
- **IP Address**: Network location
- **Manufacturer**: Brand (if detected)
- **Model**: Camera model number (if available)
- **Discovery Method**: How it was found (ONVIF or Port Scan)
- **Available Streams**: List of stream profiles (Main, Sub, etc.)
- **Status**: Whether already added to system

### Step 5: Add Camera to System
1. Click **"Add to System"** on desired camera
2. Modal dialog opens with:
   - **Camera Name**: Edit or keep discovered name
   - **Stream Profile**: Select quality (Main/Sub stream)
   - **Username**: Enter camera credentials
   - **Password**: Enter camera password
   - **Enable Immediately**: Check to activate camera
3. Click **"Add Camera"**
4. Camera is added to database and ready to use!

## Features

### Smart Duplicate Detection
- Cameras already in the system are marked "Already Added"
- Prevents accidental duplicates
- Shows which cameras are new vs. existing

### Manufacturer-Specific Defaults
The system knows common default credentials for:
- **Hikvision**: admin/12345, admin/admin
- **Dahua**: admin/admin, admin/admin123
- **Amcrest**: admin/admin
- **Axis**: root/pass, root/root
- **Reolink**: admin/admin

### Stream Profile Selection
For supported manufacturers, the system automatically provides:
- **Main Stream**: High quality (1920x1080)
- **Sub Stream**: Lower quality (640x480) for bandwidth savings

### Automatic RTSP URL Construction
The system builds the correct RTSP URL based on:
- Camera IP address
- Selected stream profile
- Manufacturer-specific path format
- Your credentials

## Network Requirements

### Firewall Rules
Ensure the following ports are open:
- **UDP 3702**: ONVIF WS-Discovery
- **TCP 554**: RTSP streaming
- **TCP 80/8080**: HTTP camera web interface

### Network Configuration
- Server and cameras must be on same network or routable subnet
- Multicast must be enabled for ONVIF discovery
- No VLAN isolation between server and cameras

## Troubleshooting

### No Cameras Found
**Possible Causes:**
- Cameras are powered off
- Cameras on different subnet
- Firewall blocking discovery traffic
- ONVIF disabled on cameras

**Solutions:**
1. Verify cameras are powered on and connected
2. Check network range setting
3. Increase timeout to 60 seconds
4. Try manual IP scan if ONVIF fails
5. Enable ONVIF in camera settings

### Camera Found But Won't Add
**Possible Causes:**
- Incorrect credentials
- RTSP port blocked
- Camera firmware outdated

**Solutions:**
1. Verify username/password
2. Try default credentials for manufacturer
3. Test RTSP URL in VLC Media Player
4. Update camera firmware
5. Check camera's RTSP settings

### Discovery Takes Too Long
**Solutions:**
- Reduce network range (scan fewer IPs)
- Decrease timeout value
- Scan during off-peak hours
- Use specific IP range instead of full subnet

## Technical Details

### Supported Protocols
- **ONVIF Profile S**: Network Video Transmitter
- **RTSP/RTP**: Real-Time Streaming Protocol
- **HTTP**: Web interface access

### Manufacturer-Specific RTSP Paths

**Hikvision:**
```
Main: /Streaming/Channels/101
Sub:  /Streaming/Channels/102
```

**Dahua:**
```
Main: /cam/realmonitor?channel=1&subtype=0
Sub:  /cam/realmonitor?channel=1&subtype=1
```

**Amcrest:**
```
Main: /cam/realmonitor?channel=1&subtype=1
Sub:  /cam/realmonitor?channel=1&subtype=2
```

**Reolink:**
```
Main: /h264Preview_01_main
Sub:  /h264Preview_01_sub
```

### Security Considerations
- Credentials are stored encrypted in database
- Discovery uses read-only network scanning
- No camera settings are modified during discovery
- Audit log entry created when camera is added

## Best Practices

1. **Scan During Setup**
   - Run discovery when first setting up camera system
   - Re-run when adding new cameras to network

2. **Use Descriptive Names**
   - Rename cameras with location-based names
   - Examples: "Main Entrance", "Parking Lot", "Back Door"

3. **Choose Appropriate Stream**
   - Main stream for recording and high-quality viewing
   - Sub stream for live monitoring to save bandwidth

4. **Change Default Passwords**
   - Always change camera default passwords
   - Use strong, unique passwords for each camera

5. **Regular Re-Discovery**
   - Periodically scan to find new cameras
   - Verify existing cameras are still accessible

## Integration with Camera System

Once added via discovery, cameras are:
- Available in **Camera Grid** for multi-view
- Accessible in **Live View** for individual viewing
- Listed in **Camera Management** for configuration
- Included in **Recording Management** for playback
- Tracked in **Camera Audit Log** for security

## Future Enhancements

Planned improvements:
- Bulk add multiple cameras at once
- Automatic credential testing
- Camera health monitoring
- Scheduled re-discovery
- Network topology mapping
- Bandwidth estimation
