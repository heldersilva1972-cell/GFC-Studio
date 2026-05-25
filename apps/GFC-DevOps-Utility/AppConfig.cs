using System;
using System.IO;
using System.Text.Json;

namespace GFCDevOpsUtility
{
    public class AppConfig
    {
        public string WorkspacePath { get; set; } = string.Empty;
        public string PublishOutputPath { get; set; } = string.Empty;
        public string ZipInputPath { get; set; } = string.Empty;

        // Deploy settings per application (saved to restore user selections)
        public string MobileLivePath { get; set; } = @"C:\inetpub\wwwroot\GFCMobile";
        public string MobileStagingPath { get; set; } = @"C:\inetpub\PublishGFCMobile";
        public string MobileBackupPath { get; set; } = @"C:\inetpub\history_mobile";
        public string MobileIisSite { get; set; } = "GFCMobile";
        public string MobileIisAppPool { get; set; } = "GFCMobile";

        public string PosLivePath { get; set; } = @"C:\inetpub\wwwroot\GFCPOS";
        public string PosStagingPath { get; set; } = @"C:\inetpub\PublishGFCPos";
        public string PosBackupPath { get; set; } = @"C:\inetpub\history_pos";
        public string PosIisSite { get; set; } = "Default Web Site";
        public string PosIisAppPool { get; set; } = "DefaultAppPool";
        public bool PosDeployMobileApk { get; set; } = false;
        public bool PosPublishMobileApk { get; set; } = false;
        public string PosApkDistFolder { get; set; } = @"C:\inetpub\wwwroot\GFCPOS\downloads";

        public string WebAppLivePath { get; set; } = @"C:\inetpub\GFCWebApp";
        public string WebAppStagingPath { get; set; } = @"C:\inetpub\PublishGFCWebApp";
        public string WebAppBackupPath { get; set; } = @"C:\inetpub\history_webapp";
        public string WebAppIisSite { get; set; } = "GFCWebApp";
        public string WebAppIisAppPool { get; set; } = "GFCWebApp";

        public bool AutoConfigureWebConfig { get; set; } = true;
        public bool PurgeFiles { get; set; } = true;

        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "GFC-DevOps-Utility", 
            "config.json"
        );

        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var config = JsonSerializer.Deserialize<AppConfig>(json);
                    if (config != null) return config;
                }
            }
            catch
            {
                // Fallback to default config on error
            }

            var defaultConfig = new AppConfig();
            defaultConfig.WorkspacePath = FindDefaultWorkspace();
            defaultConfig.PublishOutputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            defaultConfig.ZipInputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return defaultConfig;
        }

        public void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(ConfigPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch
            {
                // Fail silently or handle
            }
        }

        private static string FindDefaultWorkspace()
        {
            // Scans upwards or checks current/parent folder to see if we are running inside the GFC-Studio structure
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(dir))
            {
                if (File.Exists(Path.Combine(dir, "sync-version.ps1")) && Directory.Exists(Path.Combine(dir, "apps")))
                {
                    return dir;
                }
                dir = Path.GetDirectoryName(dir) ?? string.Empty;
            }

            // Fallback to a common developer path or leave empty
            string devPath = @"C:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2";
            if (Directory.Exists(devPath)) return devPath;

            return string.Empty;
        }
    }
}
