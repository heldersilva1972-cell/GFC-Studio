using System.Data;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Controllers;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GFC.BlazorServer.Services;

public class DiagnosticsService
{
    private readonly GfcDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly IVersionService _versionService;
    private readonly IControllerClient _controllerClient;

    public DiagnosticsService(
        GfcDbContext dbContext,
        IConfiguration configuration,
        IHostEnvironment environment,
        IVersionService versionService,
        IControllerClient controllerClient)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _versionService = versionService ?? throw new ArgumentNullException(nameof(versionService));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
    }

    public async Task<SystemDiagnosticsInfo> GetDiagnosticsAsync(CancellationToken cancellationToken = default)
    {
        var diagnostics = new SystemDiagnosticsInfo
        {
            EnvironmentName = _environment.EnvironmentName,
            ApplicationVersion = _versionService.GetFullVersion(),
            AgentApiBaseUrl = _configuration["AgentApi:BaseUrl"] ?? string.Empty
        };

        await PopulateDatabaseInfoAsync(diagnostics, cancellationToken);
        await PopulateAgentStatusAsync(diagnostics, cancellationToken);

        return diagnostics;
    }

    private async Task PopulateDatabaseInfoAsync(SystemDiagnosticsInfo diagnostics, CancellationToken cancellationToken)
    {
        var connection = _dbContext.Database.GetDbConnection();
        diagnostics.DatabaseProvider = connection.GetType().Name;

        var shouldCloseConnection = false;
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync(cancellationToken);
                shouldCloseConnection = true;
            }

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT @@SERVERNAME, DB_NAME()";

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                diagnostics.DatabaseServer = reader.IsDBNull(0) ? "(unknown)" : reader.GetString(0);
                diagnostics.DatabaseName = reader.IsDBNull(1) ? "(unknown)" : reader.GetString(1);
            }
        }
        finally
        {
            if (shouldCloseConnection && connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
            }
        }
    }

    private async Task PopulateAgentStatusAsync(SystemDiagnosticsInfo diagnostics, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(diagnostics.AgentApiBaseUrl))
        {
            diagnostics.AgentApiReachable = null;
            diagnostics.AgentApiError = "Agent API base URL not configured.";
            return;
        }

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(3));
            var reachable = await _controllerClient.PingAsync(cts.Token);
            diagnostics.AgentApiReachable = reachable;
            if (!reachable)
            {
                diagnostics.AgentApiError = "Agent API responded but returned a non-success status.";
            }
        }
        catch (Exception ex)
        {
            diagnostics.AgentApiReachable = false;
            diagnostics.AgentApiError = ex.Message;
        }
    }
}

