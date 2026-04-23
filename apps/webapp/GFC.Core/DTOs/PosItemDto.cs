namespace GFC.Core.DTOs;

public class PosItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public string Category { get; set; } = "MISC";
    public int DisplayOrder { get; set; }
}
