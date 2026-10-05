// [MODIFIED]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class RentalService : IRentalService
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;
        private readonly INotificationService _notificationService;
        private readonly INotificationRoutingService _routingService;
        private readonly IWebsiteSettingsService _websiteSettingsService;
        private readonly IRentalEmailDispatcher _emailDispatcher;
        private readonly IGoogleCalendarService _googleCalendarService;

        public RentalService(
            IDbContextFactory<GfcDbContext> contextFactory,
            INotificationService notificationService,
            INotificationRoutingService routingService,
            IWebsiteSettingsService websiteSettingsService,
            IRentalEmailDispatcher emailDispatcher,
            IGoogleCalendarService googleCalendarService)
        {
            _contextFactory = contextFactory;
            _notificationService = notificationService;
            _routingService = routingService;
            _websiteSettingsService = websiteSettingsService;
            _emailDispatcher = emailDispatcher;
            _googleCalendarService = googleCalendarService;
        }

        public static event Action? OnGlobalCalendarUpdated;
        public event Action? OnCalendarUpdated
        {
            add => OnGlobalCalendarUpdated += value;
            remove => OnGlobalCalendarUpdated -= value;
        }

        public void NotifyCalendarUpdated()
        {
            try
            {
                OnGlobalCalendarUpdated?.Invoke();
            }
            catch { }
        }

        public async Task<HallRentalRequest?> GetRentalRequestAsync(int id)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.HallRentalRequests.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) { return null; }
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsAsync()
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.HallRentalRequests.AsNoTracking().ToListAsync();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) { return Enumerable.Empty<HallRentalRequest>(); }
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsByStatusAsync(string status)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.HallRentalRequests.Where(r => r.Status == status).ToListAsync();
             }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) { return Enumerable.Empty<HallRentalRequest>(); }
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.HallRentalRequests
                    .Where(r => r.RequestedDate.Date >= startDate.Date && r.RequestedDate.Date <= endDate.Date)
                    .ToListAsync();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) { return Enumerable.Empty<HallRentalRequest>(); }
        }

        public async Task<bool> IsDateAlreadyBookedAsync(DateTime date)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                
                var rentalExists = await context.HallRentalRequests
                    .AnyAsync(r => r.RequestedDate.Date == date.Date && r.Status == RentalStatus.Approved);
                    
                var calendarExists = await context.AvailabilityCalendars
                    .AnyAsync(c => c.Date.Date == date.Date && (c.Status == "Club Event" || c.Status == "Blackout" || c.Status == "Booked"));
                    
                return rentalExists || calendarExists;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) { return false; }
        }

        public async Task CreateRentalRequestAsync(HallRentalRequest request)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.HallRentalRequests.Add(request);
            await context.SaveChangesAsync();

            var directorEmail = await _routingService.GetEmailForActionAsync("Rental Inquiry");
            if (!string.IsNullOrEmpty(directorEmail))
            {
                // Fire and forget email notification
                // Assuming a method like SendRentalInquiryEmailAsync exists on INotificationService
                // This method would need to be created if it doesn't exist.
                // For the purpose of this task, I will add a placeholder comment.
                // await _notificationService.SendGeneralNotificationAsync(directorEmail, "New Rental Inquiry", $"A new rental inquiry has been submitted by {request.ContactName}.");
            }
        }

        public async Task<bool> UpdateRentalRequestAsync(HallRentalRequest request)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Entry(request).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true; // Indicate success
        }

        public async Task<bool> ApproveRentalRequestAsync(int requestId, string adminNotes, string approvedBy = "Admin")
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var request = await context.HallRentalRequests.FindAsync(requestId);
            if (request == null) return false;

            if (await IsDateAlreadyBookedAsync(request.RequestedDate))
            {
                return false; // Double booking detected
            }

            request.Status = RentalStatus.Approved;
            request.ApprovedBy = approvedBy;
            request.ApprovalDate = DateTime.UtcNow;
            request.StatusChangedBy = approvedBy;
            request.StatusChangedDate = DateTime.UtcNow;
            request.InternalNotes = adminNotes;
            context.Entry(request).State = EntityState.Modified;

            // Update calendar with the SPECIFIC details from the request so it shows correctly on the website
            var displayTime = request.StartTime != null && request.EndTime != null ? $"{request.StartTime} - {request.EndTime}" : null;
            await UpdateCalendarAvailabilityInternalAsync(context, request.RequestedDate, "Booked", request.EventType, request.StartTime, request.EndTime);
            await context.SaveChangesAsync();

            // Fire and forget email notification
            _ = _notificationService.SendRentalConfirmationEmailAsync(request);

            NotifyCalendarUpdated();
            return true;
        }

        public async Task<bool> DenyRentalRequestAsync(int requestId, string adminNotes, string deniedBy = "Admin")
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var request = await context.HallRentalRequests.FindAsync(requestId);
            if (request == null) return false;

            request.Status = RentalStatus.Denied;
            request.DeniedBy = deniedBy;
            request.DenialDate = DateTime.UtcNow;
            request.StatusChangedBy = deniedBy;
            request.StatusChangedDate = DateTime.UtcNow;
            request.InternalNotes = adminNotes;
            context.Entry(request).State = EntityState.Modified;

            // Remove from calendar (make available again)
            await UpdateCalendarAvailabilityInternalAsync(context, request.RequestedDate, "Available");
            await context.SaveChangesAsync();

            // Fire and forget email notification
            _ = _notificationService.SendRentalDenialEmailAsync(request, "Request denied by administrator");

            NotifyCalendarUpdated();
            return true;
        }

        public async Task DeleteRentalRequestAsync(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var request = await context.HallRentalRequests.FindAsync(id);
            if (request != null)
            {
                // Clean up availability calendar entry so the date is available again
                await UpdateCalendarAvailabilityInternalAsync(context, request.RequestedDate, "Available");

                context.HallRentalRequests.Remove(request);
                await context.SaveChangesAsync();
                NotifyCalendarUpdated();
            }
        }

        public async Task UpdateCalendarAvailabilityAsync(DateTime date, string status, string? description = null, string? startTime = null, string? endTime = null)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await UpdateCalendarAvailabilityInternalAsync(context, date, status, description, startTime, endTime);
            await context.SaveChangesAsync();
            NotifyCalendarUpdated();
        }

        private async Task UpdateCalendarAvailabilityInternalAsync(GfcDbContext context, DateTime date, string status, string? description = null, string? startTime = null, string? endTime = null)
        {
            var calendarEntry = await context.AvailabilityCalendars.FirstOrDefaultAsync(c => c.Date.Date == date.Date);

            if (status == "Available")
            {
                // Remove the entry to make the date available
                if (calendarEntry != null)
                {
                    context.AvailabilityCalendars.Remove(calendarEntry);
                }
            }
            else
            {
                // Add or update the entry for Booked/Blackout status
                if (calendarEntry != null)
                {
                    calendarEntry.Status = status;
                    if (description != null) calendarEntry.Description = description;
                    if (startTime != null) calendarEntry.StartTime = startTime;
                    if (endTime != null) calendarEntry.EndTime = endTime;
                }
                else
                {
                    context.AvailabilityCalendars.Add(new AvailabilityCalendar 
                    { 
                        Date = date, 
                        Status = status,
                        Description = description,
                        StartTime = startTime,
                        EndTime = endTime
                    });
                }
            }
        }

        public async Task<IEnumerable<HallRentalRequest>> GetApprovedRentalsAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.HallRentalRequests
                .Where(r => r.Status == RentalStatus.Approved)
                .ToListAsync();
        }

        public async Task CreateRentalInquiryAsync(HallRentalInquiry inquiry)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.HallRentalInquiries.Add(inquiry);
            await context.SaveChangesAsync();
        }

        public async Task<List<DateTime>> GetReservedDatesAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.AvailabilityCalendars
                                 .Where(d => d.Status == "Booked")
                                 .Select(d => d.Date)
                                 .ToListAsync();
        }

        public async Task<List<AvailabilityCalendar>> GetBlackoutEventsAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.AvailabilityCalendars
                .Where(d => d.Status == "Blackout")
                .ToListAsync();
        }

        public async Task<IEnumerable<AvailabilityCalendar>> GetClubEventsAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.AvailabilityCalendars
                .Where(d => d.Status == "Club Event")
                .ToListAsync();
        }

        public async Task UpdateBlackoutDateAsync(int id, DateTime date, string description, string startTime, string endTime)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var existing = await context.AvailabilityCalendars.FindAsync(id);
            if (existing != null)
            {
                existing.Date = date;
                existing.Description = description;
                existing.StartTime = startTime;
                existing.EndTime = endTime;
                context.Entry(existing).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task AddBlackoutDateAsync(DateTime date, string? description = null, string? startTime = null, string? endTime = null)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var existing = await context.AvailabilityCalendars.FirstOrDefaultAsync(d => d.Date.Date == date.Date && d.Status == "Blackout");
            if (existing == null)
            {
                context.AvailabilityCalendars.Add(new AvailabilityCalendar 
                { 
                    Date = date.Date, 
                    Status = "Blackout",
                    Description = description,
                    StartTime = startTime,
                    EndTime = endTime
                });
            }
            else 
            {
                existing.Description = description;
                existing.StartTime = startTime;
                existing.EndTime = endTime;
                context.Entry(existing).State = EntityState.Modified;
            }
            await context.SaveChangesAsync();
        }

        public async Task RemoveBlackoutDateAsync(DateTime date)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var existing = await context.AvailabilityCalendars.FirstOrDefaultAsync(d => d.Date.Date == date.Date && d.Status == "Blackout");
            if (existing != null)
            {
                context.AvailabilityCalendars.Remove(existing);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<UnavailableDateDto>> GetUnavailableDatesAsync()
        {
            var results = new List<UnavailableDateDto>();
            await using var context = await _contextFactory.CreateDbContextAsync();

            // 1. Get manually blocked/booked dates (Blackouts/Club Events from AvailabilityCalendars table)
            var calendarDates = await context.AvailabilityCalendars
                .Where(d => d.Status == "Booked" || d.Status == "Blackout")
                .Select(d => new UnavailableDateDto 
                { 
                    Date = d.Date, 
                    Status = d.Status == "Blackout" ? "Blackout" : "Booked", 
                    EventType = d.Description ?? "Private Event",
                    EventTime = d.StartTime != null && d.EndTime != null ? $"{d.StartTime} - {d.EndTime}" : null
                })
                .ToListAsync();
            
            results.AddRange(calendarDates);

            // 2. Get dates from active rental requests
            var requestDates = await context.HallRentalRequests
                .Where(r => r.Status != "Denied" && r.Status != "Cancelled")
                .ToListAsync();
            
            foreach (var r in requestDates)
            {
                var dto = new UnavailableDateDto
                {
                    Date = r.RequestedDate,
                    Status = r.Status == "Approved" ? "Booked" : "Pending",
                    EventType = r.EventType,
                    EventTime = r.StartTime != null && r.EndTime != null ? $"{r.StartTime} - {r.EndTime}" : null
                };
                results.Add(dto);
            }

            // 3. Get Club Events / External Bookings from Google Calendar Service
            try
            {
                if (_googleCalendarService != null)
                {
                    var calSettings = await _googleCalendarService.GetSettingsAsync();
                    var externalEvents = new List<CalendarEventItem>();

                    if (calSettings != null)
                    {
                        // Fetch configured feeds
                        if (calSettings.Feeds != null && calSettings.Feeds.Any(f => f.IsEnabled && !string.IsNullOrWhiteSpace(f.Url)))
                        {
                            try
                            {
                                var fetched = await _googleCalendarService.FetchAllFeedsAsync(calSettings);
                                if (fetched != null)
                                {
                                    externalEvents.AddRange(fetched);
                                }
                            }
                            catch { }
                        }
                        else if (!string.IsNullOrWhiteSpace(calSettings.PublicCalendarUrl))
                        {
                            try
                            {
                                var fetched = await _googleCalendarService.FetchEventsFromUrlAsync(calSettings.PublicCalendarUrl);
                                if (fetched != null)
                                {
                                    externalEvents.AddRange(fetched);
                                }
                            }
                            catch { }
                        }
                    }

                    // Map Google Calendar / Club events to UnavailableDateDto
                    foreach (var evt in externalEvents)
                    {
                        if (string.Equals(evt.Status, "CANCELLED", StringComparison.OrdinalIgnoreCase))
                            continue;

                        // Check if event is all day or has specific times
                        string? eventTime = null;
                        if (!evt.IsAllDay && evt.Start != default && evt.End != default && (evt.Start.TimeOfDay != TimeSpan.Zero || evt.End.TimeOfDay != TimeSpan.Zero))
                        {
                            eventTime = $"{evt.Start:h:mm tt} - {evt.End:h:mm tt}";
                        }

                        // Avoid duplicate entries if this event was already loaded from HallRentalRequests
                        bool alreadyPresent = results.Any(x => x.Date.Date == evt.Start.Date && 
                            string.Equals(x.EventTime, eventTime, StringComparison.OrdinalIgnoreCase) &&
                            (string.Equals(x.EventType, evt.Title, StringComparison.OrdinalIgnoreCase) || x.Status == "Booked"));

                        if (!alreadyPresent)
                        {
                            results.Add(new UnavailableDateDto
                            {
                                Date = evt.Start.Date,
                                Status = "Booked",
                                EventType = !string.IsNullOrWhiteSpace(evt.Title) ? evt.Title : "Club Event",
                                EventTime = eventTime
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RentalService] Notice: Error fetching external calendar feeds for conflict check: {ex.Message}");
            }

            // 4. Return all distinct unavailable interval entries (preserving multiple slots per day)
            return results
                .GroupBy(x => new { Date = x.Date.Date, EventTime = x.EventTime ?? "FULL_DAY", EventType = x.EventType ?? "" })
                .Select(g => g.First())
                .ToList();
        }

        public async Task<HallRentalInquiry> SaveInquiryAsync(string formData)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var inquiry = new HallRentalInquiry
            {
                ResumeToken = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"), // Longer, more random token
                FormData = formData,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30) // Inquiries are valid for 30 days
            };

            context.HallRentalInquiries.Add(inquiry);
            await context.SaveChangesAsync();

            return inquiry;
        }

        public async Task<HallRentalInquiry> GetInquiryAsync(string resumeToken)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.HallRentalInquiries
                .FirstOrDefaultAsync(i => i.ResumeToken == resumeToken && i.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<string> CleanupDuplicateEventsAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            int deniedCount = 0;
            
            // 1. Get all booked dates (Approved Rentals + Club Events/Blackouts)
            var approvedDates = await context.HallRentalRequests
                .Where(r => r.Status == RentalStatus.Approved)
                .Select(r => r.RequestedDate.Date)
                .ToListAsync();
                
            var calendarDates = await context.AvailabilityCalendars
                .Where(c => c.Status == "Club Event" || c.Status == "Blackout")
                .Select(c => c.Date.Date)
                .ToListAsync();
                
            var busyDates = new HashSet<DateTime>(approvedDates);
            foreach(var d in calendarDates) busyDates.Add(d);
            
            // 2. Find Pending requests on these dates
            var conflictingPending = await context.HallRentalRequests
                .Where(r => r.Status == RentalStatus.Pending)
                .ToListAsync();
                
            foreach (var req in conflictingPending)
            {
                if (busyDates.Contains(req.RequestedDate.Date))
                {
                    req.Status = RentalStatus.Denied;
                    req.InternalNotes = (req.InternalNotes ?? "") + " [System: Auto-Denied during cleanup due to date conflict]";
                    req.DenialDate = DateTime.UtcNow;
                    req.DeniedBy = "System Cleanup";
                    context.Entry(req).State = EntityState.Modified;
                    deniedCount++;
                }
            }
            
            await context.SaveChangesAsync();
            return $"Cleanup Complete: Auto-denied {deniedCount} pending requests due to conflicts.";
        }

        public async Task<IEnumerable<HallRentalPayment>> GetPaymentsForRequestAsync(int requestId)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.HallRentalPayments
                    .Where(p => p.HallRentalRequestId == requestId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208)
            {
                return Enumerable.Empty<HallRentalPayment>();
            }
        }

        public async Task<HallRentalPayment> RecordPaymentAsync(HallRentalPayment payment)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.HallRentalPayments.Add(payment);

            var req = await context.HallRentalRequests.FindAsync(payment.HallRentalRequestId);
            if (req != null)
            {
                // Calculate total paid across non-refund payments
                var existingPayments = await context.HallRentalPayments
                    .Where(p => p.HallRentalRequestId == payment.HallRentalRequestId)
                    .ToListAsync();

                decimal total = existingPayments.Sum(p => p.PaymentType == "Refund" ? -p.Amount : p.Amount) + 
                                (payment.PaymentType == "Refund" ? -payment.Amount : payment.Amount);

                req.AmountPaid = Math.Max(0, total);
                req.IsPaid = req.AmountPaid >= req.TotalPrice && req.TotalPrice > 0;
                req.PaymentDate = payment.PaymentDate;
                req.PaymentMethod = payment.PaymentMethod;

                if (payment.PaymentType == "Security Deposit")
                {
                    req.SecurityDepositPaid = true;
                    req.SecurityDepositAmount = payment.Amount;
                }

                context.Entry(req).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
            return payment;
        }

        public async Task<HallRentalPayment> UpdatePaymentAsync(HallRentalPayment payment)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var existing = await context.HallRentalPayments.FindAsync(payment.Id);
            if (existing != null)
            {
                existing.PaymentType = payment.PaymentType;
                existing.Amount = payment.Amount;
                existing.PaymentDate = payment.PaymentDate;
                existing.PaymentMethod = payment.PaymentMethod;
                existing.ReferenceOrCheckNumber = payment.ReferenceOrCheckNumber;
                existing.Notes = payment.Notes;
                existing.RecordedBy = payment.RecordedBy;
                context.Entry(existing).State = EntityState.Modified;
                await context.SaveChangesAsync();

                // Recalculate request totals
                var req = await context.HallRentalRequests.FindAsync(existing.HallRentalRequestId);
                if (req != null)
                {
                    var allPayments = await context.HallRentalPayments
                        .Where(p => p.HallRentalRequestId == existing.HallRentalRequestId)
                        .ToListAsync();

                    decimal total = allPayments.Sum(p => p.PaymentType == "Refund" ? -p.Amount : p.Amount);
                    req.AmountPaid = Math.Max(0, total);
                    req.IsPaid = req.AmountPaid >= req.TotalPrice && req.TotalPrice > 0;
                    context.Entry(req).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            return payment;
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var payment = await context.HallRentalPayments.FindAsync(paymentId);
            if (payment == null) return false;

            int reqId = payment.HallRentalRequestId;
            context.HallRentalPayments.Remove(payment);
            await context.SaveChangesAsync();

            // Recalculate request balance
            var req = await context.HallRentalRequests.FindAsync(reqId);
            if (req != null)
            {
                var remaining = await context.HallRentalPayments
                    .Where(p => p.HallRentalRequestId == reqId)
                    .ToListAsync();

                decimal total = remaining.Sum(p => p.PaymentType == "Refund" ? -p.Amount : p.Amount);
                req.AmountPaid = Math.Max(0, total);
                req.IsPaid = req.AmountPaid >= req.TotalPrice && req.TotalPrice > 0;
                context.Entry(req).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<int> CleanupTestRecordsAsync()
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var testRequests = await context.HallRentalRequests
                .Where(r => r.IsTestRecord)
                .ToListAsync();

            int count = testRequests.Count;
            if (count > 0)
            {
                var reqIds = testRequests.Select(r => r.Id).ToList();
                var testPayments = await context.HallRentalPayments
                    .Where(p => reqIds.Contains(p.HallRentalRequestId) || p.IsTestPayment)
                    .ToListAsync();

                context.HallRentalPayments.RemoveRange(testPayments);
                context.HallRentalRequests.RemoveRange(testRequests);
                await context.SaveChangesAsync();
            }

            return count;
        }

        private async Task HealDatabaseAsync()
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                var sql = @"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterAddress')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterAddress] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterCity')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterCity] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterState')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterState] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequesterZip')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequesterZip] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'ApplicantSignature')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [ApplicantSignature] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'AlternateEventDate')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [AlternateEventDate] DATETIME2 NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RoomSelected')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [RoomSelected] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'EventDescription')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [EventDescription] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'TermsAgreed')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [TermsAgreed] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'CancellationPolicyAgreed')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [CancellationPolicyAgreed] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'KitchenPolicyAgreed')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [KitchenPolicyAgreed] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'BartenderRequested')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [BartenderRequested] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequiresSetupTime')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequiresSetupTime] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'AmountPaid')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [AmountPaid] DECIMAL(18,2) NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'SecurityDepositAmount')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [SecurityDepositAmount] DECIMAL(18,2) NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'IsTestRecord')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [IsTestRecord] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'AgreedPoliciesJson')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [AgreedPoliciesJson] NVARCHAR(MAX) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'RequestPhoneCall')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [RequestPhoneCall] BIT NOT NULL DEFAULT 0;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'PreferredContactMethod')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [PreferredContactMethod] NVARCHAR(50) NULL;

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'QualificationAnswersJson')
                        ALTER TABLE [dbo].[HallRentalRequests] ADD [QualificationAnswersJson] NVARCHAR(MAX) NULL;

                    -- Ensure GuestCount allows NULL or has a default of 0
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[HallRentalRequests]') AND name = 'GuestCount' AND is_nullable = 0)
                    BEGIN
                        ALTER TABLE [dbo].[HallRentalRequests] ALTER COLUMN [GuestCount] INT NULL;
                    END
                ";
                await context.Database.ExecuteSqlRawAsync(sql);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RentalService] HealDatabaseAsync warning: {ex.Message}");
            }
        }

        public async Task<HallRentalRequest> SubmitPublicRentalRequestAsync(HallRentalRequest request, bool isTest = false)
        {
            await HealDatabaseAsync();
            await using var context = await _contextFactory.CreateDbContextAsync();

            request.IsTestRecord = isTest;
            request.CreatedDate = DateTime.UtcNow;
            request.RequestedDate = request.EventDate;
            request.Status = RentalStatus.Pending;

            context.HallRentalRequests.Add(request);
            await context.SaveChangesAsync();

            // Auto-schedule pending calendar entry in local database and Google Calendar
            try
            {
                string titlePrefix = "PENDING: ";
                string room = string.IsNullOrEmpty(request.RoomSelected) ? "Function Hall" : request.RoomSelected;
                string desc = $"Applicant: {request.ApplicantName}\nPhone: {request.RequesterPhone}\nEmail: {request.RequesterEmail}\nRoom: {room}\nGuests: {request.GuestCount}\nTotal: ${request.TotalPrice}";

                await UpdateCalendarAvailabilityAsync(
                    request.EventDate,
                    $"{titlePrefix}{request.ApplicantName} ({request.EventType ?? "Rental"})",
                    desc,
                    request.StartTime ?? "12:00 PM",
                    request.EndTime ?? "5:00 PM"
                );

                // Push to Google Calendar directly if Service Account / Write API is configured
                var calSettings = await _googleCalendarService.GetSettingsAsync();

                string? targetCalId = calSettings?.PrimaryGoogleCalendarId?.Trim();

                if (string.IsNullOrWhiteSpace(targetCalId) && !string.IsNullOrWhiteSpace(calSettings?.PublicCalendarUrl))
                {
                    var m = System.Text.RegularExpressions.Regex.Match(calSettings.PublicCalendarUrl, @"ical/([^/]+)/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        targetCalId = System.Uri.UnescapeDataString(m.Groups[1].Value);
                    }
                }

                if (!string.IsNullOrWhiteSpace(targetCalId) &&
                    calSettings != null &&
                    !string.IsNullOrWhiteSpace(calSettings.ServiceAccountEmail) &&
                    !string.IsNullOrWhiteSpace(calSettings.ServiceAccountPrivateKey))
                {
                    var activeCalSettings = new GoogleCalendarSettings
                    {
                        PrimaryGoogleCalendarId = targetCalId,
                        ServiceAccountEmail = calSettings.ServiceAccountEmail,
                        ServiceAccountPrivateKey = calSettings.ServiceAccountPrivateKey,
                        ServiceAccountProjectNumber = calSettings.ServiceAccountProjectNumber,
                        EnableWriteApi = true,
                        GoogleEventVisibility = calSettings.GoogleEventVisibility,
                        EventTitleFields = calSettings.EventTitleFields,
                        PushApplicantNameToTitle = calSettings.PushApplicantNameToTitle,
                        PushApplicantNameToDescription = calSettings.PushApplicantNameToDescription,
                        PushApplicantPhoneToDescription = calSettings.PushApplicantPhoneToDescription,
                        PushApplicantEmailToDescription = calSettings.PushApplicantEmailToDescription,
                        PushPricingQuoteToDescription = calSettings.PushPricingQuoteToDescription,
                        PushGuestCountToDescription = calSettings.PushGuestCountToDescription,
                        PushServicesToDescription = calSettings.PushServicesToDescription,
                        PushBarServiceToDescription = calSettings.PushBarServiceToDescription,
                        BarServiceCalendarDisplayName = calSettings.BarServiceCalendarDisplayName,
                        PushKitchenUsageToDescription = calSettings.PushKitchenUsageToDescription,
                        KitchenUsageCalendarDisplayName = calSettings.KitchenUsageCalendarDisplayName,
                        PushAvEquipmentToDescription = calSettings.PushAvEquipmentToDescription,
                        AvEquipmentCalendarDisplayName = calSettings.AvEquipmentCalendarDisplayName,
                        PushAddressToDescription = calSettings.PushAddressToDescription,
                        PushNotesToDescription = calSettings.PushNotesToDescription
                    };

                    DateTime start = request.EventDate.Date.AddHours(14);
                    DateTime end = request.EventDate.Date.AddHours(20);
                    if (DateTime.TryParse(request.StartTime, out var ps)) start = request.EventDate.Date.Add(ps.TimeOfDay);
                    if (DateTime.TryParse(request.EndTime, out var pe)) end = request.EventDate.Date.Add(pe.TimeOfDay);

                    string eventType = string.IsNullOrWhiteSpace(request.EventType) ? "Hall Rental" : request.EventType;
                    string roomName = string.IsNullOrWhiteSpace(request.RoomSelected) ? "Main Function Hall" : request.RoomSelected;

                    var services = new List<string>();
                    if (request.BartenderRequested && calSettings.PushBarServiceToDescription)
                    {
                        var barName = !string.IsNullOrWhiteSpace(calSettings.BarServiceCalendarDisplayName) ? calSettings.BarServiceCalendarDisplayName : "Bar / Bartender Service";
                        services.Add(barName);
                    }
                    if (request.KitchenUsage && calSettings.PushKitchenUsageToDescription)
                    {
                        var kitchenName = !string.IsNullOrWhiteSpace(calSettings.KitchenUsageCalendarDisplayName) ? calSettings.KitchenUsageCalendarDisplayName : "Kitchen Access";
                        services.Add(kitchenName);
                    }
                    if (request.AvEquipmentUsage && calSettings.PushAvEquipmentToDescription)
                    {
                        var avName = !string.IsNullOrWhiteSpace(calSettings.AvEquipmentCalendarDisplayName) ? calSettings.AvEquipmentCalendarDisplayName : "A/V Equipment";
                        services.Add(avName);
                    }
                    if (request.RequiresSetupTime) services.Add("Setup Time Requested");

                    // Ordered Title Fields Construction (Pipe '|' Delimited)
                    var titleFields = calSettings.EventTitleFields ?? GoogleCalendarSettings.GetDefaultGoogleTitleFields();
                    var enabledOrderedFields = titleFields.Where(f => f.IsEnabled).OrderBy(f => f.Order).ToList();

                    var titleSegments = new List<string>();
                    foreach (var field in enabledOrderedFields)
                    {
                        switch (field.FieldKey)
                        {
                            case "applicant_name":
                                if (!string.IsNullOrWhiteSpace(request.ApplicantName))
                                    titleSegments.Add(request.ApplicantName.Trim());
                                break;
                            case "event_type":
                                if (!string.IsNullOrWhiteSpace(eventType))
                                    titleSegments.Add(eventType.Trim());
                                break;
                            case "room_location":
                                if (!string.IsNullOrWhiteSpace(roomName))
                                    titleSegments.Add(roomName.Trim());
                                break;
                            case "time_window":
                                if (!string.IsNullOrWhiteSpace(request.StartTime) && !string.IsNullOrWhiteSpace(request.EndTime))
                                    titleSegments.Add($"{request.StartTime} - {request.EndTime}");
                                break;
                            case "services":
                                if (services.Any())
                                    titleSegments.Add(string.Join(", ", services));
                                break;
                            case "guest_count":
                                if (request.GuestCount > 0)
                                    titleSegments.Add($"{request.GuestCount} Guests");
                                break;
                            case "pricing_quote":
                                if (request.TotalPrice > 0)
                                    titleSegments.Add($"${request.TotalPrice:N0}");
                                break;
                            case "contact_phone":
                                if (!string.IsNullOrWhiteSpace(request.RequesterPhone))
                                    titleSegments.Add(request.RequesterPhone.Trim());
                                break;
                            case "contact_email":
                                if (!string.IsNullOrWhiteSpace(request.RequesterEmail))
                                    titleSegments.Add(request.RequesterEmail.Trim());
                                break;
                        }
                    }

                    string eventTitle;
                    if (titleSegments.Any())
                    {
                        eventTitle = $"{titlePrefix}{string.Join(" | ", titleSegments)}";
                    }
                    else
                    {
                        eventTitle = $"{titlePrefix}{request.ApplicantName} - {eventType}";
                    }

                    var lines = new List<string>();
                    if (calSettings.PushApplicantNameToDescription && !string.IsNullOrWhiteSpace(request.ApplicantName))
                        lines.Add($"Applicant: {request.ApplicantName}");
                    if (calSettings.PushApplicantPhoneToDescription && !string.IsNullOrWhiteSpace(request.RequesterPhone))
                        lines.Add($"Phone: {request.RequesterPhone}");
                    if (calSettings.PushApplicantEmailToDescription && !string.IsNullOrWhiteSpace(request.RequesterEmail))
                        lines.Add($"Email: {request.RequesterEmail}");
                    if (calSettings.PushGuestCountToDescription && request.GuestCount > 0)
                        lines.Add($"Guest Count: {request.GuestCount}");
                    if (calSettings.PushPricingQuoteToDescription)
                        lines.Add($"Total Quote: ${request.TotalPrice:N2}");
                    if (!string.IsNullOrWhiteSpace(request.RoomSelected))
                        lines.Add($"Space: {request.RoomSelected}");
                    if (calSettings.PushServicesToDescription && services.Any())
                        lines.Add($"Services: {string.Join(", ", services)}");
                    if (calSettings.PushNotesToDescription && !string.IsNullOrWhiteSpace(request.InternalNotes))
                        lines.Add($"Applicant Notes: {request.InternalNotes}");

                    var googleEvt = new CalendarEventItem
                    {
                        Title = eventTitle,
                        Start = start,
                        End = end,
                        Location = request.RoomSelected ?? "Function Hall",
                        Description = lines.Any() ? string.Join("\n", lines) : $"{titlePrefix}{eventType}"
                    };

                    await _googleCalendarService.CreateGoogleEventAsync(googleEvt, activeCalSettings);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RentalService] Calendar sync warning: {ex.Message}");
            }

            // Dispatch Outgoing Applicant Auto-Responder & Staff Alerts via Website Domain Mailer
            try
            {
                var settings = await _websiteSettingsService.GetWebsiteSettingsAsync();
                if (settings != null && !settings.MasterEmailKillSwitch)
                {
                    // 1. Send Applicant Confirmation Email (if enabled and applicant provided email)
                    if (settings.SendApplicantConfirmation && !string.IsNullOrWhiteSpace(request.RequesterEmail))
                    {
                        string targetEmail = request.RequesterEmail;

                        decimal effectiveDeposit = request.SecurityDepositAmount > 0 
                            ? request.SecurityDepositAmount 
                            : (settings.SecurityDepositAmount ?? 200m);

                        string subject = settings.ApplicantConfirmationEmailSubject ?? "Your Hall Rental Application Confirmation - {ClubName}";
                        subject = subject
                            .Replace("{ApplicantName}", request.ApplicantName ?? "")
                            .Replace("{EventDate}", request.EventDate.ToString("MMMM dd, yyyy"))
                            .Replace("{RoomSelected}", request.RoomSelected ?? "Function Hall")
                            .Replace("{TotalPrice}", request.TotalPrice.ToString("N2"))
                            .Replace("{DepositAmount}", effectiveDeposit.ToString("N2"))
                            .Replace("{ClubPhone}", string.IsNullOrWhiteSpace(settings.ClubPhone) ? "(978) 283-2889" : settings.ClubPhone)
                            .Replace("{ClubName}", "Gloucester Fraternity Club");

                        if (isTest) subject = "[TEST] " + subject;

                        string depositNote = (effectiveDeposit > 0 && settings.RequireSecurityDeposit)
                            ? $"- Security Deposit: ${effectiveDeposit:N2}"
                            : "";

                        string body = settings.ApplicantConfirmationEmailBody ?? "";
                        if (string.IsNullOrWhiteSpace(body))
                        {
                            body = "Dear {ApplicantName},\n\nThank you for submitting your Hall Rental Application for {ClubName}.\n\nEvent Date: {EventDate}\nRoom: {RoomSelected}\nTotal Quote: ${TotalPrice}\n\nOur rental committee will review your application and reach out shortly.\nGloucester Fraternity Club | {ClubPhone}";
                        }

                        body = body
                            .Replace("{ApplicantName}", request.ApplicantName ?? "")
                            .Replace("{EventDate}", request.EventDate.ToString("MMMM dd, yyyy"))
                            .Replace("{RoomSelected}", request.RoomSelected ?? "Function Hall")
                            .Replace("{TotalPrice}", request.TotalPrice.ToString("N2"))
                            .Replace("{DepositAmount}", effectiveDeposit.ToString("N2"))
                            .Replace("{DepositDetails}", depositNote)
                            .Replace("{ClubPhone}", string.IsNullOrWhiteSpace(settings.ClubPhone) ? "(978) 283-2889" : settings.ClubPhone)
                            .Replace("{ClubName}", "Gloucester Fraternity Club");

                        var sendResult = await _emailDispatcher.SendRentalEmailAsync(settings, targetEmail, subject, body, settings.RentalEmailCc);
                        if (sendResult.Success)
                        {
                            var sbLog = new System.Text.StringBuilder();
                            sbLog.AppendLine($"[{DateTime.Now:yyyy-MM-dd h:mm tt} by System / Online Submission]");
                            sbLog.AppendLine($"📧 Email Sent: Application Submission Confirmation");
                            sbLog.AppendLine($"• Recipient: {targetEmail}");
                            sbLog.AppendLine($"• Subject: {subject}");
                            sbLog.AppendLine();
                            request.InternalNotes = (sbLog.ToString() + (request.InternalNotes ?? "")).Trim();
                        }
                    }

                    // 2. Send Staff Notification Alert (if enabled)
                    if (settings.NotifyOnNewSubmission)
                    {
                        var recipients = settings.GetRecipientsList();
                        if (recipients.Any())
                        {
                            string staffSubject = $"{(isTest ? "[TEST] " : "")}New Hall Rental Request: {request.ApplicantName} ({request.EventDate:MM/dd/yyyy})";
                            string staffBody = $@"
                                <h3>New Hall Rental Application Submitted</h3>
                                <p>A new rental application has been submitted online:</p>
                                <ul>
                                    <li><strong>Applicant / Organization:</strong> {request.ApplicantName}</li>
                                    <li><strong>Contact Phone:</strong> {request.RequesterPhone}</li>
                                    <li><strong>Contact Email:</strong> {request.RequesterEmail}</li>
                                    <li><strong>Event Date:</strong> {request.EventDate:dddd, MMMM dd, yyyy}</li>
                                    <li><strong>Time Window:</strong> {request.StartTime} - {request.EndTime}</li>
                                    <li><strong>Space:</strong> {request.RoomSelected}</li>
                                    <li><strong>Guest Count:</strong> {request.GuestCount}</li>
                                    <li><strong>Total Quote:</strong> ${request.TotalPrice:N2}</li>
                                    <li><strong>Bartender Requested:</strong> {(request.BartenderRequested ? "Yes" : "No")}</li>
                                    <li><strong>Kitchen Access:</strong> {(request.KitchenUsage ? "Yes" : "No")}</li>
                                    <li><strong>Extra Setup Time Requested:</strong> {(request.RequiresSetupTime ? "Yes" : "No")}</li>
                                </ul>
                                <p>View and manage this booking in <a href='/hall-rentals'>GFC Studio Hall Rentals</a>.</p>";

                            foreach (var recipient in recipients)
                            {
                                string targetStaffEmail = recipient;

                                await _emailDispatcher.SendRentalEmailAsync(settings, targetStaffEmail, staffSubject, staffBody);
                            }
                        }
                    }

                    // Save any updated audit notes from the email dispatching
                    if (!string.IsNullOrWhiteSpace(request.InternalNotes) && request.Id > 0)
                    {
                        context.HallRentalRequests.Update(request);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RentalService] Rental email delivery error: {ex.Message}");
            }

            return request;
        }

        public async Task<HallRentalRequest> SubmitGeneralInquiryAsync(HallRentalRequest inquiry, bool isTest = false)
        {
            await HealDatabaseAsync();
            await using var context = await _contextFactory.CreateDbContextAsync();

            var newInquiry = new HallRentalRequest
            {
                Id = 0,
                IsTestRecord = isTest,
                CreatedDate = DateTime.UtcNow,
                RequestedDate = inquiry.EventDate > DateTime.MinValue ? inquiry.EventDate : DateTime.UtcNow,
                EventDate = inquiry.EventDate > DateTime.MinValue ? inquiry.EventDate : DateTime.UtcNow,
                Status = RentalStatus.Inquiry,
                ApplicantName = !string.IsNullOrWhiteSpace(inquiry.ApplicantName) ? inquiry.ApplicantName : (inquiry.RequesterName ?? "General Inquiry"),
                RequesterName = !string.IsNullOrWhiteSpace(inquiry.RequesterName) ? inquiry.RequesterName : inquiry.ApplicantName,
                RequesterEmail = inquiry.RequesterEmail ?? "",
                RequesterPhone = inquiry.RequesterPhone ?? "",
                RequestPhoneCall = inquiry.RequestPhoneCall,
                PreferredContactMethod = inquiry.RequestPhoneCall ? "Phone" : "Email",
                GuestCount = inquiry.GuestCount ?? 0,
                EventType = !string.IsNullOrWhiteSpace(inquiry.EventType) ? inquiry.EventType : "General Inquiry",
                EventDescription = inquiry.EventDescription,
                RoomSelected = inquiry.RoomSelected ?? "Function Hall",
                TotalPrice = 0,
                AmountPaid = 0,
                SecurityDepositAmount = 0
            };

            context.HallRentalRequests.Add(newInquiry);
            await context.SaveChangesAsync();
            inquiry.Id = newInquiry.Id;

            // Dispatch Notifications to Staff & Visitor (if configured)
            try
            {
                var settings = await _websiteSettingsService.GetWebsiteSettingsAsync();
                if (settings != null)
                {
                    // 1. Outgoing Confirmation Email to Visitor
                    if (settings.SendInquiryConfirmationEmail && !string.IsNullOrWhiteSpace(inquiry.RequesterEmail))
                    {
                        string subject = !string.IsNullOrWhiteSpace(settings.InquiryConfirmationEmailSubject)
                            ? settings.InquiryConfirmationEmailSubject
                            : "We received your inquiry - Gloucester Fraternity Club";

                        string rawBody = !string.IsNullOrWhiteSpace(settings.InquiryConfirmationEmailBody)
                            ? settings.InquiryConfirmationEmailBody
                            : "Dear {ApplicantName},\n\nThank you for reaching out to the Gloucester Fraternity Club!\n\nWe have received your question regarding hall rentals and our rental coordinator will review it and reply back to you shortly.\n\nYour Message / Question:\n\"{Question}\"\n\nGloucester Fraternity Club | {ClubPhone}";

                        string renderedBody = rawBody
                            .Replace("{ApplicantName}", inquiry.ApplicantName)
                            .Replace("{EventDate}", inquiry.EventDate > DateTime.MinValue ? inquiry.EventDate.ToString("MMMM dd, yyyy") : "TBD")
                            .Replace("{Question}", inquiry.EventDescription ?? "No message entered.")
                            .Replace("{ClubPhone}", !string.IsNullOrWhiteSpace(settings.ClubPhone) ? settings.ClubPhone : "(978) 283-2889")
                            .Replace("{ClubName}", "Gloucester Fraternity Club");

                        await _emailDispatcher.SendRentalEmailAsync(settings, inquiry.RequesterEmail, subject, renderedBody);
                    }

                    // 2. Incoming Notification Alert to Staff Recipients
                    if (settings.NotifyOnInquirySubmitted)
                    {
                        var recipients = settings.GetRecipientsList();
                        if (recipients.Any())
                        {
                            string phonePreferenceBadge = inquiry.RequestPhoneCall
                                ? "<span style='background-color:#16a34a; color:#ffffff; font-weight:bold; padding:2px 8px; border-radius:12px; font-size:12px;'>📞 Phone Call Requested</span>"
                                : "<span style='background-color:#64748b; color:#ffffff; font-weight:bold; padding:2px 8px; border-radius:12px; font-size:12px;'>✉️ Email Preferred</span>";

                            string staffSubject = $"{(isTest ? "[TEST] " : "")}[GFC Rental Inquiry] {(inquiry.RequestPhoneCall ? "[PHONE CALLBACK REQUESTED] " : "")}Question from {inquiry.ApplicantName}";
                            string staffBody = $@"
                                <div style='font-family: Arial, sans-serif; max-width: 600px;'>
                                    <h3 style='color: #0d1b2a;'>New Pre-Booking Question / Inquiry Received</h3>
                                    {(inquiry.RequestPhoneCall ? "<div style='background:#f0fdf4; border:1px solid #bbf7d0; border-left:4px solid #16a34a; padding:10px 14px; border-radius:6px; margin-bottom:16px; font-weight:bold; color:#15803d;'>📞 The visitor has requested a phone callback.</div>" : "")}
                                    <p>A visitor submitted a general question on the Hall Rental page:</p>
                                    <table style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                                        <tr><td style='padding: 6px; font-weight: bold; width: 35%; border-bottom: 1px solid #e2e8f0;'>Name:</td><td style='padding: 6px; border-bottom: 1px solid #e2e8f0;'>{inquiry.ApplicantName}</td></tr>
                                        <tr><td style='padding: 6px; font-weight: bold; border-bottom: 1px solid #e2e8f0;'>Email:</td><td style='padding: 6px; border-bottom: 1px solid #e2e8f0;'><a href='mailto:{inquiry.RequesterEmail}'>{inquiry.RequesterEmail}</a></td></tr>
                                        <tr><td style='padding: 6px; font-weight: bold; border-bottom: 1px solid #e2e8f0;'>Contact Phone:</td><td style='padding: 6px; border-bottom: 1px solid #e2e8f0;'>{inquiry.RequesterPhone} {phonePreferenceBadge}</td></tr>
                                        <tr><td style='padding: 6px; font-weight: bold; border-bottom: 1px solid #e2e8f0;'>Target Date:</td><td style='padding: 6px; border-bottom: 1px solid #e2e8f0;'>{(inquiry.EventDate > DateTime.MinValue ? inquiry.EventDate.ToString("dddd, MMMM dd, yyyy") : "Flexible / Not Specified")}</td></tr>
                                        <tr><td style='padding: 6px; font-weight: bold; border-bottom: 1px solid #e2e8f0;'>Event Type / Guest Est:</td><td style='padding: 6px; border-bottom: 1px solid #e2e8f0;'>{(string.IsNullOrWhiteSpace(inquiry.EventType) ? "General Question" : inquiry.EventType)} {(inquiry.GuestCount > 0 ? $"({inquiry.GuestCount} guests)" : "")}</td></tr>
                                    </table>
                                    <div style='background-color: #f8fafc; border-left: 4px solid #3b82f6; padding: 12px; margin-bottom: 20px;'>
                                        <strong>Question / Message:</strong><br/>
                                        <p style='margin-top: 6px; white-space: pre-wrap;'>{inquiry.EventDescription}</p>
                                    </div>
                                    <p><a href='/hall-rentals' style='display: inline-block; background-color: #0d1b2a; color: white; padding: 8px 16px; text-decoration: none; border-radius: 6px;'>View in GFC Studio Hall Rentals</a></p>
                                </div>";

                            foreach (var recipient in recipients)
                            {
                                await _emailDispatcher.SendRentalEmailAsync(settings, recipient, staffSubject, staffBody);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RentalService] Inquiry email delivery notice: {ex.Message}");
            }

            return inquiry;
        }

        public async Task<(bool HasConflict, string? ConflictReason)> ValidateTimeSlotConflictAsync(DateTime date, string? startTime, string? endTime, string? roomName = null)
        {
            try
            {
                var settings = await _websiteSettingsService.GetWebsiteSettingsAsync();
                var dayConfigs = settings?.GetDaySchedulesList();
                var dayConfig = dayConfigs?.FirstOrDefault(d => d.DayOfWeek == date.DayOfWeek);

                // 1. Check if day of week is closed in Matrix Tier Engine
                var tierCards = settings?.GetTierCardsList() ?? WebsiteSettings.GetDefaultTierCards();
                var dayOfWeek = date.DayOfWeek;
                bool isAnyTierAvailableOnDay = tierCards.Any(c => c.IsEnabled && (
                    dayOfWeek == DayOfWeek.Monday ? c.MondayAvailable :
                    dayOfWeek == DayOfWeek.Tuesday ? c.TuesdayAvailable :
                    dayOfWeek == DayOfWeek.Wednesday ? c.WednesdayAvailable :
                    dayOfWeek == DayOfWeek.Thursday ? c.ThursdayAvailable :
                    dayOfWeek == DayOfWeek.Friday ? c.FridayAvailable :
                    dayOfWeek == DayOfWeek.Saturday ? c.SaturdayAvailable :
                    dayOfWeek == DayOfWeek.Sunday ? c.SundayAvailable : false
                ));

                if (!isAnyTierAvailableOnDay)
                {
                    return (true, $"The club is closed / unavailable for private rentals on {date:dddd}s.");
                }

                // 3. Fetch unavailable dates from local database
                var unavailable = await GetUnavailableDatesAsync();
                var dayEvents = unavailable.Where(u => u.Date.Date == date.Date).ToList();

                // 4. Also fetch active Google Calendar events if configured
                try
                {
                    if (_googleCalendarService != null)
                    {
                        var gCalSettings = await _googleCalendarService.GetSettingsAsync();
                        var externalEvents = await _googleCalendarService.FetchAllFeedsAsync(gCalSettings);
                        foreach (var ev in externalEvents.Where(e => e.Start.Date == date.Date))
                        {
                            dayEvents.Add(new UnavailableDateDto
                            {
                                Date = ev.Start.Date,
                                Status = "Booked",
                                EventType = !string.IsNullOrWhiteSpace(ev.Title) ? ev.Title : (ev.Description ?? "Google Calendar Event"),
                                EventTime = ev.IsAllDay ? null : $"{ev.Start:hh:mm tt} - {ev.End:hh:mm tt}"
                            });
                        }
                    }
                }
                catch { }

                if (!dayEvents.Any())
                {
                    return (false, null);
                }

                // Full-day blackout / event checks
                var fullDayBlock = dayEvents.FirstOrDefault(e => string.IsNullOrWhiteSpace(e.EventTime));
                if (fullDayBlock != null)
                {
                    return (true, $"The entire day is already booked or blacked out for '{fullDayBlock.EventType ?? "Private Event"}'.");
                }

                // Time interval checks
                var startMin = ParseTimeToMinutes(startTime);
                var endMin = ParseTimeToMinutes(endTime);

                if (startMin >= 0 && endMin > startMin)
                {
                    foreach (var ev in dayEvents)
                    {
                        if (string.IsNullOrWhiteSpace(ev.EventTime))
                        {
                            return (true, $"Date conflict with existing event: {ev.EventType ?? "Private Event"}.");
                        }

                        var parts = ev.EventTime.Split('-', StringSplitOptions.TrimEntries);
                        if (parts.Length == 2)
                        {
                            var evStart = ParseTimeToMinutes(parts[0]);
                            var evEnd = ParseTimeToMinutes(parts[1]);

                            if (evStart >= 0 && evEnd >= 0 && startMin < evEnd && endMin > evStart)
                            {
                                return (true, $"Time slot ({startTime} - {endTime}) overlaps with existing event ({ev.EventTime}: {ev.EventType ?? "Reserved"}).");
                            }
                        }
                        else
                        {
                            return (true, $"Date is reserved for existing event: {ev.EventType ?? "Reserved"}.");
                        }
                    }
                }

                return (false, null);
            }
            catch (Exception ex)
            {
                return (false, null);
            }
        }

        private static int ParseTimeToMinutes(string? timeStr)
        {
            if (string.IsNullOrWhiteSpace(timeStr)) return -1;
            timeStr = timeStr.Trim();

            if (timeStr.Equals("12:00 Midnight", StringComparison.OrdinalIgnoreCase))
            {
                return 24 * 60;
            }

            if (DateTime.TryParse(timeStr, out var dt))
            {
                return dt.Hour * 60 + dt.Minute;
            }
            return -1;
        }
    }
}


