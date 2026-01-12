using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class ImportResult
    {
        public int ProcessedCount { get; set; }
        public int SuccessCount { get; set; }
        public int ErrorCount { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
