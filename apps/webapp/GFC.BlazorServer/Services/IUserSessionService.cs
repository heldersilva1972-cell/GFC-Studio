// [NEW]
using System;

namespace GFC.BlazorServer.Services
{
    public interface IUserSessionService
    {
        DateTime LoginTimeUtc { get; }
        void SetLoginTime(DateTime loginTime);
    }
}
