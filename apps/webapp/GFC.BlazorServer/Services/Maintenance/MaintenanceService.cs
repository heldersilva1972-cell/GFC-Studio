using System.Reflection;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services.Controllers;
using GFC.Core.Interfaces;
using GFC.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Maintenance;

public class MaintenanceService : IMaintenanceService
{
    private readonly GfcDbContext _dbContext;
    private readonly ControllerRegistryService _controllerRegistry;
    private readonly IControllerClient _controllerClient; // Use for GetRunStatusAsync
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<MaintenanceService> _logger;

    public MaintenanceService(
        GfcDbContext dbContext,
        ControllerRegistryService controllerRegistry,
        IControllerClient controllerClient,
        IMemberRepository memberRepository,
        ILogger<MaintenanceService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _controllerRegistry = controllerRegistry ?? throw new ArgumentNullException(nameof(controllerRegistry));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string GetAppVersion()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version?.ToString() ?? "Unknown";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get assembly version");
            return "Unknown";
        }
    }

    public List<string> GetDbMigrationStatus()
    {
        try
        {
            var migrations = _dbContext.Database.GetAppliedMigrations().ToList();
            return migrations;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get migration status");
            return new List<string> { "Error retrieving migrations" };
        }
    }

    public async Task<MaintenanceCountsDto> GetCountsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var allMembers = await Task.Run(() => _memberRepository.GetAllMembers(), cancellationToken);
            var members = allMembers.Count;
            var activeMembers = allMembers.Count(m => m.Status == "REGULAR" || m.Status == "REGULAR-NP" || m.Status == "LIFE");
            
            var cards = await Task.Run(() =>
            {
                try
                {
                    using var connection = Db.GetConnection();
                    connection.Open();
                    using var command = new SqlCommand("SELECT COUNT(*) FROM dbo.KeyCards", connection);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch
                {
                    return 0;
                }
            }, cancellationToken);
            var reimbursements = await _dbContext.ReimbursementRequests.CountAsync(cancellationToken);
            var events = await _dbContext.ControllerEvents.CountAsync(cancellationToken);

            return new MaintenanceCountsDto
            {
                Members = members,
                ActiveMembers = activeMembers,
                Cards = cards,
                Reimbursements = reimbursements,
                ControllerEvents = events
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get counts");
            return new MaintenanceCountsDto();
        }
    }

    public async Task<bool> PingAgentAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(3));

            var result = await _controllerClient.PingAsync(cts.Token);
            if (result) return true;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Agent ping timed out");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to ping agent");
        }
        
        try
        {
            var controllers = await _controllerRegistry.GetControllersAsync(includeDoors: false, cancellationToken);
            var firstEnabled = controllers.FirstOrDefault(c => c.IsEnabled);
            if (firstEnabled != null)
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(TimeSpan.FromSeconds(3));
                var status = await _controllerClient.GetRunStatusAsync(firstEnabled.SerialNumberDisplay, cts.Token);
                return status != null;
            }
        }
        catch (Exception)
        {
            return false;
        }
        
        return false;
    }

    public async Task<Dictionary<int, bool>> PingControllersAsync(CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<int, bool>();
        
        try
        {
            var controllers = await _controllerRegistry.GetControllersAsync(includeDoors: false, cancellationToken);
            var enabledControllers = controllers.Where(c => c.IsEnabled).ToList();

            foreach (var controller in enabledControllers)
            {
                try
                {
                    using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    cts.CancelAfter(TimeSpan.FromSeconds(3));
                    
                    var status = await _controllerClient.GetRunStatusAsync(controller.SerialNumberDisplay, cts.Token);
                    result[controller.Id] = status != null;
                }
                catch (OperationCanceledException)
                {
                    result[controller.Id] = false;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to ping controller {ControllerId}", controller.Id);
                    result[controller.Id] = false;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to ping controllers");
        }

        return result;
    }
}
