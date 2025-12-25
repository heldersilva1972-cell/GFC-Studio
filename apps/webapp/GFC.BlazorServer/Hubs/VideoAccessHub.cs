// [NEW]
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.BlazorServer.Hubs
{
    public class VideoAccessHub : Hub
    {
        public async Task SendSessionStarted(VpnSession session)
        {
            await Clients.All.SendAsync("SessionStarted", session);
        }

        public async Task SendSessionEnded(int sessionId)
        {
            await Clients.All.SendAsync("SessionEnded", sessionId);
        }
    }
}
