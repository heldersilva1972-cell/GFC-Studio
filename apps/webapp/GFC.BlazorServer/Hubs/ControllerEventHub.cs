using Microsoft.AspNetCore.SignalR;

namespace GFC.BlazorServer.Hubs;

public class ControllerEventHub : Hub
{
    public async Task SendEventUpdate(int controllerId)
    {
        await Clients.All.SendAsync("ReceiveEventUpdate", controllerId);
    }
}
