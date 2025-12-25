// [NEW]
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Hubs
{
    public class StudioPreviewHub : Hub
    {
        public async Task SendPreviewUpdate(string pageContent)
        {
            await Clients.All.SendAsync("ReceivePreviewUpdate", pageContent);
        }
    }
}
