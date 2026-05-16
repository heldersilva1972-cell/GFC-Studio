using System;

namespace GFC.BlazorServer.Services.Notifications;

public interface IControllerNotificationService
{
    event Action<int>? OnEventUpdate;
    void NotifyEventUpdate(int controllerId);
}

public class ControllerNotificationService : IControllerNotificationService
{
    public event Action<int>? OnEventUpdate;

    public void NotifyEventUpdate(int controllerId)
    {
        OnEventUpdate?.Invoke(controllerId);
    }
}
