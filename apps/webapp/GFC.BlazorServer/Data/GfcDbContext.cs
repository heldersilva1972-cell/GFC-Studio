// [MODIFIED]
using System.Threading;
using System.Threading.Tasks;
using GFC.Core.Models;
using GFC.Core.Models.Diagnostics;
using GFC.Core.Models.Security;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Data;

public class GfcDbContext : DbContext
{
    public GfcDbContext(DbContextOptions<GfcDbContext> options) : base(options)
    {
    }

    public DbSet<ControllerDevice> Controllers => Set<ControllerDevice>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Door> Doors => Set<Door>();
    public DbSet<ControllerEvent> ControllerEvents => Set<ControllerEvent>();
    public DbSet<ControllerLastIndex> ControllerLastIndexes => Set<ControllerLastIndex>();
    public DbSet<ControllerCommandInfo> ControllerCommandInfos => Set<ControllerCommandInfo>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<SpecialEvent> SpecialEvents => Set<SpecialEvent>();
    public DbSet<TimeProfile> TimeProfiles => Set<TimeProfile>();
    public DbSet<TimeProfileInterval> TimeProfileIntervals => Set<TimeProfileInterval>();
    public DbSet<ControllerTimeProfileLink> ControllerTimeProfileLinks => Set<ControllerTimeProfileLink>();
    public DbSet<TaskEntry> TaskEntries => Set<TaskEntry>();
    public DbSet<DoorConfig> DoorConfigs => Set<DoorConfig>();
    public DbSet<DoorAutoOpenSchedule> DoorAutoOpenSchedules => Set<DoorAutoOpenSchedule>();
    public DbSet<DoorBehaviorOptions> DoorBehaviorOptions => Set<DoorBehaviorOptions>();
    public DbSet<ControllerBehaviorOptions> ControllerBehaviorOptions => Set<ControllerBehaviorOptions>();
    public DbSet<ControllerNetworkConfig> ControllerNetworkConfigs => Set<ControllerNetworkConfig>();
    public DbSet<ControllerCommandLog> ControllerCommandLogs => Set<ControllerCommandLog>();
    public DbSet<CardReaderProfile> CardReaderProfiles => Set<CardReaderProfile>();
    public DbSet<SystemSettings> SystemSettings => Set<SystemSettings>();
    public DbSet<GFC.BlazorServer.Data.Entities.MemberDoorAccess> MemberDoorAccesses => Set<GFC.BlazorServer.Data.Entities.MemberDoorAccess>();
    public DbSet<DuesPayment> DuesPayments => Set<DuesPayment>();
    public DbSet<Waiver> Waivers => Set<Waiver>();
    public DbSet<GFC.BlazorServer.Data.Entities.NPQueueEntry> NPQueueEntries => Set<GFC.BlazorServer.Data.Entities.NPQueueEntry>();
    public DbSet<KeyHistory> KeyHistories => Set<KeyHistory>();
    public DbSet<DuesWaiverPeriod> DuesWaiverPeriods => Set<DuesWaiverPeriod>();
    public DbSet<ReimbursementRequest> ReimbursementRequests => Set<ReimbursementRequest>();
    public DbSet<ReimbursementItem> ReimbursementItems => Set<ReimbursementItem>();
    public DbSet<ReimbursementCategory> ReimbursementCategories => Set<ReimbursementCategory>();
    public DbSet<ReceiptFile> ReceiptFiles => Set<ReceiptFile>();
    public DbSet<ReimbursementChangeLog> ReimbursementChangeLogs => Set<ReimbursementChangeLog>();
    public DbSet<ReimbursementSettings> ReimbursementSettings => Set<ReimbursementSettings>();
    public DbSet<UserNotificationPreferences> UserNotificationPreferences => Set<UserNotificationPreferences>();
    public DbSet<PushSubscription> PushSubscriptions => Set<PushSubscription>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    
    // Diagnostics System
    public DbSet<GFC.Core.Models.Diagnostics.PerformanceSnapshot> PerformanceSnapshots => Set<GFC.Core.Models.Diagnostics.PerformanceSnapshot>();
    public DbSet<GFC.Core.Models.Diagnostics.AlertThreshold> AlertThresholds => Set<GFC.Core.Models.Diagnostics.AlertThreshold>();
    public DbSet<GFC.Core.Models.Diagnostics.DiagnosticAlert> DiagnosticAlerts => Set<GFC.Core.Models.Diagnostics.DiagnosticAlert>();
    
    // Camera System
    public DbSet<GFC.Core.Models.Camera> Cameras => Set<GFC.Core.Models.Camera>();
    public DbSet<GFC.Core.Models.VpnProfile> VpnProfiles => Set<GFC.Core.Models.VpnProfile>();
    public DbSet<GFC.Core.Models.AppPage> AppPages => Set<GFC.Core.Models.AppPage>();
    public DbSet<GFC.Core.Models.PagePermission> PagePermissions => Set<GFC.Core.Models.PagePermission>();
    public DbSet<UserPagePermission> UserPagePermissions => Set<UserPagePermission>();
    public DbSet<GFC.Core.Models.CameraEvent> CameraEvents => Set<GFC.Core.Models.CameraEvent>();
    public DbSet<GFC.Core.Models.Recording> Recordings => Set<GFC.Core.Models.Recording>();
    public DbSet<GFC.Core.Models.CameraPermission> CameraPermissions => Set<GFC.Core.Models.CameraPermission>();
    public DbSet<GFC.Core.Models.CameraAuditLog> CameraAuditLogs => Set<GFC.Core.Models.CameraAuditLog>();
    public DbSet<GFC.Core.Models.VideoAccessAudit> VideoAccessAudits => Set<GFC.Core.Models.VideoAccessAudit>();

    // Camera Security & Remote Access
    public DbSet<GFC.Core.Models.VpnSession> VpnSessions => Set<GFC.Core.Models.VpnSession>();
    public DbSet<GFC.Core.Models.SecurityAlert> SecurityAlerts => Set<GFC.Core.Models.SecurityAlert>();
    public DbSet<AuthorizedUser> AuthorizedUsers => Set<AuthorizedUser>();
    public DbSet<GFC.Core.Models.KeyCard> KeyCards => Set<GFC.Core.Models.KeyCard>();
    public DbSet<TrustedDevice> TrustedDevices => Set<TrustedDevice>();
    public DbSet<DeviceInviteToken> DeviceInviteTokens => Set<DeviceInviteToken>();
    public DbSet<UserPasskey> UserPasskeys => Set<UserPasskey>();
    public DbSet<MagicLinkToken> MagicLinkTokens => Set<MagicLinkToken>();
    public DbSet<VpnOnboardingToken> VpnOnboardingTokens => Set<VpnOnboardingToken>();
    // public DbSet<VpnProfile> VpnProfiles => Set<VpnProfile>(); // Commented due to CS0102 duplicate error - duplicate location unknown

