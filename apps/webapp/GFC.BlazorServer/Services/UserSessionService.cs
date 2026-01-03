// [NEW]
using System;

namespace GFC.BlazorServer.Services
{
    public class UserSessionService : IUserSessionService
    {
        public DateTime LoginTimeUtc { get; private set; }

        public void SetLoginTime(DateTime loginTime)
        {
            LoginTimeUtc = loginTime;
        }
    }
}
