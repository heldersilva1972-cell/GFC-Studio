// [NEW]
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public class CloudflareTunnelHealthService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CloudflareTunnelHealthService> _logger;
        private readonly TunnelStatusService _tunnelStatusService;

        public CloudflareTunnelHealthService(IServiceProvider services, ILogger<CloudflareTunnelHealthService> logger, TunnelStatusService tunnelStatusService)
        {
            _services = services;
            _logger = logger;
            _tunnelStatusService = tunnelStatusService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckTunnelHealthAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        public async Task CheckTunnelHealthAsync()
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GfcDbContext>();
            var settings = await dbContext.SystemSettings.FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(settings?.PublicDomain))
            {
                await _tunnelStatusService.SetStatusAsync(false);
                return;
            }

            var httpClient = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
            try
            {
                var response = await httpClient.GetAsync($"https://{settings.PublicDomain}/health");
                await _tunnelStatusService.SetStatusAsync(response.IsSuccessStatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking Cloudflare tunnel health.");
                await _tunnelStatusService.SetStatusAsync(false);
            }
        }
    }
}
