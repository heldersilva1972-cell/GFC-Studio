// [NEW]
using System;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IImportService
    {
        Task ImportPageFromUrlAsync(string url, Action<string> onLog);
    }
}
