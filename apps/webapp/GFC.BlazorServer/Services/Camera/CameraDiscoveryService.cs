// [NEW] - Camera Discovery Service Implementation
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services.Camera.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

using GFC.BlazorServer.Services;

namespace GFC.BlazorServer.Services.Camera
{
    /// <summary>
    /// Service for discovering IP cameras on the network using ONVIF, UPnP, and port scanning
    /// </summary>
    public class CameraDiscoveryService : ICameraDiscoveryService
    {
        private readonly GfcDbContext _context;
        private readonly ICameraService _cameraService;
        private readonly ISystemSettingsService _systemSettingsService;
        private readonly ILogger<CameraDiscoveryService> _logger;

        // Common RTSP ports to scan
        private readonly int[] _commonRtspPorts = { 554, 8554, 10554 };
        
        // Common HTTP ports for cameras
        private readonly int[] _commonHttpPorts = { 80, 8080, 8000, 8081 };

        public CameraDiscoveryService(
            GfcDbContext context,
            ICameraService cameraService,
            ISystemSettingsService systemSettingsService,
            ILogger<CameraDiscoveryService> logger)
        {
            _context = context;
            _cameraService = cameraService;
            _systemSettingsService = systemSettingsService;
            _logger = logger;
        }

        /// <summary>
        /// Discovers cameras on the network using multiple methods
        /// </summary>
        public async Task<List<DiscoveredCamera>> DiscoverCamerasAsync(string networkRange = null, int timeoutSeconds = 30, Action<string> onStatusUpdate = null)
        {
            var discoveredCameras = new List<DiscoveredCamera>();

            try
            {
                _logger.LogInformation("Starting camera discovery...");
                onStatusUpdate?.Invoke("Starting discovery process...");

                // Get existing cameras to mark duplicates
                var existingCameras = await _context.Cameras.ToListAsync();

                // Method 0: NVR Discovery
                onStatusUpdate?.Invoke("Checking configured NVR...");
                var nvrCameras = await this.DiscoverCamerasFromNvrAsync(_systemSettingsService, _logger, onStatusUpdate);
                if (nvrCameras.Any())
                {
                   onStatusUpdate?.Invoke($"Found {nvrCameras.Count} cameras from NVR.");
                }
                discoveredCameras.AddRange(nvrCameras);

                // Method 1: ONVIF WS-Discovery (multicast)
                onStatusUpdate?.Invoke("Scanning network for ONVIF devices...");
                var onvifCameras = await DiscoverOnvifCamerasAsync(timeoutSeconds);
                if (onvifCameras.Any())
                {
                   onStatusUpdate?.Invoke($"Found {onvifCameras.Count} ONVIF devices.");
                }
                discoveredCameras.AddRange(onvifCameras);

                // Method 2: Port scanning for RTSP services
                onStatusUpdate?.Invoke("Scanning network ports for RTSP streams...");
                var ipRange = networkRange ?? GetLocalSubnet();
                var scannedCameras = await ScanNetworkForCamerasAsync(ipRange, timeoutSeconds);
                
                // Merge results (avoid duplicates)
                foreach (var camera in scannedCameras)
                {
                    if (!discoveredCameras.Any(c => c.IpAddress == camera.IpAddress))
                    {
                        discoveredCameras.Add(camera);
                    }
                }

                // Mark cameras that are already added
                foreach (var camera in discoveredCameras)
                {
                    camera.IsAlreadyAdded = existingCameras.Any(c => 
                        c.RtspUrl != null && c.RtspUrl.Contains(camera.IpAddress));
                }

                _logger.LogInformation($"Discovery complete. Found {discoveredCameras.Count} cameras.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during camera discovery");
            }

            return discoveredCameras;
        }

