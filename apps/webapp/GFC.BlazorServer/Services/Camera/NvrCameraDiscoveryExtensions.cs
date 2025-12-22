// [NEW] NVR Camera Discovery Extension
using GFC.BlazorServer.Services.Camera.Models;
using GFC.BlazorServer.Services;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace GFC.BlazorServer.Services.Camera
{
    /// <summary>
    /// Extension methods for discovering cameras from NVR via ISAPI
    /// </summary>
    public static class NvrCameraDiscoveryExtensions
    {
        /// <summary>
        /// Discovers cameras from Hikvision NVR via ISAPI
        /// </summary>
        public static async Task<List<DiscoveredCamera>> DiscoverCamerasFromNvrAsync(
            this CameraDiscoveryService service,
            ISystemSettingsService settingsService,
            ILogger logger,
            Action<string> onStatusUpdate = null)
        {
            var cameras = new List<DiscoveredCamera>();

            try
            {
                // Get NVR settings from database
                var settings = await settingsService.GetAsync();
                
                if (string.IsNullOrEmpty(settings?.NvrIpAddress) || 
                    string.IsNullOrEmpty(settings?.NvrUsername) ||
                    string.IsNullOrEmpty(settings?.NvrPassword))
                {
                    logger.LogInformation("NVR not configured, skipping NVR camera discovery");
                    onStatusUpdate?.Invoke("NVR not configured in settings. Skipping.");
                    return cameras;
                }

                var nvrHost = settings.NvrIpAddress;
                var nvrPort = settings.NvrPort ?? 80;
                var username = settings.NvrUsername;
                var password = settings.NvrPassword;

                var msg = $"Connecting to NVR at {nvrHost}:{nvrPort}...";
                logger.LogInformation(msg);
                onStatusUpdate?.Invoke(msg);

                // Create HTTP client with authentication
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var httpClient = new HttpClient(handler);
                httpClient.Timeout = TimeSpan.FromSeconds(10);

                // Create basic auth header
                var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);

                // Query NVR for channel list via ISAPI
                var channelListUrl = $"http://{nvrHost}:{nvrPort}/ISAPI/System/Video/inputs/channels";
                
                try
                {
                    var response = await httpClient.GetAsync(channelListUrl);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var xmlContent = await response.Content.ReadAsStringAsync();
                        logger.LogInformation($"NVR Response Length: {xmlContent.Length} chars");
                        onStatusUpdate?.Invoke($"Received response from NVR ({xmlContent.Length} bytes). Parsing...");
                        
                        cameras = ParseHikvisionChannelList(xmlContent, nvrHost, username, password, logger);
                        
                        logger.LogInformation($"Found {cameras.Count} cameras from NVR");
                    }
                    else
                    {
                        var failMsg = $"Failed to get camera list from NVR. Status: {response.StatusCode} ({response.ReasonPhrase})";
                        logger.LogWarning(failMsg);
                        onStatusUpdate?.Invoke(failMsg);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error querying NVR for camera list");
                    onStatusUpdate?.Invoke($"Error querying NVR: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error discovering cameras from NVR");
                onStatusUpdate?.Invoke($"Critical error in NVR discovery: {ex.Message}");
            }

            return cameras;
        }

        private static List<DiscoveredCamera> ParseHikvisionChannelList(
            string xmlContent,
            string nvrHost,
            string username,
            string password,
            ILogger logger)
        {
            var cameras = new List<DiscoveredCamera>();

            try
            {
                var doc = XDocument.Parse(xmlContent);
                // Use LocalName to ignore namespaces which often cause issues
                var channels = doc.Descendants().Where(x => x.Name.LocalName == "VideoInputChannel");

                foreach (var channel in channels)
                {
                    var channelId = channel.Elements().FirstOrDefault(x => x.Name.LocalName == "id")?.Value;
                    var channelName = channel.Elements().FirstOrDefault(x => x.Name.LocalName == "name")?.Value ?? $"Camera {channelId}";
                    var enabled = channel.Elements().FirstOrDefault(x => x.Name.LocalName == "enabled")?.Value == "true";

                    if (!string.IsNullOrEmpty(channelId) && int.TryParse(channelId, out int chId))
                    {
                        // Calculate RTSP channel ID (Channel 1 = 101, Channel 2 = 201, etc.)
                        var rtspChannelId = (chId * 100) + 1;

                        var camera = new DiscoveredCamera
                        {
                            Name = channelName,
                            IpAddress = nvrHost,
                            Port = 554, // RTSP port
                            Manufacturer = "Hikvision",
                            Model = "NVR Channel",
                            DiscoveryMethod = "NVR ISAPI",
                            SupportsOnvif = false,
                            RtspUrl = $"rtsp://{username}:{password}@{nvrHost}:554/Streaming/Channels/{rtspChannelId}",
                            IsAlreadyAdded = false,
                            AvailableStreams = new List<StreamProfile>
                            {
                                new StreamProfile
                                {
                                    Name = "Main Stream",
                                    RtspPath = $"/Streaming/Channels/{rtspChannelId}",
                                    Resolution = "1920x1080"
                                },
                                new StreamProfile
                                {
                                    Name = "Sub Stream",
                                    RtspPath = $"/Streaming/Channels/{rtspChannelId + 1}",
                                    Resolution = "640x480"
                                }
                            }
                        };

                        cameras.Add(camera);
                        logger.LogInformation($"Found NVR camera: {channelName} (Channel {chId})");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error parsing NVR channel list");
            }

            return cameras;
        }
    }
}
