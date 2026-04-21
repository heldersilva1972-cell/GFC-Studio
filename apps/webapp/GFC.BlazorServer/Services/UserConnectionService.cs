using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
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
        public bool IsMobile { get; set; }

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
            // Only detect if it's unknown. Once set (by middleware), preserve it.
            if (LocationType == LocationType.Unknown || string.IsNullOrEmpty(IpAddress))
            {
                DetectConnection();
            }
        }
 
        public async Task DetectConnectionIfNeededAsync()
        {
            if (LocationType == LocationType.Unknown || string.IsNullOrEmpty(IpAddress))
            {
                await DetectConnectionAsync();
            }
        }

        private async Task DetectConnectionAsync()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    LocationType = LocationType.Unknown;
                    return;
                }
 
                // Detect IP and Mobile
                DetectBasicInfo(httpContext);
 
                if (string.IsNullOrEmpty(IpAddress) || !IPAddress.TryParse(IpAddress, out var remoteIp))
                {
                    LocationType = LocationType.Unknown;
                    return;
                }
 
                // Check localhost
                if (IPAddress.IsLoopback(remoteIp) || remoteIp.ToString() == "::1" || remoteIp.ToString().EndsWith(":1"))
                {
                    LocationType = LocationType.Local;
                    return;
                }
 
                // Get system settings (ASYNC)
                var settings = await _systemSettingsService.GetAsync();
                if (settings == null)
                {
                    LocationType = LocationType.Public;
                    return;
                }
 
                // Check subnets
                LocationType = DetectSubnet(remoteIp, settings);
            }
            catch (Exception ex)
            {
                IpAddress ??= "Detection Error: " + ex.Message;
                LocationType = LocationType.Public;
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
 
                DetectBasicInfo(httpContext);
 
                if (string.IsNullOrEmpty(IpAddress) || !IPAddress.TryParse(IpAddress, out var remoteIp))
                {
                    LocationType = LocationType.Unknown;
                    return;
                }
 
                if (IPAddress.IsLoopback(remoteIp) || remoteIp.ToString() == "::1" || remoteIp.ToString().EndsWith(":1"))
                {
                    LocationType = LocationType.Local;
                    return;
                }
 
                var settings = _systemSettingsService.GetSettings();
                if (settings == null)
                {
                    LocationType = LocationType.Public;
                    return;
                }
 
                LocationType = DetectSubnet(remoteIp, settings);
            }
            catch (Exception ex)
            {
                IpAddress ??= "Error: " + ex.Message;
                LocationType = LocationType.Public;
            }
        }
 
        private void DetectBasicInfo(HttpContext httpContext)
        {
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
 
            // Normalize
            if (!string.IsNullOrEmpty(ipStr) && IPAddress.TryParse(ipStr, out var remoteIp))
            {
                if (remoteIp.IsIPv4MappedToIPv6)
                {
                    ipStr = remoteIp.MapToIPv4().ToString();
                }
            }
            IpAddress = ipStr;
 
            // Detect Mobile Device via User Agent
            if (httpContext.Request.Headers.TryGetValue("User-Agent", out var ua))
            {
                var userAgent = ua.ToString().ToLower();
                IsMobile = userAgent.Contains("android") || 
                           userAgent.Contains("iphone") || 
                           userAgent.Contains("ipad") || 
                           userAgent.Contains("ipod") || 
                           userAgent.Contains("mobile");
            }
        }
 
        private LocationType DetectSubnet(IPAddress remoteIp, SystemSettings settings)
        {
            // Check LAN subnet
            if (!string.IsNullOrEmpty(settings.LanSubnet) && IsInSubnet(remoteIp, settings.LanSubnet))
            {
                return LocationType.LAN;
            }
 
            // Check VPN subnet (WireGuard)
            if (!string.IsNullOrEmpty(settings.WireGuardSubnet) && IsInSubnet(remoteIp, settings.WireGuardSubnet))
            {
                return LocationType.VPN;
            }
 
            return LocationType.Public;
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

                // Normalize IPv4-mapped IPv6 addresses for comparison
                if (address.IsIPv4MappedToIPv6)
                {
                    addressBytes = address.MapToIPv4().GetAddressBytes();
                }
                
                if (subnetAddress.IsIPv4MappedToIPv6)
                {
                    subnetBytes = subnetAddress.MapToIPv4().GetAddressBytes();
                }

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


