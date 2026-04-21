using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GFC.Core.Services;

/// <summary>
/// Handles persistence of <see cref="BackupConfig"/> information to disk and
/// provides helper utilities (defaults, detection of latest backup file, etc.).
/// </summary>
public class BackupConfigService
{
    private const string ConfigFileName = "BackupConfig.json";

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Gets the full path to the JSON configuration file.
    /// </summary>
    public string GetConfigFilePath()
    {
        var baseFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "GFC",
            "ClubMembership");

        return Path.Combine(baseFolder, ConfigFileName);
    }

    /// <summary>
    /// Returns the default backup folder recommended by the application.
    /// </summary>
    public string GetDefaultBackupFolder()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "GFC", "ClubMembership", "Backups");
    }

    /// <summary>
    /// Loads the current configuration from disk or returns a default instance if no file exists.
    /// </summary>
    public BackupConfig Load()
    {
        try
        {
            var path = GetConfigFilePath();
            if (!File.Exists(path))
            {
                return new BackupConfig
                {
                    BackupFolder = GetDefaultBackupFolder()
                };
            }

            var json = File.ReadAllText(path);
            var config = JsonSerializer.Deserialize<BackupConfig>(json, _serializerOptions);
            return config ?? new BackupConfig { BackupFolder = GetDefaultBackupFolder() };
        }
        catch
        {
            // If the file is corrupt we fall back to defaults so the user can reconfigure.
            return new BackupConfig
            {
                BackupFolder = GetDefaultBackupFolder()
            };
        }
    }

    /// <summary>
    /// Persists the supplied configuration to disk, creating folders as needed.
    /// </summary>
    public void Save(BackupConfig config)
    {
        var path = GetConfigFilePath();
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (string.IsNullOrWhiteSpace(config.BackupFolder))
        {
            config.BackupFolder = GetDefaultBackupFolder();
        }

        var json = JsonSerializer.Serialize(config, _serializerOptions);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Returns whether the stored configuration has already been completed.
    /// </summary>
    public bool IsConfigured()
    {
        var config = Load();
        return config.IsConfigured;
    }

    /// <summary>
    /// Attempts to detect the timestamp of the newest .bak file inside the backup folder.
    /// </summary>
    public DateTime? GetLastBackupTimestamp(BackupConfig config)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(config.BackupFolder) || !Directory.Exists(config.BackupFolder))
            {
                return null;
            }

            var directory = new DirectoryInfo(config.BackupFolder);
            var newest = directory.GetFiles("*.bak", SearchOption.TopDirectoryOnly)
                .OrderByDescending(f => f.LastWriteTimeUtc)
                .FirstOrDefault();

            return newest?.LastWriteTime;
        }
        catch
        {
            return null;
        }
    }
}

