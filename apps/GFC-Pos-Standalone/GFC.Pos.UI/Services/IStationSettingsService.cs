namespace GFC.Pos.UI.Services;

public interface IStationSettingsService
{
    Task<string> GetTerminalNameAsync();
    Task SaveTerminalNameAsync(string name);
    Task<bool> GetCustomerDisplayEnabledAsync();
    Task SaveCustomerDisplayEnabledAsync(bool enabled);
    Task<int> GetCustomerDisplayDurationSecondsAsync();
    Task SaveCustomerDisplayDurationSecondsAsync(int seconds);

    // Customer Screen Announcements
    Task<bool> GetCustomerAnnouncementsEnabledAsync();
    Task SaveCustomerAnnouncementsEnabledAsync(bool enabled);
    Task<int> GetCustomerAnnouncementIntervalSecondsAsync();
    Task SaveCustomerAnnouncementIntervalSecondsAsync(int seconds);

    // Cart Comp Gestures
    Task<bool> GetCartCompGestureEnabledAsync();
    Task SaveCartCompGestureEnabledAsync(bool enabled);
}
