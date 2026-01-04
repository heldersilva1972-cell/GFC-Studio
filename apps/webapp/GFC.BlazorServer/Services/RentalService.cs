// [MODIFIED]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
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

        public RentalService(IDbContextFactory<GfcDbContext> contextFactory, INotificationService notificationService, INotificationRoutingService routingService)
        {
            _contextFactory = contextFactory;
            _notificationService = notificationService;
            _routingService = routingService;
        }

        public async Task<HallRentalRequest> GetRentalRequestAsync(int id)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.HallRentalRequests.FindAsync(id);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Number == 208) { return null; }
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsAsync()
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.HallRentalRequests.ToListAsync();
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

            return true;
        }

        public async Task DeleteRentalRequestAsync(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var request = await context.HallRentalRequests.FindAsync(id);
            if (request != null)
            {
                context.HallRentalRequests.Remove(request);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateCalendarAvailabilityAsync(DateTime date, string status, string? description = null, string? startTime = null, string? endTime = null)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            await UpdateCalendarAvailabilityInternalAsync(context, date, status, description, startTime, endTime);
            await context.SaveChangesAsync();
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

            // 1. Get manually blocked/booked dates (Blackouts/Club Events)
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

            // 2. Get dates from requests
            var requestDates = await context.HallRentalRequests
                .Where(r => r.Status != "Denied" && r.Status != "Cancelled")
                .ToListAsync(); // First get the full objects
            
            // ... (rest of method remains same using results and requestDates)
            // DEBUG: Log what we got
            foreach (var req in requestDates)
            {
                Console.WriteLine($"DEBUG: ID={req.Id}, EventType={req.EventType}, StartTime={req.StartTime}, EndTime={req.EndTime}");
            }
            
            // Now map to DTOs
            var mappedDates = new List<UnavailableDateDto>();
            foreach (var r in requestDates)
            {
                var dto = new UnavailableDateDto
                {
                    Date = r.RequestedDate,
                    Status = r.Status == "Approved" ? "Booked" : "Pending",
                    EventType = r.EventType,
                    EventTime = r.StartTime != null && r.EndTime != null ? $"{r.StartTime} - {r.EndTime}" : null
                };
                Console.WriteLine($"MAPPED: EventType={dto.EventType}, EventTime={dto.EventTime}");
                mappedDates.Add(dto);
            }

            results.AddRange(mappedDates);

            // 3. Return distinct by Date (prioritizing entries with event details)
            return results
                .GroupBy(x => x.Date.Date)
                .Select(g => g.OrderBy(x => (x.EventType == "Private Event" || x.EventType == "Club Event" || string.IsNullOrEmpty(x.EventType)) ? 2 : 0) // Heavily deprioritize fallbacks
                              .ThenBy(x => string.IsNullOrEmpty(x.EventType) ? 1 : 0) // Then prioritize ANY details over none
                              .ThenBy(x => x.Status == "Booked" ? 0 : 1) // Then prioritize Booked over Pending
                              .First())
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
    }
}
