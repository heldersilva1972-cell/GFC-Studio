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
            ILogger logger)
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
                    return cameras;
                }

                var nvrHost = settings.NvrIpAddress;
                var nvrPort = settings.NvrPort ?? 80;
                var username = settings.NvrUsername;
                var password = settings.NvrPassword;

                logger.LogInformation($"Discovering cameras from NVR at {nvrHost}:{nvrPort}");

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
                        cameras = ParseHikvisionChannelList(xmlContent, nvrHost, username, password, logger);
                        
                        logger.LogInformation($"Found {cameras.Count} cameras from NVR");
                    }
                    else
                    {
                        logger.LogWarning($"Failed to get camera list from NVR. Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error querying NVR for camera list");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error discovering cameras from NVR");
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
                var channels = doc.Descendants("VideoInputChannel");

                foreach (var channel in channels)
                {
                    var channelId = channel.Element("id")?.Value;
                    var channelName = channel.Element("name")?.Value ?? $"Camera {channelId}";
                    var enabled = channel.Element("enabled")?.Value == "true";

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
