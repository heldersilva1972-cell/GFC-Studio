using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

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

        private List<LotteryShiftDto> _shifts = new();
        private List<LotteryShiftSummaryDto> _dailySummaries = new();
        private List<LotteryShiftSummaryDto> _weeklySummaries = new();
        private List<LotteryShiftSummaryDto> _monthlySummaries = new();
        private List<string> _employeeNames = new();
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
        
        private DateTime _filterStartDate = DateTime.Today.AddDays(-30);
        private DateTime _filterEndDate = DateTime.Today;
        private string _filterEmployee = string.Empty;
        private bool? _showReconciled = null;
        private string _viewMode = "shifts";
        private int _selectedYear = DateTime.Now.Year;

        // Summary stats computed from _shifts list using Business Day logic
        private decimal TotalSales => _shifts.Where(s => s.Status == "Submitted")
            .GroupBy(s => s.ShiftDate.Date)
            .Sum(g => g.OrderByDescending(s => s.ShiftId).First().TotalSales);

        private decimal TotalPayouts => _shifts.Where(s => s.Status == "Submitted")
            .GroupBy(s => s.ShiftDate.Date)
            .Sum(g => g.OrderByDescending(s => s.ShiftId).First().TotalPayouts);

        private decimal TotalNetSales => _shifts.Where(s => s.Status == "Submitted")
            .GroupBy(s => s.ShiftDate.Date)
            .Sum(g => g.OrderByDescending(s => s.ShiftId).First().NetSales);

        private decimal TotalEnvelope => _shifts.Where(s => s.Status == "Submitted").Sum(s => s.EnvelopeAmount);
        private decimal TotalVariance => _shifts.Where(s => s.Status == "Submitted").Sum(s => s.Variance);

        private ShiftFormModel _shiftForm = new();
        private ShiftFormModel _originalForm = new(); // CHANGE TRACKER

        private bool ShowContent => !_loading && string.IsNullOrEmpty(_error);

        private bool IsTableError => !string.IsNullOrEmpty(_error) && 
            (_error.Contains("table does not exist", System.StringComparison.OrdinalIgnoreCase) || 
             _error.Contains("Invalid object name", System.StringComparison.OrdinalIgnoreCase));

        protected override async Task OnInitializedAsync()
        {
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
                
                if (_viewMode == "shifts")
                {
                    await LoadShifts();
                }
                else if (_viewMode == "daily")
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
            var endDate = _filterEndDate.Date.AddDays(1).AddTicks(-1); // End of day
            var shifts = await Task.Run(() => LotteryService.GetShiftsByDateRange(startDate, endDate));
            
            if (!string.IsNullOrEmpty(_filterEmployee))
            {
                shifts = shifts.Where(s => s.EmployeeName == _filterEmployee).ToList();
            }
            
            if (_showReconciled.HasValue)
            {
                shifts = shifts.Where(s => s.IsReconciled == _showReconciled.Value).ToList();
            }
            
            _shifts = shifts
                .OrderByDescending(s => s.ShiftDate.Date)
                .ThenBy(s => s.ShiftType == "Day" ? 0 : s.ShiftType == "Night" ? 1 : 2)
                .ToList();
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

        private async Task ChangeReconciledFilter(bool? value)
        {
            _showReconciled = value;
            await LoadData();
        }

        private async Task ChangeViewMode(string mode)
        {
            _viewMode = mode;
            await LoadData();
        }

        private async Task ChangeYear(int year)
        {
            _selectedYear = year;
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
                Notes = shiftEntity.Notes ?? string.Empty,
                Status = shiftEntity.Status ?? "Submitted",
                CreatedBy = shiftEntity.CreatedBy ?? string.Empty,
                CreatedDate = shiftEntity.CreatedDate
            };
            _originalEmployeeName = shiftEntity.EmployeeName;
            _originalForm = (ShiftFormModel)_shiftForm.Clone(); // SNAPSHOT ORIGINAL STATE
            _modalError = string.Empty;
            _showShiftModal = true;
        }

        private void OnEmployeeChanged(ChangeEventArgs e)
        {
            var username = e.Value?.ToString() ?? string.Empty;
            if (string.IsNullOrEmpty(username) || username == _shiftForm.CreatedBy) return;

            var metadata = _employeeMetadata.FirstOrDefault(m => m.Username == username);
            _pendingUsername = username;
            _pendingFullName = metadata.FullName ?? username;
            _showReassignConfirmation = true;
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
                shift.EnvelopeAmount = _shiftForm.EnvelopeAmount;
                shift.BagRefillAmount = _shiftForm.BagRefillAmount ?? 0;
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
            
            public string Notes { get; set; } = string.Empty;
            public string Status { get; set; } = "Submitted";
            public string CreatedBy { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; } // HANG PREVENTION
            
            public decimal NetSales => (TotalSales ?? 0) - (TotalPayouts ?? 0) - (TotalCancels ?? 0);
            public decimal ExpectedCash => (StartingCash ?? 0) + NetSales + (BagRefillAmount ?? 0);
            public decimal Variance => (EndingCash ?? 0) - ExpectedCash;
            
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
                       CreatedBy != other.CreatedBy;
            }
        }
    }
}
