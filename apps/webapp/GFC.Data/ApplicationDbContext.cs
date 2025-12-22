using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GFC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<NPQueueEntry> NPQueueEntries { get; set; } = null!;
    public DbSet<KeyHistoryEntry> KeyHistoryEntries { get; set; } = null!;
    public DbSet<MemberDoorAccess> MemberDoorAccess { get; set; } = null!;
    public DbSet<AuditLogEntry> AuditLogs { get; set; } = null!;
    public DbSet<Camera> Cameras { get; set; } = null!;
    public DbSet<CameraEvent> CameraEvents { get; set; } = null!;
    public DbSet<Recording> Recordings { get; set; } = null!;
    public DbSet<CameraPermission> CameraPermissions { get; set; } = null!;
    public DbSet<CameraAuditLog> CameraAuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("AppUsers", t => t.ExcludeFromMigrations());
            entity.HasKey(u => u.UserId);
        });

        modelBuilder.Entity<AuditLogEntry>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(a => a.AuditLogId);

            entity.Property(a => a.TimestampUtc)
                .HasColumnType("datetime2");

            entity.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(a => a.PerformedBy)
                .WithMany()
                .HasForeignKey(a => a.PerformedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(a => a.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Members", t => t.ExcludeFromMigrations());
            entity.HasKey(m => m.MemberID);
        });

        modelBuilder.Entity<KeyCard>(entity =>
        {
            entity.ToTable("KeyCards", t => t.ExcludeFromMigrations());
            entity.HasKey(k => k.KeyCardId);
        });

        modelBuilder.Entity<TimeProfile>(entity =>
        {
            entity.ToTable("TimeProfiles", t => t.ExcludeFromMigrations());
            entity.HasKey(tp => tp.Id);
            entity.Property(tp => tp.Name)
                .HasMaxLength(200);
        });

        modelBuilder.Entity<NPQueueEntry>(entity =>
        {
            entity.ToTable("NPQueueEntries");

            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<KeyHistoryEntry>(entity =>
        {
            entity.ToTable("KeyHistory");

            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Member)
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.OldKeyCard)
                .WithMany()
                .HasForeignKey(e => e.OldKeyCardId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.NewKeyCard)
                .WithMany()
                .HasForeignKey(e => e.NewKeyCardId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<MemberDoorAccess>(entity =>
        {
            entity.ToTable("MemberDoorAccess");

            entity.HasKey(e => e.Id);

            entity.HasOne<Member>()
                .WithMany()
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.MemberId, e.DoorId, e.CardNumber });

            entity.Property(e => e.CardNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.LastSyncResult)
                .HasMaxLength(500);

            entity.HasOne(e => e.TimeProfile)
                .WithMany()
                .HasForeignKey(e => e.TimeProfileId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Camera>(entity =>
        {
            entity.ToTable("Cameras");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.RtspUrl).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<CameraEvent>(entity =>
        {
            entity.ToTable("CameraEvents");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Camera)
                .WithMany()
                .HasForeignKey(e => e.CameraId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Recording>(entity =>
        {
            entity.ToTable("Recordings");
            entity.HasKey(r => r.Id);
            entity.HasOne(r => r.Camera)
                .WithMany()
                .HasForeignKey(r => r.CameraId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CameraPermission>(entity =>
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

        modelBuilder.Entity<CameraAuditLog>(entity =>
        {
            entity.ToTable("CameraAuditLogs");
            entity.HasKey(l => l.Id);
            entity.HasOne(l => l.Camera)
                .WithMany()
                .HasForeignKey(l => l.CameraId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
