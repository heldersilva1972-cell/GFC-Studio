using OfficeOpenXml;
using OfficeOpenXml.Style;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.Data;
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

        public DataExportService(
            IMemberRepository memberRepository,
            IDuesRepository duesRepository,
            IKeyCardRepository keyCardRepository,
            IPhysicalKeyService physicalKeyService,
            ILotteryShiftService lotteryShiftService,
            IBoardRepository boardRepository,
            INpQueueService npQueueService,
            ILifeEligibilityService lifeEligibilityService,
            IUserManagementService userManagementService)
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
            
            if (options.IncludeBoardMembers)
                AddBoardMembersSheet(package);
            
            if (options.IncludeNpQueue)
                AddNpQueueSheet(package);
            
            if (options.IncludeLifeEligibility)
                AddLifeEligibilitySheet(package);
            
            if (options.IncludeUsers)
                AddUsersSheet(package);

            return package.GetAsByteArray();
        }

        private void AddMembersSheet(ExcelPackage package)
        {
            var worksheet = package.Workbook.Worksheets.Add("Members");
            var members = _memberRepository.GetAllMembers();

            // Headers
            worksheet.Cells[1, 1].Value = "Member ID";
            worksheet.Cells[1, 2].Value = "First Name";
            worksheet.Cells[1, 3].Value = "Last Name";
            worksheet.Cells[1, 4].Value = "Status";
            worksheet.Cells[1, 5].Value = "Address";
            worksheet.Cells[1, 6].Value = "City";
            worksheet.Cells[1, 7].Value = "State";
            worksheet.Cells[1, 8].Value = "Postal Code";
            worksheet.Cells[1, 9].Value = "Phone";
            worksheet.Cells[1, 10].Value = "Cell Phone";
            worksheet.Cells[1, 11].Value = "Email";
            worksheet.Cells[1, 12].Value = "Join Date";
            worksheet.Cells[1, 13].Value = "Accepted Date";
            worksheet.Cells[1, 14].Value = "Date of Birth";
            worksheet.Cells[1, 15].Value = "Non-Portuguese Origin";
            worksheet.Cells[1, 16].Value = "Notes";

            // Style headers
            using (var range = worksheet.Cells[1, 1, 1, 16])
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
                worksheet.Cells[row, 3].Value = member.LastName;
                worksheet.Cells[row, 4].Value = member.Status;
                worksheet.Cells[row, 5].Value = member.Address1;
                worksheet.Cells[row, 6].Value = member.City;
                worksheet.Cells[row, 7].Value = member.State;
                worksheet.Cells[row, 8].Value = member.PostalCode;
                worksheet.Cells[row, 9].Value = member.Phone;
                worksheet.Cells[row, 10].Value = member.CellPhone;
                worksheet.Cells[row, 11].Value = member.Email;
                worksheet.Cells[row, 12].Value = member.ApplicationDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 13].Value = member.AcceptedDate?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 14].Value = member.DateOfBirth?.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 15].Value = member.IsNonPortugueseOrigin ? "Yes" : "No";
                worksheet.Cells[row, 16].Value = member.Notes;
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
            var memberLookup = members.ToDictionary(m => m.MemberID, m => $"{m.FirstName} {m.LastName}".Trim());

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
            var keyCards = GetAllKeyCards();

            // Headers
            worksheet.Cells[1, 1].Value = "Key Card ID";
            worksheet.Cells[1, 2].Value = "Member ID";
            worksheet.Cells[1, 3].Value = "Card Number";
            worksheet.Cells[1, 4].Value = "Is Active";
            worksheet.Cells[1, 5].Value = "Created Date";
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
            foreach (var card in keyCards)
            {
                worksheet.Cells[row, 1].Value = card.KeyCardId;
                worksheet.Cells[row, 2].Value = card.MemberId;
                worksheet.Cells[row, 3].Value = card.CardNumber;
                worksheet.Cells[row, 4].Value = card.IsActive ? "Yes" : "No";
                worksheet.Cells[row, 5].Value = card.CreatedDate.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 6].Value = card.Notes;
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

        private List<KeyCard> GetAllKeyCards()
        {
            var keyCards = new List<KeyCard>();
            using var connection = Db.GetConnection();
            connection.Open();

            const string sql = @"
                SELECT KeyCardId, MemberID, CardNumber, Notes, 
                       CASE WHEN EXISTS (
                           SELECT 1 FROM MemberKeycardAssignments 
                           WHERE KeyCardId = kc.KeyCardId AND ToDate IS NULL
                       ) THEN 1 ELSE 0 END AS IsActive,
                       (SELECT MIN(FromDate) FROM MemberKeycardAssignments WHERE KeyCardId = kc.KeyCardId) AS CreatedDate
                FROM dbo.KeyCards kc
                ORDER BY KeyCardId";

            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                keyCards.Add(new KeyCard
                {
                    KeyCardId = (int)reader["KeyCardId"],
                    MemberId = (int)reader["MemberID"],
                    CardNumber = reader["CardNumber"]?.ToString() ?? string.Empty,
                    IsActive = reader["IsActive"] is DBNull ? false : (int)reader["IsActive"] == 1,
                    Notes = reader["Notes"] as string,
                    CreatedDate = reader["CreatedDate"] is DBNull ? DateTime.MinValue : (DateTime)reader["CreatedDate"]
                });
            }

            return keyCards;
        }
        public async Task<ImportResult> ImportFromExcelAsync(Stream fileStream)
        {
            return await Task.Run(() =>
            {
                var result = new ImportResult();
                try
                {
                    using var package = new ExcelPackage(fileStream);

                    // Process Members Sheet
                    var membersSheet = package.Workbook.Worksheets["Members"];
                    if (membersSheet != null)
                    {
                        ProcessMembersSheet(membersSheet, result);
                    }
                    else
                    {
                         // If no members sheet, we could check others, but for now we just log it as info if strictly looking for members.
                         // Instead, let's treat it as "nothing to do" or specific error if user expects it.
                         // But the user might be uploading just Key Cards.
                         // For this request, I'll focus on Members.
                         result.Errors.Add("No 'Members' worksheet found. Currently only Member updates are supported.");
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

        private void ProcessMembersSheet(ExcelWorksheet worksheet, ImportResult result)
        {
            int row = 2;
            while (worksheet.Cells[row, 1].Value != null)
            {
                result.ProcessedCount++;
                try
                {
                    // 1. Get Member ID
                    if (!int.TryParse(worksheet.Cells[row, 1].Value?.ToString(), out int memberId))
                    {
                        result.Errors.Add($"Row {row}: Invalid Member ID.");
                        result.ErrorCount++;
                        row++;
                        continue;
                    }

                    // 2. Find Existing Member
                    var member = _memberRepository.GetMemberById(memberId);
                    if (member == null)
                    {
                        result.Errors.Add($"Row {row}: Member ID {memberId} not found in database.");
                        result.ErrorCount++;
                        row++;
                        continue;
                    }

                    // 3. Update Fields
                    // Columns: 
                    // 1:ID, 2:First, 3:Last, 4:Status, 5:Addr, 6:City, 7:State, 8:Zip, 
                    // 9:Phone, 10:Cell, 11:Email, 12:JoinDate, 13:AcceptedDate, 14:DOB, 15:NonPort, 16:Notes

                    member.FirstName = GetString(worksheet, row, 2);
                    member.LastName = GetString(worksheet, row, 3);
                    member.Status = GetString(worksheet, row, 4);
                    member.Address1 = GetString(worksheet, row, 5);
                    member.City = GetString(worksheet, row, 6);
                    member.State = GetString(worksheet, row, 7);
                    member.PostalCode = GetString(worksheet, row, 8);
                    member.Phone = GetString(worksheet, row, 9);
                    member.CellPhone = GetString(worksheet, row, 10);
                    member.Email = GetString(worksheet, row, 11);
                    
                    member.ApplicationDate = GetDate(worksheet, row, 12);
                    member.AcceptedDate = GetDate(worksheet, row, 13);
                    member.DateOfBirth = GetDate(worksheet, row, 14);

                    var npString = GetString(worksheet, row, 15);
                    member.IsNonPortugueseOrigin = npString?.Equals("Yes", StringComparison.OrdinalIgnoreCase) == true;

                    member.Notes = GetString(worksheet, row, 16);

                    // 4. Save
                    _memberRepository.UpdateMember(member);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Row {row}: Error updating member - {ex.Message}");
                    result.ErrorCount++;
                }
                row++;
            }
        }

        private string GetString(ExcelWorksheet ws, int row, int col)
        {
            return ws.Cells[row, col].Value?.ToString()?.Trim() ?? string.Empty;
        }

        private DateTime? GetDate(ExcelWorksheet ws, int row, int col)
        {
            var val = ws.Cells[row, col].Value;
            if (val is DateTime dt) return dt;
            if (val is string s && DateTime.TryParse(s, out var parsed)) return parsed;
            return null;
        }
    }
}

