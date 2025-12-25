// [NEW]
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using GFC.Core.Models;
using System.Collections.Generic;

namespace GFC.BlazorServer.Hubs
{
    public class AnimationHub : Hub
    {
        public async Task SendAnimationUpdate(IEnumerable<AnimationKeyframe> keyframes, double currentTime)
        {
            await Clients.Others.SendAsync("ReceiveAnimationUpdate", keyframes, currentTime);
        }
    }
}
