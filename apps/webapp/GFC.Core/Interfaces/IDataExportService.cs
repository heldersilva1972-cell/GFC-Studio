namespace GFC.Core.Interfaces
{
    public interface IDataExportService
    {
        byte[] ExportToExcel(ExportOptions options);
        Task<Models.ImportResult> ImportFromExcelAsync(System.IO.Stream fileStream);
    }

    public class ExportOptions
    {
        public bool IncludeMembers { get; set; } = false;
        public bool IncludeDues { get; set; } = false;
        public bool IncludeKeyCards { get; set; } = false;
        public bool IncludePhysicalKeys { get; set; } = false;
        public bool IncludeLotteryShifts { get; set; } = false;
        public bool IncludeBoardMembers { get; set; } = false;
        public bool IncludeNpQueue { get; set; } = false;
        public bool IncludeLifeEligibility { get; set; } = false;
        public bool IncludeUsers { get; set; } = false;
        public bool IncludeSignInNumberDraw { get; set; } = false;
        public bool IncludeBarSales { get; set; } = false;
        public bool IncludePagePermissions { get; set; } = false;
    }
}

