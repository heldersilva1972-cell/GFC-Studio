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
        private List<LotteryShiftSummaryDto> _weeklySummaries = new();
        private List<LotteryShiftSummaryDto> _monthlySummaries = new();
        private List<string> _employeeNames = new();
        private List<LotteryWeeklyStat> _weeklyCommissionsStats = new();
        private bool _showMetricCommissions = true;
        private bool _showMetricCashBonus = true;
        private bool _showMetricClaimsBonus = true;
        private bool _showMetricFees = true;
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
        private bool _showMetricNetDue = false;
        private bool _showMetricSales = false;
        private bool _showMetricVariance = false;
        private bool _showMetricWeeklyDue = false;
        private string _breakdownRangeType = "week"; // "week", "month", "year", "custom"
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
            public string PeakDropDayLabel { get; set; } = "N/A";
            public decimal PeakDropDayAmount { get; set; }
            public int TotalShifts { get; set; }
            public decimal TotalWeeklyStatementDue { get; set; }
        }

        public class DailyBreakdownItem
        {
            public DateTime Date { get; set; }
            public string DayName { get; set; } = string.Empty;
            public string DateLabel { get; set; } = string.Empty;
            public decimal EnvelopeAmount { get; set; }
            public decimal NetDue { get; set; }
            public decimal TotalSales { get; set; }
            public decimal Variance { get; set; }
            public decimal WeeklyStatementDue { get; set; }
            public int ShiftCount { get; set; }
            public double PercentageOfTotal { get; set; }
            public List<LotteryShiftDto> Shifts { get; set; } = new();
        }

        private async Task OnMonthYearChanged()
        {
            if (_viewMode == "weekly" || _viewMode == "analytics" || (_viewMode == "breakdown" && _breakdownRangeType == "month"))
            {
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (_viewMode == "monthly" || (_viewMode == "breakdown" && _breakdownRangeType == "year"))
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
        private decimal TotalSales => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalSales) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.TotalSales));

        private decimal TotalPayouts => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalPayouts) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.TotalPayouts));

        private decimal TotalNetSales => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalNetSales) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.NetSales));

        private decimal TotalEnvelope => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalEnvelope) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.EnvelopeAmount));

        private decimal TotalVariance => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalVariance) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.Variance));

        private decimal TotalEarnings => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalIncome) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.Commission));

        private decimal TotalFees => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalFees) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.IdentifiedFees));
        
        private decimal TotalBagOut => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalBagOut) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.BackupBagAmount));

        private decimal TotalBagIn => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalBagIn) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.BagRefillAmount));

        private decimal NetBagDebt => TotalBagOut - TotalBagIn;

        private decimal TotalInstantTickets => (_viewMode == "weekly" ? _weeklySummaries.Sum(s => s.TotalCancels) : 
            _shifts.Where(s => s.Status != "Draft").Sum(s => s.TotalCancels));

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
            await LoadData();
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
                
                if (_viewMode == "shifts" || _viewMode == "daily")
                {
                    await LoadShifts();
                }
                if (_viewMode == "daily")
                {
                    await LoadDailySummaries();
                }
                else if (_viewMode == "weekly")
                {
                    await LoadWeeklySummaries();
                }
                else if (_viewMode == "monthly")
                {
                    await LoadMonthlySummaries();
                }
                else if (_viewMode == "commissions")
                {
                    await LoadCommissionsData();
                }
                else if (_viewMode == "analytics")
                {
                    await LoadAnalyticsData();
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
            // Start from the current Sat-Fri week and go back 12 weeks
            var currentSat = GetWeekStart(DateTime.Today);
            for (int i = 0; i < 12; i++)
            {
                var start = currentSat.AddDays(-7 * i);
                var end = start.AddDays(6);
                _availableWeeks.Add((start, end, $"{start:MMM d} - {end:MMM d, yyyy}"));
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
            var end = lastSaturday; // The last Saturday itself
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

        private async Task LoadWeeklySummaries()
        {
            try
            {
                var summaries = await Task.Run(() => LotteryService.GetWeeklySummaries(_filterStartDate, _filterEndDate));
                _weeklySummaries = summaries ?? new List<LotteryShiftSummaryDto>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading weekly summaries");
                _weeklySummaries = new List<LotteryShiftSummaryDto>();
                throw;
            }
        }

        private async Task LoadMonthlySummaries()
        {
            try
            {
                var summaries = await Task.Run(() => LotteryService.GetMonthlySummaries(_selectedYear));
                _monthlySummaries = summaries ?? new List<LotteryShiftSummaryDto>();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error loading monthly summaries");
                _monthlySummaries = new List<LotteryShiftSummaryDto>();
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
            if (_viewMode == "daily" || _viewMode == "breakdown" || _viewMode == "commissions")
            {
                if (_viewMode == "daily" || _breakdownRangeType == "week")
                {
                    _filterStartDate = GetWeekStart(_filterStartDate);
                    _filterEndDate = _filterStartDate.AddDays(6);
                }
                else if (_breakdownRangeType == "month")
                {
                    var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                    _filterStartDate = range.Start;
                    _filterEndDate = range.End;
                }
                else if (_breakdownRangeType == "year")
                {
                    var range = GetSnappedYearRange(_selectedYear);
                    _filterStartDate = range.Start;
                    _filterEndDate = range.End;
                }
                else if (_breakdownRangeType == "custom")
                {
                    // Keep custom date ranges
                }
            }
            else if (_viewMode == "weekly")
            {
                // Snap to the full month for the weekly totals view
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (_viewMode == "analytics")
            {
                // Snap to the full month for analytics view as requested
                var range = GetSnappedMonthRange(_selectedYear, _selectedMonth);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (_viewMode == "monthly")
            {
                // Snap to the full year for the monthly totals (Yearly View)
                var range = GetSnappedYearRange(_selectedYear);
                _filterStartDate = range.Start;
                _filterEndDate = range.End;
            }
            else if (_viewMode == "imported")
            {
                // No special date snapping needed for imported reports workspace
            }

            await LoadData();
        }

        private async Task ChangeYear(int year)
        {
            _selectedYear = year;
            if (_viewMode == "monthly")
            {
                var range = GetSnappedYearRange(_selectedYear);
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
            
            // [FIX] Find the PREVIOUS shift on the same day to get the baseline for cumulative subtraction
            decimal baselineSales = 0, baselinePrizes = 0, baselineTickets = 0;
            if (string.Equals(shiftEntity.ShiftType, "Night", StringComparison.OrdinalIgnoreCase))
            {
                var dayShift = _shifts.FirstOrDefault(s => s.ShiftDate.Date == shiftEntity.ShiftDate.Date && string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase));
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
                // This ensures the database always has the "Money Added/Removed" for that shift specifically.
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

                if (_editingShiftId.HasValue)
                {
                    await Task.Run(() => LotteryService.UpdateShift(shift, username));
                }
                else
                {
                    await Task.Run(() => LotteryService.CreateShift(shift, username));
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
            
            [Required(ErrorMessage = "Ending cash is required")]
            [Range(0, double.MaxValue, ErrorMessage = "Ending cash must be 0 or greater")]
            public decimal? EndingCash { get; set; }
            
            [Required(ErrorMessage = "Total sales is required")]
            [Range(0, double.MaxValue, ErrorMessage = "Total sales must be 0 or greater")]
            public decimal? TotalSales { get; set; }
            
            [Required(ErrorMessage = "Total payouts is required")]
            [Range(0, double.MaxValue, ErrorMessage = "Total payouts must be 0 or greater")]
            public decimal? TotalPayouts { get; set; }
            
            [Required(ErrorMessage = "Tickets (RPT 34) is required")]
            [Range(0, double.MaxValue, ErrorMessage = "Tickets must be 0 or greater")]
            public decimal? TotalCancels { get; set; }

            [Required(ErrorMessage = "Net Due (RPT 50) is required")]
            public decimal? NetDue { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "Backup Bag must be 0 or greater")]
            public decimal? BagRefillAmount { get; set; }

            public decimal BackupBagAmount { get; set; }
            
            public string Notes { get; set; } = string.Empty;
            public string Status { get; set; } = "Submitted";
            public string CreatedBy { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; } // HANG PREVENTION
            
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
                    decimal activeSales = (TotalSales ?? 0) - BaselineSales;
                    decimal activePrizes = (TotalPayouts ?? 0) - BaselinePrizes;
                    decimal activeTickets = (TotalCancels ?? 0) - BaselineTickets;
                    
                    return activeSales - activePrizes - activeTickets;
                }
            }

            // [FIX]: ExpectedCash for display should be the target baseline ($1,200) after distributions.
            public decimal ExpectedCash => (StartingCash ?? 0) + NetSales + BackupBagAmount - (BagRefillAmount ?? 0) - EnvelopeAmount;
            
            // [CRITICAL FIX]: Variance calculation must ignore drops/refills to avoid "Ghost Discrepancies".
            // Variance = EndingCash - (Expected Cash BEFORE Drops)
            public decimal Variance => (EndingCash ?? 0) - ((StartingCash ?? 0) + NetSales + BackupBagAmount);
            
            // PERSIST THE ACTIVITY FIELDS FOR REPOSITORY
            public decimal ShiftSalesActivity => (TotalSales ?? 0) - BaselineSales;
            public decimal ShiftPayoutsActivity => (TotalPayouts ?? 0) - BaselinePrizes;
            public decimal ShiftCancelsActivity => (TotalCancels ?? 0) - BaselineTickets;

            // AUTOMATIC ENVELOPE CALCULATION ($1,200 Bag Target)
            public decimal EnvelopeAmount => (ShiftType != "Day" && (EndingCash ?? 0) > 1200) ? (EndingCash ?? 0) - 1200 : 0;

            public object Clone() => this.MemberwiseClone();

            public bool IsDirty(ShiftFormModel other)
            {
                if (other == null) return true;
                return ShiftDate != other.ShiftDate ||
                       EmployeeName != other.EmployeeName ||
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
                
                // Filtering
                IEnumerable<LotteryShift> query = shifts;
                if (!string.IsNullOrEmpty(_filterEmployee))
                {
                    query = query.Where(s => s.EmployeeName != null && 
                        s.EmployeeName.Trim().Equals(_filterEmployee.Trim(), StringComparison.OrdinalIgnoreCase));
                }
                
                _analyticsShifts = query.OrderByDescending(s => s.ShiftDate).ToList();
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
            if (!_analyticsShifts.Any())
            {
                _stats = new LotteryAnalyticsStats();
                return;
            }

            _stats.TotalIncome = _analyticsShifts.Sum(s => s.LotteryIncome);
            _stats.TotalVariance = _analyticsShifts.Sum(s => s.Variance);
            _stats.TotalNetProfit = _stats.TotalIncome + _stats.TotalVariance;
            _stats.AvgVariance = _analyticsShifts.Average(s => s.Variance);
            
            _stats.PerfectShiftCount = _analyticsShifts.Count(s => Math.Abs(s.Variance) <= 0.05m);
            _stats.ShortShiftCount = _analyticsShifts.Count(s => s.Variance < -0.05m);
            _stats.StabilityScore = (decimal)_stats.PerfectShiftCount / _analyticsShifts.Count * 100;
            _stats.ShortageFrequency = (decimal)_stats.ShortShiftCount / _analyticsShifts.Count * 100;
            
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
            var dailyData = _analyticsShifts
                .GroupBy(s => s.ShiftDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { 
                    Date = g.Key.ToString("MM/dd"), 
                    Income = g.Sum(s => s.LotteryIncome), 
                    Variance = g.Sum(s => s.Variance),
                    Net = g.Sum(s => s.LotteryIncome + s.Variance)
                }).ToList();

            var labels = dailyData.Select(d => d.Date).ToList();
            
            var incomeVsVarDatasets = new List<object>
            {
                new { label = "Actual Income", data = dailyData.Select(d => d.Income).ToList(), color = "#3b82f6", bg = "rgba(59, 130, 246, 0.7)", type = "bar" },
                new { label = "Variance", data = dailyData.Select(d => d.Variance).ToList(), color = "#ef4444", bg = "rgba(239, 68, 68, 0.7)", type = "bar" },
                new { label = "Net Profit", data = dailyData.Select(d => d.Net).ToList(), color = "#10b981", bg = "rgba(16, 185, 129, 0.1)", type = "line" }
            };

            await JS.InvokeVoidAsync("financialCharts.renderChart", "incomeVarianceChart", new { 
                type = "bar", 
                labels = labels, 
                datasets = incomeVsVarDatasets
            });

            var varianceTrendDatasets = new List<object>
            {
                new { label = "Daily Variance", data = dailyData.Select(d => d.Variance).ToList(), color = "#f59e0b", bg = "rgba(245, 158, 11, 0.1)", type = "line" }
            };

            await JS.InvokeVoidAsync("financialCharts.renderChart", "varianceTrendChart", new { 
                type = "line", 
                labels = labels, 
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

                        var nightShift = dayShifts.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));

                        decimal dayNetDue = nightShift != null ? nightShift.NetDue : 0;
                        decimal daySales = nightShift != null ? nightShift.TotalCancels : 0;
                        decimal dayVariance = nightShift != null ? nightShift.Variance : 0;

                        double pct = periodTotalEnvelope > 0 ? (double)(dayEnvelope / periodTotalEnvelope * 100) : 0;

                        var dayStatement = weeklyStats.FirstOrDefault(w => w.WeekEndingDate.Date == currentDate.Date);
                        decimal dayStatementDue = dayStatement != null ? Math.Abs(dayStatement.TotalDue) : 0;

                        _breakdownDailyItems.Add(new DailyBreakdownItem
                        {
                            Date = currentDate,
                            DayName = currentDate.ToString("dddd"),
                            DateLabel = currentDate.ToString("ddd MM/dd"),
                            EnvelopeAmount = dayEnvelope,
                            NetDue = dayNetDue,
                            TotalSales = daySales,
                            Variance = dayVariance,
                            WeeklyStatementDue = dayStatementDue,
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

                        decimal weekNetDue = weekShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var night = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
                                return night != null ? night.NetDue : 0;
                            });

                        decimal weekSales = weekShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var night = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
                                return night != null ? night.TotalCancels : 0;
                            });

                        decimal weekVariance = weekShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var night = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
                                return night != null ? night.Variance : 0;
                            });

                        double pct = periodTotalEnvelope > 0 ? (double)(weekEnvelope / periodTotalEnvelope * 100) : 0;

                        var weekStatements = weeklyStats.Where(w => w.WeekEndingDate.Date >= weekStart && w.WeekEndingDate.Date <= weekEnd).ToList();
                        decimal weekStatementDue = weekStatements.Sum(w => Math.Abs(w.TotalDue));

                        _breakdownDailyItems.Add(new DailyBreakdownItem
                        {
                            Date = weekStart,
                            DayName = $"Week of {weekStart:MMM d}",
                            DateLabel = $"{weekStart:MMM d} - {weekEnd:MMM d}",
                            EnvelopeAmount = weekEnvelope,
                            NetDue = weekNetDue,
                            TotalSales = weekSales,
                            Variance = weekVariance,
                            WeeklyStatementDue = weekStatementDue,
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

                        decimal mNetDue = monthShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var night = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
                                return night != null ? night.NetDue : 0;
                            });

                        decimal mSales = monthShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var night = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
                                return night != null ? night.TotalCancels : 0;
                            });

                        decimal mVariance = monthShifts
                            .GroupBy(s => s.ShiftDate.Date)
                            .Sum(g => {
                                var night = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
                                return night != null ? night.Variance : 0;
                            });

                        double pct = periodTotalEnvelope > 0 ? (double)(mEnvelope / periodTotalEnvelope * 100) : 0;

                        var monthStatements = weeklyStats.Where(w => w.WeekEndingDate.Date >= mStart && w.WeekEndingDate.Date <= mEnd).ToList();
                        decimal monthStatementDue = monthStatements.Sum(w => Math.Abs(w.TotalDue));

                        _breakdownDailyItems.Add(new DailyBreakdownItem
                        {
                            Date = mStart,
                            DayName = mStart.ToString("MMMM yyyy"),
                            DateLabel = mStart.ToString("MMM yyyy"),
                            EnvelopeAmount = mEnvelope,
                            NetDue = mNetDue,
                            TotalSales = mSales,
                            Variance = mVariance,
                            WeeklyStatementDue = monthStatementDue,
                            ShiftCount = monthShifts.Count,
                            PercentageOfTotal = Math.Round(pct, 1),
                            Shifts = monthShifts
                        });

                        mStart = mStart.AddMonths(1);
                    }
                }

                _breakdownStats.TotalEnvelopeDrops = periodTotalEnvelope;
                _breakdownStats.TotalWeeklyStatementDue = _breakdownDailyItems.Sum(d => d.WeeklyStatementDue);

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

                _ = Task.Delay(100).ContinueWith(async _ => await UpdateBreakdownChart());
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
            else if (metric == "netdue") _showMetricNetDue = !_showMetricNetDue;
            else if (metric == "sales") _showMetricSales = !_showMetricSales;
            else if (metric == "variance") _showMetricVariance = !_showMetricVariance;
            else if (metric == "weeklydue") _showMetricWeeklyDue = !_showMetricWeeklyDue;

            await UpdateBreakdownChart();
        }

        private async Task UpdateBreakdownChart()
        {
            if (!_breakdownDailyItems.Any()) return;

            var labels = _breakdownDailyItems.Select(d => d.DateLabel).ToList();
            var datasets = new List<object>();

            if (_showMetricEnvelope)
            {
                datasets.Add(new { 
                    label = "Envelope Drops", 
                    data = _breakdownDailyItems.Select(d => d.EnvelopeAmount).ToList(), 
                    color = "#10b981", 
                    bg = "rgba(16, 185, 129, 0.7)", 
                    type = "bar" 
                });
            }

            if (_showMetricNetDue)
            {
                datasets.Add(new { 
                    label = "Net Due to Lottery", 
                    data = _breakdownDailyItems.Select(d => d.NetDue).ToList(), 
                    color = "#3b82f6", 
                    bg = "rgba(59, 130, 246, 0.7)", 
                    type = "bar" 
                });
            }

            if (_showMetricSales)
            {
                datasets.Add(new { 
                    label = "Tickets", 
                    data = _breakdownDailyItems.Select(d => d.TotalSales).ToList(), 
                    color = "#6366f1", 
                    bg = "rgba(99, 102, 241, 0.7)", 
                    type = "bar" 
                });
            }

            if (_showMetricVariance)
            {
                datasets.Add(new { 
                    label = "Cash Variance", 
                    data = _breakdownDailyItems.Select(d => d.Variance).ToList(), 
                    color = "#ef4444", 
                    bg = "rgba(239, 68, 68, 0.7)", 
                    type = "bar" 
                });
            }

            if (_showMetricWeeklyDue)
            {
                datasets.Add(new { 
                    label = "Weekly Statement Due", 
                    data = _breakdownDailyItems.Select(d => d.WeeklyStatementDue).ToList(), 
                    color = "#ec4899", 
                    bg = "rgba(236, 72, 153, 0.7)", 
                    type = "bar" 
                });
            }

            if (!datasets.Any())
            {
                _showMetricEnvelope = true;
                datasets.Add(new { 
                    label = "Envelope Drops", 
                    data = _breakdownDailyItems.Select(d => d.EnvelopeAmount).ToList(), 
                    color = "#10b981", 
                    bg = "rgba(16, 185, 129, 0.7)", 
                    type = "bar" 
                });
            }

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

            var labels = _weeklyCommissionsStats.Select(w => w.WeekEndingDate.ToString("MM/dd")).ToList();
            var datasets = new List<object>();

            if (_showMetricCommissions)
            {
                datasets.Add(new { 
                    label = "Commissions", 
                    data = _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission)).ToList(), 
                    color = "#10b981", 
                    bg = "rgba(16, 185, 129, 0.7)", 
                    type = "bar",
                    stack = "earnings"
                });
            }

            if (_showMetricCashBonus)
            {
                datasets.Add(new { 
                    label = "Cash Bonus", 
                    data = _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus)).ToList(), 
                    color = "#3b82f6", 
                    bg = "rgba(59, 130, 246, 0.7)", 
                    type = "bar",
                    stack = "earnings"
                });
            }

            if (_showMetricClaimsBonus)
            {
                datasets.Add(new { 
                    label = "Claims Bonus", 
                    data = _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)).ToList(), 
                    color = "#8b5cf6", 
                    bg = "rgba(139, 92, 246, 0.7)", 
                    type = "bar",
                    stack = "earnings"
                });
            }

            if (_showMetricFees)
            {
                datasets.Add(new { 
                    label = "Weekly Fees", 
                    data = _weeklyCommissionsStats.Select(w => Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)).ToList(), 
                    color = "#ef4444", 
                    bg = "rgba(239, 68, 68, 0.7)", 
                    type = "bar",
                    stack = "fees"
                });
            }

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
    }
}


