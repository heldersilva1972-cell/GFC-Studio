using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IAppSettingsRepository
{
    AppSettings? GetSetting(string key);
    void SetSetting(string key, string value, string? modifiedBy = null);
    bool GetBoolSetting(string key, bool defaultValue = false);
    void SetBoolSetting(string key, bool value, string? modifiedBy = null);
}

