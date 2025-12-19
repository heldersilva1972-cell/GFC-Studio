using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ReimbursementCategories")]
public class ReimbursementCategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<ReimbursementItem> Items { get; set; } = new List<ReimbursementItem>();
}
