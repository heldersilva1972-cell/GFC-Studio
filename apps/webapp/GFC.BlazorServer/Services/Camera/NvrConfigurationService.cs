using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GFC.BlazorServer.Services.Camera
{
    public interface INvrConfigurationService
    {
        Task<NvrSettings> GetSettingsAsync();
        Task<bool> SaveSettingsAsync(NvrSettings settings);
    }

    public class NvrConfigurationService : INvrConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public NvrConfigurationService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public Task<NvrSettings> GetSettingsAsync()
        {
            var settings = new NvrSettings
            {
                IpAddress = _configuration["NvrSettings:IpAddress"] ?? "192.168.1.64",
                RtspPort = int.Parse(_configuration["NvrSettings:RtspPort"] ?? "554"),
                HttpPort = int.Parse(_configuration["NvrSettings:HttpPort"] ?? "80"),
                Username = _configuration["NvrSettings:Username"] ?? "",
                Password = _configuration["NvrSettings:Password"] ?? ""
            };

            // Clear placeholder values
            if (settings.Username?.Contains("REPLACE") == true) settings.Username = "";
            if (settings.Password?.Contains("REPLACE") == true) settings.Password = "";

            return Task.FromResult(settings);
        }

        public async Task<bool> SaveSettingsAsync(NvrSettings settings)
        {
            try
            {
                // Find appsettings.json in the project root (not bin folder)
                var projectRoot = _environment.ContentRootPath;
                var appSettingsPath = Path.Combine(projectRoot, "appsettings.json");

                // Log the path for debugging
                Console.WriteLine($"[NvrConfig] Attempting to save to: {appSettingsPath}");
                Console.WriteLine($"[NvrConfig] File exists: {File.Exists(appSettingsPath)}");

                if (!File.Exists(appSettingsPath))
                {
                    Console.WriteLine($"[NvrConfig] ERROR: appsettings.json not found at {appSettingsPath}");
                    throw new FileNotFoundException($"appsettings.json not found at {appSettingsPath}");
                }

                // Read current JSON
                var json = await File.ReadAllTextAsync(appSettingsPath);
                Console.WriteLine($"[NvrConfig] Read {json.Length} characters from file");
                
                var jsonDoc = JsonDocument.Parse(json);

                using var stream = new MemoryStream();
                using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
                {
                    writer.WriteStartObject();

                    foreach (var property in jsonDoc.RootElement.EnumerateObject())
                    {
                        if (property.Name == "NvrSettings")
                        {
                            writer.WritePropertyName("NvrSettings");
                            writer.WriteStartObject();
                            writer.WriteString("IpAddress", settings.IpAddress);
                            writer.WriteNumber("RtspPort", settings.RtspPort);
                            writer.WriteNumber("HttpPort", settings.HttpPort);
                            writer.WriteString("Username", settings.Username);
                            writer.WriteString("Password", settings.Password);

                            Console.WriteLine($"[NvrConfig] Writing credentials - Username: {settings.Username}, Password: {(string.IsNullOrEmpty(settings.Password) ? "EMPTY" : "***")}");

                            // Preserve Cameras array if it exists
                            if (property.Value.TryGetProperty("Cameras", out var cameras))
                            {
                                writer.WritePropertyName("Cameras");
                                cameras.WriteTo(writer);
                            }

                            writer.WriteEndObject();
                        }
                        else
                        {
                            property.WriteTo(writer);
                        }
                    }

                    writer.WriteEndObject();
                }

                var updatedJson = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                Console.WriteLine($"[NvrConfig] Generated {updatedJson.Length} characters of JSON");
                
                await File.WriteAllTextAsync(appSettingsPath, updatedJson);
                Console.WriteLine($"[NvrConfig] Successfully wrote to file");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NvrConfig] ERROR: {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine($"[NvrConfig] Stack: {ex.StackTrace}");
                return false;
            }
        }
    }

    public class NvrSettings
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "IP Address is required")]
        public string IpAddress { get; set; } = "192.168.1.64";
        
        public int RtspPort { get; set; } = 554;
        public int HttpPort { get; set; } = 80;
        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = "";
        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
    }
}
