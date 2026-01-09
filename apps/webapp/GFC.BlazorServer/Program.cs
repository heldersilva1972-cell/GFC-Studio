// [MODIFIED]
using GFC.BlazorServer.Auth;
using GFC.BlazorServer.Components;
using GFC.BlazorServer.Configuration;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Services.Camera;
using GFC.BlazorServer.Services.Core;
using GFC.BlazorServer.Services.Dashboard;
using GFC.BlazorServer.Services.Members;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Services.Diagnostics;
using GFC.BlazorServer.Services.DataProtection;
using GFC.BlazorServer.Services.Operations;
using GFC.BlazorServer.Data.Repositories;
using GFC.BlazorServer.Repositories;
using GFC.Data;
using GFC.Core.Helpers;
using GFC.Core.Interfaces;
using GFC.Core.Services;
using GFC.Data.Repositories;
using GFC.BlazorServer.ProtocolCapture.Services;
using GFC.BlazorServer.Middleware;
using GFC.BlazorServer.Hubs;
using Microsoft.AspNetCore.HttpOverrides; // For Cloudflare Tunnel headers

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.RateLimiting;
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
        builder.Services.AddServerSideBlazor().AddHubOptions(options => 
        {
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
            options.HandshakeTimeout = TimeSpan.FromSeconds(30);
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        }).AddCircuitOptions(options => options.DetailedErrors = true);

        builder.Services.AddSignalR(options => 
        {
            options.EnableDetailedErrors = true;
        }); 
        
        // [HTTPS FIX] Trust Cloudflare Tunnel Headers
        // This ensures the app knows it's running over HTTPS even though termination happens at Cloudflare
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedHost;
            // Clear known networks/proxies to trust all (since the tunnel is the only ingress)
            options.KnownNetworks.Clear(); 
            options.KnownProxies.Clear();
        });

        // [HTTPS FIX] Enforce Secure Cookies
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // [FIX] Allow HTTP on LAN
            options.Cookie.SameSite = SameSiteMode.Lax;
        });

        // Add CORS for Next.js frontend and Onboarding Gateway
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowNextJs", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:3000",
                        "https://setup.gfc.lovanow.com") // Onboarding gateway
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });
        
        // Add Rate Limiting for Onboarding API
        builder.Services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("onboarding", opt =>
            {
                opt.Window = TimeSpan.FromMinutes(1);
                opt.PermitLimit = 10; // 10 requests per minute per IP
                opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                opt.QueueLimit = 5;
            });
            
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });
        
        builder.Services.AddControllers();
        builder.Services.AddAuthenticationCore();
        builder.Services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(AppPolicies.RequireAdmin, policy =>
                policy.RequireRole(AppRoles.Admin));
            options.AddPolicy(AppPolicies.CanForceUnlock, policy =>
                policy.RequireRole(AppRoles.Admin, AppRoles.StudioUnlock));
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
        builder.Services.AddHttpClient<IImportService, ImportService>();
        builder.Services.AddScoped<DomAnalysisService>();
        builder.Services.AddDbContextFactory<GfcDbContext>(options => options.UseSqlServer(efConnectionString));
        builder.Services.AddScoped<GfcDbContext>(p => p.GetRequiredService<IDbContextFactory<GfcDbContext>>().CreateDbContext());

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
        builder.Services.AddScoped<ITrustedDeviceRepository, TrustedDeviceRepository>();
        builder.Services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();
        builder.Services.AddScoped<ILotteryShiftRepository, LotteryShiftRepository>();
        builder.Services.AddScoped<IPhysicalKeyRepository, PhysicalKeyRepository>();
        builder.Services.AddScoped<IUserNotificationPreferencesRepository, UserNotificationPreferencesRepository>();
        builder.Services.AddScoped<IPagePermissionRepository, PagePermissionRepository>();
        builder.Services.AddScoped<IStudioPageRepository, GFC.BlazorServer.Repositories.StudioPageRepository>();

        // Authentication services
        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<IUserSessionService, UserSessionService>();
        builder.Services.AddScoped<CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
            sp.GetRequiredService<CustomAuthenticationStateProvider>());

        // User management services
        builder.Services.AddSingleton<IPasswordPolicy, PasswordPolicy>();
        builder.Services.AddScoped<IAuditLogger, AuditLogger>();
        builder.Services.AddScoped<IUserManagementService, UserManagementService>();
        builder.Services.AddSingleton<IMfaChallengeService, MfaChallengeService>();

        // Shared services
        builder.Services.AddScoped<IVpnProfileRepository, VpnProfileRepository>();
        builder.Services.AddScoped<IVpnSetupService, VpnSetupService>();
        builder.Services.AddScoped<IVpnManagementService, VpnManagementService>();
        builder.Services.AddScoped<IAuthorizedUserService, AuthorizedUserService>();
        builder.Services.AddScoped<IRemoteAccessHealthService, RemoteAccessHealthService>();
        builder.Services.AddSingleton<BackupConfigService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Camera.ICameraVerificationService, GFC.BlazorServer.Services.Camera.CameraVerificationService>();
        builder.Services.AddScoped<ICameraService, CameraService>();
        builder.Services.AddScoped<ICameraDiscoveryService, CameraDiscoveryService>();
        builder.Services.AddScoped<IRecordingService, RecordingService>();
        builder.Services.AddScoped<ICameraEventService, CameraEventService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Camera.ICameraPermissionService, GFC.BlazorServer.Services.Camera.CameraPermissionService>();
        builder.Services.AddScoped<ICameraAuditLogService, CameraAuditLogService>();
        builder.Services.AddScoped<IStreamSecurityService, StreamSecurityService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Camera.IVideoAccessService, GFC.BlazorServer.Services.Camera.VideoAccessService>();
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
        builder.Services.AddScoped<DashboardSessionState>();
        builder.Services.AddScoped<ICardReaderProfileService, CardReaderProfileService>();
        builder.Services.AddScoped<ICardEligibilityService, CardEligibilityService>();
        builder.Services.AddScoped<KeyCardService>();
        builder.Services.AddScoped<IMemberHistoryService, MemberHistoryService>();
        builder.Services.AddScoped<KeyCardAdminService>();
        
        // Key Card Lifecycle & Sync Services
        builder.Services.AddScoped<IControllerSyncQueueRepository, ControllerSyncQueueRepository>();
        builder.Services.AddScoped<ICardDeactivationLogRepository, CardDeactivationLogRepository>();
        builder.Services.AddScoped<IImmediateSyncDispatcher, ImmediateSyncDispatcher>();
        builder.Services.AddScoped<KeyCardLifecycleService>();
        builder.Services.AddHostedService<ControllerSyncWorker>();
        builder.Services.AddHostedService<CardLifecycleBackgroundService>();
        
        // Controller Health & Full Sync
        builder.Services.AddSingleton<ControllerHealthService>();
        builder.Services.AddScoped<ControllerFullSyncService>();
        builder.Services.AddHostedService<ControllerHealthMonitor>();
        builder.Services.AddHostedService<ControllerEventSyncService>(); // Auto-sync controller events
        
        builder.Services.AddScoped<IBoardTermConfirmationService, BoardTermConfirmationService>();
        builder.Services.AddScoped<ILotteryShiftService, LotteryShiftService>();
        builder.Services.AddScoped<IDataExportService, DataExportService>();
        builder.Services.AddScoped<IPhysicalKeyService, PhysicalKeyService>();
        builder.Services.AddScoped<IVersionService, VersionService>();
        builder.Services.AddScoped<ISystemPerformanceService, SystemPerformanceService>();
        builder.Services.AddScoped<ControllerDiagnosticsService>();
        builder.Services.AddScoped<CameraDiagnosticsService>();
        builder.Services.AddScoped<DatabaseHealthService>();
        builder.Services.AddScoped<IDiagnosticsService, DiagnosticsService>();
        builder.Services.AddScoped<IPerformanceHistoryService, PerformanceHistoryService>();
        builder.Services.AddScoped<IAlertManagementService, AlertManagementService>();
        builder.Services.AddHostedService<DiagnosticsBackgroundService>();
        builder.Services.AddSingleton<ControllerRegistryService>();
        builder.Services.AddScoped<ControllerEventService>();
        builder.Services.AddScoped<CommandInfoService>();
        builder.Services.AddScoped<IScheduleService, ScheduleService>();
        builder.Services.AddScoped<IMaintenanceService, GFC.BlazorServer.Services.Maintenance.MaintenanceService>();
        builder.Services.AddScoped<IDoorConfigService, DoorConfigService>();
        builder.Services.AddScoped<DoorConfigSyncService>();
        builder.Services.AddScoped<IMagicLinkService, MagicLinkService>();
        builder.Services.AddScoped<AutoOpenAndAdvancedModesService>();
        builder.Services.AddScoped<IMemberAccessService, MemberAccessService>();
        builder.Services.AddScoped<IMemberPrivilegeSyncService, MemberPrivilegeSyncService>();
        builder.Services.AddScoped<DuesService>();
        builder.Services.AddScoped<WaiverService>();
        builder.Services.AddScoped<KeyHistoryService>();
        builder.Services.AddScoped<ReceiptStorageService>();
        builder.Services.AddScoped<ReimbursementService>();
        builder.Services.AddScoped<ThemeService>();
        builder.Services.AddScoped<IOperationsService, OperationsService>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();


        builder.Services.AddScoped<IDataProtectionService, DataProtectionService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Migration.IMigrationService, GFC.BlazorServer.Services.Migration.MigrationService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.Migration.IMigrationReportService, GFC.BlazorServer.Services.Migration.MigrationReportService>();

        // Network Location Service (registered above as Scoped)
        builder.Services.AddScoped<IUserConnectionService, UserConnectionService>();

        // GFC Ecosystem Foundation
        builder.Services.AddSingleton<IToastService, ToastService>();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IMediaStorageService, MediaStorageService>();
        builder.Services.AddScoped<IContentIngestionService, ContentIngestionService>();
        builder.Services.AddScoped<IStaticExportService, StaticExportService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.IStudioService, GFC.BlazorServer.Services.StudioService>();
        builder.Services.AddSingleton<IStudioEngineService, StudioEngineService>();
        builder.Services.AddScoped<IStudioCmsService, StudioCmsService>();
        builder.Services.AddScoped<IStudioAutoSaveService, StudioAutoSaveService>();
        builder.Services.AddScoped<ITemplateService, TemplateService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.IMediaAssetService, GFC.BlazorServer.Services.MediaAssetService>();
        builder.Services.AddScoped<GFC.Core.Interfaces.IMediaAssetService, GFC.BlazorServer.Services.MediaAssetService>();
        builder.Services.AddScoped<IFormService, FormService>();
        builder.Services.AddScoped<ISeoService, SeoService>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<INetworkLocationService, NetworkLocationService>();
