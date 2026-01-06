using System;

namespace GFC.Core.DTOs;

/// <summary>
/// DTO representing information about a database backup.
/// </summary>
public class BackupInfoDto
{
    public int Id { get; set; }
    
    public string FileName { get; set; } = string.Empty;
    
    public long FileSizeBytes { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    
    public string CreatedByUsername { get; set; } = string.Empty;
    
    public string BackupType { get; set; } = string.Empty;
    
    public string? Notes { get; set; }
    
    public bool FileExists { get; set; }
    
    /// <summary>
    /// Human-readable file size (e.g., "33.3 MB").
    /// </summary>
    public string FileSizeFormatted
    {
        get
        {
            if (FileSizeBytes < 1024)
                return $"{FileSizeBytes} B";
            if (FileSizeBytes < 1024 * 1024)
                return $"{FileSizeBytes / 1024.0:F1} KB";
            if (FileSizeBytes < 1024 * 1024 * 1024)
                return $"{FileSizeBytes / (1024.0 * 1024.0):F1} MB";
            return $"{FileSizeBytes / (1024.0 * 1024.0 * 1024.0):F1} GB";
        }
    }
}
