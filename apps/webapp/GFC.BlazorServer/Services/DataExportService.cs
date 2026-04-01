using OfficeOpenXml;
using OfficeOpenXml.Style;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.Data;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Drawing;

namespace GFC.BlazorServer.Services
{
    public class DataExportService : IDataExportService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IDuesRepository _duesRepository;
        private readonly IKeyCardRepository _keyCardRepository;
        private readonly IPhysicalKeyService _physicalKeyService;
        private readonly ILotteryShiftService _lotteryShiftService;
        private readonly IBoardRepository _boardRepository;
        private readonly INpQueueService _npQueueService;
        private readonly ILifeEligibilityService _lifeEligibilityService;
        private readonly IUserManagementService _userManagementService;
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;

        public DataExportService(
            IMemberRepository memberRepository,
            IDuesRepository duesRepository,
            IKeyCardRepository keyCardRepository,
            IPhysicalKeyService physicalKeyService,
            ILotteryShiftService lotteryShiftService,
            IBoardRepository boardRepository,
            INpQueueService npQueueService,
            ILifeEligibilityService lifeEligibilityService,
            IUserManagementService userManagementService,
            IDbContextFactory<GfcDbContext> dbFactory)
        {
            _memberRepository = memberRepository;
            _duesRepository = duesRepository;
            _keyCardRepository = keyCardRepository;
            _physicalKeyService = physicalKeyService;
            _lotteryShiftService = lotteryShiftService;
            _boardRepository = boardRepository;
            _npQueueService = npQueueService;
            _lifeEligibilityService = lifeEligibilityService;
            _userManagementService = userManagementService;
            _dbFactory = dbFactory;
            
            // Set EPPlus license context (required for non-commercial use)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public byte[] ExportToExcel(ExportOptions options)
        {
            using var package = new ExcelPackage();
            
            if (options.IncludeMembers)
                AddMembersSheet(package);
            
            if (options.IncludeDues)
                AddDuesSheet(package);
            
            if (options.IncludeKeyCards)
                AddKeyCardsSheet(package);
            
            if (options.IncludePhysicalKeys)
                AddPhysicalKeysSheet(package);
            
            if (options.IncludeLotteryShifts)
                AddLotteryShiftsSheet(package);
            
            if (options.IncludeBarSales)
                AddBarSalesSheet(package);
            
            if (options.IncludeBoardMembers)
                AddBoardMembersSheet(package);
            
            if (options.IncludeNpQueue)
                AddNpQueueSheet(package);
            
            if (options.IncludeLifeEligibility)
                AddLifeEligibilitySheet(package);
            
            if (options.IncludeUsers)
                AddUsersSheet(package);
            
            if (options.IncludeSignInNumberDraw)
                AddSignInNumberDrawSheet(package);

            return package.GetAsByteArray();
        }

        private void AddMembersSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Members");
            var members = _memberRepository.GetAllMembers();
            
            var currentYear = DateTime.Today.Year;
            var currentYearDues = _duesRepository.GetDuesForYear(currentYear);
            var paidMemberIds = currentYearDues.Where(d => d.PaymentType != "UNPAID").Select(d => d.MemberID).ToHashSet();

            // Headers
            worksheet.Cells[1, 1].Value = "Member ID";
            worksheet.Cells[1, 2].Value = "First Name";
            worksheet.Cells[1, 3].Value = "Middle Name";
            worksheet.Cells[1, 4].Value = "Last Name";
            worksheet.Cells[1, 5].Value = "Suffix";
            worksheet.Cells[1, 6].Value = "Status";
            worksheet.Cells[1, 7].Value = "Address";
            worksheet.Cells[1, 8].Value = "City";
            worksheet.Cells[1, 9].Value = "State";
            worksheet.Cells[1, 10].Value = "Postal Code";
            worksheet.Cells[1, 11].Value = "Phone";
            worksheet.Cells[1, 12].Value = "Cell Phone";
            worksheet.Cells[1, 13].Value = "Email";
            worksheet.Cells[1, 14].Value = "Join Date";
            worksheet.Cells[1, 15].Value = "Accepted Date";
            worksheet.Cells[1, 16].Value = "Date of Birth";
            worksheet.Cells[1, 17].Value = "Non-Portuguese Origin";
            worksheet.Cells[1, 18].Value = "Notes";
            worksheet.Cells[1, 19].Value = "Current Year Paid";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 19])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var member in members)
            {
                worksheet.Cells[row, 1].Value = member.MemberID;
                worksheet.Cells[row, 2].Value = member.FirstName;
                worksheet.Cells[row, 3].Value = member.MiddleName;
                worksheet.Cells[row, 4].Value = member.LastName;
                worksheet.Cells[row, 5].Value = member.Suffix;
                worksheet.Cells[row, 6].Value = member.Status;
                worksheet.Cells[row, 7].Value = member.Address1;
                worksheet.Cells[row, 8].Value = member.City;
                worksheet.Cells[row, 9].Value = member.State;
                worksheet.Cells[row, 10].Value = member.PostalCode;
                worksheet.Cells[row, 11].Value = member.Phone;
                worksheet.Cells[row, 12].Value = member.CellPhone;
                worksheet.Cells[row, 13].Value = member.Email;
                worksheet.Cells[row, 14].Value = member.ApplicationDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 15].Value = member.AcceptedDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 16].Value = member.DateOfBirth?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 17].Value = member.IsNonPortugueseOrigin ? "Yes" : "No";
                worksheet.Cells[row, 18].Value = member.Notes;
                worksheet.Cells[row, 19].Value = paidMemberIds.Contains(member.MemberID) ? "Yes" : "No";
                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddDuesSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Dues Payments");
            var dues = _duesRepository.GetAllDues();
            
            // Get all members to create a lookup dictionary
            var members = _memberRepository.GetAllMembers();
            var memberLookup = members.ToDictionary(m => m.MemberID, m => FormatMemberName(m));

            // Headers
            worksheet.Cells[1, 1].Value = "Payment ID";
            worksheet.Cells[1, 2].Value = "Member Name";
            worksheet.Cells[1, 3].Value = "Year";
            worksheet.Cells[1, 4].Value = "Amount";
            worksheet.Cells[1, 5].Value = "Paid Date";
            worksheet.Cells[1, 6].Value = "Payment Type";
            worksheet.Cells[1, 7].Value = "Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var due in dues)
            {
                worksheet.Cells[row, 1].Value = due.DuesPaymentID;
                worksheet.Cells[row, 2].Value = memberLookup.TryGetValue(due.MemberID, out var memberName) ? memberName : $"Member ID: {due.MemberID}";
                worksheet.Cells[row, 3].Value = due.Year;
                worksheet.Cells[row, 4].Value = due.Amount;
                worksheet.Cells[row, 5].Value = due.PaidDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 6].Value = due.PaymentType;
                worksheet.Cells[row, 7].Value = due.Notes;
                row++;
            }

            // Format amount column
            worksheet.Cells[2, 4, row - 1, 4].Style.Numberformat.Format = "$#,##0.00";
            
            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddKeyCardsSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Key Cards");
            var keyCards = _keyCardRepository.GetAll();

            var members = _memberRepository.GetAllMembers();
            var memberLookup = members.ToDictionary(m => m.MemberID, m => FormatMemberName(m));

            // Headers
            worksheet.Cells[1, 1].Value = "Key Card ID";
            worksheet.Cells[1, 2].Value = "Member ID";
            worksheet.Cells[1, 3].Value = "Member Name";
            worksheet.Cells[1, 4].Value = "Card Number";
            worksheet.Cells[1, 5].Value = "Is Active";
            worksheet.Cells[1, 6].Value = "Created Date";
            worksheet.Cells[1, 7].Value = "Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var card in keyCards)
            {
                worksheet.Cells[row, 1].Value = card.KeyCardId;
                worksheet.Cells[row, 2].Value = card.MemberId;
                worksheet.Cells[row, 3].Value = memberLookup.TryGetValue(card.MemberId, out var memberName) ? memberName : $"Unknown ({card.MemberId})";
                worksheet.Cells[row, 4].Value = card.CardNumber;
                worksheet.Cells[row, 5].Value = card.IsActive ? "Yes" : "No";
                worksheet.Cells[row, 6].Value = card.CreatedDate > DateTime.MinValue ? card.CreatedDate.ToString("yyyy-MM-dd") : string.Empty;
                worksheet.Cells[row, 7].Value = card.Notes;
                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddPhysicalKeysSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Physical Keys");
            var keys = _physicalKeyService.GetAllKeys();

            // Headers
            worksheet.Cells[1, 1].Value = "Key ID";
            worksheet.Cells[1, 2].Value = "Member Name";
            worksheet.Cells[1, 3].Value = "Issued Date";
            worksheet.Cells[1, 4].Value = "Issued By";
            worksheet.Cells[1, 5].Value = "Returned Date";
            worksheet.Cells[1, 6].Value = "Returned By";
            worksheet.Cells[1, 7].Value = "Status";
            worksheet.Cells[1, 8].Value = "Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 8])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var key in keys)
            {
                worksheet.Cells[row, 1].Value = key.PhysicalKeyID;
                worksheet.Cells[row, 2].Value = key.MemberName;
                worksheet.Cells[row, 3].Value = key.IssuedDate.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 4].Value = key.IssuedBy ?? "--";
                worksheet.Cells[row, 5].Value = key.ReturnedDate?.ToString("yyyy-MM-dd") ?? "--";
                worksheet.Cells[row, 6].Value = key.ReturnedBy ?? "--";
                worksheet.Cells[row, 7].Value = key.IsReturned ? "Returned" : "Active";
                worksheet.Cells[row, 8].Value = key.Notes ?? "--";
                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddLotteryShiftsSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Lottery Shifts");
            var shifts = _lotteryShiftService.GetAllShifts();

            // Headers
            worksheet.Cells[1, 1].Value = "Shift ID";
            worksheet.Cells[1, 2].Value = "Employee Name";
            worksheet.Cells[1, 3].Value = "Date Worked";
            worksheet.Cells[1, 4].Value = "Starting Cash";
            worksheet.Cells[1, 5].Value = "Ending Cash";
            worksheet.Cells[1, 6].Value = "Total Sales";
            worksheet.Cells[1, 7].Value = "Total Payouts";
            worksheet.Cells[1, 8].Value = "Total Cancels";
            worksheet.Cells[1, 9].Value = "Net Sales";
            worksheet.Cells[1, 10].Value = "Expected Cash";
            worksheet.Cells[1, 11].Value = "Variance";
            worksheet.Cells[1, 12].Value = "Reconciled";
            worksheet.Cells[1, 13].Value = "Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 13])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var shift in shifts)
            {
                worksheet.Cells[row, 1].Value = shift.ShiftId;
                worksheet.Cells[row, 2].Value = shift.EmployeeName;
                worksheet.Cells[row, 3].Value = shift.ShiftDate.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 4].Value = shift.StartingCash;
                worksheet.Cells[row, 5].Value = shift.EndingCash;
                worksheet.Cells[row, 6].Value = shift.TotalSales;
                worksheet.Cells[row, 7].Value = shift.TotalPayouts;
                worksheet.Cells[row, 8].Value = shift.TotalCancels;
                worksheet.Cells[row, 9].Value = shift.NetSales;
                worksheet.Cells[row, 10].Value = shift.ExpectedCash;
                worksheet.Cells[row, 11].Value = shift.Variance;
                worksheet.Cells[row, 12].Value = shift.IsReconciled ? "Yes" : "No";
                worksheet.Cells[row, 13].Value = shift.Notes;
                row++;
            }

            // Format currency columns
            for (int col = 4; col <= 11; col++)
            {
                worksheet.Cells[2, col, row - 1, col].Style.Numberformat.Format = "$#,##0.00";
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddBarSalesSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Bar Sales & Hours");
            using var db = _dbFactory.CreateDbContext();
            var entries = db.BarSaleEntries.OrderByDescending(e => (e.AdjustedSaleDate ?? e.SaleDate)).ToList();

            // Headers
            worksheet.Cells[1, 1].Value = "Entry ID";
            worksheet.Cells[1, 2].Value = "Recorded Date";
            worksheet.Cells[1, 3].Value = "Shift Type";
            worksheet.Cells[1, 4].Value = "Assigned Location";
            worksheet.Cells[1, 5].Value = "Total Net Sales";
            worksheet.Cells[1, 6].Value = "Total Hours Worked";
            worksheet.Cells[1, 7].Value = "Submitted By";
            worksheet.Cells[1, 8].Value = "Submitted Time (UTC)";
            worksheet.Cells[1, 9].Value = "Employee Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 9])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var entry in entries)
            {
                worksheet.Cells[row, 1].Value = entry.Id;
                worksheet.Cells[row, 2].Value = (entry.AdjustedSaleDate ?? entry.SaleDate).ToString("yyyy-MM-dd");
                worksheet.Cells[row, 3].Value = entry.Shift;
                worksheet.Cells[row, 4].Value = entry.IsRentalHall ? "Upstairs Hall" : "Downstairs Bar";
                worksheet.Cells[row, 5].Value = entry.TotalSales;
                worksheet.Cells[row, 6].Value = entry.TotalHours;
                worksheet.Cells[row, 7].Value = entry.CreatedBy;
                worksheet.Cells[row, 8].Value = entry.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cells[row, 9].Value = entry.Notes;
                row++;
            }

            // Format Currency
            worksheet.Cells[2, 5, Math.Max(2, row - 1), 5].Style.Numberformat.Format = "$#,##0.00";
            
            // Format Number
            worksheet.Cells[2, 6, Math.Max(2, row - 1), 6].Style.Numberformat.Format = "#,##0.00";

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddBoardMembersSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Board Members");
            var boardAssignments = _boardRepository.GetAllAssignments();

            // Headers
            worksheet.Cells[1, 1].Value = "Assignment ID";
            worksheet.Cells[1, 2].Value = "Member ID";
            worksheet.Cells[1, 3].Value = "Member Name";
            worksheet.Cells[1, 4].Value = "Position";
            worksheet.Cells[1, 5].Value = "Term Year";
            worksheet.Cells[1, 6].Value = "Start Date";
            worksheet.Cells[1, 7].Value = "End Date";
            worksheet.Cells[1, 8].Value = "Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 8])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var assignment in boardAssignments)
            {
                worksheet.Cells[row, 1].Value = assignment.AssignmentID;
                worksheet.Cells[row, 2].Value = assignment.MemberID;
                worksheet.Cells[row, 3].Value = assignment.MemberName;
                worksheet.Cells[row, 4].Value = assignment.PositionName;
                worksheet.Cells[row, 5].Value = assignment.TermYear;
                worksheet.Cells[row, 6].Value = assignment.StartDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 7].Value = assignment.EndDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 8].Value = assignment.Notes;
                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddNpQueueSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("NP Queue");
            var queueItems = _npQueueService.GetQueueAsync().GetAwaiter().GetResult();

            // Headers
            worksheet.Cells[1, 1].Value = "Position";
            worksheet.Cells[1, 2].Value = "Member ID";
            worksheet.Cells[1, 3].Value = "Full Name";
            worksheet.Cells[1, 4].Value = "Application Date";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 4])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var item in queueItems)
            {
                worksheet.Cells[row, 1].Value = item.Position;
                worksheet.Cells[row, 2].Value = item.MemberId;
                worksheet.Cells[row, 3].Value = item.FullName;
                worksheet.Cells[row, 4].Value = item.ApplicationDate?.ToString("yyyy-MM-dd");
                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddLifeEligibilitySheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Life Eligibility");
            var eligibleMembers = _lifeEligibilityService.GetEligibleMembersAsync(false).GetAwaiter().GetResult();

            // Headers
            worksheet.Cells[1, 1].Value = "Member ID";
            worksheet.Cells[1, 2].Value = "Full Name";
            worksheet.Cells[1, 3].Value = "Age";
            worksheet.Cells[1, 4].Value = "Regular Since";
            worksheet.Cells[1, 5].Value = "Eligibility Date";
            worksheet.Cells[1, 6].Value = "Eligible Now";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var member in eligibleMembers)
            {
                worksheet.Cells[row, 1].Value = member.MemberId;
                worksheet.Cells[row, 2].Value = member.FullName;
                worksheet.Cells[row, 3].Value = member.Age;
                worksheet.Cells[row, 4].Value = member.RegularSince?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 5].Value = member.EligibilityDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 6].Value = member.EligibleNow ? "Yes" : "No";
                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddUsersSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Users");
            var users = _userManagementService.GetAllUsers();

            // Headers
            worksheet.Cells[1, 1].Value = "User ID";
            worksheet.Cells[1, 2].Value = "Username";
            worksheet.Cells[1, 3].Value = "Is Admin";
            worksheet.Cells[1, 4].Value = "Is Active";
            worksheet.Cells[1, 5].Value = "Member ID";
            worksheet.Cells[1, 6].Value = "Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // Data
            int row = 2;
            foreach (var user in users)
            {
                worksheet.Cells[row, 1].Value = user.UserId;
                worksheet.Cells[row, 2].Value = user.Username;
                worksheet.Cells[row, 3].Value = user.IsAdmin ? "Yes" : "No";
                worksheet.Cells[row, 4].Value = user.IsActive ? "Yes" : "No";
                worksheet.Cells[row, 5].Value = user.MemberId;
                worksheet.Cells[row, 6].Value = user.Notes;
                row++;
            }

            // Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        }

        private void AddSignInNumberDrawSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("updated list for sign-in number draw");
            var members = _memberRepository.GetAllMembers();
            
            var currentYear = DateTime.Today.Year;
            var currentYearDues = _duesRepository.GetDuesForYear(currentYear);
            
            var paidMemberIds = currentYearDues
                .Where(d => !string.Equals(d.PaymentType, "UNPAID", StringComparison.OrdinalIgnoreCase))
                .Select(d => d.MemberID)
                .ToHashSet();

            // Add Header with Generation Date (to tell if list is up to date)
            worksheet.Cells[1, 1, 1, 5].Merge = true;
            worksheet.Cells[1, 1].Value = $"List Generated On: {DateTime.Now:MMMM dd, yyyy HH:mm}";
            worksheet.Cells[1, 1].Style.Font.Italic = true;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            // Headers
            worksheet.Cells[2, 1].Value = "Member ID";
            worksheet.Cells[2, 2].Value = "First Name";
            worksheet.Cells[2, 3].Value = "Middle Name";
            worksheet.Cells[2, 4].Value = "Last Name";
            worksheet.Cells[2, 5].Value = $"Dues Status ({currentYear})";

            // Style headers
            using (var range = worksheet.Cells[2, 1, 2, 5])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Data
            int row = 3;
            foreach (var member in members.OrderBy(m => m.MemberID))
            {
                var normalizedStatus = GFC.Core.BusinessRules.MemberStatusHelper.NormalizeStatus(member.Status);
                bool isPaid = normalizedStatus is "LIFE" or "BOARD" || paidMemberIds.Contains(member.MemberID);

                // Filter: Only include members that are "paid"
                if (!isPaid) continue;

                // Member ID (Bold & Larger)
                var idCell = worksheet.Cells[row, 1];
                idCell.Value = member.MemberID;
                idCell.Style.Font.Bold = true;
                idCell.Style.Font.Size = 14;

                worksheet.Cells[row, 2].Value = member.FirstName;
                worksheet.Cells[row, 3].Value = member.MiddleName;
                worksheet.Cells[row, 4].Value = member.LastName;
                
                // Show Year instead of just "Yes"
                worksheet.Cells[row, 5].Value = $"{currentYear} PAID";
                
                // Center all columns in the row
                worksheet.Cells[row, 1, row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[row, 1, row, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                
                row++;
            }

            // Auto-fit columns
            if (worksheet.Dimension != null)
            {
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            }

            // Printer settings for hard copies
            // Repeat the Generation Date (Row 1) and Column Headers (Row 2) on every page
            worksheet.PrinterSettings.RepeatRows = new ExcelAddress("$1:$2");
            
            // Add page numbers to the footer (e.g. "Page 1 of 3")
            worksheet.HeaderFooter.OddFooter.RightAlignedText = "Page &P of &N";
            
            // Ensure data fits within the width of a standard page
            worksheet.PrinterSettings.FitToPage = true;
            worksheet.PrinterSettings.FitToWidth = 1;
            worksheet.PrinterSettings.FitToHeight = 0; // Automatic number of pages
        }


        public async Task<ImportResult> ImportFromExcelAsync(Stream fileStream)
        {
            return await Task.Run(() =>
            {
                var result = new ImportResult();
                try
                {
                    using var package = new ExcelPackage(fileStream);

                    // Process Members Sheet (case-insensitive, accept common variations)
                    var membersSheet = FindWorksheet(package.Workbook, "Members", "Member", "Member List", "MemberList");
                    bool anyProcessed = false;

                    if (membersSheet != null)
                    {
                        ProcessMembersSheet(membersSheet, result);
                        anyProcessed = true;
                    }

                    // Process Bar Sales Sheet
                    var barSalesSheet = FindWorksheet(package.Workbook, "Bar Sales & Hours", "Bar Sales", "BarSales");
                    if (barSalesSheet != null)
                    {
                        ProcessBarSalesSheet(barSalesSheet, result);
                        anyProcessed = true;
                    }

                    if (!anyProcessed)
                    {
                         result.Errors.Add("No recognizable worksheet found. Please ensure your Excel file has a worksheet named 'Members' or 'Bar Sales & Hours'.");
                         result.ErrorCount++;
                    }
                }
                catch (Exception ex)
                {
                     result.Errors.Add($"Critical error reading Excel file: {ex.Message}");
                     result.ErrorCount++;
                }

                return result;
            });
        }


        /// <summary>
        /// Finds a worksheet by name, case-insensitive, trying multiple common variations.
        /// </summary>
        private ExcelWorksheet? FindWorksheet(ExcelWorkbook workbook, params string[] possibleNames)
        {
            foreach (var name in possibleNames)
            {
                var sheet = workbook.Worksheets.FirstOrDefault(ws => 
                    ws.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (sheet != null) return sheet;
            }
            return null;
        }

        private void ProcessMembersSheet(ExcelWorksheet worksheet, ImportResult result)
        {
            int row = 2;
            // Continue until we hit a row where FirstName AND LastName are both empty
            while (!IsRowEmpty(worksheet, row))
            {
                result.ProcessedCount++;
                try
                {
                    // 1. Get Member ID
                    int memberId = 0;
                    var idString = worksheet.Cells[row, 1].Value?.ToString();

                    // If ID is provided, try parse. If blank, treating as 0 (New Member)
                    if (!string.IsNullOrWhiteSpace(idString) && !int.TryParse(idString, out memberId))
                    {
                         // Check if maybe it's just "New" or literal text
                         if (!string.Equals(idString, "New", StringComparison.OrdinalIgnoreCase))
                         {
                             result.Errors.Add($"Row {row}: Invalid Member ID '{idString}'. Use 0 or leave blank for new members.");
                             result.ErrorCount++;
                             row++;
                             continue;
                         }
                         memberId = 0;
                    }
                    
                    // 2. Find Existing Member (if ID > 0)
                    Member member = null;
                    if (memberId > 0)
                    {
                        member = _memberRepository.GetMemberById(memberId);
                    }
                    
                    if (member == null)
                    {
                        // Create New
                        member = new Member();
                        MapMemberFromRow(member, worksheet, row);
                        // Note: Database will assign a new ID, we cannot force the Excel ID without IDENTITY_INSERT
                        _memberRepository.InsertMember(member);
                        result.CreatedCount++;
                        result.SuccessCount++;
                    }
                    else
                    {
                        // Update Existing
                        MapMemberFromRow(member, worksheet, row);
                        _memberRepository.UpdateMember(member);
                        result.UpdatedCount++;
                        result.SuccessCount++;
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Row {row}: Error processing member - {ex.Message}");
                    result.ErrorCount++;
                }
                row++;
            }
        }

        /// <summary>
        /// Checks if a row is empty by looking at FirstName (col 2) and LastName (col 4).
        /// </summary>
        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            var firstName = worksheet.Cells[row, 2].Value?.ToString();
            var lastName = worksheet.Cells[row, 4].Value?.ToString();
            return string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName);
        }

        private void ProcessBarSalesSheet(ExcelWorksheet worksheet, ImportResult result)
        {
            using var db = _dbFactory.CreateDbContext();
            int row = 2; // skip header
            
            while (!IsBarSalesRowEmpty(worksheet, row))
            {
                result.ProcessedCount++;
                try
                {
                    // 1. Get Entry ID
                    int entryId = 0;
                    var idString = worksheet.Cells[row, 1].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(idString) && !int.TryParse(idString, out entryId))
                    {
                         // Treat literal "New" as ID=0
                         if (!string.Equals(idString, "New", StringComparison.OrdinalIgnoreCase))
                         {
                             result.Errors.Add($"Row {row}: Invalid Entry ID '{idString}'. Use 0 or leave blank for new entries.");
                             result.ErrorCount++;
                             row++;
                             continue;
                         }
                         entryId = 0;
                    }

                    // 2. Map standard fields
                    var dateVal = GetDate(worksheet, row, 2);
                    if (dateVal == null)
                    {
                        result.Errors.Add($"Row {row}: Valid Recorded Date is required.");
                        result.ErrorCount++;
                        row++;
                        continue;
                    }
                    
                    var shiftType = GetString(worksheet, row, 3);
                    if (string.IsNullOrWhiteSpace(shiftType)) shiftType = "Day";
                    
                    var location = GetString(worksheet, row, 4);
                    bool isRentalHall = location.Contains("Upstairs", StringComparison.OrdinalIgnoreCase) || 
                                        location.Contains("Hall", StringComparison.OrdinalIgnoreCase);

                    var salesString = GetString(worksheet, row, 5).Replace("$", "").Replace(",", "").Trim();
                    decimal.TryParse(salesString, out decimal totalSales);
                    
                    var hoursString = GetString(worksheet, row, 6).Replace(",", "").Trim();
                    decimal.TryParse(hoursString, out decimal totalHours);
                    
                    var submittedBy = GetString(worksheet, row, 7);
                    if (string.IsNullOrWhiteSpace(submittedBy)) submittedBy = "System Import";
                    
                    var notes = GetString(worksheet, row, 9);

                    // 3. Find By ID in DB
                    BarSaleEntry entry = null;
                    if (entryId > 0)
                    {
                        entry = db.BarSaleEntries.Find(entryId);
                    }
                    
                    if (entry == null)
                    {
                        // Create New
                        entry = new BarSaleEntry
                        {
                            SaleDate = dateVal.Value,
                            AdjustedSaleDate = dateVal.Value,
                            Shift = shiftType,
                            IsRentalHall = isRentalHall,
                            TotalSales = totalSales,
                            TotalHours = totalHours,
                            CreatedBy = submittedBy,
                            CreatedAt = DateTime.UtcNow,
                            Notes = notes
                        };
                        db.BarSaleEntries.Add(entry);
                        result.CreatedCount++;
                        result.SuccessCount++;
                    }
                    else
                    {
                        // Update Existing
                        entry.AdjustedSaleDate = dateVal.Value;
                        entry.Shift = shiftType;
                        entry.IsRentalHall = isRentalHall;
                        entry.TotalSales = totalSales;
                        entry.TotalHours = totalHours;
                        entry.ModifiedBy = submittedBy;
                        entry.ModifiedAt = DateTime.UtcNow;
                        entry.Notes = notes;
                        
                        db.BarSaleEntries.Update(entry);
                        result.UpdatedCount++;
                        result.SuccessCount++;
                    }
                    
                    db.SaveChanges(); // Commit immediately so ID conflicts or other DB errors map to the exact row
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Row {row}: Error processing bar sale - {ex.Message}");
                    result.ErrorCount++;
                }
                row++;
            }
        }

        private bool IsBarSalesRowEmpty(ExcelWorksheet worksheet, int row)
        {
            var date = worksheet.Cells[row, 2].Value?.ToString();
            var sales = worksheet.Cells[row, 5].Value?.ToString();
            var hours = worksheet.Cells[row, 6].Value?.ToString();
            return string.IsNullOrWhiteSpace(date) && string.IsNullOrWhiteSpace(sales) && string.IsNullOrWhiteSpace(hours);
        }

        private void MapMemberFromRow(Member member, ExcelWorksheet worksheet, int row)
        {
            // Columns: 
            // 1:ID, 2:First, 3:Middle, 4:Last, 5:Suffix, 6:Status, 7:Addr, 8:City, 9:State, 10:Zip, 
            // 11:Phone, 12:Cell, 13:Email, 14:JoinDate, 15:AcceptedDate, 16:DOB, 17:NonPort, 18:Notes

            // Normalize and Format Fields (with safe defaults for required fields)
            var firstName = FormatName(GetString(worksheet, row, 2));
            var lastName = FormatName(GetString(worksheet, row, 4));
            var rawStatus = GetString(worksheet, row, 6);

            // Provide safe defaults if required fields are missing
            member.FirstName = string.IsNullOrWhiteSpace(firstName) ? "Unknown" : firstName;
            member.MiddleName = FormatName(GetString(worksheet, row, 3));
            member.LastName = string.IsNullOrWhiteSpace(lastName) ? "Unknown" : lastName;
            member.Suffix = GetString(worksheet, row, 5); // Suffix usually keeps user styling (Sr., III)

            // Normalize Status using Business Logic Rule (default to GUEST if empty)
            var normalizedStatus = GFC.Core.BusinessRules.MemberStatusHelper.NormalizeStatus(rawStatus);
            member.Status = string.IsNullOrWhiteSpace(normalizedStatus) ? "GUEST" : normalizedStatus;

            member.Address1 = FormatAddress(GetString(worksheet, row, 7));
            member.City = FormatName(GetString(worksheet, row, 8));
            member.State = GetString(worksheet, row, 9)?.Trim().ToUpperInvariant();
            member.PostalCode = GetPostalCode(worksheet, row, 10);
            member.Phone = FormatPhone(GetString(worksheet, row, 11));
            member.CellPhone = FormatPhone(GetString(worksheet, row, 12));
            var email = GetString(worksheet, row, 13);
            member.Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
            
            member.ApplicationDate = GetDate(worksheet, row, 14);
            member.AcceptedDate = GetDate(worksheet, row, 15);
            member.DateOfBirth = GetDate(worksheet, row, 16);

            member.IsNonPortugueseOrigin = GetBool(worksheet, row, 17);

            member.Notes = GetString(worksheet, row, 18);
        }

        private string GetString(ExcelWorksheet ws, int row, int col)
        {
            return ws.Cells[row, col].Value?.ToString()?.Trim() ?? string.Empty;
        }

        private string GetPostalCode(ExcelWorksheet ws, int row, int col)
        {
            var val = ws.Cells[row, col].Value;
            if (val == null) return string.Empty;

            var s = val.ToString()?.Trim() ?? string.Empty;

            // If it's a number and shorter than 5 digits, pad it to 5 (standard US zip)
            if (s.Length > 0 && s.Length < 5 && int.TryParse(s, out _))
            {
                return s.PadLeft(5, '0');
            }

            return s;
        }

        private DateTime? GetDate(ExcelWorksheet ws, int row, int col)
        {
            var val = ws.Cells[row, col].Value;
            
            // Case 1: Already a DateTime object (Excel stores dates as DateTime)
            if (val is DateTime dt) return dt;
            
            // Case 2: Excel serial date number (e.g., 44927 for 2023-01-01)
            if (val is double dbl)
            {
                try
                {
                    return DateTime.FromOADate(dbl);
                }
                catch
                {
                    return null;
                }
            }
            
            // Case 3: String representation
            if (val is string s && !string.IsNullOrWhiteSpace(s))
            {
                // Try standard parsing first
                if (DateTime.TryParse(s, out var parsed))
                {
                    return parsed;
                }
                
                // Try common formats with 2-digit years (e.g., "1/15/25", "01/15/25")
                string[] formats = new[]
                {
                    "M/d/yy", "MM/dd/yy", "M/dd/yy", "MM/d/yy",
                    "M-d-yy", "MM-dd-yy", "M-dd-yy", "MM-d-yy",
                    "M/d/yyyy", "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy",
                    "M-d-yyyy", "MM-dd-yyyy", "M-dd-yyyy", "MM-d-yyyy",
                    "yyyy-MM-dd", "yyyy/MM/dd"
                };
                
                if (DateTime.TryParseExact(s, formats, 
                    System.Globalization.CultureInfo.InvariantCulture, 
                    System.Globalization.DateTimeStyles.None, 
                    out var exactParsed))
                {
                    return exactParsed;
                }
            }
            
            return null;
        }

        private bool GetBool(ExcelWorksheet ws, int row, int col)
        {
            var val = ws.Cells[row, col].Value;
            if (val == null) return false;
            if (val is bool b) return b;

            var s = val.ToString()?.Trim().ToLowerInvariant() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(s)) return false;

            return s == "yes" || s == "y" || s == "true" || s == "1" || s == "sim" || s == "s" || s == "checked" || s == "x";
        }

        // --- Formatting Helpers (Matches AddMember.razor logic) ---

        // Format names: Capitalize first letter of each word, lowercase the rest
        private string? FormatName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            
            var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(name.Trim().ToLowerInvariant());
        }
        
        // Format addresses: Proper capitalization (same as FormatName but semantic naming)
        private string? FormatAddress(string? address)
        {
            return FormatName(address);
        }
        
        // Format phone numbers to xxx-xxx-xxxx if they are 10 digits
        private string? FormatPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return phone;
            
            // Remove allowed characters to check for digits
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            
            // Only reformat if it's exactly 10 digits (US Standard)
            if (digits.Length == 10)
            {
                return $"{digits.Substring(0, 3)}-{digits.Substring(3, 3)}-{digits.Substring(6)}";
            }
            
            return phone?.Trim();
        }
        private string FormatMemberName(Member member)
        {
            var first = member.FirstName?.Trim() ?? "";
            var last = member.LastName?.Trim() ?? "";
            var middle = member.MiddleName?.Trim() ?? "";
            var suffix = member.Suffix?.Trim() ?? "";

            var mFirst = first;
            var mLast = last;
            var mMiddle = string.IsNullOrWhiteSpace(middle) ? "" : " " + middle[0] + ".";
            var mSuffix = string.IsNullOrWhiteSpace(suffix) ? "" : " " + suffix;

            if (string.IsNullOrWhiteSpace(mLast)) return (mFirst + mMiddle + mSuffix).Trim();
            return $"{mLast}, {mFirst}{mMiddle}{mSuffix}".Trim().Replace("  ", " ");
        }
    }
}

