// [NEW]
namespace GFC.Core.Models;

public class StudioPage
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<StudioSection> Sections { get; set; } = new();
}