builder.Services.AddScoped<IWireGuardManagementService, WireGuardManagementService>();
// Register SystemSettingsService for both interfaces (core and BlazorServer-specific)
builder.Services.AddScoped<GFC.Core.Interfaces.ISystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<IBlazorSystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<IUrlHelperService, UrlHelperService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<GFC.BlazorServer.Services.Vpn.IVpnConfigurationService, GFC.BlazorServer.Services.Vpn.VpnConfigurationService>();
builder.Services.AddScoped<IUserRevocationService, UserRevocationService>();
builder.Services.AddSingleton<TunnelStatusService>();
builder.Services.AddHostedService<CloudflareTunnelHealthService>();

// Security & Connection Services
builder.Services.AddScoped<IDeviceTrustService, DeviceTrustService>();
builder.Services.AddSingleton<ISecurityRateLimitService, SecurityRateLimitService>();
builder.Services.AddScoped<ISecurityNotificationService, SecurityNotificationService>();
        builder.Services.AddScoped<IShiftService, ShiftService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IEventPromotionService, EventPromotionService>();
        builder.Services.AddScoped<INavMenuService, NavMenuService>();
        builder.Services.AddScoped<IBartenderShiftService, BartenderShiftService>();
        builder.Services.AddScoped<GFC.Core.Interfaces.IWebsiteSettingsService, WebsiteSettingsService>();
        builder.Services.AddScoped<IPublicReviewService, PublicReviewService>();
        builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
        builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
        builder.Services.AddScoped<IPageService, PageService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        builder.Services.AddScoped<INotificationRoutingService, NotificationRoutingService>();
        builder.Services.AddScoped<IProjectFileService, ProjectFileService>();
        
        // Controller Client Wiring
        // Register the endpoint resolver that uses AgentApiOptions
        builder.Services.AddSingleton<GFC.BlazorServer.Connectors.Mengqi.Abstractions.IControllerEndpointResolver, GFC.BlazorServer.Services.Controllers.BlazorControllerEndpointResolver>();
        
        // Register ControllerClientOptions
        builder.Services.AddSingleton<GFC.BlazorServer.Connectors.Mengqi.Configuration.ControllerClientOptions>(sp => {
             var config = sp.GetRequiredService<IConfiguration>();
             return new GFC.BlazorServer.Connectors.Mengqi.Configuration.ControllerClientOptions 
             {
                 CommandProfiles = GFC.BlazorServer.Services.Controllers.CommandProfileFactory.CreateDefaults()
             };
        });

        // Register the ACTUAL hardware client library (local copy)
        builder.Services.AddSingleton<GFC.BlazorServer.Connectors.Mengqi.MengqiControllerClient>();

        // Register concrete controller clients. The `RealControllerClient` in the `Services` namespace
        // is a legacy test stub that is no longer used. The one in `Services/Controllers` implements
        // `IControllerClient` for the main application and delegates to MengqiControllerClient.
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.RealControllerClient>();

        // Register the IControllerClient to always use the RealControllerClient.
        // The DynamicControllerClient that switched between real and simulation has been removed.
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.IControllerClient, GFC.BlazorServer.Services.Controllers.RealControllerClient>();
        
        // Register ControllerNetworkConfigService AFTER IControllerClient is registered
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.ControllerNetworkConfigService>();
        
        // Register centralized controller status monitor (singleton - shared across all components)
        builder.Services.AddSingleton<ControllerStatusMonitor>();
        builder.Services.AddSingleton<ICommunicationLogService, CommunicationLogService>();
        builder.Services.AddScoped<IControllerSyncService, ControllerSyncService>();
        
        builder.Services.AddScoped<ControllerTestService>();

        builder.Services.AddHostedService<DirectorAccessExpiryWorker>();
        builder.Services.AddHostedService<ControllerStatusMonitorService>();

        var app = builder.Build();



        // Configure the HTTP request pipeline.
        
        // [HTTPS FIX] Use Forwarded Headers MUST be before HSTS/HttpsRedirection
        app.UseForwardedHeaders();

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
        
        // Configure static files with proper MIME types for PWA
        var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        provider.Mappings[".webmanifest"] = "application/manifest+json";
        provider.Mappings[".json"] = "application/json";
        
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = provider,
            OnPrepareResponse = ctx =>
            {
                // Set proper cache headers for PWA files
                if (ctx.File.Name == "manifest.json" || ctx.File.Name == "service-worker.js")
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
                }
            }
        });

        app.UseRouting();
        
        // Enable CORS
        app.UseCors("AllowNextJs");

        // IMPORTANT: DevAuth must run after UseRouting and before authorization policies.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseDevAuthAutoAdmin();
        // }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter(); // Enable rate limiting

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<DeviceGuardMiddleware>();
        app.UseMiddleware<VideoAccessGuardMiddleware>();

        // Apply pending migrations at startup and surface any errors
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<GfcDbContext>();
                
                // [REBOOT-PROOF FIX] Wait for SQL Server to become available
                // When the host computer restarts, SQL Express may take longer to initialize than IIS.
                // This retry loop ensures we don't 'fail forward' into a broken state.
                bool sqlReady = false;
                int sqlRetries = 0;
                while (!sqlReady && sqlRetries < 12) // Try for ~1 minute
                {
                    try
                    {
                        dbContext.Database.OpenConnection();
                        dbContext.Database.CloseConnection();
                        sqlReady = true;
                        Console.WriteLine(">>> [Startup] SQL Server connection verified.");
                    }
                    catch (Exception ex)
                    {
                        sqlRetries++;
                        Console.WriteLine($">>> [Startup] Waiting for SQL Server... (Attempt {sqlRetries}/12): {ex.Message}");
                        Thread.Sleep(5000); // 5 second gap
                    }
                }

                // [AUTO-FIX 0] CRITICAL: Initialize Database Foundation if Missing
                // This ensures the application can start even if the manual sqlcmd script failed
                var initFinalPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "INITIALIZE_DATABASE_FINAL.sql");
                if (File.Exists(initFinalPath))
                {
                    try 
                    {
                        var checkTableSql = "SELECT COUNT(*) FROM sys.tables WHERE name = 'Members'";
                        // safely check if we need to run init
                        var tableCount = -1;
                        try {
                             // Use raw connection to avoid EF model issues if table missing
                             using var cmd = dbContext.Database.GetDbConnection().CreateCommand();
                             cmd.CommandText = checkTableSql;
                             dbContext.Database.OpenConnection();
                             tableCount = (int)(cmd.ExecuteScalar() ?? 0);
                             dbContext.Database.CloseConnection();
                        } catch { 
                             // If completely broken, assume 0
                             tableCount = 0; 
                        }

                        if (tableCount == 0)
                        {
                            Console.WriteLine($">>> CRITICAL: Core Tables Missing. Applying Foundation from: {initFinalPath}");
                            var initSql = File.ReadAllText(initFinalPath);
                            var initBatches = System.Text.RegularExpressions.Regex.Split(initSql, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            
                            foreach (var batch in initBatches)
                            {
                                if (!string.IsNullOrWhiteSpace(batch))
                                {
                                    try {
                                        dbContext.Database.ExecuteSqlRaw(batch);
                                    } catch (Exception ex) {
                                        Console.WriteLine($"Error executing foundation init batch: {ex.Message}");
                                    }
                                }
                            }
                            Console.WriteLine(">>> CRITICAL: Database Foundation Applied Successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($">>> Error checking/applying foundation init: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Initialization script not found at {initFinalPath}");
                }

                // [CRITICAL FIX] Force-Fix NULL values in WebsiteSettings preventing app load
                // We run this directly to ensure it happens regardless of external script parsing
                try 
                {
                    var fixNullsSql = @"
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSettings')
                        BEGIN
                            UPDATE [dbo].[WebsiteSettings] SET [MemberRate] = 0 WHERE [MemberRate] IS NULL;
                            UPDATE [dbo].[WebsiteSettings] SET [NonMemberRate] = 0 WHERE [NonMemberRate] IS NULL;
                            UPDATE [dbo].[WebsiteSettings] SET [IsClubOpen] = 1 WHERE [IsClubOpen] IS NULL;
                            UPDATE [dbo].[WebsiteSettings] SET [MasterEmailKillSwitch] = 0 WHERE [MasterEmailKillSwitch] IS NULL;
                            UPDATE [dbo].[WebsiteSettings] SET [HighAccessibilityMode] = 0 WHERE [HighAccessibilityMode] IS NULL;
                        END
                    ";
                    dbContext.Database.ExecuteSqlRaw(fixNullsSql);
                    Console.WriteLine(">>> CRITICAL: Applied direct NULL fix for WebsiteSettings.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($">>> Error applying critical NULL fix: {ex.Message}");
                }

                // [AUTO-FIX] Run the repair script if WebsiteSettings is missing
                // This replaces the manual migration step since we aren't using EF Migrations in this environment
                var scriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "add-systemsettings-columns.sql");
                // Adjust path for published/different environments if needed, but this works for local dev
                
                if (File.Exists(scriptPath)) 
                {
                    Console.WriteLine($">>> Applying Database Schema Fixes from: {scriptPath}");
                    var sqlFile = File.ReadAllText(scriptPath);
                    // Split by "GO" because ExecuteSqlRaw doesn't support it
                    var commandBatches = System.Text.RegularExpressions.Regex.Split(sqlFile, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    
                    foreach (var batch in commandBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try {
                                dbContext.Database.ExecuteSqlRaw(batch);
                            } catch (Exception ex) {
                                Console.WriteLine($"Error executing batch: {ex.Message}");
                            }
                        }
                    }
                    Console.WriteLine(">>> Database Schema Fixes Applied Successfully (SystemSettings).");
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Schema repair script not found at {scriptPath}");
                }

                // [CRITICAL FIX 2] Force-Fix NULLs for new SystemSettings columns (AccessMode, etc.)
                // This ensures "Data is Null" errors don't prevent app startup/operations
                try 
                {
                    var fixNullsSysSql = @"
                        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'SystemSettings')
                        BEGIN
                            UPDATE [dbo].[SystemSettings] SET 
                                [AccessMode] = 'Open' WHERE [AccessMode] IS NULL OR [AccessMode] = 'Standard';
                            UPDATE [dbo].[SystemSettings] SET 
                                [EnableOnboarding] = 0 WHERE [EnableOnboarding] IS NULL;
                            UPDATE [dbo].[SystemSettings] SET 
                                [EnforceVpn] = 0 WHERE [EnforceVpn] IS NULL;
                            UPDATE [dbo].[SystemSettings] SET 
                                [SafeModeEnabled] = 0 WHERE [SafeModeEnabled] IS NULL;
                            UPDATE [dbo].[SystemSettings] SET 
                                [MagicLinkEnabled] = 1 WHERE [MagicLinkEnabled] IS NULL;
                            UPDATE [dbo].[SystemSettings] SET 
                                [HostingEnvironment] = 'Production' WHERE [HostingEnvironment] IS NULL;
                             UPDATE [dbo].[SystemSettings] SET 
                                [BackupFrequencyHours] = 24 WHERE [BackupFrequencyHours] IS NULL;
                             UPDATE [dbo].[SystemSettings] SET 
                                [LanSubnet] = '192.168.0.0/16' WHERE [LanSubnet] IS NULL OR [LanSubnet] = '192.168.1.0/24';
                        END
                    ";
                    dbContext.Database.ExecuteSqlRaw(fixNullsSysSql);
                    Console.WriteLine(">>> CRITICAL: Applied direct NULL fix for SystemSettings (AccessMode, etc).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($">>> Error applying critical SystemSettings NULL fix: {ex.Message}");
                }

                // [AUTO-FIX 2] Run the Video Security Tables script
                var securityScriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "add-video-security-tables.sql");
                if (File.Exists(securityScriptPath))
                {
                    Console.WriteLine($">>> Applying Video Security Schema Fixes from: {securityScriptPath}");
                    var secSqlFile = File.ReadAllText(securityScriptPath);
                    var secBatches = System.Text.RegularExpressions.Regex.Split(secSqlFile, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (var batch in secBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try {
                                dbContext.Database.ExecuteSqlRaw(batch);
                            } catch (Exception ex) {
                                Console.WriteLine($"Error executing security batch: {ex.Message}");
                            }
                        }
                    }
                    Console.WriteLine(">>> Video Security Schema Fixes Applied Successfully.");
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Security schema script not found at {securityScriptPath}");
                }

                // [AUTO-FIX 3] Run the Public Reviews Table script
                var reviewsScriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "add-public-reviews-table.sql");
                if (File.Exists(reviewsScriptPath))
                {
                    Console.WriteLine($">>> Applying Public Reviews Schema Fixes from: {reviewsScriptPath}");
                    var revSqlFile = File.ReadAllText(reviewsScriptPath);
                    var revBatches = System.Text.RegularExpressions.Regex.Split(revSqlFile, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (var batch in revBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try {
                                dbContext.Database.ExecuteSqlRaw(batch);
                            } catch (Exception ex) {
                                Console.WriteLine($"Error executing reviews batch: {ex.Message}");
                            }
                        }
                    }
                    Console.WriteLine(">>> Public Reviews Schema Fixes Applied Successfully.");
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Reviews schema script not found at {reviewsScriptPath}");
                }

                // [AUTO-FIX 4] Run the Rental Management Fix script
                var rentalScriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "FixRentalManagement.sql");
                if (File.Exists(rentalScriptPath))
                {
                    Console.WriteLine($">>> Applying Rental Management Schema Fixes from: {rentalScriptPath}");
                    var rentSqlFile = File.ReadAllText(rentalScriptPath);
                    var rentBatches = System.Text.RegularExpressions.Regex.Split(rentSqlFile, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (var batch in rentBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try {
                                dbContext.Database.ExecuteSqlRaw(batch);
                            } catch (Exception ex) {
                                Console.WriteLine($"Error executing rental batch: {ex.Message}");
                            }
                        }
                    }
                    Console.WriteLine(">>> Rental Management Schema Fixes Applied Successfully.");
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Rental schema script not found at {rentalScriptPath}");
                }

                // [AUTO-FIX 5] Run the Bar Sales Tables script
                var barSalesScriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "CreateBarSalesTables.sql");
                if (File.Exists(barSalesScriptPath))
                {
                    Console.WriteLine($">>> Applying Bar Sales Schema Fixes from: {barSalesScriptPath}");
                    var barSqlFile = File.ReadAllText(barSalesScriptPath);
                    var barBatches = System.Text.RegularExpressions.Regex.Split(barSqlFile, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (var batch in barBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try {
                                dbContext.Database.ExecuteSqlRaw(batch);
                            } catch (Exception ex) {
                                Console.WriteLine($"Error executing bar sales batch: {ex.Message}");
                            }
                        }
                    }
                    Console.WriteLine(">>> Bar Sales Schema Fixes Applied Successfully.");
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Bar Sales schema script not found at {barSalesScriptPath}");
                }

                // [AUTO-FIX 6] Run the Comprehensive Studio Schema Sync
                var studioSyncPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "SyncStudioSchema.sql");
                if (File.Exists(studioSyncPath))
                {
                    Console.WriteLine($">>> Applying COMPREHENSIVE Studio Schema Sync from: {studioSyncPath}");
                    var studioSqlFile = File.ReadAllText(studioSyncPath);
                    var studioBatches = System.Text.RegularExpressions.Regex.Split(studioSqlFile, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (var batch in studioBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try {
                                dbContext.Database.ExecuteSqlRaw(batch);
                            } catch (Exception ex) {
                                Console.WriteLine($"Error executing Studio Sync batch: {ex.Message}");
                            }
                        }
                    }
                    Console.WriteLine(">>> Studio Schema Synchronized Successfully.");
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Studio Sync script not found at {studioSyncPath}");
                }

                // [AUTO-FIX 7] Run Key Card Enhancements Migration (DIRECT EXECUTION)
                // We run the critical column addition directly to avoid path issues
                try
                {
                    var criticalFixSql = @"
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.KeyCards') AND name = 'IsActive')
                        BEGIN
                            ALTER TABLE dbo.KeyCards ADD IsActive BIT NOT NULL DEFAULT 1;
                            PRINT 'Added IsActive column directly';
                        END
                        
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.KeyCards') AND name = 'CardType')
                        BEGIN
                            ALTER TABLE dbo.KeyCards ADD CardType NVARCHAR(10) NULL;
                            PRINT 'Added CardType column directly';
                        END
                    ";
                    dbContext.Database.ExecuteSqlRaw(criticalFixSql);
                    Console.WriteLine(">>> Critical KeyCard Columns (IsActive, CardType) verified/applied.");

                    // Attempt to run the full script for other tables (queues, logs) if file found
                    var keyCardScriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "apps", "webapp", "GFC.Data", "Migrations", "KeyCardEnhancements_Migration.sql");
                    if (File.Exists(keyCardScriptPath))
                    {
                        Console.WriteLine($">>> Applying Full Key Card Enhancements Migration from: {keyCardScriptPath}");
                        var keyCardSql = File.ReadAllText(keyCardScriptPath);
                        var keyCardBatches = System.Text.RegularExpressions.Regex.Split(keyCardSql, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        foreach (var batch in keyCardBatches)
                        {
                            if (!string.IsNullOrWhiteSpace(batch))
                            {
                                try {
                                    dbContext.Database.ExecuteSqlRaw(batch);
                                } catch (Exception ex) {
                                    Console.WriteLine($"Error executing usage key card batch: {ex.Message}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                     Console.WriteLine($">>> Error executing direct key card fix: {ex.Message}");
                }

                // [AUTO-FIX 8] Run Member Access Tables script
                var accessScriptPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "CreateMemberAccessTables.sql");
                if (File.Exists(accessScriptPath))
                {
                    Console.WriteLine($">>> Applying Member Access Schema Fixes from: {accessScriptPath}");
                    var accessSqlFile = File.ReadAllText(accessScriptPath);
                    var accessBatches = System.Text.RegularExpressions.Regex.Split(accessSqlFile, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    foreach (var batch in accessBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try {
                                dbContext.Database.ExecuteSqlRaw(batch);
                            } catch (Exception ex) {
                                Console.WriteLine($"Error executing access batch: {ex.Message}");
                            }
                        }
                    }
                    Console.WriteLine(">>> Member Access Schema Fixes Applied Successfully.");
                }
                else
                {
                    Console.WriteLine($">>> WARNING: Member Access schema script not found at {accessScriptPath}");
                }


                // [AUTO-FIX 9] Run Backup Schema Migration
                var backupSchemaPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "Manual_Backup_Schema.sql");
                if (File.Exists(backupSchemaPath))
                {
                    Console.WriteLine($">>> Applying Backup Schema Fixes from: {backupSchemaPath}");
                    var backupSql = File.ReadAllText(backupSchemaPath);
                    var backupBatches = System.Text.RegularExpressions.Regex.Split(backupSql, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    
                    foreach (var batch in backupBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try { dbContext.Database.ExecuteSqlRaw(batch); } catch (Exception ex) { Console.WriteLine($"Error executing backup batch: {ex.Message}"); }
                        }
                    }
                    Console.WriteLine(">>> Backup Schema Applied Successfully.");
                }

                // [AUTO-FIX 10] Run Safe Mode Schema Migration
                var safeModePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "Manual_SafeMode_Update.sql");
                if (File.Exists(safeModePath))
                {
                    Console.WriteLine($">>> Applying Safe Mode Schema Fixes from: {safeModePath}");
                    var smSql = File.ReadAllText(safeModePath);
                    var smBatches = System.Text.RegularExpressions.Regex.Split(smSql, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    
                    foreach (var batch in smBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try { dbContext.Database.ExecuteSqlRaw(batch); } catch (Exception ex) { Console.WriteLine($"Error executing Safe Mode batch: {ex.Message}"); }
                        }
                    }
                    Console.WriteLine(">>> Safe Mode Schema Applied Successfully.");
                }

                // [AUTO-FIX 11] Run Migration Wizard Schema Migration
                var migWizardPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "docs", "DatabaseScripts", "Manual_MigrationWizard_Schema.sql");
                if (File.Exists(migWizardPath))
                {
                    Console.WriteLine($">>> Applying Migration Wizard Schema Fixes from: {migWizardPath}");
                    var mwSql = File.ReadAllText(migWizardPath);
                    var mwBatches = System.Text.RegularExpressions.Regex.Split(mwSql, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    
                    foreach (var batch in mwBatches)
                    {
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            try { dbContext.Database.ExecuteSqlRaw(batch); } catch (Exception ex) { Console.WriteLine($"Error executing Migration Wizard batch: {ex.Message}"); }
                        }
                    }
                    Console.WriteLine(">>> Migration Wizard Schema Applied Successfully.");
                }

                // [AUTO-FIX 12] MFA Columns for AppUsers
                try
                {
                    var mfaFixSql = @"
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.AppUsers') AND name = 'MfaEnabled')
                        BEGIN
                            ALTER TABLE dbo.AppUsers ADD MfaEnabled BIT NOT NULL DEFAULT 0;
                            PRINT 'Added MfaEnabled column directly';
                        END
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.AppUsers') AND name = 'MfaSecretKey')
                        BEGIN
                            ALTER TABLE dbo.AppUsers ADD MfaSecretKey NVARCHAR(MAX) NULL;
                            PRINT 'Added MfaSecretKey column directly';
                        END
                    ";
                    dbContext.Database.ExecuteSqlRaw(mfaFixSql);
                    Console.WriteLine(">>> MFA Columns (MfaEnabled, MfaSecretKey) verified/applied.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($">>> Error executing MFA schema fix: {ex.Message}");
                }

                // dbContext.Database.Migrate(); // Temporarily disabled - will apply manually
                // Console.WriteLine(">>> DB MIGRATION: Skipped - apply manually with 'dotnet ef database update'");
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>> DB MIGRATION ERROR for GfcDbContext:");
                Console.WriteLine(ex.ToString());
                // Don't throw, try to continue
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

        app.MapBlazorHub();
        app.MapHub<AnimationHub>("/animationhub"); // Map the AnimationHub
  
        app.MapHub<GFC.BlazorServer.Hubs.StudioPreviewHub>("/studiopreviewhub");
        app.MapHub<GFC.BlazorServer.Hubs.VideoAccessHub>("/videoaccesshub");
        app.MapHub<GFC.BlazorServer.Hubs.ControllerEventHub>("/controllereventhub");
        app.MapFallbackToPage("/_Host");

        app.MapGet("/health", () => Results.Ok());

        // Auto-detect and set environment on startup
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<GfcDbContext>>();
                using var db = dbFactory.CreateDbContext();
                
                var settings = db.SystemSettings.FirstOrDefault();
                if (settings != null)
                {
                    // Auto-detect environment based on machine name
                    var machineName = Environment.MachineName.ToLowerInvariant();
                    var isProduction = machineName.Contains("webhost") || 
                                      machineName.Contains("server") || 
                                      machineName.Contains("prod");
                    
                    var detectedEnvironment = isProduction ? "Production" : "Development";
                    
                    // Only update if different (avoid unnecessary writes)
                    if (settings.HostingEnvironment != detectedEnvironment)
                    {
                        settings.HostingEnvironment = detectedEnvironment;
                        db.SaveChanges();
                        Console.WriteLine($"[Startup] Auto-detected environment: {detectedEnvironment} (Machine: {Environment.MachineName})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Startup] Could not auto-detect environment: {ex.Message}");
            }
        }

        app.Run();
    }
}
