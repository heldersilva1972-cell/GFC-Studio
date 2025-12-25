using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Controllers;
using GFC.BlazorServer.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

public class ScheduleService : IScheduleService
{
    private readonly GfcDbContext _dbContext;
    private readonly ILogger<ScheduleService> _logger;
    private readonly IControllerClient _controllerClient;
    private const int MaxTimeZones = 64; // Controller firmware limit
    private const int MaxHolidays = 64; // Controller firmware limit

    public ScheduleService(GfcDbContext dbContext, ILogger<ScheduleService> logger, IControllerClient controllerClient)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _controllerClient = controllerClient ?? throw new ArgumentNullException(nameof(controllerClient));
    }

    public async Task<List<Holiday>> GetHolidaysAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Holidays
            .OrderBy(h => h.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<Holiday?> GetHolidayAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Holidays
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
    }

    public async Task<int> AddHolidayAsync(Holiday holiday, CancellationToken cancellationToken = default)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(holiday.Name))
        {
            throw new ArgumentException("Holiday name is required.", nameof(holiday));
        }
        
        holiday.Name = holiday.Name.Trim();
        
        // Check for duplicates
        var duplicate = await _dbContext.Holidays
            .FirstOrDefaultAsync(h => h.Name == holiday.Name && 
                                      h.Date == holiday.Date && 
                                      h.IsRecurring == holiday.IsRecurring, 
                                cancellationToken);
        if (duplicate != null)
        {
            throw new InvalidOperationException("A holiday with the same name, date, and recurring setting already exists.");
        }
        
        _dbContext.Holidays.Add(holiday);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return holiday.Id;
    }

    public async Task UpdateHolidayAsync(Holiday holiday, CancellationToken cancellationToken = default)
    {
        _dbContext.Holidays.Update(holiday);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteHolidayAsync(int id, CancellationToken cancellationToken = default)
    {
        var holiday = await _dbContext.Holidays.FindAsync(new object[] { id }, cancellationToken);
        if (holiday != null)
        {
            _dbContext.Holidays.Remove(holiday);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<SpecialEvent>> GetSpecialEventsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SpecialEvents
            .Include(e => e.TimeProfile)
            .OrderBy(e => e.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<SpecialEvent?> GetSpecialEventAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SpecialEvents
            .Include(e => e.TimeProfile)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<int> AddSpecialEventAsync(SpecialEvent specialEvent, CancellationToken cancellationToken = default)
    {
        _dbContext.SpecialEvents.Add(specialEvent);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return specialEvent.Id;
    }

    public async Task UpdateSpecialEventAsync(SpecialEvent specialEvent, CancellationToken cancellationToken = default)
    {
        _dbContext.SpecialEvents.Update(specialEvent);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSpecialEventAsync(int id, CancellationToken cancellationToken = default)
    {
        var specialEvent = await _dbContext.SpecialEvents.FindAsync(new object[] { id }, cancellationToken);
        if (specialEvent != null)
        {
            _dbContext.SpecialEvents.Remove(specialEvent);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<TimeProfile>> GetProfilesAsync(bool activeOnly = true, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.TimeProfiles
            .Include(p => p.Intervals)
            .AsQueryable();
        
        if (activeOnly)
        {
            query = query.Where(p => p.IsActive);
        }
        
        return await query.OrderBy(p => p.Name).ToListAsync(cancellationToken);
    }

    public async Task<TimeProfile?> GetProfileAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TimeProfiles
            .Include(p => p.Intervals)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<TimeProfileInterval>> GetIntervalsAsync(int profileId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TimeProfileIntervals
            .Where(i => i.TimeProfileId == profileId)
            .OrderBy(i => i.DayOfWeek)
            .ThenBy(i => i.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> SaveProfileAsync(TimeProfile profile, CancellationToken cancellationToken = default)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(profile.Name))
        {
            throw new ArgumentException("Profile name is required.", nameof(profile));
        }
        
        profile.Name = profile.Name.Trim();
        
        // Check for name uniqueness (case-insensitive)
        var existingWithName = await _dbContext.TimeProfiles
            .FirstOrDefaultAsync(p => p.Name.ToLower() == profile.Name.ToLower() && p.Id != profile.Id, cancellationToken);
        if (existingWithName != null)
        {
            throw new InvalidOperationException("A time profile with this name already exists.");
        }
        
        // Validate intervals
        foreach (var interval in profile.Intervals)
        {
            if (interval.StartTime >= interval.EndTime)
            {
                throw new ArgumentException($"Interval start time must be before end time for {interval.DayOfWeek}.", nameof(profile));
            }
        }
        
        // Check for overlapping intervals per day
        var intervalsByDay = profile.Intervals.GroupBy(i => i.DayOfWeek);
        foreach (var dayGroup in intervalsByDay)
        {
            var sorted = dayGroup.OrderBy(i => i.StartTime).ToList();
            for (int i = 0; i < sorted.Count - 1; i++)
            {
                if (sorted[i].EndTime > sorted[i + 1].StartTime)
                {
                    throw new ArgumentException($"Overlapping intervals detected for {dayGroup.Key}.", nameof(profile));
                }
            }
        }
        
        if (profile.Id == 0)
        {
            // New profile - set TimeProfileId on intervals after adding
            var intervals = profile.Intervals.ToList();
            profile.Intervals.Clear();
            _dbContext.TimeProfiles.Add(profile);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            // Now add intervals with the correct TimeProfileId
            foreach (var interval in intervals)
            {
                interval.TimeProfileId = profile.Id;
                _dbContext.TimeProfileIntervals.Add(interval);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var existing = await _dbContext.TimeProfiles
                .Include(p => p.Intervals)
                .FirstOrDefaultAsync(p => p.Id == profile.Id, cancellationToken);

            if (existing != null)
            {
                existing.Name = profile.Name;
                existing.Description = profile.Description;
                
                // Remove deleted intervals
                var existingIds = profile.Intervals.Where(i => i.Id > 0).Select(i => i.Id).ToHashSet();
                var toRemove = existing.Intervals.Where(i => !existingIds.Contains(i.Id)).ToList();
                foreach (var interval in toRemove)
                {
                    _dbContext.TimeProfileIntervals.Remove(interval);
                }

                // Update or add intervals
                foreach (var interval in profile.Intervals)
                {
                    if (interval.Id > 0)
                    {
                        var existingInterval = existing.Intervals.FirstOrDefault(i => i.Id == interval.Id);
                        if (existingInterval != null)
                        {
                            existingInterval.DayOfWeek = interval.DayOfWeek;
                            existingInterval.StartTime = interval.StartTime;
                            existingInterval.EndTime = interval.EndTime;
                            existingInterval.Order = interval.Order;
                        }
                    }
                    else
                    {
                        interval.TimeProfileId = existing.Id;
                        existing.Intervals.Add(interval);
                    }
                }
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        return profile.Id;
    }

    public async Task DeleteProfileAsync(int id, CancellationToken cancellationToken = default)
    {
        var profile = await _dbContext.TimeProfiles
            .Include(p => p.Intervals)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (profile != null)
        {
            _dbContext.TimeProfiles.Remove(profile);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<List<ControllerTimeProfileLink>> GetControllerLinksAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ControllerTimeProfileLinks
            .Include(l => l.TimeProfile)
            .Where(l => l.ControllerId == controllerId)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveControllerLinkAsync(int controllerId, int profileId, int controllerProfileIndex, CancellationToken cancellationToken = default)
    {
        // Validation
        const int MaxProfileIndex = 63;
        if (controllerProfileIndex < 0 || controllerProfileIndex > MaxProfileIndex)
        {
            throw new ArgumentException($"Controller profile index must be between 0 and {MaxProfileIndex}.", nameof(controllerProfileIndex));
        }

        // Check if index is already used by another link
        var existingWithIndex = await _dbContext.ControllerTimeProfileLinks
            .FirstOrDefaultAsync(l => l.ControllerId == controllerId && 
                                      l.ControllerProfileIndex == controllerProfileIndex && 
                                      l.TimeProfileId != profileId, 
                                cancellationToken);
        if (existingWithIndex != null)
        {
            throw new InvalidOperationException($"Controller profile index {controllerProfileIndex} is already in use by another profile.");
        }

        // Verify profile exists and is active
        var profile = await _dbContext.TimeProfiles.FindAsync(new object[] { profileId }, cancellationToken);
        if (profile == null)
        {
            throw new InvalidOperationException($"Time profile with ID {profileId} does not exist.");
        }
        if (!profile.IsActive)
        {
            throw new InvalidOperationException($"Time profile '{profile.Name}' is not active and cannot be linked.");
        }

        // Find existing link for this profile
        var existing = await _dbContext.ControllerTimeProfileLinks
            .FirstOrDefaultAsync(l => l.ControllerId == controllerId && l.TimeProfileId == profileId, cancellationToken);

        if (existing != null)
        {
            existing.ControllerProfileIndex = controllerProfileIndex;
            existing.IsEnabled = true;
        }
        else
        {
            _dbContext.ControllerTimeProfileLinks.Add(new ControllerTimeProfileLink
            {
                ControllerId = controllerId,
                TimeProfileId = profileId,
                ControllerProfileIndex = controllerProfileIndex,
                IsEnabled = true
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveLinksAsync(int controllerId, List<ControllerTimeProfileLink> links, CancellationToken cancellationToken = default)
    {
        // Validation
        const int MaxProfileIndex = 63; // 0-based, firmware supports 0-63
        
        var enabledLinks = links.Where(l => l.IsEnabled).ToList();
        var indices = enabledLinks.Select(l => l.ControllerProfileIndex).ToList();
        
        // Check for duplicate indices
        if (indices.Count != indices.Distinct().Count())
        {
            throw new InvalidOperationException("Each controller profile index must be unique.");
        }
        
        // Check index range
        foreach (var link in enabledLinks)
        {
            if (link.ControllerProfileIndex < 0 || link.ControllerProfileIndex > MaxProfileIndex)
            {
                throw new ArgumentException($"Controller profile index must be between 0 and {MaxProfileIndex}.", nameof(links));
            }
        }
        
        // Verify all referenced profiles exist and are active
        var profileIds = links.Select(l => l.TimeProfileId).Distinct().ToList();
        var profiles = await _dbContext.TimeProfiles
            .Where(p => profileIds.Contains(p.Id))
            .ToListAsync(cancellationToken);
        
        foreach (var link in enabledLinks)
        {
            var profile = profiles.FirstOrDefault(p => p.Id == link.TimeProfileId);
            if (profile == null)
            {
                throw new InvalidOperationException($"Time profile with ID {link.TimeProfileId} does not exist.");
            }
            if (!profile.IsActive)
            {
                throw new InvalidOperationException($"Time profile '{profile.Name}' is not active and cannot be linked.");
            }
        }
        
        var existing = await _dbContext.ControllerTimeProfileLinks
            .Where(l => l.ControllerId == controllerId)
            .ToListAsync(cancellationToken);

        _dbContext.ControllerTimeProfileLinks.RemoveRange(existing);

        foreach (var link in links)
        {
            link.ControllerId = controllerId;
            _dbContext.ControllerTimeProfileLinks.Add(link);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TimeScheduleCompiledDto> CompileForControllerAsync(int controllerId, CancellationToken cancellationToken = default)
    {
        var links = await GetControllerLinksAsync(controllerId, cancellationToken);
        var holidays = await GetHolidaysAsync(cancellationToken);
        var specialEvents = await GetSpecialEventsAsync(cancellationToken);
        
        // Only include enabled links with active profiles
        var enabledLinks = links.Where(l => l.IsEnabled).ToList();
        var profileIds = enabledLinks.Select(l => l.TimeProfileId).ToList();
        var profiles = await _dbContext.TimeProfiles
            .Include(p => p.Intervals)
            .Where(p => profileIds.Contains(p.Id) && p.IsActive)
            .ToListAsync(cancellationToken);

        var timeZones = new List<TimeScheduleDto.TimeZoneBlock>();
        var profileIndexMapping = new Dictionary<int, int>();

        foreach (var link in enabledLinks.OrderBy(l => l.ControllerProfileIndex))
        {
            var profile = profiles.FirstOrDefault(p => p.Id == link.TimeProfileId);
            if (profile == null || !profile.IsActive) continue;

            var dailySchedules = new List<TimeScheduleDto.DailySchedule>();
            
            // Convert intervals to daily schedules (ordered by day of week, then order)
            var sortedIntervals = profile.Intervals
                .OrderBy(i => i.DayOfWeek)
                .ThenBy(i => i.Order)
                .ToList();

            foreach (var interval in sortedIntervals)
            {
                dailySchedules.Add(new TimeScheduleDto.DailySchedule(
                    (byte)interval.StartTime.Hour,
                    (byte)interval.StartTime.Minute,
                    (byte)interval.EndTime.Hour,
                    (byte)interval.EndTime.Minute));
            }

            timeZones.Add(new TimeScheduleDto.TimeZoneBlock(
                link.ControllerProfileIndex,
                dailySchedules));

            profileIndexMapping[profile.Id] = link.ControllerProfileIndex;
        }

        // Convert holidays to firmware format
        var holidayBlocks = new List<TimeScheduleDto.HolidayBlock>();
        foreach (var holiday in holidays.Take(MaxHolidays))
        {
            // For recurring holidays, use the current year's date
            var date = holiday.Date;
            if (holiday.IsRecurring)
            {
                // Keep the month/day, but use current year for the date
                date = new DateOnly(DateTime.Now.Year, date.Month, date.Day);
            }
            holidayBlocks.Add(new TimeScheduleDto.HolidayBlock(
                holidayBlocks.Count,
                date,
                date));
        }

        foreach (var specialEvent in specialEvents)
        {
            if (holidayBlocks.Count >= MaxHolidays) break;

            var date = specialEvent.Date;
            holidayBlocks.Add(new TimeScheduleDto.HolidayBlock(
                holidayBlocks.Count,
                date,
                date,
                profileIndexMapping.GetValueOrDefault(specialEvent.TimeProfileId, 0)
                ));
        }

        // Include tasks if enabled
        var tasks = await _dbContext.Set<TaskEntry>()
            .Where(t => t.ControllerId == controllerId && t.Enabled)
            .OrderBy(t => t.Time)
            .ToListAsync(cancellationToken);
        
        var taskBlocks = new List<TimeScheduleDto.TaskBlock>();
        foreach (var task in tasks.Take(64)) // Max 64 tasks
        {
            // Map task to firmware format
            // Type maps to Action, Door is optional, ScheduleId would come from time profile link if needed
            taskBlocks.Add(new TimeScheduleDto.TaskBlock(
                taskBlocks.Count,
                (byte)task.Type,
                (byte)(task.Door ?? 0),
                0)); // ScheduleId - would need to be mapped from time profile if task uses one
        }

        var compiled = new TimeScheduleCompiledDto
        {
            Schedule = new TimeScheduleWriteDto
            {
                TimeZones = timeZones.Take(MaxTimeZones).ToList(),
                Holidays = holidayBlocks,
                Tasks = taskBlocks
            },
            ProfileIndexMapping = profileIndexMapping
        };

        return compiled;
    }

    public async Task<bool> SyncToControllerAsync(int controllerId, uint controllerSerialNumber, CancellationToken cancellationToken = default)
    {
        try
        {
            var compiled = await CompileForControllerAsync(controllerId, cancellationToken);
            await _controllerClient.WriteTimeSchedulesAsync(controllerSerialNumber.ToString(), compiled.Schedule, cancellationToken);
            _logger.LogInformation("Time schedules synced successfully to controller {ControllerId} (SN: {SerialNumber})", controllerId, controllerSerialNumber);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error syncing time schedules to controller {ControllerId}: {Message}", controllerId, ex.Message);
            return false;
        }
    }
}

