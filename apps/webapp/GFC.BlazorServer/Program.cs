using GFC.BlazorServer.Auth;
using GFC.BlazorServer.Components;
using GFC.BlazorServer.Configuration;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Services.Core;
using GFC.BlazorServer.Services.Dashboard;
using GFC.BlazorServer.Services.Members;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Data.Repositories;
using GFC.Data;
using GFC.Core.Helpers;
using GFC.Core.Interfaces;
using GFC.Core.Services;
using GFC.Data.Repositories;
using GFC.BlazorServer.ProtocolCapture.Services;
using GFC.BlazorServer.Services.SimulationReplay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace GFC.BlazorServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure database connection
        var gfcConnectionString = builder.Configuration.GetConnectionString("GFC");
        if (!string.IsNullOrWhiteSpace(gfcConnectionString))
        {
            Db.ConnectionStringOverride = gfcConnectionString;
        }
        var efConnectionString = gfcConnectionString ?? builder.Configuration.GetConnectionString("GFC");
        if (string.IsNullOrWhiteSpace(efConnectionString))
        {
            throw new InvalidOperationException("Connection string 'GFC' is required for controller storage.");
        }

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddControllers();
        builder.Services.AddAuthenticationCore();
        builder.Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(AppPolicies.RequireAdmin, policy =>
                policy.RequireRole(AppRoles.Admin));
        });
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ProtectedSessionStorage>();
        builder.Services.Configure<AgentApiOptions>(builder.Configuration.GetSection("AgentApi"));
        builder.Services.Configure<DevAuthOptions>(builder.Configuration.GetSection("DevAuth"));
        builder.Services.AddHttpClient<AgentApiClient>((sp, client) =>
        {
            var opts = sp.GetRequiredService<IOptions<AgentApiOptions>>().Value;
            client.BaseAddress = new Uri(opts.BaseUrl);
        });
        builder.Services.AddDbContext<GfcDbContext>(options => options.UseSqlServer(efConnectionString));

        // BEGIN ProtocolCapture feature wiring
        // Delete the following line (and the ProtocolCapture folder) to remove the feature.
        builder.Services.AddSingleton<CaptureService>();
        // END ProtocolCapture feature wiring

        // Repository registrations
        builder.Services.AddScoped<IMemberRepository, MemberRepository>();
        builder.Services.AddScoped<IDuesRepository, DuesRepository>();
        builder.Services.AddScoped<IBoardRepository, BoardRepository>();
        builder.Services.AddScoped<IDuesWaiverRepository, DuesWaiverRepository>();
        builder.Services.AddScoped<IDuesYearSettingsRepository, DuesYearSettingsRepository>();
        builder.Services.AddScoped<IMemberKeycardRepository, MemberKeycardRepository>();
        builder.Services.AddScoped<IGlobalNoteRepository, GlobalNoteRepository>();
        builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
        builder.Services.AddScoped<IKeyCardDashboardRepository, KeyCardDashboardRepository>();
        builder.Services.AddScoped<IKeyCardRepository, KeyCardRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ILoginHistoryRepository, LoginHistoryRepository>();
        builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        builder.Services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();
        builder.Services.AddScoped<ILotteryShiftRepository, LotteryShiftRepository>();
        builder.Services.AddScoped<IPhysicalKeyRepository, PhysicalKeyRepository>();
        builder.Services.AddScoped<IUserNotificationPreferencesRepository, UserNotificationPreferencesRepository>();
        builder.Services.AddScoped<IPagePermissionRepository, PagePermissionRepository>();

        // Authentication services
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
            sp.GetRequiredService<CustomAuthenticationStateProvider>());

        // User management services
        builder.Services.AddSingleton<IPasswordPolicy, PasswordPolicy>();
        builder.Services.AddScoped<IAuditLogger, AuditLogger>();
        builder.Services.AddScoped<IUserManagementService, UserManagementService>();

        // Shared services
        builder.Services.AddSingleton<BackupConfigService>();
        builder.Services.AddSingleton<IDatabaseBackupService, GFC.BlazorServer.Services.DatabaseBackupService>();
        builder.Services.AddHostedService<GFC.BlazorServer.Services.BackupSchedulerService>();
        builder.Services.AddScoped<OverdueCalculationService>();
        builder.Services.AddScoped<MemberService>();
        builder.Services.AddScoped<IMemberActivityTimelineService, MemberActivityTimelineService>();
        builder.Services.AddScoped<IMemberQueryService, MemberQueryService>();
        builder.Services.AddScoped<IDuesInsightService, DuesInsightService>();
        builder.Services.AddScoped<INpQueueService, NpQueueService>();
        builder.Services.AddScoped<ILifeEligibilityService, LifeEligibilityService>();
        builder.Services.AddScoped<IDashboardService, DashboardService>();
        builder.Services.AddScoped<IDashboardMetricsService, DashboardMetricsService>();
        builder.Services.AddScoped<ICardReaderProfileService, CardReaderProfileService>();
        builder.Services.AddScoped<ICardEligibilityService, CardEligibilityService>();
        builder.Services.AddScoped<KeyCardService>();
        builder.Services.AddScoped<IMemberHistoryService, MemberHistoryService>();
        builder.Services.AddScoped<KeyCardAdminService>();
        builder.Services.AddScoped<IBoardTermConfirmationService, BoardTermConfirmationService>();
        builder.Services.AddScoped<ILotteryShiftService, LotteryShiftService>();
        builder.Services.AddScoped<IDataExportService, DataExportService>();
        builder.Services.AddScoped<IPhysicalKeyService, PhysicalKeyService>();
        builder.Services.AddScoped<IVersionService, VersionService>();
        builder.Services.AddScoped<DiagnosticsService>();
        builder.Services.AddScoped<ControllerRegistryService>();
        builder.Services.AddScoped<ControllerEventService>();
        builder.Services.AddScoped<ISimulationTraceService, SimulationTraceService>();
        builder.Services.AddScoped<IAccessSimulationScenarioService, AccessSimulationScenarioService>();
        builder.Services.AddScoped<CommandInfoService>();
        builder.Services.AddScoped<IScheduleService, ScheduleService>();
        builder.Services.AddScoped<IMaintenanceService, GFC.BlazorServer.Services.Maintenance.MaintenanceService>();
        builder.Services.AddScoped<IDoorConfigService, DoorConfigService>();
        builder.Services.AddScoped<DoorConfigSyncService>();
        builder.Services.AddScoped<AutoOpenAndAdvancedModesService>();
        builder.Services.AddScoped<IMemberAccessService, MemberAccessService>();
        builder.Services.AddScoped<IMemberPrivilegeSyncService, MemberPrivilegeSyncService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.ISimulationGuard, GFC.BlazorServer.Services.Controllers.SimulationGuard>();
        builder.Services.AddScoped<DuesService>();
        builder.Services.AddScoped<WaiverService>();
        builder.Services.AddScoped<KeyHistoryService>();
        builder.Services.AddScoped<ReceiptStorageService>();
        builder.Services.AddScoped<ReimbursementService>();
        builder.Services.AddScoped<IReplayService, ReplayService>();
        builder.Services.AddScoped<ThemeService>();
        
        // BEGIN Simulation/Real controller toggle wiring
        builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
        builder.Services.AddScoped<IControllerModeProvider, ControllerModeProvider>();
        
        // Register simulation state store as singleton (shared across all requests)
        builder.Services.AddSingleton<GFC.BlazorServer.Services.Controllers.SimControllerStateStore>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Simulation.ISimulationStateEngine, GFC.BlazorServer.Services.Simulation.SimulationStateEngine>();
        
        // Register simulation controller options
        builder.Services.Configure<GFC.BlazorServer.Configuration.SimControllerOptions>(
            builder.Configuration.GetSection("SimController"));
        builder.Services.Configure<AccessControlSimulationOptions>(
            builder.Configuration.GetSection("AccessControlSimulation"));
        
        // Register simulation services
        builder.Services.AddScoped<GFC.BlazorServer.Services.Simulation.ISimulationModeService, GFC.BlazorServer.Services.Simulation.SimulationModeService>();
        builder.Services.AddSingleton<GFC.BlazorServer.Services.Simulation.SimulationPresetService>();
        
        // Register simulation engine service (both as hosted service and as control service)
        builder.Services.AddSingleton<GFC.BlazorServer.Services.Simulation.SimControllerEngineService>();
        builder.Services.AddSingleton<GFC.BlazorServer.Services.Simulation.ISimulationEngineControlService>(sp => 
            sp.GetRequiredService<GFC.BlazorServer.Services.Simulation.SimControllerEngineService>());
        builder.Services.AddHostedService<GFC.BlazorServer.Services.Simulation.SimControllerEngineService>(sp => 
            sp.GetRequiredService<GFC.BlazorServer.Services.Simulation.SimControllerEngineService>());
        
        // Register concrete controller clients
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.SimulationControllerClient>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.RealControllerClient>();
        
        // Register Mengqi controller clients used by controller-test tooling
        builder.Services.AddScoped<GFC.BlazorServer.Services.SimulationControllerClient>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.RealControllerClient>();

        // Register Dynamic proxies
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.DynamicControllerClient>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.DynamicMengqiControllerClient>();
        
        // Register IControllerClient that dynamically resolves based on UseRealControllers setting
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.IControllerClient>(sp =>
            sp.GetRequiredService<GFC.BlazorServer.Services.Controllers.DynamicControllerClient>());
        
        // Register ControllerNetworkConfigService AFTER IControllerClient is registered
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.ControllerNetworkConfigService>();
        
        // Wire IMengqiControllerClient to dynamically resolve based on UseRealControllers setting
        builder.Services.AddScoped<IMengqiControllerClient>(sp =>
            sp.GetRequiredService<GFC.BlazorServer.Services.DynamicMengqiControllerClient>());
        // END Simulation/Real controller toggle wiring
        
        builder.Services.AddScoped<ControllerTestService>();

        var app = builder.Build();

        var accessControlOptions = app.Services.GetRequiredService<IOptions<AccessControlSimulationOptions>>().Value;
        app.Logger.LogInformation("Access Control Simulation Mode: {Mode}", accessControlOptions.UseAccessControlSimulation ? "ON" : "OFF");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        if (app.Environment.IsDevelopment())
        {
            app.UseDevAuthAutoAdmin();
        }

        // Apply pending migrations at startup and surface any errors
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<GfcDbContext>();
                dbContext.Database.Migrate();
                Console.WriteLine(">>> DB MIGRATION: Completed successfully for GfcDbContext.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> DB MIGRATION ERROR for GfcDbContext:");
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        // Initialize default admin user if it doesn't exist.
        // NOTE: Production deployments must set InitialAdminPassword (env/appsettings)
        // before first run and change the password via UI after the initial login.
        try
        {
            using var scope = app.Services.CreateScope();
            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = app.Logger;

            const string seedUsername = "admin";
            var adminUser = userRepo.GetByUsername(seedUsername);
            if (adminUser == null)
            {
                var configuredPassword = configuration["InitialAdminPassword"];
                var trimmedPassword = configuredPassword?.Trim();

                if (string.IsNullOrWhiteSpace(trimmedPassword))
                {
                    logger.LogWarning("Initial admin user was NOT created because InitialAdminPassword is missing. Set InitialAdminPassword and restart, or create an admin manually.");
                }
                else
                {
                    var passwordPolicy = scope.ServiceProvider.GetRequiredService<IPasswordPolicy>();
                    var validationResult = passwordPolicy.Validate(seedUsername, trimmedPassword);
                    if (!validationResult.IsValid)
                    {
                        logger.LogWarning("Initial admin user was NOT created because InitialAdminPassword failed the password policy. {Reason}", string.IsNullOrWhiteSpace(validationResult.ErrorMessage) ? passwordPolicy.RequirementSummary : validationResult.ErrorMessage);
                    }
                    else
                    {
                        var passwordHash = GFC.Core.Helpers.PasswordHelper.HashPassword(trimmedPassword);
                        var newAdmin = new GFC.Core.Models.AppUser
                        {
                            Username = seedUsername,
                            PasswordHash = passwordHash,
                            IsAdmin = true,
                            IsActive = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = "System"
                        };
                        var createdAdminId = userRepo.CreateUser(newAdmin);
                        var auditLogger = scope.ServiceProvider.GetRequiredService<IAuditLogger>();
                        auditLogger.LogAdminCreation(null, createdAdminId, seedUsername, memberId: null);
                        logger.LogInformation("Initial admin user 'admin' was created using the password supplied via configuration. Change this password immediately after first login.");
                    }
                }
            }
        }
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) // Invalid object name
        {
            // Tables don't exist yet - user needs to run CreateAuthTables.sql
            // This is expected on first run before the SQL script is executed
        }
        catch
        {
            // Silently fail if database isn't ready yet - will be created on first run
        }

        app.MapControllers();
        app.MapGet("/simulation/replay/{sessionId}",
            async (string sessionId, IReplayService replay, CancellationToken ct) =>
            {
                var steps = await replay.BuildReplayStepsAsync(sessionId, ct);
                return Results.Ok(steps);
            });
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}
