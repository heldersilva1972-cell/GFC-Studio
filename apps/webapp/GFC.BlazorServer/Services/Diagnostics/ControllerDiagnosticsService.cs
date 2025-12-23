// [NEW]
using GFC.Core.Models.Diagnostics;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Diagnostics
{
    public class ControllerDiagnosticsService
    {
        private readonly GFC.BlazorServer.Services.ControllerRegistryService _registry;
        private readonly GFC.BlazorServer.Services.Controllers.IControllerClient _client;

        public ControllerDiagnosticsService(
            GFC.BlazorServer.Services.ControllerRegistryService registry,
            GFC.BlazorServer.Services.Controllers.IControllerClient client)
        {
            _registry = registry;
            _client = client;
        }

        public async Task<ControllerHealthInfo> GetControllerHealthAsync()
        {
            try
            {
                var controllers = await _registry.GetControllersAsync();
                var mainController = controllers.FirstOrDefault();

                if (mainController == null)
                {
                    return new ControllerHealthInfo
                    {
                        IsConnected = false,
                        Status = HealthStatus.Unknown,
                        Name = "No Controller Found"
                    };
                }

                var status = await _client.GetRunStatusAsync(mainController.Id, System.Threading.CancellationToken.None);
                
                return new ControllerHealthInfo
                {
                    IsConnected = status.IsOnline,
                    Status = status.IsOnline ? HealthStatus.Healthy : HealthStatus.Critical,
                    Name = mainController.Name,
                    ResponseTime = System.TimeSpan.Zero, // Would need actual ping timing
                    DoorCount = mainController.Doors.Count,
                    ReaderCount = mainController.Doors.Count * 2, // Approx 2 readers per door usually
                    CardCount = 0, // Need API to fetch card count
                    EventCount24h = 0, // Need event log query
                    LastCommunication = status.IsOnline ? System.DateTime.UtcNow : System.DateTime.MinValue
                };
            }
            catch
            {
                return new ControllerHealthInfo { Status = HealthStatus.Critical, Name = "Error Fetching Data" };
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            // Placeholder: Ideally ping the main controller
             await Task.Yield();
             return true;
        }
    }
}
