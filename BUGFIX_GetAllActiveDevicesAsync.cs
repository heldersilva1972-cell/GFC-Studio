    public async Task<List<DeviceSessionDto>> GetAllActiveDevicesAsync()
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            
            // BUG FIX: Fetch devices first, THEN project in memory
            // EF Core cannot translate complex string interpolation with null checks to SQL
            var devices = await context.TrustedDevices
                                .Include(d => d.User)
                                .Where(d => !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                                .OrderByDescending(d => d.LastUsedUtc)
                                .ToListAsync();
            
            // Now project in memory where C# can handle the null checks
            var sessions = devices.Select(d => new DeviceSessionDto
            {
                UserId = d.UserId,
                Username = d.IsStation 
                    ? $"Station Device (Auth by: {(d.User != null ? d.User.Username : "Unknown")})"
                    : (d.User != null ? d.User.Username : "Deleted User"),
                DeviceToken = d.DeviceToken,
                UserAgent = d.UserAgent,
                IpAddress = d.IpAddress,
                LastUsedUtc = d.LastUsedUtc,
                ExpiresAtUtc = d.ExpiresAtUtc,
                IsRevoked = d.IsRevoked,
                IsStation = d.IsStation
            }).ToList();
            
            return sessions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all active devices");
            return new List<DeviceSessionDto>();
        }
    }
