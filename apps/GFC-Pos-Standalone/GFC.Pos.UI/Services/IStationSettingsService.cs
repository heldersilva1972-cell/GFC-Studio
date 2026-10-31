namespace GFC.Pos.UI.Services;

public interface IStationSettingsService
{
    Task<string> GetTerminalNameAsync();
    Task SaveTerminalNameAsync(string name);
    Task<bool> GetCustomerDisplayEnabledAsync();
    Task SaveCustomerDisplayEnabledAsync(bool enabled);
    Task<int> GetCustomerDisplayDurationSecondsAsync();
    Task SaveCustomerDisplayDurationSecondsAsync(int seconds);
}
