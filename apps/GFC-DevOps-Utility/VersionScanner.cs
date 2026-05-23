using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GFCDevOpsUtility
{
    public static class VersionScanner
    {
        public static string GetMobileVersion(string workspaceRoot)
        {
            try
            {
                string jsonPath = Path.Combine(workspaceRoot, "apps", "GFC-Mobile-Standalone", "version.json");
                if (File.Exists(jsonPath))
                {
                    string content = File.ReadAllText(jsonPath);
                    using (JsonDocument doc = JsonDocument.Parse(content))
                    {
                        if (doc.RootElement.TryGetProperty("version", out JsonElement verProp))
                        {
                            return verProp.GetString() ?? "Unknown";
                        }
                    }
                }
            }
            catch
            {
                // Suppress and return fallback
            }
            return "Unknown";
        }

        public static string GetPosVersion(string workspaceRoot)
        {
            try
            {
                string propsPath = Path.Combine(workspaceRoot, "apps", "GFC-Pos-Standalone", "PosVersion.props");
                if (File.Exists(propsPath))
                {
                    string content = File.ReadAllText(propsPath);
                    Match m = Regex.Match(content, @"<PosVersion>(.*?)</PosVersion>");
                    if (m.Success)
                    {
                        return m.Groups[1].Value.Trim();
                    }
                    Match m2 = Regex.Match(content, @"<PosBuild>(.*?)</PosBuild>");
                    if (m2.Success)
                    {
                        return m2.Groups[1].Value.Trim();
                    }
                }
            }
            catch
            {
                // Suppress and return fallback
            }
            return "Unknown";
        }

        public static string GetWebAppVersion(string workspaceRoot)
        {
            try
            {
                string settingsPath = Path.Combine(workspaceRoot, "apps", "webapp", "GFC.BlazorServer", "appsettings.json");
                if (File.Exists(settingsPath))
                {
                    string content = File.ReadAllText(settingsPath);
                    using (JsonDocument doc = JsonDocument.Parse(content))
                    {
                        if (doc.RootElement.TryGetProperty("ApplicationVersion", out JsonElement appVerProp))
                        {
                            if (appVerProp.TryGetProperty("Revision", out JsonElement revProp))
                            {
                                return revProp.GetString() ?? "Unknown";
                            }
                        }
                    }
                }
            }
            catch
            {
                // Suppress and return fallback
            }
            return "Unknown";
        }
    }
}
