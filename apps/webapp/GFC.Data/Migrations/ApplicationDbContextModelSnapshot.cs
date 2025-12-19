#nullable disable

using System;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GFC.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GFC.Core.Models.AppUser", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PasswordChangeRequired")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("AppUsers", (string)null);

                    b.Metadata.SetIsTableExcludedFromMigrations(true);
                });

            modelBuilder.Entity("GFC.Core.Models.AuditLogEntry", b =>
                {
                    b.Property<int>("AuditLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuditLogId"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PerformedByUserId")
                        .HasColumnType("int");

                    b.Property<int?>("TargetUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimestampUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("AuditLogId");

                    b.HasIndex("PerformedByUserId");

                    b.HasIndex("TargetUserId");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("GFC.Core.Models.KeyCard", b =>
                {
                    b.Property<int>("KeyCardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KeyCardId"));

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("KeyCards", (string)null);

                    b.Metadata.SetIsTableExcludedFromMigrations(true);
                });

            modelBuilder.Entity("GFC.Core.Models.Member", b =>
                {
                    b.Property<int>("MemberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MemberID"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Members", (string)null);

                    b.Metadata.SetIsTableExcludedFromMigrations(true);
                });

            modelBuilder.Entity("GFC.Core.Models.KeyHistoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ChangedUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int?>("NewKeyCardId")
                        .HasColumnType("int");

                    b.Property<int?>("OldKeyCardId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("NewKeyCardId");

                    b.HasIndex("OldKeyCardId");

                    b.ToTable("KeyHistory");
                });

            modelBuilder.Entity("GFC.Core.Models.TimeProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("TimeProfiles", (string)null, t => t.ExcludeFromMigrations());
                });

            modelBuilder.Entity("GFC.Core.Models.MemberDoorAccess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("DoorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastSyncedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastSyncResult")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int?>("TimeProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("TimeProfileId");

                    b.HasIndex("MemberId", "DoorId", "CardNumber");

                    b.ToTable("MemberDoorAccess");
                });

            modelBuilder.Entity("GFC.Core.Models.NPQueueEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeactivatedUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("NPQueueEntries");
                });

            modelBuilder.Entity("GFC.Core.Models.KeyHistoryEntry", b =>
                {
                    b.HasOne("GFC.Core.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .HasPrincipalKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GFC.Core.Models.KeyCard", "NewKeyCard")
                        .WithMany()
                        .HasForeignKey("NewKeyCardId")
                        .HasPrincipalKey("KeyCardId");

                    b.HasOne("GFC.Core.Models.KeyCard", "OldKeyCard")
                        .WithMany()
                        .HasForeignKey("OldKeyCardId")
                        .HasPrincipalKey("KeyCardId");

                    b.Navigation("Member");

                    b.Navigation("NewKeyCard");

                    b.Navigation("OldKeyCard");
                });

            modelBuilder.Entity("GFC.Core.Models.MemberDoorAccess", b =>
                {
                    b.HasOne("GFC.Core.Models.Member", null)
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .HasPrincipalKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GFC.Core.Models.TimeProfile", "TimeProfile")
                        .WithMany()
                        .HasForeignKey("TimeProfileId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("TimeProfile");
                });

            modelBuilder.Entity("GFC.Core.Models.AuditLogEntry", b =>
                {
                    b.HasOne("GFC.Core.Models.AppUser", "PerformedBy")
                        .WithMany()
                        .HasForeignKey("PerformedByUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("GFC.Core.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("TargetUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("PerformedBy");
                });

            modelBuilder.Entity("GFC.Core.Models.NPQueueEntry", b =>
                {
                    b.HasOne("GFC.Core.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .HasPrincipalKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });
#pragma warning restore 612, 618
        }
    }
}
