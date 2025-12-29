// [MODIFIED]
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Data.Entities.Security;
using GFC.Core.Models;
using GFC.Core.Models.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Data;

public class GfcDbContext : DbContext
{
    public GfcDbContext(DbContextOptions<GfcDbContext> options) : base(options)
    {
    }

    public DbSet<ControllerDevice> Controllers => Set<ControllerDevice>();
    public DbSet<Door> Doors => Set<Door>();
    public DbSet<ControllerEvent> ControllerEvents => Set<ControllerEvent>();
    public DbSet<ControllerLastIndex> ControllerLastIndexes => Set<ControllerLastIndex>();
    public DbSet<ControllerCommandInfo> ControllerCommandInfos => Set<ControllerCommandInfo>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<SpecialEvent> SpecialEvents => Set<SpecialEvent>();
    public DbSet<GFC.BlazorServer.Data.Entities.TimeProfile> TimeProfiles => Set<GFC.BlazorServer.Data.Entities.TimeProfile>();
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
    public DbSet<GFC.BlazorServer.Data.Entities.DuesPayment> DuesPayments => Set<GFC.BlazorServer.Data.Entities.DuesPayment>();
    public DbSet<Waiver> Waivers => Set<Waiver>();
    public DbSet<GFC.BlazorServer.Data.Entities.NPQueueEntry> NPQueueEntries => Set<GFC.BlazorServer.Data.Entities.NPQueueEntry>();
    public DbSet<KeyHistory> KeyHistories => Set<KeyHistory>();
    public DbSet<ReimbursementRequest> ReimbursementRequests => Set<ReimbursementRequest>();
    public DbSet<ReimbursementItem> ReimbursementItems => Set<ReimbursementItem>();
    public DbSet<ReimbursementCategory> ReimbursementCategories => Set<ReimbursementCategory>();
    public DbSet<ReceiptFile> ReceiptFiles => Set<ReceiptFile>();
    public DbSet<ReimbursementChangeLog> ReimbursementChangeLogs => Set<ReimbursementChangeLog>();
    public DbSet<ReimbursementSettings> ReimbursementSettings => Set<ReimbursementSettings>();
    public DbSet<UserNotificationPreferences> UserNotificationPreferences => Set<UserNotificationPreferences>();
    
    // Diagnostics System
    public DbSet<GFC.Core.Models.Diagnostics.PerformanceSnapshot> PerformanceSnapshots => Set<GFC.Core.Models.Diagnostics.PerformanceSnapshot>();
    public DbSet<GFC.Core.Models.Diagnostics.AlertThreshold> AlertThresholds => Set<GFC.Core.Models.Diagnostics.AlertThreshold>();
    public DbSet<GFC.Core.Models.Diagnostics.DiagnosticAlert> DiagnosticAlerts => Set<GFC.Core.Models.Diagnostics.DiagnosticAlert>();
    
    // Camera System
    public DbSet<GFC.Core.Models.Camera> Cameras => Set<GFC.Core.Models.Camera>();
    public DbSet<GFC.Core.Models.VpnProfile> VpnProfiles => Set<GFC.Core.Models.VpnProfile>();
    public DbSet<GFC.Core.Models.AppPage> AppPages => Set<GFC.Core.Models.AppPage>();
    public DbSet<GFC.Core.Models.PagePermission> PagePermissions => Set<GFC.Core.Models.PagePermission>();
    public DbSet<GFC.Core.Models.CameraEvent> CameraEvents => Set<GFC.Core.Models.CameraEvent>();
    public DbSet<GFC.Core.Models.Recording> Recordings => Set<GFC.Core.Models.Recording>();
    public DbSet<GFC.Core.Models.CameraPermission> CameraPermissions => Set<GFC.Core.Models.CameraPermission>();
    public DbSet<GFC.Core.Models.CameraAuditLog> CameraAuditLogs => Set<GFC.Core.Models.CameraAuditLog>();
    public DbSet<GFC.Core.Models.VideoAccessAudit> VideoAccessAudits => Set<GFC.Core.Models.VideoAccessAudit>();

