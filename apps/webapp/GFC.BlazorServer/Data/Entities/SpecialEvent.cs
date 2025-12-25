// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("SpecialEvents")]
public class SpecialEvent
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = "";

    [Required]
    [Column(TypeName = "date")]
    public DateOnly Date { get; set; }

    [Required]
    public int TimeProfileId { get; set; }

    [ForeignKey(nameof(TimeProfileId))]
    public TimeProfile? TimeProfile { get; set; }
}
