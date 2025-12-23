// [NEW]
using System;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class ToastService
    {
        public event Func<string, ToastLevel, Task> OnShow;

        public async Task ShowToastAsync(string message, ToastLevel level)
        {
            if (OnShow != null)
            {
                await OnShow.Invoke(message, level);
            }
        }
    }

    public enum ToastLevel
    {
        Info,
        Success,
        Warning,
        Error
    }
}