    // Camera Security & Remote Access
    public DbSet<GFC.Core.Models.VpnSession> VpnSessions => Set<GFC.Core.Models.VpnSession>();
    public DbSet<GFC.Core.Models.SecurityAlert> SecurityAlerts => Set<GFC.Core.Models.SecurityAlert>();
    public DbSet<AuthorizedUser> AuthorizedUsers => Set<AuthorizedUser>();
    // public DbSet<VpnProfile> VpnProfiles => Set<VpnProfile>(); // Commented due to CS0102 duplicate error - duplicate location unknown

    // GFC Ecosystem Foundation
    public DbSet<StudioPage> StudioPages => Set<StudioPage>();
    public DbSet<StudioSection> StudioSections => Set<StudioSection>();
    public DbSet<StudioDraft> StudioDrafts => Set<StudioDraft>();
    public DbSet<StudioLock> StudioLocks => Set<StudioLock>();
    public DbSet<StudioTemplate> StudioTemplates => Set<StudioTemplate>();
    public DbSet<StudioSetting> StudioSettings => Set<StudioSetting>();
    public DbSet<HallRental> HallRentals => Set<HallRental>();
    public DbSet<HallRentalRequest> HallRentalRequests => Set<HallRentalRequest>();
    public DbSet<StaffShift> StaffShifts => Set<StaffShift>();
    public DbSet<StaffMember> StaffMembers => Set<StaffMember>();
    public DbSet<ShiftReport> ShiftReports => Set<ShiftReport>();
    public DbSet<SystemNotification> SystemNotifications => Set<SystemNotification>();
    public DbSet<AvailabilityCalendar> AvailabilityCalendars => Set<AvailabilityCalendar>();
    public DbSet<EventPromotion> EventPromotions => Set<EventPromotion>();
    public DbSet<NavMenuEntry> NavMenuEntries => Set<NavMenuEntry>();
    public DbSet<WebsiteSettings> WebsiteSettings => Set<WebsiteSettings>();

    // Phase 14: Integrated Utility Suite
    public DbSet<PublicReview> PublicReviews => Set<PublicReview>();
    public DbSet<NotificationRouting> NotificationRoutings => Set<NotificationRouting>();
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        modelBuilder.Entity<GFC.BlazorServer.Data.Entities.TimeProfile>(entity =>
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
            entity.ToTable("SystemSettings");
            // Ensure only one row exists (Id = 1)
            entity.HasData(new SystemSettings
            {
                Id = 1,
                LastUpdatedUtc = null
            });
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

        modelBuilder.Entity<GFC.BlazorServer.Data.Entities.DuesPayment>(entity =>
        {
            entity.ToTable("DuesPayments");
            entity.HasKey(d => new { d.MemberId, d.Year });
        });

        modelBuilder.Entity<Waiver>(entity =>
        {
            entity.ToTable("Waivers");
            entity.HasIndex(w => new { w.MemberId, w.Year });
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
                new ReimbursementCategory { Id = 1, Name = "Costco", IsActive = true },
                new ReimbursementCategory { Id = 2, Name = "BJ's", IsActive = true },
                new ReimbursementCategory { Id = 3, Name = "Supplies", IsActive = true },
                new ReimbursementCategory { Id = 4, Name = "Misc", IsActive = true }
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
            entity.ToTable("AppUsers");
            entity.HasKey(u => u.UserId);
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
        });

        modelBuilder.Entity<HallRentalRequest>(entity =>
        {
            entity.ToTable("HallRentalRequests");
        });

        modelBuilder.Entity<StaffShift>(entity =>
        {
            entity.ToTable("StaffShifts");
        });

        modelBuilder.Entity<ShiftReport>(entity =>
        {
            entity.ToTable("ShiftReports");
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
            entity.ToTable("BarSaleEntries");
            entity.HasIndex(e => e.SaleDate);
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
}
