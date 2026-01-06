        public async Task<List<DiagnosticEntry>> RunDiagnosticsAsync()
        {
            var logs = new List<DiagnosticEntry>();

            // Helper to record logs
            void Log(string component, string status, string msg, long ms = 0)
            {
                logs.Add(new DiagnosticEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Component = component,
                    Status = status,
                    Message = msg,
                    DurationMs = ms
                });
            }

            // 1. Host Connectivity (Google DNS Ping)
            var sw = Stopwatch.StartNew();
            try 
            {
                using var ping = new System.Net.NetworkInformation.Ping();
                var result = await ping.SendPingAsync("8.8.8.8", 2000);
                sw.Stop();
                if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                    Log("Internet", "Success", $"Outbound connectivity verified (Ping: {result.RoundtripTime}ms)", sw.ElapsedMilliseconds);
                else
                    Log("Internet", "Warning", $"Ping 8.8.8.8 failed: {result.Status}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("Internet", "Failed", $"Outbound connectivity error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 2. Database Functional Check
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var canConnect = await db.Database.CanConnectAsync();
                if (canConnect) 
                {
                    var cmd = db.Database.GetDbConnection().CreateCommand();
                    cmd.CommandText = "SELECT 1";
                    await db.Database.OpenConnectionAsync();
                    await cmd.ExecuteScalarAsync();
                    sw.Stop();
                    Log("Database", "Success", "Connection & Read Query Validated", sw.ElapsedMilliseconds);
                }
                else
                {
                    sw.Stop();
                    Log("Database", "Failed", "CanConnectAsync returned false", sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("Database", "Failed", $"DB Connection Error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 3. Local Web Server (Loopback)
            sw.Restart();
            try
            {
                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(2);
                var response = await client.GetAsync("http://localhost/"); 
                sw.Stop();
                Log("LocalHost", "Success", $"Local Web Server responded: {response.StatusCode}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("LocalHost", "Warning", $"Local HTTP Check Failed: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 4. Cloudflared Service Status
            sw.Restart();
            var serviceRunning = CheckIfServiceRunning("cloudflared");
            if (serviceRunning)
            {
                Log("TunnelService", "Success", "Cloudflared Service is Running", 0);
            }
            else
            {
                Log("TunnelService", "Failed", "Cloudflared Service is NOT Running", 0);
            }

            // 5. DNS Resolution for Public Domain
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var settings = await db.SystemSettings.FirstOrDefaultAsync();
                var domain = settings?.PrimaryDomain ?? "gfc.lovanow.com";
                
                var addresses = await System.Net.Dns.GetHostAddressesAsync(domain);
                sw.Stop();
                if (addresses.Length > 0)
                {
                    var ipList = string.Join(", ", addresses.Select(a => a.ToString()));
                    Log("DNS Resolution", "Success", $"{domain} → {ipList}", sw.ElapsedMilliseconds);
                }
                else
                {
                    Log("DNS Resolution", "Failed", $"{domain} did not resolve", sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("DNS Resolution", "Failed", $"DNS lookup error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 6. Public Domain HTTPS Check
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var settings = await db.SystemSettings.FirstOrDefaultAsync();
                var domain = settings?.PrimaryDomain ?? "gfc.lovanow.com";
                
                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                var response = await client.GetAsync($"https://{domain}/health");
                sw.Stop();
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Log("Public HTTPS", "Success", $"https://{domain}/health → {response.StatusCode}: {content}", sw.ElapsedMilliseconds);
                }
                else
                {
                    Log("Public HTTPS", "Warning", $"https://{domain}/health → {response.StatusCode}", sw.ElapsedMilliseconds);
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                sw.Stop();
                Log("Public HTTPS", "Failed", $"HTTPS request error: {ex.Message} | Inner: {ex.InnerException?.Message}", sw.ElapsedMilliseconds);
            }
            catch (TaskCanceledException)
            {
                sw.Stop();
                Log("Public HTTPS", "Failed", "Request timed out after 10 seconds", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("Public HTTPS", "Failed", $"Unexpected error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 7. Database Connection Details & Version
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var connStr = db.Database.GetConnectionString();
                var builder = new SqlConnectionStringBuilder(connStr);
                
                var cmd = db.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = "SELECT @@VERSION";
                await db.Database.OpenConnectionAsync();
                var version = await cmd.ExecuteScalarAsync();
                sw.Stop();
                
                var versionShort = version?.ToString()?.Split('\n')[0] ?? "Unknown";
                Log("DB Version", "Success", $"{builder.DataSource}\\{builder.InitialCatalog} - {versionShort}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("DB Version", "Failed", $"Cannot retrieve version: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 8. Database Table & Data Verification
            sw.Restart();
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var memberCount = await db.Members.CountAsync();
                var settingsExists = await db.SystemSettings.AnyAsync();
                sw.Stop();
                
                Log("DB Schema", "Success", $"Members: {memberCount}, SystemSettings: {(settingsExists ? "Configured" : "Missing")}", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log("DB Schema", "Failed", $"Schema check error: {ex.Message}", sw.ElapsedMilliseconds);
            }

            // 9. Tunnel Configuration File Analysis
            sw.Restart();
            var configPath = @"C:\ProgramData\cloudflared\config.yml";
            if (File.Exists(configPath))
            {
                try
                {
                    var configContent = await File.ReadAllTextAsync(configPath);
                    var tunnelId = "Not found";
                    var url = "Not found";
                    
                    foreach (var line in configContent.Split('\n'))
                    {
                        if (line.Contains("tunnel:")) tunnelId = line.Split(':')[1].Trim();
                        if (line.Contains("url:")) url = line.Split(':')[1].Trim();
                    }
                    
                    Log("Tunnel Config", "Success", $"Tunnel: {tunnelId} → Target: {url}", 0);
                }
                catch (Exception ex)
                {
                    Log("Tunnel Config", "Warning", $"Config exists but unreadable: {ex.Message}", 0);
                }
            }
            else
            {
                Log("Tunnel Config", "Failed", $"Config not found at {configPath}", 0);
            }

            // 10. Request Context & Path Analysis
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                var scheme = context.Request.Scheme;
                var host = context.Request.Host.ToString();
                var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var forwardedProto = context.Request.Headers["X-Forwarded-Proto"].ToString();
                var forwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();
                
                var pathInfo = $"{scheme}://{host} from {remoteIp}";
                if (!string.IsNullOrEmpty(forwardedProto))
                    pathInfo += $" | X-Forwarded-Proto: {forwardedProto}";
                if (!string.IsNullOrEmpty(forwardedFor))
                    pathInfo += $" | X-Forwarded-For: {forwardedFor}";
                
                var status = (scheme == "https" || forwardedProto == "https") ? "Success" : "Warning";
                Log("Request Path", status, pathInfo, 0);
            }
            else
            {
                Log("Request Path", "Warning", "No HTTP context (running outside web request)", 0);
            }

            return logs;
        }
