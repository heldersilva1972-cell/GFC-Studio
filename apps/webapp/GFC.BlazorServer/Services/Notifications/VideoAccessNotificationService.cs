using System;
using GFC.Core.Models;

namespace GFC.BlazorServer.Services.Notifications;

public interface IVideoAccessNotificationService
{
    event Action<VpnSession>? OnSessionStarted;
    event Action<int>? OnSessionEnded;
    void NotifySessionStarted(VpnSession session);
    void NotifySessionEnded(int sessionId);
}

public class VideoAccessNotificationService : IVideoAccessNotificationService
{
    public event Action<VpnSession>? OnSessionStarted;
    public event Action<int>? OnSessionEnded;

    public void NotifySessionStarted(VpnSession session)
    {
        OnSessionStarted?.Invoke(session);
    }

    public void NotifySessionEnded(int sessionId)
    {
        OnSessionEnded?.Invoke(sessionId);
    }
}