        /// <summary>
        /// Discovers cameras using ONVIF WS-Discovery protocol
        /// </summary>
        private async Task<List<DiscoveredCamera>> DiscoverOnvifCamerasAsync(int timeoutSeconds)
        {
            var cameras = new List<DiscoveredCamera>();

            try
            {
                // ONVIF WS-Discovery multicast message
                var probeMessage = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" 
            xmlns:a=""http://schemas.xmlsoap.org/ws/2004/08/addressing"">
    <s:Header>
        <a:Action s:mustUnderstand=""1"">http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe</a:Action>
        <a:MessageID>uuid:" + Guid.NewGuid() + @"</a:MessageID>
        <a:ReplyTo>
            <a:Address>http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous</a:Address>
        </a:ReplyTo>
        <a:To s:mustUnderstand=""1"">urn:schemas-xmlsoap-org:ws:2005:04:discovery</a:To>
    </s:Header>
    <s:Body>
        <Probe xmlns=""http://schemas.xmlsoap.org/ws/2005/04/discovery"">
            <d:Types xmlns:d=""http://schemas.xmlsoap.org/ws/2005/04/discovery"" 
                     xmlns:dp0=""http://www.onvif.org/ver10/network/wsdl"">dp0:NetworkVideoTransmitter</d:Types>
        </Probe>
    </s:Body>
</s:Envelope>";

                using var udpClient = new UdpClient();
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                
                var multicastEndpoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 3702);
                var probeBytes = Encoding.UTF8.GetBytes(probeMessage);

                // Send probe
                await udpClient.SendAsync(probeBytes, probeBytes.Length, multicastEndpoint);

                // Listen for responses
                var endTime = DateTime.UtcNow.AddSeconds(timeoutSeconds);
                udpClient.Client.ReceiveTimeout = 1000; // 1 second timeout per receive

                while (DateTime.UtcNow < endTime)
                {
                    try
                    {
                        var receiveTask = udpClient.ReceiveAsync();
                        var delayTask = Task.Delay(1000); // 1 second timeout per receive attempt
                        
                        var completedTask = await Task.WhenAny(receiveTask, delayTask);
                        
                        if (completedTask == receiveTask)
                        {
                            var result = await receiveTask;
                            var response = Encoding.UTF8.GetString(result.Buffer);
                            
                            var camera = ParseOnvifResponse(response, result.RemoteEndPoint.Address.ToString());
                            if (camera != null && !cameras.Any(c => c.IpAddress == camera.IpAddress))
                            {
                                cameras.Add(camera);
                                _logger.LogInformation($"Discovered ONVIF camera: {camera.Name} at {camera.IpAddress}");
                            }
                        }
                        else
                        {
                            // Timeout for this receive attempt, loop will check total time
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error receiving ONVIF response");
                        break;
                    }
                }

                udpClient.Close();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ONVIF discovery failed, will try other methods");
            }

            return cameras;
        }

