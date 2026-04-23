using GFC.Core.Models;

namespace GFC.Core.DTOs;

public class PosMenuDto
{
    public List<string> Categories { get; set; } = new();
    public List<PosItemDto> Items { get; set; } = new();
    public List<PosToken> Tokens { get; set; } = new();
}
