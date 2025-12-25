// [NEW]
using Microsoft.AspNetCore.Http;
using System;

namespace GFC.BlazorServer.Services
{
    public class DeviceDetectionService : IDeviceDetectionService
    {
        public DeviceInfo DetectDevice(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            return new DeviceInfo
            {
                Type = DetermineDeviceType(userAgent),
                OS = DetermineOS(userAgent),
                AppStoreUrl = GetAppStoreUrl(userAgent)
            };
        }

        private string DetermineDeviceType(string userAgent)
        {
            if (userAgent.Contains("iPhone") || userAgent.Contains("iPad") || userAgent.Contains("Android"))
                return "Mobile";
            return "Desktop";
        }

        private string DetermineOS(string userAgent)
        {
            if (userAgent.Contains("iPhone") || userAgent.Contains("iPad")) return "iOS";
            if (userAgent.Contains("Android")) return "Android";
            if (userAgent.Contains("Macintosh")) return "macOS";
            if (userAgent.Contains("Windows")) return "Windows";
            if (userAgent.Contains("Linux")) return "Linux";
            return "Unknown";
        }

        private string GetAppStoreUrl(string userAgent)
        {
            if (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
                return "https://apps.apple.com/app/wireguard/id1441195209";
            if (userAgent.Contains("Android"))
                return "https://play.google.com/store/apps/details?id=com.wireguard.android";
            if (userAgent.Contains("Macintosh"))
                return "https://apps.apple.com/app/wireguard/id1451685025";
            if (userAgent.Contains("Windows"))
                return "https://download.wireguard.com/windows-client/wireguard-installer.exe";

            return "https://www.wireguard.com/install/";
        }
    }
}
