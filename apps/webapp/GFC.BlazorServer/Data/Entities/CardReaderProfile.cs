using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("CardReaderProfiles")]
public class CardReaderProfile
{
    [Key]
    public int Id { get; set; }

    public bool DigitsOnly { get; set; } = true;

    public int? MinLength { get; set; }

    public int? MaxLength { get; set; }

    [MaxLength(50)]
    public string? PrefixToTrim { get; set; }

    [MaxLength(50)]
    public string? SuffixToTrim { get; set; }

    public string? LastSampleRaw { get; set; }

    public string? LastSampleParsed { get; set; }

    public DateTime? LastUpdatedUtc { get; set; }
}
