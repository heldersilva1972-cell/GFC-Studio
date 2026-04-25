namespace GFC.Pos.UI.Services;

public interface IPrinterService
{
    Task<bool> PrintReceiptAsync(string content);
    Task<bool> KickDrawerAsync();
}
