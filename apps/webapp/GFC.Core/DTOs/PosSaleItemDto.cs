namespace GFC.Core.DTOs;

public class PosSaleItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public System.Collections.Generic.List<PosSaleItemDto> Modifiers { get; set; } = new();
    public bool IsComped { get; set; }
    public string? CompType { get; set; }
    public decimal OriginalPrice { get; set; }
}
