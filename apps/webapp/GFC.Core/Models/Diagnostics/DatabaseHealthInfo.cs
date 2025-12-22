// [NEW]
using System;

namespace GFC.Core.Models.Diagnostics
{
    /// <summary>
    /// Represents the health information of the database.
    /// </summary>
    public class DatabaseHealthInfo
    {
        /// <summary>
        /// Gets or sets the overall health status of the database.
        /// </summary>
        public HealthStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the number of active connections in the pool.
        /// </summary>
        public int ActiveConnections { get; set; }

        /// <summary>
        /// Gets or sets the number of idle connections in the pool.
        /// </summary>
        public int IdleConnections { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the connection pool.
        /// </summary>
        public int MaxPoolSize { get; set; }

        /// <summary>
        /// Gets or sets the database size in gigabytes.
        /// </summary>
        public double DatabaseSizeGb { get; set; }

        /// <summary>
        /// Gets or sets the date of the last backup.
        /// </summary>
        public DateTime? LastBackupDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the last backup was successful.
        /// </summary>
        public bool LastBackupSuccessful { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are pending migrations.
        /// </summary>
        public bool HasPendingMigrations { get; set; }

        /// Gets or sets the database connection response time in milliseconds.
        /// </summary>
        public long ConnectionResponseTimeMs { get; set; }

        /// <summary>
        /// Gets or sets a message providing more details about the health status.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database provider name.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the server name.
        /// </summary>
        public string ServerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the database size in bytes.
        /// </summary>
        public long DatabaseSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets the current schema version.
        /// </summary>
        public string CurrentSchemaVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of pending migrations.
        /// </summary>
        public int PendingMigrations { get; set; }

        /// <summary>
        /// Gets or sets when this health info was collected.
        /// </summary>
        public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
    }
}
