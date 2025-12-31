// [MODIFIED]
using System;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IToastService
    {
        event Func<string, ToastLevel, Task> OnShow;
        Task ShowToastAsync(string message, ToastLevel level);
        Task ShowSuccess(string message);
        Task ShowError(string message);
        Task ShowWarning(string message);
        Task ShowInfo(string message);
    }

    public class ToastService : IToastService
    {
        public event Func<string, ToastLevel, Task> OnShow;

        public async Task ShowToastAsync(string message, ToastLevel level)
        {
            if (OnShow != null)
            {
                await OnShow.Invoke(message, level);
            }
        }

        // Convenience methods
        public async Task ShowSuccess(string message)
        {
            await ShowToastAsync(message, ToastLevel.Success);
        }

        public async Task ShowError(string message)
        {
            await ShowToastAsync(message, ToastLevel.Error);
        }

        public async Task ShowWarning(string message)
        {
            await ShowToastAsync(message, ToastLevel.Warning);
        }

        public async Task ShowInfo(string message)
        {
            await ShowToastAsync(message, ToastLevel.Info);
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