        /// <summary>
        /// Scans network range for devices with open RTSP ports
        /// </summary>
        private async Task<List<DiscoveredCamera>> ScanNetworkForCamerasAsync(string ipRange, int timeoutSeconds)
        {
            var cameras = new List<DiscoveredCamera>();
            var ipAddresses = GenerateIpRange(ipRange);

            _logger.LogInformation($"Scanning {ipAddresses.Count} IP addresses for cameras...");

            var tasks = ipAddresses.Select(async ip =>
            {
                try
                {
                    // Check if RTSP port is open
                    foreach (var port in _commonRtspPorts)
                    {
                        if (await IsPortOpenAsync(ip, port, 2))
                        {
                            var camera = new DiscoveredCamera
                            {
                                IpAddress = ip,
                                Port = port,
                                Name = $"Camera at {ip}",
                                DiscoveryMethod = "Port Scan",
                                SupportsOnvif = false,
                                RtspUrl = $"rtsp://{ip}:{port}"
                            };

                            // Try to identify manufacturer by banner grabbing
                            await TryIdentifyManufacturerAsync(camera);

                            return camera;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, $"Error scanning {ip}");
                }

                return null;
            });

            var results = await Task.WhenAll(tasks);
            cameras.AddRange(results.Where(c => c != null));

            return cameras;
        }

        /// <summary>
        /// Tests connection to a specific camera
        /// </summary>
        public async Task<DiscoveredCamera> TestCameraConnectionAsync(string ipAddress, string username, string password)
        {
            try
            {
                // Try common RTSP ports
                foreach (var port in _commonRtspPorts)
                {
                    if (await IsPortOpenAsync(ipAddress, port, 5))
                    {
                        var camera = new DiscoveredCamera
                        {
                            IpAddress = ipAddress,
                            Port = port,
                            Name = $"Camera at {ipAddress}",
                            DiscoveryMethod = "Manual Test",
                            RtspUrl = $"rtsp://{username}:{password}@{ipAddress}:{port}"
                        };

                        await TryIdentifyManufacturerAsync(camera);
                        await TryGetStreamProfilesAsync(camera, username, password);

                        return camera;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error testing camera at {ipAddress}");
            }

            return null;
        }

        /// <summary>
        /// Adds a discovered camera to the database
        /// </summary>
        public async Task<GFC.Core.Models.Camera> AddDiscoveredCameraAsync(AddDiscoveredCameraRequest request)
        {
            var rtspUrl = string.IsNullOrEmpty(request.SelectedStreamPath)
                ? $"rtsp://{request.Username}:{request.Password}@{request.IpAddress}:554"
                : $"rtsp://{request.Username}:{request.Password}@{request.IpAddress}:554{request.SelectedStreamPath}";

            var camera = new GFC.Core.Models.Camera
            {
                Name = string.IsNullOrEmpty(request.CustomName) 
                    ? $"Camera at {request.IpAddress}" 
                    : request.CustomName,
                RtspUrl = rtspUrl,
                IsEnabled = request.EnableImmediately,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _cameraService.CreateCameraAsync(camera);
        }

        /// <summary>
        /// Gets default credentials for common manufacturers
        /// </summary>
        public Dictionary<string, string> GetDefaultCredentials(string manufacturer)
        {
            var credentials = new Dictionary<string, string>();

            switch (manufacturer?.ToLower())
            {
                case "hikvision":
                    credentials["admin"] = "12345";
                    credentials["admin"] = "admin";
                    break;
                case "dahua":
                    credentials["admin"] = "admin";
                    credentials["admin"] = "admin123";
                    break;
                case "amcrest":
                    credentials["admin"] = "admin";
                    break;
                case "axis":
                    credentials["root"] = "pass";
                    credentials["root"] = "root";
                    break;
                case "reolink":
                    credentials["admin"] = "admin";
                    break;
                default:
                    credentials["admin"] = "admin";
                    credentials["admin"] = "12345";
                    credentials["root"] = "root";
                    break;
            }

            return credentials;
        }

        #region Helper Methods

        private DiscoveredCamera ParseOnvifResponse(string xml, string ipAddress)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                var ns = doc.Root?.GetDefaultNamespace();

                var camera = new DiscoveredCamera
                {
                    IpAddress = ipAddress,
                    Port = 80,
                    DiscoveryMethod = "ONVIF",
                    SupportsOnvif = true
                };

                // Try to extract device information from ONVIF response
                // This is simplified - real ONVIF parsing is more complex
                var scopes = doc.Descendants().Where(e => e.Name.LocalName == "Scopes").FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(scopes))
                {
                    // Parse manufacturer and model from scopes
                    if (scopes.Contains("hardware"))
                    {
                        var parts = scopes.Split('/');
                        camera.Manufacturer = parts.FirstOrDefault(p => p.Contains("manufacturer"))?.Split(':').LastOrDefault();
                        camera.Model = parts.FirstOrDefault(p => p.Contains("model"))?.Split(':').LastOrDefault();
                    }

                    if (scopes.Contains("name"))
                    {
                        camera.Name = scopes.Split('/').FirstOrDefault(p => p.Contains("name"))?.Split(':').LastOrDefault() 
                            ?? $"Camera at {ipAddress}";
                    }
                }

                return camera;
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> IsPortOpenAsync(string host, int port, int timeoutSeconds)
        {
            try
            {
                using var client = new TcpClient();
                var connectTask = client.ConnectAsync(host, port);
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                
                if (completedTask == connectTask && client.Connected)
                {
                    return true;
                }
            }
            catch
            {
                // Port is closed or unreachable
            }

            return false;
        }

        private async Task TryIdentifyManufacturerAsync(DiscoveredCamera camera)
        {
            try
            {
                // Try to identify by HTTP banner or RTSP response
                using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
                var response = await httpClient.GetAsync($"http://{camera.IpAddress}");
                var server = response.Headers.Server?.ToString();

                if (!string.IsNullOrEmpty(server))
                {
                    if (server.Contains("Hikvision", StringComparison.OrdinalIgnoreCase))
                        camera.Manufacturer = "Hikvision";
                    else if (server.Contains("Dahua", StringComparison.OrdinalIgnoreCase))
                        camera.Manufacturer = "Dahua";
                    else if (server.Contains("Amcrest", StringComparison.OrdinalIgnoreCase))
                        camera.Manufacturer = "Amcrest";
                    else if (server.Contains("Axis", StringComparison.OrdinalIgnoreCase))
                        camera.Manufacturer = "Axis";
                }
            }
            catch
            {
                // Unable to identify manufacturer
                camera.Manufacturer = "Unknown";
            }
        }

        private async Task TryGetStreamProfilesAsync(DiscoveredCamera camera, string username, string password)
        {
            // This would require ONVIF GetProfiles request
            // For now, add common stream paths based on manufacturer
            camera.AvailableStreams = new List<StreamProfile>();

            switch (camera.Manufacturer?.ToLower())
            {
                case "hikvision":
                    camera.AvailableStreams.Add(new StreamProfile
                    {
                        Name = "Main Stream",
                        RtspPath = "/Streaming/Channels/101",
                        Resolution = "1920x1080"
                    });
                    camera.AvailableStreams.Add(new StreamProfile
                    {
                        Name = "Sub Stream",
                        RtspPath = "/Streaming/Channels/102",
                        Resolution = "640x480"
                    });
                    break;

                case "dahua":
                    camera.AvailableStreams.Add(new StreamProfile
                    {
                        Name = "Main Stream",
                        RtspPath = "/cam/realmonitor?channel=1&subtype=0",
                        Resolution = "1920x1080"
                    });
                    camera.AvailableStreams.Add(new StreamProfile
                    {
                        Name = "Sub Stream",
                        RtspPath = "/cam/realmonitor?channel=1&subtype=1",
                        Resolution = "640x480"
                    });
                    break;

                default:
                    camera.AvailableStreams.Add(new StreamProfile
                    {
                        Name = "Default Stream",
                        RtspPath = "/stream1",
                        Resolution = "Unknown"
                    });
                    break;
            }

            await Task.CompletedTask;
        }

        private string GetLocalSubnet()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var localIp = host.AddressList.FirstOrDefault(ip => 
                    ip.AddressFamily == AddressFamily.InterNetwork && 
                    !IPAddress.IsLoopback(ip));

                if (localIp != null)
                {
                    var ipBytes = localIp.GetAddressBytes();
                    return $"{ipBytes[0]}.{ipBytes[1]}.{ipBytes[2]}.0/24";
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not determine local subnet");
            }

            return "192.168.1.0/24"; // Default fallback
        }

        private List<string> GenerateIpRange(string cidr)
        {
            var ips = new List<string>();

            try
            {
                var parts = cidr.Split('/');
                var baseIp = parts[0];
                var prefix = int.Parse(parts[1]);

                var ipBytes = baseIp.Split('.').Select(byte.Parse).ToArray();
                var baseAddress = new IPAddress(ipBytes);

                // For /24 subnet, scan all 254 addresses
                if (prefix == 24)
                {
                    for (int i = 1; i < 255; i++)
                    {
                        ips.Add($"{ipBytes[0]}.{ipBytes[1]}.{ipBytes[2]}.{i}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating IP range from {cidr}");
            }

            return ips;
        }

        #endregion
    }
}
