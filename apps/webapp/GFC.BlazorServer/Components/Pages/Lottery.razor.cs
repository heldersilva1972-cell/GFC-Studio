using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.Models.Finance;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using Microsoft.JSInterop;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Components.Pages
{
    public partial class Lottery : ComponentBase
    {
        [Inject]
        public ILotteryShiftService LotteryService { get; set; } = null!;

        [Inject]
        public GFC.BlazorServer.Services.CustomAuthenticationStateProvider AuthStateProvider { get; set; } = null!;

        [Inject]
        public ILogger<Lottery> Logger { get; set; } = null!;

        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        [Inject]
        public GFC.BlazorServer.Services.IFinancialAnalyticsService FinancialService { get; set; } = null!;

        [Inject]
        public IJSRuntime JS { get; set; } = null!;

        [Inject]
        public ILotterySettlementService LotterySettlementService { get; set; } = null!;

        [Inject]
        public ISystemSettingsService SettingsService { get; set; } = null!;

        [Inject]
        public IFinanceService FinanceService { get; set; } = null!;

        [Inject]
        public IDbContextFactory<GfcDbContext> DbFactory { get; set; } = null!;

        private List<LotteryShiftDto> _shifts = new();
        private List<LotteryShiftSummaryDto> _dailySummaries = new();
        private List<string> _employeeNames = new();
        private List<LotteryWeeklyStat> _weeklyCommissionsStats = new();
        private List<LotteryWeeklyStat> _analyticsWeeklyStats = new();
        private bool _showMetricCommissions = true;
        private bool _showMetricCashBonus = true;
        private bool _showMetricClaimsBonus = true;
        private bool _showMetricFees = false;
        private List<(string Username, string FullName)> _employeeMetadata = new();
        private bool _loading = true;
        private string _error = string.Empty;
        private string _modalError = string.Empty;
        private bool _showShiftModal = false;
        private bool _savingShift = false;
        private int? _editingShiftId = null;
        private bool _showDeleteModal = false;
        private int? _shiftToDelete = null;
        private bool _deletingShift = false;
        
        // Reassignment confirmation state
        private bool _showReassignConfirmation = false;
        private string _pendingUsername = string.Empty;
        private string _pendingFullName = string.Empty;
        private string _originalEmployeeName = string.Empty;
        private bool _showNoChangesModal = false; // DECISION MODAL STATE

        // Full Day Merge Confirmation State
        private bool _showFullDayMergePrompt = false;
        private decimal _existingDayShiftSales = 0;
        private string _existingDayShiftEmployee = string.Empty;
        
        private DateTime _filterStartDate = GetWeekStart(DateTime.Today);
        private DateTime _filterEndDate = GetWeekStart(DateTime.Today).AddDays(6);
        private string _filterEmployee = string.Empty;
        private bool? _showReconciled = null;
        private string _viewMode = "daily";
        private int _selectedYear = DateTime.Now.Year;
        private int _selectedMonth = DateTime.Now.Month;
        
        // Analytics State
        private LotteryAnalyticsStats _stats = new();
        private List<LotteryShift> _analyticsShifts = new();

        // Breakdown State
        private LotteryBreakdownStats _breakdownStats = new();
        private List<DailyBreakdownItem> _breakdownDailyItems = new();
        private HashSet<DateTime> _expandedBreakdownDays = new();
        private bool _showMetricEnvelope = true;
        private bool _showMetricVending = true;
        private bool _showMetricNetDue = false;
        private bool _showMetricSales = false;
        private bool _showMetricVariance = false;
        private bool _showMetricWeeklyDue = true;
        private bool _showMetricOnlineDue = false;
        private bool _showDataGuide = false;

        // Vending Machine State
        private List<LotteryVendingCollection> _vendingCollections = new();
        private LotteryVendingCollection _editingVendingCollection = new();
        private bool _showVendingModal = false;
        private bool _showVendingHistoryModal = false;
        private string _vendingChartMode = "monthly"; // "monthly" or "pickups"
        private bool _needsChartUpdate = false;

        private void OpenVendingHistoryModal() => _showVendingHistoryModal = true;
        private void CloseVendingHistoryModal() => _showVendingHistoryModal = false;

        private async Task SetVendingChartMode(string mode)
        {
            _vendingChartMode = mode;
            await UpdateVendingChart();
        }
        private void ToggleDataGuide() => _showDataGuide = !_showDataGuide;
        private string _breakdownRangeType = "month"; // "week", "month", "year", "custom"
        private DateTime _breakdownCustomStart = DateTime.Today.AddDays(-14);
        private DateTime _breakdownCustomEnd = DateTime.Today;

        private async Task SetBreakdownRangeType(string type)
        {
            _breakdownRangeType = type;
            if (type == "week")
            {
                _filterStartDate = GetWeekStart(_filterStartDate);
                _filterEndDate = _filterStartDate.AddDays(6);
            }
            else if (type == "month")
            {
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (type == "year")
            {
                var range = GetSnappedYearRange(_selectedYear);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (type == "custom")
            {
                _filterStartDate = _breakdownCustomStart;
                _filterEndDate = _breakdownCustomEnd;
            }
            await LoadData();
        }

        private async Task OnBreakdownCustomDatesChanged()
        {
            if (_breakdownCustomEnd < _breakdownCustomStart)
            {
                _breakdownCustomEnd = _breakdownCustomStart;
            }
            _filterStartDate = _breakdownCustomStart;
            _filterEndDate = _breakdownCustomEnd;
            await LoadData();
        }

        public class LotteryBreakdownStats
        {
            public decimal TotalEnvelopeDrops { get; set; }
            public decimal TotalVendingDrops { get; set; }
            public decimal TotalCashAvailable => TotalEnvelopeDrops + TotalVendingDrops;
            public string PeakDropDayLabel { get; set; } = "N/A";
            public decimal PeakDropDayAmount { get; set; }
            public int TotalShifts { get; set; }
            public decimal TotalWeeklyStatementDue { get; set; }
            public decimal TotalOnlineDue { get; set; }
        }

        public class DailyBreakdownItem
        {
            public DateTime Date { get; set; }
            public string DayName { get; set; } = string.Empty;
            public string DateLabel { get; set; } = string.Empty;
            public decimal EnvelopeAmount { get; set; }
            public decimal VendingAmount { get; set; }
            public decimal TotalCashAvailable => EnvelopeAmount + VendingAmount;
            public decimal NetDue { get; set; }
            public decimal TotalSales { get; set; }
            public decimal Variance { get; set; }
            public decimal WeeklyStatementDue { get; set; }
            public decimal OnlineDue { get; set; }
            public int ShiftCount { get; set; }
            public double PercentageOfTotal { get; set; }
            public List<LotteryShiftDto> Shifts { get; set; } = new();
        }

        private async Task OnMonthYearChanged()
        {
            if ((_viewMode == "breakdown" || _viewMode == "commissions" || _viewMode == "vending" || _viewMode == "analytics") && _breakdownRangeType == "month")
            {
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if ((_viewMode == "breakdown" || _viewMode == "commissions" || _viewMode == "vending" || _viewMode == "analytics") && _breakdownRangeType == "year")
            {
                var range = GetSnappedYearRange(_selectedYear);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            
            await LoadData();
        }

        private HashSet<DateTime> _expandedDays = new();

        private void ToggleDayExpansion(DateTime date)
        {
            if (_expandedDays.Contains(date.Date))
                _expandedDays.Remove(date.Date);
            else
                _expandedDays.Add(date.Date);
            
            StateHasChanged();
        }

        // Commission Rates State
        private bool _showRatesModal = false;
        private List<LotteryCommissionRate> _commissionRates = new();

        // Summary stats computed from _shifts list using Business Day logic
        private decimal TotalSales => _shifts.Where(s => s.Status != "Draft").Sum(s => s.TotalSales);
        private decimal TotalPayouts => _shifts.Where(s => s.Status != "Draft").Sum(s => s.TotalPayouts);
        private decimal TotalNetSales => _shifts.Where(s => s.Status != "Draft").Sum(s => s.NetSales);
        private decimal TotalEnvelope => _shifts.Where(s => s.Status != "Draft").Sum(s => s.EnvelopeAmount);
        private decimal TotalVariance => _shifts.Where(s => s.Status != "Draft").Sum(s => s.Variance);
        private decimal TotalEarnings => _shifts.Where(s => s.Status != "Draft").Sum(s => s.Commission);
        private decimal TotalFees => _shifts.Where(s => s.Status != "Draft").Sum(s => s.IdentifiedFees);
        private decimal TotalBagOut => _shifts.Where(s => s.Status != "Draft").Sum(s => s.BackupBagAmount);
        private decimal TotalBagIn => _shifts.Where(s => s.Status != "Draft").Sum(s => s.BagRefillAmount);
        private decimal NetBagDebt => TotalBagOut - TotalBagIn;
        private decimal TotalInstantTickets => _shifts.Where(s => s.Status != "Draft").Sum(s => s.TotalCancels);

        private ShiftFormModel _shiftForm = new();
        private ShiftFormModel _originalForm = new(); // CHANGE TRACKER

        private bool ShowContent => !_loading && string.IsNullOrEmpty(_error);

        private bool IsTableError => !string.IsNullOrEmpty(_error) && 
            (_error.Contains("table does not exist", System.StringComparison.OrdinalIgnoreCase) || 
             _error.Contains("Invalid object name", System.StringComparison.OrdinalIgnoreCase));

        protected override async Task OnInitializedAsync()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            if (Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("tab", out var tabValue))
            {
                var tab = tabValue.ToString().ToLower();
                if (tab == "imported")
                {
                    _viewMode = "imported";
                }
            }

            InitializeWeeks();
            await LoadAvailableYears();

            if (_viewMode == "daily")
            {
                _filterStartDate = GetWeekStart(DateTime.Today);
                _filterEndDate = _filterStartDate.AddDays(6);
            }
            else if (_breakdownRangeType == "month")
            {
                var monthRange = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = monthRange.Start;
                _filterEndDate = monthRange.End;
            }

            await LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (_needsChartUpdate)
            {
                _needsChartUpdate = false;
                try
                {
                    if (_viewMode == "breakdown")
                    {
                        await UpdateBreakdownChart();
                    }
                    else if (_viewMode == "vending")
                    {
                        await UpdateVendingChart();
                    }
                    else if (_viewMode == "commissions")
                    {
                        await UpdateCommissionsChart();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error updating chart on after render");
                }
            }
        }

        private bool ShouldShowContent() => !_loading && string.IsNullOrEmpty(_error);

        private async Task LoadData()
        {
            _loading = true;
            _error = string.Empty;
            try
            {
                _employeeMetadata = await Task.Run(() => LotteryService.GetEmployeeMetadata());
                _employeeNames = _employeeMetadata.Select(m => m.FullName).ToList();
                _commissionRates = await Task.Run(() => LotteryService.GetAllRates());
                await LoadAvailableYears();
                
                if (_viewMode == "shifts" || _viewMode == "daily")
                {
                    await LoadShifts();
                }
                if (_viewMode == "daily")
                {
                    await LoadDailySummaries();
                }
                else if (_viewMode == "commissions")
                {
                    await LoadCommissionsData();
                }
                else if (_viewMode == "analytics")
                {
                    await LoadAnalyticsData();
                }
                else if (_viewMode == "vending")
                {
                    await LoadVendingData();
                }
                else if (_viewMode == "breakdown")
                {
                    await LoadBreakdownData();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading lottery data");
                _error = "Failed to load lottery data: " + ex.Message;
            }
            finally
            {
                _loading = false;
                _needsChartUpdate = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task LoadShifts()
        {
            var startDate = _filterStartDate.Date;
            var endDate = _filterEndDate.Date;
            var shifts = await Task.Run(() => LotteryService.GetShiftsByDateRange(startDate, endDate));
            
            if (!string.IsNullOrEmpty(_filterEmployee))
            {
                shifts = shifts.Where(s => s.EmployeeName != null && 
                    s.EmployeeName.Trim().Equals(_filterEmployee.Trim(), StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            
            if (_showReconciled.HasValue)
            {
                shifts = shifts.Where(s => s.IsReconciled == _showReconciled.Value).ToList();
            }
            
            _shifts = shifts
                .OrderByDescending(s => s.ShiftDate.Date)
                .ThenBy(s => s.ShiftType == "Day" ? 0 : s.ShiftType == "Night" ? 1 : 2)
                .ThenBy(s => s.Status == "Draft" ? 0 : 1)
                .ToList();
        }

        private List<(DateTime Start, DateTime End, string Label)> _availableWeeks = new();

        private void InitializeWeeks()
        {
            _availableWeeks.Clear();
            
            // Find the earliest shift date in the database to dynamically determine how far back to go
            DateTime earliestDate = DateTime.Today.AddMonths(-18); // Default to 18 months back
            try
            {
                using var db = DbFactory.CreateDbContext();
                var firstShift = db.LotteryShifts.AsNoTracking()
                    .OrderBy(s => s.ShiftDate)
                    .Select(s => (DateTime?)s.ShiftDate)
                    .FirstOrDefault();
                if (firstShift.HasValue)
                {
                    earliestDate = firstShift.Value;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error fetching earliest shift date for available weeks");
            }

            var currentSat = GetWeekStart(DateTime.Today);
            var earliestWeekStart = GetWeekStart(earliestDate);
            
            var temp = currentSat;
            int count = 0;
            // Limit to at most 156 weeks (3 years) to avoid select element performance issues
            while (temp >= earliestWeekStart && count < 156)
            {
                var start = temp;
                var end = start.AddDays(6);
                _availableWeeks.Add((start, end, $"{start:MMM d} - {end:MMM d, yyyy}"));
                temp = temp.AddDays(-7);
                count++;
            }
            
            // Fallback to at least 52 weeks (1 year) if database is empty or date range is small
            if (_availableWeeks.Count < 52)
            {
                _availableWeeks.Clear();
                for (int i = 0; i < 52; i++)
                {
                    var start = currentSat.AddDays(-7 * i);
                    var end = start.AddDays(6);
                    _availableWeeks.Add((start, end, $"{start:MMM d} - {end:MMM d, yyyy}"));
                }
            }
        }

        private List<int> _availableYears = new() { DateTime.Now.Year };

        private async Task LoadAvailableYears()
        {
            try
            {
                using var db = await DbFactory.CreateDbContextAsync();
                
                var shiftYears = await db.LotteryShifts.AsNoTracking()
                    .Select(s => s.ShiftDate.Year)
                    .Distinct()
                    .ToListAsync();

                var weeklyYears = await db.LotteryWeeklyStats.AsNoTracking()
                    .Select(w => w.WeekEndingDate.Year)
                    .Distinct()
                    .ToListAsync();

                var combinedYears = shiftYears.Concat(weeklyYears)
                    .Append(DateTime.Now.Year)
                    .Where(y => y > 2000)
                    .Distinct()
                    .OrderByDescending(y => y)
                    .ToList();

                if (combinedYears.Any())
                {
                    _availableYears = combinedYears;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading available years dynamically");
            }
        }

        private async Task OnWeekSelected(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var start))
            {
                _filterStartDate = start;
                _filterEndDate = start.AddDays(6);
                await OnFilterChanged();
            }
        }

        private static DateTime GetWeekStart(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private static (DateTime Start, DateTime End) GetSnappedMonthRange(int year, int month)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Find all Saturdays in the month
            var saturdays = new List<DateTime>();
            for (var date = firstDayOfMonth; date <= lastDayOfMonth; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    saturdays.Add(date);
                }
            }

            if (!saturdays.Any())
            {
                return (firstDayOfMonth, lastDayOfMonth);
            }

            var firstSaturday = saturdays.Min();
            var lastSaturday = saturdays.Max();

            var start = GetWeekStart(firstSaturday); // Sunday of the week ending on first Saturday
            var end = lastSaturday; // The last Saturday itself
            return (start, end);
        }

        private static (DateTime Start, DateTime End) GetSnappedYearRange(int year)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            // Find all Saturdays in the year
            var saturdays = new List<DateTime>();
            for (var date = firstDayOfYear; date <= lastDayOfYear; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    saturdays.Add(date);
                }
            }

            if (!saturdays.Any())
            {
                return (firstDayOfYear, lastDayOfYear);
            }

            var firstSaturday = saturdays.Min();
            var lastSaturday = saturdays.Max();

            var start = GetWeekStart(firstSaturday); // Sunday of the week ending on first Saturday
            var end = lastSaturday < lastDayOfYear ? lastDayOfYear : lastSaturday;
            return (start, end);
        }

        private async Task LoadDailySummaries()
        {
            try
            {
                var summaries = new List<LotteryShiftSummaryDto>();
                var startDate = _filterStartDate.Date;
                var endDate = _filterEndDate.Date;
                var currentDate = startDate;
                var seenDates = new HashSet<DateTime>();
                
                while (currentDate <= endDate)
                {
                    if (!seenDates.Contains(currentDate))
                    {
                        var summary = await Task.Run(() => LotteryService.GetDailySummary(currentDate));
                        if (summary != null && summary.ShiftCount > 0)
                        {
                            summaries.Add(summary);
                            seenDates.Add(currentDate);
                        }
                    }
                    currentDate = currentDate.AddDays(1);
                }
                _dailySummaries = summaries;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading daily summaries");
                _dailySummaries = new List<LotteryShiftSummaryDto>();
                throw;
            }
        }





        private async Task OnFilterChanged()
        {
            await LoadData();
        }

        private async Task ApplyTodayFilter()
        {
            _filterStartDate = DateTime.Today;
            _filterEndDate = DateTime.Today;
            await OnFilterChanged();
        }

        private async Task ApplyCurrentWeekFilter()
        {
            _filterStartDate = GetWeekStart(DateTime.Today);
            _filterEndDate = _filterStartDate.AddDays(6);
            await LoadData();
        }

        private async Task ApplyPastWeekFilter()
        {
            _filterStartDate = GetWeekStart(DateTime.Today).AddDays(-7);
            _filterEndDate = _filterStartDate.AddDays(6);
            await LoadData();
        }

        private async Task ChangeReconciledFilter(bool? value)
        {
            _showReconciled = value;
            await LoadData();
        }

        private async Task ChangeViewMode(string mode)
        {
            _viewMode = mode;
            _error = string.Empty;

            // INTELLIGENT DATE SNAPPING
            if (_viewMode == "breakdown")
            {
                _breakdownRangeType = "month";
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (_viewMode == "commissions" || _viewMode == "vending")
            {
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (_viewMode == "daily")
            {
                _filterStartDate = GetWeekStart(DateTime.Today);
                _filterEndDate = _filterStartDate.AddDays(6);
            }
            else if (_viewMode == "analytics")
            {
                // Snap to the full month for analytics view as requested
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (_viewMode == "summaries" || _viewMode == "compliance" || _viewMode == "imported")
            {
                // Child component handles its own filter controls and data loading
            }

            await LoadData();
        }

        private async Task ChangeYear(int year)
        {
            _selectedYear = year;
            if ((_viewMode == "breakdown" || _viewMode == "commissions" || _viewMode == "vending" || _viewMode == "analytics") && _breakdownRangeType == "year")
            {
                var range = GetSnappedYearRange(_selectedYear);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if ((_viewMode == "breakdown" || _viewMode == "commissions" || _viewMode == "vending" || _viewMode == "analytics") && _breakdownRangeType == "month")
            {
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            await LoadData();
        }

        private void StartAddShift()
        {
            var currentUser = AuthStateProvider.GetCurrentUser();
            var username = currentUser?.Username ?? string.Empty;
            var fullName = _employeeMetadata.FirstOrDefault(m => m.Username == username).FullName ?? username;

            _editingShiftId = null;
            _shiftForm = new ShiftFormModel
            {
                ShiftDate = DateTime.Now,
                Status = "Submitted",
                CreatedBy = username,
                EmployeeName = fullName
            };
            _modalError = string.Empty;
            _showShiftModal = true;
        }

        private async Task EditShift(int shiftId)
        {
            var shiftEntity = await Task.Run(() => LotteryService.GetShift(shiftId));
            if (shiftEntity == null) return;

            _editingShiftId = shiftId;
            
            bool isFullDay = (shiftEntity.Notes != null && shiftEntity.Notes.Contains("[Full Day Shift]")) ||
                             (shiftEntity.EmployeeName != null && shiftEntity.EmployeeName.Contains("(Full Day)"));

            // [FIX] Find the PREVIOUS shift on the same day to get the baseline for cumulative subtraction
            decimal baselineSales = 0, baselinePrizes = 0, baselineTickets = 0;
            if (string.Equals(shiftEntity.ShiftType, "Night", StringComparison.OrdinalIgnoreCase) && !isFullDay)
            {
                var dayShift = _shifts.FirstOrDefault(s => s.ShiftDate.Date == shiftEntity.ShiftDate.Date && string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase) && (s.Notes == null || !s.Notes.Contains("Included in Full Day Closeout")));
                if (dayShift != null)
                {
                    baselineSales = dayShift.TotalSales;
                    baselinePrizes = dayShift.TotalPayouts;
                    baselineTickets = dayShift.TotalCancels;
                }
            }

            _shiftForm = new ShiftFormModel
            {
                ShiftDate = shiftEntity.ShiftDate,
                EmployeeName = shiftEntity.EmployeeName,
                ShiftType = shiftEntity.ShiftType ?? string.Empty,
                MachineId = shiftEntity.MachineId ?? string.Empty,
                StartingCash = shiftEntity.StartingCash,
                EndingCash = shiftEntity.EndingCash,
                TotalSales = shiftEntity.TotalSales,
                TotalPayouts = shiftEntity.TotalPayouts,
                TotalCancels = shiftEntity.TotalCancels,
                NetDue = shiftEntity.NetDue,
                BagRefillAmount = shiftEntity.BagRefillAmount,
                BackupBagAmount = shiftEntity.BackupBagAmount,
                Notes = shiftEntity.Notes ?? string.Empty,
                Status = shiftEntity.Status ?? "Submitted",
                CreatedBy = shiftEntity.CreatedBy ?? string.Empty,
                CreatedDate = shiftEntity.CreatedDate,
                IsFullDayShift = isFullDay,
                
                // [FIX] Set the baselines for accurate cumulative-to-activity math
                BaselineSales = baselineSales,
                BaselinePrizes = baselinePrizes,
                BaselineTickets = baselineTickets,
                
                // Keep persisted values as additional backup/reference
                PersistedNetSales = shiftEntity.NetSales,
                PersistedExpectedCash = shiftEntity.ExpectedCash,
                PersistedVariance = shiftEntity.Variance
            };
            _originalEmployeeName = shiftEntity.EmployeeName;
            _originalForm = (ShiftFormModel)_shiftForm.Clone(); // SNAPSHOT ORIGINAL STATE
            _modalError = string.Empty;
            _showShiftModal = true;
        }

        private void OnFullDayToggleChanged(ChangeEventArgs e)
        {
            if (e.Value is bool isChecked)
            {
                if (isChecked)
                {
                    // Check if a Day Shift with actual sales exists for this date
                    var dayShift = _shifts.FirstOrDefault(s => s.ShiftDate.Date == _shiftForm.ShiftDate.Date && string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase) && (s.Notes == null || !s.Notes.Contains("Included in Full Day Closeout")));
                    if (dayShift != null && dayShift.TotalSales > 0)
                    {
                        _existingDayShiftSales = dayShift.TotalSales;
                        _existingDayShiftEmployee = dayShift.EmployeeName;
                        _showFullDayMergePrompt = true;
                        StateHasChanged();
                        return;
                    }
                    ApplyFullDayShift(true);
                }
                else
                {
                    ApplyFullDayShift(false);
                }
            }
        }

        private void ConfirmFullDayMerge()
        {
            _showFullDayMergePrompt = false;
            ApplyFullDayShift(true);
        }

        private void CancelFullDayMerge()
        {
            _showFullDayMergePrompt = false;
            _shiftForm.IsFullDayShift = false;
            StateHasChanged();
        }

        private void ApplyFullDayShift(bool isFullDay)
        {
            _shiftForm.IsFullDayShift = isFullDay;
            if (isFullDay)
            {
                _shiftForm.BaselineSales = 0;
                _shiftForm.BaselinePrizes = 0;
                _shiftForm.BaselineTickets = 0;
            }
            else
            {
                var dayShift = _shifts.FirstOrDefault(s => s.ShiftDate.Date == _shiftForm.ShiftDate.Date && string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase) && (s.Notes == null || !s.Notes.Contains("Included in Full Day Closeout")));
                if (dayShift != null)
                {
                    _shiftForm.BaselineSales = dayShift.TotalSales;
                    _shiftForm.BaselinePrizes = dayShift.TotalPayouts;
                    _shiftForm.BaselineTickets = dayShift.TotalCancels;
                }
            }
            StateHasChanged();
        }

        private void OnEmployeeChanged(ChangeEventArgs e)
        {
            var username = e.Value?.ToString() ?? string.Empty;
            if (username == _shiftForm.CreatedBy) return;

            if (string.IsNullOrEmpty(username))
            {
                _pendingUsername = string.Empty;
                _pendingFullName = "Unassigned";
                _showReassignConfirmation = true;
            }
            else
            {
                var metadata = _employeeMetadata.FirstOrDefault(m => m.Username == username);
                _pendingUsername = username;
                _pendingFullName = metadata.FullName ?? username;
                _showReassignConfirmation = true;
            }
        }

        private void ConfirmReassignment()
        {
            _shiftForm.CreatedBy = _pendingUsername;
            _shiftForm.EmployeeName = _pendingFullName;
            _showReassignConfirmation = false;
        }

        private void CancelReassignment()
        {
            _showReassignConfirmation = false;
            _pendingUsername = string.Empty;
            _pendingFullName = string.Empty;
        }

        private async Task SaveShift()
        {
            if (_editingShiftId.HasValue && !_shiftForm.IsDirty(_originalForm))
            {
                _showNoChangesModal = true;
                return;
            }

            _savingShift = true;
            _modalError = string.Empty;
            try
            {
                var currentUser = AuthStateProvider.GetCurrentUser();
                var username = currentUser?.Username ?? "Unknown";

                LotteryShift shift;
                if (_editingShiftId.HasValue)
                {
                    shift = await Task.Run(() => LotteryService.GetShift(_editingShiftId.Value));
                    if (shift == null)
                    {
                        _modalError = "Unable to find the original shift record.";
                        return;
                    }
                }
                else
                {
                    shift = new LotteryShift { CreatedDate = DateTime.Now };
                }

                // MERGE FORM DATA INTO ENTITY
                shift.ShiftDate = _shiftForm.ShiftDate;
                shift.EmployeeName = _shiftForm.EmployeeName;
                shift.StartingCash = _shiftForm.StartingCash ?? 0;
                shift.EndingCash = _shiftForm.EndingCash ?? 0;
                shift.TotalSales = _shiftForm.TotalSales ?? 0;
                shift.TotalPayouts = _shiftForm.TotalPayouts ?? 0;
                shift.TotalCancels = _shiftForm.TotalCancels ?? 0;
                shift.NetDue = _shiftForm.NetDue ?? 0;
                
                // [FIX]: Save the SHIFT-SPECIFIC activity results, not the cumulative machine totals.
                shift.NetSales = _shiftForm.NetSales;
                shift.ExpectedCash = _shiftForm.ExpectedCash;
                shift.Variance = _shiftForm.Variance;
                
                shift.ShiftSalesActivity = _shiftForm.ShiftSalesActivity;
                shift.ShiftPayoutsActivity = _shiftForm.ShiftPayoutsActivity;
                shift.ShiftCancelsActivity = _shiftForm.ShiftCancelsActivity;
                
                shift.EnvelopeAmount = _shiftForm.EnvelopeAmount;
                shift.BagRefillAmount = _shiftForm.BagRefillAmount ?? 0;
                shift.BackupBagAmount = _shiftForm.BackupBagAmount;
                shift.Notes = string.IsNullOrWhiteSpace(_shiftForm.Notes) ? null : _shiftForm.Notes;
                shift.Status = _shiftForm.Status;
                shift.CreatedBy = _shiftForm.CreatedBy;

                if (_shiftForm.IsFullDayShift)
                {
                    if (string.IsNullOrWhiteSpace(shift.Notes)) shift.Notes = "[Full Day Shift]";
                    else if (!shift.Notes.Contains("[Full Day Shift]")) shift.Notes = $"[Full Day Shift] {shift.Notes}";
                }
                else if (_originalForm.IsFullDayShift && !_shiftForm.IsFullDayShift)
                {
                    if (!string.IsNullOrEmpty(shift.Notes))
                    {
                        shift.Notes = shift.Notes.Replace("[Full Day Shift]", "").Trim();
                        if (string.IsNullOrWhiteSpace(shift.Notes)) shift.Notes = null;
                    }
                }

                if (_editingShiftId.HasValue)
                {
                    await Task.Run(() => LotteryService.UpdateShift(shift, username));
                }
                else
                {
                    await Task.Run(() => LotteryService.CreateShift(shift, username));
                }

                // [STATUS SYNC] Synchronize status to corresponding BarSaleEntry
                if (!_shiftForm.IsFullDayShift)
                {
                    using var db = await DbFactory.CreateDbContextAsync();
                    var barEntry = await db.BarSaleEntries.FirstOrDefaultAsync(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == shift.ShiftDate.Date && e.Shift == shift.ShiftType && !e.IsRentalHall);
                    if (barEntry != null && barEntry.Status != _shiftForm.Status)
                    {
                        barEntry.Status = _shiftForm.Status;
                        await db.SaveChangesAsync();
                    }
                }

                // [FULL DAY AUTO-FULFILLMENT & TWO-WAY REVERSIBILITY]
                if (_shiftForm.IsFullDayShift && string.Equals(shift.ShiftType, "Night", StringComparison.OrdinalIgnoreCase))
                {
                    using var db = await DbFactory.CreateDbContextAsync();
                    
                    // 1. Day Bar Entry
                    var dayBar = await db.BarSaleEntries.FirstOrDefaultAsync(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == shift.ShiftDate.Date && e.Shift == "Day" && !e.IsRentalHall);
                    if (dayBar == null)
                    {
                        dayBar = new BarSaleEntry
                        {
                            SaleDate = shift.ShiftDate.Date,
                            AdjustedSaleDate = shift.ShiftDate.Date,
                            Shift = "Day",
                            IsRentalHall = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = username,
                            EmployeeUsername = shift.CreatedBy,
                            TotalSales = 0,
                            TotalHours = 0,
                            Notes = "[Included in Full Day Closeout]",
                            Status = "Submitted"
                        };
                        db.BarSaleEntries.Add(dayBar);
                    }
                    else if (dayBar.TotalSales == 0)
                    {
                        dayBar.Notes = "[Included in Full Day Closeout]";
                        dayBar.Status = "Submitted";
                        dayBar.EmployeeUsername = shift.CreatedBy;
                    }
                    await db.SaveChangesAsync();

                    // 2. Day Lottery Shift
                    var allShiftsOnDate = await Task.Run(() => LotteryService.GetShiftsByDateRange(shift.ShiftDate.Date, shift.ShiftDate.Date));
                    var dayLottoDto = allShiftsOnDate.FirstOrDefault(s => string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase));
                    if (dayLottoDto == null)
                    {
                        var newDayLotto = new LotteryShift
                        {
                            ShiftDate = shift.ShiftDate.Date,
                            ShiftType = "Day",
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = username,
                            EmployeeName = $"{shift.EmployeeName} (Full Day)",
                            StartingCash = 300,
                            EndingCash = 300,
                            TotalSales = 0,
                            TotalPayouts = 0,
                            TotalCancels = 0,
                            NetDue = 0,
                            EnvelopeAmount = 0,
                            BagRefillAmount = 0,
                            ShiftSalesActivity = 0,
                            ShiftPayoutsActivity = 0,
                            ShiftCancelsActivity = 0,
                            Variance = 0,
                            Status = "Submitted",
                            IsReconciled = true,
                            Notes = "Included in Full Day Closeout"
                        };
                        await Task.Run(() => LotteryService.CreateShift(newDayLotto, username));
                    }
                    else if (dayLottoDto.Notes == null || !dayLottoDto.Notes.Contains("Included in Full Day Closeout"))
                    {
                        // Save snapshot of existing Day Shift before merging so we can restore if ever unchecked
                        var existingDay = await Task.Run(() => LotteryService.GetShift(dayLottoDto.ShiftId));
                        if (existingDay != null)
                        {
                            var snapshot = System.Text.Json.JsonSerializer.Serialize(new {
                                existingDay.EmployeeName,
                                existingDay.StartingCash,
                                existingDay.EndingCash,
                                existingDay.TotalSales,
                                existingDay.TotalPayouts,
                                existingDay.TotalCancels,
                                existingDay.NetDue,
                                existingDay.ShiftSalesActivity,
                                existingDay.ShiftPayoutsActivity,
                                existingDay.ShiftCancelsActivity,
                                existingDay.Notes
                            });
                            existingDay.EmployeeName = $"{shift.EmployeeName} (Full Day)";
                            existingDay.Notes = $"[ORIGINAL_DAY_SNAPSHOT:{snapshot}] Included in Full Day Closeout";
                            existingDay.Status = "Submitted";
                            existingDay.IsReconciled = true;
                            await Task.Run(() => LotteryService.UpdateShift(existingDay, username));
                        }
                    }
                }
                else if (_originalForm.IsFullDayShift && !_shiftForm.IsFullDayShift && string.Equals(shift.ShiftType, "Night", StringComparison.OrdinalIgnoreCase))
                {
                    // [UNMERGE RESTORATION] Restore Day shift if snapshot exists, or remove placeholder
                    var allShiftsOnDate = await Task.Run(() => LotteryService.GetShiftsByDateRange(shift.ShiftDate.Date, shift.ShiftDate.Date));
                    var dayLottoDto = allShiftsOnDate.FirstOrDefault(s => string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase));
                    if (dayLottoDto != null)
                    {
                        var existingDay = await Task.Run(() => LotteryService.GetShift(dayLottoDto.ShiftId));
                        if (existingDay != null)
                        {
                            if (existingDay.Notes != null && existingDay.Notes.Contains("[ORIGINAL_DAY_SNAPSHOT:"))
                            {
                                int startIdx = existingDay.Notes.IndexOf("[ORIGINAL_DAY_SNAPSHOT:") + "[ORIGINAL_DAY_SNAPSHOT:".Length;
                                int endIdx = existingDay.Notes.IndexOf("]", startIdx);
                                if (endIdx > startIdx)
                                {
                                    var json = existingDay.Notes.Substring(startIdx, endIdx - startIdx);
                                    using var doc = System.Text.Json.JsonDocument.Parse(json);
                                    var root = doc.RootElement;
                                    existingDay.EmployeeName = root.GetProperty("EmployeeName").GetString() ?? existingDay.EmployeeName;
                                    existingDay.StartingCash = root.GetProperty("StartingCash").GetDecimal();
                                    existingDay.EndingCash = root.GetProperty("EndingCash").GetDecimal();
                                    existingDay.TotalSales = root.GetProperty("TotalSales").GetDecimal();
                                    existingDay.TotalPayouts = root.GetProperty("TotalPayouts").GetDecimal();
                                    existingDay.TotalCancels = root.GetProperty("TotalCancels").GetDecimal();
                                    existingDay.NetDue = root.GetProperty("NetDue").GetDecimal();
                                    existingDay.ShiftSalesActivity = root.GetProperty("ShiftSalesActivity").GetDecimal();
                                    existingDay.ShiftPayoutsActivity = root.GetProperty("ShiftPayoutsActivity").GetDecimal();
                                    existingDay.ShiftCancelsActivity = root.GetProperty("ShiftCancelsActivity").GetDecimal();
                                    existingDay.Notes = root.TryGetProperty("Notes", out var n) ? n.GetString() : null;
                                    await Task.Run(() => LotteryService.UpdateShift(existingDay, username));
                                }
                            }
                            else if (existingDay.Notes != null && existingDay.Notes.Contains("Included in Full Day Closeout") && existingDay.TotalSales == 0)
                            {
                                // Placeholder created automatically - delete it on unmerge
                                await Task.Run(() => LotteryService.DeleteShift(existingDay.ShiftId));
                            }
                        }
                    }
                }

                _showShiftModal = false; // CLOSE MODAL IMMEDIATELY
                _ = InvokeAsync(async () => {
                    await LoadData();
                    StateHasChanged();
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving shift");
                _modalError = "Error saving: " + ex.Message;
            }
            finally
            {
                _savingShift = false;
            }
        }

        private void CancelShiftModal()
        {
            _showShiftModal = false;
            _editingShiftId = null;
            _shiftForm = new ShiftFormModel();
            _modalError = string.Empty;
            _showNoChangesModal = false;
            _showReassignConfirmation = false;
            _showFullDayMergePrompt = false;
        }

        private void ConfirmDelete(int shiftId)
        {
            _shiftToDelete = shiftId;
            _showDeleteModal = true;
        }

        private void CancelDelete()
        {
            _showDeleteModal = false;
            _shiftToDelete = null;
        }

        private async Task DeleteShiftConfirmed()
        {
            if (!_shiftToDelete.HasValue) return;

            _deletingShift = true;
            try
            {
                await Task.Run(() => LotteryService.DeleteShift(_shiftToDelete.Value));
                _showDeleteModal = false;
                _showShiftModal = false; // CLOSE EDIT MODAL ON SUCCESSFUL DELETE
                _editingShiftId = null;  // CLEAR EDITING ID
                _shiftToDelete = null;
                await LoadData();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting shift");
                _error = "Failed to delete shift: " + ex.Message;
            }
            finally
            {
                _deletingShift = false;
            }
        }

        private async Task MarkReconciled(int shiftId)
        {
            try
            {
                var currentUser = AuthStateProvider.GetCurrentUser();
                var username = currentUser?.Username ?? "Unknown";
                await Task.Run(() => LotteryService.MarkReconciled(shiftId, username));
                await LoadData();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error marking shift as reconciled");
                _error = "Failed to mark shift as reconciled: " + ex.Message;
            }
        }

        private async Task MarkUnreconciled(int shiftId)
        {
            try
            {
                await Task.Run(() => LotteryService.MarkUnreconciled(shiftId));
                await LoadData();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error marking shift as unreconciled");
                _error = "Failed to mark shift as unreconciled: " + ex.Message;
            }
        }

        // --- COMMISSION RATE MANAGEMENT ---
        private void OpenRatesModal()
        {
            _showRatesModal = true;
        }

        private async Task SaveCommissionRate(LotteryCommissionRate rate)
        {
            try
            {
                var currentUser = AuthStateProvider.GetCurrentUser();
                rate.CreatedBy = currentUser?.Username ?? "Admin";
                await Task.Run(() => LotteryService.SaveRate(rate));
                
                // Refresh data to reflect new calculations globally
                await LoadData();
            }
            catch (Exception ex)
            {
                _modalError = "Failed to save rate: " + ex.Message;
            }
        }

        private void AddNewYearRate()
        {
            int nextYear = _commissionRates.Any() ? _commissionRates.Max(r => r.Year) + 1 : DateTime.Now.Year;
            var prevRate = _commissionRates.FirstOrDefault(); // Higher year due to DESC sort
            
            _commissionRates.Insert(0, new LotteryCommissionRate 
            { 
                Year = nextYear,
                // Carry over previous values per user request
                SalesRate = prevRate?.SalesRate ?? 5.00m,
                CashingRate = prevRate?.CashingRate ?? 1.00m,
                TicketRate = prevRate?.TicketRate ?? 1.00m,
                DailySystemFee = prevRate?.DailySystemFee ?? 0.00m,
                DailyBondingFee = prevRate?.DailyBondingFee ?? 1.00m,
                TargetDrawerAmount = prevRate?.TargetDrawerAmount ?? 1200.00m
            });
        }

        public class ShiftFormModel : ICloneable
        {
            [Required(ErrorMessage = "Audit date is required")]
            public DateTime ShiftDate { get; set; } = DateTime.Now;
            
            [Required(ErrorMessage = "Employee name is required")]
            [StringLength(100, ErrorMessage = "Employee cannot exceed 100 characters")]
            public string EmployeeName { get; set; } = string.Empty;
            
            public string ShiftType { get; set; } = string.Empty;
            public string MachineId { get; set; } = string.Empty;
            
            [Required(ErrorMessage = "Starting cash is required")]
            [Range(0, double.MaxValue, ErrorMessage = "Starting cash must be 0 or greater")]
            public decimal? StartingCash { get; set; }
            
            [Range(0, double.MaxValue, ErrorMessage = "Ending cash must be 0 or greater")]
            public decimal? EndingCash { get; set; }
            
            [Range(0, double.MaxValue, ErrorMessage = "Total sales must be 0 or greater")]
            public decimal? TotalSales { get; set; }
            
            [Range(0, double.MaxValue, ErrorMessage = "Total payouts must be 0 or greater")]
            public decimal? TotalPayouts { get; set; }
            
            [Range(0, double.MaxValue, ErrorMessage = "Tickets must be 0 or greater")]
            public decimal? TotalCancels { get; set; }

            public decimal? NetDue { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "Backup Bag must be 0 or greater")]
            public decimal? BagRefillAmount { get; set; }

            public decimal BackupBagAmount { get; set; }
            
            public string Notes { get; set; } = string.Empty;
            public string Status { get; set; } = "Submitted";
            public string CreatedBy { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; } // HANG PREVENTION
            public bool IsFullDayShift { get; set; } = false;
            
            // Persistent trackers to hold the math from the mobile submission or service
            public decimal PersistedNetSales { get; set; }
            public decimal PersistedExpectedCash { get; set; }
            public decimal PersistedVariance { get; set; }
            
            // Baselines from the previous shift of the same day (used to extract activity from cumulative totals)
            public decimal BaselineSales { get; set; }
            public decimal BaselinePrizes { get; set; }
            public decimal BaselineTickets { get; set; }

            public decimal NetSales {
                get {
                    // [SMART-MATH]: Shift Activity = (Current Cumulative Reading) - (Baseline from previous shift)
                    decimal activeSales = (TotalSales ?? 0) - (IsFullDayShift ? 0 : BaselineSales);
                    decimal activePrizes = (TotalPayouts ?? 0) - (IsFullDayShift ? 0 : BaselinePrizes);
                    decimal activeTickets = (TotalCancels ?? 0) - (IsFullDayShift ? 0 : BaselineTickets);
                    
                    return activeSales - activePrizes - activeTickets;
                }
            }

            // [FIX]: ExpectedCash for display should be the target baseline ($1,200) after distributions.
            public decimal ExpectedCash => (StartingCash ?? 0) + NetSales + BackupBagAmount - (BagRefillAmount ?? 0) - EnvelopeAmount;
            
            // [CRITICAL FIX]: Variance calculation must ignore drops/refills to avoid "Ghost Discrepancies".
            // Variance = EndingCash - (Expected Cash BEFORE Drops)
            public decimal Variance => (EndingCash ?? 0) - ((StartingCash ?? 0) + NetSales + BackupBagAmount);
            
            // PERSIST THE ACTIVITY FIELDS FOR REPOSITORY
            public decimal ShiftSalesActivity => (TotalSales ?? 0) - (IsFullDayShift ? 0 : BaselineSales);
            public decimal ShiftPayoutsActivity => (TotalPayouts ?? 0) - (IsFullDayShift ? 0 : BaselinePrizes);
            public decimal ShiftCancelsActivity => (TotalCancels ?? 0) - (IsFullDayShift ? 0 : BaselineTickets);

            // AUTOMATIC ENVELOPE CALCULATION ($1,200 Bag Target)
            public decimal EnvelopeAmount => (ShiftType != "Day" && (EndingCash ?? 0) > 1200) ? (EndingCash ?? 0) - 1200 : 0;

            public object Clone() => this.MemberwiseClone();

            public bool IsDirty(ShiftFormModel other)
            {
                if (other == null) return true;
                return ShiftDate != other.ShiftDate ||
                       EmployeeName != other.EmployeeName ||
                       Status != other.Status ||
                       Notes != other.Notes ||
                       IsFullDayShift != other.IsFullDayShift ||
                       StartingCash != other.StartingCash ||
                       EndingCash != other.EndingCash ||
                       TotalSales != other.TotalSales ||
                       TotalPayouts != other.TotalPayouts ||
                       TotalCancels != other.TotalCancels ||
                       NetDue != other.NetDue ||
                       BagRefillAmount != other.BagRefillAmount ||
                       BackupBagAmount != other.BackupBagAmount ||
                       CreatedBy != other.CreatedBy;
            }
        }
        private async Task LoadAnalyticsData()
        {
            try
            {
                var shifts = await FinancialService.GetLotteryAnalyticsAsync(_filterStartDate, _filterEndDate);
                _analyticsShifts = shifts.OrderByDescending(s => s.ShiftDate).ToList();

                using var db = await DbFactory.CreateDbContextAsync();
                _analyticsWeeklyStats = await db.LotteryWeeklyStats
                    .Where(w => w.WeekEndingDate >= _filterStartDate.Date && w.WeekEndingDate <= _filterEndDate.Date)
                    .OrderBy(w => w.WeekEndingDate)
                    .ToListAsync();

                CalculateAnalyticsStats();
                
                // We need to wait for the UI to render the canvas before calling JS
                _ = Task.Delay(100).ContinueWith(async _ => await UpdateAnalyticsCharts());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading analytics data");
                _error = "Failed to load analytics: " + ex.Message;
            }
        }

        private void CalculateAnalyticsStats()
        {
            if (!_analyticsShifts.Any() && !_analyticsWeeklyStats.Any())
            {
                _stats = new LotteryAnalyticsStats();
                return;
            }

            decimal totalWeeklyIncome = _analyticsWeeklyStats.Sum(w => 
                Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) + 
                Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) + 
                Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus) - 
                (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)));

            _stats.TotalIncome = totalWeeklyIncome;
            _stats.TotalVariance = _analyticsShifts.Sum(s => s.Variance);
            _stats.TotalNetProfit = _stats.TotalIncome + _stats.TotalVariance;
            _stats.AvgVariance = _analyticsShifts.Any() ? _analyticsShifts.Average(s => s.Variance) : 0m;
            
            _stats.PerfectShiftCount = _analyticsShifts.Count(s => Math.Abs(s.Variance) <= 0.05m);
            _stats.ShortShiftCount = _analyticsShifts.Count(s => s.Variance < -0.05m);
            _stats.StabilityScore = _analyticsShifts.Any() ? (decimal)_stats.PerfectShiftCount / _analyticsShifts.Count * 100 : 0m;
            _stats.ShortageFrequency = _analyticsShifts.Any() ? (decimal)_stats.ShortShiftCount / _analyticsShifts.Count * 100 : 0m;
            
            var shortUsers = _analyticsShifts
                .Where(s => s.Variance < 0)
                .GroupBy(s => s.EmployeeName)
                .Select(g => new { Name = g.Key, TotalShort = g.Sum(s => s.Variance) })
                .OrderBy(u => u.TotalShort)
                .FirstOrDefault();
                
            _stats.TopShortUser = shortUsers?.Name;
        }

        private async Task UpdateAnalyticsCharts()
        {
            List<object> incomeVsVarDatasets;
            List<object> varianceTrendDatasets;
            List<string> labels;

            if (_breakdownRangeType == "year")
            {
                var year = _selectedYear;
                var monthLabels = new List<string>();
                var monthlyIncome = new List<decimal>();
                var monthlyVariance = new List<decimal>();
                var monthlyNet = new List<decimal>();

                for (int m = 1; m <= 12; m++)
                {
                    var mStart = new DateTime(year, m, 1);
                    var mEnd = mStart.AddMonths(1).AddDays(-1);

                    var mWeekly = _analyticsWeeklyStats
                        .Where(w => w.WeekEndingDate >= mStart && w.WeekEndingDate <= mEnd)
                        .ToList();

                    decimal mIncome = mWeekly.Sum(w => 
                        Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) + 
                        Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) + 
                        Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus) - 
                        (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)));

                    decimal mVariance = _analyticsShifts
                        .Where(s => s.ShiftDate.Date >= mStart && s.ShiftDate.Date <= mEnd)
                        .Sum(s => s.Variance);

                    monthLabels.Add(mStart.ToString("MMM"));
                    monthlyIncome.Add(mIncome);
                    monthlyVariance.Add(mVariance);
                    monthlyNet.Add(mIncome + mVariance);
                }

                incomeVsVarDatasets = new List<object>
                {
                    new { label = "Downloaded Commission Income", data = monthlyIncome, color = "#3b82f6", bg = "rgba(59, 130, 246, 0.7)", type = "bar" },
                    new { label = "Cash Variance", data = monthlyVariance, color = "#ef4444", bg = "rgba(239, 68, 68, 0.7)", type = "bar" },
                    new { label = "Net Profit", data = monthlyNet, color = "#10b981", bg = "rgba(16, 185, 129, 0.1)", type = "line" }
                };

                await JS.InvokeVoidAsync("financialCharts.renderChart", "incomeVarianceChart", new { 
                    type = "bar", 
                    labels = monthLabels, 
                    datasets = incomeVsVarDatasets
                });

                varianceTrendDatasets = new List<object>
                {
                    new { label = "Monthly Variance", data = monthlyVariance, color = "#f59e0b", bg = "rgba(245, 158, 11, 0.1)", type = "line" }
                };

                await JS.InvokeVoidAsync("financialCharts.renderChart", "varianceTrendChart", new { 
                    type = "line", 
                    labels = monthLabels, 
                    datasets = varianceTrendDatasets
                });
                return;
            }

            if (_analyticsWeeklyStats.Any())
            {
                var weeklyData = _analyticsWeeklyStats.Select(w => {
                    var weekStart = w.WeekEndingDate.AddDays(-6);
                    var weekEnd = w.WeekEndingDate;
                    decimal income = Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) + 
                                     Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) + 
                                     Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus) - 
                                     (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee));
                    decimal variance = _analyticsShifts
                        .Where(s => s.ShiftDate.Date >= weekStart && s.ShiftDate.Date <= weekEnd)
                        .Sum(s => s.Variance);
                    return new {
                        Date = $"{w.WeekEndingDate:MM/dd}",
                        Income = income,
                        Variance = variance,
                        Net = income + variance
                    };
                }).ToList();

                labels = weeklyData.Select(d => d.Date).ToList();
                incomeVsVarDatasets = new List<object>
                {
                    new { label = "Downloaded Commission Income", data = weeklyData.Select(d => d.Income).ToList(), color = "#3b82f6", bg = "rgba(59, 130, 246, 0.7)", type = "bar" },
                    new { label = "Cash Variance", data = weeklyData.Select(d => d.Variance).ToList(), color = "#ef4444", bg = "rgba(239, 68, 68, 0.7)", type = "bar" },
                    new { label = "Net Profit", data = weeklyData.Select(d => d.Net).ToList(), color = "#10b981", bg = "rgba(16, 185, 129, 0.1)", type = "line" }
                };
            }
            else
            {
                var dailyData = _analyticsShifts
                    .GroupBy(s => s.ShiftDate.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new { 
                        Date = g.Key.ToString("MM/dd"), 
                        Income = 0m, 
                        Variance = g.Sum(s => s.Variance),
                        Net = g.Sum(s => s.Variance)
                    }).ToList();

                labels = dailyData.Select(d => d.Date).ToList();
                incomeVsVarDatasets = new List<object>
                {
                    new { label = "Downloaded Commission Income", data = dailyData.Select(d => d.Income).ToList(), color = "#3b82f6", bg = "rgba(59, 130, 246, 0.7)", type = "bar" },
                    new { label = "Cash Variance", data = dailyData.Select(d => d.Variance).ToList(), color = "#ef4444", bg = "rgba(239, 68, 68, 0.7)", type = "bar" },
                    new { label = "Net Profit", data = dailyData.Select(d => d.Net).ToList(), color = "#10b981", bg = "rgba(16, 185, 129, 0.1)", type = "line" }
                };
            }

            await JS.InvokeVoidAsync("financialCharts.renderChart", "incomeVarianceChart", new { 
                type = "bar", 
                labels = labels, 
                datasets = incomeVsVarDatasets
            });

            var dailyVarianceData = _analyticsShifts
                .GroupBy(s => s.ShiftDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { 
                    Date = g.Key.ToString("MM/dd"), 
                    Variance = g.Sum(s => s.Variance)
                }).ToList();

            varianceTrendDatasets = new List<object>
            {
                new { label = "Daily Variance", data = dailyVarianceData.Select(d => d.Variance).ToList(), color = "#f59e0b", bg = "rgba(245, 158, 11, 0.1)", type = "line" }
            };

            await JS.InvokeVoidAsync("financialCharts.renderChart", "varianceTrendChart", new { 
                type = "line", 
                labels = dailyVarianceData.Select(d => d.Date).ToList(), 
                datasets = varianceTrendDatasets
            });
        }

        private async Task LoadBreakdownData()
        {
            try
            {
                await LoadShifts();

                using var db = await DbFactory.CreateDbContextAsync();
                var startDate = _filterStartDate.Date;
                var endDate = _filterEndDate.Date;

                var weeklyStats = await db.LotteryWeeklyStats
                    .Where(w => w.WeekEndingDate >= startDate.AddDays(-7) && w.WeekEndingDate <= endDate.AddDays(7))
                    .ToListAsync();

                var vendingDrops = await db.LotteryVendingCollections
                    .Where(v => v.CollectionDate.Date >= startDate && v.CollectionDate.Date <= endDate)
                    .ToListAsync();

                _breakdownDailyItems.Clear();
                decimal periodTotalEnvelope = _shifts.Sum(s => s.EnvelopeAmount);
                int totalDays = (endDate - startDate).Days + 1;

                if (totalDays <= 14)
                {
                    // Daily aggregation
                    var currentDate = startDate;
                    while (currentDate <= endDate)
                    {
                        var dayShifts = _shifts.Where(s => s.ShiftDate.Date == currentDate.Date).ToList();
                        decimal dayEnvelope = dayShifts.Sum(s => s.EnvelopeAmount);
                        decimal dayVending = vendingDrops.Where(v => v.CollectionDate.Date == currentDate.Date).Sum(v => v.AmountCollected);

                        var closingShift = dayShifts.OrderByDescending(s => s.ShiftId).FirstOrDefault();
                        decimal dayNetDue = closingShift != null ? closingShift.NetDue : 0;
                        decimal daySales = dayShifts.Sum(s => s.TotalSales);
                        decimal dayVariance = dayShifts.Sum(s => s.Variance);

                        double pct = periodTotalEnvelope > 0 ? (double)(dayEnvelope / periodTotalEnvelope * 100) : 0;

                        var dayStatement = weeklyStats.FirstOrDefault(w => w.WeekEndingDate.Date == currentDate.Date);
                        decimal dayStatementDue = dayStatement != null ? Math.Abs(dayStatement.TotalDue) : 0;
                        decimal dayOnlineDue = dayStatement != null ? Math.Abs(dayStatement.OnlineDue) : 0;

                        _breakdownDailyItems.Add(new DailyBreakdownItem
                        {
                            Date = currentDate,
                            DayName = currentDate.ToString("dddd"),
                            DateLabel = currentDate.ToString("ddd MM/dd"),
                            EnvelopeAmount = dayEnvelope,
                            VendingAmount = dayVending,
                            NetDue = dayNetDue,
                            TotalSales = daySales,
                            Variance = dayVariance,
                            WeeklyStatementDue = dayStatementDue,
                            OnlineDue = dayOnlineDue,
                            ShiftCount = dayShifts.Count,
                            PercentageOfTotal = Math.Round(pct, 1),
                            Shifts = dayShifts
                        });

                        currentDate = currentDate.AddDays(1);
                    }
                }
                else if (totalDays <= 60)
                {
                    // Weekly aggregation
                    var weekStart = GetWeekStart(startDate);
                    while (weekStart <= endDate)
                    {
                        var weekEnd = weekStart.AddDays(6);
                        var weekShifts = _shifts.Where(s => s.ShiftDate.Date >= weekStart && s.ShiftDate.Date <= weekEnd).ToList();
                        decimal weekEnvelope = weekShifts.Sum(s => s.EnvelopeAmount);
                        decimal weekVending = vendingDrops.Where(v => v.CollectionDate.Date >= weekStart && v.CollectionDate.Date <= weekEnd).Sum(v => v.AmountCollected);

                        decimal weekNetDue = weekShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var lastShift = g.OrderByDescending(s => s.ShiftId).FirstOrDefault();
                                return lastShift != null ? lastShift.NetDue : 0;
                            });

                        decimal weekSales = weekShifts.Sum(s => s.TotalSales);
                        decimal weekVariance = weekShifts.Sum(s => s.Variance);

                        double pct = periodTotalEnvelope > 0 ? (double)(weekEnvelope / periodTotalEnvelope * 100) : 0;

                        var weekStatements = weeklyStats.Where(w => w.WeekEndingDate.Date >= weekStart && w.WeekEndingDate.Date <= weekEnd).ToList();
                        decimal weekStatementDue = weekStatements.Sum(w => Math.Abs(w.TotalDue));
                        decimal weekOnlineDue = weekStatements.Sum(w => Math.Abs(w.OnlineDue));

                        _breakdownDailyItems.Add(new DailyBreakdownItem
                        {
                            Date = weekStart,
                            DayName = $"Week of {weekStart:MMM d}",
                            DateLabel = $"{weekStart:MMM d} - {weekEnd:MMM d}",
                            EnvelopeAmount = weekEnvelope,
                            VendingAmount = weekVending,
                            NetDue = weekNetDue,
                            TotalSales = weekSales,
                            Variance = weekVariance,
                            WeeklyStatementDue = weekStatementDue,
                            OnlineDue = weekOnlineDue,
                            ShiftCount = weekShifts.Count,
                            PercentageOfTotal = Math.Round(pct, 1),
                            Shifts = weekShifts
                        });

                        weekStart = weekStart.AddDays(7);
                    }
                }
                else
                {
                    // Monthly aggregation
                    var mStart = new DateTime(startDate.Year, startDate.Month, 1);
                    var mLimit = new DateTime(endDate.Year, endDate.Month, 1);
                    while (mStart <= mLimit)
                    {
                        var mEnd = mStart.AddMonths(1).AddDays(-1);
                        var monthShifts = _shifts.Where(s => s.ShiftDate.Date >= mStart && s.ShiftDate.Date <= mEnd).ToList();
                        decimal mEnvelope = monthShifts.Sum(s => s.EnvelopeAmount);
                        decimal mVending = vendingDrops.Where(v => v.CollectionDate.Date >= mStart && v.CollectionDate.Date <= mEnd).Sum(v => v.AmountCollected);

                        decimal mNetDue = monthShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var lastShift = g.OrderByDescending(s => s.ShiftId).FirstOrDefault();
                                return lastShift != null ? lastShift.NetDue : 0;
                            });

                        decimal mSales = monthShifts.Sum(s => s.TotalSales);
                        decimal mVariance = monthShifts.Sum(s => s.Variance);

                        double pct = periodTotalEnvelope > 0 ? (double)(mEnvelope / periodTotalEnvelope * 100) : 0;

                        var monthStatements = weeklyStats.Where(w => w.WeekEndingDate.Date >= mStart && w.WeekEndingDate.Date <= mEnd).ToList();
                        decimal monthStatementDue = monthStatements.Sum(w => Math.Abs(w.TotalDue));
                        decimal monthOnlineDue = monthStatements.Sum(w => Math.Abs(w.OnlineDue));

                        _breakdownDailyItems.Add(new DailyBreakdownItem
                        {
                            Date = mStart,
                            DayName = mStart.ToString("MMMM yyyy"),
                            DateLabel = mStart.ToString("MMM yyyy"),
                            EnvelopeAmount = mEnvelope,
                            VendingAmount = mVending,
                            NetDue = mNetDue,
                            TotalSales = mSales,
                            Variance = mVariance,
                            WeeklyStatementDue = monthStatementDue,
                            OnlineDue = monthOnlineDue,
                            ShiftCount = monthShifts.Count,
                            PercentageOfTotal = Math.Round(pct, 1),
                            Shifts = monthShifts
                        });

                        mStart = mStart.AddMonths(1);
                    }
                }

                _breakdownStats.TotalEnvelopeDrops = periodTotalEnvelope;
                _breakdownStats.TotalVendingDrops = _breakdownDailyItems.Sum(d => d.VendingAmount);
                _breakdownStats.TotalWeeklyStatementDue = _breakdownDailyItems.Sum(d => d.WeeklyStatementDue);
                if (_breakdownStats.TotalWeeklyStatementDue == 0 && weeklyStats.Any())
                {
                    _breakdownStats.TotalWeeklyStatementDue = weeklyStats.Sum(w => Math.Abs(w.TotalDue));
                }
                _breakdownStats.TotalOnlineDue = _breakdownDailyItems.Sum(d => d.OnlineDue);

                var peakDay = _breakdownDailyItems.OrderByDescending(d => d.EnvelopeAmount).FirstOrDefault();
                if (peakDay != null && peakDay.EnvelopeAmount > 0)
                {
                    _breakdownStats.PeakDropDayLabel = peakDay.DateLabel;
                    _breakdownStats.PeakDropDayAmount = peakDay.EnvelopeAmount;
                }
                else
                {
                    _breakdownStats.PeakDropDayLabel = "N/A";
                    _breakdownStats.PeakDropDayAmount = 0;
                }
                _breakdownStats.TotalShifts = _shifts.Count;
                _needsChartUpdate = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading breakdown data");
                _error = "Failed to load breakdown: " + ex.Message;
            }
        }

        private async Task ToggleBreakdownMetric(string metric)
        {
            if (metric == "envelope") _showMetricEnvelope = !_showMetricEnvelope;
            else if (metric == "vending") _showMetricVending = !_showMetricVending;
            else if (metric == "netdue") _showMetricNetDue = !_showMetricNetDue;
            else if (metric == "sales") _showMetricSales = !_showMetricSales;
            else if (metric == "variance") _showMetricVariance = !_showMetricVariance;
            else if (metric == "weeklydue") _showMetricWeeklyDue = !_showMetricWeeklyDue;
            else if (metric == "onlinedue") _showMetricOnlineDue = !_showMetricOnlineDue;

            await UpdateBreakdownChart();
        }

        private bool _isBreakdownStacked = true;

        private async Task ToggleBreakdownStackMode()
        {
            _isBreakdownStacked = !_isBreakdownStacked;
            await UpdateBreakdownChart();
        }

        private async Task UpdateBreakdownChart()
        {
            if (!_breakdownDailyItems.Any()) return;

            var labels = _breakdownDailyItems.Select(d => d.DateLabel).ToList();
            var datasets = new List<object>();

            string? cashStack = _isBreakdownStacked ? "cash" : null;
            string? netDueStack = _isBreakdownStacked ? "netdue" : null;
            string? salesStack = _isBreakdownStacked ? "sales" : null;
            string? varianceStack = _isBreakdownStacked ? "variance" : null;
            string? weeklyDueStack = _isBreakdownStacked ? "weeklydue" : null;
            string? onlineDueStack = _isBreakdownStacked ? "onlinedue" : null;

            datasets.Add(new { 
                label = "Envelope Drops", 
                data = _breakdownDailyItems.Select(d => d.EnvelopeAmount).ToList(), 
                color = "#10b981", 
                bg = "rgba(16, 185, 129, 0.85)", 
                type = "bar",
                stack = cashStack,
                hidden = !_showMetricEnvelope
            });

            datasets.Add(new { 
                label = "Vending Machine Drops", 
                data = _breakdownDailyItems.Select(d => d.VendingAmount).ToList(), 
                color = "#f59e0b", 
                bg = "rgba(245, 158, 11, 0.9)", 
                type = "bar",
                stack = cashStack,
                hidden = !_showMetricVending
            });

            datasets.Add(new { 
                label = "Net Due to Lottery", 
                data = _breakdownDailyItems.Select(d => d.NetDue).ToList(), 
                color = "#3b82f6", 
                bg = "rgba(59, 130, 246, 0.75)", 
                type = "bar",
                stack = netDueStack,
                hidden = !_showMetricNetDue
            });

            datasets.Add(new { 
                label = "Tickets", 
                data = _breakdownDailyItems.Select(d => d.TotalSales).ToList(), 
                color = "#6366f1", 
                bg = "rgba(99, 102, 241, 0.75)", 
                type = "bar",
                stack = salesStack,
                hidden = !_showMetricSales
            });

            datasets.Add(new { 
                label = "Cash Variance", 
                data = _breakdownDailyItems.Select(d => d.Variance).ToList(), 
                color = "#ef4444", 
                bg = "rgba(239, 68, 68, 0.75)", 
                type = "bar",
                stack = varianceStack,
                hidden = !_showMetricVariance
            });

            datasets.Add(new { 
                label = "Weekly Statement Due", 
                data = _breakdownDailyItems.Select(d => d.WeeklyStatementDue).ToList(), 
                color = "#ec4899", 
                bg = "rgba(236, 72, 153, 0.75)", 
                type = "bar",
                stack = weeklyDueStack,
                hidden = !_showMetricWeeklyDue
            });

            datasets.Add(new { 
                label = "Online Due", 
                data = _breakdownDailyItems.Select(d => d.OnlineDue).ToList(), 
                color = "#a855f7", 
                bg = "rgba(168, 85, 247, 0.75)", 
                type = "bar",
                stack = onlineDueStack,
                hidden = !_showMetricOnlineDue
            });

            await JS.InvokeVoidAsync("financialCharts.renderChart", "breakdownEnvelopeChart", new { 
                type = "bar", 
                labels = labels, 
                datasets = datasets 
            });
        }

        private async Task LoadCommissionsData()
        {
            try
            {
                using var db = await DbFactory.CreateDbContextAsync();
                var startDate = _filterStartDate.Date;
                var endDate = _filterEndDate.Date;

                _weeklyCommissionsStats = await db.LotteryWeeklyStats
                    .Where(w => w.WeekEndingDate >= startDate && w.WeekEndingDate <= endDate)
                    .OrderBy(w => w.WeekEndingDate)
                    .ToListAsync();

                _ = Task.Delay(100).ContinueWith(async _ => await UpdateCommissionsChart());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading weekly commissions data");
                _error = "Failed to load commissions: " + ex.Message;
            }
        }

        private async Task ToggleCommissionsMetric(string metric)
        {
            if (metric == "commissions") _showMetricCommissions = !_showMetricCommissions;
            else if (metric == "cashbonus") _showMetricCashBonus = !_showMetricCashBonus;
            else if (metric == "claimsbonus") _showMetricClaimsBonus = !_showMetricClaimsBonus;
            else if (metric == "fees") _showMetricFees = !_showMetricFees;

            await UpdateCommissionsChart();
        }

        private async Task UpdateCommissionsChart()
        {
            if (!_weeklyCommissionsStats.Any()) return;

            if (_breakdownRangeType == "year")
            {
                var year = _selectedYear;
                var monthLabels = new List<string>();
                var commData = new List<decimal>();
                var cashBonusData = new List<decimal>();
                var claimsBonusData = new List<decimal>();
                var feesData = new List<decimal>();

                for (int m = 1; m <= 12; m++)
                {
                    var mStart = new DateTime(year, m, 1);
                    var mEnd = mStart.AddMonths(1).AddDays(-1);
                    var mStats = _weeklyCommissionsStats.Where(w => w.WeekEndingDate >= mStart && w.WeekEndingDate <= mEnd).ToList();

                    monthLabels.Add(mStart.ToString("MMM"));
                    commData.Add(_showMetricCommissions ? mStats.Sum(w => Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission)) : 0m);
                    cashBonusData.Add(_showMetricCashBonus ? mStats.Sum(w => Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus)) : 0m);
                    claimsBonusData.Add(_showMetricClaimsBonus ? mStats.Sum(w => Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)) : 0m);
                    feesData.Add(_showMetricFees ? mStats.Sum(w => Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)) : 0m);
                }

                var yearDatasets = new List<object>
                {
                    new { label = "Commissions", data = commData, color = "#10b981", bg = "rgba(16, 185, 129, 0.7)", type = "bar", stack = "earnings" },
                    new { label = "Cash Bonus", data = cashBonusData, color = "#3b82f6", bg = "rgba(59, 130, 246, 0.7)", type = "bar", stack = "earnings" },
                    new { label = "Claims Bonus", data = claimsBonusData, color = "#8b5cf6", bg = "rgba(139, 92, 246, 0.7)", type = "bar", stack = "earnings" },
                    new { label = "Weekly Fees", data = feesData, color = "#ef4444", bg = "rgba(239, 68, 68, 0.7)", type = "bar", stack = "fees" }
                };

                await JS.InvokeVoidAsync("financialCharts.renderChart", "commissionsReportChart", new { 
                    type = "bar", 
                    labels = monthLabels, 
                    datasets = yearDatasets 
                });
                return;
            }

            var labels = _weeklyCommissionsStats.Select(w => w.WeekEndingDate.ToString("MM/dd")).ToList();
            var datasets = new List<object>();

            datasets.Add(new { 
                label = "Commissions", 
                data = _showMetricCommissions ? _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission)).ToList() : _weeklyCommissionsStats.Select(w => 0m).ToList(), 
                color = "#10b981", 
                bg = "rgba(16, 185, 129, 0.7)", 
                type = "bar",
                stack = "earnings"
            });

            datasets.Add(new { 
                label = "Cash Bonus", 
                data = _showMetricCashBonus ? _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus)).ToList() : _weeklyCommissionsStats.Select(w => 0m).ToList(), 
                color = "#3b82f6", 
                bg = "rgba(59, 130, 246, 0.7)", 
                type = "bar",
                stack = "earnings"
            });

            datasets.Add(new { 
                label = "Claims Bonus", 
                data = _showMetricClaimsBonus ? _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)).ToList() : _weeklyCommissionsStats.Select(w => 0m).ToList(), 
                color = "#8b5cf6", 
                bg = "rgba(139, 92, 246, 0.7)", 
                type = "bar",
                stack = "earnings"
            });

            datasets.Add(new { 
                label = "Weekly Fees", 
                data = _showMetricFees ? _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)).ToList() : _weeklyCommissionsStats.Select(w => 0m).ToList(), 
                color = "#ef4444", 
                bg = "rgba(239, 68, 68, 0.7)", 
                type = "bar",
                stack = "fees"
            });

            await JS.InvokeVoidAsync("financialCharts.renderChart", "commissionsReportChart", new { 
                type = "bar", 
                labels = labels, 
                datasets = datasets 
            });
        }

        private void ToggleBreakdownDayDetail(DateTime date)
        {
            if (_expandedBreakdownDays.Contains(date.Date))
                _expandedBreakdownDays.Remove(date.Date);
            else
                _expandedBreakdownDays.Add(date.Date);
        }

        public class LotteryAnalyticsStats
        {
            public decimal TotalIncome { get; set; }
            public decimal TotalVariance { get; set; }
            public decimal TotalNetProfit { get; set; }
            public decimal AvgVariance { get; set; }
            public int PerfectShiftCount { get; set; }
            public int ShortShiftCount { get; set; }
            public decimal StabilityScore { get; set; }
            public decimal ShortageFrequency { get; set; }
            public string? TopShortUser { get; set; }
        }



        private async Task CreateClosedShift(DateTime date, string shiftType)
        {
            try
            {
                var currentUser = AuthStateProvider.GetCurrentUser();
                var username = currentUser?.Username ?? "System";
                
                decimal startingCash = 1200; // default target
                
                // Fetch previous shifts to find baseline ending cash to carry over
                var previousShifts = await Task.Run(() => LotteryService.GetShiftsByDateRange(date.AddDays(-7), date));
                var latestShift = previousShifts
                    .Where(s => s.ShiftDate.Date < date.Date || (s.ShiftDate.Date == date.Date && string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase) && string.Equals(shiftType, "Night", StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(s => s.ShiftDate.Date)
                    .ThenBy(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase) ? 1 : 0)
                    .FirstOrDefault();

                if (latestShift != null)
                {
                    startingCash = latestShift.EndingCash;
                }

                var shift = new LotteryShift
                {
                    ShiftDate = date.Date,
                    EmployeeName = "Club Closed",
                    ShiftType = shiftType,
                    MachineId = "MAIN",
                    StartingCash = startingCash,
                    EndingCash = startingCash, // Ending cash is same as starting cash
                    TotalSales = 0,
                    TotalPayouts = 0,
                    TotalCancels = 0,
                    NetDue = 0,
                    NetSales = 0,
                    ExpectedCash = startingCash,
                    Variance = 0,
                    Notes = "Club Closed",
                    Status = "Submitted",
                    IsReconciled = false,
                    CreatedBy = username,
                    CreatedDate = DateTime.UtcNow
                };

                await Task.Run(() => LotteryService.CreateShift(shift, username));
                await LoadData();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating closed shift");
                _error = "Failed to mark shift as closed: " + ex.Message;
            }
        }

        private async Task LoadVendingData()
        {
            try
            {
                using var db = await DbFactory.CreateDbContextAsync();
                _vendingCollections = await db.LotteryVendingCollections
                    .OrderByDescending(v => v.CollectionDate)
                    .ToListAsync();

                if (_viewMode == "vending")
                {
                    await UpdateVendingChart();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading vending collections");
                _error = "Failed to load vending collections: " + ex.Message;
            }
        }

        private IEnumerable<LotteryVendingCollection> FilteredVendingCollections
        {
            get
            {
                return _vendingCollections
                    .Where(v => v.CollectionDate.Date >= _filterStartDate.Date && v.CollectionDate.Date <= _filterEndDate.Date)
                    .OrderByDescending(v => v.CollectionDate);
            }
        }

        private async Task UpdateVendingChart()
        {
            List<string> labels = new();
            List<decimal> values = new();
            string chartLabel = "Vending Machine Drops ($)";

            var periodDrops = FilteredVendingCollections.OrderBy(v => v.CollectionDate).ToList();

            if (periodDrops.Any())
            {
                labels = periodDrops.Select(v => v.CollectionDate.ToString("MM/dd")).ToList();
                values = periodDrops.Select(v => v.AmountCollected).ToList();
            }
            else
            {
                labels.Add($"{_filterStartDate:MM/dd} - {_filterEndDate:MM/dd}");
                values.Add(0);
            }

            var datasets = new List<object>
            {
                new {
                    label = chartLabel,
                    data = values,
                    color = "#f59e0b",
                    bg = "rgba(245, 158, 11, 0.7)",
                    type = "bar"
                }
            };

            await JS.InvokeVoidAsync("financialCharts.renderChart", "vendingCollectionChart", new {
                type = "bar",
                labels = labels,
                datasets = datasets
            });
        }

        private void OpenNewVendingModal()
        {
            var currentUser = AuthStateProvider.GetCurrentUser();
            var username = currentUser?.Username ?? "Staff";
            var userFullName = _employeeMetadata.FirstOrDefault(e => string.Equals(e.Username, username, StringComparison.OrdinalIgnoreCase)).FullName;
            var loggedInUser = !string.IsNullOrEmpty(userFullName) ? userFullName : username;

            _editingVendingCollection = new LotteryVendingCollection
            {
                CollectionDate = default,
                PeriodStartDate = default,
                PeriodEndDate = default,
                EnteredBy = loggedInUser
            };
            _modalError = string.Empty;
            _showVendingModal = true;
        }

        private void OpenEditVendingModal(LotteryVendingCollection item)
        {
            _editingVendingCollection = new LotteryVendingCollection
            {
                Id = item.Id,
                CollectionDate = item.CollectionDate,
                PeriodStartDate = item.PeriodStartDate,
                PeriodEndDate = item.PeriodEndDate,
                AmountCollected = item.AmountCollected,
                EnteredBy = item.EnteredBy,
                Notes = item.Notes,
                CreatedAt = item.CreatedAt
            };
            _modalError = string.Empty;
            _showVendingModal = true;
        }

        private void CloseVendingModal()
        {
            _showVendingModal = false;
        }

        private void OnVendingStartDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var startDate))
            {
                _editingVendingCollection.PeriodStartDate = startDate;
            }
            else
            {
                _editingVendingCollection.PeriodStartDate = default;
            }
        }

        private void OnVendingEndDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var endDate))
            {
                _editingVendingCollection.PeriodEndDate = endDate;
                _editingVendingCollection.CollectionDate = endDate;
            }
            else
            {
                _editingVendingCollection.PeriodEndDate = default;
                _editingVendingCollection.CollectionDate = default;
            }
        }

        private async Task SaveVendingCollection()
        {
            try
            {
                if (_editingVendingCollection.PeriodStartDate == default || _editingVendingCollection.PeriodEndDate == default)
                {
                    _modalError = "Please select both a report start date and report end date.";
                    return;
                }

                // Always sync CollectionDate to PeriodEndDate (cash removed date matches report end date)
                _editingVendingCollection.CollectionDate = _editingVendingCollection.PeriodEndDate;

                if (_editingVendingCollection.AmountCollected <= 0)
                {
                    _modalError = "Amount collected must be greater than $0.";
                    return;
                }

                if (_editingVendingCollection.PeriodStartDate.Date > _editingVendingCollection.PeriodEndDate.Date)
                {
                    _modalError = "Report start date cannot be after the report end date.";
                    return;
                }

                using var db = await DbFactory.CreateDbContextAsync();

                var newStart = _editingVendingCollection.PeriodStartDate.Date;
                var newEnd = _editingVendingCollection.PeriodEndDate.Date;

                // Prevent interior overlapping collection date ranges (e.g. 6/19-6/20 inside 6/18-6/23)
                // Sharing a transition date (e.g. 6/23-6/25 and 6/25-6/26) is permitted.
                var existingDuplicate = await db.LotteryVendingCollections
                    .FirstOrDefaultAsync(v => v.Id != _editingVendingCollection.Id &&
                        (newStart < v.PeriodEndDate.Date && newEnd > v.PeriodStartDate.Date));

                if (existingDuplicate != null)
                {
                    _modalError = $"Overlapping entry prevented: A vending collection for {existingDuplicate.PeriodStartDate:MM/dd/yyyy} - {existingDuplicate.PeriodEndDate:MM/dd/yyyy} (${existingDuplicate.AmountCollected:N2} by {existingDuplicate.EnteredBy}) already covers part of this date range.";
                    return;
                }

                if (_editingVendingCollection.Id == 0)
                {
                    _editingVendingCollection.CreatedAt = DateTime.Now;
                    db.LotteryVendingCollections.Add(_editingVendingCollection);
                }
                else
                {
                    _editingVendingCollection.UpdatedAt = DateTime.Now;
                    db.LotteryVendingCollections.Update(_editingVendingCollection);
                }

                await db.SaveChangesAsync();
                _showVendingModal = false;
                await LoadVendingData();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error saving vending collection");
                _modalError = "Failed to save collection: " + ex.Message;
            }
        }

        private bool _showDeleteVendingModal = false;
        private LotteryVendingCollection? _vendingToDelete = null;

        private void PromptDeleteVendingCollection(LotteryVendingCollection item)
        {
            _vendingToDelete = item;
            _showDeleteVendingModal = true;
        }

        private void CloseDeleteVendingModal()
        {
            _showDeleteVendingModal = false;
            _vendingToDelete = null;
        }

        private async Task ConfirmDeleteVendingCollectionAction()
        {
            if (_vendingToDelete != null)
            {
                int id = _vendingToDelete.Id;
                _showDeleteVendingModal = false;
                _vendingToDelete = null;
                await DeleteVendingCollection(id);
            }
        }

        private async Task HandleVendingKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SaveVendingCollection();
            }
        }

        private async Task DeleteVendingCollection(int id)
        {
            try
            {
                using var db = await DbFactory.CreateDbContextAsync();
                var item = await db.LotteryVendingCollections.FindAsync(id);
                if (item != null)
                {
                    db.LotteryVendingCollections.Remove(item);
                    await db.SaveChangesAsync();
                    await LoadVendingData();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting vending collection");
                _error = "Failed to delete collection: " + ex.Message;
            }
        }
    }
}


