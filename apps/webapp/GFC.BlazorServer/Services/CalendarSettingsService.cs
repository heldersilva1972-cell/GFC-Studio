using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GFC.BlazorServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    /// <summary>
    /// Reads and writes <see cref="CalendarSettings"/> to a JSON file in App_Data.
    /// Settings include visual preferences (colours, default view) for the FullCalendar preview.
    /// </summary>
    public class CalendarSettingsService
    {
        private readonly string _filePath;
        private readonly ILogger<CalendarSettingsService> _logger;

        public CalendarSettingsService(IWebHostEnvironment env, ILogger<CalendarSettingsService> logger)
        {
            _filePath = Path.Combine(env.ContentRootPath, "App_Data", "calendar_ui_settings.json");
            _logger = logger;
        }

        /// <summary>Returns the stored settings, or sensible defaults if none exist.</summary>
        public async Task<CalendarSettings> GetAsync()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    var settings = JsonSerializer.Deserialize<CalendarSettings>(json);
                    if (settings != null) return settings;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "CalendarSettingsService: failed to read {File}", _filePath);
            }

            return new CalendarSettings();
        }

        /// <summary>Persists the provided <paramref name="settings"/> to disk.</summary>
        public async Task SaveAsync(CalendarSettings settings)
        {
            try
            {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CalendarSettingsService: failed to save {File}", _filePath);
                throw;
            }
        }
    }
}
