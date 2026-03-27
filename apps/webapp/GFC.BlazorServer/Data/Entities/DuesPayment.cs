using System.ComponentModel.DataAnnotations;
using GFC.Core.Models;

namespace GFC.BlazorServer.Data.Entities;

public class DuesPayment : BaseEntity
{
    [Required]
    public int MemberId { get; set; }

    [Required]
    public int Year { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaidDate { get; set; }

    [MaxLength(50)]
    public string? PaymentType { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int? RecordedByUserId { get; set; }

    [MaxLength(255)]
    public string? RecordedBy { get; set; }
}