    // GFC Ecosystem Foundation
    public DbSet<StudioPage> StudioPages => Set<StudioPage>();
    public DbSet<StudioSection> StudioSections => Set<StudioSection>();
    public DbSet<StudioDraft> StudioDrafts => Set<StudioDraft>();
    public DbSet<StudioLock> StudioLocks => Set<StudioLock>();
    public DbSet<StudioTemplate> StudioTemplates => Set<StudioTemplate>();
    public DbSet<StudioSetting> StudioSettings => Set<StudioSetting>();
    
    // Studio CMS
    public DbSet<StudioCollection> StudioCollections => Set<StudioCollection>();
    public DbSet<StudioCollectionField> StudioCollectionFields => Set<StudioCollectionField>();
    public DbSet<StudioCollectionItem> StudioCollectionItems => Set<StudioCollectionItem>();

    public DbSet<HallRental> HallRentals => Set<HallRental>();
    public DbSet<HallRentalRequest> HallRentalRequests => Set<HallRentalRequest>();
    public DbSet<StaffShift> StaffShifts => Set<StaffShift>();
    public DbSet<StaffMember> StaffMembers => Set<StaffMember>();
    public DbSet<ShiftReport> ShiftReports => Set<ShiftReport>();
    public DbSet<BoardAssignment> BoardAssignments => Set<BoardAssignment>();
    public DbSet<BoardPosition> BoardPositions => Set<BoardPosition>();
    public DbSet<SystemNotification> SystemNotifications => Set<SystemNotification>();
    public DbSet<AvailabilityCalendar> AvailabilityCalendars => Set<AvailabilityCalendar>();
    public DbSet<EventPromotion> EventPromotions => Set<EventPromotion>();
    public DbSet<NavMenuEntry> NavMenuEntries => Set<NavMenuEntry>();
    public DbSet<WebsiteSettings> WebsiteSettings => Set<WebsiteSettings>();

    // Phase 14: Integrated Utility Suite
    public DbSet<PublicReview> PublicReviews => Set<PublicReview>();
    public DbSet<NotificationRouting> NotificationRoutings => Set<NotificationRouting>();
    public DbSet<StudioSectionAsset> StudioSectionAssets => Set<StudioSectionAsset>();
    public DbSet<AssetFolder> AssetFolders => Set<AssetFolder>();
    public DbSet<MediaAsset> MediaAssets => Set<MediaAsset>();
    public DbSet<MediaRendition> MediaRenditions => Set<MediaRendition>();
    public DbSet<Form> Forms => Set<Form>();
    public DbSet<FormField> FormFields => Set<FormField>();
    public DbSet<FormSubmission> FormSubmissions => Set<FormSubmission>();
    public DbSet<HallRentalInquiry> HallRentalInquiries => Set<HallRentalInquiry>();
    public DbSet<SeoSettings> SeoSettings => Set<SeoSettings>();
    public DbSet<ProtectedDocument> ProtectedDocuments => Set<ProtectedDocument>();
    public DbSet<BarSaleEntry> BarSaleEntries => Set<BarSaleEntry>();
    public DbSet<DynamicForm> DynamicForms => Set<DynamicForm>();
    public DbSet<UrlRedirect> UrlRedirects => Set<UrlRedirect>();
    
    // Network Migration
    public DbSet<BylawDocument> BylawDocuments => Set<BylawDocument>();
    public DbSet<BylawRevision> BylawRevisions => Set<BylawRevision>();
    
    // Liquor Inventory System
    public DbSet<LiquorItem> LiquorItems => Set<LiquorItem>();
    public DbSet<LiquorVendor> LiquorVendors => Set<LiquorVendor>();
    public DbSet<LiquorOrder> LiquorOrders => Set<LiquorOrder>();
    public DbSet<LiquorOrderItem> LiquorOrderItems => Set<LiquorOrderItem>();
    public DbSet<LiquorTransaction> LiquorTransactions => Set<LiquorTransaction>();
    public DbSet<LiquorNotificationRule> LiquorNotificationRules => Set<LiquorNotificationRule>();
    public DbSet<UserPageUsage> UserPageUsage => Set<UserPageUsage>();
    public DbSet<LotteryWeeklyStat> LotteryWeeklyStats => Set<LotteryWeeklyStat>();
    public DbSet<LotteryShift> LotteryShifts => Set<LotteryShift>();
    public DbSet<ClubEvent> ClubEvents => Set<ClubEvent>();
    public DbSet<ClubEventTransaction> ClubEventTransactions => Set<ClubEventTransaction>();
    public DbSet<TaxBracket> TaxBrackets => Set<TaxBracket>();
    public DbSet<TaxStandardDeduction> TaxStandardDeductions => Set<TaxStandardDeduction>();
    public DbSet<YearlyWage> YearlyWages => Set<YearlyWage>();
    public DbSet<PosCategory> PosCategories => Set<PosCategory>();
    public DbSet<PosToken> PosTokens => Set<PosToken>();
    public DbSet<PosSale> PosSales => Set<PosSale>();
    public DbSet<PosZReport> PosZReports => Set<PosZReport>();
    public DbSet<PosMenuProfile> PosMenuProfiles => Set<PosMenuProfile>();
    public DbSet<PosMenuOverride> PosMenuOverrides => Set<PosMenuOverride>();
    public DbSet<PosTerminal> PosTerminals => Set<PosTerminal>();
    
    // Events & Banquets
    public DbSet<EventTemplate> EventTemplates => Set<EventTemplate>();
    public DbSet<ActiveEvent> ActiveEvents => Set<ActiveEvent>();
    
    // Finance System (Bills & Invoices)
    public DbSet<GFC.Core.Models.Finance.FinanceBill> FinanceBills => Set<GFC.Core.Models.Finance.FinanceBill>();
    public DbSet<GFC.Core.Models.Finance.FinanceVendor> FinanceVendors => Set<GFC.Core.Models.Finance.FinanceVendor>();
    public DbSet<GFC.Core.Models.Finance.FinanceCategory> FinanceCategories => Set<GFC.Core.Models.Finance.FinanceCategory>();
    public DbSet<GFC.Core.Models.Finance.FinancePayment> FinancePayments => Set<GFC.Core.Models.Finance.FinancePayment>();
    public DbSet<GFC.Core.Models.Finance.FinanceLoan> FinanceLoans => Set<GFC.Core.Models.Finance.FinanceLoan>();
    public DbSet<GFC.Core.Models.Finance.FinancePaymentType> FinancePaymentTypes => Set<GFC.Core.Models.Finance.FinancePaymentType>();
    public DbSet<GFC.Core.Models.Finance.FinanceAuditLog> FinanceAuditLogs => Set<GFC.Core.Models.Finance.FinanceAuditLog>();
    public DbSet<GFC.Core.Models.Finance.LotteryWeeklySettlement> LotteryWeeklySettlements => Set<GFC.Core.Models.Finance.LotteryWeeklySettlement>();

