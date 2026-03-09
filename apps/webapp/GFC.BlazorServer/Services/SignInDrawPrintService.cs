using System;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class SignInDrawPrintService
    {
        public event Func<Task>? OnPrintRequested;

        public async Task RequestPrintAsync()
        {
            if (OnPrintRequested != null)
            {
                await OnPrintRequested.Invoke();
            }
        }
    }
}
