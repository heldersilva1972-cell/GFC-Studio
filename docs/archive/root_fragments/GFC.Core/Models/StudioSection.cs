// [NEW]
namespace GFC.Core.Models;

public class StudioSection
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int Order { get; set; }
    public int StudioPageId { get; set; }
    public StudioPage Page { get; set; }
}
