using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

/// <summary>
/// Represents a dues payment record for a member for a specific year.
/// </summary>
[Table("DuesPayments")]
public class DuesPayment : BaseEntity
{
    [Key]
    [Column("DuesPaymentID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("MemberID")]
    public int MemberId { get; set; }
    
    // Alias for legacy support
    [NotMapped]
    public int MemberID { get => MemberId; set => MemberId = value; }
    
    // Alias for legacy support
    [NotMapped]
    public int DuesPaymentID { get => Id; set => Id = value; }

    [Required]
    public int Year { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Amount { get; set; }

    public DateTime? PaidDate { get; set; }

    [MaxLength(50)]
    public string? PaymentType { get; set; } // CASH, CHECK, CARD, WAIVED

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int? RecordedByUserId { get; set; }

    [MaxLength(255)]
    public string? RecordedBy { get; set; }
}
