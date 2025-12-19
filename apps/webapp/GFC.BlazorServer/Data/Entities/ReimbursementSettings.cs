using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ReimbursementSettings")]
public class ReimbursementSettings
{
    [Key]
    public int Id { get; set; }

    [Required]
    public bool ReceiptRequired { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? NotificationRecipients { get; set; } // Comma-separated list of member IDs
}
