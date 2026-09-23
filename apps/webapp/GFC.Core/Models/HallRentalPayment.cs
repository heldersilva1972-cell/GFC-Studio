using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class HallRentalPayment
    {
        [Key]
        public int Id { get; set; }

        public int HallRentalRequestId { get; set; }

        [ForeignKey("HallRentalRequestId")]
        public virtual HallRentalRequest? RentalRequest { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentType { get; set; } = "Rental Fee"; // "Security Deposit", "Rental Fee", "Refund", "Extra Hours", "Kitchen Fee", "Bar Fee"

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Today;

        [StringLength(50)]
        public string PaymentMethod { get; set; } = "Check"; // "Check", "Cash", "Card", "Venmo", "Dropped Off"

        [StringLength(50)]
        public string? ReferenceOrCheckNumber { get; set; }

        [StringLength(100)]
        public string? RecordedBy { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsTestPayment { get; set; } = false;
    }
}
