using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Data.Repositories;

/// <summary>
/// Repository implementation for managing user notification preferences
/// </summary>
public class UserNotificationPreferencesRepository : IUserNotificationPreferencesRepository
{
    private readonly GfcDbContext _context;
    private readonly ILogger<UserNotificationPreferencesRepository> _logger;

    public UserNotificationPreferencesRepository(
        GfcDbContext context,
        ILogger<UserNotificationPreferencesRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserNotificationPreferences?> GetByUserIdAsync(int userId)
    {
        try
        {
            return await _context.Set<UserNotificationPreferences>()
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notification preferences for user {UserId}", userId);
            throw;
        }
    }

    public async Task<List<UserNotificationPreferences>> GetAllAsync()
    {
        try
        {
            return await _context.Set<UserNotificationPreferences>()
                .OrderBy(p => p.UserId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all notification preferences");
            throw;
        }
    }

    public async Task<List<UserNotificationPreferences>> GetByUserIdsAsync(List<int> userIds)
    {
        try
        {
            return await _context.Set<UserNotificationPreferences>()
                .Where(p => userIds.Contains(p.UserId))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notification preferences for multiple users");
            throw;
        }
    }

    public async Task<List<UserNotificationPreferences>> GetUsersWithReimbursementNotificationsAsync()
    {
        try
        {
            return await _context.Set<UserNotificationPreferences>()
                .Where(p => p.ReimbursementNotifyEmail || p.ReimbursementNotifySMS)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting users with reimbursement notifications");
            throw;
        }
    }

    public async Task<UserNotificationPreferences> CreateAsync(UserNotificationPreferences preferences)
    {
        try
        {
            preferences.CreatedAt = DateTime.UtcNow;
            preferences.UpdatedAt = DateTime.UtcNow;
            
            _context.Set<UserNotificationPreferences>().Add(preferences);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Created notification preferences for user {UserId}", preferences.UserId);
            return preferences;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating notification preferences for user {UserId}", preferences.UserId);
            throw;
        }
    }

    public async Task<UserNotificationPreferences> UpdateAsync(UserNotificationPreferences preferences)
    {
        try
        {
            preferences.UpdatedAt = DateTime.UtcNow;
            
            _context.Set<UserNotificationPreferences>().Update(preferences);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Updated notification preferences for user {UserId}", preferences.UserId);
            return preferences;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification preferences for user {UserId}", preferences.UserId);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var preferences = await _context.Set<UserNotificationPreferences>()
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (preferences != null)
            {
                _context.Set<UserNotificationPreferences>().Remove(preferences);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Deleted notification preferences {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification preferences {Id}", id);
            throw;
        }
    }

    public async Task DisableAllNotificationsAsync(int userId)
    {
        try
        {
            var preferences = await GetByUserIdAsync(userId);
            
            if (preferences != null)
            {
                // Disable all notification flags
                preferences.ReimbursementNotifyEmail = false;
                preferences.ReimbursementNotifySMS = false;
                preferences.MemberSignupNotifyEmail = false;
                preferences.MemberSignupNotifySMS = false;
                preferences.DuesPaymentNotifyEmail = false;
                preferences.DuesPaymentNotifySMS = false;
                preferences.SystemAlertNotifyEmail = false;
                preferences.SystemAlertNotifySMS = false;
                preferences.LotterySalesNotifyEmail = false;
                preferences.LotterySalesNotifySMS = false;
                preferences.ControllerEventNotifyEmail = false;
                preferences.ControllerEventNotifySMS = false;
                
                await UpdateAsync(preferences);
                
                _logger.LogInformation("Disabled all notifications for user {UserId}", userId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling all notifications for user {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> HasAnyNotificationsEnabledAsync(int userId)
    {
        try
        {
            var preferences = await GetByUserIdAsync(userId);
            
            if (preferences == null)
                return false;
            
            return preferences.ReimbursementNotifyEmail || preferences.ReimbursementNotifySMS ||
                   preferences.MemberSignupNotifyEmail || preferences.MemberSignupNotifySMS ||
                   preferences.DuesPaymentNotifyEmail || preferences.DuesPaymentNotifySMS ||
                   preferences.SystemAlertNotifyEmail || preferences.SystemAlertNotifySMS ||
                   preferences.LotterySalesNotifyEmail || preferences.LotterySalesNotifySMS ||
                   preferences.ControllerEventNotifyEmail || preferences.ControllerEventNotifySMS;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user {UserId} has notifications enabled", userId);
            throw;
        }
    }
}
