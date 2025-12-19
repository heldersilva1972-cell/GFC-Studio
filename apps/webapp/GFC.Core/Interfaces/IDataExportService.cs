namespace GFC.Core.Interfaces
{
    public interface IDataExportService
    {
        byte[] ExportToExcel(ExportOptions options);
    }

    public class ExportOptions
    {
        public bool IncludeMembers { get; set; } = true;
        public bool IncludeDues { get; set; } = true;
        public bool IncludeKeyCards { get; set; } = true;
        public bool IncludePhysicalKeys { get; set; } = true;
        public bool IncludeLotteryShifts { get; set; } = true;
        public bool IncludeBoardMembers { get; set; } = true;
        public bool IncludeNpQueue { get; set; } = true;
        public bool IncludeLifeEligibility { get; set; } = true;
        public bool IncludeUsers { get; set; } = false;
    }
}

