using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GFCDevOpsUtility
{
    /// <summary>
    /// Self-contained C# port of sync-version.ps1.
    /// Eliminates the external PowerShell script dependency entirely.
    /// Handles version synchronization across all GFC project files and the remote database.
    /// </summary>
    public static class VersionSyncEngine
    {
        private const string ServerUrl = "https://gfc.lovanow.com";
        private const string ApiKey = "GFC_SYNC_V2_SECRET_2026";

        public delegate void LogCallback(string message, bool isError = false);

        /// <summary>
        /// Main entry point. Syncs a project's version across all relevant source files and the remote DB.
        /// </summary>
        public static async Task<bool> SyncVersionAsync(
            string project,
            string workspace,
            string? explicitVersion,
            bool useNext,
            bool dryRun,
            bool noSync,
            LogCallback log)
        {
            try
            {
                // --- 1. RESOLVE PATHS ---
                string posVersionService = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.UI", "Services", "PosVersionService.cs");
                string posCsproj        = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Mobile", "GFC.Pos.Mobile.csproj");
                string posProps         = Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "PosVersion.props");
                string webappSettings   = Path.Combine(workspace, "apps", "webapp", "GFC.BlazorServer", "appsettings.json");
                string mobileVersionJson = Path.Combine(workspace, "apps", "GFC-Mobile-Standalone", "version.json");

                string[] posSWFiles = new[]
                {
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.UI", "wwwroot", "service-worker.js"),
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.UI", "wwwroot", "service-worker.published.js"),
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Terminal", "wwwroot", "service-worker.js"),
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Terminal", "wwwroot", "service-worker.published.js"),
                };

                string[] posVersionTxtFiles = new[]
                {
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Terminal", "wwwroot", "version.txt"),
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.UI", "wwwroot", "version.txt"),
                };

                string[] posIndexFiles = new[]
                {
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.Terminal", "wwwroot", "index.html"),
                    Path.Combine(workspace, "apps", "GFC-Pos-Standalone", "GFC.Pos.UI", "wwwroot", "index.html"),
                };

                // --- 2. READ CURRENT VERSION ---
                string currentVersion = ReadCurrentVersion(project, posProps, webappSettings, mobileVersionJson);
                if (string.IsNullOrEmpty(currentVersion))
                {
                    log($"!!! Could not read current version for project: {project}", true);
                    return false;
                }

                // --- 3. DETERMINE TARGET VERSION ---
                string targetVersion = explicitVersion ?? string.Empty;
                if (useNext)
                {
                    targetVersion = IncrementVersion(currentVersion);
                }

                if (string.IsNullOrEmpty(targetVersion))
                {
                    log("!!! No target version specified. Provide a version or use -Next.", true);
                    return false;
                }

                log($"[SYNC] Syncing {project} → {targetVersion}{(dryRun ? " [DRY RUN]" : "")}");

                // --- 4. APPLY UPDATES ---
                if (project.Equals("POS", StringComparison.OrdinalIgnoreCase))
                {
                    string appVer = targetVersion.Replace(".", "");

                    // PosVersionService.cs
                    ApplyRegexToFile(posVersionService, dryRun, log,
                        (@"(GetRevision\(\)\s*=>\s*"")[^""]+("")", $"${{1}}{targetVersion}${{2}}"),
                        (@"(return\s*"")[^""]+("";)", $"${{1}}{targetVersion}${{2}}")
                    );

                    // PosVersion.props
                    ApplyRegexToFile(posProps, dryRun, log,
                        (@"(<PosVersion>)[^<]+(</PosVersion>)", $"${{1}}{targetVersion}${{2}}"),
                        (@"(<PosBuild>)[^<]+(</PosBuild>)", $"${{1}}{appVer}${{2}}")
                    );

                    // GFC.Pos.Mobile.csproj
                    ApplyRegexToFile(posCsproj, dryRun, log,
                        (@"<ApplicationDisplayVersion>[^<]+</ApplicationDisplayVersion>", $"<ApplicationDisplayVersion>{targetVersion}</ApplicationDisplayVersion>"),
                        (@"<ApplicationVersion>[^<]+</ApplicationVersion>", $"<ApplicationVersion>{appVer}</ApplicationVersion>")
                    );

                    // Service Workers
                    foreach (var f in posSWFiles)
                    {
                        ApplyRegexToFile(f, dryRun, log,
                            (@"// GFC POS Revision: .*", $"// GFC POS Revision: {targetVersion}"),
                            (@"(\$\{cacheNamePrefix\})[^`']+", $"${{1}}{targetVersion}")
                        );
                    }

                    // index.html cache busters
                    foreach (var f in posIndexFiles)
                    {
                        ApplyRegexToFile(f, dryRun, log,
                            (@"(const version = ')[^']+", $"${{1}}{targetVersion}"),
                            (@"(\.css\?v=)[^""]+", $"${{1}}{targetVersion}")
                        );
                    }

                    // version.txt files (POS only, not WebApp)
                    foreach (var f in posVersionTxtFiles)
                    {
                        string? dir = Path.GetDirectoryName(f);
                        if (dir != null && Directory.Exists(dir))
                        {
                            if (dryRun)
                                log($"[DRY]  Would write '{targetVersion}' to {Path.GetFileName(f)}");
                            else
                            {
                                File.WriteAllText(f, targetVersion);
                                log($"[OK]   Updated {Path.GetFileName(f)} → {targetVersion}");
                            }
                        }
                    }

                    // appsettings.json (PosRevision)
                    UpdateAppSettings(webappSettings, dryRun, log, json =>
                    {
                        if (json["ApplicationVersion"] is JsonObject av)
                        {
                            av["PosRevision"] = targetVersion;
                            av["PosBuildNumber"] = appVer;
                        }
                    });
                }
                else if (project.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
                {
                    // version.json
                    if (File.Exists(mobileVersionJson))
                    {
                        if (dryRun)
                        {
                            log($"[DRY]  Would update version.json → {targetVersion}");
                        }
                        else
                        {
                            var node = JsonNode.Parse(File.ReadAllText(mobileVersionJson))!.AsObject();
                            node["version"] = targetVersion;
                            node["build"] = targetVersion;
                            node["description"] = $"GFC Mobile Revision: {targetVersion} (Sync)";
                            File.WriteAllText(mobileVersionJson, node.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                            log($"[OK]   Updated version.json → {targetVersion}");
                        }
                    }

                    // Service Workers
                    string[] mobileSWFiles = new[]
                    {
                        Path.Combine(workspace, "apps", "GFC-Mobile-Standalone", "GFC.Mobile", "wwwroot", "service-worker.js"),
                        Path.Combine(workspace, "apps", "GFC-Mobile-Standalone", "GFC.Mobile", "wwwroot", "service-worker.published.js"),
                    };
                    foreach (var f in mobileSWFiles)
                    {
                        ApplyRegexToFile(f, dryRun, log,
                            (@"// GFC Mobile Revision: .*", $"// GFC Mobile Revision: {targetVersion}")
                        );
                    }

                    // appsettings.json (MobileRevision)
                    UpdateAppSettings(webappSettings, dryRun, log, json =>
                    {
                        if (json["ApplicationVersion"] is JsonObject av)
                        {
                            av["MobileRevision"] = targetVersion;
                            // Increment the main revision
                            string rev = av["Revision"]?.GetValue<string>() ?? "0.0.0";
                            av["Revision"] = IncrementVersion(rev);
                        }
                    });
                }
                else if (project.Equals("WebApp", StringComparison.OrdinalIgnoreCase))
                {
                    UpdateAppSettings(webappSettings, dryRun, log, json =>
                    {
                        if (json["ApplicationVersion"] is JsonObject av)
                        {
                            av["Revision"] = targetVersion;
                        }
                    });
                }

                // --- 5. REMOTE DATABASE SYNC ---
                if (!noSync)
                {
                    log("[SYNC] Syncing version to Remote Database...");
                    if (dryRun)
                    {
                        log($"[DRY]  Would POST to {ServerUrl}/api/mobile-reporting/sync-version?project={project}&version={targetVersion}");
                    }
                    else
                    {
                        await RemoteSyncAsync(project, targetVersion, log);
                    }
                }
                else
                {
                    log("[SYNC] Skipping Remote Database Sync (NoSync is active).");
                }

                log($"[OK]   DONE! {project} is now on {targetVersion}");
                return true;
            }
            catch (Exception ex)
            {
                log($"!!! SYNC EXCEPTION: {ex.Message}", true);
                return false;
            }
        }

        // -------------------------------------------------------------------------
        // HELPERS
        // -------------------------------------------------------------------------

        public static string ReadCurrentVersion(string project, string posProps, string webappSettings, string mobileVersionJson)
        {
            try
            {
                if (project.Equals("POS", StringComparison.OrdinalIgnoreCase) && File.Exists(posProps))
                {
                    string content = File.ReadAllText(posProps);
                    var m = Regex.Match(content, @"<PosVersion>(.*?)</PosVersion>");
                    if (m.Success) return m.Groups[1].Value.Trim();
                    m = Regex.Match(content, @"<PosBuild>(.*?)</PosBuild>");
                    if (m.Success) return m.Groups[1].Value.Trim();
                }
                else if (project.Equals("WebApp", StringComparison.OrdinalIgnoreCase) && File.Exists(webappSettings))
                {
                    var node = JsonNode.Parse(File.ReadAllText(webappSettings));
                    return node?["ApplicationVersion"]?["Revision"]?.GetValue<string>() ?? string.Empty;
                }
                else if (project.Equals("Mobile", StringComparison.OrdinalIgnoreCase) && File.Exists(mobileVersionJson))
                {
                    var node = JsonNode.Parse(File.ReadAllText(mobileVersionJson));
                    return node?["version"]?.GetValue<string>() ?? string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }

        private static string IncrementVersion(string version)
        {
            var parts = version.Split('.');
            if (parts.Length >= 1 && int.TryParse(parts[^1], out int last))
            {
                parts[^1] = (last + 1).ToString();
            }
            return string.Join(".", parts);
        }

        private static void ApplyRegexToFile(string filePath, bool dryRun, LogCallback log, params (string pattern, string replacement)[] replacements)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                string content = File.ReadAllText(filePath);
                string newContent = content;

                foreach (var (pattern, replacement) in replacements)
                {
                    newContent = Regex.Replace(newContent, pattern, replacement);
                }

                if (dryRun)
                {
                    log($"[DRY]  Would update {Path.GetFileName(filePath)}");
                }
                else if (newContent != content)
                {
                    File.WriteAllText(filePath, newContent);
                    log($"[OK]   Updated {Path.GetFileName(filePath)}");
                }
                else
                {
                    log($"[SKIP] No changes needed in {Path.GetFileName(filePath)}");
                }
            }
            catch (Exception ex)
            {
                log($"[WARN] Failed to update {Path.GetFileName(filePath)}: {ex.Message}", true);
            }
        }

        private static void UpdateAppSettings(string filePath, bool dryRun, LogCallback log, Action<JsonObject> mutate)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                string content = File.ReadAllText(filePath);
                var node = JsonNode.Parse(content)?.AsObject();
                if (node == null) return;

                if (dryRun)
                {
                    log($"[DRY]  Would update {Path.GetFileName(filePath)}");
                    return;
                }

                mutate(node);
                File.WriteAllText(filePath, node.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                log($"[OK]   Updated {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                log($"[WARN] Failed to update {Path.GetFileName(filePath)}: {ex.Message}", true);
            }
        }

        private static async Task RemoteSyncAsync(string project, string version, LogCallback log)
        {
            try
            {
                string url = $"{ServerUrl}/api/mobile-reporting/sync-version?project={project}&version={version}&apiKey={ApiKey}";
                using var http = new HttpClient();
                http.Timeout = TimeSpan.FromSeconds(15);
                var response = await http.PostAsync(url, new StringContent("", System.Text.Encoding.UTF8, "application/json"));
                string body = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                    log($"[OK]   Remote DB Sync successful: {body}");
                else
                    log($"[WARN] Remote DB Sync returned {(int)response.StatusCode}: {body}", true);
            }
            catch (Exception ex)
            {
                log($"[WARN] Remote DB Sync failed (non-critical): {ex.Message}", true);
            }
        }
    }
}
