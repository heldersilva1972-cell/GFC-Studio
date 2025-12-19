using System;

namespace GFC.Core.Services;

/// <summary>
/// Strongly typed configuration object describing how automatic database backups
/// should run for the ClubMembership SQL Server database.
/// </summary>
public class BackupConfig
{
    public string ServerInstance { get; set; } = "localhost";
    public string DatabaseName { get; set; } = "ClubMembership";
    public string BackupFolder { get; set; } = @"C:\GFC_Backups\ClubMembership";
    public int RetentionDays { get; set; } = 30;
    public TimeSpan DailyBackupTime { get; set; } = new TimeSpan(2, 0, 0);
    public bool IsConfigured { get; set; }
    public DateTime? LastBackupTime { get; set; }
}

