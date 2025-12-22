using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

    [Required]
    public int MemberId { get; set; }

    [Required]
    public int Year { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaidDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
