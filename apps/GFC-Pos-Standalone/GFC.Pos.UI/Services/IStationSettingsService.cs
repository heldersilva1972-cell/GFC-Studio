namespace GFC.Pos.UI.Services;

public interface IStationSettingsService
{
    Task<string> GetTerminalNameAsync();
    Task SaveTerminalNameAsync(string name);
}
