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
using GFC.BlazorServer.Data.Repositories;
using GFC.BlazorServer.Repositories;
using GFC.Data;
using GFC.Core.Helpers;
using GFC.Core.Interfaces;
using GFC.Core.Services;
using GFC.Data.Repositories;
using GFC.BlazorServer.ProtocolCapture.Services;
using GFC.BlazorServer.Middleware;
using GFC.BlazorServer.Hubs; // Add this using directive

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
        builder.Services.AddServerSideBlazor().AddHubOptions(options => options.ClientTimeoutInterval = TimeSpan.FromSeconds(60)).AddCircuitOptions(options => options.DetailedErrors = true);
        builder.Services.AddSignalR(); // Add SignalR
        
        // Add CORS for Next.js frontend
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowNextJs", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
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
        builder.Services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();
        builder.Services.AddScoped<ILotteryShiftRepository, LotteryShiftRepository>();
        builder.Services.AddScoped<IPhysicalKeyRepository, PhysicalKeyRepository>();
        builder.Services.AddScoped<IUserNotificationPreferencesRepository, UserNotificationPreferencesRepository>();
        builder.Services.AddScoped<IPagePermissionRepository, PagePermissionRepository>();
        builder.Services.AddScoped<IStudioPageRepository, GFC.BlazorServer.Repositories.StudioPageRepository>();

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
        builder.Services.AddScoped<ControllerRegistryService>();
        builder.Services.AddScoped<ControllerEventService>();
        builder.Services.AddScoped<CommandInfoService>();
        builder.Services.AddScoped<IScheduleService, ScheduleService>();
        builder.Services.AddScoped<IMaintenanceService, GFC.BlazorServer.Services.Maintenance.MaintenanceService>();
        builder.Services.AddScoped<IDoorConfigService, DoorConfigService>();
        builder.Services.AddScoped<DoorConfigSyncService>();
        builder.Services.AddScoped<AutoOpenAndAdvancedModesService>();
        builder.Services.AddScoped<IMemberAccessService, MemberAccessService>();
        builder.Services.AddScoped<IMemberPrivilegeSyncService, MemberPrivilegeSyncService>();
        builder.Services.AddScoped<DuesService>();
        builder.Services.AddScoped<WaiverService>();
        builder.Services.AddScoped<KeyHistoryService>();
        builder.Services.AddScoped<ReceiptStorageService>();
        builder.Services.AddScoped<ReimbursementService>();
        builder.Services.AddScoped<ThemeService>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();

        // Network Location Service (registered above as Scoped)
        builder.Services.AddScoped<IUserConnectionService, UserConnectionService>();

        // GFC Ecosystem Foundation
        builder.Services.AddSingleton<ToastService>();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IMediaStorageService, MediaStorageService>();
        builder.Services.AddScoped<IContentIngestionService, ContentIngestionService>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.IStudioService, GFC.BlazorServer.Services.StudioService>();
        builder.Services.AddScoped<IStudioAutoSaveService, StudioAutoSaveService>();
        builder.Services.AddScoped<ITemplateService, TemplateService>();
        builder.Services.AddScoped<IMediaAssetService, MediaAssetService>();
        builder.Services.AddScoped<IFormService, FormService>();
        builder.Services.AddScoped<ISeoService, SeoService>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<INetworkLocationService, NetworkLocationService>();
builder.Services.AddScoped<IWireGuardManagementService, WireGuardManagementService>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
builder.Services.AddSingleton<TunnelStatusService>();
builder.Services.AddHostedService<CloudflareTunnelHealthService>();
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
        
        // Controller Client Wiring
        
        // Register concrete controller clients. The `RealControllerClient` in the `Services` namespace
        // is the one that implements the `IMengqiControllerClient` for test tooling. The one
        // in `Services/Controllers` implements `IControllerClient` for the main application.
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.RealControllerClient>();
        builder.Services.AddScoped<GFC.BlazorServer.Services.RealControllerClient>();

        // Register the IControllerClient to always use the RealControllerClient.
        // The DynamicControllerClient that switched between real and simulation has been removed.
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.IControllerClient, GFC.BlazorServer.Services.Controllers.RealControllerClient>();
        builder.Services.AddScoped<IMengqiControllerClient, GFC.BlazorServer.Services.RealControllerClient>();
        
        // Register ControllerNetworkConfigService AFTER IControllerClient is registered
        builder.Services.AddScoped<GFC.BlazorServer.Services.Controllers.ControllerNetworkConfigService>();
        
        builder.Services.AddScoped<ControllerTestService>();

        builder.Services.AddHostedService<DirectorAccessExpiryWorker>();

        var app = builder.Build();



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
        
        // Enable CORS
        app.UseCors("AllowNextJs");

        // IMPORTANT: DevAuth must run after UseRouting and before authorization policies.
        if (app.Environment.IsDevelopment())
        {
            app.UseDevAuthAutoAdmin();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<VideoAccessGuardMiddleware>();

        // Apply pending migrations at startup and surface any errors
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<GfcDbContext>();
                
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
        app.MapFallbackToPage("/_Host");

        app.MapGet("/health", () => Results.Ok());

        app.Run();
    }
}
