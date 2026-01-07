using GFC.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace GFC.BlazorServer.Services
{
    public class UserConnectionService : IUserConnectionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBlazorSystemSettingsService _systemSettingsService;

        public UserConnectionService(
            IHttpContextAccessor httpContextAccessor,
            IBlazorSystemSettingsService systemSettingsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _systemSettingsService = systemSettingsService;
            // Don't call DetectConnection here - middleware will set IP and LocationType
        }

        public string? IpAddress { get; set; }
        public LocationType LocationType { get; set; } = LocationType.Unknown;

        /// <summary>
        /// Sets the connection information. Called by middleware.
        /// </summary>
        public void SetConnectionInfo(string ipAddress, LocationType locationType)
        {
            IpAddress = ipAddress;
            LocationType = locationType;
        }

        /// <summary>
        /// Detects connection info if not already set by middleware (fallback).
        /// </summary>
        public void DetectConnectionIfNeeded()
        {
            if (string.IsNullOrEmpty(IpAddress))
            {
                DetectConnection();
            }
        }

        private void DetectConnection()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    LocationType = LocationType.Unknown;
                    return;
                }

                // Get the user's IP address (handling Cloudflare/Proxies)
                string? ipStr = null;
                if (httpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfIp))
                {
                    ipStr = cfIp.ToString();
                }
                else if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
                {
                    ipStr = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();
                }
                
                if (string.IsNullOrEmpty(ipStr))
                {
                    ipStr = httpContext.Connection.RemoteIpAddress?.ToString();
                }

                if (string.IsNullOrEmpty(ipStr) || !IPAddress.TryParse(ipStr, out var remoteIp))
                {
                    LocationType = LocationType.Unknown;
                    return;
                }

                IpAddress = ipStr;

                // Check if localhost (IPv4 or IPv6)
                if (IPAddress.IsLoopback(remoteIp) || remoteIp.ToString() == "::1")
                {
                    LocationType = LocationType.Local;
                    return;
                }

                // Get system settings
                var settings = _systemSettingsService.GetSettings();
                if (settings == null)
                {
                    LocationType = LocationType.Public;
                    return;
                }

                // Check LAN subnet
                if (!string.IsNullOrEmpty(settings.LanSubnet) && IsInSubnet(remoteIp, settings.LanSubnet))
                {
                    LocationType = LocationType.LAN;
                    return;
                }

                // Check VPN subnet (WireGuard)
                if (!string.IsNullOrEmpty(settings.WireGuardSubnet) && IsInSubnet(remoteIp, settings.WireGuardSubnet))
                {
                    LocationType = LocationType.VPN;
                    return;
                }

                // Default to Public if not in any known subnet
                LocationType = LocationType.Public;
            }
            catch
            {
                // If detection fails, default to Public for safety
                LocationType = LocationType.Public;
            }
        }

        private bool IsInSubnet(IPAddress address, string cidr)
        {
            try
            {
                var parts = cidr.Split('/');
                if (parts.Length != 2)
                    return false;

                var subnetAddress = IPAddress.Parse(parts[0]);
                var prefixLength = int.Parse(parts[1]);

                var addressBytes = address.GetAddressBytes();
                var subnetBytes = subnetAddress.GetAddressBytes();

                if (addressBytes.Length != subnetBytes.Length)
                    return false;

                var maskBits = prefixLength;
                for (int i = 0; i < addressBytes.Length; i++)
                {
                    if (maskBits >= 8)
                    {
                        if (addressBytes[i] != subnetBytes[i])
                            return false;
                        maskBits -= 8;
                    }
                    else if (maskBits > 0)
                    {
                        var mask = (byte)(0xFF << (8 - maskBits));
                        if ((addressBytes[i] & mask) != (subnetBytes[i] & mask))
                            return false;
                        maskBits = 0;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
