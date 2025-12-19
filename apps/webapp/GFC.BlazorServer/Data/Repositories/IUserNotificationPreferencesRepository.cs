using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Data.Repositories;

/// <summary>
/// Repository interface for managing user notification preferences
/// </summary>
public interface IUserNotificationPreferencesRepository
{
    /// <summary>
    /// Get notification preferences for a specific user
    /// </summary>
    Task<UserNotificationPreferences?> GetByUserIdAsync(int userId);
    
    /// <summary>
    /// Get all notification preferences
    /// </summary>
    Task<List<UserNotificationPreferences>> GetAllAsync();
    
    /// <summary>
    /// Get notification preferences for multiple users
    /// </summary>
    Task<List<UserNotificationPreferences>> GetByUserIdsAsync(List<int> userIds);
    
    /// <summary>
    /// Get all users who have reimbursement notifications enabled
    /// </summary>
    Task<List<UserNotificationPreferences>> GetUsersWithReimbursementNotificationsAsync();
    
    /// <summary>
    /// Create new notification preferences for a user
    /// </summary>
    Task<UserNotificationPreferences> CreateAsync(UserNotificationPreferences preferences);
    
    /// <summary>
    /// Update existing notification preferences
    /// </summary>
    Task<UserNotificationPreferences> UpdateAsync(UserNotificationPreferences preferences);
    
    /// <summary>
    /// Delete notification preferences
    /// </summary>
    Task DeleteAsync(int id);
    
    /// <summary>
    /// Disable all notifications for a user (when they leave a role like Director)
    /// </summary>
    Task DisableAllNotificationsAsync(int userId);
    
    /// <summary>
    /// Check if user has any notifications enabled
    /// </summary>
    Task<bool> HasAnyNotificationsEnabledAsync(int userId);
}
