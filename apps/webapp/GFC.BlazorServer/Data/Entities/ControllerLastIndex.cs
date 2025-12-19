using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ControllerLastIndexes")]
public class ControllerLastIndex
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }

    public ControllerDevice? Controller { get; set; }

    public uint LastRecordIndex { get; set; }
}