    // BINGO
    public DbSet<BingoSession> BingoSessions => Set<BingoSession>();
    public DbSet<BingoGameEntry> BingoGameEntries => Set<BingoGameEntry>();
    public DbSet<BingoSheetDefinition> BingoSheetDefinitions => Set<BingoSheetDefinition>();
    public DbSet<BingoGameDefinition> BingoGameDefinitions => Set<BingoGameDefinition>();
    public DbSet<BingoAdmissionDefinition> BingoAdmissionDefinitions => Set<BingoAdmissionDefinition>();
    public DbSet<BingoAdmissionEntry> BingoAdmissionEntries => Set<BingoAdmissionEntry>();
    public DbSet<BingoPayoutTier> BingoPayoutTiers => Set<BingoPayoutTier>();
    public DbSet<BingoExpenseCategory> BingoExpenseCategories => Set<BingoExpenseCategory>();
    public DbSet<BingoLotteryTransaction> BingoLotteryTransactions => Set<BingoLotteryTransaction>();
    public DbSet<BingoActiveLoan> BingoActiveLoans => Set<BingoActiveLoan>();
    public DbSet<BingoReconciliation> BingoReconciliations => Set<BingoReconciliation>();
    public DbSet<PullTabGameDefinition> PullTabGameDefinitions => Set<PullTabGameDefinition>();
    public DbSet<PullTabPrizeOption> PullTabPrizeOptions => Set<PullTabPrizeOption>();
    public DbSet<PullTabGameEntry> PullTabGameEntries => Set<PullTabGameEntry>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // [PROFESSIONAL SYNC FIX] Configure Concurrency Tokens (RowVersion) for all entities inheriting from BaseEntity.
        // This replaces the [Timestamp] attribute, allowing the API to accept mobile data without 'RowVersion'
        // while still enforcing database-level concurrency protection.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property("RowVersion")
                    .IsRowVersion();
            }
        }

        modelBuilder.Entity<YearlyWage>(entity =>
        {
            entity.ToTable("YearlyWages", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(y => !y.IsDeleted);
        });

        modelBuilder.Entity<TaxBracket>(entity =>
        {
            entity.ToTable("TaxBrackets", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LowerBound).HasColumnType("decimal(18,2)");
            entity.Property(e => e.UpperBound).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BaseTax).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Rate).HasColumnType("decimal(18,4)"); // Rate usually needs more scale
        });

        modelBuilder.Entity<TaxStandardDeduction>(entity =>
        {
            entity.ToTable("TaxStandardDeductions", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<BingoAdmissionDefinition>(entity =>
        {
            entity.ToTable("BingoAdmissionDefinitions", "dbo");
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(a => !a.IsDeleted);
        });

        modelBuilder.Entity<BingoAdmissionEntry>(entity =>
        {
            entity.ToTable("BingoAdmissionEntries", "dbo");
            entity.Property(e => e.PriceAtTime).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<BingoPayoutTier>(entity =>
        {
            entity.ToTable("BingoPayoutTiers", "dbo");
            entity.Property(e => e.PayoutPercentage).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        // Standard "Gold Standard" Query Filter for Soft Deletes
        modelBuilder.Entity<Member>().ToTable("Members", "dbo").HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<BarSaleEntry>().ToTable("BarSaleEntries", "dbo").HasQueryFilter(b => b.IsDeleted == false);
        modelBuilder.Entity<DuesPayment>().ToTable("DuesPayments", "dbo").HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<StaffShift>().ToTable("StaffShifts", "dbo").HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<ShiftReport>().ToTable("ShiftReports", "dbo").HasQueryFilter(s => !s.IsDeleted);
        modelBuilder.Entity<ClubEvent>().ToTable("ClubEvents", "dbo").HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ClubEventTransaction>().ToTable("ClubEventTransactions", "dbo").HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<BingoActiveLoan>().ToTable("BingoActiveLoans", "dbo").HasQueryFilter(l => !l.IsDeleted);
        modelBuilder.Entity<BingoReconciliation>().ToTable("BingoReconciliations", "dbo").HasQueryFilter(r => !r.IsDeleted);

        modelBuilder.Entity<PullTabGameDefinition>(entity =>
        {
            entity.ToTable("PullTabGameDefinitions", "dbo");
            entity.Property(e => e.TicketPrice).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<PullTabPrizeOption>(entity =>
        {
            entity.ToTable("PullTabPrizeOptions", "dbo");
            entity.Property(e => e.PayoutAmount).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<PullTabGameEntry>(entity =>
        {
            entity.ToTable("PullTabGameEntries", "dbo");
            entity.Property(e => e.PrizesPaid).HasColumnType("decimal(18,2)");
            entity.Property(e => e.StartingBank).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CashReceived).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });



        modelBuilder.Entity<ControllerDevice>()
            .ToTable("Controllers")
            .HasIndex(c => c.SerialNumber)
            .IsUnique();

        modelBuilder.Entity<Door>()
            .ToTable("Doors")
            .HasIndex(d => new { d.ControllerId, d.DoorIndex })
            .IsUnique();

        modelBuilder.Entity<ControllerEvent>(entity =>
        {
            entity.ToTable("ControllerEvents");
            entity.HasIndex(e => new { e.ControllerId, e.RawIndex })
                .HasDatabaseName("IX_ControllerEvents_Controller_RawIndex");

            entity.HasOne(e => e.Controller)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.ControllerId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Door)
                .WithMany()
                .HasForeignKey(e => e.DoorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ControllerCommandInfo>()
            .HasIndex(c => c.Key)
            .IsUnique();

        modelBuilder.Entity<ControllerCommandInfo>()
            .HasData(GetCommandSeedData());

        modelBuilder.Entity<TimeProfile>(entity =>
        {
            entity.ToTable("TimeProfiles");
            entity.HasMany(t => t.Intervals)
                .WithOne(i => i.TimeProfile)
                .HasForeignKey(i => i.TimeProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TimeProfileInterval>(entity =>
        {
            entity.ToTable("TimeProfileIntervals");
            entity.HasIndex(i => new { i.TimeProfileId, i.DayOfWeek, i.Order });
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.ToTable("Holidays");
            entity.HasIndex(h => h.Date);
        });

        modelBuilder.Entity<SpecialEvent>(entity =>
        {
            entity.ToTable("SpecialEvents");
            entity.HasIndex(e => e.Date);
            entity.HasOne(e => e.TimeProfile)
                .WithMany()
                .HasForeignKey(e => e.TimeProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ControllerTimeProfileLink>(entity =>
        {
            entity.ToTable("ControllerTimeProfileLinks");
            entity.HasIndex(l => new { l.ControllerId, l.ControllerProfileIndex })
                .IsUnique();
            entity.HasIndex(l => new { l.ControllerId, l.TimeProfileId })
                .IsUnique();
            entity.HasOne(l => l.Controller)
                .WithMany()
                .HasForeignKey(l => l.ControllerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(l => l.TimeProfile)
                .WithMany()
                .HasForeignKey(l => l.TimeProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TaskEntry>(entity =>
        {
            entity.ToTable("TaskEntries");
            entity.HasIndex(t => t.ControllerId);
            entity.HasOne(t => t.Controller)
                .WithMany()
                .HasForeignKey(t => t.ControllerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DoorConfig>(entity =>
        {
            entity.ToTable("DoorConfigs");
            entity.HasIndex(d => d.DoorId)
                .IsUnique();
            entity.HasOne(d => d.Door)
                .WithOne()
                .HasForeignKey<DoorConfig>(d => d.DoorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DoorAutoOpenSchedule>(entity =>
        {
            entity.ToTable("DoorAutoOpenSchedules");
            entity.HasIndex(d => d.DoorId)
                .IsUnique();
            entity.HasOne(d => d.Door)
                .WithMany()
                .HasForeignKey(d => d.DoorId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.TimeProfile)
                .WithMany()
                .HasForeignKey(d => d.TimeProfileId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<DoorBehaviorOptions>(entity =>
        {
            entity.ToTable("DoorBehaviorOptions");
            entity.HasIndex(d => d.DoorId)
                .IsUnique();
            entity.HasOne(d => d.Door)
                .WithMany()
                .HasForeignKey(d => d.DoorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ControllerBehaviorOptions>(entity =>
        {
            entity.ToTable("ControllerBehaviorOptions");
            entity.HasIndex(c => c.ControllerId)
                .IsUnique();
            entity.HasOne(c => c.Controller)
                .WithMany()
                .HasForeignKey(c => c.ControllerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ControllerNetworkConfig>(entity =>
        {
            entity.ToTable("ControllerNetworkConfigs");
            entity.HasIndex(c => c.ControllerId).IsUnique();
            entity.HasOne(c => c.Controller)
                .WithMany()
                .HasForeignKey(c => c.ControllerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ControllerCommandLog>(entity =>
        {
            entity.ToTable("ControllerCommandLogs");
            entity.HasIndex(c => c.ControllerId);
            entity.HasIndex(c => c.TimestampUtc);
            entity.HasOne(c => c.Controller)
                .WithMany()
                .HasForeignKey(c => c.ControllerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CardReaderProfile>(entity =>
        {
            entity.ToTable("CardReaderProfiles");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.DigitsOnly)
                .HasDefaultValue(true);

            entity.Property(p => p.PrefixToTrim)
                .HasMaxLength(50);

            entity.Property(p => p.SuffixToTrim)
                .HasMaxLength(50);

            entity.Property(p => p.LastSampleRaw)
                .HasColumnType("nvarchar(max)");

            entity.Property(p => p.LastSampleParsed)
                .HasColumnType("nvarchar(max)");
        });

        modelBuilder.Entity<SystemSettings>(entity =>
        {
            entity.ToTable("SystemSettings", "dbo");
            // Ensure only one row exists (Id = 1)
            entity.HasData(new SystemSettings
            {
                Id = 1,
                LastUpdatedUtc = null
            });

            entity.Property(e => e.MaStateTaxRate).HasColumnType("decimal(18,4)");
            entity.Property(e => e.PfmlEmployeeRate).HasColumnType("decimal(18,4)");
            entity.Property(e => e.PfmlEmployerRate).HasColumnType("decimal(18,4)");
            entity.Property(e => e.FicaEmployeeRate).HasColumnType("decimal(18,4)");
            entity.Property(e => e.FicaEmployerRate).HasColumnType("decimal(18,4)");
            entity.Property(e => e.MaUnemploymentRate).HasColumnType("decimal(18,4)");
            entity.Property(e => e.GlobalLiquorPourSize).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<GFC.BlazorServer.Data.Entities.MemberDoorAccess>(entity =>
        {
            entity.ToTable("MemberDoorAccess");
            entity.HasIndex(m => new { m.MemberId, m.DoorId, m.CardNumber });
            entity.HasOne(m => m.Door)
                .WithMany()
                .HasForeignKey(m => m.DoorId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(m => m.TimeProfile)
                .WithMany()
                .HasForeignKey(m => m.TimeProfileId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<DuesPayment>(entity =>
        {
            entity.ToTable("DuesPayments");
            entity.HasKey(d => new { d.MemberId, d.Year });
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Waiver>(entity =>
        {
            entity.ToTable("Waivers");
            entity.HasIndex(w => new { w.MemberId, w.Year });
        });

        modelBuilder.Entity<DuesWaiverPeriod>(entity =>
        {
            entity.ToTable("DuesWaiverPeriods");
            entity.HasKey(w => w.WaiverId);
            entity.HasIndex(w => w.MemberId);
        });

        modelBuilder.Entity<GFC.BlazorServer.Data.Entities.NPQueueEntry>(entity =>
        {
            entity.ToTable("NPQueueEntries");
            entity.HasIndex(n => n.QueuePosition);
            entity.HasIndex(n => n.MemberId);
        });

        modelBuilder.Entity<KeyHistory>(entity =>
        {
            entity.ToTable("KeyHistory");
            entity.HasIndex(k => k.MemberId);
            entity.HasIndex(k => k.CardNumber);
            entity.HasIndex(k => k.Date);
        });

        modelBuilder.Entity<ReimbursementRequest>(entity =>
        {
            entity.ToTable("ReimbursementRequests");
            entity.HasIndex(r => r.RequestorMemberId);
            entity.HasIndex(r => r.RequestDate);
            entity.HasIndex(r => r.Status);
        });

        modelBuilder.Entity<ReimbursementItem>(entity =>
        {
            entity.ToTable("ReimbursementItems");
            entity.HasIndex(i => i.RequestId);
            entity.HasIndex(i => i.CategoryId);
            entity.HasOne(i => i.Request)
                .WithMany(r => r.Items)
                .HasForeignKey(i => i.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReimbursementCategory>(entity =>
        {
            entity.ToTable("ReimbursementCategories");
            entity.HasIndex(c => c.IsActive);
            
            // Seed categories
            entity.HasData(
                new ReimbursementCategory { Id = 1, Name = "Supplies (General house items)", IsActive = true },
                new ReimbursementCategory { Id = 2, Name = "Bar / Kitchen (Food, garnishes, napkins)", IsActive = true },
                new ReimbursementCategory { Id = 3, Name = "Repairs & Maintenance (Building or equipment fixes)", IsActive = true },
                new ReimbursementCategory { Id = 4, Name = "Events (Specific costs for a club party or event)", IsActive = true },
                new ReimbursementCategory { Id = 5, Name = "Office (Postage, paper, ink)", IsActive = true },
                new ReimbursementCategory { Id = 6, Name = "Other (Miscellaneous)", IsActive = true }
            );
        });

        modelBuilder.Entity<ReceiptFile>(entity =>
        {
            entity.ToTable("ReceiptFiles");
            entity.HasIndex(f => f.RequestItemId);
            entity.HasIndex(f => f.UploadedByMemberId);
            entity.HasOne(f => f.RequestItem)
                .WithMany(i => i.ReceiptFiles)
                .HasForeignKey(f => f.RequestItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReimbursementChangeLog>(entity =>
        {
            entity.ToTable("ReimbursementChangeLogs");
            entity.HasIndex(c => c.RequestId);
            entity.HasIndex(c => c.ChangedByMemberId);
            entity.HasIndex(c => c.ChangeUtc);
            entity.HasOne(c => c.Request)
                .WithMany(r => r.ChangeLogs)
                .HasForeignKey(c => c.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReimbursementSettings>(entity =>
        {
            entity.ToTable("ReimbursementSettings");
            // Ensure only one row exists (Id = 1)
            entity.HasData(new ReimbursementSettings
            {
                Id = 1,
                ReceiptRequired = false,
                NotificationRecipients = null
            });
        });

        // AppUser configuration
        modelBuilder.Entity<GFC.Core.Models.AppUser>(entity =>
        {
            entity.ToTable("AppUsers", "dbo");
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.HourlyRate).HasColumnType("decimal(18,2)");
            entity.Property(u => u.DependentsAmount).HasColumnType("decimal(18,2)");
            entity.Property(u => u.OtherIncomeAmount).HasColumnType("decimal(18,2)");
            entity.Property(u => u.DeductionsAmount).HasColumnType("decimal(18,2)");
            entity.Property(u => u.ExtraWithholdingAmount).HasColumnType("decimal(18,2)");
        });

        // Camera system configuration
        modelBuilder.Entity<GFC.Core.Models.Camera>(entity =>
        {
            entity.ToTable("Cameras");
            entity.HasKey(c => c.Id);
        });

        modelBuilder.Entity<GFC.Core.Models.CameraEvent>(entity =>
        {
            entity.ToTable("CameraEvents");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Camera)
                .WithMany()
                .HasForeignKey(e => e.CameraId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GFC.Core.Models.Recording>(entity =>
        {
            entity.ToTable("Recordings");
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.Camera)
                .WithMany()
                .HasForeignKey(r => r.CameraId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GFC.Core.Models.CameraPermission>(entity =>
        {
            entity.ToTable("CameraPermissions");
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.Camera)
                .WithMany()
                .HasForeignKey(p => p.CameraId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GFC.Core.Models.CameraAuditLog>(entity =>
        {
            entity.ToTable("CameraAuditLogs");
            entity.HasKey(a => a.Id);
            entity.HasOne(a => a.Camera)
                .WithMany()
                .HasForeignKey(a => a.CameraId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<GFC.Core.Models.KeyCard>(entity =>
        {
            entity.ToTable("KeyCards");
            entity.HasKey(e => e.KeyCardId);
            entity.HasIndex(e => e.CardNumber);
            entity.HasIndex(e => e.MemberId);
        });

        // GFC Ecosystem Foundation / Studio V2
        modelBuilder.Entity<StudioPage>(entity =>
        {
            entity.ToTable("Pages");

            entity.HasIndex(p => p.Slug).IsUnique();
            entity.HasIndex(p => p.Status);
            entity.HasIndex(p => p.IsDeleted);
            entity.Ignore(p => p.IsPublished);

            entity.HasMany(p => p.Sections)
                .WithOne(s => s.StudioPage)
                .HasForeignKey(s => s.StudioPageId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Drafts)
                .WithOne(d => d.StudioPage)
                .HasForeignKey(d => d.StudioPageId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StudioSection>(entity =>
        {
            entity.ToTable("Sections");
            entity.Property(s => s.StudioPageId).HasColumnName("PageId");
            entity.Property(s => s.Data).HasColumnName("ContentJson");
            entity.Property(s => s.AnimationSettingsJson).HasColumnName("AnimationJson");

            entity.HasIndex(s => s.OrderIndex);
            entity.HasIndex(s => s.ComponentType);
        });

        modelBuilder.Entity<StudioDraft>(entity =>
        {
            entity.ToTable("Drafts");

            entity.HasOne(d => d.StudioPage)
                  .WithMany(p => p.Drafts)
                  .HasForeignKey(d => d.StudioPageId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(d => new { d.StudioPageId, d.Version }).IsDescending(false, true);
            entity.HasIndex(d => d.CreatedAt).IsDescending();
        });

        modelBuilder.Entity<StudioTemplate>(entity =>
        {
            entity.ToTable("Templates");
            entity.HasIndex(t => t.Category);
            entity.HasIndex(t => t.CreatedBy);
            entity.HasIndex(t => t.UsageCount).IsDescending();
        });

        modelBuilder.Entity<HallRental>(entity =>
        {
            entity.ToTable("HallRentals");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<HallRentalRequest>(entity =>
        {
            entity.ToTable("HallRentalRequests");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<StaffShift>(entity =>
        {
            entity.ToTable("StaffShifts");
        });

        modelBuilder.Entity<StaffMember>(entity =>
        {
            entity.ToTable("StaffMembers");
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<ShiftReport>(entity =>
        {
            entity.ToTable("ShiftReports");
            entity.Property(e => e.BarSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.LottoSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalDeposit).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<SystemNotification>(entity =>
        {
            entity.ToTable("SystemNotifications");
        });

        modelBuilder.Entity<AvailabilityCalendar>(entity =>
        {
            entity.ToTable("AvailabilityCalendars");
        });

        modelBuilder.Entity<EventPromotion>(entity =>
        {
            entity.ToTable("EventPromotions");
        });

        modelBuilder.Entity<NavMenuEntry>(entity =>
        {
            entity.ToTable("NavMenuEntries");
        });

        modelBuilder.Entity<WebsiteSettings>(entity =>
        {
            entity.ToTable("WebsiteSettings");
            entity.Property(e => e.AdditionalHourRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.AvEquipmentFee).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BartenderServiceFee).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CoalitionMemberRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CoalitionNonMemberRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FunctionHallMemberRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FunctionHallNonMemberRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.KitchenFee).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MemberRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NonMemberRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NonProfitRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SecurityDepositAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.YouthOrganizationMemberRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.YouthOrganizationNonMemberRate).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<StudioSetting>(entity =>
        {
            entity.ToTable("StudioSettings");
            entity.HasIndex(s => s.SettingKey).IsUnique();
        });

        // Phase 14: Integrated Utility Suite
        modelBuilder.Entity<MediaAsset>(entity =>
        {
            entity.ToTable("MediaAssets");
            entity.HasMany(a => a.Renditions)
                .WithOne(r => r.MediaAsset)
                .HasForeignKey(r => r.MediaAssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MediaRendition>(entity =>
        {
            entity.ToTable("MediaRenditions");
        });

        modelBuilder.Entity<Form>(entity =>
        {
            entity.ToTable("Forms");
            entity.HasMany(f => f.FormFields)
                .WithOne(ff => ff.Form)
                .HasForeignKey(ff => ff.FormId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FormField>(entity =>
        {
            entity.ToTable("FormFields");
        });

        modelBuilder.Entity<FormSubmission>(entity =>
        {
            entity.ToTable("FormSubmissions");
        });

        modelBuilder.Entity<HallRentalInquiry>(entity =>
        {
            entity.ToTable("HallRentalInquiries");
            entity.HasIndex(i => i.ResumeToken).IsUnique();
        });

        modelBuilder.Entity<SeoSettings>(entity =>
        {
            entity.ToTable("SeoSettings");
            entity.HasIndex(s => s.StudioPageId).IsUnique();
        });

        modelBuilder.Entity<ProtectedDocument>(entity =>
        {
            entity.ToTable("ProtectedDocuments");
        });

        modelBuilder.Entity<BarSaleEntry>(entity =>
        {
            entity.ToTable("BarSaleEntries", "dbo");
            entity.HasIndex(e => e.SaleDate);
            entity.HasIndex(e => new { e.AdjustedSaleDate, e.Shift, e.IsRentalHall }).IsUnique();
            entity.Property(e => e.TotalSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OriginalTotalSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalHours).HasColumnType("decimal(18,2)");
            entity.Property(e => e.HourlyRate_AtTimeOfShift).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalEmployeeTaxes_AtTimeOfShift).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalEmployerTaxes_AtTimeOfShift).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<StudioSectionAsset>(entity =>
        {
            entity.ToTable("StudioSectionAssets");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.StudioSection)
                .WithMany()
                .HasForeignKey(e => e.StudioSectionId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.MediaAsset)
                .WithMany()
                .HasForeignKey(e => e.MediaAssetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // BINGO
        modelBuilder.Entity<BingoSession>(entity =>
        {
            entity.ToTable("BingoSessions");
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.GameEntries)
                  .WithOne(e => e.Session)
                  .HasForeignKey(e => e.SessionId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.TotalGrossReceipts).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalPrizesPaid).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalLotteryTake).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalClubTake).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RoundingAdjustment).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.DoorPrizeAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<BingoGameEntry>(entity =>
        {
            entity.ToTable("BingoGameEntries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PricePerSheet).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GrossReceipts).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PrizePaid).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LotteryTake).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClubTake).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetProceeds).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LotteryPercentage).HasColumnType("decimal(18, 4)").HasColumnName("LotteryPercentage");
            entity.Property(e => e.ClubPercentage).HasColumnType("decimal(18, 4)").HasColumnName("ClubPercentage");
            entity.Property(e => e.RoundingAdjustment).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        modelBuilder.Entity<BingoSheetDefinition>(entity =>
        {
            entity.ToTable("BingoSheetDefinitions");
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Games)
                  .WithOne(e => e.Sheet)
                  .HasForeignKey(e => e.SheetDefinitionId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.DefaultPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LotteryPercentage).HasColumnType("decimal(18, 4)").HasColumnName("LotteryPercentage");
            entity.Property(e => e.ClubPercentage).HasColumnType("decimal(18, 4)").HasColumnName("ClubPercentage");
        });

        modelBuilder.Entity<BingoAdmissionEntry>(entity =>
        {
            entity.ToTable("BingoAdmissionEntries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PriceAtTime).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<BingoGameDefinition>(entity =>
        {
            entity.ToTable("BingoGameDefinitions");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<BingoExpenseCategory>(entity =>
        {
            entity.ToTable("BingoExpenseCategories", "dbo");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<BingoLotteryTransaction>(entity =>
        {
            entity.ToTable("BingoLotteryTransactions", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.HasOne(e => e.Category)
                  .WithMany()
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Session)
                  .WithMany()
                  .HasForeignKey(e => e.SessionId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Trusted Devices (Session Management)
        modelBuilder.Entity<TrustedDevice>(entity =>
        {
            entity.ToTable("TrustedDevices");
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.DeviceToken).IsUnique();
            entity.HasIndex(t => t.UserId);
            entity.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Device Invites
        modelBuilder.Entity<DeviceInviteToken>(entity =>
        {
            entity.ToTable("DeviceInviteTokens");
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.Token).IsUnique();
            entity.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // User Passkeys
        modelBuilder.Entity<UserPasskey>(entity =>
        {
            entity.ToTable("UserPasskeys");
            entity.HasKey(t => t.Id);
            entity.HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PosZReport>(entity =>
        {
            entity.ToTable("PosZReports");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CashTotal).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalGrossSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BanquetSummaryJson).IsRequired(false);
        });

        // Finance System Configuration
        modelBuilder.Entity<GFC.Core.Models.Finance.FinanceBill>(entity =>
        {
            entity.ToTable("FinanceBills");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.OriginalAmount).HasColumnType("decimal(18,2)");
            
            entity.HasIndex(b => b.DueDate);
            entity.HasIndex(b => b.Status);
            entity.HasIndex(b => b.VendorId);

            entity.HasOne(b => b.Vendor)
                .WithMany(v => v.Bills)
                .HasForeignKey(b => b.VendorId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(b => b.Category)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<GFC.Core.Models.Finance.LotteryWeeklySettlement>(entity =>
        {
            entity.ToTable("LotteryWeeklySettlements");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.NetDueAmount).HasColumnType("decimal(18,2)");
            entity.Property(s => s.EnvelopeDropAmount).HasColumnType("decimal(18,2)");
            entity.HasOne(s => s.LinkedBill)
                .WithMany()
                .HasForeignKey(s => s.LinkedBillId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<GFC.Core.Models.Finance.FinanceVendor>(entity =>
        {
            entity.ToTable("FinanceVendors");
            entity.HasKey(v => v.Id);
            entity.HasIndex(v => v.Name);
            entity.HasOne(v => v.DefaultPaymentType)
                .WithMany()
                .HasForeignKey(v => v.DefaultPaymentTypeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<GFC.Core.Models.Finance.FinancePaymentType>(entity =>
        {
            entity.ToTable("FinancePaymentTypes");
            entity.HasKey(pt => pt.Id);
            entity.HasIndex(pt => pt.Name).IsUnique();
        });

        modelBuilder.Entity<GFC.Core.Models.Finance.FinanceCategory>(entity =>
        {
            entity.ToTable("FinanceCategories");
            entity.HasKey(c => c.Id);
        });

        modelBuilder.Entity<GFC.Core.Models.Finance.FinancePayment>(entity =>
        {
            entity.ToTable("FinancePayments");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.AmountPaid).HasColumnType("decimal(18,2)");
            
            entity.HasIndex(p => p.BillId);
            entity.HasIndex(p => p.PaymentDate);

            entity.HasOne(p => p.Bill)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BillId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Liquor Inventory Configuration
        modelBuilder.Entity<LiquorItem>(entity =>
        {
            entity.ToTable("LiquorItems");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UpcCode).IsUnique().HasFilter("[UpcCode] IS NOT NULL");
        });

        modelBuilder.Entity<LiquorTransaction>(entity =>
        {
            entity.ToTable("LiquorTransactions");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LiquorNotificationRule>(entity =>
        {
            entity.ToTable("LiquorNotificationRules");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
        });

        modelBuilder.Entity<LiquorVendor>(entity =>
        {
            entity.ToTable("LiquorVendors");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MinimumOrderAmount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<LiquorOrder>(entity =>
        {
            entity.ToTable("LiquorOrders");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Vendor)
                .WithMany(v => v.Orders)
                .HasForeignKey(e => e.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LiquorOrderItem>(entity =>
        {
            entity.ToTable("LiquorOrderItems");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.LiquorItem)
                .WithMany(i => i.OrderHistory)
                .HasForeignKey(e => e.LiquorItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserPagePermission>(entity =>
        {
            entity.ToTable("UserPagePermissions");
            entity.HasKey(e => e.PermissionId);
        });

        modelBuilder.Entity<LotteryShift>(entity =>
        {
            entity.ToTable("LotteryShifts", "dbo");
            entity.HasKey(e => e.ShiftId);
            entity.HasIndex(e => new { e.ShiftDate, e.ShiftType }).IsUnique();
            
            entity.Property(e => e.StartingCash).HasColumnType("decimal(18,2)");
            entity.Property(e => e.EndingCash).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPayouts).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalCancels).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Commission).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CashBonus).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ClaimsBonus).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NetDue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BackupBagAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.EnvelopeAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.BagRefillAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NetSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ExpectedCash).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Variance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.LotteryIncome).HasColumnType("decimal(18,2)");
            entity.Property(e => e.NetIncome).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ShiftSalesActivity).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ShiftPayoutsActivity).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ShiftCancelsActivity).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ShiftNetDueActivity).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OriginalTotalSales).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<LotteryWeeklyStat>(entity =>
        {
            entity.ToTable("LotteryWeeklyStats", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OnlineNetSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineCommission).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineCashes).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineCashBonus).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineClaimsBonus).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineAdjustments).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineServiceFee).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineBondingFee).HasColumnType("decimal(18,2)");
            entity.Property(e => e.OnlineDue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantGrossSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantReturnSales).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantCommission).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantCashes).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantCashBonus).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantClaimsBonus).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantAdjustments).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InstantDue).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalDue).HasColumnType("decimal(18,2)");
        });


        modelBuilder.Entity<ClubEvent>(entity =>
        {
            entity.ToTable("ClubEvents");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InitialBudget).HasColumnType("decimal(18,2)");
            entity.HasMany(e => e.Transactions)
                  .WithOne(t => t.Event)
                  .HasForeignKey(t => t.EventId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ClubEventTransaction>(entity =>
        {
            entity.ToTable("ClubEventTransactions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<PosCategory>(entity =>
        {
            entity.ToTable("PosCategories");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.DisplayOrder);
        });

        modelBuilder.Entity<PosToken>(entity =>
        {
            entity.ToTable("PosTokens", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<BoardAssignment>(entity =>
        {
            entity.ToTable("BoardAssignments");
            entity.HasKey(e => e.AssignmentID);
        });

        modelBuilder.Entity<BoardPosition>(entity =>
        {
            entity.ToTable("BoardPositions");
            entity.HasKey(e => e.PositionID);
        });

        modelBuilder.Entity<PosSale>(entity =>
        {
            entity.ToTable("PosSales", "dbo");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<PosMenuProfile>(entity =>
        {
            entity.ToTable("PosMenuProfiles");
            entity.HasKey(e => e.Id);
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<PosMenuOverride>(entity =>
        {
            entity.ToTable("PosMenuOverrides");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OverridePrice).HasColumnType("decimal(18,2)");
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<PosTerminal>(entity =>
        {
            entity.ToTable("PosTerminals");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TerminalName).IsUnique();
            entity.HasQueryFilter(e => !e.IsDeleted);
        });
    }

    private static IEnumerable<ControllerCommandInfo> GetCommandSeedData()
    {
        return new[]
        {
            new ControllerCommandInfo
            {
                Id = 1,
                Key = "OpenDoor",
                DisplayName = "Open Door",
                Category = "Door Control",
                Phase = "Phase 1",
                ShortDescription = "Momentarily energize a door relay for a door or elevator output.",
                LongDescription = "Sends the Agent OpenDoor command which issues the Mengqi type 32/code 1 packet with optional duration override.",
                RiskLevel = 1,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/door/{doorNo}/open",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 2,
                Key = "SyncTime",
                DisplayName = "Sync Controller Time",
                Category = "Timekeeping",
                Phase = "Phase 1",
                ShortDescription = "Align the controller RTC with the server clock.",
                LongDescription = "Uses the AdjustTime command (type 32/code 48) via the Agent to ensure logs timestamps stay accurate.",
                RiskLevel = 0,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/sync-time",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 3,
                Key = "AddOrUpdateCard",
                DisplayName = "Add or Update Card",
                Category = "Privileges",
                Phase = "Phase 3A",
                ShortDescription = "Apply a single card privilege row to the controller.",
                LongDescription = "Wraps the Mengqi privilege packet (type 36/code 62) through the Agent bulk/individual privilege APIs.",
                RiskLevel = 1,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/cards",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 4,
                Key = "DeleteCard",
                DisplayName = "Delete Card",
                Category = "Privileges",
                Phase = "Phase 3A",
                ShortDescription = "Remove a single card slot from the controller.",
                LongDescription = "Marks the privilege row as deleted via the Agent DELETE /cards/{cardNo} endpoint.",
                RiskLevel = 1,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent DELETE /{sn}/cards/{cardNo}",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 5,
                Key = "ClearAllCards",
                DisplayName = "Clear All Cards",
                Category = "Privileges",
                Phase = "Phase 3A",
                ShortDescription = "Erase the entire privilege table.",
                LongDescription = "Triggers the dangerous type 36/code 64 command through the Agent ClearAllCards API.",
                RiskLevel = 3,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/cards/clear-all",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 6,
                Key = "BulkUploadCards",
                DisplayName = "Bulk Upload Cards",
                Category = "Privileges",
                Phase = "Phase 3A",
                ShortDescription = "Stream a batch of privilege rows to the controller.",
                LongDescription = "Uses the Agent bulk upload endpoint which internally sequences the WGPacket privilege writes.",
                RiskLevel = 2,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/cards/bulk-upload",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 7,
                Key = "ReadEvents",
                DisplayName = "Read Events",
                Category = "Monitoring",
                Phase = "Phase 2",
                ShortDescription = "Fetch swipe/event records since the last index.",
                LongDescription = "Calls Agent GET /events which wraps GetLastRecordIndex + GetSingleSwipeRecord + confirm index.",
                RiskLevel = 0,
                IsReadOperation = true,
                IsWriteOperation = false,
                ProtocolInfo = "Agent GET /{sn}/events?lastIndex=x",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 8,
                Key = "ReadRunStatus",
                DisplayName = "Read Run/Status Block",
                Category = "Monitoring",
                Phase = "Phase 2",
                ShortDescription = "Poll live door, relay, and alarm states.",
                LongDescription = "Uses Agent GET /run-status which relays the Mengqi run info command (type 32/code 32).",
                RiskLevel = 0,
                IsReadOperation = true,
                IsWriteOperation = false,
                ProtocolInfo = "Agent GET /{sn}/run-status",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 9,
                Key = "SyncDoorConfig",
                DisplayName = "Sync Door Configuration",
                Category = "Configuration",
                Phase = "Phase 3B",
                ShortDescription = "Write door timing/mode settings.",
                LongDescription = "Uses Agent POST /door-config/sync which maps to extended config flash writes.",
                RiskLevel = 2,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/door-config/sync",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 10,
                Key = "SyncTimeProfiles",
                DisplayName = "Sync Time Profiles",
                Category = "Schedules",
                Phase = "Phase 3C",
                ShortDescription = "Write controller time zones / schedules.",
                LongDescription = "Wraps the Agent POST /time-schedules/sync endpoint that fills the holiday/time zone blocks.",
                RiskLevel = 2,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/time-schedules/sync",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 11,
                Key = "SyncHolidays",
                DisplayName = "Sync Holidays",
                Category = "Schedules",
                Phase = "Phase 3C",
                ShortDescription = "Update the holiday block used by privilege filtering.",
                LongDescription = "Part of the same FLASH region as time profiles; triggered through the schedule sync API.",
                RiskLevel = 2,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Included in POST /{sn}/time-schedules/sync",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 12,
                Key = "SyncAutoOpen",
                DisplayName = "Sync Auto Open Tasks",
                Category = "Schedules",
                Phase = "Phase 2F",
                ShortDescription = "Write automatic door unlock/lock tasks.",
                LongDescription = "Uses the Agent POST /auto-open/sync endpoint to replace the task table.",
                RiskLevel = 2,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/auto-open/sync",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 13,
                Key = "SyncAdvancedDoorModes",
                DisplayName = "Sync Advanced Door Modes",
                Category = "Configuration",
                Phase = "Phase 2F",
                ShortDescription = "Write extended per-door options (double lock, first card, etc.).",
                LongDescription = "Maps to Agent POST /advanced-door-modes/sync which writes the extended FRAM structures.",
                RiskLevel = 2,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/advanced-door-modes/sync",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 14,
                Key = "RebootController",
                DisplayName = "Reboot Controller",
                Category = "Maintenance",
                Phase = "Phase 5",
                ShortDescription = "Soft reboot the panel.",
                LongDescription = "Agent POST /reboot triggers the Mengqi special command (type 32/code 254).",
                RiskLevel = 3,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/reboot",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 15,
                Key = "SyncNetworkConfig",
                DisplayName = "Sync Network Config",
                Category = "Configuration",
                Phase = "Phase 4F",
                ShortDescription = "Update controller IP address, subnet mask, gateway, and port.",
                LongDescription = "Changes the controller's network configuration. Incorrect settings may cause loss of connectivity.",
                RiskLevel = 3,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/network-config",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 16,
                Key = "SyncAllowedPc",
                DisplayName = "Sync Allowed PC IP",
                Category = "Configuration",
                Phase = "Phase 4F",
                ShortDescription = "Update the allowed PC IP address whitelist.",
                LongDescription = "Sets which PC IP address is allowed to communicate with the controller. Incorrect settings may block legitimate access.",
                RiskLevel = 3,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/allowed-pc",
                Enabled = true
            },
            new ControllerCommandInfo
            {
                Id = 17,
                Key = "SyncControllerPassword",
                DisplayName = "Sync Controller Password",
                Category = "Configuration",
                Phase = "Phase 4F",
                ShortDescription = "Change the controller's communication password.",
                LongDescription = "Updates the password required for controller communication. If forgotten, controller may become inaccessible.",
                RiskLevel = 3,
                IsReadOperation = false,
                IsWriteOperation = true,
                ProtocolInfo = "Agent POST /{sn}/allowed-pc",
                Enabled = true
            }
        };
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                if (entry.Entity.GlobalId == Guid.Empty) 
                    entry.Entity.GlobalId = Guid.NewGuid();
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}
