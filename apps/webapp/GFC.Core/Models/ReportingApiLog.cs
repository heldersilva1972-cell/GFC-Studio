using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class ReportingApiLog
    {
        [Key]
        public long Id { get; set; }

        public int? ApiKeyId { get; set; }

        [ForeignKey("ApiKeyId")]
        public ReportingApiKey? ApiKey { get; set; }

        public DateTime RequestTimestampUtc { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(200)]
        public string Endpoint { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? QueryParameters { get; set; }

        [Required]
        [MaxLength(50)]
        public string IpAddress { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ResolvedDeviceName { get; set; }

        [MaxLength(255)]
        public string? UserAgent { get; set; }

        public int DurationMs { get; set; }

        public int RecordCount { get; set; }

        public int ResponseStatusCode { get; set; }

        [MaxLength(500)]
        public string? ErrorMessage { get; set; }
    }
}
