using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("Holidays")]
public class Holiday
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = "";

    [Required]
    [Column(TypeName = "date")]
    public DateOnly Date { get; set; }

    public bool IsRecurring { get; set; }
}

