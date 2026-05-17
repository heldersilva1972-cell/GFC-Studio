using GFC.Core.Models;

namespace GFC.Core.DTOs;

public class PosMenuDto
{
    public List<string> Categories { get; set; } = new();
    public List<PosItemDto> Items { get; set; } = new();
    public List<PosToken> Tokens { get; set; } = new();
    public List<ActiveEvent> ActiveEvents { get; set; } = new();
    public List<EventTemplate> EventTemplates { get; set; } = new();
    public List<string> ModifierCategories { get; set; } = new();
    public List<PosModifierDto> Modifiers { get; set; } = new();
}

public class PosModifierDto
{
    public int ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal PourSize { get; set; }
}
